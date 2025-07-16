<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" CodeBehind="ProyEstrategico.aspx.cs" Inherits="SIDec.ProyEstrategico" ViewStateMode="Enabled" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/Particular/Activity.ascx" TagPrefix="uc" TagName="Activity" %>
<%@ Register Src="~/UserControls/Particular/ActivityManagement.ascx" TagPrefix="uc" TagName="ActivityManagement" %>
<%@ Register Src="~/UserControls/Particular/ProjectBankHeaderPE.ascx" TagPrefix="uc" TagName="ProjectBankHeader" %>
<%@ Register Src="~/UserControls/Particular/Tracing.ascx" TagPrefix="uc" TagName="Tracing" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="./styles/jquery/jcarousel.basic.css" />
    <link rel="stylesheet" href="./styles/jsgantt/jsgantt.css" />
    <link rel="stylesheet" href="./styles/sumoselect/sumoselect.css" />
    <script type="text/javascript" src="./styles/jquery/jquery.jcarousel.min.js"></script>
    <script type="text/javascript" src="./styles/jsgantt/jsgantt.js"></script>
    <script type="text/javascript" src="./styles/sumoselect/jquery.sumoselect.min.js"></script>


</asp:Content>
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
    <div id="modProyEstrategico" class="modal fade" data-backdrop="static" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header modal-bg-info">
                    <h5 class="modal-title">Confirmar acción</h5>
                </div>
                <div class="modal-body">
                    ¿Está seguro de continuar con la acción solicitada?
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnConfirmarProyEstrategico" runat="server" Text="" CssClass="btn btn-outline-primary" OnClick="btnConfirmar_Click" data-dismiss="modal">
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
    <asp:HiddenField runat="server" ID="hfEvtGVProyEstrategico" Value="" />
    <asp:HiddenField ID="hfGVProyEstrategicoSV" runat="server" />
    <asp:HiddenField ID="hfGVProyEstrategicoSH" runat="server" />
    <asp:HiddenField ID="hfGVProyEstrategicoManzanasSV" runat="server" />
    <asp:HiddenField ID="hfGVProyEstrategicoManzanasSH" runat="server" />
    <asp:HiddenField ID="hfGVProyEstrategicoCesionesSV" runat="server" />
    <asp:HiddenField ID="hfGVProyEstrategicoCesionesSH" runat="server" />
    <asp:HiddenField ID="hfGVProyEstrategicoActosSV" runat="server" />
    <asp:HiddenField ID="hfGVProyEstrategicoActosSH" runat="server" />
    <asp:HiddenField ID="hfGVProyEstrategicoLicenciasSV" runat="server" />
    <asp:HiddenField ID="hfGVProyEstrategicoLicenciasSH" runat="server" />
    <asp:HiddenField ID="hfGVProyEstrategicoVisitasSV" runat="server" />
    <asp:HiddenField ID="hfGVProyEstrategicoVisitasSH" runat="server" />

    <asp:HiddenField ID="hdd_Proyecto_ProyEstrategico_Id" runat="server" />


    <div id="divData" runat="server">
        <div class="col-12" role="main">

            <%--********************************************************************--%>
            <%--*************** ProyEstrategico de Proyectos --%>
            <%--********************************************************************--%>
            <div class="card mt-3 mb-5">
                <div class="card-header card-header-main">
                    <div class="row">
                        <div class="col-sm-6 text-primary">
                            <h4>Seguimiento Proyectos Estratégicos</h4>
                        </div>
                        <div class="col-sm-6">
                            <asp:UpdatePanel ID="pnlBuscar" runat="server" DefaultButton="btnBuscar">
                                <ContentTemplate>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control" placeholder="Búsqueda por código, nombre, tipo proyecto o estado del proyecto" />
                                        <div class="input-group-append">
                                            <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn btn-outline-primary" CausesValidation="false" OnClick="btnBuscar_Click" />
                                        </div>
                                        <asp:Button runat="server" ID="btnVolver" Text="Volver a información general" Visible="false" CssClass="btn btn-outline-primary" CausesValidation="false" OnClick="btnVolver_Click" />
                                        <asp:UpdatePanel ID="upImprimir" runat="server" DefaultButton="btnImprimir">
                                            <ContentTemplate>
                                                <asp:Button runat="server" ID="btnImprimir" Text="Imprimir" CssClass="btn btn-outline-success" CausesValidation="false" OnClick="btnImprimir_Click" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnImprimir" />
                                            </Triggers>
                                        </asp:UpdatePanel>
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
                    <asp:UpdatePanel runat="server" ID="upProyEstrategico" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hddsectionview" runat="server" Value="" />

                            <asp:MultiView runat="server" ID="mvProyEstrategico">
                                <asp:View runat="server" ID="vProyEstrategico">
                                    <div class="gv-w">
                                        <asp:GridView ID="gvProyEstrategico" CssClass="gv" runat="server" PageSize="20" AllowPaging="true"
                                            DataKeyNames="idbanco,cod_usu_responsable,idproyecto"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvProyEstrategico_PageIndexChanging"
                                            AllowSorting="true" OnSorting="gvProyEstrategico_Sorting" OnSelectedIndexChanged="gvProyEstrategico_SelectedIndexChanged"
                                            OnRowDataBound="gvProyEstrategico_RowDataBound" OnRowCreated="gv_RowCreated"  OnRowCommand="gvProyEstrategico_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="codigo" HeaderText="Código" SortExpression="codigo" />
                                                <asp:BoundField DataField="nombre" HeaderText="Nombre" SortExpression="nombre" />
                                                <asp:BoundField DataField="tipo_proyecto" HeaderText="Tipo_proyecto" SortExpression="tipo_proyecto" />
                                                <asp:BoundField DataField="estado_proyecto" HeaderText="Estado proyecto" SortExpression="estado_proyecto" />

                                                <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                                                    <HeaderTemplate>
                                                        <div class="btn-group">
                                                            <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="_Add" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnAddBank_Click">
										                        <i class="fas fa-plus"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="btn-group">
                                                            <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn btn-grid-detail" style="padding: 0rem 0.75rem;" Text="Detalle" CausesValidation="false" CommandName="_Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Visualizar registro"> 
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
                                </asp:View>
                                <asp:View runat="server" ID="vProyEstrategicoDetalle">
                                    <div runat="server" id="divProyEstrategico" class="">
                                        <div id="accordion">
                                            <div class="card">
                                                <div class="card-header card-header-dark">
                                                    <button type="button" class="noBtn nav-link" id="heading1" data-toggle="collapse" data-target="#collapse1" aria-expanded="true" aria-controls="collapse1">
                                                        FICHA DE PROYECTO
                                                    </button>
                                                </div>
                                                <div id="collapse1" class="collapse show" aria-labelledby="heading1" data-parent="#accordion">
                                                    <div class="card-body">
                                                        <asp:UpdatePanel runat="server" ID="upFichaProyecto" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <uc:ProjectBankHeader ID="PB_Header" runat="server" ValidationGroup="vgFichaProyecto" ControlID="PB_Header.ProyectoEstrategico" IsStrategicProject="true"/>
                                                                <div>
                                                                    <asp:UpdatePanel runat="server" ID="upFichaProyectoFoot" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <asp:UpdatePanel runat="server" ID="upFichaProyectoMsg" UpdateMode="Conditional" class="alert-card card-header-message">
                                                                                    <ContentTemplate>
                                                                                        <div runat="server" id="msgFichaProyecto" class="alert d-none" role="alert"></div>
                                                                                        <asp:ValidationSummary runat="server" ID="vsFichaProyecto" DisplayMode="SingleParagraph" CssClass="invalid-feedback" HeaderText="Falta informar: " ShowSummary="true" ShowValidationErrors="true" ValidationGroup="vgFichaProyecto" />
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>

                                                                            <div class="row">

                                                                                <div class="col-auto card-header-buttons" style="padding-top: .5rem">
                                                                                    <asp:Panel ID="pFichaProyectoExecAction" runat="server">
                                                                                        <div class="btn-group flex-wrap">
                                                                                            <asp:LinkButton ID="btnFichaProyectoAccionFinal" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgFichaProyecto"
                                                                                                OnClientClick="if(Page_ClientValidate('vgFichaProyecto')) return openModal('modProyEstrategico');">
										                                            <i class="fas fa-check"></i>&nbspAceptar
                                                                                            </asp:LinkButton>
                                                                                            <asp:LinkButton ID="btnFichaProyectoCancelar" runat="server" CssClass="btn btn-secondary btn-sm" CommandArgument="0" OnClick="btnFichaProyectoCancelar_Click" CausesValidation="false">
										                                            <i class="fas fa-times"></i>&nbspCancelar
                                                                                            </asp:LinkButton>
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">

                                                                                <div class="col-auto ml-auto card-header-buttons">
                                                                                    <asp:Panel ID="pFichaProyectoAction" runat="server">
                                                                                        <div class="btn-group">
                                                                                            <asp:LinkButton ID="btnFichaProyectoList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="_List" CommandArgument="4" ToolTip="Listado" OnClick="btnFichaProyectoAccion_Click">
										                                                        <i class="fas fa-th-list"></i>
                                                                                            </asp:LinkButton>
                                                                                            <asp:LinkButton ID="btnFichaProyectoAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="_Add" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnFichaProyectoAccion_Click">
										                                                        <i class="fas fa-plus"></i>
                                                                                            </asp:LinkButton>
                                                                                            <asp:LinkButton ID="btnFichaProyectoEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="_Edit" CommandArgument="1" ToolTip="Editar registro" OnClick="btnFichaProyectoAccion_Click">
										                                                        <i class="fas fa-pencil-alt"></i>
                                                                                            </asp:LinkButton>
                                                                                            <asp:LinkButton ID="btnFichaProyectoDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="_Delete" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnFichaProyectoAccion_Click">
										                                                        <i class="far fa-trash-alt"></i>
                                                                                            </asp:LinkButton>
                                                                                            <asp:LinkButton ID="btnFichaProyectoExcel" runat="server" CssClass="btn" Text="Ficha" CausesValidation="false" CommandName="_Excel" CommandArgument="4" ToolTip="Generar ficha" OnClick="btnFichaProyectoAccion_Click">
										                                                        <i class="far fa-file-excel"></i>
                                                                                            </asp:LinkButton>
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="btnFichaProyectoAdd" EventName="Click" />
                                                                            <asp:AsyncPostBackTrigger ControlID="btnFichaProyectoEdit" EventName="Click" />
                                                                            <asp:AsyncPostBackTrigger ControlID="btnFichaProyectoDel" EventName="Click" />
                                                                            <asp:PostBackTrigger ControlID="btnFichaProyectoExcel" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card">
                                                <div class="card-header card-header-dark">
                                                    <button type="button" class="noBtn nav-link" id="heading2" data-toggle="collapse" data-target="#collapse2" aria-expanded="true" aria-controls="collapse2">
                                                        GESTIÓN Y ACTIVIDADES REQUERIDAS «<asp:Label ID="lblNombreProyecto_Sec2" runat="server"></asp:Label>»
                                                    </button>
                                                </div>
                                                <div id="collapse2" class="collapse" aria-labelledby="heading2" data-parent="#accordion">
                                                    <div class="card-body">
                                                        <uc:Activity ID="ucActivity" runat="server" ControlID="ProyEstrategicoActividad" Section="Create" OnToList="ucActivity_ToList" OnSave="ucActivity_OnSave"/>
                                                    </div>
                                                </div>
                                            </div> 
                                            <div class="card">
                                                <div class="card-header card-header-dark">
                                                    <button type="button" class="noBtn nav-link" id="heading3" data-toggle="collapse" data-target="#collapse3" aria-expanded="true" aria-controls="collapse3">
                                                       ACTIVIDADES EN PROCESO «<asp:Label ID="lblNombreProyecto_Sec3" runat="server"></asp:Label>»
                                                    </button>
                                                </div>
                                                <div id="collapse3" class="collapse" aria-labelledby="heading3" data-parent="#accordion">
                                                    <div class="card-body">
                                                        <uc:ActivityManagement ID="ucActivityManagement" runat="server" ControlID="ProyEstrategicoActividad" Section="Create" OnToList="ucActivity_ToList" OnViewDoc="uc_ViewDoc"/>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card">
                                                <div class="card-header card-header-dark">
                                                    <button type="button" class="noBtn nav-link" id="heading4" data-toggle="collapse" data-target="#collapse4" aria-expanded="true" aria-controls="collapse4">
                                                        SEGUIMIENTO - MESAS DE TRABAJO «<asp:Label ID="lblNombreProyecto_Sec4" runat="server"></asp:Label> »
                                                    </button>
                                                </div>
                                                <div id="collapse4" class="collapse" aria-labelledby="heading4" data-parent="#accordion">
                                                    <div class="card-body">
                                                        <uc:Tracing ID="ucTracing" runat="server" ControlID="ProyEstrategicoSeguimiento" OnToList="ucTracing_ToList" OnViewDoc="uc_ViewDoc"
                                                            ReturnToList="true" OnUserControlException="ucTracing_UserControlException" OnSaveActivity="ucTracing_SaveActivity"></uc:Tracing>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </asp:View>
                            </asp:MultiView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyEstrategicoNavFirst" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyEstrategicoNavBack" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyEstrategicoNavNext" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnProyEstrategicoNavLast" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="gvProyEstrategico" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ucTracing" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <div class="card-footer noprint">
                    <asp:UpdatePanel runat="server" ID="upProyEstrategicoFoot" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:UpdatePanel runat="server" ID="upProyEstrategicoMsg" UpdateMode="Conditional" class="alert-card">
                                <ContentTemplate>
                                    <div runat="server" id="msgProyEstrategico" class="alert d-none" role="alert"></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row">
                                <div class="col-auto va-m mr-auto">
                                    
                                </div>

                                <div class="col-auto va-m mr-auto">
                                    <asp:Label ID="lblProyEstrategicoCuenta" runat="server"></asp:Label>
                                </div>
                                <div class="col-auto">
                                    <asp:Panel ID="pProyEstrategicoNavegacion" runat="server">
                                        <div class="btn-group">
                                            <asp:LinkButton ID="btnProyEstrategicoNavFirst" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnProyEstrategicoNavegacion_Click">
											  <i class="fas fa-angle-double-left"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProyEstrategicoNavBack" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnProyEstrategicoNavegacion_Click">
											  <i class="fas fa-angle-left"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProyEstrategicoNavNext" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnProyEstrategicoNavegacion_Click">
											  <i class="fas fa-angle-right"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProyEstrategicoNavLast" runat="server" CssClass="btn btn-outline-secondary" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnProyEstrategicoNavegacion_Click">
											  <i class="fas fa-angle-double-right"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>

                                <div class="col-auto ml-auto">
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <%--********************************************************************--%>
    <%--*************** Scripts --%>
    <%--********************************************************************--%>
    <script type="text/javascript">
        
        function pageLoad() {
            if ($("#<%= hddsectionview.ClientID%>").val() != "") {
                sessionStorage.setItem('accordion', $("#<%= hddsectionview.ClientID%>").val());
                $("#<%= hddsectionview.ClientID%>").val("");
            }

            var last = sessionStorage.getItem('accordion');

            if (last != null) {
                $('[id^=collapse]').removeClass('show');
                $('#' + last).addClass('show');
            }

            $('[id^=heading]').click(function () {
                sessionStorage.setItem('accordion', $(this).attr('id').replace('heading', 'collapse'));
            });
        }

        
        function ValidateFileUpload(clientID) {
            var fuData = document.getElementById(clientID.id);
            var lblError = document.getElementById(clientID.id.replace("fu", "lbl"));
            var FileUploadPath = fuData.value;
            if (FileUploadPath == '') {
                args.IsValid = false;
            }
            else {
                var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();
                var filename = FileUploadPath.substring(FileUploadPath.lastIndexOf('\\') + 1).toLowerCase();

                if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif" || Extension == "bmp") {
                    lblError.innerHTML = filename;
                    boton.disable = false;
                    //return true;
                }
                else {
                    fuData.value = "";
                    lblError.innerHTML = "<span style='color:red'>Solo se permiten imágenes con extensión png, jpg, gif o bmp</span>";
                    boton.disable = true;
                    // return false;
                }
            }
        }
    </script>

    <asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
        <ContentTemplate>
            <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
            <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
        </ContentTemplate>
    </asp:UpdatePanel>

    



</asp:Content>
