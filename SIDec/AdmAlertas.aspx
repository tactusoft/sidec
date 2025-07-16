<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" AutoEventWireup="true" CodeBehind="AdmAlertas.aspx.cs" Inherits="SIDec.AdmAlertas" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    

    <asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
        <ContentTemplate>
            <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
            <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--Valores para scroll de los GV--%>

    <div class="col-12" role="main">
        <div class="card mt-3 mb-5">

            <div class="card-header card-header-main">
                <div class="row">
                    <div class="col-sm-6 text-primary">
                        <h4>
                            <asp:Label ID="lblTitle" runat="server" Text="Administración de Alertas - Cuadro de control"></asp:Label></h4>
                    </div>
                    <div class="col-sm-6">
                        <asp:UpdatePanel ID="pnlBuscar" runat="server" DefaultButton="btnBuscar">
                            <ContentTemplate>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control" placeholder="Nombre de estado o trámite" />
                                    <div class="input-group-append">
                                        <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn btn-primary btn-sm" CausesValidation="false" OnClick="btnBuscar_Click" />
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
            <asp:UpdatePanel ID="upMsgMain" runat="server" UpdateMode="Conditional" class="">
                <ContentTemplate>
                    <div runat="server" id="msgAdmin" class="alert " ></div><%--role="alert"--%>
                    <div runat="server" id="msgMain" class="d-none" role="alert">
                        <span runat="server" id="msgMainText"></span>
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAccept" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server" ID="upAdminNavegation" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlAdminNavegation">
                        <div id="divData" runat="server" style="top: 45px;">

                            <div class="row">
                                <div class="col-auto card-header-buttons" style="padding-top: .75rem; padding-bottom: .25rem; top: 45px;">
                                    <asp:Panel ID="pAdminExecAction" runat="server">
                                        <div class="btn-group flex-wrap">
                                            <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgAdmin" OnClick="btnAccept_Click">
										                                            <i class="fas fa-check"></i>&nbspAceptar
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-secondary btn-sm" CommandArgument="0" OnClick="btnCancel_Click" CausesValidation="false">
										                                            <i class="fas fa-times"></i>&nbspCancelar
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-auto ml-auto card-header-buttons" style="top: 45px;">
                                    <asp:Panel ID="pAdminAction" runat="server">
                                        <div class="btn-group">
                                            <asp:LinkButton ID="btnAdminList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnAdminAccion_Click">
										        <i class="fas fa-th-list"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnAdminEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnAdminAccion_Click">
										        <i class="fas fa-pencil-alt"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAdminList" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnAdminEdit" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="gvAdmin" />
                </Triggers>
            </asp:UpdatePanel>

            <div class="card-body">

                <asp:UpdatePanel runat="server" ID="upAdmin" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:MultiView runat="server" ID="mvAdmin" ActiveViewIndex="0">
                            <asp:View runat="server" ID="vAdminGrid">
                                <div class="gv-w gv-scroll-xl">

                                    <div class="mt-2 mb-0">
                                        <asp:RadioButtonList runat="server" ID="rbl_tipo" CssClass="" AutoPostBack="true" OnSelectedIndexChanged="rbl_tipo_SelectedIndexChanged" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Value="55" Text="Estado"></asp:ListItem>
                                            <asp:ListItem Value="76" Text="Trámite"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <asp:GridView ID="gvAdmin" CssClass="gv" runat="server" PageSize="20" AllowPaging="true"
                                        DataKeyNames="idrango_ejecucion" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvAdmin_PageIndexChanging"
                                        AllowSorting="true" OnSelectedIndexChanged="gvAdmin_SelectedIndexChanged" OnRowDataBound="gvAdmin_RowDataBound"  OnRowCommand="gvAdmin_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="tipo" HeaderText="Tipo" ItemStyle-CssClass="w100 t-c" />
                                            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                            <asp:BoundField DataField="dias_limite" HeaderText="Días ejecución"  ItemStyle-CssClass="t-c w100"/>
                                            <asp:BoundField DataField="porcentaje_limite" HeaderText="% alerta"  ItemStyle-CssClass="t-c w100"/>
                                            <asp:BoundField DataField="dias_limite_critico" HeaderText="Días alerta crítico"  ItemStyle-CssClass="t-c w100"/>
                                            <asp:BoundField DataField="porcentaje_limite_critico" HeaderText="% crítico"  ItemStyle-CssClass="t-c w100"/>
                                            <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                                                <ItemTemplate>
                                                    <div class="btn-group">
                                                        <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn btn-grid-detail" Text="Detalle" CausesValidation="false" CommandName="_Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Visualizar registro"> 
										                        <i class="fas fa-info-circle"></i>
                                                        </asp:LinkButton>
                                                    </div>
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

                            <asp:View runat="server" ID="vAdminDetalle">
                                <asp:Panel ID="pnlDetail" runat="server">
                                    <div class="row">
                                        <div class="form-group-sm col-4 col-md-3">
                                            <asp:HiddenField ID="hdd_idrango_ejecucion" runat="server" Value="0" />
                                            <asp:Label id="lbltipo" runat="server">Tipo</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_tipo" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-5 col-md-7">
                                           <asp:Label id="lblnombre" runat="server">Nombre estado o trámite</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_nombre" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group-sm col-12 col-sm-6 col-md-3">
                                            <asp:Label id="lbldias_limite" runat="server">Días para ejecución</asp:Label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgAdmin" ControlToValidate="txt_dias_limite">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox runat="server" ID="txt_dias_limite" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender runat="server" ID="fte_dias_limite" FilterType="Numbers" TargetControlID="txt_dias_limite" />
                                        </div>
                                        <div class="form-group-sm col-12 col-sm-6 col-md-3">
                                            <asp:Label id="lblporcentaje_limite" runat="server">% Crítico</asp:Label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgAdmin" ControlToValidate="txt_vporcentaje_limite">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox runat="server" ID="txt_vporcentaje_limite" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender runat="server" ID="fte_porcentaje_limite" FilterType="Numbers" TargetControlID="txt_vporcentaje_limite" />
                                        </div>
                                        <div class="form-group-sm col-12 col-sm-6 col-md-3">
                                            <asp:Label id="lbldias_limite_critico" runat="server">Días ejecución avanzados</asp:Label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgAdmin" ControlToValidate="txt_dias_limite_critico">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox runat="server" ID="txt_dias_limite_critico" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender runat="server" ID="fte_dias_limite_critico" FilterType="Numbers" TargetControlID="txt_dias_limite_critico" />
                                        </div>
                                        <div class="form-group-sm col-12 col-sm-6 col-md-3">
                                            <asp:Label id="lblporcentaje_limite_critico" runat="server">% Críticos avanzados</asp:Label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgAdmin" ControlToValidate="txt_vporcentaje_limite_critico">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox runat="server" ID="txt_vporcentaje_limite_critico" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender runat="server" ID="fte_porcentaje_limite_critico" FilterType="Numbers" TargetControlID="txt_vporcentaje_limite_critico" />
                                        </div>
                                    </div>

                                </asp:Panel>
                            </asp:View>
                        </asp:MultiView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdminNavFirst" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdminNavBack" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdminNavNext" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdminNavLast" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdminEdit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="gvAdmin" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>

                <asp:UpdatePanel runat="server" ID="upAdminFoot" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="divSPMessage" id="DivMsgAdmin"></div>
                        <div class="row-cols-1 navegation">
                            <asp:Panel ID="divAdminNavegation" runat="server">
                                <asp:LinkButton ID="btnAdminNavFirst" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnAdminNavegation_Click">
								<i class="fas fa-angle-double-left"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnAdminNavBack" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnAdminNavegation_Click">
								<i class="fas fa-angle-left"></i>
                                </asp:LinkButton>
                                <asp:Label runat="server" ID="lblAdminCuenta"></asp:Label>
                                <asp:LinkButton ID="btnAdminNavNext" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnAdminNavegation_Click">
								<i class="fas fa-angle-right"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnAdminNavLast" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnAdminNavegation_Click">
								<i class="fas fa-angle-double-right"></i>
                                </asp:LinkButton>
                            </asp:Panel>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdminList" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdminEdit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAccept" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdminNavFirst" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdminNavBack" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdminNavNext" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdminNavLast" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="gvAdmin" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
    </div>
    </div>
</asp:Content>
