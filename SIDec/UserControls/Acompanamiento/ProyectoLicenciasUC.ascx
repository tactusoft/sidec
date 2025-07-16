<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ProyectoLicenciasUC.ascx.cs" Inherits="SIDec.UserControls.Acompanamiento.ProyectoLicenciasUC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/FileUpload.ascx" TagName="FileUpload" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>
    <script type="text/javascript" src="./UserControls/Acompanamiento/Acompanamiento.js"></script>

<asp:HiddenField ID="hddProyectoLicenciaPrimary" runat="server" Value="0" />
<asp:HiddenField ID="hddId" runat="server" Value="UserControlProyectoLicencias" />
<asp:HiddenField ID="hddIdProyecto" runat="server" Value="0" />

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>


<asp:UpdatePanel ID="upProyectoLicencia" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:UpdatePanel runat="server" ID="upProyectoLicenciaFoot" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divData" runat="server" class="main-section">
                    <div class="row">
                        <asp:UpdatePanel runat="server" ID="upProyectoLicenciaMsg" UpdateMode="Conditional" class="alert-main msgusercontrol">
                            <ContentTemplate>
                                <div runat="server" id="msgProyectoLicencia" class="alert d-none" role="alert"></div>
                                <div runat="server" id="msgProyectoLicenciaMain" class="d-none" role="alert">
                                    <span runat="server" id="msgMainText"></span>
                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <asp:ValidationSummary runat="server" ID="vsProyectoLicencia" DisplayMode="SingleParagraph" CssClass="invalid-feedback" HeaderText="Falta informar: " ShowSummary="true" ShowValidationErrors="true" ValidationGroup="vgProyectoLicencia" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pProyectoLicenciaExecAction" runat="server">
                            <div class="col-auto card-header-buttons" >
                                <div class="btn-group flex-wrap">
                                    <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgProyectoLicencia" OnClick="btnAccept_Click">
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
                        <asp:Panel ID="pProyectoLicenciaAction" runat="server">
                            <div class="col-auto ml-auto card-header-buttons">
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnProyectoLicenciaList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnProyectoLicenciaAccion_Click">
							            <i class="fas fa-th-list"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnProyectoLicenciaAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnProyectoLicenciaAccion_Click">
								        <i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnProyectoLicenciaEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnProyectoLicenciaAccion_Click">
								        <i class="fas fa-pencil-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnProyectoLicenciaDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnProyectoLicenciaAccion_Click">
								        <i class="far fa-trash-alt"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProyectoLicenciaList" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoLicenciaAdd" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoLicenciaEdit" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoLicenciaDel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="gvProyectoLicencia" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:Panel ID="pnlGrid" runat="server">
            <div class="gv-w">
                <asp:GridView ID="gvProyectoLicencia" CssClass="gv" runat="server" PageSize="500" AllowPaging="true" DataKeyNames="au_proyecto_licencia,idorigen,ruta_licencia" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
                    OnRowDataBound="gvProyectoLicencia_RowDataBound" OnPageIndexChanging="gvProyectoLicencia_PageIndexChanging" OnRowCommand="gvProyectoLicencia_RowCommand" EmptyDataText="No se encontraron registros">
                    <Columns>
                        <asp:BoundField DataField="origen" HeaderText="Origen" ItemStyle-CssClass="t-c" />
                        <asp:BoundField DataField="tipo_licencia" HeaderText="Tipo licencia" />
                        <asp:BoundField DataField="curador" HeaderText="Curadoria" ItemStyle-CssClass="t-c" />
                        <asp:BoundField DataField="licencia" HeaderText="No. Licencia" />
                        <asp:BoundField DataField="fecha_ejecutoria" HeaderText="Fecha ejecutoria" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-CssClass="t-c" />
                        <asp:BoundField DataField="termino_vigencia_meses" HeaderText="Vigencia" ItemStyle-CssClass="t-c"/>
                        <asp:BoundField DataField="nombreproyecto" HeaderText="Nombre proyecto"/>
                        <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="w40">
                            <HeaderTemplate>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnProyectoLicenciaAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="_Add" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnProyectoLicenciaAdd_Click">
										<i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn btn-grid-detail" Text="Detalle" CausesValidation="false" CommandName="_Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Visualizar registro"> 
									    <i class="fas fa-info-circle"></i>
                                    </asp:LinkButton>
                                    <asp:ImageButton runat="server" CommandName="_OpenFile" CommandArgument='<%# Container.DisplayIndex %>' ImageUrl="~/images/icon/pdf-icon.png" CssClass="btn"
                                        Visible='<%# (String.IsNullOrEmpty(Eval("ruta_licencia").ToString())) ? false : true %>' ToolTip="Abrir documento" />
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-grid-edit" Text="Editar" CausesValidation="false" CommandName="_Edit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Editar registro" Visible="false"> 
										<i class="fas fa-pencil-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-grid-delete" Text="Eliminar" CausesValidation="false" CommandName="_Delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Eliminar registro" Visible="false"> 
										<i class="fas fa-trash-alt"></i>
                                    </asp:LinkButton>                                 
                                </div>
                                </itemtemplate>
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
            <div class="form-group">
                <asp:Panel ID="pnlProyectoLicencia" runat="server" Width="100%">
                    <asp:HiddenField ID="hdd_au_proyecto_licencia" runat="server" Value="0" />
                    <asp:HiddenField ID="hdd_idorigen" runat="server" Value="0" />
                    <div class="row first-row">
                        <asp:TextBox runat="server" ID="txt_au_proyecto_licencia" Enabled="false" Visible="false"></asp:TextBox>
                        <asp:TextBox runat="server" ID="txt_cod_proyecto__licencia" Enabled="false" Visible="false"></asp:TextBox>

                        <div class="form-group-sm col-6 col-sm-4 col-md-3 col-xl-2">
                            <asp:Label runat="server" id="lbl_ddlb_id_fuente_informacion" class="">Fuente información</asp:Label>
                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectoLicencia" ControlToValidate="ddlb_id_fuente_informacion">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                            <asp:DropDownList runat="server" ID="ddlb_id_fuente_informacion" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group-sm col-6 col-sm-5 col-md-3 col-xl-2">
                            <asp:Label runat="server" id="lbl_ddl_id_tipo_licencia" class="">Tipo licencia</asp:Label>
                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectoLicencia" ControlToValidate="ddl_id_tipo_licencia">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                            <asp:DropDownList runat="server" ID="ddl_id_tipo_licencia" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group-sm col-4 col-sm-3 col-md-2">
                            <asp:Label runat="server" id="lbl_ddl_curador" class="">Curaduría</asp:Label>
                            <asp:DropDownList runat="server" ID="ddl_curador" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                <asp:ListItem Value="1">Curaduría 1</asp:ListItem>
                                <asp:ListItem Value="2">Curaduría 2</asp:ListItem>
                                <asp:ListItem Value="3">Curaduría 3</asp:ListItem>
                                <asp:ListItem Value="4">Curaduría 4</asp:ListItem>
                                <asp:ListItem Value="5">Curaduría 5</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group-sm col-8 col-sm-6 col-md-4">
                            <asp:Label runat="server" id="lbl_txt_licencia" class="">Número licencia</asp:Label>
                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectoLicencia" ControlToValidate="txt_licencia">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" ID="txt_licencia" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col-6 col-sm-3 col-md-3 col-lg-2">
                            <asp:Label runat="server" id="lbl_txt_fecha_ejecutoria" class="">Fecha ejecutoria</asp:Label>
                            <asp:RegularExpressionValidator ID="revfecha_ejecutoria" runat="server" ValidationGroup="vgProyectoLicencia" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_ejecutoria" Display="Dynamic">
                                                                        <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                            </asp:RegularExpressionValidator>
                            <asp:RangeValidator runat="server" ID="rvfecha_ejecutoria" ValidationGroup="vgProyectoLicencia" ControlToValidate="txt_fecha_ejecutoria">
                                                                        <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                            </asp:RangeValidator>
                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectoLicencia" ControlToValidate="txt_fecha_ejecutoria">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                            <ajax:CalendarExtender ID="cal_fecha_ejecutoria" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_ejecutoria" PopupButtonID="txt_fecha_ejecutoria" Format="yyyy-MM-dd" />
                            <asp:TextBox runat="server" ID="txt_fecha_ejecutoria" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col-6 col-sm-3 col-md-2">
                            <asp:Label runat="server" id="lbl_txt_termino_vigencia_meses" class="">Vigencia meses</asp:Label>
                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectoLicencia" ControlToValidate="txt_termino_vigencia_meses">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" ID="txt_termino_vigencia_meses" CssClass="form-control form-control-xs" MaxLength="3" onkeypress="return SoloEntero(event);"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col-12 col-sm-8 col-md-5 col-xl-6">
                            <asp:Label runat="server" id="lbl_txta_nombreproyecto" class="">Nombre proyecto</asp:Label>
                            <asp:TextBox runat="server" ID="txt_nombreproyecto" CssClass="form-control form-control-xs"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col-12 col-sm-4 col-md-2 col-lg-3">
                            </br>
                            <div style="float: left; min-width: 50px;">
                                <asp:LinkButton ID="lblPdfLicense" runat="server" CssClass="btn btn-danger btn-sm" Text="" CausesValidation="false" OnClick="lblPdfLicense_Click" ToolTip="Ver documento">
						            <i class="fas fa-file-pdf"></i>
                                </asp:LinkButton>&nbsp;
                                <asp:LinkButton ID="lblLoadLicense" runat="server" CssClass="btn btn-success btn-sm" Text="" CausesValidation="false" ToolTip="Cargar archivo">
									<i class="fas fa-upload"></i>
                                </asp:LinkButton>
                            </div>
                            <div class="labelLimited" style="padding-left: 10px;">
                                <asp:HiddenField ID="hdd_ruta_licencia" runat="server" />
                                <asp:Label ID="lblInfoFileLicense" runat="server" />
                                <asp:TextBox ID="txt_ruta_licencia" runat="server" Style="display: none" />
                                <asp:Label ID="lblErrorFileLicense" runat="server" CssClass="error" />
                                <ajax:AsyncFileUpload id="fuLoadLicense" runat="server" onchange="infoFileLicense();" style="display: none" PersistFile="true" 
                                    CompleteBackColor="Transparent" ErrorBackColor="Red" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12 col-sm-12 col-md-4 col-lg-3">
                            <asp:Panel runat="server" ID="pPlanesPLicenciasUrb">
                                <div class="card mb-3">
                                    <div class="card-header-title">Urbanismo</div>
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="form-group-sm col-8">
                                                <asp:Label runat="server" ID="lbl_txt_plano_urbanistico_aprobado" class="text-truncate" ToolTip="Plano urbanístico aprobado">Plano urb. aprobado</asp:Label>
                                                <asp:TextBox runat="server" ID="txt_plano_urbanistico_aprobado" CssClass="form-control form-control-xs"></asp:TextBox>
                                            </div>
                                            <div class="form-group-sm col-4">
                                                <div style="width: 80%; float: left;" class="text-truncate">
                                                    <asp:Label runat="server" id="lbl_txt_porcentaje_ejecucion_urbanismo" ToolTip="% Ejecutado">% Ejecutado</asp:Label>
                                                </div>
                                                <asp:RangeValidator runat="server" ValidationGroup="vgProyectoLicencia" ControlToValidate="txt_porcentaje_ejecucion_urbanismo" MinimumValue="0" MaximumValue="100" Type="Double" Display="Dynamic">
                                                    <uc:ToolTip width="160px" ToolTip="El valor VIP debe estar entre 0 y 100" runat="server"/>
                                                </asp:RangeValidator>
                                                <div class="input-group input-group-xs">
                                                    <asp:TextBox runat="server" ID="txt_porcentaje_ejecucion_urbanismo" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    <div class="input-group-append">
                                                        <span class="input-group-text">%</span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-12">                                                
                                                <div class="col-12 fwb">
                                                    <caption>Datos urbanísticos (m<sup>2</sup>)</caption>
                                                </div>
                                                <div class="row section-sm">
                                                    <div class="col-4 col-md-12">
                                                        <asp:Label Text="lblarea_bruta__licencia" runat="server" ToolTip="Área bruta">Área bruta</asp:Label> 
                                                        <asp:TextBox runat="server" ID="txt_area_bruta__licencia" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4 col-md-12">
                                                        <asp:Label Text="lblarea_neta" runat="server" ToolTip="Área neta">Área neta</asp:Label> 
                                                        <asp:TextBox runat="server" ID="txt_area_neta" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4 col-md-12">
                                                        <asp:Label Text="lblarea_util__licencia" runat="server" ToolTip="Área útil">Área útil</asp:Label> 
                                                        <asp:TextBox runat="server" ID="txt_area_util__licencia" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                                
                                                <div class="col-12 fwb">
                                                    <caption>Cesiones (m<sup>2</sup>)</caption>
                                                </div>
                                                 <div class="row section-sm">
                                                    <div class="col-4 col-md-12">
                                                        <asp:Label Text="lblarea_cesion_zonas_verdes" runat="server" ToolTip="Zonas verdes">Zonas verdes</asp:Label> 
                                                        <asp:TextBox runat="server" ID="txt_area_cesion_zonas_verdes" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4 col-md-12">
                                                        <asp:Label Text="lblarea_cesion_vias" runat="server" ToolTip="Vías">Vías</asp:Label> 
                                                        <asp:TextBox runat="server" ID="txt_area_cesion_vias" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4 col-md-12">
                                                        <asp:Label Text="lblarea_cesion_eq_comunal" runat="server" ToolTip="Equipamiento comunal">Eq. comunal</asp:Label>                                                         
                                                        <asp:TextBox runat="server" ID="txt_area_cesion_eq_comunal" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </asp:Panel>
                        </div>

                        <div class="col-12 col-sm-12 col-md-8 col-lg-9">
                            <asp:Panel runat="server" ID="pPlanesPLicenciasConst">
                                <div class="card mb-3">
                                    <div class="card-header-title">Construcción</div>
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-lg-9">
                                                <div class="row">
                                                    <div class="col-6">
                                                        <div class="form-group-sm">
                                                            <asp:Label runat="server" id="lbl_ddlb_id_obligacion_VIS" class="">Obligación VIS</asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlb_id_obligacion_VIS" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-6">
                                                        <div class="form-group-sm">
                                                            <asp:Label runat="server" id="lbl_ddlb_id_obligacion_VIP" class="">Obligación VIP</asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlb_id_obligacion_VIP" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="row" style="padding-top:10px">
                                                    <div class="form-group-sm col-1">
                                                    </div>
                                                    <div class="form-group-sm col-3">
                                                        <asp:Label Text="lblarea_terreno" runat="server" ToolTip="Área terreno (m&sup2;)">Área terreno (m<sup>2</sup>)</asp:Label> 
                                                    </div>
                                                    <div class="form-group-sm col-3">
                                                        <asp:Label Text="lblarea_construida_VIP" runat="server" ToolTip="Área construida (m&sup2;)">Área construida (m<sup>2</sup>)</asp:Label> 
                                                    </div>
                                                    <div class="form-group-sm col-2">
                                                        <div style="width: 80%; float: left;" class="text-truncate">
                                                            <asp:Label Text="lblporc_obligacion_VIP" runat="server" ToolTip="% Obligación">% Obligación</asp:Label>
                                                        </div>                                                        
                                                        <asp:RangeValidator runat="server" ValidationGroup="vgProyectoLicencia" ControlToValidate="txt_porcentaje_obligacion_VIP" MinimumValue="0" MaximumValue="100" Type="Double" Display="Dynamic">
                                                                <uc:ToolTip width="160px" ToolTip="El valor VIP debe estar entre 0 y 100" runat="server"/>
                                                        </asp:RangeValidator>
                                                        <asp:RangeValidator runat="server" ValidationGroup="vgProyectoLicencia" ControlToValidate="txt_porcentaje_obligacion_VIS" MinimumValue="0" MaximumValue="100" Type="Double" Display="Dynamic">
                                                                <uc:ToolTip width="160px" ToolTip="El valor VIS debe estar entre 0 y 100" runat="server"/>
                                                        </asp:RangeValidator>
                                                    </div>
                                                    <div class="form-group-sm col-3">
                                                        <asp:Label Text="lblunidades_vivienda_VIP" runat="server" ToolTip="Unidades">Unidades</asp:Label>
                                                    </div>
                                                </div>
                                                
                                                <div class="row section-sm oneline">
                                                    <div class="form-group-sm col-1">VIP
                                                    </div>
                                                    <div class="form-group-sm col-3 ">
                                                        <asp:TextBox runat="server" ID="txt_area_terreno_VIP" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm col-3 ">
                                                        <asp:TextBox runat="server" ID="txt_area_construida_VIP" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm col-2 ">
                                                        <asp:TextBox runat="server" ID="txt_porcentaje_obligacion_VIP" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm col-3">
                                                        <asp:TextBox runat="server" ID="txt_unidades_vivienda_VIP" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row section-sm oneline">
                                                    <div class="form-group-sm col-1">VIS
                                                    </div>
                                                    <div class="form-group-sm col-3 ">
                                                        <asp:TextBox runat="server" ID="txt_area_terreno_VIS" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm col-3 ">
                                                        <asp:TextBox runat="server" ID="txt_area_construida_VIS" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm col-2 ">
                                                        <asp:TextBox runat="server" ID="txt_porcentaje_obligacion_VIS" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm col-3">
                                                        <asp:TextBox runat="server" ID="txt_unidades_vivienda_VIS" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row section-sm oneline">
                                                    <div class="form-group-sm col-1 ">No VIS
                                                    </div>
                                                    <div class="form-group-sm col-3 ">
                                                        <asp:TextBox runat="server" ID="txt_area_terreno_no_VIS" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm col-3 ">
                                                        <asp:TextBox runat="server" ID="txt_area_construida_no_VIS" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm col-2 "></div>
                                                    <div class="form-group-sm col-3 ">
                                                        <asp:TextBox runat="server" ID="txt_unidades_vivienda_no_VIS" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-lg-3">
                                                <div class="col-12 fwb">
                                                    <caption>Datos usos (m<sup>2</sup>)</caption>
                                                </div>
                                                <div class="row section-sm">
                                                    <div class="col-6 col-sm-3 col-lg-12">
                                                        <asp:Label Text="lblarea_comercio" runat="server" ToolTip="Comercio">Comercio</asp:Label>
                                                        <asp:TextBox runat="server" ID="txt_area_comercio" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-6 col-sm-3 col-lg-12">
                                                        <asp:Label Text="lblarea_oficina" runat="server" ToolTip="Oficina">Oficina</asp:Label>
                                                        <asp:TextBox runat="server" ID="txt_area_oficina" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-6 col-sm-3 col-lg-12">
                                                        <asp:Label Text="lblarea_institucional" runat="server" ToolTip="Institucional">Institucional</asp:Label>
                                                        <asp:TextBox runat="server" ID="txt_area_institucional" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-6 col-sm-3 col-lg-12">
                                                        <asp:Label Text="lblarea_industria" runat="server" ToolTip="Industria">Industria</asp:Label>
                                                        <asp:TextBox runat="server" ID="txt_area_industria" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="col-12 fwb">
                                                    <caption>Datos proyecto (m<sup>2</sup>)</caption>
                                                </div>
                                                <div class="row section-sm">
                                                    <div class="col-6 col-sm-3">
                                                        <asp:Label Text="lblarea_lote" runat="server" ToolTip="Lote">Lote</asp:Label>
                                                        <asp:TextBox runat="server" ID="txt_area_lote" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-6 col-sm-3">
                                                        <asp:Label Text="lblarea_sotano" runat="server" ToolTip="Sótano">Sótano</asp:Label>
                                                        <asp:TextBox runat="server" ID="txt_area_sotano" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-6 col-sm-3">
                                                        <asp:Label Text="lblarea_semisotano" runat="server" ToolTip="Semisótano">Semisótano</asp:Label>                                                        
                                                        <asp:TextBox runat="server" ID="txt_area_semisotano" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-6 col-sm-3">
                                                        <asp:Label Text="lblarea_primer_piso" runat="server" ToolTip="Primer piso">Primer piso</asp:Label>                                                         
                                                        <asp:TextBox runat="server" ID="txt_area_primer_piso" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-6 col-sm-3">
                                                        <asp:Label Text="lblarea_pisos_restantes" runat="server" ToolTip="Primer piso">Pisos restantes</asp:Label>                                                         
                                                        <asp:TextBox runat="server" ID="txt_area_pisos_restantes" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-6 col-sm-3">
                                                        <asp:Label Text="lblarea_construida_total" runat="server" ToolTip="Primer piso">Construida total</asp:Label> 
                                                        <asp:TextBox runat="server" ID="txt_area_construida_total" CssClass="form-control form-control-xs" MaxLength="365" Enabled="true" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-6 col-sm-3">
                                                        <asp:Label Text="lblarea_libre_primer_piso" runat="server" ToolTip="Primer piso">Libre primer piso</asp:Label> 
                                                        <asp:TextBox runat="server" ID="txt_area_libre_primer_piso" CssClass="form-control form-control-xs" MaxLength="10" Enabled="true" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-6 col-sm-3">
                                                        <asp:Label Text="lblporc_ejecucion_construccion" runat="server" ToolTip="% Ejecutado">% Ejecutado </asp:Label>                                                         
                                                        <asp:RangeValidator runat="server" ValidationGroup="vgProyectoLicencia" ControlToValidate="txt_porcentaje_ejecucion_construccion" MinimumValue="0" MaximumValue="100" Type="Double" Display="Dynamic">
                                                            <uc:ToolTip width="160px" ToolTip="El valor debe estar entre 0 y 100" runat="server"/>
                                                        </asp:RangeValidator>
                                                        <div class="input-group input-group-xs">
                                                            <asp:TextBox runat="server" ID="txt_porcentaje_ejecucion_construccion" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                            <div class="input-group-append">
                                                                <span class="input-group-text">%</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </asp:Panel>
                        </div>  
                    </div>
                    <div class="row">
                        <div class="form-group-sm col">
                            <asp:Label runat="server" id="lbl_txt_observacion__licencia" class="">Observaciones</asp:Label>
                            <asp:TextBox runat="server" ID="txt_observacion__licencia" CssClass="form-control form-control-xs"
                                TextMode="MultiLine" onKeyDown="MaxLengthText(this,2000);" onKeyUp="MaxLengthText(this,2000);" MaxLength="2000"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
