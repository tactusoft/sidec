 <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivityManagement.ascx.cs" Inherits="SIDec.UserControls.Particular.ActivityManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/ListBox.ascx" TagName="ListBox" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>



<asp:UpdatePanel runat="server" ID="upActivityFoot" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divData" runat="server" >
            <div class="row">
                <asp:UpdatePanel runat="server" ID="upActivityMsg" UpdateMode="Conditional" class="alert-card card-header-message uc-div-message">
                    <ContentTemplate>
                        <div runat="server" id="msgActivity" class="alert d-none" role="alert"></div>
                        <div runat="server" id="msgActivityMain" class="d-none" role="alert">
                            <span runat="server" id="msgMainText"></span>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <asp:ValidationSummary runat="server" ID="vsActivity" DisplayMode="SingleParagraph" CssClass="invalid-feedback" HeaderText="Falta informar: " ShowSummary="true" ShowValidationErrors="true" ValidationGroup="vgActivityManagement" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="row">
                <div class="col-auto card-header-buttons" style="padding-top: .75rem; padding-bottom: .25rem;">
                    <asp:Panel ID="pActivityExecAction" runat="server">
                        <div class="btn-group flex-wrap">
                            <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgActivityManagement" OnClick="btnAccept_Click">
										                                            <i class="fas fa-check"></i>&nbspAceptar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-secondary btn-sm" CommandArgument="0" OnClick="btnCancel_Click" CausesValidation="false">
										                                            <i class="fas fa-times"></i>&nbspCancelar
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>

            <div class="row">
                <div class="col-auto ml-auto card-header-buttons">
                    <asp:Panel ID="pActivityAction" runat="server">
                        <div class="btn-group">
                            <asp:LinkButton ID="btnActivityList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnActivityAccion_Click">
										                                            <i class="fas fa-th-list"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnActivityAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnActivityAccion_Click">
										                                            <i class="fas fa-plus"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnActivityEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnActivityAccion_Click">
										                                            <i class="fas fa-pencil-alt"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnActivityDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnActivityAccion_Click" >
										                                            <i class="far fa-trash-alt"></i>
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnActivityList" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnActivityAdd" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnActivityEdit" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnActivityDel" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="gvActivity" />
    </Triggers>
</asp:UpdatePanel>



