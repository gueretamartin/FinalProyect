<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Presupuestos.aspx.cs" Inherits="Octopus.Reports.Presupuestos1" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html5>
<html>
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="728px" Width="1794px" style="margin-right: 574px" ShowCredentialPrompts="False" ShowToolBar="False" >
            <LocalReport ReportPath="Reports\Presupuestos.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource Name="DataSet1" DataSourceId="ObjectDataSource1"></rsweb:ReportDataSource>
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource runat="server" SelectMethod="GetData" TypeName="Octopus.OctopusDataSetTableAdapters.PRESUPUESTOSTableAdapter" ID="ObjectDataSource1" DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Name="Original_PRE_ID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="FEC_ID" Type="Int32" />
                <asp:Parameter Name="EMP_ID" Type="Int32" />
                <asp:Parameter Name="PRE_MARCA" Type="String" />
                <asp:Parameter Name="PRE_MODELO" Type="String" />
                <asp:Parameter Name="PRE_VERSION" Type="String" />
                <asp:Parameter Name="PRE_ANIO" Type="String" />
                <asp:Parameter Name="PRE_PATENTE" Type="String" />
                <asp:Parameter Name="PRE_ESTADO_CHASIS" Type="String" />
                <asp:Parameter Name="PRE_ESTADO_PINTURA" Type="String" />
                <asp:Parameter Name="PRE_ESTADO_PARABRISAS" Type="String" />
                <asp:Parameter Name="PRE_ESTADO_NEUMATICOS" Type="String" />
                <asp:Parameter Name="PRE_ESTADO_INTERIOR" Type="String" />
                <asp:Parameter Name="PRE_DETALLES" Type="String" />
                <asp:Parameter Name="CLI_ID" Type="Int32" />
                <asp:Parameter Name="ES_ID" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="FEC_ID" Type="Int32" />
                <asp:Parameter Name="EMP_ID" Type="Int32" />
                <asp:Parameter Name="PRE_MARCA" Type="String" />
                <asp:Parameter Name="PRE_MODELO" Type="String" />
                <asp:Parameter Name="PRE_VERSION" Type="String" />
                <asp:Parameter Name="PRE_ANIO" Type="String" />
                <asp:Parameter Name="PRE_PATENTE" Type="String" />
                <asp:Parameter Name="PRE_ESTADO_CHASIS" Type="String" />
                <asp:Parameter Name="PRE_ESTADO_PINTURA" Type="String" />
                <asp:Parameter Name="PRE_ESTADO_PARABRISAS" Type="String" />
                <asp:Parameter Name="PRE_ESTADO_NEUMATICOS" Type="String" />
                <asp:Parameter Name="PRE_ESTADO_INTERIOR" Type="String" />
                <asp:Parameter Name="PRE_DETALLES" Type="String" />
                <asp:Parameter Name="CLI_ID" Type="Int32" />
                <asp:Parameter Name="ES_ID" Type="Int32" />
                <asp:Parameter Name="Original_PRE_ID" Type="Int32" />
            </UpdateParameters>
        </asp:ObjectDataSource>
        <asp:Button Text="Print PDF" runat="server" OnClick="Print" CommandName="PDF" />
    
    </div>
    </form>
</body>
</html>
