using RDLC.DesignerTool.Models;
using System;
using System.Windows.Forms;
using System.Xml;

namespace RDLC.DesignerTool
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            lblFileName.Text = "";

            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "RDLC Report Files (*.rdlc)|*.rdlc|All files (*.*)|*.*";
                dialog.Title = "Select an RDLC Report File";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblFileName.Text = dialog.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(lblFileName.Text))
            {
                LoadReportXml(lblFileName.Text);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lblFileName.Text))
            {
                LoadReportXml(lblFileName.Text);
            }
        }

        private void LoadReportXml(string fileName)
        {
            var document = new XmlDocument();
            document.Load(fileName);

            var xmlPageNode = document.SelectSingleNode(XPath.PageNode);
        }
    }

    static class XPath
    {
        public const string PageNode = "//*[local-name()='Page']";
    }
}
