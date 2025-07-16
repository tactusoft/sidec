<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true"
    CodeBehind="Proyectos.aspx.cs" Inherits="SIDec.Proyectos" ViewStateMode="Enabled" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/Proyectos/ProyectoInformacion.ascx" TagName="ProyectoInfo" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Proyectos/ProyectoPredios.ascx" TagName="ProyectoPredios" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Proyectos/ProyectoCarta.ascx" TagName="ProyectoCarta" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Proyectos/ProyectoLicencia.ascx" TagName="ProyectoLicencia" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Particular/Actor.ascx" TagName="Actor" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Proyectos/VisitasSitio.ascx" TagName="VisitaSitio" TagPrefix="uc" %>

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


    <div id="divData" runat="server">
        <div class="col-12" role="main">
            <%--*************************************************** Proyectos *************************************************************--%>
            <div class="card mt-3 mb-2">
                <div class="card-header card-header-main">
                    <div class="row">
                        <div class="col-sm-6 text-primary">
                            <h4>Proyectos Asociativos</h4>
                        </div>
                        <div class="col-sm-6">
                            <asp:UpdatePanel ID="pnlBuscar" runat="server" DefaultButton="btnBuscar">
                                <ContentTemplate>
                                    <div class="input-group">
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

                <div class="card-body" style="padding-bottom:0.2rem;">
                    <div class="card-user-control">
                        <asp:UpdatePanel runat="server" ID="upProyectos" UpdateMode="Conditional" class="user_control main-section">
                            <ContentTemplate>
                                <uc:ProyectoInfo ID="ucProyecto" runat="server" ControlID="Proyectos" OnSelectedProyecto="ucProyecto_SelectedProyecto" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>
        </div>
        <div class="col-12">
            <asp:UpdatePanel runat="server" ID="upProyectosSection" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card mt-3 mb-2">
                        <div class="card-body" style="padding-top:0.2rem;">

                            <div class="subcard-body">
                                <div class="card-header card-header-main text-center">
                                    <ul runat="server" id="ulProyectosSection" class="nav nav-pills border-bottom-0 mr-auto">
                                        <li class="nav-item">
                                            <asp:LinkButton ID="lbProyectosSection_0" runat="server" CssClass="nav-link active" CommandArgument="0" CausesValidation="false" OnClick="btnProyectosSection_Click">Predios</asp:LinkButton>
                                        </li>
                                        <li class="nav-item">
                                            <asp:LinkButton ID="lbProyectosSection_1" runat="server" CssClass="nav-link" CommandArgument="1" CausesValidation="false" OnClick="btnProyectosSection_Click">Cartas de Intención</asp:LinkButton>
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
                                    </ul>
                                </div>

                                <div class="card-body">
                                    <asp:MultiView runat="server" ID="mvProyectosSection" ActiveViewIndex="0">
                                        <asp:View runat="server" ID="vProyectosPredios">
                                            <asp:UpdatePanel runat="server" ID="upProyectosPredios" UpdateMode="Conditional" class="user_control main-section">
                                                <ContentTemplate>
                                                    <uc:ProyectoPredios ID="ucPredios" runat="server" ControlID="Predios" OnGoPredio="ucPredios_GoPredios" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:View>
                                        <asp:View runat="server" ID="vProyectosCartas">
                                            <asp:UpdatePanel runat="server" ID="upProyectosCartas" UpdateMode="Conditional" class="user_control main-section">
                                                <ContentTemplate>
                                                    <uc:ProyectoCarta ID="ucCartas" runat="server" ControlID="Cartas" OnViewDoc="uc_ViewDoc"/>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:View>
                                        <asp:View runat="server" ID="vProyectosLicencias">
                                            <asp:UpdatePanel runat="server" ID="upProyectosLicencias" UpdateMode="Conditional" class="user_control main-section">
                                                <ContentTemplate>
                                                    <uc:ProyectoLicencia ID="ucLicencias" runat="server" ControlID="Licencias" OnViewDoc="uc_ViewDoc" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:View>
                                        <asp:View runat="server" ID="vProyectosResponsables">
                                            <asp:UpdatePanel runat="server" ID="upProyectosResponsables" UpdateMode="Conditional" class="user_control main-section">
                                                <ContentTemplate>
                                                    <uc:Actor ID="ucResponsables" runat="server" ControlID="Actor" ReferenceTypeID="2" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:View>
                                        <asp:View runat="server" ID="vProyectosVisitas">
                                            <asp:UpdatePanel runat="server" ID="upProyectosVisitas" UpdateMode="Conditional" class="user_control main-section">
                                                <ContentTemplate>
                                                    <uc:VisitaSitio ID="ucVisitas" runat="server" ControlID="VisitasPA" OnUserControlException="ucVisita_UserControlException"
                                                        OnViewDoc="ucVisita_ViewDoc" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:View>
                                    </asp:MultiView>
                                </div>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:HiddenField ID="hfGVProyectoSV" runat="server" />
        <asp:HiddenField ID="hfGVProyectoSH" runat="server" />
        <asp:HiddenField ID="hfGVProyectosPrediosSV" runat="server" />
        <asp:HiddenField ID="hfGVProyectosPrediosSH" runat="server" />
        <asp:HiddenField ID="hfGVProyectosLicenciasSV" runat="server" />
        <asp:HiddenField ID="hfGVProyectosLicenciasSH" runat="server" />
    </div>

    <script type="text/javascript">
        function pageLoad() {
            //*****************************************ESTILO GRIDVIEWS
            if ($('#<%= ucProyecto.FindControl("gvProyecto").ClientID%> tr').length >= 10) {
               $('#<%= ucProyecto.FindControl("gvProyecto").ClientID%>').gridviewScroll({
                   height: 300,
                   startVertical: $("#<%=hfGVProyectoSV.ClientID%>").val(),
                   startHorizontal: $("#<%=hfGVProyectoSH.ClientID%>").val(),
                   onScrollVertical: function (delta) { $("#<%=hfGVProyectoSV.ClientID%>").val(delta); },
                   onScrollHorizontal: function (delta) { $("#<%=hfGVProyectoSH.ClientID%>").val(delta); }
               });
           }
           //*****************************************ESTILO GRIDVIEWS
           if ($('#<%= ucPredios.FindControl("gvProyectoPredio").ClientID%> tr').length > 5) {
               $('#<%= ucPredios.FindControl("gvProyectoPredio").ClientID%>').gridviewScroll({
                   height: 200,
                   startVertical: $("#<%=hfGVProyectosPrediosSV.ClientID%>").val(),
                   startHorizontal: $("#<%=hfGVProyectosPrediosSH.ClientID%>").val(),
                   onScrollVertical: function (delta) { $("#<%=hfGVProyectosPrediosSV.ClientID%>").val(delta); },
                   onScrollHorizontal: function (delta) { $("#<%=hfGVProyectosPrediosSH.ClientID%>").val(delta); }
               });
           }
           //*****************************************ESTILO GRIDVIEWS
           if ($('#<%= ucLicencias.FindControl("gvProyectoLicencia").ClientID%> tr').length > 5) {
               $('#<%= ucLicencias.FindControl("gvProyectoLicencia").ClientID%>').gridviewScroll({
                   height: 200,
                   startVertical: $("#<%=hfGVProyectosLicenciasSV.ClientID%>").val(),
                   startHorizontal: $("#<%=hfGVProyectosLicenciasSH.ClientID%>").val(),
                   onScrollVertical: function (delta) { $("#<%=hfGVProyectosLicenciasSV.ClientID%>").val(delta); },
                   onScrollHorizontal: function (delta) { $("#<%=hfGVProyectosLicenciasSH.ClientID%>").val(delta); }
               });
            }
        }
        window.addEventListener('resize', (function () {
            setTimeout(pageLoad(), 1000)
        }));
    </script>

</asp:Content>

