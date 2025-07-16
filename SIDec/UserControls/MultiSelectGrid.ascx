<%@ Control Language="C#" AutoEventWireup="True" Inherits="SIDec.UserControls.MultiSelectGrid" Codebehind="MultiSelectGrid.ascx.cs" %>
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

    <asp:HiddenField ID="hddClientID" runat="server" Value="MultiSelectGridID" />

    <div id="dvMessageBox">
        <div class="modal-header modal-bg-info row modal-responsive">
            <h6 class="modal-title d-flex">
                <asp:Label ID="lblTitle" runat="server" Text="Confirmar acción"></asp:Label>
                <asp:Button runat="server" ID="btnCerrar" Text=" X " OnClick="btnCerrar_Click" CssClass="ml-auto" />
            </h6>
        </div>
        <div>
            <asp:Panel ID="pnlBody" runat="server" CssClass ="modal-body row modal-responsive">
                <div class="form-group-sm col-5">
                    <asp:Label class="lblBasic" ID="lblParentName" runat="server">Valor</asp:Label>
                    <asp:RequiredFieldValidator ID="rfv_parentvalue" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                        ValidationGroup="vgMultiSelectGridGroup" ControlToValidate="ddl_parent">
                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:UpdatePanel ID="upParent" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddl_parent" runat="server" CssClass="form-control form-control-xs" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddl_parent_SelectedIndexChanged">
                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form-group-sm col-5">
                    <asp:Label class="lblBasic" ID="lblchildName" runat="server">Valor</asp:Label>
                    <asp:RequiredFieldValidator ID="rfv_childvalue" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                        ValidationGroup="vgMultiSelectGridGroup" ControlToValidate="ddl_child">
                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:UpdatePanel ID="upChild" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddl_child" runat="server" CssClass="form-control form-control-xs" AppendDataBoundItems="true" AutoPostBack="true">
                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form-group-sm col-1 ml-auto">
                    <asp:LinkButton ID="btnAceptar" runat="server" Text="" CssClass="btn btn-outline-primary" OnClick="btnAceptar_Click"
                        ValidationGroup="vgMultiSelectGridGroup">
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
                <asp:GridView ID="gvMultiSelectGrid" CssClass="gv" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No se ha seleccionado ninguna opción"
                    DataKeyNames="id" AllowSorting="true" OnRowCommand="gvMultiSelectGrid_RowCommand" ><%--OnRowDataBound="gvMultiSelectGrid_RowDataBound"
                    OnSelectedIndexChanged="gvTracing_SelectedIndexChanged" OnPageIndexChanging="gvTracing_PageIndexChanging"
                    OnSorting="gvTracing_Sorting"  OnRowCreated="gv_RowCreated"--%>
                    <Columns>
                        <asp:BoundField DataField="nombreParent" HeaderText="Valor"/>
                        <asp:BoundField DataField="nombreChild" HeaderText="Valor"/>
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