namespace ME.Extension.NTier.Models;
public class SolutionItem {
	public string Name { get; set; }
	public string Path { get; set; }
	public string SolutionName { get; set; }
	public string SolutionPath { get; set; }
	public string SolutionFilePath { get { return System.IO.Path.Combine(SolutionPath, SolutionName); } }
}