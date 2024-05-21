using EnvDTE;
using ME.Extension.NTier;
using ME.Extension.NTier.Commands;
using static ME.Extension.NTier.Constants;

namespace ME.Extension;
[Command(PackageIds.CreateDto)]
internal sealed class CreateDto : BaseCreate<CreateDto> {
	public CreateDto() : base(CreateTypes.Dto) { }
}