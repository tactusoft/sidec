<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" AutoEventWireup="true" CodeBehind="rptEstadoPredio.aspx.cs" Inherits="SIDec.rptEstadoPredio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
        <ContentTemplate>
            <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:HiddenField ID="hfgvDetalleSV" runat="server" />
    <asp:HiddenField ID="hfgvDetalleSH" runat="server" />
    <asp:HiddenField ID="hfgvConsolidadoSV" runat="server" />
    <asp:HiddenField ID="hfgvConsolidadoSH" runat="server" />

    <div id="divData" runat="server">
        <div class="col-12" role="main">

            <div class="card mt-3 mb-5">
                <div class="card-header card-header-main">
                    <div class="row">
                        <div class="col-12 text-primary">
                            <h4>Reporte - Estado Predios</h4>
                        </div>
                    </div>
                </div>

                <div class="card-body">

                    <asp:UpdatePanel ID="upReporte" runat="server" UpdateMode="Conditional" DefaultButton="btnListar">
                        <ContentTemplate>
                            <asp:UpdatePanel runat="server" ID="upReporteFoot" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divData1" runat="server" class="main-section">
                                        <div class="row">
                                            <asp:Panel ID="pReporteAction" runat="server">
                                                <div class="col-auto ml-auto card-header-buttons">
                                                    <div class="btn-group">
                                                        <asp:LinkButton ID="btnListar" runat="server" disabled="" ValidationGroup="vgReporte" CssClass="btn" Text="Listar" CausesValidation="true" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnBuscar_Click">
									                            <i class="fas fa-th-list"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnExportar" runat="server" ValidationGroup="vgReporte" CssClass="btn" Text="Exportar" CausesValidation="true" CommandName="Excel" CommandArgument="4" ToolTip="Generar Excel" OnClick="btnExportar_Click">
									                            <i class="far fa-file-excel"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnListar" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="btnExportar" />
                                </Triggers>
                            </asp:UpdatePanel>

                            <div class="form-group">
                                <asp:UpdatePanel runat="server" ID="upReporteSection" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="upBuscar" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="form-group-sm col-2 col-lg-1">
                                                        <asp:Label ID="lblcoddeclaratoria" runat="server" class="lblBasic" ToolTip="Resolución Declaratoria">Res. Dec.</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddl_cod_declaratoria" CssClass="form-control form-control-xs" AppendDataBoundItems="true" TabIndex="1">
                                                            <asp:ListItem Value="">Todas</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group-sm col-3 col-lg-2">
                                                        <asp:Label ID="lblestado2" runat="server" class="lblBasic" ToolTip="Estado">Estado</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddl_id_estado_predio_declarado2" CssClass="form-control form-control-xs" AutoPostBack="true" OnSelectedIndexChanged="ddl_id_estado_predio_declarado2_SelectedIndexChanged">
                                                            <asp:ListItem Value="0">Todos</asp:ListItem>
                                                            <asp:ListItem Value="1">Vigente</asp:ListItem>
                                                            <asp:ListItem Value="2">No vigente</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group-sm col-6 col-sm-5 col-lg-3">
                                                        <asp:Label ID="lblestado" runat="server" class="lblBasic" ToolTip="Estado">Estado predio</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddl_id_estado_predio_declarado" CssClass="form-control form-control-xs" />
                                                    </div>
                                                    <div class="form-group-sm col-8 col-lg-3">
                                                        <asp:Label ID="lbl_cod_usuario" runat="server" class="lblBasic" ToolTip="Usuario">Usuario</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddl_cod_usuario" CssClass="form-control form-control-xs" />
                                                    </div>
                                                    <div class="form-group-sm col-4 col-lg-2">
                                                        <asp:Label ID="lbltiempocumplimiento" runat="server" class="lblBasic" ToolTip="Estado">Tiempo cumplimiento</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddl_id_tiempo_cumplimiento" CssClass="form-control form-control-xs">
                                                            <asp:ListItem Value="0">Todos</asp:ListItem>
                                                            <asp:ListItem Value="1">Nulos</asp:ListItem>
                                                            <asp:ListItem Value="2">Entre 0 y 4 años</asp:ListItem>
                                                            <asp:ListItem Value="3">Entre 4 y 5 años</asp:ListItem>
                                                            <asp:ListItem Value="4">Mayor de 5 años</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>


                                        <div class="card mt-3 mb-5">
                                            <asp:Panel ID="pnlGrids" runat="server" Visible="false">
                                                <div class="card-header card-header-main text-center">
                                                    <ul runat="server" id="ulReporteSection" class="nav nav-pills border-bottom-0 mr-auto">
                                                        <li class="nav-item">
                                                            <asp:LinkButton ID="lbReporteSection_0" runat="server" CssClass="nav-link active" CommandArgument="0" CausesValidation="false" OnClick="btnReporteSection_Click">Consolidado</asp:LinkButton>
                                                        </li>
                                                        <li class="nav-item">
                                                            <asp:LinkButton ID="lbReporteSection_1" runat="server" CssClass="nav-link" CommandArgument="1" CausesValidation="false" OnClick="btnReporteSection_Click">Detalle</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </div>

                                                <div>
                                                    <asp:MultiView ID="mvReporteSection" runat="server" ActiveViewIndex="0">
                                                        <asp:View ID="vwConsolidado" runat="server">
                                                            <div class="card-body">
                                                                <div class="gv-w">
                                                                    <asp:Label runat="server" ID="lbl_total_consolidado" CssClass="fwb"></asp:Label>
                                                                    <asp:GridView ID="gvConsolidado" CssClass="gv" runat="server" AutoGenerateColumns="false" AllowPaging="false" PageSize="100">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="declaratoria" HeaderText="Declaratoria" />
                                                                            <asp:BoundField DataField="vigente" HeaderText="Vigente" />
                                                                            <asp:BoundField DataField="estado_predio_declarado" HeaderText="Estado predio" />
                                                                            <asp:BoundField DataField="cantidad" HeaderText="Predios" ItemStyle-HorizontalAlign="Right" />
                                                                        </Columns>
                                                                        <SelectedRowStyle CssClass="gvItemSelected" />
                                                                        <HeaderStyle CssClass="gvHeaderSm" />
                                                                        <RowStyle CssClass="gvItemSm" />
                                                                        <PagerStyle CssClass="gvPager" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </asp:View>

                                                        <%--*************** Detalle *********************************************--%>
                                                        <asp:View ID="vwDetalle" runat="server">
                                                            <div class="card-body">
                                                                <div class="gv-w">
                                                                    <asp:Label runat="server" ID="lbl_total_detalle" CssClass="fwb"></asp:Label>
                                                                    <asp:GridView ID="gvDetalle" CssClass="gv" runat="server" AutoGenerateColumns="false" AllowPaging="false" PageSize="100">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="declaratoria" HeaderText="Declaratoria" SortExpression="declaratoria" ItemStyle-CssClass="w80" />
                                                                            <asp:BoundField DataField="chip" HeaderText="CHIP" SortExpression="chip" ItemStyle-CssClass="t-c" />
                                                                            <asp:BoundField DataField="vigente" HeaderText="Vigente" SortExpression="vigente" ItemStyle-CssClass="w80" />
                                                                            <asp:BoundField DataField="estado_predio_declarado" HeaderText="Estado predio" SortExpression="estado_predio_declarado" />
                                                                            <asp:BoundField DataField="fecha_cumplimiento" HeaderText="Fecha cumplimiento" SortExpression="fecha_cumplimiento" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="t-c" />
                                                                            <asp:BoundField DataField="anos_tiempo_cumplimiento" HeaderText="Años cumplimiento" SortExpression="anos_tiempo_cumplimiento" DataFormatString="{0:N2}" HeaderStyle-CssClass="d-n" ItemStyle-CssClass="d-n" />
                                                                            <asp:BoundField DataField="tiempo_cumplimiento" HeaderText="Tiempo cumplimiento" SortExpression="tiempo_cumplimiento" ItemStyle-CssClass="t-c" />
                                                                            <asp:BoundField DataField="usu_responsable" HeaderText="Usuario responsable" SortExpression="usu_responsable" />
                                                                            <asp:BoundField DataField="area_terreno_UAECD" HeaderText="Área UAECD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                                            <asp:BoundField DataField="area_terreno_folio" HeaderText="Área folio" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="d-n" ItemStyle-CssClass="d-n" />
                                                                            <asp:BoundField DataField="cod_lote" HeaderText="Código lote" />
                                                                            <asp:BoundField DataField="matricula" HeaderText="Matrícula inm." />
                                                                            <asp:BoundField DataField="direccion" HeaderText="Dirección" />
                                                                            <asp:BoundField DataField="nombre_barrio" HeaderText="Barrio" />
                                                                            <asp:BoundField DataField="nombre_UPZ" HeaderText="UPZ" />
                                                                            <asp:BoundField DataField="nombre_localidad" HeaderText="Localidad" />
                                                                        </Columns>
                                                                        <SelectedRowStyle CssClass="gvItemSelected" />
                                                                        <HeaderStyle CssClass="gvHeaderSm" />
                                                                        <RowStyle CssClass="gvItemSm" />
                                                                        <PagerStyle CssClass="gvPager" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                </div>
                                            </asp:Panel>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnListar" EventName="Click" />
                            <asp:PostBackTrigger ControlID="btnExportar" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="pnlLoading" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upReporte">
                        <ProgressTemplate>
                            <asp:Image runat="server" CssClass="imgCargando" ImageUrl="~/images/icon/cargando.gif" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
            </div>
        </div>
    </div>

    <%--********************************************************************************************************************************************************************************--%>
    <%--*****************************************************************							 SECTION DE SCRIPTS							******************************************************************--%>
    <%--********************************************************************************************************************************************************************************--%>
    <script>
        function pageLoad() {
            $('#<%=gvDetalle.ClientID%>').gridviewScroll({
                width: 1150,
                height: 450,
                railcolor: gvValores("railcolor"),
                barcolor: gvValores("barcolor"),
                barhovercolor: gvValores("barhovercolor"),
                bgcolor: gvValores("bgcolor"),
                varrowtopimg: gvValores("varrowtopimg"),
                varrowbottomimg: gvValores("varrowbottomimg"),
                harrowleftimg: gvValores("harrowleftimg"),
                harrowrightimg: gvValores("harrowrightimg"),
                arrowsize: 16,
                headerrowcount: 1,
                railsize: 16,
                barsize: 12,

                startVertical: $("#<%=hfgvDetalleSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfgvDetalleSH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfgvDetalleSV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfgvDetalleSH.ClientID%>").val(delta);
                }
            });

            $('#<%=gvConsolidado.ClientID%>').gridviewScroll({
                width: 1150,
                height: 450,
                railcolor: gvValores("railcolor"),
                barcolor: gvValores("barcolor"),
                barhovercolor: gvValores("barhovercolor"),
                bgcolor: gvValores("bgcolor"),
                varrowtopimg: gvValores("varrowtopimg"),
                varrowbottomimg: gvValores("varrowbottomimg"),
                harrowleftimg: gvValores("harrowleftimg"),
                harrowrightimg: gvValores("harrowrightimg"),
                arrowsize: 16,
                headerrowcount: 1,
                railsize: 16,
                barsize: 12,

                startVertical: $("#<%=hfgvConsolidadoSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfgvConsolidadoSH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfgvConsolidadoSV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfgvConsolidadoSH.ClientID%>").val(delta);
                }
            });
        }
    </script>
</asp:Content>
