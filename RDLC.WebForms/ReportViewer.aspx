<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="RDLC.WebForms.ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rdlc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h5>Report Viewer</h5>

    <form id="form1" runat="server">
        <!-- The ScriptManager must be declared before the ReportViewer -->
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <!-- Your ReportViewer Control -->
        <rdlc:ReportViewer ID="rptViewer" runat="server" Width="100%" Height="600px" AsyncRendering="true" ProcessingMode="Local">
            <LocalReport></LocalReport>
        </rdlc:ReportViewer>
    </form>

</body>
</html>
