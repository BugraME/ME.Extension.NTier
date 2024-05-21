using EnvDTE;
using ME.Extension.NTier.Commands;
using System.Text;
using static ME.Extension.NTier.Constants;

namespace ME.Extension.NTier;
[Command(PackageIds.CreateService)]
internal sealed class CreateService : BaseCreate<CreateService> {
	public CreateService() : base(CreateTypes.Service) { }
	protected override string GetContent(string projectName, string folderName) {
		StringBuilder content = new();
		//using
		content.AppendLine($"using {projectName}.ServiceHelper.Abstract;");
		//namespace
		content.Append($"namespace {projectName}.ServiceHelper");
		if (!string.IsNullOrWhiteSpace(folderName)) content.Append('.').Append(folderName);
		content.AppendLine(";");
		//class
		content.AppendLine($"public class {SolutionItem.Name}{Key} : Base{Key}<{SolutionItem.Name}Repository, {SolutionItem.Name}Dto, {SolutionItem.Name}> {{ }}");
		return content.ToString();
	}
}