using System;
using System.Windows.Forms;

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
            lblFileName.Tag = "";

            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "RDLC Report Files (*.rdlc)|*.rdlc|All files (*.*)|*.*";
                dialog.Title = "Select an RDLC Report File";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblFileName.Text = dialog.FileName;
                        lblFileName.Tag = dialog.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Do nothing
        }
    }
}
