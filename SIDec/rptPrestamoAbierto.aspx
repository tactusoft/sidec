<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" AutoEventWireup="true" CodeBehind="rptPrestamoAbierto.aspx.cs" Inherits="SIDec.rptPrestamoAbierto" %>

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
                            <h4>Reporte - Prestamos abiertos</h4>
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
                                    <asp:PostBackTrigger ControlID="btnExportar" />
                                </Triggers>
                            </asp:UpdatePanel>

                            <div class="form-group">
                                <asp:UpdatePanel runat="server" ID="upReporteSection" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="card mt-3 mb-5">
                                            <%--*************** Detalle *********************************************--%>
                                            <div class="gv-w">
                                                <asp:Label runat="server" ID="lbl_total_detalle" CssClass="fwb"></asp:Label>
                                                <asp:GridView ID="gvDetalle" CssClass="gv" runat="server" AutoGenerateColumns="false" AllowPaging="false" PageSize="100">
                                                    <Columns>
                                                        <asp:BoundField DataField="area_solicita_prestamo" HeaderText="Área solicita" />
                                                        <asp:BoundField DataField="fecha_entrega_prestamo" HeaderText="Fecha prestamo" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="w90" />
                                                        <asp:BoundField DataField="resolucion" HeaderText="Resolución" ItemStyle-CssClass="w90" />
                                                        <asp:BoundField DataField="chip" HeaderText="Chip" />
                                                        <asp:BoundField DataField="usu_solicita_prestamo" HeaderText="Usuario solicita" ItemStyle-CssClass="w180" />
                                                        <asp:BoundField DataField="obs_prestamo" HeaderText="Observación" />
                                                        <asp:BoundField DataField="usu_entrega_prestamo" HeaderText="Usuario entrega" ItemStyle-CssClass="w180" />
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="gvItemSelected" />
                                                    <HeaderStyle CssClass="gvHeaderSm" />
                                                    <RowStyle CssClass="gvItemSm" />
                                                    <PagerStyle CssClass="gvPager" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </ContentTemplate>
                        <Triggers>
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
        }
    </script>
</asp:Content>
