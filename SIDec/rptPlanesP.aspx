<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" AutoEventWireup="true" CodeBehind="rptPlanesP.aspx.cs" Inherits="SIDec.rptPlanesP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

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
    <%--*************** gridviews --%>
    <%--********************************************************************--%>
    <asp:HiddenField ID="hfGVReporte_0SV" runat="server" />
    <asp:HiddenField ID="hfGVReporte_0SH" runat="server" />
    <asp:HiddenField ID="hfGVReporte_1SV" runat="server" />
    <asp:HiddenField ID="hfGVReporte_1SH" runat="server" />
    <asp:HiddenField ID="hfGVReporte_2SV" runat="server" />
    <asp:HiddenField ID="hfGVReporte_2SH" runat="server" />
    <asp:HiddenField ID="hfGVReporte_2_1SV" runat="server" />
    <asp:HiddenField ID="hfGVReporte_2_1SH" runat="server" />
    <asp:HiddenField ID="hfGVReporte_2_2SV" runat="server" />
    <asp:HiddenField ID="hfGVReporte_2_2SH" runat="server" />

    <div class="">
        <div class="card-header bg-light text-primary">
            <h3>Reportes Planes Parciales</h3>
        </div>
    </div>

    <%--********************************************************************--%>
    <%--*************** Lista --%>
    <%--********************************************************************--%>
    <asp:UpdatePanel runat="server" ID="upReportes" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="col-12">
                <ul class="nav nav-pills " runat="server" id="ulReportes">
                    <li class="nav-item">
                        <asp:LinkButton ID="lbReporte_0" runat="server" CssClass="nav-link active" CommandArgument="0" CausesValidation="false" OnClick="btnReporte_Click">General</asp:LinkButton>
                    </li>
                    <li class="nav-item">
                        <asp:LinkButton ID="lbReporte_1" runat="server" CssClass="nav-link" CommandArgument="1" CausesValidation="false" OnClick="btnReporte_Click">Cesiones</asp:LinkButton>
                    </li>
                    <li class="nav-item">
                        <asp:LinkButton ID="lbReporte_2" runat="server" CssClass="nav-link" CommandArgument="2" CausesValidation="false" OnClick="btnReporte_Click">Indicadores</asp:LinkButton>
                    </li>
                </ul>

                <div class="card mt-3">
                    <div class="card-body">
                        <div class="row">
                            <div class="form-group form-group-sm col-sm-auto">
                                <label for="ddlb_id_tipo_tratamiento" class="">Tipo de Tratamiento</label>
                                <asp:DropDownList runat="server" ID="ddlb_id_tipo_tratamiento" CssClass="form-control form-control-sm" placeholder="Filtro por tipo" AppendDataBoundItems="true" TabIndex="1">
                                    <asp:ListItem Value="">Todos</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <asp:Panel runat="server" ID="pParam2" class="form-group form-group-sm col-sm-auto">
                                <label for="ddlb_rango_porc_ejecutado" class="">Rango de Ejecución</label>
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text">Entre</span>
                                    </div>
                                    <asp:DropDownList runat="server" ID="ddlb_rango_1" CssClass="form-control form-control-sm" AppendDataBoundItems="true" TabIndex="5"></asp:DropDownList>
                                    <asp:DropDownList runat="server" ID="ddlb_rango_2" CssClass="form-control form-control-sm" AppendDataBoundItems="true" TabIndex="10"></asp:DropDownList>
                                </div>
                            </asp:Panel>

                            <asp:Panel runat="server" ID="pParam3" class="form-group form-group-sm col-sm-auto">
                                <label for="ddlb_rango_ano" class="">Año Adopción</label>
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text">Entre</span>
                                    </div>
                                    <asp:DropDownList runat="server" ID="ddlb_ano_1" CssClass="form-control form-control-sm" AppendDataBoundItems="true" TabIndex="15"></asp:DropDownList>
                                    <asp:DropDownList runat="server" ID="ddlb_ano_2" CssClass="form-control form-control-sm" AppendDataBoundItems="true" TabIndex="20"></asp:DropDownList>
                                </div>
                            </asp:Panel>

                            <div class="form-group form-group-sm col-sm-auto va-e">
                                <asp:UpdatePanel ID="pnlEjecutar" runat="server" DefaultButton="btnEjecutar">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="btnEjecutar" runat="server" CssClass="btn btn-outline-primary" Text="Exportar" CausesValidation="false" ToolTip="" OnClick="btnEjecutar_Click">
                      <i class="fas fa-play"></i>&nbsp;&nbsp;Ejecutar
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnExportar" runat="server" CssClass="btn btn-outline-success" Text="Exportar" CausesValidation="false" ToolTip="" OnClick="btnExportar_Click">
                        <i class="far fa-file-excel"></i>&nbsp;&nbsp;Exportar
                                        </asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdateProgress ID="pnlLoading" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="pnlEjecutar">
                                    <ProgressTemplate>
                                        <asp:Image runat="server" CssClass="imgCargando" ImageUrl="~/images/icon/cargando.gif" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="mt-3">
                    <asp:MultiView ID="mvReportes" runat="server" ActiveViewIndex="0">
                        <%--********************************************************************--%>
                        <%--*************** General --%>
                        <%--********************************************************************--%>
                        <asp:View ID="View1" runat="server">
                            <asp:UpdatePanel runat="server" ID="upReporte" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="card">
                                        <div class="card-header text-primary">
                                            <h4>General</h4>
                                        </div>
                                        <div class="card-body">
                                            <div class="gv-w">
                                                <asp:GridView ID="gvReporte_0" CssClass="gv" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" AllowPaging="false">
                                                    <Columns>
                                                        <asp:BoundField DataField="au_planp" HeaderText="No Plan Parcial" DataFormatString="{0:N0}" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="nombre_planp" HeaderText="Nombre" ItemStyle-Width="150" />
                                                        <asp:BoundField DataField="tipo_tratamiento" HeaderText="Tipo tratamiento" />
                                                        <asp:BoundField DataField="porc_SE_total" HeaderText="% Suelo ejecutado total" DataFormatString="{0:P0}" SortExpression="porc_SE_total" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="ano_adopcion" HeaderText="Año adopción" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="localidad" HeaderText="Localidad" />
                                                        <asp:BoundField DataField="area_bruta" HeaderText="Área bruta" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="area_neta_urbanizable" HeaderText="Área neta" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="area_util" HeaderText="Área útil" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="area_afectaciones" HeaderText="Área afectaciones" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="SP_cesion_total" HeaderText="Área cesiones" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="unidades_potencial_VIP" HeaderText="Unidades potencial VIP" DataFormatString="{0:N0}" ItemStyle-CssClass="w100 t-r" />
                                                        <asp:BoundField DataField="unidades_potencial_VIS" HeaderText="Unidades potencial VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="w100 t-r" />
                                                        <asp:BoundField DataField="unidades_potencial_no_VIS" HeaderText="Unidades potencial no VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="w100 t-r" />
                                                        <asp:BoundField DataField="unidades_potencial_vivienda" HeaderText="Unidades potencial vivienda" DataFormatString="{0:N0}" ItemStyle-CssClass="w100 t-r" />
                                                        <asp:BoundField DataField="unidades_ejecutadas_VIP" HeaderText="Unidades ejecutadas VIP" DataFormatString="{0:N0}" ItemStyle-CssClass="w100 t-r" />
                                                        <asp:BoundField DataField="unidades_ejecutadas_VIS" HeaderText="Unidades ejecutadas VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="w100 t-r" />
                                                        <asp:BoundField DataField="unidades_ejecutadas_no_VIS" HeaderText="Unidades ejecutadas no VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="w100 t-r" />
                                                        <asp:BoundField DataField="unidades_ejecutadas_vivienda" HeaderText="Unidades ejecutadas vivienda" DataFormatString="{0:N0}" ItemStyle-CssClass="w100 t-r" />

                                                    </Columns>
                                                    <SelectedRowStyle CssClass="gvItemSelected" />
                                                    <HeaderStyle CssClass="gvHeader" />
                                                    <RowStyle CssClass="gvItem" />
                                                    <PagerStyle CssClass="gvPager" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <div class="card-footer mt-2">
                                            <div class="row">
                                                <div class="col-auto va-m mr-auto">
                                                    <asp:Label runat="server" ID="lbl_total_0" CssClass=""></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnEjecutar" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:View>
                        <%--********************************************************************--%>
                        <%--*************** Cesiones --%>
                        <%--********************************************************************--%>
                        <asp:View ID="View2" runat="server">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="card">
                                        <div class="card-header text-primary">
                                            <h4>Cesiones</h4>
                                        </div>
                                        <div class="card-body">
                                            <div class="gv-w">
                                                <asp:GridView ID="gvReporte_1" CssClass="gv" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" AllowPaging="false">
                                                    <Columns>
                                                        <asp:BoundField DataField="nombre_planp" HeaderText="Nombre" ItemStyle-Width="150" />
                                                        <asp:BoundField DataField="area_bruta" HeaderText="Área bruta" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="unidades_ejecutadas_vivienda" HeaderText="Unidades ejecutadas vivienda" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="SD_cesion_control_ambiental" HeaderText="Cesión Disponible - Control Ambiental" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="SD_cesion_equipamiento" HeaderText="Cesión Disponible - Equipamiento" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="SD_cesion_parque" HeaderText="Cesión Disponible - Parque" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="SD_cesion_vias_locales" HeaderText="Cesión Disponible - Vías Locales" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="SD_cesion_subtotal" HeaderText="Cesión Disponible - Subtotal" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="porc_SE_cesion_subtotal" HeaderText="% SE Cesión - Subtotal" DataFormatString="{0:P2}" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_SE_total" HeaderText="% Suelo Ejecutado Total" DataFormatString="{0:P0}" SortExpression="porc_SE_total" ItemStyle-CssClass="t-c" />
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="gvItemSelected" />
                                                    <HeaderStyle CssClass="gvHeader" />
                                                    <RowStyle CssClass="gvItem" />
                                                    <PagerStyle CssClass="gvPager" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <div class="card-footer mt-2">
                                            <div class="row">
                                                <div class="col-auto va-m mr-auto">
                                                    <asp:Label runat="server" ID="lbl_total_1" CssClass=""></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnEjecutar" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:View>
                        <%--********************************************************************--%>
                        <%--*************** Indicadores --%>
                        <%--********************************************************************--%>
                        <asp:View ID="View3" runat="server">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="card">
                                        <div class="card-header card-header-main text-primary">
                                            <h4>Indicadores</h4>
                                        </div>
                                        <div class="card-body">
                                            <h5 class="">Detalle</h5>
                                            <div class="gv-w">
                                                <asp:GridView ID="gvReporte_2" CssClass="gv" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" AllowPaging="false">
                                                    <Columns>
                                                        <asp:BoundField DataField="nombre_planp" HeaderText="Nombre" ItemStyle-Width="150" />
                                                        <asp:BoundField DataField="area_bruta" HeaderText="Área bruta" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="area_util" HeaderText="Área útil" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="porc_SE_total" HeaderText="% Suelo ejecutado total" DataFormatString="{0:P0}" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="unidades_ejecutadas_VIP" HeaderText="Unidades Ejecutadas VIP" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="unidades_ejecutadas_VIS" HeaderText="Unidades Ejecutadas VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="porc_UE_VIP" HeaderText="% Unidades ejecutadas VIP" DataFormatString="{0:P0}" SortExpression="porc_UE_VIP" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_UE_VIS" HeaderText="% Unidades ejecutadas VIS" DataFormatString="{0:P0}" SortExpression="porc_UE_VIS" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_UE_no_VIS" HeaderText="% Unidades ejecutadas No VIS" DataFormatString="{0:P0}" SortExpression="porc_UE_no_VIS" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_UE_vivienda" HeaderText="% Unidades ejecutadas vivienda" DataFormatString="{0:P0}" SortExpression="porc_UE_vivienda" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_SE_espacio_publico" HeaderText="% Suelo ejecutado espacio público" DataFormatString="{0:P0}" SortExpression="porc_SE_espacio_publico" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_SE_equipamiento" HeaderText="% Suelo ejecutado equipamientos" DataFormatString="{0:P0}" SortExpression="porc_SE_equipamiento" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="meses_iniciacion" HeaderText="Meses iniciación" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="gvItemSelected" />
                                                    <HeaderStyle CssClass="gvHeader" />
                                                    <RowStyle CssClass="gvItem" />
                                                    <PagerStyle CssClass="gvPager" />
                                                </asp:GridView>
                                            </div>

                                            <h5 class="mt-3">Anual</h5>
                                            <div class="gv-w">
                                                <asp:GridView ID="gvReporte_2_1" CssClass="gv" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" AllowPaging="false">
                                                    <Columns>
                                                        <asp:BoundField DataField="ano_adopcion" HeaderText="Año Adopción" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="planes_parciales" HeaderText="Planes Parciales" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="area_bruta" HeaderText="Área bruta" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="area_util" HeaderText="Área útil" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="porc_SE_total" HeaderText="% Suelo ejecutado total" DataFormatString="{0:P0}" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="unidades_ejecutadas_VIP" HeaderText="Unidades Ejecutadas VIP" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="unidades_ejecutadas_VIS" HeaderText="Unidades Ejecutadas VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="porc_UE_VIP" HeaderText="% Unidades ejecutadas VIP" DataFormatString="{0:P0}" SortExpression="porc_UE_VIP" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_UE_VIS" HeaderText="% Unidades ejecutadas VIS" DataFormatString="{0:P0}" SortExpression="porc_UE_VIS" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_UE_no_VIS" HeaderText="% Unidades ejecutadas No VIS" DataFormatString="{0:P0}" SortExpression="porc_UE_no_VIS" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_UE_vivienda" HeaderText="% Unidades ejecutadas vivienda" DataFormatString="{0:P0}" SortExpression="porc_UE_vivienda" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_SE_espacio_publico" HeaderText="% Suelo ejecutado espacio público" DataFormatString="{0:P0}" SortExpression="porc_SE_espacio_publico" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_SE_equipamiento" HeaderText="% Suelo ejecutado equipamientos" DataFormatString="{0:P0}" SortExpression="porc_SE_equipamiento" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="meses_iniciacion" HeaderText="Meses iniciación" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="gvItemSelected" />
                                                    <HeaderStyle CssClass="gvHeader" />
                                                    <RowStyle CssClass="gvItem" />
                                                    <PagerStyle CssClass="gvPager" />
                                                </asp:GridView>
                                            </div>

                                            <h5 class="mt-3">Resumen</h5>
                                            <div class="gv-w">
                                                <asp:GridView ID="gvReporte_2_2" CssClass="gv" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" AllowPaging="false">
                                                    <Columns>
                                                        <asp:BoundField DataField="ano_adopcion_inicial" HeaderText="Adopción Inicial" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="ano_adopcion_final" HeaderText="Adopción Final" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="planes_parciales" HeaderText="Planes Parciales" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="area_bruta" HeaderText="Área bruta" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="area_util" HeaderText="Área útil" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="porc_SE_total" HeaderText="% Suelo ejecutado total" DataFormatString="{0:P0}" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="unidades_ejecutadas_VIP" HeaderText="Unidades Ejecutadas VIP" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="unidades_ejecutadas_VIS" HeaderText="Unidades Ejecutadas VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                        <asp:BoundField DataField="porc_UE_VIP" HeaderText="% Unidades ejecutadas VIP" DataFormatString="{0:P0}" SortExpression="porc_UE_VIP" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_UE_VIS" HeaderText="% Unidades ejecutadas VIS" DataFormatString="{0:P0}" SortExpression="porc_UE_VIS" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_UE_no_VIS" HeaderText="% Unidades ejecutadas No VIS" DataFormatString="{0:P0}" SortExpression="porc_UE_no_VIS" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_UE_vivienda" HeaderText="% Unidades ejecutadas vivienda" DataFormatString="{0:P0}" SortExpression="porc_UE_vivienda" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_SE_espacio_publico" HeaderText="% Suelo ejecutado espacio público" DataFormatString="{0:P0}" SortExpression="porc_SE_espacio_publico" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="porc_SE_equipamiento" HeaderText="% Suelo ejecutado equipamientos" DataFormatString="{0:P0}" SortExpression="porc_SE_equipamiento" ItemStyle-CssClass="t-c" />
                                                        <asp:BoundField DataField="meses_iniciacion" HeaderText="Meses iniciación" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="gvItemSelected" />
                                                    <HeaderStyle CssClass="gvHeader" />
                                                    <RowStyle CssClass="gvItem" />
                                                    <PagerStyle CssClass="gvPager" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <div class="card-footer mt-2">
                                            <div class="row">
                                                <div class="col-auto va-m mr-auto">
                                                    <asp:Label runat="server" ID="lbl_total_2" CssClass=""></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnEjecutar" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        function pageLoad() {
            $('#<%=gvReporte_0.ClientID%>').gridviewScroll({
            height: 400,
            freezesize: 1,
            startVertical: $("#<%=hfGVReporte_0SV.ClientID%>").val(),
          startHorizontal: $("#<%=hfGVReporte_0SH.ClientID%>").val(),
          onScrollVertical: function (delta) { $("#<%=hfGVReporte_0SV.ClientID%>").val(delta); },
          onScrollHorizontal: function (delta) { $("#<%=hfGVReporte_0SH.ClientID%>").val(delta); }
      });

        $('#<%=gvReporte_1.ClientID%>').gridviewScroll({
            height: 400,
            freezesize: 1,
            startVertical: $("#<%=hfGVReporte_1SV.ClientID%>").val(),
          startHorizontal: $("#<%=hfGVReporte_1SH.ClientID%>").val(),
          onScrollVertical: function (delta) { $("#<%=hfGVReporte_1SV.ClientID%>").val(delta); },
          onScrollHorizontal: function (delta) { $("#<%=hfGVReporte_1SH.ClientID%>").val(delta); }
      });

        $('#<%=gvReporte_2.ClientID%>').gridviewScroll({
            height: 300,
            freezesize: 1,
            startVertical: $("#<%=hfGVReporte_2SV.ClientID%>").val(),
          startHorizontal: $("#<%=hfGVReporte_2SH.ClientID%>").val(),
          onScrollVertical: function (delta) { $("#<%=hfGVReporte_2SV.ClientID%>").val(delta); },
          onScrollHorizontal: function (delta) { $("#<%=hfGVReporte_2SH.ClientID%>").val(delta); }
      });

        $('#<%=gvReporte_2_1.ClientID%>').gridviewScroll({
            height: 300,
            freezesize: 1,
            startVertical: $("#<%=hfGVReporte_2_1SV.ClientID%>").val(),
          startHorizontal: $("#<%=hfGVReporte_2_1SH.ClientID%>").val(),
          onScrollVertical: function (delta) { $("#<%=hfGVReporte_2_1SV.ClientID%>").val(delta); },
          onScrollHorizontal: function (delta) { $("#<%=hfGVReporte_2_1SH.ClientID%>").val(delta); }
      });

        $('#<%=gvReporte_2_2.ClientID%>').gridviewScroll({
            height: 300,
            freezesize: 2,
            startVertical: $("#<%=hfGVReporte_2_2SV.ClientID%>").val(),
          startHorizontal: $("#<%=hfGVReporte_2_2SH.ClientID%>").val(),
          onScrollVertical: function (delta) { $("#<%=hfGVReporte_2_2SV.ClientID%>").val(delta); },
          onScrollHorizontal: function (delta) { $("#<%=hfGVReporte_2_2SH.ClientID%>").val(delta); }
      });
        }
    </script>
</asp:Content>
