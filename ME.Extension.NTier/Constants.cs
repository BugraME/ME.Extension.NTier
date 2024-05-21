namespace ME.Extension.NTier;
public abstract class Constants {
	public static string CsprojContent
		=> $@"<Project Sdk=""Microsoft.NET.Sdk"">
    			<PropertyGroup>
    				<TargetFramework>net8.0</TargetFramework>
    				<ImplicitUsings>enable</ImplicitUsings>
    				<Nullable>disable</Nullable>
    			</PropertyGroup>
    	    </Project>";
	public enum CreateTypes {
		Dto = 1,
		Repository = 2,
		Service = 3
	}
}