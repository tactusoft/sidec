<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" AutoEventWireup="true" CodeBehind="rptGestionDocumento.aspx.cs" Inherits="SIDec.rptGestionDocumento" %>

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
                            <h4>Reporte - Gesti&oacute;n de Documentos</h4>
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
                                        <asp:UpdatePanel ID="upBuscar" runat="server">
                                            <ContentTemplate>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="form-group-sm col-4 col-sm-3 col-md-2">
                                                        <asp:Label ID="lblfecha_inicial" runat="server" class="lblBasic" ToolTip="Fecha inicial">Fecha inicial</asp:Label>
                                                        <asp:CustomValidator ID="cv_fecha_inicial" runat="server" ClientValidationFunction="DateClientValidate" Display="Dynamic" ValidateEmptyText="true" ControlToValidate="txt_fecha_inicial">
                                                                <uc:ToolTip width="150px" ToolTip="Fecha con formato inválido" runat="server"/>
                                                        </asp:CustomValidator>
                                                        <asp:RequiredFieldValidator ID="rfv_fecha_inicial" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgReporte" ControlToValidate="txt_fecha_inicial">
                                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RangeValidator ID="rv_fecha_inicial" runat="server" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgReporte" ControlToValidate="txt_fecha_inicial">
                                                                <uc:ToolTip width="130px" ToolTip="Fecha inválida" runat="server"/>
                                                        </asp:RangeValidator>
                                                        <ajax:CalendarExtender ID="cefecha_inicial" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_inicial" PopupButtonID="txt_fecha_inicial" Format="yyyy-MM-dd" />
                                                        <asp:TextBox runat="server" ID="txt_fecha_inicial" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm col-4 col-sm-3 col-md-2">
                                                        <asp:Label ID="lblfecha_final" runat="server" class="lblBasic" ToolTip="Fecha final">Fecha final</asp:Label>
                                                        <asp:CustomValidator ID="cv_fecha_final" runat="server" ClientValidationFunction="DateClientValidate" Display="Dynamic" ValidateEmptyText="true" ControlToValidate="txt_fecha_final">
                                                                <uc:ToolTip width="150px" ToolTip="Fecha con formato inválido" runat="server"/>
                                                        </asp:CustomValidator>
                                                        <asp:RequiredFieldValidator ID="rfv_fecha_final" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgReporte" ControlToValidate="txt_fecha_final">
                                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RangeValidator ID="rv_fecha_final" runat="server" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgReporte" ControlToValidate="txt_fecha_final">
                                                                <uc:ToolTip width="130px" ToolTip="Fecha inválida" runat="server"/>
                                                        </asp:RangeValidator>
                                                        <ajax:CalendarExtender ID="ce_fecha_final" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_final" PopupButtonID="txt_fecha_final" Format="yyyy-MM-dd" />
                                                        <asp:TextBox runat="server" ID="txt_fecha_final" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm col-12 col-sm-4">
                                                        <asp:Label ID="lbl_cod_usuario" runat="server" class="lblBasic" ToolTip="Usuario">Usuario </asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlb_cod_usuario" CssClass="form-control form-control-xs" />

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
                                                                            <asp:BoundField DataField="fecha_inicial" HeaderText="Fecha inicial" HtmlEncode="false" DataFormatString="{0:dd/MMM/yyyy}" ItemStyle-CssClass="t-c" />
                                                                            <asp:BoundField DataField="fecha_final" HeaderText="Fecha final" HtmlEncode="false" DataFormatString="{0:dd/MMM/yyyy}" ItemStyle-CssClass="t-c" />
                                                                            <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                                                                            <asp:BoundField DataField="registros_digitados" HeaderText="Reg. digitados" ItemStyle-HorizontalAlign="Right" />
                                                                            <asp:BoundField DataField="folios_digitados" HeaderText="Folios digitados" ItemStyle-HorizontalAlign="Right" />
                                                                            <asp:BoundField DataField="registros_escaneados" HeaderText="Reg. escaneados" ItemStyle-HorizontalAlign="Right" />
                                                                            <asp:BoundField DataField="folios_escaneados" HeaderText="Folios escaneados" ItemStyle-HorizontalAlign="Right" />
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
                                                                            <asp:BoundField DataField="fecha" HeaderText="Fecha" HtmlEncode="false" DataFormatString="{0:dd/MMM/yyyy}" ItemStyle-CssClass="t-c" />
                                                                            <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                                                                            <asp:BoundField DataField="resolucion" HeaderText="Resolución" ItemStyle-HorizontalAlign="Center" />
                                                                            <asp:BoundField DataField="chip" HeaderText="CHIP" ItemStyle-HorizontalAlign="Center" />
                                                                            <asp:BoundField DataField="registros_digitados" HeaderText="Reg. digitados" ItemStyle-HorizontalAlign="Right" />
                                                                            <asp:BoundField DataField="folios_digitados" HeaderText="Folios digitados" ItemStyle-HorizontalAlign="Right" />
                                                                            <asp:BoundField DataField="registros_escaneados" HeaderText="Reg. escaneados" ItemStyle-HorizontalAlign="Right" />
                                                                            <asp:BoundField DataField="folios_escaneados" HeaderText="Folios escaneados" ItemStyle-HorizontalAlign="Right" />
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
