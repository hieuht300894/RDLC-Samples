namespace RDLC.DesignerTool.Models
{
    public class ReportNode : BaseNode
    {
        public string ReportID { get; set; }
        public string ReportUnitType { get; set; }
        public PageNode PageNode { get; } = new PageNode();
    }
}
