<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" CodeBehind="ComIntersectorial.aspx.cs" Inherits="SIDec.ComIntersectorial" ViewStateMode="Enabled" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/ComIntersectorial/Folios.ascx" TagPrefix="uc" TagName="Folios" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>

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
    <%--******************************* Modal ******************************--%>
    <%--********************************************************************--%>
    <div id="modComIntersectorial" class="modal fade" data-backdrop="static" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header modal-bg-info">
                    <h5 class="modal-title">Confirmar acción</h5>
                </div>
                <div class="modal-body">
                    ¿Está seguro de continuar con la acción solicitada?
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnConfirmarComIntersectorial" runat="server" Text="" CssClass="btn btn-outline-primary" OnClick="btnConfirmar_Click" data-dismiss="modal">
							<i class="fas fa-check"></i>&nbsp&nbspAceptar
                    </asp:LinkButton>
                    <button type="button" class="btn btn-outline-secondary" data-dismiss="modal" onclick="MsgMain(0);"><i class="fas fa-times"></i>&nbsp&nbspCancelar</button>
                </div>
            </div>
        </div>
    </div>

    <%--********************************************************************--%>
    <%--*************** gridviews --%>
    <%--********************************************************************--%>
    <asp:HiddenField runat="server" ID="hfEvtGVComIntersectorial" Value="" />
    <asp:HiddenField ID="hfGVComIntersectorialSV" runat="server" />
    <asp:HiddenField ID="hfGVComIntersectorialSH" runat="server" />

    <asp:HiddenField ID="hdd_Proyecto_ComIntersectorial_Id" runat="server" />

    <div id="divData" runat="server">
        <div class="col-12" role="main">

            <%--********************************************************************--%>
            <%--*************** ComIntersectorial de Proyectos --%>
            <%--********************************************************************--%>
            <div class="card mt-3 mb-5">
                <div class="card-header card-header-main">
                    <div class="row">
                        <div class="col-sm-6 text-primary">
                            <h4>Comisi&oacute;n Intersectorial</h4>
                        </div>
                        <div class="col-sm-6">
                            <asp:UpdatePanel ID="pnlBuscar" runat="server" DefaultButton="btnBuscar">
                                <ContentTemplate>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control" placeholder="Búsqueda por nombre" MaxLength="200"/>
                                        <div class="input-group-append">
                                            <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn btn-outline-primary" CausesValidation="false" OnClick="btnBuscar_Click" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdateProgress ID="pnlLoading" runat="server" DynamicLayout="true">
                                <ProgressTemplate>
                                    <asp:Image runat="server" CssClass="imgCargando" ImageUrl="./images/icon/cargando.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                    </div>
                </div>

                <div class="card-body">

                    <asp:UpdatePanel runat="server" ID="upProyectosSection" ChildrenAsTriggers="true" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:UpdatePanel runat="server" ID="upProyectosActores" UpdateMode="Conditional" class="user_control main-section" >
                                <ContentTemplate>
                                    <asp:HiddenField ID="hdd_idactor" runat="server" Value="0" />
                                    <uc:Folios ID="ucFolios" runat="server" ControlID="ComIntersectorialActividad" Section="Create" OnViewDoc="ucFolios_ViewDoc" OnReport="ucFolios_Report" />
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <%--********************************************************************--%>
    <%--*************** Scripts --%>
    <%--********************************************************************--%>

    <asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
        <ContentTemplate>
            <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
            <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
        </ContentTemplate>
    </asp:UpdatePanel>
      



</asp:Content>
