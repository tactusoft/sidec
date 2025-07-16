<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Graphics.ascx.cs" Inherits="SIDec.UserControls.Indicador.Graphics" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<link rel="stylesheet" href="./UserControls/Indicador/Indicador.css" />

<div class="section_graph">
    <asp:HiddenField ID="hddId" runat="server" Value="0" />
    <asp:HiddenField ID="hddDetail" runat="server" Value="0" />
    <asp:HiddenField ID="hddFont" runat="server" Value="8" />
    <asp:HiddenField ID="hddMonth" runat="server" Value="0" />
    <asp:HiddenField ID="hhdNext" runat="server" Value="0" />
    <div class="row title_row">
        <div class="title_graph">
            <asp:Label ID="lbl_ind" runat="server" />
        </div>
        <div class="btns_graph btn-group'">
            <asp:UpdatePanel runat="server" ID="upGraph" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton ID="btnDetail" runat="server" Text="Ver más" CausesValidation="false" OnClick="btnDetail_Click" ToolTip="Ver más" CssClass="btn">
			            <i class="fas fa-eye"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnMaximize" runat="server" Text="Aumentar" CausesValidation="false" OnClick="btnMaximize_Click" ToolTip="Aumentar" CssClass="btn">
			            <i class="fas fa-expand-arrows-alt"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnMinimize" runat="server" Text="Reducir" CausesValidation="false" OnClick="btnMinimize_Click" ToolTip="Reducir" CssClass="btn" Visible="false">
			            <i class="fas fa-compress-arrows-alt"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnExcel" runat="server" Text="Exportar Excel" CausesValidation="false" OnClick="btnExcel_Click" ToolTip="Exportar Excel" CssClass="btn">
				        <i class="far fa-file-excel"></i>
                    </asp:LinkButton>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExcel" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <canvas id="cnv_ind" runat="server" style="width: 100%"></canvas>
</div>
