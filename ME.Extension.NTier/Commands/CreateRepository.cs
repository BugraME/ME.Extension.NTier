using EnvDTE;
using ME.Extension.NTier.Commands;
using System.Text;
using static ME.Extension.NTier.Constants;
namespace ME.Extension.NTier;
[Command(PackageIds.CreateRepository)]
internal sealed class CreateRepository : BaseCreate<CreateRepository> {
	public CreateRepository() : base(CreateTypes.Repository) { }
	protected override string GetContent(string projectName, string folderName) {
		StringBuilder content = new();
		//using
		content.AppendLine($"using {projectName}.{Key}.Abstract;");
		//namespace
		content.Append($"namespace {projectName}.{Key}");
		if (!string.IsNullOrWhiteSpace(folderName)) content.Append('.').Append(folderName);
		content.AppendLine(";");
		//class
		content.AppendLine($"public class {SolutionItem.Name}{Key} : Base{Key}<{SolutionItem.Name}> {{");
		//content
		content.Append("\t").AppendLine($"public {SolutionItem.Name}{Key}({projectName}Context dbContext) : base(dbContext) {{ }}");
		content.Append("\t").AppendLine($"public {SolutionItem.Name}{Key}() {{ }}");
		content.AppendLine("}");
		return content.ToString();
	}
}