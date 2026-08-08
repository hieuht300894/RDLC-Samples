using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace RDLC.DesignerTool
{
    public partial class FormMain : Form
    {
        private readonly HashSet<XmlNode> _trackingNodes = new HashSet<XmlNode>();

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

            var reportNode = document.SelectSingleNode(XPath.ReportNode);
            var pageNode = reportNode.SelectSingleNode(XPath.PageNode);
            var pageWidthNode = pageNode.SelectSingleNode(XPath.PageWidthNode);
            var pageHeightNode = pageNode.SelectSingleNode(XPath.PageHeightNode);
            var leftMarginNode = pageNode.SelectSingleNode(XPath.LeftMarginNode);
            var rightMarginNode = pageNode.SelectSingleNode(XPath.RightMarginNode);
            var topMarginNode = pageNode.SelectSingleNode(XPath.TopMarginNode);
            var bottomMarginNode = pageNode.SelectSingleNode(XPath.BottomMarginNode);
            var columnSpacingNode = pageNode.SelectSingleNode(XPath.ColumnSpacingNode);

            _trackingNodes.Clear();
            _trackingNodes.Add(pageWidthNode);
            _trackingNodes.Add(pageHeightNode);
            _trackingNodes.Add(leftMarginNode);
            _trackingNodes.Add(rightMarginNode);
            _trackingNodes.Add(topMarginNode);
            _trackingNodes.Add(bottomMarginNode);

            cbbNode.Items.Clear();
            cbbNode.Items.AddRange(_trackingNodes.Select(x => new ComboBoxItem() { Text = string.Format("{0}[{1}]", x.Name, x.Attributes["name"]), Tag = x }).ToArray());
        }
    }

    static class XPath
    {
        public const string ReportNode = "//*[local-name()='Report']";
        public const string PageNode = "//*[local-name()='Page']";
        public const string PageWidthNode = "//*[local-name()='PageWidth']";
        public const string PageHeightNode = "//*[local-name()='PageHeight']";
        public const string LeftMarginNode = "//*[local-name()='LeftMargin']";
        public const string RightMarginNode = "//*[local-name()='RightMargin']";
        public const string TopMarginNode = "//*[local-name()='TopMargin']";
        public const string BottomMarginNode = "//*[local-name()='BottomMargin']";
        public const string ColumnSpacingNode = "//*[local-name()='ColumnSpacing']";
    }

    class ComboBoxItem
    {
        public string Text { get; set; }
        public object Tag { get; set; }
    }
}
