using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using SolutionItem = ME.Extension.NTier.Models.SolutionItem;
namespace ME.Extension.NTier;
internal static class Methods {
	internal static async Task<SolutionItem> GetCurrentItemAsync(this AsyncPackage package, bool removeExtension = false) {
		SolutionItem solutionItem = new();
		await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

		IVsMonitorSelection monitorSelection = await package.GetServiceAsync(typeof(SVsShellMonitorSelection)) as IVsMonitorSelection;
		monitorSelection.GetCurrentSelection(out IntPtr hierarchyPointer, out uint itemID, out _, out _);
		Marshal.Release(hierarchyPointer);
		if (Marshal.GetObjectForIUnknown(hierarchyPointer) is IVsHierarchy selectedHierarchy) {
			selectedHierarchy.GetCanonicalName(itemID, out string filePath);
			solutionItem.Path = filePath;

			selectedHierarchy.GetProperty(itemID, (int)__VSHPROPID.VSHPROPID_ExtObject, out object selectedItemObject);
			if (selectedItemObject is ProjectItem selectedProjectItem) {
				solutionItem.Name = removeExtension ? Path.GetFileNameWithoutExtension(selectedProjectItem.Name) : selectedProjectItem.Name;
			}
		}
		else throw new("Cannot access selected solution item.");

		DTE dte = await package.GetServiceAsync(typeof(DTE)) as DTE;
		EnvDTE.Solution solution = dte.Solution;
		solutionItem.SolutionPath = Path.GetDirectoryName(solution.FullName);
		solutionItem.SolutionName = Path.GetFileName(solution.FullName);
		return solutionItem;
	}
	internal static void CreateFile(string path, string content) {
		Directory.CreateDirectory(Path.GetDirectoryName(path));
		using StreamWriter sw = new(path);
		sw.WriteLine(content);
	}

	//Winform
	internal static Form CreateWinForm(Action<Form> formAction) {
		Form form = new() {
			Size = new Size(435, 130),
			BackColor = Color.FromArgb(64, 64, 64),
			ForeColor = Color.White,
			StartPosition = FormStartPosition.CenterScreen,
			FormBorderStyle = FormBorderStyle.None
		};
		formAction(form);
		return form;
	}
	internal static Form AddElements(this Form form, Constants.CreateTypes type, string itemName) {
		Point lastPoint = new();

		Panel panel = new() {
			Size = new Size(form.Size.Width - 2, form.Size.Height - 2),
			Location = new Point(1, 1),
			BackColor = Color.FromArgb(31, 31, 31),
		};
		panel.MouseDown += (_, e) => {
			if (e.Button == MouseButtons.Left) { lastPoint = new Point(e.X, e.Y); }
		};
		panel.MouseMove += (_, e) => {
			if (e.Button == MouseButtons.Left) {
				form.Left += e.X - lastPoint.X;
				form.Top += e.Y - lastPoint.Y;
			}
		};

		form.Controls.Add(panel);

		Label titleLabel = new() {
			Text = form.Text,
			Location = new Point(5, 8),
			AutoSize = true,
			ForeColor = form.ForeColor,
			BackColor = Color.FromArgb(31, 31, 31),
		};
		form.Controls.Add(titleLabel);

		Label closeLabel = new() {
			Text = "✕",
			Location = new Point(405, 8),
			AutoSize = true,
			ForeColor = form.ForeColor,
			BackColor = Color.FromArgb(31, 31, 31),
		};
		closeLabel.Click += (_, _) => form.Close();
		form.Controls.Add(closeLabel);

		Label infrastLabel = new() {
			Text = "Infrastructure/",
			Location = new Point(20, 40),
			AutoSize = true,
			ForeColor = form.ForeColor,
			BackColor = Color.FromArgb(31, 31, 31),
		};
		form.Controls.Add(infrastLabel);

		int leftSize = infrastLabel.Left + infrastLabel.Size.Width + 5;

		TextBox projectTextBox = new() {
			Name = "ProjectName",
			Location = new Point(leftSize, 40),
			Size = new Size(100, 60),
			BackColor = Color.FromArgb(64, 64, 64),
			ForeColor = form.ForeColor,
			BorderStyle = BorderStyle.None
		};
		form.Controls.Add(projectTextBox);

		leftSize += projectTextBox.Width + 5;

		Label dtoLabel = new() {
			Text = $".{type}/",
			Location = new Point(leftSize, 40),
			AutoSize = true,
			ForeColor = form.ForeColor,
			BackColor = Color.FromArgb(31, 31, 31),
		};
		form.Controls.Add(dtoLabel);

		leftSize += dtoLabel.Width + 5;

		TextBox folderTextBox = new() {
			Name = "FolderName",
			Location = new Point(leftSize, 40),
			Size = new Size(40, 60),
			BackColor = Color.FromArgb(64, 64, 64),
			ForeColor = form.ForeColor,
			BorderStyle = BorderStyle.None
		};
		form.Controls.Add(folderTextBox);

		leftSize += folderTextBox.Width + 5;

		Label itemLabel = new() {
			Text = $"/{itemName}{type}.cs",
			Location = new Point(leftSize, 40),
			AutoSize = true,
			ForeColor = form.ForeColor,
			BackColor = Color.FromArgb(31, 31, 31),
		};
		form.Controls.Add(itemLabel);

		Button cancelButton = new() {
			Text = "Cancel",
			Location = new Point(200, 80),
			Size = new Size(100, 25),
			DialogResult = DialogResult.Cancel,
			BackColor = Color.FromArgb(64, 64, 64),
			ForeColor = form.ForeColor,
			FlatStyle = FlatStyle.Flat,
			FlatAppearance = { BorderSize = 0 }
		};
		cancelButton.Click += (_, _) => form.Close();

		form.Controls.Add(cancelButton);

		Button okButton = new() {
			Text = "Create",
			Location = new Point(315, 80),
			Size = new Size(100, 25),
			DialogResult = DialogResult.OK,
			BackColor = Color.FromArgb(64, 64, 64),
			ForeColor = form.ForeColor,
			FlatStyle = FlatStyle.Flat,
			FlatAppearance = { BorderSize = 0 }
		};
		form.Controls.Add(okButton);

		panel.SendToBack();
		return form;
	}
	internal static bool Render(this Form form, out string project, out string folder) {
		DialogResult diagloResult = form.ShowDialog();
		TextBox projectTextbox = form.Controls.Find("ProjectName", true).FirstOrDefault() as TextBox;
		TextBox folderTextbox = form.Controls.Find("FolderName", true).FirstOrDefault() as TextBox;
		project = projectTextbox?.Text;
		folder = folderTextbox?.Text;
		return diagloResult == DialogResult.OK;
	}
}