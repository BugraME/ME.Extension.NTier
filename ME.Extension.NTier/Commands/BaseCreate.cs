using EnvDTE;
using Microsoft.Build.Construction;
using System.IO;
using System.Linq;
using System.Text;
using static ME.Extension.NTier.Constants;
using SolutionItem = ME.Extension.NTier.Models.SolutionItem;

namespace ME.Extension.NTier.Commands;
public abstract class BaseCreate<T>(CreateTypes key) : BaseCommand<T> where T : class, new() {
	protected CreateTypes Key { get; set; } = key;
	protected SolutionItem SolutionItem { get; set; }
	protected virtual string GetContent(string projectName, string folderName) {
		StringBuilder content = new();
		//using
		content.AppendLine($"using {projectName}.{Key}.Abstract;");
		//namespace
		content.Append($"namespace {projectName}.{Key}");
		if (!string.IsNullOrWhiteSpace(folderName)) content.Append('.').Append(folderName);
		content.AppendLine(";");
		//class
		content.AppendLine($"public class {SolutionItem.Name}{Key} : Base{Key} {{");
		//content
		foreach (string line in File.ReadAllLines(SolutionItem.Path).Where(x => x.Contains("get;set;") || x.Contains("get; set;"))) content.AppendLine(line);
		content.AppendLine("}");
		return content.ToString();
	}
	protected override async Task ExecuteAsync(OleMenuCmdEventArgs e) {
		SolutionItem = await Package.GetCurrentItemAsync(true);
		await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
		bool formResult = Methods.CreateWinForm(x => { x.Text = $"Create {Key}"; })
								 .AddElements(Key, SolutionItem.Name)
								 .Render(out string projectName, out string folderName);
		if (formResult) {
			string projectPath = CheckProject(projectName, Key);
			string fileName = $"{SolutionItem.Name}{Key}.cs";
			string filePath = string.IsNullOrWhiteSpace(folderName)
							  ? Path.Combine(projectPath, fileName)
							  : Path.Combine(projectPath, folderName, fileName);
			string fileContent = GetContent(projectName, folderName).Trim();
			Methods.CreateFile(filePath, fileContent);
		}
	}
	private string CheckProject(string projectName, CreateTypes type) {
		projectName += $".{type}";
		string infrastructure = Path.Combine(SolutionItem.SolutionPath) + "\\Infrastructure\\";
		SolutionFile solutionFile = SolutionFile.Parse(SolutionItem.SolutionFilePath);
		string projectPath = Path.Combine(infrastructure, projectName);
		if (!solutionFile.ProjectsInOrder.Any(p => p.ProjectName == projectName)) {
			Directory.CreateDirectory(projectPath);
			string csprojPath = Path.Combine(projectPath, projectName + ".csproj");
			File.WriteAllText(csprojPath, CsprojContent);
			AddProjectReferenceToSolution(csprojPath);
		}
		return projectPath;
	}
	private void AddProjectReferenceToSolution(string projectPath) {
		ThreadHelper.ThrowIfNotOnUIThread();
		DTE dte = (DTE)Activator.CreateInstance(Type.GetTypeFromProgID("VisualStudio.DTE"));
		EnvDTE.Solution solution = dte.Solution;
		solution.Open(SolutionItem.SolutionFilePath);
		solution.AddFromFile(projectPath, false);
		solution.SaveAs(SolutionItem.SolutionFilePath);
		dte.Quit();
	}
}