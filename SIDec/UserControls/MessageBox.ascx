<%@ Control Language="C#" AutoEventWireup="True" Inherits="SIDec.UserControls.MessageBox" Codebehind="MessageBox.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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
        <div class="modal-header" id="divHeader" runat="server">
            <h5 class="modal-title d-flex">
                <asp:Label ID="lblTitle" runat="server" Text="Confirmar acción"></asp:Label>
                <asp:Button runat="server" ID="btnCerrar" Text=" X " OnClick="btnCerrar_Click" CssClass="ml-auto" />
            </h5>
        </div>
        <div class="modal-body">
            <asp:Label ID="lblTexto" runat="server" Text="¿Está seguro de continuar con la acción solicitada?"></asp:Label>
            <asp:LinkButton runat="server" ID="LinkButton1"></asp:LinkButton>
        </div>
        <div class="modal-footer">
            <asp:LinkButton ID="btnAceptar" runat="server" Text="" CssClass="btn btn-outline-primary" OnClick="btnAceptar_Click" data-dismiss="modal">
                <i class="fas fa-check"></i>&nbsp&nbsp;<asp:Label ID="lblAceptar" runat="server" Text="Aceptar"></asp:Label>
            </asp:LinkButton>
            <asp:LinkButton runat="server" ID="btnCancelar" class="btn btn-outline-secondary" OnClick="btnCancelar_Click" Text="Cancelar">
            <i class="fas fa-times"></i>&nbsp&nbsp;<asp:Label ID="lblCancelar" runat="server" Text="Cancelar"></asp:Label>
            </asp:LinkButton>
        </div>
    </div>
</asp:Panel>
<script type="text/javascript" language="javascript">
    function HideButton(IdButtonAceptar, IdButtonCancelar) {
        var vElement = document.getElementById(IdButtonAceptar);
        if (vElement != null) {
            vElement.style.visibility = "hidden";
        }
        vElement = document.getElementById(IdButtonCancelar);
        if (vElement != null) {
            vElement.style.visibility = "hidden";
        }
    }

    function StartForcus(IdButton) {
        var vElement = document.getElementById(IdButton);
        vElement.focus();
    }
</script>
