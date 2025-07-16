<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="True"
    CodeBehind="Proyectos_old.aspx.cs" Inherits="SIDec.Proyectos" ViewStateMode="Enabled" EnableEventValidation="false" %>

<%--<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/FileUpload.ascx" TagName="FileUpload" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Particular/Actor.ascx" TagName="Actor" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Proyectos/VisitasSitio.ascx" TagPrefix="uc" TagName="VisitaSitio" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
        <ContentTemplate>
            <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
            <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--************************************************** Alert Msg Main **************************************************************--%>
    <asp:UpdatePanel ID="upMsgMain" runat="server" UpdateMode="Conditional" class="alert-main">
        <ContentTemplate>
            <div runat="server" id="msgMain" class="d-none" role="alert">
                <span runat="server" id="msgMainText"></span>
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--******************************************************* Modal ******************************************************************--%>
    <div id="modProyectos" class="modal fade" data-backdrop="static" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header modal-bg-info">
                    <h5 class="modal-title">Confirmar acción</h5>
                </div>
                <div class="modal-body">
                    ¿Está seguro de continuar con la acción solicitada?
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-secondary" data-dismiss="modal" onclick="MsgMain(0);"><i class="fas fa-times"></i>&nbsp&nbspCancelar</button>
                    <asp:LinkButton ID="btnConfirmarProyectos" runat="server" Text="" CssClass="btn btn-outline-primary" OnClick="btnConfirmar_Click" data-dismiss="modal">
							<i class="fas fa-check"></i>&nbsp&nbspAceptar
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>


    <%--**************************************************** gridviews *****************************************************************--%>
    <div id="divData" runat="server">
        <div class="col-12" role="main">
            <%--*************************************************** Proyectos *************************************************************--%>
            <div class="card mt-3 mb-5">
                <div class="card-header card-header-main">
                    <div class="row">
                        <div class="col-sm-6 text-primary">
                            <h4>Proyectos Asociativos</h4>
                        </div>
                        <div class="col-sm-6">
                            <asp:UpdatePanel ID="pnlBuscar" runat="server" DefaultButton="btnBuscar">
                                <ContentTemplate>
                                    <div class="input-group">
                                        <asp:HiddenField ID="hdd_topview" runat="server" Value="1" />
                                        <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control" placeholder="Búsqueda por nombre o número del proyecto asociativo" />
                                        <div class="input-group-append">
                                            <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn btn-outline-primary" CausesValidation="false" OnClick="btnBuscar_Click" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdateProgress ID="pnlLoading" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="pnlBuscar">
                                <ProgressTemplate>
                                    <asp:Image runat="server" CssClass="imgCargando" ImageUrl="~/images/icon/cargando.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                    </div>
                </div>

                <div class="card-body">
                    <asp:UpdatePanel runat="server" ID="upProyectos" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hddProyEstado" runat="server" Value="0" />
                            <asp:MultiView runat="server" ID="mvProyectos" ActiveViewIndex="0">
                                <asp:View runat="server" ID="vProyectos">
                                    <div class="gv-w">
                                        <asp:GridView ID="gvProyectos" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="au_proyecto,cod_usu_responsable,idactor,id_resultado_proyecto" 
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                            AllowSorting="true" OnSorting="gvProyectos_Sorting" OnSelectedIndexChanged="gvProyectos_SelectedIndexChanged" 
                                            OnDataBinding="gvProyectos_DataBinding" OnRowDataBound="gvProyectos_RowDataBound" OnRowCreated="gv_RowCreated">
                                            <Columns>
                                                <asp:BoundField DataField="au_proyecto" HeaderText="Cód. PA" SortExpression="au_proyecto" />
                                                <asp:BoundField DataField="nombre_proyecto" HeaderText="Nombre" SortExpression="nombre_proyecto" />
                                                <asp:TemplateField HeaderText="Es Plan Parcial" ItemStyle-CssClass="t-c">
                                                    <ItemTemplate><%# Convert.ToString(Eval("cod_planp")) != "" ? "Si" : "" %></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="localidad" HeaderText="Localidad" />
                                                <asp:BoundField DataField="origen_proyecto" HeaderText="Origen" SortExpression="origen_proyecto" />
                                                <asp:BoundField DataField="clasificacion_suelo" HeaderText="Clasificación suelo" SortExpression="clasificacion_suelo" />
                                                <asp:BoundField DataField="tratamiento_urbanistico" HeaderText="Tratamiento urbanistico" SortExpression="tratamiento_urbanistico" />
                                                <asp:BoundField DataField="instrumento_gestion" HeaderText="Instrumento de gestión" SortExpression="instrumento_gestion" />
                                                <asp:BoundField DataField="area_bruta" HeaderText="Área bruta (m2)" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                <asp:BoundField DataField="area_neta_urbanizable" HeaderText="Área neta urbanizable (m2)" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                <asp:BoundField DataField="area_util" HeaderText="Área útil (m2)" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                <asp:BoundField DataField="porc_SE_total" HeaderText="% Suelo ejecutado" DataFormatString="{0:P0}" ItemStyle-CssClass="t-r w100" SortExpression="porc_SE_total" />
                                                <asp:BoundField DataField="completo_PA" HeaderText="completo_PA" SortExpression="completo_PA" visible="false"/>
                                                <asp:BoundField DataField="resultado_proyecto" HeaderText="Estado asociativo" SortExpression="resultado_proyecto" />
                                                <asp:BoundField DataField="usu_responsable" HeaderText="Usuario responsable" ItemStyle-CssClass="" />
                                            </Columns>
                                            <SelectedRowStyle CssClass="gvItemSelected" />
                                            <HeaderStyle CssClass="gvHeader"/> 
                                            <RowStyle CssClass="gvItem" />
                                            <PagerStyle CssClass="gvPager" />
                                        </asp:GridView>
                                    </div>
                                </asp:View>
                                <asp:View runat="server" ID="vProyectosDetalle">
                                    <asp:Panel ID="pProyectosDetalle" runat="server">
                                        <div class="div_auditoria"><asp:Label ID="lbl_fec_auditoria_proyecto" runat="server"/></div>
                                        <div runat="server" id="divProyectos" class="">
                                            <div class="row">
                                                <div class="col-12 col-sm-6 col-xl-3">
                                                    <div class="row">
                                                        <div class="col-3">
                                                            <div class="form-group-sm col">
                                                                <label for="txt_au_proyecto" class="">Cód. PA</label>
                                                                <asp:TextBox runat="server" ID="txt_au_proyecto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-9">
                                                            <div class="form-group-sm col">
                                                                <label for="ddl_id_origen_proyecto" class="">Origen proyecto</label>
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectos" ControlToValidate="ddl_id_origen_proyecto">
                                                                    <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                                </asp:RequiredFieldValidator>
                                                                <asp:DropDownList runat="server" ID="ddl_id_origen_proyecto" CssClass="form-control form-control-xs" AppendDataBoundItems="true" OnSelectedIndexChanged="ddl_id_origen_proyecto_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-12 col-sm-6 col-xl-3" id="divPlanParcialFind" runat="server">
                                                    <div class="form-group-sm col">
                                                        <label for="ddl_cod_planp" class="">Plan Parcial</label>
                                                        <asp:LinkButton ID="btnGoPlanP" runat="server" CssClass="btn btn-outline-warning btn-xs btnLoad right" Text="" CausesValidation="false" OnClick="btnGoPlanP_Click"  ToolTip="Ir al plan" >
													            <i class="fas fa-eye"></i>
                                                        </asp:LinkButton>
                                                        <asp:DropDownList runat="server" ID="ddl_cod_planp" CssClass="form-control form-control-xs" AppendDataBoundItems="true" OnSelectedIndexChanged="ddl_cod_planp_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-12 col-sm-6 col-xl-3" id="divChipFind" runat="server">
                                                    <div class="form-group-sm col">
                                                        <asp:HiddenField ID="hdd_chip" runat="server" Value="" />
                                                        <label for="txt_chipfind" class="">CHIP</label>
                                                        <asp:LinkButton ID="btnGoPredio" runat="server" CssClass="btn btn-outline-warning btn-xs btnLoad right" Text="" CausesValidation="false" OnClick="btnGoPredio_Click"  ToolTip="Ir al predio" >
													            <i class="fas fa-eye"></i> 
                                                        </asp:LinkButton>
                                                        <asp:TextBox runat="server" ID="txt_chipfind" CssClass="form-control form-control-xs" MaxLength="11" OnTextChanged="txt_chipfind_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="ftbchipfind" runat="server" FilterType="Numbers,UppercaseLetters,LowercaseLetters" TargetControlID="txt_chipfind"/>
                                                    </div>
                                                </div>
                                                <div class="col-12 col-sm-6 col-xl-3" id="divNombreNoPP" runat="server">
                                                    <div class="form-group-sm col">
                                                        <label for="txt_nombre_proyecto" class="">Nombre</label>
                                                        <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectos" ControlToValidate="txt_nombre_proyecto">
                                                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                        </asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" ID="txt_nombre_proyecto" CssClass="form-control form-control-xs" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="row row-cols-1 row-cols-sm-2 row-cols-lg-3 row-cols-xl-4">
                                            <div class="form-group-sm col">
                                                <label for="txt_direccion_proyecto" class="">Dirección</label>
                                                <asp:TextBox runat="server" ID="txt_direccion_proyecto" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                            </div>
                                            <div class="form-group-sm col">
                                                <label for="ddl_cod_localidad" class="">Localidad</label>
                                                <asp:DropDownList runat="server" ID="ddl_cod_localidad" CssClass="form-control form-control-xs" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddl_codlocalidad_SelectedIndexChanged">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group-sm col">
                                                <label for="ddl_idupz" class="">UPZ</label>
                                                <asp:DropDownList runat="server" ID="ddl_idupz" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group-sm col">
                                                <label for="ddl_id_tratamiento_urbanistico" class="">Tratamiento urbanístico</label>
                                                <asp:DropDownList runat="server" ID="ddl_id_tratamiento_urbanistico" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group-sm col">
                                                <label for="ddl_id_clasificacion_suelo" class="">Clasificación suelo</label>
                                                <asp:DropDownList runat="server" ID="ddl_id_clasificacion_suelo" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group-sm col">
                                                <label for="ddl_id_destino_catastral" class="">Destino catastral</label>
                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectos" ControlToValidate="ddl_id_destino_catastral">
                                                    <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                </asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" ID="ddl_id_destino_catastral" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="form-group-sm col">
                                                <label for="ddl_id_instrumento_gestion" class="">Instrumento de gestión</label>
                                                <asp:DropDownList runat="server" ID="ddl_id_instrumento_gestion" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="form-group-sm col">
                                                <label for="ddl_id_instrumento_desarrollo" class="">Instrumento de desarrollo</label>
                                                <asp:DropDownList runat="server" ID="ddl_id_instrumento_desarrollo" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="form-group-sm col-12 col-sm-6 col-md-3">
                                                <label for="txt_fecha_inicio_ventas" class="">Fecha inicio ventas</label>
                                                <asp:RegularExpressionValidator ID="rev_fecha_inicio_ventas" runat="server" ValidationGroup="vgProyectos" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_inicio_ventas" Display="Dynamic">
                                                    <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                                </asp:RegularExpressionValidator>
                                                <asp:RangeValidator runat="server" ID="rv_fecha_inicio_ventas" ValidationGroup="vgProyectos" ControlToValidate="txt_fecha_inicio_ventas">
                                                    <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                                </asp:RangeValidator>
                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_inicio_ventas" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_inicio_ventas" PopupButtonID="txt_fecha_inicio_ventas" Format="yyyy-MM-dd" />
                                                <asp:TextBox runat="server" ID="txt_fecha_inicio_ventas" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                            </div>

                                            <div class="form-group-sm  col-12 col-sm-6 col-md-3">
                                                <label for="txt_fecha_inicio_obras" class="">Fecha inicio obras</label>
                                                <asp:RegularExpressionValidator ID="rev_fecha_inicio_obras" runat="server" ValidationGroup="vgProyectos" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_inicio_obras" Display="Dynamic">
                                                    <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                                </asp:RegularExpressionValidator>
                                                <asp:RangeValidator runat="server" ID="rv_fecha_inicio_obras" ValidationGroup="vgProyectos" ControlToValidate="txt_fecha_inicio_obras">
                                                    <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                                </asp:RangeValidator>
                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_inicio_obras" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_inicio_obras" PopupButtonID="txt_fecha_inicio_obras" Format="yyyy-MM-dd" />
                                                <asp:TextBox runat="server" ID="txt_fecha_inicio_obras" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                            </div>

                                            <div class="form-group-sm col">
                                                <label for="ddl_id_estado_proyecto" class="">Estado proyecto</label>
                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectos" ControlToValidate="ddl_id_estado_proyecto">
                                                    <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                </asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" ID="ddl_id_estado_proyecto" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="form-group-sm col">
                                                <label for="ddl_id_resultado_proyecto" class="">Estado del asociativo </label>
                                                <asp:DropDownList runat="server" ID="ddl_id_resultado_proyecto" CssClass="form-control form-control-xs" AppendDataBoundItems="true" 
                                                    OnSelectedIndexChanged="ddl_id_resultado_proyecto_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="form-group-sm col-12 col-sm-6 col-md-3 col-lg-3">
                                                <label for="txt_empleos" class="">Empleos</label>
                                                <asp:TextBox runat="server" ID="txt_empleos" CssClass="form-control form-control-xs" MaxLength="10"></asp:TextBox>
                                            </div>

                                            <div class="form-group-sm col-12 col-sm-6 col-md-3 col-lg-3">
                                                <label for="txt_inversion" class="">Inversión (millones)</label>
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

                                            <div class="form-group-sm col col-lg-3">
                                                <label for="ddl_cod_usu_responsable" class="">Usuario responsable</label>
                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectos" ControlToValidate="ddl_cod_usu_responsable">
                                                    <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                </asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" ID="ddl_cod_usu_responsable" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="form-group-sm col-12 col-sm-6 col-lg-3" id="dvDocumentoPA" runat="server">
                                                <label class="lblBasic">Documento de Finalización</label>
                                                <asp:RequiredFieldValidator ID="rfv_InfoFile" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectos" ControlToValidate="txt_ruta_archivo">
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
                                                    <ajax:asyncfileupload id="fuLoadProject" runat="server" onchange="infoFileProject();" style="display: none" PersistFile="true" 
                                                        CompleteBackColor="Transparent" ErrorBackColor="Red" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" id="pnlPrueba" runat="server" Visible="false">
                                            <div class="form-group-sm col-12 col-sm-6 col-md-3">
                                                <label class="">Cómo? </label>
                                                <asp:DropDownList runat="server" ID="DropDownList1" CssClass="form-control form-control-xs" AppendDataBoundItems="true" >
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group-sm  col-12 col-sm-6 col-md-3">
                                                <label for="txt_fecha_inicio_obras" class="">Cuándo?</label>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_inicio_obras" PopupButtonID="txt_fecha_inicio_obras" Format="yyyy-MM-dd" />
                                                <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-3">
                                                <label for="txt_area_bruta" class="">Área?</label>
                                                <div class="input-group input-group-xs">
                                                    <asp:TextBox runat="server" ID="TextBox2" CssClass="form-control form-control-xs" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                    <div class="input-group-append">
                                                        <span class="input-group-text">m<sup>2</sup></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group-sm col">
                                                <label for="txt_observacion" class="">Observaciones</label>
                                                <asp:TextBox runat="server" ID="txt_observacion" CssClass="form-control form-control-xs" MaxLength="2000" 
                                                    TextMode="MultiLine"  onKeyDown="MaxLengthText(this,2000);" onKeyUp="MaxLengthText(this,2000);" ></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="accordionProy">
                                            <div class="card">
                                                <div class="card-header card-header-dark">
                                                    <button type="button" class="noBtn " id="heading1" data-toggle="collapse" data-target="#collapse1" aria-expanded="true" aria-controls="collapse1">
                                                        Unidades / Actividades / Zonas
                                                    </button>
                                                </div>
                                                <div id="collapse1" class="collapse" aria-labelledby="heading1" data-parent="#accordionProy">
                                                    <div class="card-body">

                                                        <div runat="server" id="divAreas" class="row row-cols-1 row-cols-sm-2 row-cols-md-4">
                                                            <div class="form-group-sm col">
                                                                <label for="txt_area_bruta" class="">Área bruta</label>
                                                                <div class="input-group input-group-xs">
                                                                    <asp:TextBox runat="server" ID="txt_area_bruta" CssClass="form-control form-control-xs" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                    <div class="input-group-append">
                                                                        <span class="input-group-text">m<sup>2</sup></span>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="form-group-sm col">
                                                                <label for="txt_area_neta_urbanizable" class="" title="Área neta urbanizable = Área bruta - Área afectaciones">Área neta urb (redesarrollo)</label>
                                                                <div class="input-group input-group-xs">
                                                                    <asp:TextBox runat="server" ID="txt_area_neta_urbanizable" CssClass="form-control form-control-xs" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                    <div class="input-group-append">
                                                                        <span class="input-group-text">m<sup>2</sup></span>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="form-group-sm col">
                                                                <label for="txt_area_util" class="">Área útil</label>
                                                                <div class="input-group input-group-xs">
                                                                    <asp:TextBox runat="server" ID="txt_area_util" CssClass="form-control form-control-xs" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                    <div class="input-group-append">
                                                                        <span class="input-group-text">m<sup>2</sup></span>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="form-group-sm col">
                                                                <label for="txt_porcentaje_SE_total" class="">% Ejecutado</label>
                                                                <asp:RangeValidator runat="server" ValidationGroup="vgProyectos" ControlToValidate="txt_porcentaje_SE_total" MinimumValue="0" MaximumValue="100" Type="Double" >
                                                                    <uc:ToolTip width="160px" ToolTip="El valor debe estar entre 0 y 100" runat="server"/>
                                                                </asp:RangeValidator>
                                                                <div class="input-group input-group-xs">
                                                                    <asp:TextBox runat="server" ID="txt_porcentaje_SE_total" CssClass="form-control"></asp:TextBox>
                                                                    <div class="input-group-append">
                                                                        <span class="input-group-text">%</span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div runat="server" id="divViviendas" class="col-sm-12 col-lg-4 col-xl-3">
                                                                <label class="">Vivienda (unidades)</label>
                                                                <div class="row">
                                                                    <div class="col-12">
                                                                        <table class="table table-sm">
                                                                            <thead>
                                                                                <tr>
                                                                                    <td class="w-15"></td>
                                                                                    <th class="text-center" title="">Potencial</th>
                                                                                    <th class="text-center" title="">Ejecutado</th>
                                                                                    <th class="text-center w-20" title="">Disponible</th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td>VIP                                                                                        
                                                                                        <asp:CompareValidator runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgProyectos" Operator="LessThanEqual" ControlToValidate="txt_UE_VIP" ControlToCompare="txt_UP_VIP" Type="Currency" >
                                                                                                <uc:ToolTip width="180px" ToolTip="Valor ejecutado inválido, supera el potencial" runat="server"/>
                                                                                        </asp:CompareValidator>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txt_UP_VIP" CssClass="form-control form-control-xs"></asp:TextBox></td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txt_UE_VIP" CssClass="form-control form-control-xs" Enabled="true"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txt_UD_VIP" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>VIS                                                           
                                                                                        <asp:CompareValidator runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgProyectos" Operator="LessThanEqual" ControlToValidate="txt_UE_VIS" ControlToCompare="txt_UP_VIS" Type="Currency">
                                                                                            <uc:ToolTip width="180px" ToolTip="Valor ejecutado inválido, supera el potencial" runat="server"/>
                                                                                        </asp:CompareValidator></td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txt_UP_VIS" CssClass="form-control form-control-xs"></asp:TextBox></td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txt_UE_VIS" CssClass="form-control form-control-xs" Enabled="true"></asp:TextBox></td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txt_UD_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>No VIS                                                                                      
                                                                                        <asp:CompareValidator runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgProyectos" Operator="LessThanEqual" ControlToValidate="txt_UE_no_VIS" ControlToCompare="txt_UP_no_VIS" Type="Currency">
                                                                                            <uc:ToolTip width="180px" ToolTip="Valor ejecutado inválido, supera el potencial" runat="server"/>
                                                                                        </asp:CompareValidator></td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txt_UP_no_VIS" CssClass="form-control form-control-xs"></asp:TextBox></td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txt_UE_no_VIS" CssClass="form-control form-control-xs" Enabled="true"></asp:TextBox></td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txt_UD_no_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>Total</th>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txt_UP_vivienda" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txt_UE_vivienda" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txt_UD_vivienda" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <asp:TextBox runat="server" ID="txt_areas_zonas" CssClass="form-control form-control-xs col-md" Visible="false"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txt_areas_zonas_desc" CssClass="form-control form-control-xs col-md" Visible="false"></asp:TextBox>

                                                            <div runat="server" id="divAreasZonas" class="form-group-sm col-lg-8 col-xl-9">
                                                                <label class="">Áreas de actividad / Zonas</label>
                                                                <div class="row bgb m-1 p-1 rounded">
                                                                    <div class="form-group-sm col-sm">
                                                                        <label class="fwb">Residencial</label>
                                                                        <asp:CheckBox runat="server" ID="chk_az_1" CssClass="d-block" Text="Residencial neta" />
                                                                        <asp:CheckBox runat="server" ID="chk_az_2" CssClass="d-block" Text="Residencial con comercio y servicios" />
                                                                        <asp:CheckBox runat="server" ID="chk_az_3" CssClass="d-block" Text="Residencial con actividad económica" />

                                                                        <label class="fwb mt-1">Área urbana integral</label>
                                                                        <asp:CheckBox runat="server" ID="chk_az_7" CssClass="d-block" Text="Zona residencial" />
                                                                        <asp:CheckBox runat="server" ID="chk_az_8" CssClass="d-block" Text="Zona múltiple" />
                                                                        <asp:CheckBox runat="server" ID="chk_az_9" CssClass="d-block" Text="Zona de servicios e industria" />

                                                                        <label class="fwb mt-1">Industrial</label>
                                                                        <asp:CheckBox runat="server" ID="chk_az_4" CssClass="d-block" Text="Zona industrial" />
                                                                    </div>
                                                                    <div class="form-group-sm col-sm">
                                                                        <label class="fwb">Comercio y servicios</label>
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
                                                                    <div class="form-group-sm col-sm">
                                                                        <label class="fwb">Dotacional</label>
                                                                        <asp:CheckBox runat="server" ID="chk_az_10" CssClass="d-block" Text="Zona de equipamientos colectivos" />
                                                                        <asp:CheckBox runat="server" ID="chk_az_11" CssClass="d-block" Text="Zona de servicios urbanos básicos" />
                                                                        <asp:CheckBox runat="server" ID="chk_az_12" CssClass="d-block" Text="Zona de equipamientos recreativos y deportivos" />
                                                                        <asp:CheckBox runat="server" ID="chk_az_13" CssClass="d-block" Text="Parques zonales" />

                                                                        <label class="fwb mt-1">Central</label>
                                                                        <asp:CheckBox runat="server" ID="chk_az_5" CssClass="d-block" Text="Central" />

                                                                        <label class="fwb mt-1">Suelo protegido</label>
                                                                        <asp:CheckBox runat="server" ID="chk_az_14" CssClass="d-block" Text="Suelo protegido" />

                                                                        <label class="fwb mt-1">Minera</label>
                                                                        <asp:CheckBox runat="server" ID="chk_az_6" CssClass="d-block" Text="Zona de recuperación morfológica" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </asp:View>
                            </asp:MultiView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosVG" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosVD" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosNavFirst" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosNavBack" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosNavNext" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosNavLast" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosEdit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosAdd" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosDel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="gvProyectos" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <div class="card-footer">
                    <asp:UpdatePanel runat="server" ID="upProyectosFoot" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:UpdatePanel runat="server" ID="upProyectosMsg" UpdateMode="Conditional" class="alert-card">
                                <ContentTemplate>
                                    <div runat="server" id="msgProyectos" class="alert d-none" role="alert"></div>
                                    <asp:ValidationSummary runat="server" ID="vsProyectos" DisplayMode="SingleParagraph" CssClass="invalid-feedback" HeaderText="Falta informar: " ShowSummary="true" ShowValidationErrors="true" ValidationGroup="vgProyectos" />
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row">

                                <div class="col-auto va-m mr-auto">
                                    <asp:Panel ID="pProyectosView" runat="server">
                                        <div class="btn-group" role="group">
                                            <asp:LinkButton ID="btnProyectosVG" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnProyectosVista_Click">
                        <i class="fas fa-border-all"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProyectosVD" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnProyectosVista_Click">
                        <i class="fas fa-bars"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>

                                <div class="col-auto va-m mr-auto">
                                    <asp:Label ID="lblProyectosCuenta" runat="server"></asp:Label>
                                </div>

                                <div class="col-auto t-c">
                                    <asp:Panel ID="pProyectosNavegacion" runat="server">
                                        <div class="btn-group">
                                            <asp:LinkButton ID="btnProyectosNavFirst" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnProyectosNavegacion_Click">
											  <i class="fas fa-angle-double-left"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProyectosNavBack" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnProyectosNavegacion_Click">
											  <i class="fas fa-angle-left"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProyectosNavNext" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnProyectosNavegacion_Click">
											  <i class="fas fa-angle-right"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProyectosNavLast" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnProyectosNavegacion_Click">
											  <i class="fas fa-angle-double-right"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>

                                <div class="col-auto ml-auto">
                                    <asp:Panel ID="pProyectosAction" runat="server">
                                        <div class="btn-group">
                                            <asp:LinkButton ID="btnProyectosAdd" runat="server" disabled="" CssClass="btn btn-outline-danger" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnProyectosAccion_Click">
										    <i class="fas fa-plus"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProyectosEdit" runat="server" CssClass="btn btn-outline-danger" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnProyectosAccion_Click">
										    <i class="fas fa-pencil-alt"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProyectosDel" runat="server" CssClass="btn btn-outline-danger" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnProyectosAccion_Click">
										    <i class="far fa-trash-alt"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>

                                <div class="col-auto">
                                    <asp:Panel ID="pProyectosExecAction" runat="server">
                                        <div class="btn-group flex-wrap">
                                            <asp:LinkButton ID="btnProyectosCancelar" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" OnClick="btnProyectosCancelar_Click" CausesValidation="false">
										    <i class="fas fa-times"></i>&nbspCancelar
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProyectosAccionFinal" runat="server" CssClass="btn btn-outline-primary" ValidationGroup="vgProyectos" OnClick="btnProyectosAccionFinal_Click">
                                                <%--OnClientClick="if(Page_ClientValidate('vgProyectos')) return openModal('modProyectos');">--%>
										    <i class="fas fa-check"></i>&nbspAceptar
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosVG" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosVD" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosAdd" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosEdit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyectosDel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%------------------------- Tabs -------------------------%>		
        <div class="col-12">
            <asp:UpdatePanel runat="server" ID="upProyectosSection" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card mt-3 mb-5">
                        <div class="card-header card-header-main text-center">
                            <ul runat="server" id="ulProyectosSection" class="nav nav-pills border-bottom-0 mr-auto">
                                <li class="nav-item">
                                    <asp:LinkButton ID="lbProyectosSection_0" runat="server" CssClass="nav-link active" CommandArgument="0" CausesValidation="false" OnClick="btnProyectosSection_Click">Predios</asp:LinkButton>
                                </li>
                                <li class="nav-item">
                                    <asp:LinkButton ID="lbProyectosSection_1" runat="server" CssClass="nav-link" CommandArgument="1" CausesValidation="false" OnClick="btnProyectosSection_Click">Cartas de intención</asp:LinkButton>
                                </li>
                                <li class="nav-item">
                                    <asp:LinkButton ID="lbProyectosSection_2" runat="server" CssClass="nav-link" CommandArgument="2" CausesValidation="false" OnClick="btnProyectosSection_Click">Licencias</asp:LinkButton>
                                </li>
                                <li class="nav-item">
                                    <asp:LinkButton ID="lbProyectosSection_3" runat="server" CssClass="nav-link" CommandArgument="3" CausesValidation="false" OnClick="btnProyectosSection_Click">Responsables</asp:LinkButton>
                                </li>
                                <li class="nav-item">
                                    <asp:LinkButton ID="lbProyectosSection_4" runat="server" CssClass="nav-link" CommandArgument="4" CausesValidation="false" OnClick="btnProyectosSection_Click">Visitas</asp:LinkButton>
                                </li>
                                <li class="nav-item">
                                    <asp:LinkButton ID="lbProyectosSection_5" runat="server" CssClass="nav-link" CommandArgument="5" CausesValidation="false" OnClick="btnProyectosSection_Click">Seguimiento</asp:LinkButton>
                                </li>
                                <li class="nav-item ml-auto va-m">
                                    <h5>
                                        <asp:Label ID="lblProyectosSection" runat="server" CausesValidation="false" Text="" CssClass="titleSectionProyecto"></asp:Label>
                                    </h5>
                                </li>
                            </ul>
                        </div>

                        <div>
                            <asp:MultiView ID="mvProyectosSection" runat="server" ActiveViewIndex="0">
                                <%--********************************************************************--%>
                                <%--*************** Predios ********************************************--%>
                                <asp:View ID="View1" runat="server">
                                    <div class="card-body">
                                        <asp:UpdatePanel runat="server" ID="upProyectosPredios" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:MultiView runat="server" ID="mvProyectosPredios" ActiveViewIndex="0">
                                                    <asp:View runat="server" ID="vProyectosPredios">
                                                        <div class="gv-w">
                                                            <asp:GridView ID="gvProyectosPredios" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="au_proyecto_predio,chip" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                                AllowSorting="true" OnSorting="gvPredios_Sorting" OnSelectedIndexChanged="gvPredios_SelectedIndexChanged" OnRowCommand="gvPredios_RowCommand"
                                                                OnRowDataBound="gvPredios_RowDataBound" OnRowCreated="gv_RowCreated">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_proyecto_predio" HeaderText="au_proyecto_predio" Visible="false" />

                                                                    <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c gridcolmin_120" HeaderText="CHIP">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnView" runat="server" CssClass="btn btn-outline-warning btn-xs" CausesValidation="false" CommandName="_Go"
                                                                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Ir al predio" Visible='<%# (String.IsNullOrEmpty(Eval("declaratoria").ToString())) ? false : true %>'> 
										                                        <i class="fas fa-eye"></i>&nbsp;<%# Convert.ToString(Eval("chip"))%>
                                                                            </asp:LinkButton>
                                                                            <asp:Label ID="lblChip" runat="server" CssClass="t-c" Visible='<%# (String.IsNullOrEmpty(Eval("declaratoria").ToString())) ? true : false %>'> 
										                                        <%# Convert.ToString(Eval("chip"))%>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="matricula" HeaderText="Matrícula Inm." />
                                                                    <asp:TemplateField HeaderText="Es predio declarado" ItemStyle-CssClass="t-c">
                                                                        <ItemTemplate><%# Convert.ToString(Eval("declaratoria")) != "" ? "Si" : "No" %></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Cumple función social" ItemStyle-CssClass="t-c">
                                                                        <ItemTemplate><%# Convert.ToString(Eval("cumple_funcion_social")) != "0" ? "Si" : "No" %></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="declaratoria" HeaderText="Declaratoria" />
                                                                    <asp:BoundField DataField="tipo_declaratoria" HeaderText="Tipo declaratoria" />
                                                                    <asp:BoundField DataField="estado_predio_declarado" HeaderText="Estado predio" />
                                                                    <asp:BoundField DataField="causal_acto" HeaderText="Causal acto" />
                                                                    <asp:BoundField DataField="numero_ultimo_acto" HeaderText="Número último acto" />
                                                                    <asp:BoundField DataField="fecha_ultimo_acto" HeaderText="Fecha último acto" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-CssClass="t-c w120" />
                                                                    <asp:BoundField DataField="estado_ultimo_acto" HeaderText="Estado predio último acto" />
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:View>
                                                    <asp:View runat="server" ID="vProyectosPrediosDetalle">
                                                        <div runat="server" id="divProyectosPredios" class="">
                                                            <div class="div_auditoria"><asp:Label ID="lbl_fec_auditoria_predios" runat="server"/></div>
                                                            <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4">
                                                                <asp:TextBox runat="server" ID="txt_au_proyecto_predio" Enabled="false" Visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" ID="txt_cod_proyecto__predio" Enabled="false" Visible="false"></asp:TextBox>
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_chip" class="">CHIP <i class="fas fa-search"></i></label>
                                                                    <asp:LinkButton ID="btnGoPredio2" runat="server" CssClass="btn btn-outline-warning btn-xs btnLoad right" Text="" CausesValidation="false" OnClick="btnGoPredio_Click"  ToolTip="Ir al predio" >
													                    <i class="fas fa-eye"></i> 
                                                                    </asp:LinkButton>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectosPredios" ControlToValidate="txt_chip">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:TextBox runat="server" ID="txt_chip" CssClass="form-control form-control-xs" OnTextChanged="txt_chip_TextChanged" MaxLength="11" AutoPostBack="true"></asp:TextBox>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbchips" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Numbers" TargetControlID="txt_chip"/>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_matricula" class="">Matrícula inmobiliaria</label>
                                                                    <asp:TextBox runat="server" ID="txt_matricula" CssClass="form-control form-control-xs" placeholder="50X-00000000" MaxLength="12"></asp:TextBox>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbmatricula" runat="server" FilterType="Custom,Numbers" ValidChars="csnCSN-" FilterMode="ValidChars" TargetControlID="txt_matricula"/>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_direccion" class="">Dirección</label>
                                                                    <asp:TextBox runat="server" ID="txt_direccion" CssClass="form-control form-control-xs" MaxLength="100"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_declaratoria" class="">Declaratoria</label>
                                                                    <asp:TextBox runat="server" ID="txt_declaratoria" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col va-m">
                                                                    <asp:CheckBox runat="server" ID="chk_cumple_funcion_social" CssClass="" Text="Cumplió función social"/>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_tipo_declaratoria" class="">Tipo declaratoria</label>
                                                                    <asp:TextBox runat="server" ID="txt_tipo_declaratoria" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_estado_predio_declarado" class="">Estado predio</label>
                                                                    <asp:TextBox runat="server" ID="txt_estado_predio_declarado" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_causal_acto" class="">Causal acto</label>
                                                                    <asp:TextBox runat="server" ID="txt_causal_acto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_numero_ultimo_acto" class="">Número último acto</label>
                                                                    <asp:TextBox runat="server" ID="txt_numero_ultimo_acto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_fecha_ultimo_acto" class="">Fecha último acto</label>
                                                                    <asp:TextBox runat="server" ID="txt_fecha_ultimo_acto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_fecha_ejecutoria_ultimo_acto" class="">Fecha ejecutoria último acto</label>
                                                                    <asp:TextBox runat="server" ID="txt_fecha_ejecutoria_ultimo_acto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_estado_ultimo_acto" class="">Estado último acto</label>
                                                                    <asp:TextBox runat="server" ID="txt_estado_ultimo_acto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_fecha_vencimiento_ultimo_acto" class="">Fecha vencimiento último acto</label>
                                                                    <asp:TextBox runat="server" ID="txt_fecha_vencimiento_ultimo_acto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_observacion__predio" class="">Observaciones</label>
                                                                    <asp:TextBox runat="server" ID="txt_observacion__predio" CssClass="form-control form-control-xs" 
                                                                        TextMode="MultiLine" MaxLength="2000"  onKeyDown="MaxLengthText(this,2000);" onKeyUp="MaxLengthText(this,2000);" ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosNavFirst" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosNavBack" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosNavNext" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosNavLast" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvProyectosPredios" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="card-footer">
                                        <asp:UpdatePanel runat="server" ID="upProyectosPrediosFoot" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:UpdatePanel runat="server" ID="upProyectosPrediosMsg" UpdateMode="Conditional" class="alert-card">
                                                    <ContentTemplate>
                                                        <div runat="server" id="msgProyectosPredios" class="alert d-none" role="alert"></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <div class="row">
                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Panel ID="pProyectosPrediosView" runat="server">
                                                            <div class="btn-group" role="group">
                                                                <asp:LinkButton ID="btnProyectosPrediosVG" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnPrediosVista_Click">
                                  <i class="fas fa-border-all"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosPrediosVD" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnPrediosVista_Click">
                                  <i class="fas fa-bars"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Label ID="lblProyectosPrediosCuenta" runat="server"></asp:Label>
                                                    </div>

                                                    <div class="col-auto t-c">
                                                        <asp:Panel ID="pProyectosPrediosNavegacion" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnProyectosPrediosNavFirst" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnPrediosNavegacion_Click">
											            <i class="fas fa-angle-double-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosPrediosNavBack" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnPrediosNavegacion_Click">
											            <i class="fas fa-angle-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosPrediosNavNext" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnPrediosNavegacion_Click">
											            <i class="fas fa-angle-right"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosPrediosNavLast" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnPrediosNavegacion_Click">
											            <i class="fas fa-angle-double-right"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto ml-auto">
                                                        <asp:Panel ID="pProyectosPrediosAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnProyectosPrediosAdd" runat="server" disabled="" CssClass="btn btn-outline-danger" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnPrediosAccion_Click">
										              <i class="fas fa-plus"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosPrediosEdit" runat="server" CssClass="btn btn-outline-danger" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnPrediosAccion_Click">
										              <i class="fas fa-pencil-alt"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosPrediosDel" runat="server" CssClass="btn btn-outline-danger" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnPrediosAccion_Click"> 
										              <i class="far fa-trash-alt"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto">
                                                        <asp:Panel ID="pProyectosPrediosExecAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnProyectosPrediosCancelar" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" OnClick="btnPrediosCancelar_Click" CausesValidation="false">
										              <i class="fas fa-times"></i>&nbspCancelar
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosPrediosAccionFinal" runat="server" CssClass="btn btn-outline-primary" ValidationGroup="vgProyectosPredios" OnClientClick="if(Page_ClientValidate('vgProyectosPredios')) return openModal('modProyectos');">
										              <i class="fas fa-check"></i>&nbspAceptar
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosPrediosDel" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:View>

                                <%--********************************************************************--%>
                                <%--*************** Cartas *********************************************--%>
                                <asp:View ID="View2" runat="server">
                                    <div class="card-body">
                                        <asp:UpdatePanel runat="server" ID="upProyectosCartas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:MultiView runat="server" ID="mvProyectosCartas" ActiveViewIndex="0">
                                                    <asp:View runat="server" ID="vProyectosCartas">
                                                        <div class="gv-w">
                                                            <asp:GridView ID="gvProyectosCartas" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" 
                                                                DataKeyNames="au_proyecto_carta,carta_intencion_firmada,meses_desarrollo,fecha_firma,ruta_carta" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                                AllowSorting="true" OnSorting="gvCartas_Sorting" OnSelectedIndexChanged="gvCartas_SelectedIndexChanged"
                                                                OnRowDataBound="gvCartas_RowDataBound" OnRowCreated="gv_RowCreated" OnRowCommand="gvCartas_RowCommand">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_proyecto_carta" HeaderText="au_proyecto_carta" Visible="false" />
                                                                    <asp:BoundField DataField="radicado_manifestacion_interes" HeaderText="Radicado manifestación interés" />
                                                                    <asp:BoundField DataField="fecha_radicado_manifestacion_interes" HeaderText="Fecha radicado manifestación interés" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-CssClass="t-c w120" />
                                                                    <asp:BoundField DataField="radicado_carta_intencion" HeaderText="Radicado carta interés" />
                                                                    <asp:BoundField DataField="fecha_radicado_carta_intencion" HeaderText="Fecha radicado carta intención" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-CssClass="t-c w120" />
                                                                    <asp:BoundField DataField="documento_constitucion_proyecto" HeaderText="Documento constitución proyecto" />
                                                                    <asp:TemplateField HeaderText="Carta intención firmada" ItemStyle-CssClass="t-c w100">
                                                                        <ItemTemplate><%# Convert.ToString(Eval("carta_intencion_firmada")) == "1" ? "Si" : "" %></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="fecha_firma" HeaderText="Fecha firma" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-CssClass="t-c w120" />
                                                                    <asp:TemplateField HeaderText="Tiempo de desarrollo en meses" ItemStyle-CssClass="t-c w100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSemaforo" runat="server" style="min-width:75px;"><i class="fas fa-circle"></i></asp:Label>
                                                                            <asp:Label ID="lblDiff" runat="server"></asp:Label> </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="area_util" HeaderText="Área útil (m2)" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                                    <asp:BoundField DataField="area_minima_vivienda" HeaderText="Área mínima vivienda" />
                                                                    <asp:BoundField DataField="UP_VIP" HeaderText="Unidades VIP" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                                    <asp:BoundField DataField="UP_VIS" HeaderText="Unidades VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                                    <asp:TemplateField ShowHeader="true" HeaderText="Doc" ItemStyle-CssClass="t-c w40">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton runat="server" CommandName="OpenFile" CommandArgument='<%# Container.DisplayIndex %>' ImageUrl="~/images/icon/pdf-icon.png" Visible='<%# (String.IsNullOrEmpty(Eval("ruta_carta").ToString())) ? false : true %>' ToolTip="Abrir documento" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:View>
                                                    <asp:View runat="server" ID="vProyectosCartasDetalle">
                                                        <div runat="server" id="divProyectosCartas" class="">
                                                            <div class="div_auditoria"><asp:Label ID="lbl_fec_auditoria_cartas" runat="server"/></div>
                                                            <div class="row row-cols-1 row-cols-sm-2 row-cols-md-4 row-cols-xl-6">
                                                                <asp:TextBox runat="server" ID="txt_au_proyecto_carta" Enabled="false" Visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" ID="txt_cod_proyecto__carta" Enabled="false" Visible="false"></asp:TextBox>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_radicado_manifestacion_interes" class="">Radicado manifestación interés</label>
                                                                    <asp:TextBox runat="server" ID="txt_radicado_manifestacion_interes" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_fecha_radicado_manifestacion_interes" class="">Fecha radicado m.i.</label>
                                                                    <asp:RegularExpressionValidator ID="revfecha_radicado_manifestacion_interes" runat="server" ValidationGroup="vgProyectosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_radicado_manifestacion_interes" Display="Dynamic">
                                                                        <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:RangeValidator runat="server" ID="rvfecha_radicado_manifestacion_interes" ValidationGroup="vgProyectosCartas" ControlToValidate="txt_fecha_radicado_manifestacion_interes">
                                                                        <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                                                    </asp:RangeValidator>
                                                                    <ajaxToolkit:CalendarExtender ID="cal_fecha_radicado_manifestacion_interes" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_radicado_manifestacion_interes" PopupButtonID="txt_fecha_radicado_manifestacion_interes" Format="yyyy-MM-dd" />
                                                                    <asp:TextBox runat="server" ID="txt_fecha_radicado_manifestacion_interes" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_radicado_carta_intencion" class="">Radicado carta intención</label>
                                                                    <asp:TextBox runat="server" ID="txt_radicado_carta_intencion" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_fecha_radicado_carta_intencion" class="">Fecha radicado c.i.</label>
                                                                    <asp:RegularExpressionValidator ID="revfecha_radicado_carta_intencion" runat="server" ValidationGroup="vgProyectosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_radicado_carta_intencion" Display="Dynamic">
                                                                        <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:RangeValidator runat="server" ID="rvfecha_radicado_carta_intencion" ValidationGroup="vgProyectosCartas" ControlToValidate="txt_fecha_radicado_carta_intencion">
                                                                        <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                                                    </asp:RangeValidator>
                                                                    <ajaxToolkit:CalendarExtender ID="cal_fecha_radicado_carta_intencion" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_radicado_carta_intencion" PopupButtonID="txt_fecha_radicado_carta_intencion" Format="yyyy-MM-dd" />
                                                                    <asp:TextBox runat="server" ID="txt_fecha_radicado_carta_intencion" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_radicado_otrosi" class="">Radicado otrosí</label>
                                                                    <asp:TextBox runat="server" ID="txt_radicado_otrosi" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_fecha_radicado_otrosi" class="">Fecha radicado otrosí</label>
                                                                    <asp:RegularExpressionValidator ID="revfecha_radicado_otrosi" runat="server" ValidationGroup="vgProyectosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_radicado_otrosi" Display="Dynamic">
                                                                        <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:RangeValidator runat="server" ID="rvfecha_radicado_otrosi" ValidationGroup="vgProyectosCartas" ControlToValidate="txt_fecha_radicado_otrosi">
                                                                        <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                                                    </asp:RangeValidator>
                                                                    <ajaxToolkit:CalendarExtender ID="cal_fecha_radicado_otrosi" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_radicado_otrosi" PopupButtonID="txt_fecha_radicado_otrosi" Format="yyyy-MM-dd" />
                                                                    <asp:TextBox runat="server" ID="txt_fecha_radicado_otrosi" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group-sm col">
                                                                    <label for="ddl_id_documento_constitucion_proyecto" class="">Documento constitución</label>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectosCartas" ControlToValidate="ddl_id_documento_constitucion_proyecto">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:DropDownList runat="server" ID="ddl_id_documento_constitucion_proyecto" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_tiempo_desarrollo_proyecto" class="">Tiempo desarrollo en meses</label>
                                                                    <asp:TextBox runat="server" ID="txt_meses_desarrollo" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_unidad_gestion_aplica_proyecto" class="">Unidad de gestión que aplica</label>
                                                                    <asp:TextBox runat="server" ID="txt_unidad_gestion_aplica_proyecto" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_etapa_aplica_proyecto" class="">Etapa en la que aplica</label>
                                                                    <asp:TextBox runat="server" ID="txt_etapa_aplica_proyecto" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_area_util__carta" class="">Área útil</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_area_util__carta" CssClass="form-control form-control-xs" MaxLength="13" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">m<sup>2</sup></span>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_area_minima_vivienda" class="">Área mínima vivienda</label>
                                                                    <asp:TextBox runat="server" ID="txt_area_minima_vivienda" CssClass="form-control form-control-xs" MaxLength="10"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col col-md-3 col-lg-3 col-xl-4">
                                                                    <label for="txt_localizacion_proyecto" class="">Localización proyecto</label>
                                                                    <asp:TextBox runat="server" ID="txt_localizacion_proyecto" CssClass="form-control form-control-xs" ></asp:TextBox>
                                                                </div>


                                                                <div class="form-group-sm col col-md-3 col-lg-3 col-xl-2 va-m">
                                                                        <asp:Label id="lblcarta_intencion_firmada" runat="server" />
                                                                    <asp:CheckBox runat="server" ID="chk_carta_intencion_firmada" CssClass="" Text="Carta intención firmada"/>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_fecha_firma" class="">Fecha de firma</label>
                                                                    <asp:RegularExpressionValidator ID="rev_fecha_firma" runat="server" ValidationGroup="vgProyectosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_firma" Display="Dynamic">
                                                                        <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:RangeValidator runat="server" ID="rvfecha_firma" ValidationGroup="vgProyectosCartas" ControlToValidate="txt_fecha_firma">
                                                                        <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                                                    </asp:RangeValidator>
                                                                    <br />
                                                                    <ajaxToolkit:CalendarExtender ID="cal_fecha_firma" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_firma" PopupButtonID="txt_fecha_firma" Format="yyyy-MM-dd" />
                                                                    <asp:TextBox runat="server" ID="txt_fecha_firma" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col col-sm-6 col-md-3 col-xl-4 va-m">
                                                                    <div style="float: left; min-width: 125px;">
                                                                        <asp:LinkButton ID="lbSubirCarta" runat="server" CssClass="btn btn-success btn-sm btnLoad" Text="" CausesValidation="false">
																            <i class="fas fa-upload"></i>&nbsp;Cargar archivo
                                                                        </asp:LinkButton>&nbsp;
                                                                    </div>
                                                                    <div class="labelLimited" style="padding-left:10px; height: 20px">
                                                                        <asp:Label ID="lblInfoFileCarta" runat="server" />
                                                                        <asp:FileUpload ID="fuSubirCarta" runat="server" onchange="infoFileCarta();" CssClass="fl w300 fs11 ml5" AllowMultiple="false" Style="display: none" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="card form-group bg-light mt-2">
                                                                <div class="card-header">Unidades comprometidas</div>
                                                                <div class="card-body">
                                                                    <div class="row row-cols-1 row-cols-sm-3 row-cols-md-6 row-cols-lg-6">
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_UP_VIP__carta" class="">VIP</label>
                                                                            <asp:TextBox runat="server" ID="txt_UP_VIP__carta" CssClass="form-control form-control-xs" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_UP_VIS__carta" class="">VIS</label>
                                                                            <asp:TextBox runat="server" ID="txt_UP_VIS__carta" CssClass="form-control form-control-xs" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_UP_E3" class="">Estrato 3</label>
                                                                            <asp:TextBox runat="server" ID="txt_UP_E3" CssClass="form-control form-control-xs" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_UP_E4" class="">Estrato 4</label>
                                                                            <asp:TextBox runat="server" ID="txt_UP_E4" CssClass="form-control form-control-xs" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_UP_E5" class="">Estrato 5</label>
                                                                            <asp:TextBox runat="server" ID="txt_UP_E5" CssClass="form-control form-control-xs" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_UP_E6" class="">Estrato 6</label>
                                                                            <asp:TextBox runat="server" ID="txt_UP_E6" CssClass="form-control form-control-xs" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="row">
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_observacion__carta" class="">Observaciones</label>
                                                                    <asp:TextBox runat="server" ID="txt_observacion__carta" CssClass="form-control form-control-xs" 
                                                                        TextMode="MultiLine"  onKeyDown="MaxLengthText(this,2000);" onKeyUp="MaxLengthText(this,2000);" MaxLength="2000"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasNavFirst" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasNavBack" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasNavNext" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasNavLast" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvProyectosCartas" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="card-footer">
                                        <asp:UpdatePanel runat="server" ID="upProyectosCartasFoot" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:UpdatePanel runat="server" ID="upProyectosCartasMsg" UpdateMode="Conditional" class="alert-card">
                                                    <ContentTemplate>
                                                        <div runat="server" id="msgProyectosCartas" class="alert d-none" role="alert"></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <div class="row">
                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Panel ID="pProyectosCartasView" runat="server">
                                                            <div class="btn-group" role="group">
                                                                <asp:LinkButton ID="btnProyectosCartasVG" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnCartasVista_Click">
                                  <i class="fas fa-border-all"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosCartasVD" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnCartasVista_Click">
                                  <i class="fas fa-bars"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Label ID="lblProyectosCartasCuenta" runat="server"></asp:Label>
                                                    </div>

                                                    <div class="col-auto t-c">
                                                        <asp:Panel ID="pProyectosCartasNavegacion" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnProyectosCartasNavFirst" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnCartasNavegacion_Click">
											            <i class="fas fa-angle-double-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosCartasNavBack" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnCartasNavegacion_Click">
											            <i class="fas fa-angle-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosCartasNavNext" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnCartasNavegacion_Click">
											            <i class="fas fa-angle-right"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosCartasNavLast" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnCartasNavegacion_Click">
											            <i class="fas fa-angle-double-right"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto ml-auto">
                                                        <asp:Panel ID="pProyectosCartasAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnProyectosCartasAdd" runat="server" disabled="" CssClass="btn btn-outline-danger" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnCartasAccion_Click">
										              <i class="fas fa-plus"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosCartasEdit" runat="server" CssClass="btn btn-outline-danger" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnCartasAccion_Click">
										              <i class="fas fa-pencil-alt"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosCartasDel" runat="server" CssClass="btn btn-outline-danger" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnCartasAccion_Click">
										              <i class="far fa-trash-alt"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto">
                                                        <asp:Panel ID="pProyectosCartasExecAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnProyectosCartasCancelar" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" OnClick="btnCartasCancelar_Click" CausesValidation="false">
										              <i class="fas fa-times"></i>&nbspCancelar
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosCartasAccionFinal" runat="server" CssClass="btn btn-outline-primary" ValidationGroup="vgProyectosCartas" OnClientClick="if(Page_ClientValidate('vgProyectosCartas')) return openModal('modProyectos');">
										              <i class="fas fa-check"></i>&nbspAceptar
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosCartasDel" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:View>

                                <%--********************************************************************--%>
                                <%--*************** Licencias ******************************************--%>
                                <asp:View ID="View3" runat="server">
                                    <div class="card-body">
                                        <asp:UpdatePanel runat="server" ID="upProyectosLicencias" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:MultiView runat="server" ID="mvProyectosLicencias" ActiveViewIndex="0">
                                                    <asp:View runat="server" ID="vProyectosLicencias">
                                                        <div class="gv-w">
                                                            <asp:GridView ID="gvProyectosLicencias" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="au_proyecto_licencia,origen,ruta_licencia"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" AllowSorting="true" OnSorting="gvLicencias_Sorting" OnSelectedIndexChanged="gvLicencias_SelectedIndexChanged"
                                                                OnRowDataBound="gvLicencias_RowDataBound" OnRowCreated="gv_RowCreated" OnRowCommand="gvLicencias_RowCommand">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_proyecto_licencia" HeaderText="au_proyecto_licencia" Visible="false" />
                                                                    <asp:BoundField DataField="origen" HeaderText="origen" />
                                                                    <asp:BoundField DataField="tipo_licencia" HeaderText="Tipo licencia" />
                                                                    <asp:BoundField DataField="curador" HeaderText="Curadoria" />
                                                                    <asp:BoundField DataField="licencia" HeaderText="No. Licencia" />
                                                                    <asp:BoundField DataField="observacion" HeaderText="Observaciones" />
                                                                    <asp:TemplateField ShowHeader="true" HeaderText="Doc" ItemStyle-CssClass="t-c w40">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton runat="server" CommandName="OpenFile" CommandArgument='<%# Container.DisplayIndex %>' ImageUrl="~/images/icon/pdf-icon.png" Visible='<%# (String.IsNullOrEmpty(Eval("ruta_licencia").ToString())) ? false : true %>' ToolTip="Abrir documento" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:View>
                                                    <asp:View runat="server" ID="vProyectosLicenciasDetalle">
                                                        <div runat="server" id="divProyectosLicencias" class="">
                                                            <div class="div_auditoria"><asp:Label ID="lbl_fec_auditoria_licencia" runat="server"/></div>
                                                            <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4">
                                                                <asp:TextBox runat="server" ID="txt_au_proyecto_licencia" Enabled="false" Visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" ID="txt_cod_proyecto__licencia" Enabled="false" Visible="false"></asp:TextBox>

                                                                <div class="form-group-sm col">
                                                                    <label for="ddlb_id_fuente_informacion" class="">Fuente información</label>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectosLicencias" ControlToValidate="ddlb_id_fuente_informacion">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:DropDownList runat="server" ID="ddlb_id_fuente_informacion" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="form-group-sm col">
                                                                    <label for="ddl_id_tipo_licencia" class="">Tipo licencia</label>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectosLicencias" ControlToValidate="ddl_id_tipo_licencia">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:DropDownList runat="server" ID="ddl_id_tipo_licencia" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_licencia" class="">Número licencia</label>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectosLicencias" ControlToValidate="txt_licencia">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:TextBox runat="server" ID="txt_licencia" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group-sm col">
                                                                    <label for="ddl_curador" class="">Curaduría</label>
                                                                    <asp:DropDownList runat="server" ID="ddl_curador" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                        <asp:ListItem Value="1">Curaduría 1</asp:ListItem>
                                                                        <asp:ListItem Value="2">Curaduría 2</asp:ListItem>
                                                                        <asp:ListItem Value="3">Curaduría 3</asp:ListItem>
                                                                        <asp:ListItem Value="4">Curaduría 4</asp:ListItem>
                                                                        <asp:ListItem Value="5">Curaduría 5</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="form-group-sm col col-sm-12 col-lg-2">
                                                                    <label for="txt_fecha_ejecutoria" class="">Fecha ejecutoria</label>
                                                                    <asp:RegularExpressionValidator ID="revfecha_ejecutoria" runat="server" ValidationGroup="vgProyectosLicencias" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_ejecutoria" Display="Dynamic">
                                                                        <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:RangeValidator runat="server" ID="rvfecha_ejecutoria" ValidationGroup="vgProyectosLicencias" ControlToValidate="txt_fecha_ejecutoria">
                                                                        <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                                                    </asp:RangeValidator>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectosLicencias" ControlToValidate="txt_fecha_ejecutoria">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                                    </asp:RequiredFieldValidator>
                                                                    <ajaxToolkit:CalendarExtender ID="cal_fecha_ejecutoria" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_ejecutoria" PopupButtonID="txt_fecha_ejecutoria" Format="yyyy-MM-dd" />
                                                                    <asp:TextBox runat="server" ID="txt_fecha_ejecutoria" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group-sm col col-sm-12 col-lg-2">
                                                                    <label for="txt_termino_vigencia_meses" class="">Vigencia meses</label>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectosLicencias" ControlToValidate="txt_termino_vigencia_meses">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:TextBox runat="server" ID="txt_termino_vigencia_meses" CssClass="form-control form-control-xs" MaxLength="3" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group-sm col">
                                                                    <label for="txta_nombreproyecto" class="">Nombre proyecto</label>
                                                                    <asp:TextBox runat="server" ID="txt_nombreproyecto" CssClass="form-control form-control-xs"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group-sm col col-sm-12 col-md-8 col-lg-5">
                                                                    <br>
                                                                    </br>
                                                                    <div style="float: left; min-width: 125px;">
                                                                        <asp:LinkButton ID="lbSubirLic" runat="server" CssClass="btn btn-success btn-sm btnLoad" Text="" CausesValidation="false">
																            <i class="fas fa-upload"></i>&nbspCargar archivo
                                                                        </asp:LinkButton>&nbsp;
                                                                    </div>
                                                                    <div class="labelLimited" style="padding-left:10px;">
                                                                        <asp:Label ID="lblInfoFileLic" runat="server" />
                                                                        <asp:FileUpload ID="fuSubirLic" runat="server" onchange="infoFileLicencia();" CssClass="fl w300 fs11 ml5" AllowMultiple="false" Style="display: none" />
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col-12 col-sm-12 col-md-12 col-lg-3">
                                                                    <asp:Panel runat="server" ID="pPlanesPLicenciasUrb">
                                                                        <div class="card mb-3">
                                                                            <div class="card-header">Urbanismo</div>
                                                                            <div class="card-body">
                                                                                <div class="row">
                                                                                    <div class="col-sm-6 col-lg-12">
                                                                                        <div class="form-group-sm">
                                                                                            <label for="txt_plano_urbanistico_aprobado" class="" tooltip="Plano urbanístico aprobado">Plano urb. aprobado</label>
                                                                                            <asp:TextBox runat="server" ID="txt_plano_urbanistico_aprobado" CssClass="form-control form-control-xs"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-sm-6 col-lg-12">
                                                                                        <div class="form-group-sm">
                                                                                            <label for="txt_porcentaje_ejecucion_urbanismo" class="">% Ejecutado</label>
                                                                                            <asp:RangeValidator runat="server" ValidationGroup="vgProyectosLicencias" ControlToValidate="txt_porcentaje_ejecucion_urbanismo" MinimumValue="0" MaximumValue="100" Type="Double" Display="Dynamic" >
                                                                                                <uc:ToolTip width="160px" ToolTip="El valor VIP debe estar entre 0 y 100" runat="server"/>
                                                                                            </asp:RangeValidator>
                                                                                            <div class="input-group input-group-xs">
                                                                                                <asp:TextBox runat="server" ID="txt_porcentaje_ejecucion_urbanismo" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                <div class="input-group-append">
                                                                                                    <span class="input-group-text">%</span>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-12 col-sm-6 col-lg-12">
                                                                                        <table class="table table-sm">
                                                                                            <caption>Datos urbanísticos (m<sup>2</sup>)</caption>
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td class="w-40">Área bruta</td>
                                                                                                    <td class="">
                                                                                                        <asp:TextBox runat="server" ID="txt_area_bruta__licencia" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td title="Estructura ecológica rondas">Área neta</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_neta" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td title="Estructura ecológica principal">Área útil</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_util__licencia" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div class="col-12 col-sm-6 col-lg-12">
                                                                                        <table class="table table-sm">
                                                                                            <caption>Cesiones (m<sup>2</sup>)</caption>
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td>Zonas verdes</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_cesion_zonas_verdes" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Vías</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_cesion_vias" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td title="Equipamiento comunal">Eq. comunal</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_cesion_eq_comunal" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </div>

                                                                <div class="col-12 col-sm-12 col-md-12 col-lg-9">
                                                                    <asp:Panel runat="server" ID="pPlanesPLicenciasConst">
                                                                        <div class="card mb-3">
                                                                            <div class="card-header">Construcción</div>
                                                                            <div class="card-body">

                                                                                <div class="row">
                                                                                    <div class="col-sm-6">
                                                                                        <div class="form-group-sm">
                                                                                            <label for="ddlb_id_obligacion_VIS" class="">Obligación VIS</label>
                                                                                            <asp:DropDownList runat="server" ID="ddlb_id_obligacion_VIS" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-sm-6">
                                                                                        <div class="form-group-sm">
                                                                                            <label for="ddlb_id_obligacion_VIP" class="">Obligación VIP</label>
                                                                                            <asp:DropDownList runat="server" ID="ddlb_id_obligacion_VIP" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="row">
                                                                                    <div class="col-md-8">
                                                                                        <table class="table table-sm">
                                                                                            <caption>Datos usos (m<sup>2</sup>)</caption>
                                                                                            <thead class="text-center">
                                                                                                <td class=""></td>
                                                                                                <td class="">Área terreno (m<sup>2</sup>)</td>
                                                                                                <td class="">Área construida (m<sup>2</sup>)</td>
                                                                                                <td class="">% Obligación 
                                                                                                    <asp:RangeValidator runat="server" ValidationGroup="vgProyectosLicencias" ControlToValidate="txt_porcentaje_obligacion_VIP" MinimumValue="0" MaximumValue="100" Type="Double" Display="Dynamic" >
                                                                                                        <uc:ToolTip width="160px" ToolTip="El valor VIP debe estar entre 0 y 100" runat="server"/>
                                                                                                    </asp:RangeValidator>
                                                                                                    <asp:RangeValidator runat="server" ValidationGroup="vgProyectosLicencias" ControlToValidate="txt_porcentaje_obligacion_VIS" MinimumValue="0" MaximumValue="100" Type="Double" Display="Dynamic" >
                                                                                                        <uc:ToolTip width="160px" ToolTip="El valor VIS debe estar entre 0 y 100" runat="server"/>
                                                                                                    </asp:RangeValidator>
                                                                                                </td>
                                                                                                <td class="">Unidades</td>
                                                                                            </thead>
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td>VIP</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_terreno_VIP" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_construida_VIP" CssClass="form-control form-control-xs"  MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_porcentaje_obligacion_VIP" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_unidades_vivienda_VIP" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>VIS</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_terreno_VIS" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_construida_VIS" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_porcentaje_obligacion_VIS" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_unidades_vivienda_VIS" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>No VIS</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_terreno_no_VIS" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_construida_no_VIS" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_unidades_vivienda_no_VIS" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Comercio</td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_comercio" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Oficina</td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_oficina" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Institucional</td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_institucional" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Industria</td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_industria" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </div>

                                                                                    <div class="col-md-4">
                                                                                        <table class="table table-sm">
                                                                                            <caption>Datos proyecto (m<sup>2</sup>)</caption>
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td>Lote</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_lote" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Sótano</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_sotano" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Semisótano</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_semisotano" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Primer piso</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_primer_piso" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Pisos restantes</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_pisos_restantes" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Construida total</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_construida_total" CssClass="form-control form-control-xs" MaxLength="365" Enabled="true" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Libre primer piso</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_libre_primer_piso" CssClass="form-control form-control-xs" MaxLength="10" Enabled="true" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>% Ejecutado
                                                                                                    <asp:RangeValidator runat="server" ValidationGroup="vgProyectosLicencias" ControlToValidate="txt_porcentaje_ejecucion_construccion" MinimumValue="0" MaximumValue="100" Type="Double" Display="Dynamic" >
                                                                                                        <uc:ToolTip width="160px" ToolTip="El valor debe estar entre 0 y 100" runat="server"/>
                                                                                                    </asp:RangeValidator></td>
                                                                                                    <td>
                                                                                                        <div class="input-group input-group-xs">
                                                                                                            <asp:TextBox runat="server" ID="txt_porcentaje_ejecucion_construccion" CssClass="form-control form-control-xs" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                            <div class="input-group-append">
                                                                                                                <span class="input-group-text">%</span>
                                                                                                            </div>
                                                                                                            
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                </div>

                                                                            </div>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_observacion__licencia" class="">Observaciones</label>
                                                                    <asp:TextBox runat="server" ID="txt_observacion__licencia" CssClass="form-control form-control-xs" 
                                                                        TextMode="MultiLine"  onKeyDown="MaxLengthText(this,2000);" onKeyUp="MaxLengthText(this,2000);" MaxLength="2000"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasNavFirst" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasNavBack" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasNavNext" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasNavLast" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvProyectosLicencias" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="card-footer">
                                        <asp:UpdatePanel runat="server" ID="upProyectosLicenciasFoot" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:UpdatePanel runat="server" ID="upProyectosLicenciasMsg" UpdateMode="Conditional" class="alert-card">
                                                    <ContentTemplate>
                                                        <div runat="server" id="msgProyectosLicencias" class="alert d-none" role="alert"></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <div class="row">
                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Panel ID="pProyectosLicenciasView" runat="server">
                                                            <div class="btn-group" role="group">
                                                                <asp:LinkButton ID="btnProyectosLicenciasVG" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnLicenciasVista_Click">
                                                                    <i class="fas fa-border-all"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosLicenciasVD" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnLicenciasVista_Click">
                                                                    <i class="fas fa-bars"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Label ID="lblProyectosLicenciasCuenta" runat="server"></asp:Label>
                                                    </div>

                                                    <div class="col-auto t-c">
                                                        <asp:Panel ID="pProyectosLicenciasNavegacion" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnProyectosLicenciasNavFirst" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnLicenciasNavegacion_Click">
											            <i class="fas fa-angle-double-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosLicenciasNavBack" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnLicenciasNavegacion_Click">
											            <i class="fas fa-angle-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosLicenciasNavNext" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnLicenciasNavegacion_Click">
											            <i class="fas fa-angle-right"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosLicenciasNavLast" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnLicenciasNavegacion_Click">
											            <i class="fas fa-angle-double-right"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto ml-auto">
                                                        <asp:Panel ID="pProyectosLicenciasAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnProyectosLicenciasAdd" runat="server" disabled="" CssClass="btn btn-outline-danger" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnLicenciasAccion_Click">
										                            <i class="fas fa-plus"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosLicenciasEdit" runat="server" CssClass="btn btn-outline-danger" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnLicenciasAccion_Click">
										                            <i class="fas fa-pencil-alt"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosLicenciasDel" runat="server" CssClass="btn btn-outline-danger" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnLicenciasAccion_Click">
										                            <i class="far fa-trash-alt"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto">
                                                        <asp:Panel ID="pProyectosLicenciasExecAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnProyectosLicenciasCancelar" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" OnClick="btnLicenciasCancelar_Click" CausesValidation="false">
										              <i class="fas fa-times"></i>&nbspCancelar
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnProyectosLicenciasAccionFinal" runat="server" CssClass="btn btn-outline-primary" ValidationGroup="vgProyectosLicencias" OnClientClick="if(Page_ClientValidate('vgProyectosLicencias')) return openModal('modProyectos');">
										              <i class="fas fa-check"></i>&nbspAceptar
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnProyectosLicenciasDel" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:View>

                                <%--********************************************************************--%>
                                <%--*************** Responsables *****************************************--%>
                                <asp:View ID="View4" runat="server">
                                    <div class="card-user-control">
                                        <asp:UpdatePanel runat="server" ID="upProyectosActores" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hdd_idactor" runat="server" Value="0" />
                                                <uc:Actor ID="ucRepresentative" runat="server" ReferenceTypeID="2" ControlID="RepresentantePA"/>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:View>

                                <%--********************************************************************--%>
                                <%--****************** Visitas *****************************************--%>
                                <asp:View ID="View5" runat="server">
                                    <div class="card-user-control">
                                        <asp:UpdatePanel runat="server" ID="upProyectosVisitas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hdd_idvisita" runat="server" Value="0" />
                                                <uc:VisitaSitio ID="ucVisita" runat="server" ControlID="VisitasPA" OnUserControlException="ucVisita_UserControlException"
                                                    OnViewDoc="ucVisita_ViewDoc" /><%--OnChangedSize="ucVisita_ChangedSize"--%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:View>
                                
                                <%--********************************************************************--%>
                                <%--*************** Seguimientos *****************************************--%>
                                <asp:View ID="View6" runat="server">
                                    <div class="card-body tab-uc">
                                        <asp:UpdatePanel runat="server" ID="upTracing" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlTracing" runat="server" Visible="false"  style="min-height: 50px;"> 
                                                    <asp:LinkButton ID="btnGoBanco" runat="server" Text="" CssClass="btn btn-success btnLoad" OnClick="btnGoBanco_Click" style="left: 45%; position: absolute; z-index: 1;">
							                            <i class="fas fa-check"></i>&nbsp&nbsp;Ver ficha de seguimiento 
                                                    </asp:LinkButton>
                                                    <asp:HiddenField ID="hdd_idBanco" runat="server" Value="0" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlNoTracing" runat="server" Visible="true">
                                                    <div class="row row-cols-12 ">
                                                        <div class="form-group-sm col-11 ">
                                                            <label class="fs10 fwb">El proyecto no cuenta con seguimiento. </label>
                                                        </div>
                                                        <div class="form-group-sm col-9 col-sm-8 col-md-7 col-lg-5 col-xl-4">
                                                            <label for="txt_NoTracing" class="">¿Desea iniciar el seguimiento para este proyecto asociativo? </label>
                                                        </div>
                                                        <div class="form-group-sm col-3 col-sm-4 col-md-5 col-lg-7 col-xl-8">
                                                            <asp:LinkButton ID="btnCrearSeguimiento" runat="server" Text="" CssClass="btn btn-outline-primary" OnClick="btnSeguimientoCrear_Click">
							                            <i class="fas fa-check"></i>&nbsp&nbsp;Aceptar
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:View>
                            </asp:MultiView>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:HiddenField runat="server" ID="hfEvtGVProyectos" Value="" />
        <asp:HiddenField ID="hfGVProyectosSV" runat="server" />
        <asp:HiddenField ID="hfGVProyectosSH" runat="server" />
        <asp:HiddenField ID="hfGVProyectosPrediosSV" runat="server" />
        <asp:HiddenField ID="hfGVProyectosPrediosSH" runat="server" />
        <asp:HiddenField ID="hfGVProyectosCartasSV" runat="server" />
        <asp:HiddenField ID="hfGVProyectosCartasSH" runat="server" />
        <asp:HiddenField ID="hfGVProyectosLicenciasSV" runat="server" />
        <asp:HiddenField ID="hfGVProyectosLicenciasSH" runat="server" />
        <asp:HiddenField ID="hfGVProyectosActoresSV" runat="server" />
        <asp:HiddenField ID="hfGVProyectosActoresSH" runat="server" />
        <asp:HiddenField ID="hddCardNumber" runat="server" Value="0" />
        <asp:HiddenField ID="hddCardstate" runat="server" Value="0" />
    </div>

    <%--********************************************************************--%>	<%--<asp:View ID="View3" runat="server">
                </asp:View>--%>	<%--********************************************************************--%>
    <script type="text/javascript">
        function pageLoad() {
            //*****************************************ESTILO GRIDVIEWS
            $('#<%=gvProyectos.ClientID%>').gridviewScroll({
                height: 350,
                startVertical: $("#<%=hfGVProyectosSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGVProyectosSH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    console.log("Vertical scroll:", delta);
                    $("#<%=hfGVProyectosSV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    console.log("Horizontal scroll:", delta);
                    $("#<%=hfGVProyectosSH.ClientID%>").val(delta);
                }
            });

            $('#<%=gvProyectosPredios.ClientID%>').gridviewScroll({
                height: 300,
                startVertical: $("#<%=hfGVProyectosPrediosSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGVProyectosPrediosSH.ClientID%>").val(),
                onScrollVertical: function (delta) { $("#<%=hfGVProyectosPrediosSV.ClientID%>").val(delta); },
                onScrollHorizontal: function (delta) { $("#<%=hfGVProyectosPrediosSH.ClientID%>").val(delta); }
            });

            $('#<%=gvProyectosCartas.ClientID%>').gridviewScroll({
                height: 300,
                startVertical: $("#<%=hfGVProyectosCartasSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGVProyectosCartasSH.ClientID%>").val(),
                onScrollVertical: function (delta) { $("#<%=hfGVProyectosCartasSV.ClientID%>").val(delta); },
                onScrollHorizontal: function (delta) { $("#<%=hfGVProyectosCartasSH.ClientID%>").val(delta); }
            });

            $('#<%=gvProyectosLicencias.ClientID%>').gridviewScroll({
                height: 300,
                startVertical: $("#<%=hfGVProyectosLicenciasSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGVProyectosLicenciasSH.ClientID%>").val(),
                onScrollVertical: function (delta) { $("#<%=hfGVProyectosLicenciasSV.ClientID%>").val(delta); },
                onScrollHorizontal: function (delta) { $("#<%=hfGVProyectosLicenciasSH.ClientID%>").val(delta); }
            });

            var last = sessionStorage.getItem('py-ac1-name');
            var status = sessionStorage.getItem('py-ac1-status');

            if (last != null) {
                $('#collapse1').removeClass('show');
                if (status == 1)
                    $('#' + last).addClass('show');
            }

            $("#heading1").click(function () {
                var last = sessionStorage.getItem('py-ac1-name');
                var status = sessionStorage.getItem('py-ac1-status');
                sessionStorage.setItem('py-ac1-name', "collapse1");
                sessionStorage.setItem('py-ac1-status', (last == "collapse1" ? status == 0 ? 1 : 0 : 1));

            });
        }

        function verProyectosFotos() {
            var width = 910;
            var height = 700;
            var left = (screen.width / 2) - (width / 2);
            var top = (screen.height / 2) - (height / 2);
            return window.open("ProyectosFotos.aspx", "ProyectosFotos", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + width + ', height=' + height + ', top=' + top + ', left=' + left);
        }

        function verProyectosVisitasFotos() {
            var width = 1200;
            var height = 480;
            var left = (screen.width / 2) - (width / 2);
            var top = (screen.height / 2) - (height / 2);
            return window.open("ProyectosVisitasFotos.aspx", "ProyectosVisitasFotos", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + width + ', height=' + height + ', top=' + top + ', left=' + left);
        }

        function infoFileLicencia() {
            var name = document.getElementById('<%=fuSubirLic.ClientID%>').value;
        var pos = name.lastIndexOf('\\');
        document.getElementById('<%=lblInfoFileLic.ClientID%>').innerHTML = name.substring(pos + 1);
        }

        function infoFileCarta() {
            var name = document.getElementById('<%=fuSubirCarta.ClientID%>').value;
        var pos = name.lastIndexOf('\\');
        document.getElementById('<%=lblInfoFileCarta.ClientID%>').innerHTML = name.substring(pos + 1);
        }

    </script>
</asp:Content>
