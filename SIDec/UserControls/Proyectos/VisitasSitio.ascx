<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisitasSitio.ascx.cs" Inherits="SIDec.UserControls.Proyectos.VisitasSitio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/FileUpload.ascx" TagName="FileUpload" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>
<link rel="stylesheet" href="./UserControls/Proyectos/proyecto.css" />

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>



<asp:UpdatePanel ID="upVisitasSitio" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:HiddenField ID="hddVisitaPrimary" runat="server" Value="0" />
        <asp:HiddenField ID="hddId" runat="server" Value="UserControlVisitasSitio" />
        <asp:HiddenField ID="hddIdProyecto" runat="server" Value="0" />

        <asp:UpdatePanel runat="server" ID="upVisitasSitioFoot" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divData" runat="server">
                    <div class="row">
                        <asp:UpdatePanel runat="server" ID="upMsgMain" UpdateMode="Conditional" class="alert-main msgusercontrol">
                            <ContentTemplate>
                                <div runat="server" id="msgVisitasSitio" class="alert d-none" role="alert"></div>
                                <div runat="server" id="msgVisitasSitioMain" class="d-none" role="alert">
                                    <span runat="server" id="msgMainText"></span>
                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <asp:ValidationSummary runat="server" ID="vsVisitasSitio" DisplayMode="SingleParagraph" CssClass="invalid-feedback" HeaderText="Falta informar: " ShowSummary="true" ShowValidationErrors="true" ValidationGroup="vgVisitasSitio" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pVisitasSitioExecAction" runat="server">
                            <div class="col-auto card-header-buttons" style="padding-top: .75rem; padding-bottom: .25rem;">
                                <div class="btn-group flex-wrap">
                                    <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgVisitasSitio" OnClick="btnAccept_Click">
									    <i class="fas fa-check"></i>&nbspAceptar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-secondary btn-sm" CommandArgument="0" OnClick="btnCancel_Click" CausesValidation="false">
									    <i class="fas fa-times"></i>&nbspCancelar
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pVisitasSitioAction" runat="server">
                            <div class="col-auto ml-auto card-header-buttons">
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnVisitasSitioList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnVisitasSitioAccion_Click">
									    <i class="fas fa-th-list"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnVisitasSitioAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnVisitasSitioAccion_Click">
										<i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnVisitasSitioEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnVisitasSitioAccion_Click">
										<i class="fas fa-pencil-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnVisitasSitioDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnVisitasSitioAccion_Click">
										<i class="far fa-trash-alt"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnVisitasSitioList" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnVisitasSitioAdd" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnVisitasSitioEdit" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnVisitasSitioDel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="gvVisitasSitio" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:Panel ID="pnlGrid" runat="server">
            <div class="gv-w">
                <asp:GridView ID="gvVisitasSitio" CssClass="gv" runat="server" PageSize="20" AllowPaging="true" AutoGenerateColumns="false"
                    DataKeyNames="idvisita_sitio,idarchivo" ShowHeaderWhenEmpty="true" AllowSorting="true"
                    OnRowDataBound="gvVisitasSitio_RowDataBound" OnPageIndexChanging="gvVisitasSitio_PageIndexChanging" OnRowCommand="gvVisitasSitio_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="fecha_visita" HeaderText="Fecha visita" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                        <asp:TemplateField ShowHeader="true" HeaderText="Observaciones">
                            <ItemTemplate>
                                <div class="gv-td-tl">
                                    <asp:Label ID="observaciones" runat="server" Text='<%# Eval("observaciones").ToString()%>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                            <HeaderTemplate>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnVisitasSitioAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="_Add" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnVisitasSitioAdd_Click">
										                    <i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn btn-grid-detail" Text="Detalle" CausesValidation="false" CommandName="_Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Visualizar registro"> 
                                        <i class="fas fa-info-circle"></i>
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
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>


<asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlDetail" runat="server" Visible="false" Width="100%">
            <div class="row">
                <asp:Panel ID="pnlVisita_sitio" runat="server" Width="100%">
                    <asp:HiddenField ID="hdd_idvisita_sitio" runat="server" Value="0" />
                    <div class="col-6 col-sm-4 col-md-3">
                        <div class="form-group-sm col">
                            <div style="width: 80%; float: left;" class="text-truncate">
                                <asp:Label ID="lblfechavisita" runat="server" >Fecha visita</asp:Label>
                            </div>
                            <div style="right: 0px; position: relative;">
                                <asp:RegularExpressionValidator ID="rev_fecha_visita" runat="server" ValidationGroup="vgVisitasSitio" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_visita" Display="Dynamic">
                                            <uc:ToolTip width="150px" ToolTip="Fecha con formato inválido" runat="server"/>
                                </asp:RegularExpressionValidator>
                                <asp:RangeValidator runat="server" ID="rv_fecha_visita" ValidationGroup="vgVisitasSitio" ControlToValidate="txt_fecha_visita">
                                            <uc:ToolTip width="130px" ToolTip="Fecha inválida" runat="server"/>
                                </asp:RangeValidator>
                                <asp:RequiredFieldValidator ID="rfv_fecha_visita" runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgVisitasSitio" ControlToValidate="txt_fecha_visita">
                                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                </asp:RequiredFieldValidator>
                            </div>
                            <ajax:CalendarExtender ID="ce_fecha_visita" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_visita" PopupButtonID="txt_fecha_visita" Format="yyyy-MM-dd" />
                            <asp:TextBox runat="server" ID="txt_fecha_visita" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-12">
                        <div class="form-group-sm col">
                            <label class="lblBasic">Observaciones</label>
                            <asp:RequiredFieldValidator ID="rfv_observaciones" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgVisitasSitio" ControlToValidate="txt_observaciones">
                                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" ID="txt_observaciones" CssClass="form-control form-control-xs" MaxLength="16000"
                                TextMode="MultiLine" Enabled="false" Rows="3" onKeyDown="MaxLengthText(this,16000);" onKeyUp="MaxLengthText(this,16000);"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <asp:UpdatePanel runat="server" ID="upImagenes" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlImagenes" runat="server" Visible="false" Width="100%">
                        <div class="col-12 uc-py-section-middle">
                            <div class="card-user-control subcontrol">
                                <div class="uc-center">
                                    <label class="section-title fwb">Evidencias</label></div>
                                <uc:FileUpload ID="ucImagenes" ControlID="fuImagenes" Extensions="jpg,png,jpeg,pdf" Multiple="true" runat="server"
                                    OnUserControlException="ucImagenes_UserControlException" OnViewDoc="ucImagenes_ViewDoc" />
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>


