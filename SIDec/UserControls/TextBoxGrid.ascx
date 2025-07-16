<%@ Control Language="C#" AutoEventWireup="True" Inherits="SIDec.UserControls.TextBoxGrid" Codebehind="TextBoxGrid.ascx.cs" %>
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
            <asp:Panel ID="pnlBody" runat="server" CssClass ="modal-body row modal-responsive">
                <div class="form-group-sm col-11">
                    <asp:HiddenField ID="hddClientID" runat="server" Value="TextBoxGridID" />
                    <asp:Label class="lblBasic" ID="lblColumnName" runat="server">Valor</asp:Label>
                    <asp:RequiredFieldValidator ID="rfv_value" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                        ValidationGroup="vgTextoBoxGroup" ControlToValidate="txt_value">
                    <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:TextBox ID="txt_value" runat="server" CssClass="form-control form-control-xs"></asp:TextBox>
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
        <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
        <div class="modal-footer row modal-responsive">
            <div class="gv-w">
                <asp:GridView ID="gvTextBoxGrid" CssClass="gv" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
                    DataKeyNames="value" AllowSorting="true" OnRowCommand="gvTextBoxGrid_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="value" HeaderText="Valor"/>
                        <asp:TemplateField ShowHeader="true" HeaderText="X" ItemStyle-CssClass="t-c w40" ItemStyle-Width="50">
                            <HeaderTemplate>
                                <i class="fas fa-trash-alt" style="color:white"></i>
                            </HeaderTemplate>
                            <ItemTemplate >
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn" style="color: red; padding: 0rem 0.75rem;" Text="Eliminar" CausesValidation="false" CommandName="_Delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Eliminar registro"> 
										                    <i class="fas fa-trash-alt"></i>
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle CssClass="gvItemSelected" />
                    <HeaderStyle CssClass="gvHeaderSm" />
                    <RowStyle CssClass="gvItemSm" />
                    <PagerStyle CssClass="gvPager" />
                </asp:GridView>
            </div>
        </div>
                </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
    </div>
</asp:Panel>