<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tracing.ascx.cs" Inherits="SIDec.UserControls.Particular.Tracing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/NewReferenceList.ascx" TagPrefix="uc" TagName="NewReferenceList" %>
<%@ Register Src="~/UserControls/TextBoxGrid.ascx" TagPrefix="uc" TagName="TextBoxGrid" %>

<asp:HiddenField ID="hddIdBanco" runat="server" Value="0" />
<asp:HiddenField ID="hddIdProyecto" runat="server" Value="0" />
<asp:HiddenField ID="hddId" runat="server" Value="UserControlTracing" />

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel runat="server" ID="upTracingFoot" UpdateMode="Conditional">
    <ContentTemplate>
        <uc:TextBoxGrid ID="uctbgParticipantes" runat="server" OnAccept="uctbgParticipantes_Accept" OnRemove="uctbgParticipantes_Remove" ControlID="Seguimiento.Participante"/>
                    
        <div id="divData" runat="server" >
            <div class="row">
                <asp:UpdatePanel runat="server" ID="upTracingMsg" UpdateMode="Conditional" class="alert-card card-header-message uc-div-message">
                    <ContentTemplate>
                        <div runat="server" id="msgTracing" class="alert d-none" role="alert"></div>
                        <div runat="server" id="msgMain" class="d-none" role="alert">
                            <span runat="server" id="msgMainText"></span>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <asp:ValidationSummary runat="server" ID="vsTracing" DisplayMode="SingleParagraph" CssClass="invalid-feedback" HeaderText="Falta informar: " ShowSummary="true" ShowValidationErrors="true" ValidationGroup="vgTracing" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="row">
                <div class="col-auto card-header-buttons" style="padding-top: .75rem; padding-bottom: .25rem;">
                    <asp:Panel ID="pTracingExecAction" runat="server">
                        <div class="btn-group flex-wrap">
                            <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgTracing" OnClick="btnAccept_Click">
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
                    <asp:Panel ID="pTracingAction" runat="server">
                        <div class="btn-group">
                            <asp:LinkButton ID="btnTracingList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnTracingAccion_Click">
										                                            <i class="fas fa-th-list"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnTracingAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnTracingAccion_Click">
										                                            <i class="fas fa-plus"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnTracingEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnTracingAccion_Click">
										                                            <i class="fas fa-pencil-alt"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnTracingDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnTracingAccion_Click" >
										                                            <i class="far fa-trash-alt"></i>
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnTracingList" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnTracingAdd" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnTracingEdit" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnTracingDel" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnParticipantes" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="gvTracing" />
        <asp:AsyncPostBackTrigger ControlID="gvParticipantes" />
        <asp:AsyncPostBackTrigger ControlID="gvRadicado" />
    </Triggers>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upTracing" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:Panel ID="pnlGrid" runat="server">
            <div class="form-group-sm col-6 col-md-4 col-lg-3 col-xl-2">
                <label for="txte_fec_radicacion" class="lblBasic">Mes</label>
                <ajax:CalendarExtender ID="calendar1" TargetControlID="txt_mes_visualizacion" Format="yyyy-MM" ClientIDMode="Static" runat="server"
                    DefaultView="Years" OnClientShown="onCalendarShown" OnClientHidden="onCalendarHidden" PopupButtonID="imgStart" />
                <asp:TextBox runat="server" ID="txt_mes_visualizacion" CssClass="form-control form-control-xs" MaxLength="8" TextMode="Month" AutoPostBack="true" OnTextChanged="txt_mes_visualizacion_TextChanged"></asp:TextBox>
            </div>
            <div class="gv-w">
                <asp:GridView ID="gvTracing" CssClass="gv" runat="server" PageSize="20" AllowPaging="true" DataKeyNames="idseguimiento,seg_historico" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" AllowSorting="true" OnRowCommand="gvTracing_RowCommand"
                    OnSelectedIndexChanged="gvTracing_SelectedIndexChanged" OnPageIndexChanging="gvTracing_PageIndexChanging"
                    OnSorting="gvTracing_Sorting" OnRowDataBound="gvTracing_RowDataBound" OnRowCreated="gv_RowCreated">
                    <Columns>
                        <asp:BoundField DataField="fec_seguimiento" HeaderText="Fecha" SortExpression="fec_seguimiento" DataFormatString="{0:yyyy/MM/dd}"/>
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
                                    <asp:Label ID="lblActividad" runat="server" Text='<%# Eval("actividad").ToString()%>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true" HeaderText="Gestión">
                            <ItemTemplate>
                                <div class="gv-td-tl">
                                    <asp:Label ID="lblGestion" runat="server" Text='<%# Eval("gestion").ToString() %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true" HeaderText="Compromisos">
                            <ItemTemplate>
                                <div class="gv-td-tl">
                                    <asp:Label ID="lblCompromisos" runat="server" Text='<%# Eval("compromisos").ToString()%>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                            <HeaderTemplate>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="_Add" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnTracingAdd_Click">
										                    <i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn btn-grid-detail" Text="Detalle" CausesValidation="false" CommandName="_Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Visualizar registro"> 
										                    <i class="fas fa-info-circle"></i>
                                    </asp:LinkButton>
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
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>


<asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
        <asp:Panel ID="pnlDetail" runat="server" Visible="false">
            <div class="row">
                <div class="col-sm-12 text-primary">
                    <label class="fs10 fwb">SEGUIMIENTO</label>
                </div>
            </div>
            <div class="row">
                <div class="form-group-sm col-5 col-sm-4 col-md-3 col-lg-2">
                    <asp:HiddenField ID="hdd_idseguimiento" runat="server" Value="0" />
                    <label for="txt_fec_seguimiento" class="lblBasic">Fecha de seguimiento</label>
                    <asp:RegularExpressionValidator ID="rev_fec_seguimiento" runat="server" ValidationGroup="vgTracing" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fec_seguimiento" Display="Dynamic">
                            <uc:ToolTip width="150px" ToolTip="Fecha con formato inválido" runat="server"/>
                    </asp:RegularExpressionValidator>
                    <asp:RangeValidator runat="server" ID="rv_fec_seguimiento" ValidationGroup="vgTracing" ControlToValidate="txt_fec_seguimiento">
                            <uc:ToolTip width="130px" ToolTip="Fecha inválida" runat="server"/>
                    </asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="rfv_fec_seguimiento" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgTracing" ControlToValidate="txt_fec_seguimiento">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <ajax:CalendarExtender ID="ce_fec_seguimiento" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fec_seguimiento" PopupButtonID="txt_fec_seguimiento" Format="yyyy-MM-dd" />
                    <asp:TextBox runat="server" ID="txt_fec_seguimiento" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8"></asp:TextBox>
                </div>
                <div class="form-group-sm col-7 col-sm-8 col-md-9 col-lg-10">
                    <div class="form-group-sm col">
                        <label class="lblBasic">Tema a tratar</label>
                        <asp:RequiredFieldValidator ID="rfv_asunto" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgTracing" ControlToValidate="txt_asunto">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                        </asp:RequiredFieldValidator>
                        <asp:TextBox runat="server" ID="txt_asunto" CssClass="form-control form-control-xs"  MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group-sm col-12 col-md-4 col-lg-3">
                    <label class="lblBasic">Tipo actividad</label>
                    <asp:RequiredFieldValidator ID="rfv_idtipo_actividad" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                        ValidationGroup="vgTracing" ControlToValidate="ddl_idtipo_actividad" >
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="ddl_idtipo_actividad" CssClass="form-control form-control-xs" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddl_idtipo_actividad_SelectedIndexChanged">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12 col-md-8 col-lg-3">
                    <label class="lblBasic">Fase</label>
                    <asp:TextBox runat="server" ID="txt_estado_actividad" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-lg-6">
                    <label class="lblBasic">Actividad</label>
                    <asp:RequiredFieldValidator ID="rfv_idbanco_actividad" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                        ValidationGroup="vgTracing" ControlToValidate="ddl_idbanco_actividad" InitialValue="">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfv_id_banco_actividad_na" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                        ValidationGroup="vgTracing" ControlToValidate="ddl_idbanco_actividad" InitialValue="-10">
                            <uc:ToolTip width="190px" ToolTip="Opción inválida, complete el registro de la nueva actividad" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="ddl_idbanco_actividad" CssClass="form-control form-control-xs" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddl_idbanco_actividad_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-12">
                    <div class="form-group-sm col">
                        <label class="lblBasic">Gestión realizada</label>
                        <asp:RequiredFieldValidator ID="rfv_gestion" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgTracing" ControlToValidate="txt_gestion">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                        </asp:RequiredFieldValidator>
                        <asp:TextBox runat="server" ID="txt_gestion" CssClass="form-control form-control-xs" MaxLength="10000" 
                            TextMode="MultiLine" Enabled="false" Rows="4" onKeyDown="MaxLengthText(this,10000);" onKeyUp="MaxLengthText(this,10000);"></asp:TextBox>
                    </div>
                </div>

                <div class="col-12">
                    <div class="form-group-sm col">
                        <label class="lblBasic">Compromisos</label>
                        <asp:TextBox runat="server" ID="txt_compromisos" CssClass="form-control form-control-xs" MaxLength="4000" 
                            TextMode="MultiLine" Enabled="false" Rows="4" onKeyDown="MaxLengthText(this, 4000);" onKeyUp="MaxLengthText(this, 4000);"></asp:TextBox>
                    </div>
                </div>

                <div class="col-12" style="padding: 10px; background-color: #fafafa;">
                    <div class="form-group-sm col">
                        <asp:Panel ID="pnlGridRadicado" runat="server">
                            <div class="card-header card-header-main text-center">
                                <ul runat="server" id="ulTracingSection" class="nav nav-pills border-bottom-0 mr-auto">
                                    <li class="nav-item">
                                        <asp:LinkButton ID="lbTracingSection_0" runat="server" CssClass="nav-link active" CommandArgument="0" CausesValidation="false" OnClick="btnTracingSection_Click">Participantes</asp:LinkButton>
                                    </li>
                                    <li class="nav-item">
                                        <asp:LinkButton ID="lbTracingSection_1" runat="server" CssClass="nav-link" CommandArgument="1" CausesValidation="false" OnClick="btnTracingSection_Click">Trámites ante entidades</asp:LinkButton>
                                    </li>
                                </ul>
                            </div>
                            <asp:MultiView runat="server" ID="mvTracingSection" ActiveViewIndex="0">
                                <asp:View runat="server" ID="vParticipantes"> 
                                    <div class="gv-w">
                                        <asp:GridView ID="gvParticipantes" CssClass="gv" runat="server" AllowPaging="false" DataKeyNames="idseguimiento_participante"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnRowDataBound="gvParticipante_RowDataBound" 
                                            OnRowCommand="gvParticipante_RowCommand" EmptyDataText="No se han incluido participantes" ShowHeader="true" Visible="true" >
                                            <Columns>
                                                <asp:BoundField DataField="entidad" HeaderText="Entidad"/>
                                                <asp:BoundField DataField="participantes" HeaderText="Participantes"/>
                                                <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                                                    <HeaderTemplate>
                                                        <div class="btn-group">
                                                            <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="_Add" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnParticipanteAdd_Click">
										                                    <i class="fas fa-plus"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="btn-group">
                                                            <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn btn-grid-detail" Text="Detalle" CausesValidation="false" CommandName="_Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Visualizar registro"> 
										                    <i class="fas fa-info-circle"></i>
                                                            </asp:LinkButton>
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

                                   
                                </asp:View>
                                <asp:View runat="server" ID="vRadicados">

                                    <div class="gv-w">
                                        <asp:GridView ID="gvRadicado" CssClass="gv" runat="server" PageSize="20" AllowPaging="true" DataKeyNames="idseguimiento_radicado,ruta_tramite"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnRowDataBound="gvRadicado_RowDataBound" OnRowCommand="gvRadicado_RowCommand"
                                            EmptyDataText="No se han incluido Trámites">
                                            <Columns>
                                                <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:yyyy/MM/dd}"/>
                                                <asp:BoundField DataField="entidad_radicado" HeaderText="Entidad radicado"/>
                                                <asp:BoundField DataField="tiporadicado" HeaderText="Tipo Radicado"/>
                                                <asp:BoundField DataField="radicado" HeaderText="Radicado"/>
                                                <asp:BoundField DataField="tramite" HeaderText="Trámite"/>
                                                <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                                                    <HeaderTemplate>
                                                        <div class="btn-group">
                                                            <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="_Add" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnRadicadoAdd_Click">
										                                    <i class="fas fa-plus"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="btn-group">
                                                            <asp:ImageButton runat="server" CommandName="_OpenFile" CommandArgument='<%# Container.DisplayIndex %>' ImageUrl="~/images/icon/pdf-icon.png" CssClass="btn"
                                                                Visible='<%# (String.IsNullOrEmpty(Eval("ruta_tramite").ToString())) ? false : true %>' ToolTip="Abrir documento" />

                                                            <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn btn-grid-detail" Text="Detalle" CausesValidation="false" CommandName="_Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Visualizar registro"> 
										                    <i class="fas fa-info-circle"></i>
                                                            </asp:LinkButton>
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
                                </asp:View>
                            </asp:MultiView>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upRadicado" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:HiddenField ID="hdd_idseguimiento_participante" runat="server" Value="0" Visible="false" />

        <asp:Panel ID="pnlParticipante" runat="server">
            <div class="row">
                <div class="col-sm-12 text-primary">
                    <label class="fs10 fwb">PARTICIPANTES</label>
                </div>
            </div>
            <div class="row">
                <div class="form-group-sm col-12 col-sm-5 col-md-4 col-lg-3">
                    <label for="ddl_identidad" class="">Entidad</label>
                    <asp:DropDownList runat="server" ID="ddl_identidad" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12 col-sm-7 col-md-8 col-lg-9">
                    <label class="lblBasic">Participantes</label>
                    <asp:Button runat="server" ID="btnParticipantes" CssClass="form-control form-control-xs" Style="text-align: left;" OnClick="btnParticipantes_Click" />
                </div>
            </div>
        </asp:Panel>

        <asp:HiddenField ID="hdd_idseguimiento_radicado" runat="server" Value="0" Visible="false" />
        <asp:Panel ID="pnlTramite" runat="server">
            <div class="row">
                <div class="col-sm-12 text-primary">
                    <label class="fs10 fwb">TRÁMITES</label>
                </div>
            </div>
            <div class="row">
                <div class="form-group-sm col-6 col-md-4 col-lg-3">
                    <label class="lblBasic">Entidad radicado</label>
                    <asp:RequiredFieldValidator ID="rfv_identidad_radicado" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                       ValidationGroup="vgTracing" ControlToValidate="ddl_identidad_radicado">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                   </asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="ddl_identidad_radicado" CssClass="form-control form-control-xs" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddl_identidad_radicado_SelectedIndexChanged">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-6 col-md-4 col-lg-3">
                    <label class="lblBasic">Tipo radicado</label>
                    <asp:RequiredFieldValidator ID="rfv_idtipo_radicado" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                       ValidationGroup="vgTracing" ControlToValidate="ddl_idtipo_radicado">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                   </asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="ddl_idtipo_radicado" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                        <asp:ListItem Value="1">Ingreso</asp:ListItem>
                        <asp:ListItem Value="2">Salida</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-6 col-md-4 col-lg-3">
                    <asp:Label ID="lblfecha" runat="server" class="lblBasic">Fecha</asp:Label>
                    <asp:RegularExpressionValidator ID="rev_fecha" runat="server" ValidationGroup="vgTracing" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha" Display="Dynamic">
                                <uc:ToolTip width="150px" ToolTip="Fecha con formato inválido" runat="server"/>
                    </asp:RegularExpressionValidator>
                    <asp:RangeValidator runat="server" ID="rv_fecha" ValidationGroup="vgTracing" ControlToValidate="txt_fecha">
                                <uc:ToolTip width="130px" ToolTip="Fecha inválida" runat="server"/>
                    </asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="rfv_fecha" runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgTracing" ControlToValidate="txt_fecha">
                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <ajax:CalendarExtender ID="ce_fecha" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha" PopupButtonID="txt_fecha" Format="yyyy-MM-dd" />
                    <asp:TextBox runat="server" ID="txt_fecha" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8"></asp:TextBox>
                </div>
                <div class="form-group-sm col-6 col-md-4 col-lg-3">
                    <label class="lblBasic">Asunto</label>
                    <asp:DropDownList runat="server" ID="ddl_idasunto" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12 col-md-4 col-lg-6">
                    <label class="lblBasic">Trámite asociado</label>
                    <asp:DropDownList runat="server" ID="ddlt_idtramite" CssClass="form-control form-control-xs" onchange="javascript:change_tramiteT();return false;">
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12 col-md-4 col-lg-6" id="divOtroTramiteT" >
                    <label class="lblBasic">Otro trámite asociado</label>
                    <asp:TextBox runat="server" ID="txtt_otrotramite" CssClass="form-control form-control-xs" MaxLength="200" disabled="disabled"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-sm-6">
                    <label class="lblBasic">Radicado</label>
                    <asp:TextBox runat="server" ID="txt_radicado" CssClass="form-control form-control-xs" Enabled="false" MaxLength="200"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-sm-6">
                    <label class="lblBasic">Documento</label><br />
                    <div style="float: left; min-width: 50px;">
                        <asp:LinkButton ID="lblPdfTramite" runat="server" CssClass="btn btn-danger btn-sm" Text="" CausesValidation="false" OnClick="lblPdfTramite_Click" ToolTip="Ver documento">
						    <i class="fas fa-file-pdf"></i>
                        </asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lbLoadTracing" runat="server" CssClass="btn btn-success btn-sm" Text="" CausesValidation="false" ToolTip="Cargar archivo">
						    <i class="fas fa-upload"></i>
                        </asp:LinkButton>
                    </div>
                    <div class="labelLimited" style="padding-left: 10px; height: 20px">
                        <asp:HiddenField ID="hdd_ruta_tramite" runat="server" />
                        <asp:Label ID="lblInfoFileTracing" runat="server" />
                        <asp:Label ID="lblErrorFileTracing" runat="server" CssClass="error" />
                        <ajax:AsyncFileUpload ID="fuLoadTracing" runat="server" onchange="infoFileTracing();" Style="display: none" PersistFile="true" CompleteBackColor="Transparent" ErrorBackColor="Red" />
                    </div>
                </div>

                <div class="form-group-sm col-12">
                    <label class="lblBasic">Observación radicado</label>
                    <asp:TextBox runat="server" ID="txt_observaciones_radicado" CssClass="form-control form-control-xs" MaxLength="15000" 
                        TextMode="MultiLine"  onKeyDown="MaxLengthText(this,15000);" onKeyUp="MaxLengthText(this,15000);" Enabled="false" Rows="4"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

