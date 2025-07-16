<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProyectoUC.ascx.cs" Inherits="SIDec.UserControls.Acompanamiento.ProyectoUC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>
    <script type="text/javascript" src="./UserControls/Acompanamiento/Acompanamiento.js"></script>

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc1:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc1:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upProyecto" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:HiddenField ID="hddId" runat="server" Value="UserControlProyectoUC" />
        <asp:HiddenField ID="hddViewType" runat="server" Value="0" />
        <asp:HiddenField ID="hddProyectoPrimary" runat="server" Value="0" />

        <%--************************************************** Alert Msg Main **************************************************************--%>
        <asp:UpdatePanel runat="server" ID="upProyectoFoot" UpdateMode="Conditional">
            <ContentTemplate>

                <div id="divData" runat="server">
                    <div class="row">
                        <asp:UpdatePanel ID="upMsgMain" runat="server" UpdateMode="Conditional" class="alert-main msgusercontrol alert-message">
                            <ContentTemplate>
                                <div runat="server" id="msgProyecto" class="alert d-none" role="alert"></div>
                                <div runat="server" id="msgProyectoMain" class="d-none ">
                                    <span runat="server" id="msgMainText"></span>
                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pProyectoExecAction" runat="server">
                            <div class="col-auto card-header-buttons" style="padding-top: .5rem; padding-bottom: .25rem;">
                                <div class="btn-group flex-wrap">
                                    <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgProyectoUC" OnClientClick="obtenerCamposConError('vgProyectoUC')" OnClick="btnAccept_Click">
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
                        <asp:Panel ID="pProyectoAction" runat="server">
                            <div class="col-auto ml-auto card-header-buttons">
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnProyectoList" runat="server" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnAccion_Click">
									    <i class="fas fa-th-list"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnProyectoEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnAccion_Click">
										<i class="fas fa-pencil-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnProyectoDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnAccion_Click">
										<i class="far fa-trash-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnProyectoSeg" runat="server" CssClass="btn" Text="Seguimiento" CausesValidation="false" CommandName="Seguimiento" CommandArgument="4" ToolTip="Ir a Seguimiento" OnClick="btnAccion_Click">
										<i class="fas fa-scroll"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAccept" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoList" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoEdit" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoDel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoSeg" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="gvProyecto" />
            </Triggers>
        </asp:UpdatePanel>


        <div class="card-highlighted">
            <asp:Panel ID="pnlPBProyectoActions" runat="server" Width="99%">
                <div runat="server" id="dvProyectoActions" class="ml0 mtb10 d-i">
                    <asp:Panel ID="pnlProyecto" runat="server">
                        <div runat="server" id="divProyectos" class="">
                            <div class="row first-row ">
                                <div class="col-12 col-sm-6 col-lg-3">
                                    <div class="row">
                                        <div class="col-3">
                                            <div class="form-group-sm col">
                                                <asp:HiddenField ID="hdd_chip" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdd_idBanco" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdd_au_proyecto" runat="server" Value="0" />
                                                <asp:label runat="server" ID="lblauproyecto"  class="">Cód</asp:label>
                                                <asp:TextBox runat="server" ID="txt_au_proyecto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-9">
                                            <div class="form-group-sm col">
                                                <asp:Label runat="server" ID="lblidorigenproyecto" class="">Origen proyecto</asp:Label>
                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectoUC" ControlToValidate="ddl_id_origen_proyecto">
                                                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                </asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" ID="ddl_id_origen_proyecto" CssClass="form-control form-control-xs" AppendDataBoundItems="true" OnSelectedIndexChanged="ddl_id_origen_proyecto_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="col-12 col-sm-6 col-lg-3" id="divPlanParcialFind" runat="server">
                                    <div class="form-group-sm col">
                                        <asp:Label runat="server" ID="lblcodplanp" style="float:left;">Plan Parcial </asp:Label> &nbsp;
                                        <asp:LinkButton ID="btnGoPlanP" runat="server" CssClass="btn btn-outline-warning btn-xs btnLoad" Text="" CausesValidation="false" OnClick="btnGoPlanP_Click" ToolTip="Ir al plan" >
												<i class="fas fa-eye"></i>
                                        </asp:LinkButton>
                                        <asp:DropDownList runat="server" ID="ddl_cod_planp" CssClass="form-control form-control-xs" AppendDataBoundItems="true" OnSelectedIndexChanged="ddl_cod_planp_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-12 col-sm-6 col-lg-3" id="divChipFind" runat="server">
                                    <div class="form-group-sm col">
                                        <asp:label Id="lblchipfind" runat="server" style="float:left;">CHIP</asp:label>
                                        <asp:LinkButton ID="btnGoPredio" runat="server" CssClass="btn btn-outline-warning btn-xs btnLoad" Text="" CausesValidation="false" OnClick="btnGoPredio_Click"  ToolTip="Ir al predio" >
												<i class="fas fa-eye"></i>
                                        </asp:LinkButton>
                                        <asp:TextBox runat="server" ID="txt_chipfind" CssClass="form-control form-control-xs" MaxLength="11" OnTextChanged="txt_chipfind_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <ajax:FilteredTextBoxExtender ID="ftbchipfind" runat="server" FilterType="Numbers,UppercaseLetters,LowercaseLetters" TargetControlID="txt_chipfind" />
                                    </div>
                                </div>
                                <div class="col-12 col-sm-6 col-lg-3" id="divNombreNoPP" runat="server">
                                    <div class="form-group-sm col">
                                        <asp:Label runat="server" ID="lblnombreproyecto" class="">Nombre</asp:Label>
                                        <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectoUC" ControlToValidate="txt_nombre_proyecto">
                                             <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                        </asp:RequiredFieldValidator>
                                        <asp:TextBox runat="server" ID="txt_nombre_proyecto" CssClass="form-control form-control-xs" MaxLength="100"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </asp:Panel>
                    
                    <div class="subcard-body" >
                        <div class="card-header card-header-main text-center">
                            <ul class="nav nav-pills border-bottom-0 mr-auto">
                                <li class="nav-item">
                                    <a  id="tab_1" class="nav-link active" href="#" data-original-title="" title="">Información básica</a>
                                </li>
                                <li class="nav-item">
                                    <a  id="tab_2" class="nav-link" href="#" data-original-title="" title="">Unidades</a>
                                </li>
                                <li class="nav-item">
                                    <a  id="tab_3" class="nav-link" href="#" data-original-title="" title="">Actividades / Zonas</a>
                                </li>
                            </ul>
                        </div>

                        <div class="card-body" runat="server" ID="mvProyectosSection" >
                                <div ID="tab-1">
                                    <div class="row row-cols-1 row-cols-sm-2 row-cols-lg-3 row-cols-xl-4">
                                        <div class="form-group-sm col">
                                            <asp:Label runat="server" ID="lbldireccionproyecto" class="">Dirección</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_direccion_proyecto" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col">
                                            <asp:Label runat="server" ID="lblcodlocalidad" class="">Localidad</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_cod_localidad" CssClass="form-control form-control-xs" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddl_codlocalidad_SelectedIndexChanged">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group-sm col">
                                            <asp:Label runat="server" ID="lblidupz"  class="">UPZ</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_idupz" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group-sm col">
                                            <asp:Label runat="server" ID="lblidtratamientourbanistico" class="">Tratamiento urbanístico</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_id_tratamiento_urbanistico" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group-sm col">
                                            <asp:Label runat="server" ID="lblidclasificacionsuelo" class="">Clasificación suelo</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_id_clasificacion_suelo" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group-sm col">
                                            <asp:Label runat="server" ID="lbliddestinocatastral" class="">Destino catastral</asp:Label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectoUC" ControlToValidate="ddl_id_destino_catastral">
                                                    <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:DropDownList runat="server" ID="ddl_id_destino_catastral" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group-sm col">
                                            <asp:Label runat="server" ID="lblidinstrumentogestion" class="">Instrumento de gestión</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_id_instrumento_gestion" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group-sm col">
                                            <asp:Label runat="server" ID="lblidinstrumentodesarrollo" class="">Instrumento de desarrollo</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_id_instrumento_desarrollo" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group-sm col-12 col-sm-6 col-md-3">
                                            <asp:Label runat="server" ID="lblfechainicioventas" class="">Fecha inicio ventas</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_inicio_ventas" runat="server" ValidationGroup="vgProyectoUC" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_inicio_ventas" Display="Dynamic">
                                                    <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_inicio_ventas" ValidationGroup="vgProyectoUC" ControlToValidate="txt_fecha_inicio_ventas">
                                                    <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_inicio_ventas" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_inicio_ventas" PopupButtonID="txt_fecha_inicio_ventas" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_inicio_ventas" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm  col-12 col-sm-6 col-md-3">
                                            <asp:Label runat="server" ID="lblfechainicioobras" class="">Fecha inicio obras</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_inicio_obras" runat="server" ValidationGroup="vgProyectoUC" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_inicio_obras" Display="Dynamic">
                                                    <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_inicio_obras" ValidationGroup="vgProyectoUC" ControlToValidate="txt_fecha_inicio_obras">
                                                    <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_inicio_obras" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_inicio_obras" PopupButtonID="txt_fecha_inicio_obras" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_inicio_obras" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col">
                                            <asp:Label runat="server" ID="lblidestadoproyecto" class="">Estado proyecto</asp:Label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectoUC" ControlToValidate="ddl_id_estado_proyecto">
                                                    <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:DropDownList runat="server" ID="ddl_id_estado_proyecto" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group-sm col">
                                            <asp:Label runat="server" ID="lblidresultadoproyecto" class="">Resultado</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_id_resultado_proyecto" CssClass="form-control form-control-xs" AppendDataBoundItems="true"
                                                OnSelectedIndexChanged="ddl_id_resultado_proyecto_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group-sm col-12 col-sm-6 col-md-3 col-lg-3">
                                            <asp:Label runat="server" ID="lblempleos" class="">Empleos</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_empleos" CssClass="form-control form-control-xs" MaxLength="10"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-12 col-sm-6 col-md-3 col-lg-3">
                                            <asp:Label runat="server" ID="lblinversion" class="">Inversión (millones)</asp:Label>
                                            <div class="input-group input-group-xs">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">$</span>
                                                </div>
                                                <asp:TextBox runat="server" ID="txt_inversion" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <span class="input-group-text">.000.000</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group-sm col  col-lg-3">
                                            <asp:Label runat="server" ID="lblcodusuresponsable" class="">Usuario responsable</asp:Label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectoUC" ControlToValidate="ddl_cod_usu_responsable">
                                                    <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:DropDownList runat="server" ID="ddl_cod_usu_responsable" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group-sm col-12 col-sm-6  col-lg-3" id="dvDocumentoPA" runat="server">
                                            <asp:Label runat="server" ID="lblPdf" class="lblBasic">Documento de Finalización</asp:Label>
                                            <asp:RequiredFieldValidator ID="rfv_InfoFile" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectoUC" ControlToValidate="txt_ruta_archivo">
                                                    <uc:ToolTip width="130px" ToolTip="Archivo requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <br />
                                            <div style="float: left; min-width: 50px;">
                                                <asp:LinkButton ID="lblPdfProject" runat="server" CssClass="btn btn-danger btn-sm" Text="" CausesValidation="false" OnClick="lblPdfProject_Click" ToolTip="Ver documento">
						                                <i class="fas fa-file-pdf"></i>
                                                </asp:LinkButton>&nbsp;
                                                    <asp:LinkButton ID="lbLoadProject" runat="server" CssClass="btn btn-success btn-sm" Text="" CausesValidation="false" ToolTip="Cargar archivo">
						                                <i class="fas fa-upload"></i>
                                                    </asp:LinkButton>
                                            </div>
                                            <div class="labelLimited" style="padding-left: 10px; height: 20px">
                                                <asp:HiddenField ID="hdd_ruta_archivo" runat="server" />
                                                <asp:Label ID="lblInfoFileProject" runat="server" />
                                                <asp:TextBox ID="txt_ruta_archivo" runat="server" Style="display: none" />
                                                <asp:Label ID="lblErrorFileProject" runat="server" CssClass="error" />
                                                <ajax:AsyncFileUpload ID="fuLoadProject" runat="server" onchange="infoFileProject();" Style="display: none" PersistFile="true"
                                                    CompleteBackColor="Transparent" ErrorBackColor="Red" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row row-cols-1">
                                        <div class="form-group-sm col">
                                            <asp:Label runat="server" ID="lblobservacion" class="">Observaciones</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_observacion" CssClass="form-control form-control-xs" MaxLength="2000"
                                                TextMode="MultiLine" onKeyDown="MaxLengthText(this,2000);" onKeyUp="MaxLengthText(this,2000);"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div ID="tab-2" class="hidden">
                                    <div class="row">
                                        <div runat="server" id="divAreas" class="col-sm-12 col-lg-4">
                                            <div class="row">
                                                <div class="form-group-sm col-6 col-md-3 col-lg-10">
                                                    <asp:Label runat="server" ID="lblareabruta" class="">Área bruta</asp:Label>
                                                    <div class="input-group input-group-xs">
                                                        <asp:TextBox runat="server" ID="txt_area_bruta" CssClass="form-control form-control-xs" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">m<sup>2</sup></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group-sm col-6 col-md-3 col-lg-10">
                                                    <asp:Label runat="server" ID="lblareanetaurbanizable" class="" title="Área neta urbanizable = Área bruta - Área afectaciones">Área neta urb (redesarrollo)</asp:Label>
                                                    <div class="input-group input-group-xs">
                                                        <asp:TextBox runat="server" ID="txt_area_neta_urbanizable" CssClass="form-control form-control-xs" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">m<sup>2</sup></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group-sm col-6 col-md-3 col-lg-10">
                                                    <asp:Label runat="server" ID="lblareautil" class="">Área útil</asp:Label>
                                                    <div class="input-group input-group-xs">
                                                        <asp:TextBox runat="server" ID="txt_area_util" CssClass="form-control form-control-xs" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">m<sup>2</sup></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group-sm col-6 col-md-3 col-lg-10">
                                                    <asp:Label runat="server" ID="lblporcSEtotal" class="">% Ejecutado</asp:Label>
                                                    <asp:RangeValidator runat="server" ValidationGroup="vgProyectoUC" ControlToValidate="txt_porc_SE_total" MinimumValue="0" MaximumValue="100" Type="Double">
                                                          <uc:ToolTip width="160px" ToolTip="El valor debe estar entre 0 y 100" runat="server"/>
                                                    </asp:RangeValidator>
                                                    <div class="input-group input-group-xs">
                                                        <asp:TextBox runat="server" ID="txt_porc_SE_total" CssClass="form-control"></asp:TextBox>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">%</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div runat="server" id="divViviendas" class="col-sm-12 col-lg-8 ">
                                            <asp:Label Id="lblVIviendas" runat="server" class="fwb">Vivienda (unidades)</asp:Label>
                                            <div class="row dvTbb">
                                                <div class="form-group-sm col-2 col-md-1"></div>
                                                <div class="form-group-sm col-10 col-md-11">
                                                    <div class="row dvTbb">
                                                        <div class="form-group-sm col-4 fwb text-center">Potencial</div>
                                                        <div class="form-group-sm col-4 fwb text-center">Ejecutado</div>
                                                        <div class="form-group-sm col-4 fwb text-center">Disponible</div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row dvTbb">
                                                <div class="form-group-sm col-2 col-md-1">
                                                    <asp:Label ID="lblVIP" runat="server" class="">VIP</asp:Label>
                                                    <asp:CompareValidator runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgProyectoUC" Operator="LessThanEqual"
                                                        ControlToValidate="txt_UE_VIP" ControlToCompare="txt_UP_VIP" Type="Currency">
                                                            <uc:ToolTip width="180px" ToolTip="Valor ejecutado inválido, supera el potencial" runat="server"/>
                                                    </asp:CompareValidator>
                                                </div>
                                                <div class="form-group-sm col-10 col-md-11">
                                                    <div class="row dvTbb">
                                                        <div class="form-group-sm col-4">
                                                            <asp:TextBox runat="server" ID="txt_UP_VIP" CssClass="form-control form-control-xs"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm col-4">
                                                            <asp:TextBox runat="server" ID="txt_UE_VIP" CssClass="form-control form-control-xs" Enabled="true"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm col-4">
                                                            <asp:TextBox runat="server" ID="txt_UD_VIP" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row dvTbb">
                                                <div class="form-group-sm col-2 col-md-1">
                                                    <asp:Label ID="lblVIS" runat="server" class="">VIS</asp:Label>
                                                    <asp:CompareValidator runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgProyectoUC"
                                                        Operator="LessThanEqual" ControlToValidate="txt_UE_VIS" ControlToCompare="txt_UP_VIS" Type="Currency">
                                                        <uc:ToolTip width="180px" ToolTip="Valor ejecutado inválido, supera el potencial" runat="server"/>
                                                    </asp:CompareValidator>
                                                </div>
                                                <div class="form-group-sm col-10 col-md-11">
                                                    <div class="row dvTbb">
                                                        <div class="form-group-sm col-4">
                                                            <asp:TextBox runat="server" ID="txt_UP_VIS" CssClass="form-control form-control-xs"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm col-4">
                                                            <asp:TextBox runat="server" ID="txt_UE_VIS" CssClass="form-control form-control-xs" Enabled="true"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm col-4">
                                                            <asp:TextBox runat="server" ID="txt_UD_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row dvTbb">
                                                <div class="form-group-sm col-2 col-md-1">
                                                    <asp:Label ID="lblNoVis" runat="server" class="">No VIS </asp:Label>
                                                    <asp:CompareValidator runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgProyectoUC"
                                                        Operator="LessThanEqual" ControlToValidate="txt_UE_no_VIS" ControlToCompare="txt_UP_no_VIS" Type="Currency">
                                                        <uc:ToolTip width="180px" ToolTip="Valor ejecutado inválido, supera el potencial" runat="server"/>
                                                    </asp:CompareValidator>
                                                </div>
                                                <div class="form-group-sm col-10 col-md-11">
                                                    <div class="row dvTbb">
                                                        <div class="form-group-sm col-4">
                                                            <asp:TextBox runat="server" ID="txt_UP_no_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm col-4">
                                                            <asp:TextBox runat="server" ID="txt_UE_no_VIS" CssClass="form-control form-control-xs" Enabled="true"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm col-4">
                                                            <asp:TextBox runat="server" ID="txt_UD_no_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row dvTbb">
                                                <div class="form-group-sm col-2 col-md-1">
                                                    <asp:Label ID="lblTotal" runat="server" class="fwb">Total </asp:Label>
                                                    <asp:CompareValidator runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgProyectoUC"
                                                        Operator="LessThanEqual" ControlToValidate="txt_UE_no_VIS" ControlToCompare="txt_UP_no_VIS" Type="Currency">
                                                        <uc:ToolTip width="180px" ToolTip="Valor ejecutado inválido, supera el potencial" runat="server"/>
                                                    </asp:CompareValidator>
                                                </div>
                                                <div class="form-group-sm col-10 col-md-11">
                                                    <div class="row dvTbb">
                                                        <div class="form-group-sm col-4">
                                                            <asp:TextBox runat="server" ID="txt_UP_vivienda" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm col-4">
                                                            <asp:TextBox runat="server" ID="txt_UE_vivienda" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm col-4">
                                                            <asp:TextBox runat="server" ID="txt_UD_vivienda" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div ID="tab-3" class="hidden">
                                    <asp:TextBox runat="server" ID="txt_areas_zonas" CssClass="form-control form-control-xs col-md" Visible="false"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txt_areas_zonas_desc" CssClass="form-control form-control-xs col-md" Visible="false"></asp:TextBox>
                                    <div class="row">


                                        <div runat="server" id="divAreasZonas" class="form-group-sm">
                                            <div class="row m-1 p-1">
                                                <div class="form-group-sm col-12 col-sm-6 col-md-4 ">
                                                    <asp:Label runat="server" ID="lblresidencial" class="fwb">Residencial</asp:Label>
                                                    <asp:CheckBox runat="server" ID="chk_az_1" CssClass="d-block" Text="Residencial neta" />
                                                    <asp:CheckBox runat="server" ID="chk_az_2" CssClass="d-block" Text="Residencial con comercio y servicios" />
                                                    <asp:CheckBox runat="server" ID="chk_az_3" CssClass="d-block" Text="Residencial con actividad económica" />

                                                    <asp:Label runat="server" ID="lblareaurbanaintegral" class="fwb mt-1">Área urbana integral</asp:Label>
                                                    <asp:CheckBox runat="server" ID="chk_az_7" CssClass="d-block" Text="Zona residencial" />
                                                    <asp:CheckBox runat="server" ID="chk_az_8" CssClass="d-block" Text="Zona múltiple" />
                                                    <asp:CheckBox runat="server" ID="chk_az_9" CssClass="d-block" Text="Zona de servicios e industria" />

                                                    <asp:Label runat="server" ID="lblindustrial" class="fwb mt-1">Industrial</asp:Label>
                                                    <asp:CheckBox runat="server" ID="chk_az_4" CssClass="d-block" Text="Zona industrial" />
                                                </div>
                                                <div class="form-group-sm col-12 col-sm-6 col-md-4 ">
                                                    <asp:Label runat="server" ID="lblcomercioservicios" class="fwb">Comercio y servicios</asp:Label>
                                                    <asp:CheckBox runat="server" ID="chk_az_15" CssClass="d-block" Text="Zona de servicios empresariales" />
                                                    <asp:CheckBox runat="server" ID="chk_az_16" CssClass="d-block" Text="Zona de servicios empresariales e industriales" />
                                                    <asp:CheckBox runat="server" ID="chk_az_17" CssClass="d-block" Text="Zona especial de servicios" />
                                                    <asp:CheckBox runat="server" ID="chk_az_18" CssClass="d-block" Text="Zona de servicios al automovil" />
                                                    <asp:CheckBox runat="server" ID="chk_az_19" CssClass="d-block" Text="Zona de comercio cualificado" />
                                                    <asp:CheckBox runat="server" ID="chk_az_20" CssClass="d-block" Text="Zona de comercio aglomerado" />
                                                    <asp:CheckBox runat="server" ID="chk_az_21" CssClass="d-block" Text="Zona de comercio pesado" />
                                                    <asp:CheckBox runat="server" ID="chk_az_22" CssClass="d-block" Text="Grandes superficies comerciales" />
                                                    <asp:CheckBox runat="server" ID="chk_az_23" CssClass="d-block" Text="Zona de servicios especiales de alto impacto" />
                                                </div>
                                                <div class="form-group-sm col-12 col-md-4">
                                                    <div class="row">
                                                        <div class="form-group-sm col-12 col-sm-6 col-md-12">
                                                            <asp:Label runat="server" ID="lbldotacional" class="fwb">Dotacional</asp:Label>
                                                            <asp:CheckBox runat="server" ID="chk_az_10" CssClass="d-block" Text="Zona de equipamientos colectivos" />
                                                            <asp:CheckBox runat="server" ID="chk_az_11" CssClass="d-block" Text="Zona de servicios urbanos básicos" />
                                                            <asp:CheckBox runat="server" ID="chk_az_12" CssClass="d-block" Text="Zona de equipamientos recreativos y deportivos" />
                                                            <asp:CheckBox runat="server" ID="chk_az_13" CssClass="d-block" Text="Parques zonales" />
                                                        </div>

                                                        <div class="form-group-sm col-12 col-sm-6 col-md-12">
                                                            <asp:Label runat="server" ID="lblcentral" class="fwb mt-1">Central</asp:Label>
                                                            <asp:CheckBox runat="server" ID="chk_az_5" CssClass="d-block" Text="Central" />

                                                            <asp:Label runat="server" ID="lblsueloprotegido" class="fwb mt-1">Suelo protegido</asp:Label>
                                                            <asp:CheckBox runat="server" ID="chk_az_14" CssClass="d-block" Text="Suelo protegido" />

                                                            <asp:Label runat="server" ID="lblminera" class="fwb mt-1">Minera</asp:Label>
                                                            <asp:CheckBox runat="server" ID="chk_az_6" CssClass="d-block" Text="Zona de recuperación morfológica" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="gv-w">
                        <asp:GridView ID="gvProyecto" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="au_proyecto,cod_usu_responsable,idactor,id_resultado_proyecto" AutoGenerateColumns="false"
                            AllowSorting="true" EmptyDataText="No hay registros para presentar" ShowHeaderWhenEmpty="true" OnSorting="gvProyecto_Sorting"
                            OnRowDataBound="gvProyecto_RowDataBound" OnRowCreated="gv_RowCreated" OnRowCommand="gvProyecto_RowCommand" OnSelectedIndexChanged="gvProyecto_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="au_proyecto" HeaderText="Cód." SortExpression="au_proyecto" />
                                <asp:BoundField DataField="nombre_proyecto" HeaderText="Nombre" SortExpression="nombre_proyecto" />
                                <asp:BoundField DataField="localidad" HeaderText="Localidad" />
                                <asp:BoundField DataField="area_bruta" HeaderText="Área bruta" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                <asp:BoundField DataField="area_neta_urbanizable" HeaderText="Área neta" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                <asp:BoundField DataField="area_util" HeaderText="Área útil" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                <asp:BoundField DataField="porc_SE_total" HeaderText="% Suelo eje." DataFormatString="{0:P0}" ItemStyle-CssClass="t-r w75" SortExpression="porc_SE_total" />
                                <asp:BoundField DataField="completo_PA" HeaderText="completo_PA" SortExpression="completo_PA" Visible="false" />
                                <asp:BoundField DataField="resultado_proyecto" HeaderText="Resultado." SortExpression="resultado_proyecto" ItemStyle-CssClass="w100" />
                                <asp:BoundField DataField="usu_responsable" HeaderText="Usuario responsable" ItemStyle-CssClass="" />
                                <asp:TemplateField ShowHeader="false" ItemStyle-CssClass="t-c w15" HeaderStyle-CssClass="t-c w15">
                                    <ItemTemplate>
                                        <div class="btn-group">
                                            <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn btn-grid-detail" Text="Detalle" CausesValidation="false"
                                                CommandName="_Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Visualizar registro"> 
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
                </div>
            </asp:Panel>
        </div>
        <asp:HiddenField ID="hfGVProyectoSV" runat="server" />
        <asp:HiddenField ID="hfGVProyectoSH" runat="server" />

    </ContentTemplate>
</asp:UpdatePanel>
