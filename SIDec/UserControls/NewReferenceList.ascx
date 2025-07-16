<%@ Control Language="C#" AutoEventWireup="True" Inherits="SIDec.UserControls.NewReferenceList" Codebehind="NewReferenceList.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>

<Ajax:ModalPopupExtender ID="MPE" runat="server" PopupControlID="pnlContenedor" TargetControlID="LinkButton1"
    OkControlID="btnCerrar" CancelControlID="btnCerrar" BackgroundCssClass="modalBackground">
    <Animations>
        <OnShown>
            <FadeIn Duration=".05" Fps="20" />                
        </OnShown>
    </Animations>
</Ajax:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlContenedor" Visible="false">

    <div id="dvMessageBox">
        <div class="modal-header modal-bg-info row modal-responsive">
            <h6 class="modal-title d-flex">
                <asp:Label ID="lblTitle" runat="server" Text="Confirmar acción"></asp:Label>
                <asp:Button runat="server" ID="btnCerrar" Text=" X " OnClick="btnCerrar_Click" CssClass="ml-auto" />
            </h6>
        </div>
        <div>
            <asp:Panel ID="pnlBody" runat="server" CssClass="modal-body row modal-responsive">
                <asp:HiddenField ID="hddClientID" runat="server" Value="NewReferenceListID" />
                <div class="form-group-sm col-11">
                    <asp:Label class="lblBasic" ID="lblCode" runat="server">Código</asp:Label>
                    <asp:RequiredFieldValidator ID="rfv_code" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                        ValidationGroup="vgTextoBoxGroup" ControlToValidate="txt_code">
                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:TextBox ID="txt_code" runat="server" CssClass="form-control form-control-xs"></asp:TextBox>
                    <asp:DropDownList runat="server" ID="ddl_code" CssClass="form-control form-control-xs" AppendDataBoundItems="true" Visible="false">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-11">
                    <asp:Label class="lblBasic" ID="lblName" runat="server">Nombre</asp:Label>
                    <asp:RequiredFieldValidator ID="rfv_name" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                        ValidationGroup="vgTextoBoxGroup" ControlToValidate="txt_name">
                    <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:TextBox ID="txt_name" runat="server" CssClass="form-control form-control-xs"></asp:TextBox>
                </div>
                <div class="form-group-sm col-1 ml-auto">
                    <asp:LinkButton ID="btnAceptar" runat="server" Text="" CssClass="btn btn-outline-primary" OnClick="btnAceptar_Click"
                        ValidationGroup="vgTextoBoxGroup">
                <i class="fas fa-check"></i>
                    </asp:LinkButton>
                </div>
            </asp:Panel>
            <asp:LinkButton runat="server" ID="LinkButton1"></asp:LinkButton>
        </div>
        <div class="modal-footer row modal-responsive">
        </div>
    </div>
</asp:Panel>