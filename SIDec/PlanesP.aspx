<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="PlanesP.aspx.cs" Inherits="SIDec.PlanesP" ViewStateMode="Enabled" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControls/FileUpload.ascx" TagName="FileUpload" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Conditional">
        <ContentTemplate>
            <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
            <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--********************************************************************--%>
    <%--*************** Alert Msg Main --%>
    <%--********************************************************************--%>
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

    <%--********************************************************************--%>
    <%--*************** Modal --%>
    <%--********************************************************************--%>
    <div id="modPlanesP" class="modal fade" data-backdrop="static" role="dialog" aria-hidden="true">
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
                    <asp:LinkButton ID="btnConfirmarPlanesP" runat="server" Text="" CssClass="btn btn-outline-primary" OnClick="btnConfirmar_Click" data-dismiss="modal">
							<i class="fas fa-check"></i>&nbsp&nbspAceptar
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <%--********************************************************************--%>
    <%--*************** gridviews --%>
    <%--********************************************************************--%>
    <asp:HiddenField runat="server" ID="hfEvtGVPlanesP" Value="" />
    <asp:HiddenField ID="hfGVPlanesPSV" runat="server" />
    <asp:HiddenField ID="hfGVPlanesPSH" runat="server" />
    <asp:HiddenField ID="hfGVPlanesPManzanasSV" runat="server" />
    <asp:HiddenField ID="hfGVPlanesPManzanasSH" runat="server" />
    <asp:HiddenField ID="hfGVPlanesPCesionesSV" runat="server" />
    <asp:HiddenField ID="hfGVPlanesPCesionesSH" runat="server" />
    <asp:HiddenField ID="hfGVPlanesPActosSV" runat="server" />
    <asp:HiddenField ID="hfGVPlanesPActosSH" runat="server" />
    <asp:HiddenField ID="hfGVPlanesPLicenciasSV" runat="server" />
    <asp:HiddenField ID="hfGVPlanesPLicenciasSH" runat="server" />
    <asp:HiddenField ID="hfGVPlanesPVisitasSV" runat="server" />
    <asp:HiddenField ID="hfGVPlanesPVisitasSH" runat="server" />


    <asp:HiddenField ID="hdd_Proyecto_PlanesP_Id" runat="server" />

    <div id="divData" runat="server">
        <div class="col-12" role="main">

            <%--********************************************************************--%>
            <%--*************** Planes Parciales --%>
            <%--********************************************************************--%>
            <div class="card mt-3 mb-5">
                <div class="card-header card-header-main">
                    <div class="row">
                        <div class="col-sm-6 text-primary">
                            <h4>Planes Parciales</h4>
                        </div>
                        <div class="col-sm-6">
                            <asp:UpdatePanel ID="pnlBuscar" runat="server" DefaultButton="btnBuscar">
                                <ContentTemplate>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control" placeholder="Búsqueda por nombre o número del plan parcial" />
                                        <div class="input-group-append">
                                            <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn btn-outline-primary" CausesValidation="false" OnClick="btnBuscar_Click" />
                                        </div>
                                        <asp:Button runat="server" ID="btnVolver" Text="Volver Proyecto Asociativo" Visible="false" CssClass="btn btn-outline-primary" CausesValidation="false" OnClick="btnVolver_Click" />
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
                    <asp:UpdatePanel runat="server" ID="upPlanesP" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:MultiView runat="server" ID="mvPlanesP" ActiveViewIndex="0">
                                <asp:View runat="server" ID="vPlanesP">
                                    <div class="gv-w">
                                        <asp:GridView ID="gvPlanesP" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="au_planp,idarchivo" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                            AllowSorting="true" OnSorting="gvPlanesP_Sorting" OnSelectedIndexChanged="gvPlanesP_SelectedIndexChanged"
                                            OnDataBinding="gvPlanesP_DataBinding" OnRowDataBound="gvPlanesP_RowDataBound" OnRowCreated="gv_RowCreated">
                                            <Columns>
                                                <asp:BoundField DataField="au_planp" HeaderText="Cód. PP" SortExpression="au_planp" ItemStyle-CssClass="t-c" />
                                                <asp:BoundField DataField="cod_sdp" HeaderText="Cod. SDP" SortExpression="cod_sdp" />
                                                <asp:BoundField DataField="nombre_planp" HeaderText="Nombre" SortExpression="nombre_planp" />
                                                <asp:BoundField DataField="direccion_planp" HeaderText="Dirección" />
                                                <asp:BoundField DataField="localidad" HeaderText="Localidad" />
                                                <asp:BoundField DataField="categoria_planp" HeaderText="Categoria" SortExpression="categoria_planp" />
                                                <asp:BoundField DataField="estado_planp" HeaderText="Estado" SortExpression="estado_planp" />
                                                <asp:BoundField DataField="tipo_tratamiento" HeaderText="Tipo tratamiento" SortExpression="tipo_tratamiento" />
                                                <asp:BoundField DataField="clasificacion_suelo" HeaderText="Clasificación suelo" />
                                                <asp:BoundField DataField="area_bruta" HeaderText="Área bruta" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                <asp:BoundField DataField="area_afectaciones" HeaderText="Área afectaciones" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                <asp:BoundField DataField="area_neta_urbanizable" HeaderText="Área neta urbanizable" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                <asp:BoundField DataField="SP_total" HeaderText="Suelo potencial total" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                <asp:BoundField DataField="porc_SE_total" HeaderText="% Suelo ejecutado total" DataFormatString="{0:P0}" SortExpression="porc_ejecutado" ItemStyle-CssClass="t-c" />
                                                <asp:BoundField DataField="SP_vivienda" HeaderText="Suelo potencial vivienda" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                <asp:BoundField DataField="porc_SE_vivienda" HeaderText="% Suelo ejecutado vivienda" DataFormatString="{0:P0}" ItemStyle-CssClass="t-r" />
                                                <asp:BoundField DataField="unidades_potencial_vivienda" HeaderText="Unidades potencial" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                <asp:BoundField DataField="porc_unidades_ejecutadas_vivienda" HeaderText="% Unidades ejecutadas vivienda" DataFormatString="{0:P0}" ItemStyle-CssClass="t-r" />
                                                <asp:BoundField DataField="usu_responsable" HeaderText="Usuario responsable" ItemStyle-CssClass="w250" />
                                            </Columns>
                                            <SelectedRowStyle CssClass="gvItemSelected" />
                                            <HeaderStyle CssClass="gvHeader" />
                                            <RowStyle CssClass="gvItem" />
                                            <PagerStyle CssClass="gvPager" />
                                        </asp:GridView>
                                    </div>
                                </asp:View>
                                <asp:View runat="server" ID="vPlanesPDetalle">
                                    <div runat="server" id="divPlanesP" class="">
                                        <div class="row">
                                        <%--<div class="row row-cols-1 row-cols-sm-2 row-cols-xl-4">--%>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-3 col-xl-2">
                                                <div class="row">
                                                    <div class="form-group-sm col">
                                                        <label for="txt_au_planp" class="">Código PP</label>
                                                        <asp:TextBox runat="server" ID="txt_au_planp" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm col">
                                                        <label for="txt_cod_sdp" class="">Cod. SDP</label>
                                                        <asp:TextBox runat="server" ID="txt_cod_sdp" CssClass="form-control form-control-xs" ></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            
                                            <div class="form-group-sm col-12 col-sm-6 col-md-5 col-xl-4">
                                                <label for="txt_nombre_planp" class="">Nombre</label>
                                                <asp:TextBox runat="server" ID="txt_nombre_planp" CssClass="form-control form-control-xs" MaxLength="100"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="txt_nombre_planp" />
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-2">
                                                <label for="txt_direccion_planp" class="">Dirección</label>
                                                <asp:TextBox runat="server" ID="txt_direccion_planp" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-2">
                                                <label for="ddlb_cod_localidad" class="">Localidad</label>
                                                <asp:DropDownList runat="server" ID="ddlb_cod_localidad" CssClass="form-control form-control-xs" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlb_cod_localidad_SelectedIndexChanged">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="ddlb_cod_localidad" />
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-2">
                                                <label for="ddl_idupz" class="lblBasic">UPZ</label>
                                                <asp:DropDownList runat="server" ID="ddl_idupz" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3">
                                                <label for="ddlb_id_categoria_planp" class="">Categoria</label>
                                                <asp:DropDownList runat="server" ID="ddlb_id_categoria_planp" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="ddlb_id_categoria_planp" />
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3">
                                                <label for="ddlb_id_estado_planp" class="">Estado</label>
                                                <asp:DropDownList runat="server" ID="ddlb_id_estado_planp" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="ddlb_id_estado_planp" />
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3">
                                                <label for="ddlb_id_tipo_tratamiento" class="">Tipo tratamiento</label>
                                                <asp:DropDownList runat="server" ID="ddlb_id_tipo_tratamiento" CssClass="form-control form-control-xs" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlb_id_tipo_tratamiento_SelectedIndexChanged" AutoPostBack="true" required>
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="ddlb_id_tipo_tratamiento" />
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3">
                                                <label for="ddlb_id_clasificacion_suelo" class="">Clasificación del suelo</label>
                                                <asp:DropDownList runat="server" ID="ddlb_id_clasificacion_suelo" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="ddlb_id_clasificacion_suelo" />
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-3 col-xl-2">
                                                <label for="chk_es_proyecto_asociativo" class=""></label><br />  
                                                <asp:CheckBox runat="server" ID="chk_es_proyecto_asociativo" CssClass="" Text="Es proyecto asociativo" />
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-3 col-xl-2">
                                                <label for="chk_tiene_carta_intencion" class=""></label><br />
                                                <asp:CheckBox runat="server" ID="chk_tiene_carta_intencion" CssClass="" Text="Tiene carta de intención" AutoPostBack="true" OnCheckedChanged="chk_tiene_carta_intencion_CheckedChanged" />
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-3 col-xl-2">
                                                <label for="txt_fecha_firma_carta_intencion" class="">Fecha carta</label>
                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_firma_carta_intencion" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_firma_carta_intencion" PopupButtonID="txt_fecha_firma_carta_intencion" Format="yyyy-MM-dd" />
                                                <asp:TextBox runat="server" ID="txt_fecha_firma_carta_intencion" CssClass="form-control form-control-xs" TextMode="Date" Enabled="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ID="rfv_fecha_firma_carta_intencion" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="txt_fecha_firma_carta_intencion" Enabled="false" />
                                                <asp:RangeValidator runat="server" ID="rv_fecha_firma_carta_intencion" ValidationGroup="vgPlanesP" ControlToValidate="txt_fecha_firma_carta_intencion" Enabled="false" />
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-md-3 col-xl-2">
                                                <label for="txt_fecha_iniciacion_viviendas" class="">Fecha iniciación viviendas</label>
                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_iniciacion_viviendas" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_iniciacion_viviendas" PopupButtonID="txt_fecha_iniciacion_viviendas" Format="yyyy-MM-dd" />
                                                <asp:TextBox runat="server" ID="txt_fecha_iniciacion_viviendas" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                                <asp:RangeValidator runat="server" ID="rv_fecha_iniciacion_viviendas" ValidationGroup="vgPlanesP" ControlToValidate="txt_fecha_iniciacion_viviendas" />
                                            </div>
                                            <div class="form-group-sm col-12 col-sm-6 col-xl-4">
                                                <label for="ddlb_cod_usu_responsable" class="">Usuario responsable</label>
                                                <asp:DropDownList runat="server" ID="ddlb_cod_usu_responsable" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="ddlb_cod_usu_responsable" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group-sm col">
                                                <label for="txt_observacion" class="">Observaciones</label>
                                                <asp:TextBox runat="server" ID="txt_observacion" CssClass="form-control form-control-xs" MaxLength="500" 
                                                    TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <ul class="nav nav-tabs nav-pills nav-justified nav-multiview-tab" runat="server" id="ulPlanesPSub">
                                                    <%--flex-column flex-sm---%>
                                                    <li class="nav-item nav-item-p">
                                                        <asp:LinkButton ID="lbPlanesPSub_0" runat="server" CssClass="nav-link active" CommandArgument="0" CausesValidation="false" OnClick="btnPlanesPSub_Click">Datos urbanísticos</asp:LinkButton>
                                                    </li>
                                                    <li class="nav-item nav-item-p">
                                                        <asp:LinkButton ID="lbPlanesPSub_1" runat="server" CssClass="nav-link" CommandArgument="1" CausesValidation="false" OnClick="btnPlanesPSub_Click">Rangos / Actividades / Zonas</asp:LinkButton>
                                                    </li>
                                                    <li class="nav-item nav-item-p">
                                                        <asp:LinkButton ID="lbPlanesPSub_2" runat="server" CssClass="nav-link" CommandArgument="2" CausesValidation="false" OnClick="btnPlanesPSub_Click">Obligaciones urbanísticas</asp:LinkButton>
                                                    </li>
                                                    <li class="nav-item nav-item-p">
                                                        <asp:LinkButton ID="lbPlanesPSub_3" runat="server" CssClass="nav-link" CommandArgument="3" CausesValidation="false" OnClick="btnPlanesPSub_Click">Declaratoria</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>

                                        <div class="nav-multiview">
                                            <asp:MultiView ID="mvPlanesPSub" runat="server" ActiveViewIndex="0">
                                                <%------------------------- datos urbanísticos -------------------------%>
                                                <asp:View ID="Tab1" runat="server">
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-xl-6">
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_area_bruta" class="">Área bruta</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_area_bruta" CssClass="form-control form-control-xs" MaxLength="15" onkeypress="return SoloDecimal(event);" required></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">m<sup>2</sup></span>
                                                                        </div>
                                                                    </div>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="txt_area_bruta" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_area_neta_urbanizable" class="" title="Área neta urbanizable = Área bruta - Área afectaciones">Área neta urb (redesarrollo)</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_area_neta_urbanizable" CssClass="form-control form-control-xs" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">m<sup>2</sup></span>
                                                                        </div>
                                                                    </div>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfv_area_neta_urbanizable" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="txt_area_neta_urbanizable" Enabled="false" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_area_base_calculo_cesiones" class="" title="Área base cálculo cesiones = Área neta urbanizable - Control ambiental">Área base cálculo cesiones</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_area_base_calculo_cesiones" CssClass="form-control form-control-xs" MaxLength="15" Enabled="false"></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">m<sup>2</sup></span>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_area_util" class="">Área útil</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_area_util" CssClass="form-control form-control-xs" MaxLength="15" Enabled="false"></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">m<sup>2</sup></span>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_porc_SE_total" class="">% Suelo ejecutado</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_porc_SE_total" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">%</span>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_habitantes_vivienda" class="">Habitantes / Vivienda</label>
                                                                    <asp:TextBox runat="server" ID="txt_habitantes_vivienda" CssClass="form-control form-control-xs" MaxLength="5"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="txt_habitantes_vivienda" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-xl-5">
                                                            <div class="row">
                                                                <div class="col-sm-6 col-xl-6">
                                                                    <table class="table table-sm">
                                                                        <caption>Afectaciones (m<sup>2</sup>)</caption>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td class="w-40">Malla vial arterial</td>
                                                                                <td class="">
                                                                                    <asp:TextBox runat="server" ID="txt_area_af_malla_vial_arterial" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td title="Estructura ecológica rondas">Est. eco. rondas</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_area_af_rondas" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td title="Estructura ecológica principal">Est. eco. ZMPA</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_area_af_zmpa" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Espacio público</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_area_af_espacio_publico" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Servicios públicos</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_area_af_servicios_publicos" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td title="Manejo diferenciado">Manejo diferenciado</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_area_af_manejo_dif" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="bb-0">Otro</td>
                                                                                <td class="bb-0"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_af_otro" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_area_af_otro" CssClass="form-control form-control-xs" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <th>Total</th>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_area_afectaciones" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                                <div class="col-sm-6 col-xl-6">
                                                                    <table class="table table-sm">
                                                                        <caption>Cesiones públicas (m<sup>2</sup>)</caption>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td class="w-40" title="Control ambiental">Control ambiental</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_cesion_control_ambiental" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <%--<td>
                                        <asp:TextBox runat="server" ID="txt_porc_SP_cesion_control_ambiental" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                      </td>--%>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Equipamiento</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_cesion_equipamiento" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Parques</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_cesion_parque" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Parques ZMPA</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_cesion_parque_ZMPA" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Vias locales</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_cesion_vias_locales" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Vias peatonales</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_cesion_vias_peatonales" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Zona verde</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_cesion_zona_verde" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Adicionales</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_cesion_adicionales" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Otro</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_cesion_otro" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <th>Total</th>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_cesion_total" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xl-7">
                                                            <div class="row">
                                                                <div class="col-md-6 col-xl-6">
                                                                    <div class="row">
                                                                        <div class="col-12">
                                                                            <table class="table table-sm">
                                                                                <caption>Vivienda (m<sup>2</sup>)</caption>
                                                                                <thead class="">
                                                                                    <td class="w-15"></td>
                                                                                    <th class="text-center" title="">Potencial</th>
                                                                                    <th class="text-center" title="">Ejecutado</th>
                                                                                    <th class="text-center w-20" title="% Ejecutado">% Ejec.</th>
                                                                                </thead>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td>VIP</td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_SP_VIP" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_SE_VIP" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_porc_SE_VIP" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>VIS</td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_SP_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_SE_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_porc_SE_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>No VIS</td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_SP_no_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_SE_no_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_porc_SE_no_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th>Total</th>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_SP_vivienda" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_SE_vivienda" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_porc_SE_vivienda" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </div>
                                                                        <div class="col-12">
                                                                            <table class="table table-sm">
                                                                                <caption>Vivienda (unidades)</caption>
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
                                                                                        <td>VIP</td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_unidades_potencial_VIP" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_unidades_ejecutadas_VIP" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                            <%--<asp:CompareValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe ser menor o igual al potencial" Display="Dynamic" ValidationGroup="vgPlanesP"
                                              Operator="LessThanEqual" ControlToValidate="txt_unidades_ejecutadas_VIP" ControlToCompare="txt_unidades_potencial_VIP" Type="Currency" />--%> 
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_unidades_disponibles_VIP" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>VIS</td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_unidades_potencial_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_unidades_ejecutadas_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                            <%--<asp:CompareValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe ser menor o igual al potencial" Display="Dynamic" ValidationGroup="vgPlanesP" 
                                              Operator="LessThanEqual" ControlToValidate="txt_unidades_ejecutadas_VIS" ControlToCompare="txt_unidades_potencial_VIS" Type="Currency" />--%></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_unidades_disponibles_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>No VIS</td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_unidades_potencial_no_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_unidades_ejecutadas_no_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                            <%--<asp:CompareValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe ser menor o igual al potencial" Display="Dynamic" ValidationGroup="vgPlanesP" 
                                              Operator="LessThanEqual" ControlToValidate="txt_unidades_ejecutadas_no_VIS" ControlToCompare="txt_unidades_potencial_no_VIS" Type="Currency" />--%></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_unidades_disponibles_no_VIS" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <th>Total</th>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_unidades_potencial_vivienda" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_unidades_ejecutadas_vivienda" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:TextBox runat="server" ID="txt_unidades_disponibles_vivienda" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6 col-xl-6">
                                                                    <table class="table table-sm">
                                                                        <caption>Otros usos (m<sup>2</sup>)</caption>
                                                                        <thead class="">
                                                                            <td class="w-15"></td>
                                                                            <th class="text-center" title="">Potencial</th>
                                                                            <th class="text-center" title="">Ejecutado</th>
                                                                            <th class="text-center" title="% Ejecutado">% Ejec.</th>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td title="Afectas al uso público">Afectas uso P.</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_afectas" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SE_afectas" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_porc_SE_afectas" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td title="Suelo potencial (m<sup>2</sup>)">Comercio</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_comercio" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SE_comercio" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_porc_SE_comercio" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td title="Comercio y servicios">Comercio y Servicios</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_comercio_y_servicios" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SE_comercio_y_servicios" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_porc_SE_comercio_y_servicios" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td title="">Dotacional</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_dotacional" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SE_dotacional" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_porc_SE_dotacional" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td title="">Industria</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_industria" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SE_industria" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_porc_SE_industria" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td title="Industria y servicios">Industria y Servicios</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_industria_y_servicios" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SE_industria_y_servicios" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_porc_SE_industria_y_servicios" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td title="">Servicios</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_servicios" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SE_servicios" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_porc_SE_servicios" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                            </tr>
                                                                            <%--<tr>
                                      <td title="">Multiple</td>
                                      <td>
                                        <asp:TextBox runat="server" ID="txt_SP_multiple" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                      <td>
                                        <asp:TextBox runat="server" ID="txt_SE_multiple" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                      <td>
                                        <asp:TextBox runat="server" ID="txt_porc_SE_multiple" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                    </tr>--%>
                                                                            <tr>
                                                                                <th title="">Total</th>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SP_otros_usos" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_SE_otros_usos" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_porc_SE_otros_usos" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:View>
                                                <%------------------------- zonas -------------------------%>
                                                <asp:View ID="Tab2" runat="server">
                                                    <div class="row">
                                                        <div class="form-group-sm col-md-3">
                                                            <label class="">Rango de edificabilidad</label>
                                                            <div runat="server" id="divRangoEdificabilidad" class="bgb m-1 p-2 rounded">
                                                                <asp:CheckBox runat="server" ID="chk_rango_ed_243" CssClass="d-block" Text="Rango 1" />
                                                                <asp:CheckBox runat="server" ID="chk_rango_ed_244" CssClass="d-block" Text="Rango 2" />
                                                                <asp:CheckBox runat="server" ID="chk_rango_ed_245" CssClass="d-block" Text="Rango 3" />
                                                                <asp:CheckBox runat="server" ID="chk_rango_ed_252" CssClass="d-block" Text="Rango 4A" />
                                                                <asp:CheckBox runat="server" ID="chk_rango_ed_253" CssClass="d-block" Text="Rango 4B" />
                                                                <asp:CheckBox runat="server" ID="chk_rango_ed_254" CssClass="d-block" Text="Rango 4C" />
                                                                <div class="d-none">
                                                                    <asp:TextBox runat="server" ID="txt_rango_edificabilidad" CssClass="form-control form-control-xs col-md" Visible="false"></asp:TextBox>
                                                                    <asp:TextBox runat="server" ID="txt_rango_edificabilidad_desc" CssClass="form-control form-control-xs col-md" Visible="false"></asp:TextBox>
                                                                    <asp:TextBox runat="server" ID="txt_areas_zonas" CssClass="form-control form-control-xs col-md" Visible="false"></asp:TextBox>
                                                                    <asp:TextBox runat="server" ID="txt_areas_zonas_desc" CssClass="form-control form-control-xs col-md" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div runat="server" id="divAreasZonas" class="form-group-sm col-md-9">
                                                            <label class="">Áreas de actividad / Zonas</label>
                                                            <div class="row bgb m-1 p-1 rounded">
                                                                <div class="form-group-sm col-sm">
                                                                    <label class="fwb">Residencial</label>
                                                                    <asp:CheckBox runat="server" ID="chk_az_1" CssClass="d-block" Text="Residencial neta" />
                                                                    <asp:CheckBox runat="server" ID="chk_az_2" CssClass="d-block" Text="Residencial con comercio y servicios" />
                                                                    <asp:CheckBox runat="server" ID="chk_az_3" CssClass="d-block" Text="Residencial con actividad económica" />

                                                                    <label class="fwb mt-1">Área urbana integral</label>
                                                                    <asp:CheckBox runat="server" ID="chk_az_7" CssClass="d-block" Text="Zona residencial" />
                                                                    <asp:CheckBox runat="server" ID="chk_az_8" CssClass="d-block" Text="Zona multiple" />
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
                                                </asp:View>
                                                <%------------------------- obligaciones -------------------------%>
                                                <asp:View ID="Tab3" runat="server">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <table class="table table-sm table-responsive-md">
                                                                <thead class="text-center">
                                                                    <th class="">Obligación</th>
                                                                    <th class="">Tipo vivienda</th>
                                                                    <th class="">Modalidad cumplimiento</th>
                                                                    <th class="">Área obligación (m<sup>2</sup>)</th>
                                                                    <th class="">Área ejecutada (m<sup>2</sup>)</th>
                                                                    <th class="">% Área útil</th>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td rowspan="2" class="">Suelo</td>
                                                                        <td class="">
                                                                            <asp:CheckBox runat="server" ID="chk_es_obligacion_VIP__planp" CssClass="" Text="VIP" Enabled="false" /></td>
                                                                        <td class="">
                                                                            <asp:CheckBox runat="server" ID="chk_obligacion_suelo_VIP_en_sitio" CssClass="" Text="En sitio" OnCheckedChanged="chk_obligaciones" AutoPostBack="true" />
                                                                            <asp:CheckBox runat="server" ID="chk_obligacion_suelo_VIP_traslado" CssClass="" Text="Traslado" OnCheckedChanged="chk_obligaciones" AutoPostBack="true" />
                                                                            <asp:CheckBox runat="server" ID="chk_obligacion_suelo_VIP_compensacion" CssClass="" Text="Compensación" OnCheckedChanged="chk_obligaciones" AutoPostBack="true"/>
                                                                        </td>
                                                                        <td class="">
                                                                            <asp:TextBox runat="server" ID="txt_SP_obligacion_VIP" CssClass="form-control form-control-xs" Enabled="false" /></td>
                                                                        <td class="">
                                                                            <asp:TextBox runat="server" ID="txt_SE_obligacion_VIP" CssClass="form-control form-control-xs" Enabled="false" /></td>
                                                                        <td class="">
                                                                            <asp:TextBox runat="server" ID="txt_porc_suelo_util_obligacion_VIP" CssClass="form-control form-control-xs" Enabled="false" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="">
                                                                            <asp:CheckBox runat="server" ID="chk_es_obligacion_VIS__planp" CssClass="" Text="VIS" Enabled="false" /></td>
                                                                        <td class="">
                                                                            <asp:CheckBox runat="server" ID="chk_obligacion_suelo_VIS_en_sitio" CssClass="" Text="En sitio" OnCheckedChanged="chk_obligaciones" AutoPostBack="true"/>
                                                                            <asp:CheckBox runat="server" ID="chk_obligacion_suelo_VIS_traslado" CssClass="" Text="Traslado" OnCheckedChanged="chk_obligaciones" AutoPostBack="true"/>
                                                                            <asp:CheckBox runat="server" ID="chk_obligacion_suelo_VIS_compensacion" CssClass="" Text="Compensación" OnCheckedChanged="chk_obligaciones" AutoPostBack="true"/>
                                                                        </td>
                                                                        <td class="">
                                                                            <asp:TextBox runat="server" ID="txt_SP_obligacion_VIS" CssClass="form-control form-control-xs" Enabled="false" /></td>
                                                                        <td class="">
                                                                            <asp:TextBox runat="server" ID="txt_SE_obligacion_VIS" CssClass="form-control form-control-xs" Enabled="false" /></td>
                                                                        <td class="">
                                                                            <asp:TextBox runat="server" ID="txt_porc_suelo_util_obligacion_VIS" CssClass="form-control form-control-xs" Enabled="false" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td rowspan="2" class="">Construcción</td>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="chk_es_obligacion_construccion_VIP" CssClass="" Text="VIP" AutoPostBack="true" OnCheckedChanged="chk_obligacion_construccion_VIP_CheckedChanged" /></td>
                                                                        <td class="">
                                                                            <asp:CheckBox runat="server" ID="chk_obligacion_construccion_VIP_en_sitio" CssClass="" OnCheckedChanged="chk_obligaciones" AutoPostBack="true" Text="En sitio" />
                                                                            <asp:CheckBox runat="server" ID="chk_obligacion_construccion_VIP_traslado" CssClass="" OnCheckedChanged="chk_obligaciones" AutoPostBack="true" Text="Traslado" />
                                                                            <asp:CheckBox runat="server" ID="chk_obligacion_construccion_VIP_compensacion" CssClass="" OnCheckedChanged="chk_obligaciones" AutoPostBack="true" Text="Compensación" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txt_obligacion_construccion_VIP_area" CssClass="form-control form-control-xs"/></td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txt_obligacion_construccion_VIP_area_ejecutada" CssClass="form-control form-control-xs"/></td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txt_porc_obligacion_construccion_VIP" CssClass="form-control form-control-xs"/></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="chk_es_obligacion_construccion_VIS" CssClass="" Text="VIS" AutoPostBack="true" OnCheckedChanged="chk_obligacion_construccion_VIS_CheckedChanged" /></td>
                                                                        <td class="">
                                                                            <asp:CheckBox runat="server" ID="chk_obligacion_construccion_VIS_en_sitio" CssClass="" OnCheckedChanged="chk_obligaciones" AutoPostBack="true" Text="En sitio" />
                                                                            <asp:CheckBox runat="server" ID="chk_obligacion_construccion_VIS_traslado" CssClass=""  OnCheckedChanged="chk_obligaciones" AutoPostBack="true" Text="Traslado" />
                                                                            <asp:CheckBox runat="server" ID="chk_obligacion_construccion_VIS_compensacion" CssClass="" OnCheckedChanged="chk_obligaciones" AutoPostBack="true" Text="Compensación" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txt_obligacion_construccion_VIS_area" CssClass="form-control form-control-xs" /></td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txt_obligacion_construccion_VIS_area_ejecutada" CssClass="form-control form-control-xs" /></td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txt_porc_obligacion_construccion_VIS" CssClass="form-control form-control-xs" /></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        <div class="form-group-sm col-md-6">
                                                            <label for="txt_area_base_calculo_cesiones" class="" title="">Decreto</label>
                                                            <asp:TextBox runat="server" ID="txt_decreto_obligacion" CssClass="form-control form-control-xs" TextMode="MultiLine" 
                                                                onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);"/>
                                                        </div>
                                                        <div class="form-group-sm col-md-6">
                                                            <label for="txt_area_base_calculo_cesiones" class="" title="">Artículo</label>
                                                            <asp:TextBox runat="server" ID="txt_articulo_obligacion" CssClass="form-control form-control-xs" TextMode="MultiLine"
                                                                onKeyDown="MaxLengthText(this, 200);" onKeyUp="MaxLengthText(this, 200);"/>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm">
                                                            <table class="table table-sm">
                                                                <caption>Modalidad cumplimiento</caption>
                                                                <tbody>
                                                                    <tr class="">
                                                                        <td class="w-1-12">Traslado</td>
                                                                        <td class="w-11-12">
                                                                            <div class="row">
                                                                                <div class="form-group-sm col-sm-6">
                                                                                    <label for="txt_traslado_acto_proyecto_generador" class="">Generador - licencia urbanismo</label>
                                                                                    <asp:TextBox runat="server" ID="txt_traslado_acto_proyecto_generador" CssClass="form-control form-control-xs" MaxLength="100"></asp:TextBox>
                                                                                </div>
                                                                                <div class="form-group-sm col-sm-3">
                                                                                    <label for="txt_traslado_area" class="">Traslado área</label>
                                                                                    <div class="input-group input-group-xs">
                                                                                        <asp:TextBox runat="server" ID="txt_traslado_area" CssClass="form-control form-control-xs" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                        <div class="input-group-append">
                                                                                            <span class="input-group-text">m<sup>2</sup></span>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="form-group-sm col-sm">
                                                                                    <label for="txt_traslado_acto_proyecto_receptor" class="">Receptor - licencia urbanismo</label>
                                                                                    <asp:TextBox runat="server" ID="txt_traslado_acto_proyecto_receptor" CssClass="form-control form-control-xs" MaxLength="100"></asp:TextBox>
                                                                                </div>
                                                                                <div class="form-group-sm col-sm">
                                                                                    <label for="txt_traslado_localizacion_receptor" class="">Receptor - localización</label>
                                                                                    <asp:TextBox runat="server" ID="txt_traslado_localizacion_receptor" CssClass="form-control form-control-xs" MaxLength="100"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="form-group-sm col-sm-3">
                                                                                    <asp:CheckBox runat="server" ID="chk_traslado_cumple_area_receptor" CssClass="" Text="Receptor - cumple con área de suelo" />
                                                                                </div>
                                                                                <div class="form-group-sm col-sm-3">
                                                                                    <asp:CheckBox runat="server" ID="chk_traslado_cumple_porc_receptor" CssClass="" Text="Receptor - cumple con % sobre AU" />
                                                                                </div>
                                                                                <div class="form-group-sm col-sm">
                                                                                    <asp:CheckBox runat="server" ID="chk_traslado_es_primera_etapa_receptor" CssClass="" Text="Receptor - es primera etapa de licenciamiento" />
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="w-1-12">Compensación</td>
                                                                        <td class="w-11-12">
                                                                            <div class="row">
                                                                                <div class="form-group-sm col-sm-6">
                                                                                    <label for="txt_compensacion_licencia" class="">Licencia urbanismo</label>
                                                                                    <asp:TextBox runat="server" ID="txt_compensacion_licencia" CssClass="form-control form-control-xs" MaxLength="100"></asp:TextBox>
                                                                                </div>
                                                                                <div class="form-group-sm col-sm-3">
                                                                                    <asp:CheckBox runat="server" ID="chk_compensacion_tiene_certificado_pago" CssClass="" Text="Cuenta con certificado de pago" />
                                                                                </div>
                                                                                <div class="form-group-sm col-sm">
                                                                                    <asp:CheckBox runat="server" ID="chk_compensacion_cumple_area" CssClass="" Text="Área certificada cumple con área del suelo" />
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" class="">
                                                                            <div class="row">
                                                                                <div class="form-group-sm col-sm">
                                                                                    <label for="txt_obs_modalidad_cumplimiento" class="">Observación modalidad cumplimiento</label>
                                                                                    <asp:TextBox runat="server" ID="txt_obs_modalidad_cumplimiento" CssClass="form-control form-control-xs" MaxLength="500"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </asp:View>
                                                <%------------------------- declaratoria -------------------------%>
                                                <asp:View ID="Tab4" runat="server">
                                                    <div class="row">
                                                        <div class="form-group-sm col-md-3">
                                                            <asp:CheckBox runat="server" ID="chk_es_suelo_desarrollo_prioritario" CssClass="" OnCheckedChanged="chk_es_suelo_desarrollo_prioritario_CheckedChanged" AutoPostBack="true" Text="Es suelo de desarrollo prioritario" />
                                                        </div>
                                                        <div class="form-group-sm col-md-6">
                                                            <label for="txt_decreto_declaratoria" class="">Decreto</label>
                                                            <asp:TextBox runat="server" ID="txt_decreto_declaratoria" CssClass="form-control form-control-xs"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm col-md-3">
                                                            <label for="txt_articulo_declaratoria" class="">Artículo</label>
                                                            <asp:TextBox runat="server" ID="txt_articulo_declaratoria" CssClass="form-control form-control-xs"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm col-md-3">
                                                            <label for="txt_fecha_inicio_declaratoria" class="">Fecha inicio</label>
                                                            <ajaxToolkit:CalendarExtender ID="cal_fecha_inicio_declaratoria" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_inicio_declaratoria" PopupButtonID="txt_fecha_inicio_declaratoria" Format="yyyy-MM-dd" />
                                                            <asp:TextBox runat="server" ID="txt_fecha_inicio_declaratoria" CssClass="form-control form-control-xs" TextMode="Date" Enabled="false"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm col-md-3">
                                                            <label for="txt_fecha_fin_declaratoria" class="">Fecha fin</label>
                                                            <ajaxToolkit:CalendarExtender ID="cal_fecha_fin_declaratoria" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_fin_declaratoria" PopupButtonID="txt_fecha_fin_declaratoria" Format="yyyy-MM-dd" />
                                                            <asp:TextBox runat="server" ID="txt_fecha_fin_declaratoria" CssClass="form-control form-control-xs" TextMode="Date" Enabled="false"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm col-md-3">
                                                            <label for="ddlb_id_estado_declaratoria" class="">Estado</label>
                                                            <asp:DropDownList runat="server" ID="ddlb_id_estado_declaratoria" CssClass="form-control form-control-xs" AppendDataBoundItems="true" >
                                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="form-group-sm col-md-12">
                                                            <label for="txt_observacion_declaratoria" class="">Observación declaratoria</label>
                                                            <asp:TextBox runat="server" ID="txt_observacion_declaratoria" CssClass="form-control form-control-xs" MaxLength="500"
                                                                TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </asp:View>
                                            </asp:MultiView>
                                        </div>

                                        <%--<asp:ValidationSummary runat="server" ID="vsPlanesP" DisplayMode="SingleParagraph" ShowSummary="false" HeaderText="Falta informar: " ValidationGroup="vgPlanesP" EnableClientScript="false"/>
				            <asp:RequiredFieldValidator runat="server" ID="rfv_fecha_firma_carta_intencion" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage=" {Fecha carta} "  Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="txt_fecha_firma_carta_intencion" />
				            <asp:RangeValidator runat="server" ID="rv_fecha_firma_carta_intencion" ValidationGroup="vgPlanesP" ControlToValidate="txt_fecha_firma_carta_intencion"/>
				            <asp:RangeValidator runat="server" ID="rv_fecha_iniciacion_viviendas" ValidationGroup="vgPlanesP" ControlToValidate="txt_fecha_iniciacion_viviendas"/>--%>
                                        <%--<asp:CompareValidator runat="server" ID="cv_unidades_ejecutadas_VIP" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage=" {Unidades ejecutadas VIP : El valor debe ser menor o igual a las unidades VIP} " Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="tpPlanesPAreas.txt_unidades_ejecutadas_VIP" ControlToCompare="txt_unidades_potencial_VIP" Operator="LessThanEqual" Type="Integer" />--%>
                                    </div>
                                </asp:View>
                            </asp:MultiView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPVG" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPVD" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPNavFirst" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPNavBack" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPNavNext" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPNavLast" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPEdit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPAdd" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPDel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="gvPlanesP" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <div class="card-footer">
                    <asp:UpdatePanel runat="server" ID="upPlanesPFoot" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:UpdatePanel runat="server" ID="upPlanesPMsg" UpdateMode="Conditional" class="alert-card">
                                <ContentTemplate>
                                    <div runat="server" id="msgPlanesP" class="alert d-none" role="alert"></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row">
                                <div class="col-auto va-m mr-auto">
                                    <asp:Panel ID="pPlanesPView" runat="server">
                                        <div class="btn-group" role="group">
                                            <asp:LinkButton ID="btnPlanesPVG" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnPlanesPVista_Click">
                        <i class="fas fa-border-all"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnPlanesPVD" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnPlanesPVista_Click">
                        <i class="fas fa-bars"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>

                                <div class="col-auto va-m mr-auto">
                                    <asp:Label ID="lblPlanesPCuenta" runat="server"></asp:Label>
                                </div>

                                <div class="col-auto t-c">
                                    <asp:Panel ID="pPlanesPNavegacion" runat="server">
                                        <div class="btn-group">
                                            <asp:LinkButton ID="btnPlanesPNavFirst" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnPlanesPNavegacion_Click">
											  <i class="fas fa-angle-double-left"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnPlanesPNavBack" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnPlanesPNavegacion_Click">
											  <i class="fas fa-angle-left"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnPlanesPNavNext" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnPlanesPNavegacion_Click">
											  <i class="fas fa-angle-right"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnPlanesPNavLast" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnPlanesPNavegacion_Click">
											  <i class="fas fa-angle-double-right"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>

                                <div class="col-auto ml-auto">
                                    <asp:Panel ID="pPlanesPAction" runat="server">
                                        <div class="btn-group">
                                            <asp:LinkButton ID="btnPlanesPAdd" runat="server" disabled="" CssClass="btn btn-outline-danger" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnPlanesPAccion_Click">
										    <i class="fas fa-plus"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnPlanesPEdit" runat="server" CssClass="btn btn-outline-danger" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnPlanesPAccion_Click">
										    <i class="fas fa-pencil-alt"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnPlanesPDel" runat="server" CssClass="btn btn-outline-danger" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnPlanesPAccion_Click" OnClientClick="return openModal('modPlanesP');">
										    <i class="far fa-trash-alt"></i>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="btn-group">
                                            <asp:LinkButton ID="btnPlanesPFotos" runat="server" CssClass="btn btn-outline-info" CausesValidation="false" ToolTip="Ver fotos" OnClientClick="verPlanesPFotos();">
										    <i class="far fa-image"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnPlanesPDoc1" runat="server" CssClass="btn btn-outline-success" Text="Crear ficha" CausesValidation="false" ToolTip="Crear ficha" OnClick="btnPlanesPDoc1_Click">
										    <i class="far fa-file-excel"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>

                                <div class="col-auto">
                                    <asp:Panel ID="pPlanesPExecAction" runat="server">
                                        <div class="btn-group flex-wrap">
                                            <asp:LinkButton ID="btnPlanesPCancelar" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" OnClick="btnPlanesPCancelar_Click" CausesValidation="false">
										    <i class="fas fa-times"></i>&nbspCancelar
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnPlanesPAccionFinal" runat="server" CssClass="btn btn-outline-primary" ValidationGroup="vgPlanesP"
                                                OnClientClick="if(Page_ClientValidate('vgPlanesP')) return openModal('modPlanesP');">
										    <i class="fas fa-check"></i>&nbspAceptar
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPVG" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPVD" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPAdd" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPEdit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPlanesPDel" EventName="Click" />
                            <asp:PostBackTrigger ControlID="btnPlanesPDoc1" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%--********************************************************************--%>
        <%--*************** Secciones --%>
        <%--********************************************************************--%>
        <div class="col-12">
            <asp:UpdatePanel runat="server" ID="upPlanesPSection" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card mt-3 mb-5">
                        <div class="card-header card-header-main text-center">
                            <ul runat="server" id="ulPlanesPSection" class="nav nav-pills border-bottom-0 mr-auto">
                                <li class="nav-item">
                                    <asp:LinkButton ID="lbPlanesPSection_0" runat="server" CssClass="nav-link active" CommandArgument="0" CausesValidation="false" OnClick="btnPlanesPSection_Click">Manzanas</asp:LinkButton>
                                </li>
                                <li class="nav-item">
                                    <asp:LinkButton ID="lbPlanesPSection_1" runat="server" CssClass="nav-link" CommandArgument="1" CausesValidation="false" OnClick="btnPlanesPSection_Click">Cesiones</asp:LinkButton>
                                </li>
                                <li class="nav-item">
                                    <asp:LinkButton ID="lbPlanesPSection_2" runat="server" CssClass="nav-link" CommandArgument="2" CausesValidation="false" OnClick="btnPlanesPSection_Click">Actos</asp:LinkButton>
                                </li>
                                <li class="nav-item">
                                    <asp:LinkButton ID="lbPlanesPSection_3" runat="server" CssClass="nav-link" CommandArgument="3" CausesValidation="false" OnClick="btnPlanesPSection_Click">Licencias</asp:LinkButton>
                                </li>
                                <li class="nav-item">
                                    <asp:LinkButton ID="lbPlanesPSection_4" runat="server" CssClass="nav-link" CommandArgument="4" CausesValidation="false" OnClick="btnPlanesPSection_Click">Visitas</asp:LinkButton>
                                </li>
                                <li class="nav-item">
                                    <asp:LinkButton ID="lbPlanesPSection_5" runat="server" CssClass="nav-link" CommandArgument="5" CausesValidation="false" OnClick="btnPlanesPSection_Click">Documentos</asp:LinkButton>
                                </li>
                                <li class="nav-item ml-auto va-m">
                                    <h5>
                                        <asp:Label ID="lblPlanesPSection" runat="server" CausesValidation="false" Text="" CssClass="titleSectionProyecto"></asp:Label>
                                    </h5>
                                </li>
                            </ul>
                        </div>

                        <div class="">
                            <asp:MultiView ID="mvPlanesPSection" runat="server" ActiveViewIndex="0">

                                <%--********************************************************************--%>
                                <%--*************** Manzanas --%>
                                <%--********************************************************************--%>
                                <asp:View ID="View1" runat="server">
                                    <div class="card-body">
                                        <asp:UpdatePanel runat="server" ID="upPlanesPManzanas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:MultiView runat="server" ID="mvPlanesPManzanas" ActiveViewIndex="0">
                                                    <asp:View runat="server" ID="vPlanesPManzanas">
                                                        <div class="gv-w">
                                                            <asp:GridView ID="gvPlanesPManzanas" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="au_planp_manzana" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                                AllowSorting="true" OnSorting="gvPlanesPManzanas_Sorting" OnSelectedIndexChanged="gvPlanesPManzanas_SelectedIndexChanged"
                                                                OnDataBinding="gvPlanesPManzanas_DataBinding" OnRowDataBound="gvPlanesPManzanas_RowDataBound" OnRowCreated="gv_RowCreated">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_planp_manzana" HeaderText="Cód" Visible="true" />
                                                                    <asp:BoundField DataField="unidad_gestion" HeaderText="Unidad de gestión" SortExpression="unidad_gestion" />
                                                                    <asp:BoundField DataField="manzana" HeaderText="Manzana" SortExpression="manzana" />
                                                                    <asp:BoundField DataField="uso_manzana" HeaderText="Uso" SortExpression="uso_manzana" />
                                                                    <asp:BoundField DataField="area_manzana" HeaderText="Área" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                                    <asp:BoundField DataField="porc_ejecutado" HeaderText="% Con lanzamiento" DataFormatString="{0:P0}" ItemStyle-CssClass="t-r w100" />
                                                                    <asp:BoundField DataField="fecha_fin" HeaderText="Fecha finalización" HtmlEncode="false" DataFormatString="{0:dd/MMM/yyyy}" ItemStyle-CssClass="t-c" SortExpression="fecha_fin" />

                                                                    <asp:BoundField DataField="UP_VIP" HeaderText="UP VIP" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                                    <asp:BoundField DataField="UP_VIS" HeaderText="UP VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                                    <asp:BoundField DataField="UP_no_VIS" HeaderText="UP no VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                                    <asp:BoundField DataField="UE_VIP" HeaderText="UE VIP" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                                    <asp:BoundField DataField="UE_VIS" HeaderText="UE VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                                    <asp:BoundField DataField="UE_no_VIS" HeaderText="UE no VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />

                                                                    <asp:BoundField DataField="numero_licencia" HeaderText="Licencia" />
                                                                    <asp:TemplateField HeaderText="Es obligación VIS" ItemStyle-CssClass="t-c w100">
                                                                        <ItemTemplate><%# Convert.ToString(Eval("es_obligacion_VIS")) == "1" ? "Si" : "" %></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="porc_area_obligacion_VIS" HeaderText="% área obligación VIS" DataFormatString="{0:P0}" ItemStyle-CssClass="t-r w100" />
                                                                    <asp:TemplateField HeaderText="Es obligación VIP" ItemStyle-CssClass="t-c w100">
                                                                        <ItemTemplate><%# Convert.ToString(Eval("es_obligacion_VIP")) == "1" ? "Si" : "" %></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="porc_area_obligacion_VIP" HeaderText="% área obligación VIP" DataFormatString="{0:P0}" ItemStyle-CssClass="t-r w100" />
                                                                    <asp:TemplateField HeaderText="Es obligación primera etapa" ItemStyle-CssClass="t-c w100">
                                                                        <ItemTemplate><%# Convert.ToString(Eval("es_obligacion_primera_etapa")) == "1" ? "Si" : "" %></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Es declarado" ItemStyle-CssClass="t-c w100">
                                                                        <ItemTemplate><%# Convert.ToString(Eval("es_declarado")) == "1" ? "Si" : "" %></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:View>
                                                    <asp:View runat="server" ID="vPlanesPManzanasDetalle">
                                                        <div runat="server" id="divPlanesPManzanas" class="">
                                                            <div class="row row-cols-1 row-cols-sm-2 row-cols-lg-4">
                                                                <asp:TextBox runat="server" ID="txt_au_planp_manzana" Enabled="false" Visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" ID="txt_cod_planp__manzana" Enabled="false" Visible="false"></asp:TextBox>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_unidad_gestion" class="">Unidad de gestión</label>
                                                                    <asp:TextBox runat="server" ID="txt_unidad_gestion" CssClass="form-control form-control-xs" OnTextChanged="PlanesPManzanas_Changed" MaxLength="100" required></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_manzana" class="">Manzana</label>
                                                                    <asp:TextBox runat="server" ID="txt_manzana" CssClass="form-control form-control-xs" OnTextChanged="PlanesPManzanas_Changed" MaxLength="100" required></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="txt_manzana" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="ddlb_id_uso_manzana" class="">Uso</label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_id_uso_manzana" CssClass="form-control form-control-xs" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlb_id_uso_manzana_SelectedIndexChanged">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="ddlb_id_uso_manzana" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_area_manzana" class="">Área</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_area_manzana" CssClass="form-control form-control-xs" OnTextChanged="PlanesPManzanas_Changed" MaxLength="100" required></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">m<sup>2</sup></span>
                                                                        </div>
                                                                    </div>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="txt_area_manzana" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_porc_ejecutado" class="">% Con lanzamiento</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_porc_ejecutado" CssClass="form-control" OnTextChanged="PlanesPManzanas_Changed"></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">%</span>
                                                                        </div>
                                                                    </div>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesP" ControlToValidate="txt_porc_ejecutado" />
                                                                    <asp:RangeValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe estar entre 0 y 100" Display="Dynamic" ValidationGroup="vgPlanesPManzanas" ControlToValidate="txt_porc_ejecutado" MinimumValue="0" MaximumValue="100" Type="Double" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_fecha_fin" class="">Fecha finalización</label>
                                                                    <ajaxToolkit:CalendarExtender ID="cal_fecha_fin" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_fin" PopupButtonID="txt_fecha_fin" Format="yyyy-MM-dd" />
                                                                    <asp:TextBox runat="server" ID="txt_fecha_fin" CssClass="form-control form-control-xs" TextMode="Date" OnTextChanged="PlanesPManzanas_Changed"></asp:TextBox>
                                                                    <%--<asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesPManzanas" ControlToValidate="txt_fecha_fin" />--%>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="ddlb_cod_planp_licencia" class="">Licencia</label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_cod_planp_licencia" CssClass="form-control form-control-xs" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="PlanesPManzanas_Changed">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col col-lg-9 col-xl-6">
                                                                    <table class="table table-sm">
                                                                        <caption>Vivienda (unidades)</caption>
                                                                        <thead>
                                                                            <tr>
                                                                                <td class="w-15"></td>
                                                                                <td class="text-center" title="">Potencial</td>
                                                                                <td class="text-center" title="">Con lanzamiento</td>
                                                                                <td class="text-center w-20" title="">Disponible</td>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>VIP</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_UP_VIP__manzana" OnTextChanged="PlanesPManzanas_Changed" CssClass="form-control form-control-xs"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_UE_VIP__manzana" OnTextChanged="PlanesPManzanas_Changed" CssClass="form-control form-control-xs" Enabled="true"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_UD_VIP__manzana" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>VIS</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_UP_VIS__manzana" OnTextChanged="PlanesPManzanas_Changed" CssClass="form-control form-control-xs"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_UE_VIS__manzana" OnTextChanged="PlanesPManzanas_Changed" CssClass="form-control form-control-xs" Enabled="true"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_UD_VIS__manzana" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>No VIS</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_UP_no_VIS__manzana" OnTextChanged="PlanesPManzanas_Changed" CssClass="form-control form-control-xs"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_UE_no_VIS__manzana" OnTextChanged="PlanesPManzanas_Changed" CssClass="form-control form-control-xs" Enabled="true"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_UD_no_VIS__manzana" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Total</td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_UP_vivienda__manzana" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_UE_vivienda__manzana" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txt_UD_vivienda__manzana" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>

                                                            <asp:Panel runat="server" ID="pPlanesPManzanasAreas" class="card form-group bg-light mt-2">
                                                                <div class="card-header">Áreas en uso múltiple</div>
                                                                <div class="card-body">
                                                                    <div class="row row-cols-1 row-cols-sm-2 row-cols-md-4">
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_SP_m_VIP" class="">VIP</label>
                                                                            <div class="input-group input-group-xs">
                                                                                <asp:TextBox runat="server" ID="txt_SP_m_VIP" CssClass="form-control form-control-xs t-r" OnTextChanged="PlanesPManzanas_Changed" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                <div class="input-group-append">
                                                                                    <span class="input-group-text">m<sup>2</sup></span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_SP_m_VIS" class="">VIS</label>
                                                                            <div class="input-group input-group-xs">
                                                                                <asp:TextBox runat="server" ID="txt_SP_m_VIS" CssClass="form-control form-control-xs t-r" OnTextChanged="PlanesPManzanas_Changed" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                <div class="input-group-append">
                                                                                    <span class="input-group-text">m<sup>2</sup></span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_SP_m_no_VIS" class="">No VIS</label>
                                                                            <div class="input-group input-group-xs">
                                                                                <asp:TextBox runat="server" ID="txt_SP_m_no_VIS" CssClass="form-control form-control-xs t-r" OnTextChanged="PlanesPManzanas_Changed" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                <div class="input-group-append">
                                                                                    <span class="input-group-text">m<sup>2</sup></span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_SP_m_afectas" class="">Afectas al uso público</label>
                                                                            <div class="input-group input-group-xs">
                                                                                <asp:TextBox runat="server" ID="txt_SP_m_afectas" CssClass="form-control form-control-xs t-r" OnTextChanged="PlanesPManzanas_Changed" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                <div class="input-group-append">
                                                                                    <span class="input-group-text">m<sup>2</sup></span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_SP_m_comercio" class="">Comercio</label>
                                                                            <div class="input-group input-group-xs">
                                                                                <asp:TextBox runat="server" ID="txt_SP_m_comercio" CssClass="form-control form-control-xs t-r" OnTextChanged="PlanesPManzanas_Changed" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                <div class="input-group-append">
                                                                                    <span class="input-group-text">m<sup>2</sup></span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_SP_m_comercio_y_s" class="">Comercio y Servicios</label>
                                                                            <div class="input-group input-group-xs">
                                                                                <asp:TextBox runat="server" ID="txt_SP_m_comercio_y_servicios" CssClass="form-control form-control-xs t-r" OnTextChanged="PlanesPManzanas_Changed" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                <div class="input-group-append">
                                                                                    <span class="input-group-text">m<sup>2</sup></span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_SP_m_dotacional" class="">Dotacional</label>
                                                                            <div class="input-group input-group-xs">
                                                                                <asp:TextBox runat="server" ID="txt_SP_m_dotacional" CssClass="form-control form-control-xs t-r" OnTextChanged="PlanesPManzanas_Changed" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                <div class="input-group-append">
                                                                                    <span class="input-group-text">m<sup>2</sup></span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_SP_m_industria" class="">Industria</label>
                                                                            <div class="input-group input-group-xs">
                                                                                <asp:TextBox runat="server" ID="txt_SP_m_industria" CssClass="form-control form-control-xs t-r" OnTextChanged="PlanesPManzanas_Changed" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                <div class="input-group-append">
                                                                                    <span class="input-group-text">m<sup>2</sup></span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_SP_m_industria_y_s" class="">Industria y Servicios</label>
                                                                            <div class="input-group input-group-xs">
                                                                                <asp:TextBox runat="server" ID="txt_SP_m_industria_y_servicios" CssClass="form-control form-control-xs t-r" OnTextChanged="PlanesPManzanas_Changed" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                <div class="input-group-append">
                                                                                    <span class="input-group-text">m<sup>2</sup></span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group-sm col">
                                                                            <label for="txt_SP_m_servicios" class="">Servicios</label>
                                                                            <div class="input-group input-group-xs">
                                                                                <asp:TextBox runat="server" ID="txt_SP_m_servicios" CssClass="form-control form-control-xs t-r" OnTextChanged="PlanesPManzanas_Changed" MaxLength="15" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                <div class="input-group-append">
                                                                                    <span class="input-group-text">m<sup>2</sup></span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>

                                                            <div class="row row-cols-1 row-cols-sm-2 row-cols-lg-4">
                                                                <div class="form-group-sm col va-m">
                                                                    <asp:CheckBox runat="server" ID="chk_es_obligacion_primera_etapa" CssClass="" Text="Es obligación primera etapa" AutoPostBack="true" OnCheckedChanged="PlanesPManzanas_Changed" />
                                                                </div>

                                                                <div class="form-group-sm col va-m">
                                                                    <asp:CheckBox runat="server" ID="chk_es_declarado" CssClass="" Text="Es declarado" AutoPostBack="true" OnCheckedChanged="chk_es_declarado_CheckedChanged" />
                                                                </div>
                                                            </div>

                                                            <div class="row row-cols-1 row-cols-sm-2 row-cols-md-4">
                                                                <div class="form-group-sm col va-m">
                                                                    <asp:CheckBox runat="server" ID="chk_es_obligacion_VIS" CssClass="" Text="Es obligación VIS" AutoPostBack="true" OnCheckedChanged="chk_es_obligacion_VIS_CheckedChanged" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_porc_area_obligacion_VIS" class="">% Área obligación VIS</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_porc_area_obligacion_VIS" CssClass="form-control form-control-xs" OnTextChanged="PlanesPManzanas_Changed"></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">%</span>
                                                                        </div>
                                                                    </div>
                                                                    <asp:RangeValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe estar entre 1 y 100" Display="Dynamic" ValidationGroup="vgPlanesPManzanas" ControlToValidate="txt_porc_area_obligacion_VIS" MinimumValue="1" MaximumValue="100" Type="Double" />
                                                                    <asp:RequiredFieldValidator ID="rfv_porc_area_obligacion_VIS" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesPManzanas" ControlToValidate="txt_porc_area_obligacion_VIS" />
                                                                </div>

                                                                <div class="form-group-sm col va-m">
                                                                    <asp:CheckBox runat="server" ID="chk_es_obligacion_VIP" CssClass="" Text="Es obligación VIP" AutoPostBack="true" OnCheckedChanged="chk_es_obligacion_VIP_CheckedChanged" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_porc_area_obligacion_VIP" class="">% Área obligación VIP</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_porc_area_obligacion_VIP" CssClass="form-control form-control-xs" OnTextChanged="PlanesPManzanas_Changed"></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">%</span>
                                                                        </div>
                                                                    </div>
                                                                    <asp:RangeValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe estar entre 1 y 100" Display="Dynamic" ValidationGroup="vgPlanesPManzanas" ControlToValidate="txt_porc_area_obligacion_VIP" MinimumValue="1" MaximumValue="100" Type="Double" />
                                                                    <asp:RequiredFieldValidator ID="rfv_porc_area_obligacion_VIP" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesPManzanas" ControlToValidate="txt_porc_area_obligacion_VIP" />
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_observacion__manzana" class="">Observaciones</label>
                                                                    <asp:TextBox runat="server" ID="txt_observacion__manzana" CssClass="form-control form-control-xs" OnTextChanged="PlanesPManzanas_Changed" MaxLength="500"
                                                                        TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasNavFirst" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasNavBack" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasNavNext" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasNavLast" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvPlanesPManzanas" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="card-footer">
                                        <asp:UpdatePanel runat="server" ID="upPlanesPManzanasFoot" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:UpdatePanel runat="server" ID="upPlanesPManzanasMsg" UpdateMode="Conditional" class="alert-card">
                                                    <ContentTemplate>
                                                        <div runat="server" id="msgPlanesPManzanas" class="alert d-none" role="alert"></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <div class="row">
                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Panel ID="pPlanesPManzanasView" runat="server">
                                                            <div class="btn-group" role="group">
                                                                <asp:LinkButton ID="btnPlanesPManzanasVG" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnPlanesPManzanasVista_Click">
                                    <i class="fas fa-border-all"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPManzanasVD" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnPlanesPManzanasVista_Click">
                                    <i class="fas fa-bars"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Label ID="lblPlanesPManzanasCuenta" runat="server"></asp:Label>
                                                    </div>

                                                    <div class="col-auto t-c">
                                                        <asp:Panel ID="pPlanesPManzanasNavegacion" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPManzanasNavFirst" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnPlanesPManzanasNavegacion_Click">
											              <i class="fas fa-angle-double-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPManzanasNavBack" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnPlanesPManzanasNavegacion_Click">
											              <i class="fas fa-angle-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPManzanasNavNext" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnPlanesPManzanasNavegacion_Click">
											              <i class="fas fa-angle-right"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPManzanasNavLast" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnPlanesPManzanasNavegacion_Click">
											              <i class="fas fa-angle-double-right"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto ml-auto">
                                                        <asp:Panel ID="pPlanesPManzanasAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPManzanasAdd" runat="server" disabled="" CssClass="btn btn-outline-danger" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnPlanesPManzanasAccion_Click">
										                <i class="fas fa-plus"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPManzanasEdit" runat="server" CssClass="btn btn-outline-danger" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnPlanesPManzanasAccion_Click">
										                <i class="fas fa-pencil-alt"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPManzanasDel" runat="server" CssClass="btn btn-outline-danger" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnPlanesPManzanasAccion_Click" OnClientClick="return openModal('modPlanesP');"> 
										                <i class="far fa-trash-alt"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto">
                                                        <asp:Panel ID="pPlanesPManzanasExecAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPManzanasCancelar" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" OnClick="btnPlanesPManzanasCancelar_Click" CausesValidation="false">
										                <i class="fas fa-times"></i>&nbspCancelar
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPManzanasAccionFinal" runat="server" CssClass="btn btn-outline-primary" ValidationGroup="vgPlanesPManzanas"
                                                                    OnClientClick="if(Page_ClientValidate('vgPlanesPManzanas')) return openModal('modPlanesP');">
										                <i class="fas fa-check"></i>&nbspAceptar
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPManzanasDel" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:View>

                                <%--********************************************************************--%>
                                <%--*************** Cesiones --%>
                                <%--********************************************************************--%>
                                <asp:View ID="View2" runat="server">
                                    <div class="card-body">
                                        <asp:UpdatePanel runat="server" ID="upPlanesPCesiones" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:MultiView runat="server" ID="mvPlanesPCesiones" ActiveViewIndex="0">
                                                    <asp:View runat="server" ID="vPlanesPCesiones">
                                                        <div class="gv-w">
                                                            <asp:GridView ID="gvPlanesPCesiones" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="au_planp_cesion" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                                AllowSorting="true" OnSorting="gvPlanesPCesiones_Sorting" OnSelectedIndexChanged="gvPlanesPCesiones_SelectedIndexChanged"
                                                                OnDataBinding="gvPlanesPCesiones_DataBinding" OnRowDataBound="gvPlanesPCesiones_RowDataBound" OnRowCreated="gv_RowCreated">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_planp_cesion" HeaderText="Cód" Visible="true" />
                                                                    <asp:BoundField DataField="unidad_gestion" HeaderText="Unidad de gestión" SortExpression="unidad_gestion" />
                                                                    <asp:BoundField DataField="cesion" HeaderText="Cesión" SortExpression="cesion" />
                                                                    <asp:BoundField DataField="tipo_cesion" HeaderText="Tipo" SortExpression="tipo_cesion" />
                                                                    <asp:BoundField DataField="area_cesion" HeaderText="Área" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                                    <asp:BoundField DataField="porc_area" HeaderText="% área" DataFormatString="{0:P2}" ItemStyle-CssClass="t-r w100" />
                                                                    <asp:BoundField DataField="porc_ejecutado" HeaderText="% ejecutado" DataFormatString="{0:P0}" ItemStyle-CssClass="t-r w100" />
                                                                    <asp:TemplateField HeaderText="Es suelo en sitio" ItemStyle-CssClass="t-c w100">
                                                                        <ItemTemplate><%# Convert.ToString(Eval("es_suelo_en_sitio")) == "1" ? "Si" : "" %></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Entregado a DADEP" ItemStyle-CssClass="t-c w100">
                                                                        <ItemTemplate><%# Convert.ToString(Eval("es_entregado_DADEP")) == "1" ? "Si" : "" %></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:View>
                                                    <asp:View runat="server" ID="vPlanesPCesionesDetalle">
                                                        <div runat="server" id="divPlanesPCesiones" class="">
                                                            <div class="row row-cols-1 row-cols-sm-2 row-cols-lg-4">
                                                                <asp:TextBox runat="server" ID="txt_au_planp_cesion" Enabled="false" Visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" ID="txt_cod_planp__cesiones" Enabled="false" Visible="false"></asp:TextBox>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_unidad_gestion__cesiones" class="">Unidad de gestión</label>
                                                                    <asp:TextBox runat="server" ID="txt_unidad_gestion__cesiones" CssClass="form-control form-control-xs" OnTextChanged="PlanesPCesiones_Changed" MaxLength="100"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesPCesiones" ControlToValidate="txt_unidad_gestion__cesiones" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_cesion" class="">Cesión</label>
                                                                    <asp:TextBox runat="server" ID="txt_cesion" CssClass="form-control form-control-xs" OnTextChanged="PlanesPCesiones_Changed" MaxLength="100"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesPCesiones" ControlToValidate="txt_cesion" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="ddlb_id_tipo_cesion" class="">Tipo de cesión</label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_id_tipo_cesion" CssClass="form-control form-control-xs" AppendDataBoundItems="true" OnSelectedIndexChanged="PlanesPCesiones_Changed">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesPCesiones" ControlToValidate="ddlb_id_tipo_cesion" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_porc_area" class="">% Área</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_porc_area" CssClass="form-control form-control-xs" OnTextChanged="PlanesPCesiones_Changed" onkeypress="return SoloDecimal(event);" Enabled="false" />
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">%</span>
                                                                        </div>
                                                                    </div>
                                                                    <%--<asp:RangeValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe estar entre 0 y 100" Display="Dynamic" ValidationGroup="vgPlanesPCesiones" ControlToValidate="txt_porc_area" MinimumValue="0" MaximumValue="100" Type="Double" />--%>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_area_cesion" class="">Área</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_area_cesion" CssClass="form-control form-control-xs" OnTextChanged="PlanesPCesiones_Changed" onkeypress="return SoloDecimal(event);" />
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">m<sup>2</sup></span>
                                                                        </div>
                                                                    </div>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesPCesiones" ControlToValidate="txt_area_cesion" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_porc_ejecutado__cesiones" class="">% Ejecutado</label>
                                                                    <div class="input-group input-group-xs">
                                                                        <asp:TextBox runat="server" ID="txt_porc_ejecutado__cesiones" CssClass="form-control form-control-xs" OnTextChanged="PlanesPCesiones_Changed"/>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text">%</span>
                                                                        </div>
                                                                    </div>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesPCesiones" ControlToValidate="txt_porc_ejecutado__cesiones" />
                                                                    <asp:RangeValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe estar entre 0 y 100" Display="Dynamic" ValidationGroup="vgPlanesPCesiones" ControlToValidate="txt_porc_ejecutado__cesiones" MinimumValue="0" MaximumValue="100" Type="Double" />
                                                                </div>

                                                                <div class="form-group-sm col va-m">
                                                                    <asp:CheckBox runat="server" ID="chk_es_suelo_en_sitio" CssClass="chk4" Text="Es suelo en sitio" OnCheckedChanged="PlanesPCesiones_Changed"/>
                                                                </div>

                                                                <div class="form-group-sm col va-m">
                                                                    <asp:CheckBox runat="server" ID="chk_es_entregado_DADEP" CssClass="chk4" Text="Entregado DADEP" OnCheckedChanged="PlanesPCesiones_Changed"/>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_observacion__cesiones" class="">Observaciones</label>
                                                                    <asp:TextBox runat="server" ID="txt_observacion__cesiones" CssClass="form-control form-control-xs" OnTextChanged="PlanesPCesiones_Changed" 
                                                                        TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);" MaxLength="500"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesNavFirst" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesNavBack" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesNavNext" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesNavLast" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvPlanesPCesiones" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="card-footer">
                                        <asp:UpdatePanel runat="server" ID="upPlanesPCesionesFoot" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:UpdatePanel runat="server" ID="upPlanesPCesionesMsg" UpdateMode="Conditional" class="alert-card">
                                                    <ContentTemplate>
                                                        <div runat="server" id="msgPlanesPCesiones" class="alert d-none" role="alert"></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <div class="row">
                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Panel ID="pPlanesPCesionesView" runat="server">
                                                            <div class="btn-group" role="group">
                                                                <asp:LinkButton ID="btnPlanesPCesionesVG" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnPlanesPCesionesVista_Click">
                                    <i class="fas fa-border-all"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPCesionesVD" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnPlanesPCesionesVista_Click">
                                    <i class="fas fa-bars"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Label ID="lblPlanesPCesionesCuenta" runat="server"></asp:Label>
                                                    </div>

                                                    <div class="col-auto t-c">
                                                        <asp:Panel ID="pPlanesPCesionesNavegacion" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPCesionesNavFirst" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnPlanesPCesionesNavegacion_Click">
											              <i class="fas fa-angle-double-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPCesionesNavBack" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnPlanesPCesionesNavegacion_Click">
											              <i class="fas fa-angle-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPCesionesNavNext" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnPlanesPCesionesNavegacion_Click">
											              <i class="fas fa-angle-right"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPCesionesNavLast" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnPlanesPCesionesNavegacion_Click">
											              <i class="fas fa-angle-double-right"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto ml-auto">
                                                        <asp:Panel ID="pPlanesPCesionesAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPCesionesAdd" runat="server" disabled="" CssClass="btn btn-outline-danger" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnPlanesPCesionesAccion_Click">
										                <i class="fas fa-plus"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPCesionesEdit" runat="server" CssClass="btn btn-outline-danger" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnPlanesPCesionesAccion_Click">
										                <i class="fas fa-pencil-alt"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPCesionesDel" runat="server" CssClass="btn btn-outline-danger" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnPlanesPCesionesAccion_Click" OnClientClick="return openModal('modPlanesP');"> 
										                <i class="far fa-trash-alt"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto">
                                                        <asp:Panel ID="pPlanesPCesionesExecAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPCesionesCancelar" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" OnClick="btnPlanesPCesionesCancelar_Click" CausesValidation="false">
										                <i class="fas fa-times"></i>&nbspCancelar
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPCesionesAccionFinal" runat="server" CssClass="btn btn-outline-primary" ValidationGroup="vgPlanesPCesiones"
                                                                    OnClientClick="if(Page_ClientValidate('vgPlanesPCesiones')) return openModal('modPlanesP');">
										                <i class="fas fa-check"></i>&nbspAceptar
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPCesionesDel" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:View>

                                <%--********************************************************************--%>
                                <%--*************** Actos --%>
                                <%--********************************************************************--%>
                                <asp:View ID="View3" runat="server">
                                    <div class="card-body">
                                        <asp:UpdatePanel runat="server" ID="upPlanesPActos" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:MultiView runat="server" ID="mvPlanesPActos" ActiveViewIndex="0">
                                                    <asp:View runat="server" ID="vPlanesPActos">
                                                        <div class="gv-w">
                                                            <asp:GridView ID="gvPlanesPActos" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="au_planp_acto" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                                AllowSorting="true" OnSorting="gvPlanesPActos_Sorting" OnSelectedIndexChanged="gvPlanesPActos_SelectedIndexChanged"
                                                                OnDataBinding="gvPlanesPActos_DataBinding" OnRowDataBound="gvPlanesPActos_RowDataBound" OnRowCreated="gv_RowCreated" OnRowCommand="gvPlanesPActos_RowCommand">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_planp_acto" HeaderText="au_planp_acto" Visible="false" />
                                                                    <asp:BoundField DataField="tipo_acto" HeaderText="Tipo acto" />
                                                                    <asp:BoundField DataField="numero_acto" HeaderText="Número acto" />
                                                                    <asp:BoundField DataField="fecha_acto" HeaderText="Fecha" HtmlEncode="false" DataFormatString="{0:dd/MMM/yyyy}" ItemStyle-CssClass="t-c" SortExpression="fecha_acto" />
                                                                    <asp:BoundField DataField="observacion" HeaderText="Observaciones" />
                                                                    <asp:TemplateField ShowHeader="true" HeaderText="Doc" ItemStyle-CssClass="t-c w40">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton runat="server" CommandName="OpenFile" CommandArgument='<%# Container.DisplayIndex %>' ImageUrl="~/images/icon/pdf-icon.png" Visible='<%# (String.IsNullOrEmpty(Eval("time_scan").ToString())) ? false : true %>' ToolTip="Abrir documento" />
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
                                                    <asp:View runat="server" ID="vPlanesPActosDetalle">
                                                        <div runat="server" id="divPlanesPActos" class="">
                                                            <div class="row row-cols-1 row-cols-sm-4">
                                                                <asp:TextBox runat="server" ID="txt_au_planp_acto" Enabled="false" Visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" ID="txt_cod_planp__acto" Enabled="false" Visible="false"></asp:TextBox>
                                                                <div class="form-group-sm col">
                                                                    <label for="ddlb_id_tipo_acto" class="">Tipo acto</label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_id_tipo_acto" CssClass="form-control form-control-xs" AppendDataBoundItems="true" OnTextChanged="PlanesPActos_Changed">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesPActos" ControlToValidate="ddlb_id_tipo_acto" />
                                                                </div>
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_numero_acto" class="">Número acto</label>
                                                                    <asp:TextBox runat="server" ID="txt_numero_acto" CssClass="form-control form-control-xs" OnTextChanged="PlanesPActos_Changed" MaxLength="100"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesPActos" ControlToValidate="txt_numero_acto" />
                                                                </div>
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_fecha_acto" class="">Fecha</label>
                                                                    <ajaxToolkit:CalendarExtender ID="cal_fecha_acto" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_acto" PopupButtonID="txt_fecha_acto" Format="yyyy-MM-dd" />
                                                                    <asp:TextBox runat="server" ID="txt_fecha_acto" CssClass="form-control form-control-xs" TextMode="Date" OnTextChanged="PlanesPActos_Changed"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesPActos" ControlToValidate="txt_fecha_acto" />
                                                                </div>
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_vigencia" class="">Vigencia</label>
                                                                    <ajaxToolkit:CalendarExtender ID="cal_vigencia" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_vigencia" PopupButtonID="txt_vigencia" Format="yyyy-MM-dd" />
                                                                    <asp:TextBox runat="server" ID="txt_vigencia" CssClass="form-control form-control-xs" TextMode="Date" OnTextChanged="PlanesPActos_Changed"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_observacion__acto" class="">Observaciones</label>
                                                                    <asp:TextBox runat="server" ID="txt_observacion__acto" CssClass="form-control form-control-xs" OnTextChanged="PlanesPActos_Changed" 
                                                                        TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);" MaxLength="500"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="form-group-sm col">
                                                                    <div class="btn-group">
                                                                        <asp:LinkButton ID="lb_pdf_planp_acto_doc" runat="server" CssClass="btn btn-outline-primary btn-sm" Text="" CausesValidation="false" OnClick="lb_pdf_planp_acto_doc_Click" Visible="true">
															        <i class="fas fa-file"></i>&nbspVer documento
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton ID="lb_pdf_planp_acto_delete" runat="server" CssClass="btn btn-outline-danger btn-sm" Text="" CausesValidation="false" OnClick="lb_pdf_planp_acto_delete_Click" Visible="true">
															        <i class="fas fa-trash"></i>&nbspEliminar documento
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton ID="lb_pdf_planp_acto" runat="server" CssClass="btn btn-outline-success btn-sm" Text="" CausesValidation="false" OnClick="lb_pdf_planp_acto_Click" Visible="true">
															        <i class="fas fa-upload"></i>&nbspCargar documento
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row mt-2">
                                                                <div class="form-group-sm col">
                                                                    <asp:FileUpload ID="fu_pdf_planp_acto" runat="server" CssClass="" AllowMultiple="false" Visible="true" />
                                                                    <asp:RegularExpressionValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El archivo debe ser PDF" Display="Dynamic" ValidationGroup="vgPlanesPActos" ControlToValidate="fu_pdf_planp_acto" ValidationExpression="^.*\.(p|P)(d|D)(f|F)$" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosNavFirst" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosNavBack" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosNavNext" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosNavLast" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvPlanesPActos" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="card-footer">
                                        <asp:UpdatePanel runat="server" ID="upPlanesPActosFoot" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:UpdatePanel runat="server" ID="upPlanesPActosMsg" UpdateMode="Conditional" class="alert-card">
                                                    <ContentTemplate>
                                                        <div runat="server" id="msgPlanesPActos" class="alert d-none" role="alert"></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <div class="row">
                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Panel ID="pPlanesPActosView" runat="server">
                                                            <div class="btn-group" role="group">
                                                                <asp:LinkButton ID="btnPlanesPActosVG" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnPlanesPActosVista_Click">
                                    <i class="fas fa-border-all"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPActosVD" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnPlanesPActosVista_Click">
                                    <i class="fas fa-bars"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Label ID="lblPlanesPActosCuenta" runat="server"></asp:Label>
                                                    </div>

                                                    <div class="col-auto t-c">
                                                        <asp:Panel ID="pPlanesPActosNavegacion" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPActosNavFirst" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnPlanesPActosNavegacion_Click">
											              <i class="fas fa-angle-double-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPActosNavBack" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnPlanesPActosNavegacion_Click">
											              <i class="fas fa-angle-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPActosNavNext" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnPlanesPActosNavegacion_Click">
											              <i class="fas fa-angle-right"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPActosNavLast" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnPlanesPActosNavegacion_Click">
											              <i class="fas fa-angle-double-right"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto ml-auto">
                                                        <asp:Panel ID="pPlanesPActosAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPActosAdd" runat="server" disabled="" CssClass="btn btn-outline-danger" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnPlanesPActosAccion_Click">
										                <i class="fas fa-plus"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPActosEdit" runat="server" CssClass="btn btn-outline-danger" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnPlanesPActosAccion_Click">
										                <i class="fas fa-pencil-alt"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPActosDel" runat="server" CssClass="btn btn-outline-danger" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnPlanesPActosAccion_Click" OnClientClick="return openModal('modPlanesP');"> 
										                <i class="far fa-trash-alt"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto">
                                                        <asp:Panel ID="pPlanesPActosExecAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPActosCancelar" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" OnClick="btnPlanesPActosCancelar_Click" CausesValidation="false">
										                <i class="fas fa-times"></i>&nbspCancelar
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPActosAccionFinal" runat="server" CssClass="btn btn-outline-primary" ValidationGroup="vgPlanesPActos"
                                                                    OnClientClick="if(Page_ClientValidate('vgPlanesPActos')) return openModal('modPlanesP');">
										                <i class="fas fa-check"></i>&nbspAceptar
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPActosDel" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:View>

                                <%--********************************************************************--%>
                                <%--*************** Licencias --%>
                                <%--********************************************************************--%>
                                <asp:View ID="View4" runat="server">
                                    <div class="card-body">
                                        <asp:UpdatePanel runat="server" ID="upPlanesPLicencias" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:MultiView runat="server" ID="mvPlanesPLicencias" ActiveViewIndex="0">
                                                    <asp:View runat="server" ID="vPlanesPLicencias">
                                                        <div class="gv-w">
                                                            <asp:GridView ID="gvPlanesPLicencias" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="au_planp_licencia" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                                AllowSorting="true" OnSorting="gvPlanesPLicencias_Sorting" OnSelectedIndexChanged="gvPlanesPLicencias_SelectedIndexChanged"
                                                                OnDataBinding="gvPlanesPLicencias_DataBinding" OnRowDataBound="gvPlanesPLicencias_RowDataBound" OnRowCreated="gv_RowCreated" OnRowCommand="gvPlanesPLicencias_RowCommand">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_planp_licencia" Visible="false" />
                                                                    <asp:BoundField DataField="fuente_informacion" HeaderText="Fuente información" />
                                                                    <asp:BoundField DataField="tipo_licencia" HeaderText="Tipo licencia" />
                                                                    <asp:BoundField DataField="curador" HeaderText="Curaduría" ItemStyle-CssClass="t-c" />
                                                                    <asp:BoundField DataField="numero_licencia" HeaderText="Num. licencia" />
                                                                    <asp:BoundField DataField="fecha_ejecutoria" HeaderText="Fecha ejecutoria" SortExpression="fecha_ejecutoria" HtmlEncode="false" DataFormatString="{0:dd/MMM/yyyy}" ItemStyle-CssClass="t-c" />
                                                                    <asp:BoundField DataField="termino_vigencia_meses" HeaderText="Vigencia Meses" ItemStyle-CssClass="t-c" />
                                                                    <asp:BoundField DataField="area_bruta" HeaderText="Área bruta" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                                    <asp:BoundField DataField="area_neta" HeaderText="Área neta" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                                    <asp:BoundField DataField="area_util" HeaderText="Área util" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                                    <asp:TemplateField ShowHeader="true" HeaderText="Doc" ItemStyle-CssClass="t-c w40">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton runat="server" CommandName="OpenFile" CommandArgument='<%# Container.DisplayIndex %>' ImageUrl="~/images/icon/pdf-icon.png" Visible='<%# (String.IsNullOrEmpty(Eval("time_scan").ToString())) ? false : true %>' ToolTip="Abrir documento" />
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
                                                    <asp:View runat="server" ID="vPlanesPLicenciasDetalle">
                                                        <div runat="server" id="divPlanesPLicencias" class="">
                                                            <div class="row row-cols-1 row-cols-sm-2 row-cols-lg-4">
                                                                <asp:TextBox runat="server" ID="txt_au_planp_licencia" Visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" ID="txt_cod_planp__licencia" Enabled="false" Visible="false"></asp:TextBox>

                                                                <div class="form-group-sm col">
                                                                    <label for="ddlb_id_fuente_informacion" class="">Fuente información</label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_id_fuente_informacion" CssClass="form-control form-control-xs" AppendDataBoundItems="true" OnTextChanged="PlanesPLicencias_Changed">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato obligatorio" Display="Dynamic" ValidationGroup="vgPlanesPLicencias" ControlToValidate="ddlb_id_fuente_informacion" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="ddlb_id_tipo_licencia" class="">Tipo licencia</label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_id_tipo_licencia" CssClass="form-control form-control-xs" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlb_id_tipo_licencia_SelectedIndexChanged" AutoPostBack="true">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato obligatorio" Display="Dynamic" ValidationGroup="vgPlanesPLicencias" ControlToValidate="ddlb_id_tipo_licencia" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_numero_licencia" class="">Número licencia</label>
                                                                    <asp:TextBox runat="server" ID="txt_numero_licencia" CssClass="form-control form-control-xs" MaxLength="50" OnTextChanged="PlanesPLicencias_Changed" ></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato obligatorio" Display="Dynamic" ValidationGroup="vgPlanesPLicencias" ControlToValidate="txt_numero_licencia" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_curador" class="">Curaduría</label>
                                                                    <asp:TextBox runat="server" ID="txt_curador" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="1" onkeypress="return SoloEnteroRango(event, 1, 6);"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato obligatorio" Display="Dynamic" ValidationGroup="vgPlanesPLicencias" ControlToValidate="txt_curador" />
                                                                    <asp:RangeValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe estar entre 1 y 6" Display="Dynamic" ValidationGroup="vgPlanesPLicencias" ControlToValidate="txt_curador" MinimumValue="1" MaximumValue="6" Type="Integer" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_fecha_ejecutoria" class="">Fecha ejecutoria</label>
                                                                    <ajaxToolkit:CalendarExtender ID="cal_fecha_ejecutoria" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_ejecutoria" PopupButtonID="txt_fecha_ejecutoria" Format="yyyy-MM-dd" />
                                                                    <asp:TextBox runat="server" ID="txt_fecha_ejecutoria" CssClass="form-control form-control-xs" TextMode="Date" OnTextChanged="PlanesPLicencias_Changed" ></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato obligatorio" Display="Dynamic" ValidationGroup="vgPlanesPLicencias" ControlToValidate="txt_fecha_ejecutoria" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_termino_vigencia_meses" class="">Vigencia meses</label>
                                                                    <asp:TextBox runat="server" ID="txt_termino_vigencia_meses" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="3" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato obligatorio" Display="Dynamic" ValidationGroup="vgPlanesPLicencias" ControlToValidate="txt_termino_vigencia_meses" />
                                                                </div>

                                                                <div class="form-group-sm col col-sm-12 col-lg-6">
                                                                    <label for="txt_nombre_proyecto" class="">Nombre proyecto</label>
                                                                    <asp:TextBox runat="server" ID="txt_nombre_proyecto" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" ></asp:TextBox>
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
                                                                                            <asp:TextBox runat="server" ID="txt_plano_urbanistico_aprobado" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" ></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-sm-6 col-lg-12">
                                                                                        <div class="form-group-sm">
                                                                                            <label for="txt_porc_ejecucion_urbanismo" class="">% Ejecutado</label>
                                                                                            <div class="input-group input-group-xs">
                                                                                                <asp:TextBox runat="server" ID="txt_porc_ejecucion_urbanismo" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                <div class="input-group-append">
                                                                                                    <span class="input-group-text">%</span>
                                                                                                </div>
                                                                                            </div>
                                                                                            <asp:RangeValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe estar entre 0 y 100" Display="Dynamic" ValidationGroup="vgPlanesPLicencias" ControlToValidate="txt_porc_ejecucion_urbanismo" MinimumValue="0" MaximumValue="100" Type="Double" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-12 col-sm-6 col-lg-12">
                                                                                        <table class="table table-sm">
                                                                                            <caption>Datos urbanísticos (m<sup>2</sup>)</caption>
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td class="w-40">Área bruta</td>
                                                                                                    <td class="">
                                                                                                        <asp:TextBox runat="server" ID="txt_area_bruta__licencia" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td title="Estructura ecológica rondas">Área neta</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_neta" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td title="Estructura ecológica principal">Área útil</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_util__licencia" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
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
                                                                                                        <asp:TextBox runat="server" ID="txt_area_cesion_zonas_verdes" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Vías</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_cesion_vias" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td title="Equipamiento comunal">Eq. comunal</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_cesion_eq_comunal" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
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
                                                                                            <asp:DropDownList runat="server" ID="ddlb_id_obligacion_VIS" CssClass="form-control form-control-xs" AppendDataBoundItems="true" OnTextChanged="PlanesPLicencias_Changed" AutoPostBack="true">
                                                                                                <%--OnSelectedIndexChanged="ddlb_id_obligacion_VIS_SelectedIndexChanged"--%>
                                                                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-sm-6">
                                                                                        <div class="form-group-sm">
                                                                                            <label for="ddlb_id_obligacion_VIP" class="">Obligación VIP</label>
                                                                                            <asp:DropDownList runat="server" ID="ddlb_id_obligacion_VIP" CssClass="form-control form-control-xs" AppendDataBoundItems="true" OnTextChanged="PlanesPLicencias_Changed" AutoPostBack="true">
                                                                                                <%--OnSelectedIndexChanged="ddlb_id_obligacion_VIP_SelectedIndexChanged"--%>
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
                                                                                                <td class="">% Obligación</td>
                                                                                                <td class="">Unidades</td>
                                                                                            </thead>
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td>VIP</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_terreno_VIP" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_construida_VIP" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_porc_obligacion_VIP" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                        <asp:RangeValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe estar entre 0 y 100" Display="Dynamic" ValidationGroup="vgPlanesPLicencias" ControlToValidate="txt_porc_obligacion_VIP" MinimumValue="0" MaximumValue="100" Type="Double" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_unidades_vivienda_VIP" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="5" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>VIS</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_terreno_VIS" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_construida_VIS" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_porc_obligacion_VIS" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                        <asp:RangeValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe estar entre 0 y 100" Display="Dynamic" ValidationGroup="vgPlanesPLicencias" ControlToValidate="txt_porc_obligacion_VIS" MinimumValue="0" MaximumValue="100" Type="Double" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_unidades_vivienda_VIS" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="5" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>No VIS</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_terreno_no_VIS" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_construida_no_VIS" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_unidades_vivienda_no_VIS" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="5" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Comercio</td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_comercio" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Oficina</td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_oficina" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Institucional</td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_institucional" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Industria</td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_industria" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
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
                                                                                                        <asp:TextBox runat="server" ID="txt_area_lote" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Sótano</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_sotano" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Semisótano</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_semisotano" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Primer piso</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_primer_piso" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Pisos restantes</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_pisos_restantes" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Construida total</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_construida_total" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="365" Enabled="true" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Libre primer piso</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" ID="txt_area_libre_primer_piso" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="10" Enabled="true" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>% Ejecutado</td>
                                                                                                    <td>
                                                                                                        <div class="input-group input-group-xs">
                                                                                                            <asp:TextBox runat="server" ID="txt_porc_ejecucion_construccion" CssClass="form-control form-control-xs" OnTextChanged="PlanesPLicencias_Changed" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                                                            <div class="input-group-append">
                                                                                                                <span class="input-group-text">%</span>
                                                                                                            </div>
                                                                                                            <asp:RangeValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El valor debe estar entre 0 y 100" Display="Dynamic" ValidationGroup="vgPlanesPLicencias" ControlToValidate="txt_porc_ejecucion_construccion" MinimumValue="0" MaximumValue="100" Type="Double" />
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
                                                                <div class="form-group-sm col col-sm-6 col-lg-3 va-m">
                                                                    <asp:CheckBox runat="server" ID="chk_cumple_area_obligacion" Text="Cumple con área de obligación" CssClass="" />
                                                                </div>

                                                                <div class="form-group-sm col col-sm-6 col-lg-3 va-m">
                                                                    <asp:CheckBox runat="server" ID="chk_cumple_porc_area_util" Text="Cumple con % sobre área útil" CssClass="" />
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="form-group-sm col">
                                                                    <label for="txt_observacion__acto" class="">Observaciones</label>
                                                                    <asp:TextBox runat="server" ID="txt_observacion__licencia" CssClass="form-control form-control-xs" 
                                                                        TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);" MaxLength="500"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="form-group-sm col">
                                                                    <div class="btn-group">
                                                                        <asp:LinkButton ID="lb_pdf_planp_licencia_doc" runat="server" CssClass="btn btn-outline-primary btn-sm" Text="" CausesValidation="false" OnClick="lb_pdf_planp_licencia_doc_Click" Visible="true">
															        <i class="fas fa-file"></i>&nbspVer documento
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton ID="lb_pdf_planp_licencia_delete" runat="server" CssClass="btn btn-outline-danger btn-sm" Text="" CausesValidation="false" OnClick="lb_pdf_planp_licencia_delete_Click" Visible="true">
															        <i class="fas fa-trash"></i>&nbspEliminar documento
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton ID="lb_pdf_planp_licencia" runat="server" CssClass="btn btn-outline-success btn-sm" Text="" CausesValidation="false" OnClick="lb_pdf_planp_licencia_Click" Visible="true">
															        <i class="fas fa-upload"></i>&nbspCargar documento
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row mt-2">
                                                                <div class="form-group-sm col">
                                                                    <asp:FileUpload ID="fu_pdf_planp_licencia" runat="server" CssClass="" AllowMultiple="false" Visible="true" />
                                                                    <asp:RegularExpressionValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="El archivo debe ser PDF" Display="Dynamic" ValidationGroup="vgPlanesPLicencias" ControlToValidate="fu_pdf_planp_licencia" ValidationExpression="^.*\.(p|P)(d|D)(f|F)$" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasNavFirst" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasNavBack" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasNavNext" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasNavLast" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvPlanesPLicencias" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="card-footer">
                                        <asp:UpdatePanel runat="server" ID="upPlanesPLicenciasFoot" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:UpdatePanel runat="server" ID="upPlanesPLicenciasMsg" UpdateMode="Conditional" class="alert-card">
                                                    <ContentTemplate>
                                                        <div runat="server" id="msgPlanesPLicencias" class="alert d-none" role="alert"></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <div class="row">
                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Panel ID="pPlanesPLicenciasView" runat="server">
                                                            <div class="btn-group" role="group">
                                                                <asp:LinkButton ID="btnPlanesPLicenciasVG" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnPlanesPLicenciasVista_Click">
                                    <i class="fas fa-border-all"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPLicenciasVD" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnPlanesPLicenciasVista_Click">
                                    <i class="fas fa-bars"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Label ID="lblPlanesPLicenciasCuenta" runat="server"></asp:Label>
                                                    </div>

                                                    <div class="col-auto t-c">
                                                        <asp:Panel ID="pPlanesPLicenciasNavegacion" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPLicenciasNavFirst" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnPlanesPLicenciasNavegacion_Click">
											              <i class="fas fa-angle-double-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPLicenciasNavBack" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnPlanesPLicenciasNavegacion_Click">
											              <i class="fas fa-angle-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPLicenciasNavNext" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnPlanesPLicenciasNavegacion_Click">
											              <i class="fas fa-angle-right"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPLicenciasNavLast" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnPlanesPLicenciasNavegacion_Click">
											              <i class="fas fa-angle-double-right"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto ml-auto">
                                                        <asp:Panel ID="pPlanesPLicenciasAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPLicenciasAdd" runat="server" disabled="" CssClass="btn btn-outline-danger" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnPlanesPLicenciasAccion_Click">
										                <i class="fas fa-plus"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPLicenciasEdit" runat="server" CssClass="btn btn-outline-danger" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnPlanesPLicenciasAccion_Click">
										                <i class="fas fa-pencil-alt"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPLicenciasDel" runat="server" CssClass="btn btn-outline-danger" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnPlanesPLicenciasAccion_Click" OnClientClick="return openModal('modPlanesP');"> 
										                <i class="far fa-trash-alt"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto">
                                                        <asp:Panel ID="pPlanesPLicenciasExecAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPLicenciasCancelar" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" OnClick="btnPlanesPLicenciasCancelar_Click" CausesValidation="false">
										                <i class="fas fa-times"></i>&nbspCancelar
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPLicenciasAccionFinal" runat="server" CssClass="btn btn-outline-primary" ValidationGroup="vgPlanesPLicencias"
                                                                    OnClientClick="if(Page_ClientValidate('vgPlanesPLicencias')) return openModal('modPlanesP');">
										                <i class="fas fa-check"></i>&nbspAceptar
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPLicenciasDel" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:View>
                                
                                <%--********************************************************************--%>
                                <%--*************** Visitas --%>
                                <%--********************************************************************--%>
                                <asp:View ID="View5" runat="server">
                                    <div class="card-body">
                                        <asp:UpdatePanel runat="server" ID="upPlanesPVisitas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:MultiView runat="server" ID="mvPlanesPVisitas" ActiveViewIndex="0">
                                                    <asp:View runat="server" ID="vPlanesPVisitas">
                                                        <div class="gv-w">
                                                            <asp:GridView ID="gvPlanesPVisitas" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="au_planp_visita" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                                AllowSorting="true" OnSorting="gvPlanesPVisitas_Sorting" OnSelectedIndexChanged="gvPlanesPVisitas_SelectedIndexChanged"
                                                                OnDataBinding="gvPlanesPVisitas_DataBinding" OnRowDataBound="gvPlanesPVisitas_RowDataBound" OnRowCreated="gv_RowCreated">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_planp_visita" HeaderText="au_planp_visita" Visible="false" />
                                                                    <asp:BoundField DataField="fecha_visita" HeaderText="Fecha visita" HtmlEncode="false" DataFormatString="{0:dd/MMM/yyyy}" ItemStyle-CssClass="t-c w120" SortExpression="fecha_visita" />
                                                                    <asp:BoundField DataField="observacion" HeaderText="Observaciones" />
                                                                    <asp:BoundField DataField="usu" HeaderText="Usuario" ItemStyle-CssClass="w200" />
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:View>
                                                    <asp:View runat="server" ID="vPlanesPVisitasDetalle">
                                                        <div runat="server" id="divPlanesPVisitas" class="">
                                                            <div class="row row-cols-1">
                                                                <asp:TextBox runat="server" ID="txt_au_planp_visita" Enabled="false" Visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" ID="txt_cod_planp__visita" Enabled="false" Visible="false"></asp:TextBox>

                                                                <div class="form-group-sm col col-sm-3">
                                                                    <label for="txt_fecha_visita" class="">Fecha visita</label>
                                                                    <ajaxToolkit:CalendarExtender ID="cal_fecha_visita" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_visita" PopupButtonID="txt_fecha_visita" Format="yyyy-MM-dd" />
                                                                    <asp:TextBox runat="server" ID="txt_fecha_visita" CssClass="form-control form-control-xs" TextMode="Date" OnTextChanged="PlanesPVisitas_Changed" ></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ErrorMessage="Dato requerido" Display="Dynamic" ValidationGroup="vgPlanesPVisitas" ControlToValidate="txt_fecha_visita" />
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <label for="txt_observacion__visita" class="">Observaciones</label>
                                                                    <asp:TextBox runat="server" ID="txt_observacion__visita" CssClass="form-control form-control-xs" OnTextChanged="PlanesPVisitas_Changed"
                                                                        TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);" MaxLength="500"></asp:TextBox>
                                                                </div>

                                                                <div class="form-group-sm col">
                                                                    <asp:LinkButton ID="lb_PlanesPVisitasFotos" runat="server" CssClass="btn btn-outline-success" Text="Cargar imágenes" CausesValidation="false" OnClientClick="verPlanesPVisitasFotos();">
																    <i class="fas fa-upload"></i>&nbspCargar imágenes
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasNavFirst" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasNavBack" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasNavNext" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasNavLast" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvPlanesPVisitas" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="card-footer">
                                        <asp:UpdatePanel runat="server" ID="upPlanesPVisitasFoot" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:UpdatePanel runat="server" ID="upPlanesPVisitasMsg" UpdateMode="Conditional" class="alert-card">
                                                    <ContentTemplate>
                                                        <div runat="server" id="msgPlanesPVisitas" class="alert d-none" role="alert"></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <div class="row">
                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Panel ID="pPlanesPVisitasView" runat="server">
                                                            <div class="btn-group" role="group">
                                                                <asp:LinkButton ID="btnPlanesPVisitasVG" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnPlanesPVisitasVista_Click">
                                    <i class="fas fa-border-all"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPVisitasVD" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnPlanesPVisitasVista_Click">
                                    <i class="fas fa-bars"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto va-m mr-auto">
                                                        <asp:Label ID="lblPlanesPVisitasCuenta" runat="server"></asp:Label>
                                                    </div>

                                                    <div class="col-auto t-c">
                                                        <asp:Panel ID="pPlanesPVisitasNavegacion" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPVisitasNavFirst" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnPlanesPVisitasNavegacion_Click">
											              <i class="fas fa-angle-double-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPVisitasNavBack" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnPlanesPVisitasNavegacion_Click">
											              <i class="fas fa-angle-left"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPVisitasNavNext" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnPlanesPVisitasNavegacion_Click">
											              <i class="fas fa-angle-right"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPVisitasNavLast" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnPlanesPVisitasNavegacion_Click">
											              <i class="fas fa-angle-double-right"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto ml-auto">
                                                        <asp:Panel ID="pPlanesPVisitasAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPVisitasAdd" runat="server" disabled="" CssClass="btn btn-outline-danger" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnPlanesPVisitasAccion_Click">
										                <i class="fas fa-plus"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPVisitasEdit" runat="server" CssClass="btn btn-outline-danger" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnPlanesPVisitasAccion_Click">
										                <i class="fas fa-pencil-alt"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPVisitasDel" runat="server" CssClass="btn btn-outline-danger" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnPlanesPVisitasAccion_Click" OnClientClick="return openModal('modPlanesP');"> 
										                <i class="far fa-trash-alt"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPVisitasFotos" runat="server" CssClass="btn btn-outline-info" CausesValidation="false" ToolTip="Ver fotos" OnClientClick="verPlanesPVisitasFotos();">
														        <i class="far fa-image"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div class="col-auto">
                                                        <asp:Panel ID="pPlanesPVisitasExecAction" runat="server">
                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btnPlanesPVisitasCancelar" runat="server" CssClass="btn btn-outline-secondary" CommandArgument="0" OnClick="btnPlanesPVisitasCancelar_Click" CausesValidation="false">
										                <i class="fas fa-times"></i>&nbspCancelar
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnPlanesPVisitasAccionFinal" runat="server" CssClass="btn btn-outline-primary" ValidationGroup="vgPlanesPVisitas"
                                                                    OnClientClick="if(Page_ClientValidate('vgPlanesPVisitas')) return openModal('modPlanesP');">
										                <i class="fas fa-check"></i>&nbspAceptar
                                                                </asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPlanesPVisitasDel" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:View>
                                <%--********************************************************************--%>
                                <%--*************** Documentos *****************************************--%>
                                <%--********************************************************************--%>
                                <asp:View ID="View6" runat="server">
                                    <div class="card-user-control">
                                        <asp:UpdatePanel runat="server" ID="upPlanesPFileUpload" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hdd_idactor" runat="server" Value="0" />
                                                <uc:FileUpload ID="ucFileUpload" runat="server" ControlID="PlanParcial.Documentos" Extensions="pdf" OnUserControlException="ucFileUpload_UserControlException" 
                                                    OnViewDoc="ucFileUpload_ViewDoc" Multiple="True"/>
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
    </div>

    <%--********************************************************************--%>
    <%--*************** Scripts --%>
    <%--********************************************************************--%>
    <script type="text/javascript">
        function pageLoad() {
            //*****************************************ESTILO GRIDVIEWS
            $('#<%=gvPlanesP.ClientID%>').gridviewScroll({
                height: 350,
                freezesize: 2,
                startVertical: $("#<%=hfGVPlanesPSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGVPlanesPSH.ClientID%>").val(),
                onScrollVertical: function (delta) { $("#<%=hfGVPlanesPSV.ClientID%>").val(delta); },
                onScrollHorizontal: function (delta) { $("#<%=hfGVPlanesPSH.ClientID%>").val(delta); }
            });

            $('#<%=gvPlanesPManzanas.ClientID%>').gridviewScroll({
                height: 300,
                freezesize: 1,
                startVertical: $("#<%=hfGVPlanesPManzanasSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGVPlanesPManzanasSH.ClientID%>").val(),
                onScrollVertical: function (delta) { $("#<%=hfGVPlanesPManzanasSV.ClientID%>").val(delta); },
                onScrollHorizontal: function (delta) { $("#<%=hfGVPlanesPManzanasSH.ClientID%>").val(delta); }
            });

            $('#<%=gvPlanesPCesiones.ClientID%>').gridviewScroll({
                height: 300,
                freezesize: 1,
                startVertical: $("#<%=hfGVPlanesPCesionesSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGVPlanesPCesionesSH.ClientID%>").val(),
                onScrollVertical: function (delta) { $("#<%=hfGVPlanesPCesionesSV.ClientID%>").val(delta); },
                onScrollHorizontal: function (delta) { $("#<%=hfGVPlanesPCesionesSH.ClientID%>").val(delta); }
            });

            $('#<%=gvPlanesPActos.ClientID%>').gridviewScroll({
                height: 300,
                freezesize: 1,
                startVertical: $("#<%=hfGVPlanesPActosSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGVPlanesPActosSH.ClientID%>").val(),
                onScrollVertical: function (delta) { $("#<%=hfGVPlanesPActosSV.ClientID%>").val(delta); },
                onScrollHorizontal: function (delta) { $("#<%=hfGVPlanesPActosSH.ClientID%>").val(delta); }
            });

            $('#<%=gvPlanesPLicencias.ClientID%>').gridviewScroll({
                height: 300,
                freezesize: 1,
                startVertical: $("#<%=hfGVPlanesPLicenciasSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGVPlanesPLicenciasSH.ClientID%>").val(),
                onScrollVertical: function (delta) { $("#<%=hfGVPlanesPLicenciasSV.ClientID%>").val(delta); },
                onScrollHorizontal: function (delta) { $("#<%=hfGVPlanesPLicenciasSH.ClientID%>").val(delta); }
            });

            $('#<%=gvPlanesPVisitas.ClientID%>').gridviewScroll({
                height: 300,
                freezesize: 1,
                startVertical: $("#<%=hfGVPlanesPVisitasSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGVPlanesPVisitasSH.ClientID%>").val(),
                onScrollVertical: function (delta) { $("#<%=hfGVPlanesPVisitasSV.ClientID%>").val(delta); },
                onScrollHorizontal: function (delta) { $("#<%=hfGVPlanesPVisitasSH.ClientID%>").val(delta); }
            });
        }

        function verPlanesPFotos() {
            var width = 910;
            var height = 700;
            var left = (screen.width / 2) - (width / 2);
            var top = (screen.height / 2) - (height / 2);
            return window.open("PlanesPFotos.aspx", "PlanesPFotos", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + width + ', height=' + height + ', top=' + top + ', left=' + left);
        }

        function verPlanesPVisitasFotos() {
            var width = 1200;
            var height = 480;
            var left = (screen.width / 2) - (width / 2);
            var top = (screen.height / 2) - (height / 2);
            return window.open("PlanesPVisitasFotos.aspx", "PlanesPVisitasFotos", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + width + ', height=' + height + ', top=' + top + ', left=' + left);
        }
    </script>
</asp:Content>