<asp:UpdatePanel ID="upActivity" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hddId" runat="server" Value="UserControlActivity" />
        <asp:HiddenField ID="hddIdBanco" runat="server" Value="0" />
        <asp:Panel ID="pnlGrid" runat="server">
            <div>
                <asp:CheckBox runat="server" ID="chk_activas" Text=" Solo activas" CssClass="fwb" Checked="true" OnCheckedChanged="chk_activas_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
            </div>
            <div class="gv-w" style="overflow-x:scroll">
                <asp:GridView ID="gvActivity" CssClass="gv" runat="server" PageSize="20" AllowPaging="true" AutoGenerateColumns="false" 
                    DataKeyNames="idbanco_actividad,activo,dias_disponibles,dias_en_tramite,fec_finalizacion" ShowHeaderWhenEmpty="true" AllowSorting="true" 
                    OnRowDataBound="gvActivity_RowDataBound" OnPageIndexChanging="gvActivity_PageIndexChanging" OnRowCommand="gvActivity_RowCommand">
                    <Columns>
                        <asp:TemplateField ShowHeader="true" HeaderText="Fase">
                            <ItemTemplate>
                                <div class="gv-td-tl">
                                    <asp:Label ID="lblEstado" runat="server" Text='<%# Eval("estado_actividad").ToString()%>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true" HeaderText="Actividad">
                            <ItemTemplate>
                                <div class="gv-td-tl">
                                    <asp:Label ID="lblActividad" runat="server" Text='<%# Eval("nombre").ToString()%>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="entidad" HeaderText="Entidad" />
                        <asp:BoundField DataField="fec_culminacion" HeaderText="Fec. posible cumplimiento" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Center"/>
                        <asp:TemplateField ShowHeader="true" HeaderText="Acciones">
                            <ItemTemplate>
                                <div class="gv-td-tl">
                                    <asp:Label ID="lblAcciones" runat="server" Text='<%# Eval("acciones").ToString() %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="tramite" HeaderText="Trámite asociado"/>                        
                        <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w30" HeaderText="">
                            <ItemTemplate>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn btn-grid-detail" Text="Detalle" CausesValidation="false" CommandName="_Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Visualizar registro"> 
										                    <i class="fas fa-info-circle"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-grid-edit" Text="Editar" CausesValidation="false" CommandName="_Edit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Editar registro"> 
										                    <i class="fas fa-pencil-alt"></i>
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
        
        <asp:Panel ID="pnlDetail" runat="server" Visible="false">
            <div class="row">
                <div class="form-group-sm col-12 col-md-3">
                    <asp:HiddenField ID="hdd_idbanco_actividad" runat="server" Value="0" />
                    <label class="lblBasic">Fase</label>
                    <asp:TextBox runat="server" ID="txt_estado_actividad" CssClass="form-control form-control-xs" Enabled="false" />
                </div>
                <div class="form-group-sm col-12 col-md-9">
                    <label class="lblBasic">Actividad</label>
                    <asp:TextBox runat="server" ID="txt_actividad" CssClass="form-control form-control-xs" Enabled="false" />
                </div>
                <div class="form-group-sm col-12 col-md-6">                    
                    <label class="lblBasic">Entidad</label>
                    <asp:TextBox runat="server" ID="txt_entidad" CssClass="form-control form-control-xs" Enabled="false" />
                    <asp:HiddenField ID="hdd_identidad" runat="server" Value="0" />
                </div>
                <div class="form-group-sm col-6 col-sm-3">
                    <asp:Label ID="lbl_fecposible" runat="server" class="lblBasic" ToolTip="Fecha posible cumplimiento">Fecha posible cumplimiento</asp:Label>
                    <asp:TextBox runat="server" ID="txt_fec_culminacion" CssClass="form-control form-control-xs" TextMode="Date" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-6 col-sm-3">
                    <label class="lblBasic">Documento</label><br />
                    <div style="float: left; min-width: 50px;">
                        <asp:LinkButton ID="lblPdfTramite" runat="server" CssClass="btn btn-danger btn-sm" Text="" CausesValidation="false" OnClick="lblPdfTramite_Click" ToolTip="Ver documento">
						    <i class="fas fa-file-pdf"></i>
                        </asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lbLoadActivity" runat="server" CssClass="btn btn-success btn-sm" Text="" CausesValidation="false" ToolTip="Cargar archivo">
						    <i class="fas fa-upload"></i>
                        </asp:LinkButton>
                    </div>
                    <div class="labelLimited" style="padding-left: 10px; height: 20px">
                        <asp:HiddenField ID="hdd_ruta_tramite" runat="server" />
                        <asp:Label ID="lblInfoFileActivity" runat="server" />
                        <asp:Label ID="lblErrorFileActivity" runat="server" CssClass="error" />
                        <ajax:AsyncFileUpload ID="fuLoadActivity" runat="server" onchange="infoFileActivity();" Style="display: none" PersistFile="true" CompleteBackColor="Transparent" ErrorBackColor="Red" />
                    </div>
                </div>



                <div class="form-group-sm col-12 col-lg-6">
                    <label class="lblBasic">Trámite asociado</label>
                    <asp:RequiredFieldValidator ID="rfv_idtramite" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgActivityManagement" ControlToValidate="ddla_idtramite">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="ddla_idtramite" CssClass="form-control form-control-xs" onchange="javascript:change_tramiteA();return false;">
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12 col-lg-6" id="divOtroTramiteA" style="display: none;">
                    <label class="lblBasic">Otro trámite asociado</label>
                    <asp:TextBox runat="server" ID="txta_otrotramite" CssClass="form-control form-control-xs"></asp:TextBox>
                </div>
                <div class="col-12" style="padding: 10px; background-color: #fafafa;">
                    <div class="form-group-sm col-12">
                        <asp:HiddenField ID="hdd_idactividad_accion" runat="server" Value="0" />
                        <label class="lblBasic">Acciones</label>
                        <asp:TextBox runat="server" ID="txt_acciones" CssClass="form-control form-control-xs" MaxLength="4000" 
                            TextMode="MultiLine"  onKeyDown="MaxLengthText(this,4000);" onKeyUp="MaxLengthText(this,4000);" Enabled="false" Rows="4"></asp:TextBox>
                    </div>
                    <div class="form-group-sm col">
                        <div class="gv-w">
                            <asp:GridView ID="gvAcciones" runat="server" CssClass="gv w-100" AllowSorting="false" AllowPaging="false" DataKeyNames="idactividad_accion,acciones,fecha" ShowHeader="false"
                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" OnRowDataBound="gvAcciones_RowDataBound" OnRowCommand="gvAcciones_RowCommand">
                                <Columns>
                                    <asp:TemplateField ShowHeader="true" HeaderText="Acciones">
                                        <ItemTemplate>
                                            <div class="gv-td-tl">
                                                <label for="lblAcciones"><span class="fs10 fwb"><%# Convert.ToDateTime(Eval("fecha").ToString()).ToString("yyyy/MM/dd")%></span>  <%# Eval("acciones").ToString()%></label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40" Visible="false">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-grid-edit" Text="Editar" CausesValidation="false" CommandName="_Edit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Editar registro"> 
										                                    <i class="fas fa-pencil-alt"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-grid-delete" Text="Eliminar" CausesValidation="false" CommandName="_Delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Eliminar registro"> 
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
                </div>

            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
