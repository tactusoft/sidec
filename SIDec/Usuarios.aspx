<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="SIDec.Usuarios" EnableEventValidation="false" %>

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
                            <asp:Label ID="lblTitle" runat="server" Text="Usuarios"></asp:Label></h4>
                    </div>
                    <div class="col-sm-6">
                        <asp:UpdatePanel ID="pnlBuscar" runat="server" DefaultButton="btnBuscar">
                            <ContentTemplate>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control" placeholder="Búsqueda por nombre, apellido, usuario o perfil" />
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
                    <div runat="server" id="msgUser" class="alert " ></div><%--role="alert"--%>
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
            <asp:UpdatePanel runat="server" ID="upUserNavegation" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField ID="hddID" runat="server" Value="0" />
                    <asp:Panel runat="server" ID="pnlUserNavegation">
                        <div id="divData" runat="server" style="top: 45px;">

                            <div class="row">
                                <div class="col-auto card-header-buttons" style="padding-top: .75rem; padding-bottom: .25rem; top: 45px;">
                                    <asp:Panel ID="pUserExecAction" runat="server">
                                        <div class="btn-group flex-wrap">
                                            <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgUser" OnClick="btnAccept_Click">
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
                                    <asp:Panel ID="pUserAction" runat="server">
                                        <div class="btn-group">
                                            <asp:LinkButton ID="btnUserList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnUserAccion_Click">
										        <i class="fas fa-th-list"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnUserAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnUserAccion_Click">
										        <i class="fas fa-plus"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnUserEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnUserAccion_Click">
										        <i class="fas fa-pencil-alt"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnUserDel" runat="server" CssClass="btn" Text="Reiniciar Clave" CausesValidation="false" CommandName="Reset" CommandArgument="3" ToolTip="Reiniciar clave" OnClick="btnUserAccion_Click">
										        <i class="fas fa-user-lock"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnUserList" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnUserAdd" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnUserEdit" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnUserDel" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="gvUser" />
                </Triggers>
            </asp:UpdatePanel>

            <div class="card-body">

                <asp:UpdatePanel runat="server" ID="upUser" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:MultiView runat="server" ID="mvUser" ActiveViewIndex="0">
                            <asp:View runat="server" ID="vUserGrid">
                                <div class="gv-w gv-scroll-xl">

                                    <div class="mt-2 mb-0">
                                        <asp:CheckBox runat="server" ID="chk_f_activos" CssClass="" Text="Activos" AutoPostBack="true" OnCheckedChanged="chk_f_activos_CheckedChanged" Checked="true" />
                                    </div>
                                    <asp:GridView ID="gvUser" CssClass="gv" runat="server" PageSize="20" AllowPaging="true"
                                        DataKeyNames="cod_usuario,usuario" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvUser_PageIndexChanging"
                                        AllowSorting="true" OnSorting="gvUser_Sorting" OnSelectedIndexChanged="gvUser_SelectedIndexChanged"
                                        OnRowDataBound="gvUser_RowDataBound" OnRowCreated="gv_RowCreated" OnRowCommand="gvUser_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="cod_usuario" HeaderText="Código" SortExpression="cod_usuario" ItemStyle-CssClass="w50 t-r" />
                                            <asp:BoundField DataField="usuario" HeaderText="Usuario" SortExpression="usuario" />
                                            <asp:BoundField DataField="nombre_usuario" HeaderText="Nombre" SortExpression="nombre_usuario" />
                                            <asp:BoundField DataField="apellido_usuario" HeaderText="Apellido" SortExpression="apellido_usuario" />
                                            <asp:TemplateField HeaderText="Habilitado" ItemStyle-CssClass="t-c w90">
                                                <ItemTemplate><%# Convert.ToString(Eval("habilitado")) == "1" ? "Si" : "" %></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="perfiles" HeaderText="Perfil" SortExpression="perfiles" />
                                            <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                                                <HeaderTemplate>
                                                    <div class="btn-group">
                                                        <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="_Add" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnAdd_Click">
										                        <i class="fas fa-plus"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="btn-group">
                                                        <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn btn-grid-detail" Text="Detalle" CausesValidation="false" CommandName="_Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Visualizar registro"> 
										                        <i class="fas fa-info-circle"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-grid-edit" Text="Editar" CausesValidation="false" CommandName="_Edit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Editar registro"> 
										                    <i class="fas fa-pencil-alt"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnReset" runat="server" CssClass="btn btn-grid-delete" Text="Reiniciar Clave" CausesValidation="false" CommandName="_Reset" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Reiniciar clave"> 
										                    <i class="fas fa-user-lock"></i>
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

                            <asp:View runat="server" ID="vUserDetalle">
                                <asp:Panel ID="pnlDetail" runat="server">
                                    <div class="row" style="margin-top: 22px;">
                                        <div class="form-group-sm col-4 col-md-1">
                                            <label for="txt_cod_usuario" class="">Código</label>
                                            <asp:TextBox runat="server" ID="txt_cod_usuario" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-8 col-md-3 col-lg-2">
                                            <label for="txt_usuario" class="">Usuario</label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgUser" ControlToValidate="txt_usuario">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox runat="server" ID="txt_usuario" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender runat="server" ID="fte_txtusuario" FilterMode="ValidChars" FilterType="LowercaseLetters,UppercaseLetters,Custom" TargetControlID="txt_usuario" ValidChars="." />
                                        </div>
                                        <div class="form-group-sm col-12 col-sm-6 col-md-4 col-lg-3">
                                            <label for="txt_nombre_usuario" class="">Nombre</label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgUser" ControlToValidate="txt_nombre_usuario">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox runat="server" ID="txt_nombre_usuario" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-12 col-sm-6 col-md-4 col-lg-3">
                                            <label for="txt_apellido_usuario" class="">Apellido</label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgUser" ControlToValidate="txt_apellido_usuario">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox runat="server" ID="txt_apellido_usuario" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-12 col-md-6 col-lg-3">
                                            <label for="ddlb_id_area_entidad" class="">Área</label>
                                            <asp:DropDownList runat="server" ID="ddlb_id_area_entidad" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group-sm col-12 col-md-6 col-lg-3">
                                            <label for="txt_correo_usuario" class="">Correo electrónico</label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgUser" ControlToValidate="txt_correo_usuario">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="vgUser" ControlToValidate="txt_correo_usuario">
                                                <uc:ToolTip width="130px" ToolTip="Correo inválido" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:TextBox runat="server" ID="txt_correo_usuario" CssClass="form-control form-control-xs" TextMode="Email" MaxLength="50"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-12 col-md-6 col-lg-5">
                                            <label for="ddlb_cod_cargo_usuario" class="">Cargo</label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgUser" ControlToValidate="ddlb_cod_cargo_usuario">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:DropDownList runat="server" ID="ddlb_cod_cargo_usuario" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group-sm col-8 col-md-4 col-lg-3">
                                            <label for="txt_matricula_usuario" class="">Matrícula</label>
                                            <asp:TextBox runat="server" ID="txt_matricula_usuario" CssClass="form-control form-control-xs" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-4 col-md-2 col-lg-1">
                                            <br />
                                            <asp:CheckBox runat="server" ID="chk_habilitado" Text="Habilitado" />
                                        </div>
                                        <div class="form-group-sm col-12" style="background-color: #f4f4f4; margin: 15px 0px; padding: 8px">
                                            <label for="cbl_cod_perfil" class="fwb">Perfil</label>
                                            <asp:CustomValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ClientValidationFunction="ValidateCodPerfil" ValidationGroup="vgUser">
                                                <uc:ToolTip width="210px" ToolTip="Seleccione al menos un perfil" runat="server"/>
                                            </asp:CustomValidator> 
                                            <asp:CheckBoxList ID="cbl_cod_perfil" runat="server" RepeatColumns="3" RepeatDirection="Vertical" Width="100%" CssClass="noBorder"  onchange="Page_ClientValidate('vgUser')"></asp:CheckBoxList>
                                        </div>
                                        <div class="form-group-sm col-12" style="margin-top: 15px 0px; padding: 8px">
                                            <label for="cbl_cod_perfil" class="fwb">Permisos adicionales</label>
                                        </div>
                                        <div class="form-group-sm col-4 col-md-3">
                                            <asp:CheckBox runat="server" ID="chk_asigna_usuario_predios" CssClass="_ml15" Text="Asigna predios" />
                                        </div>
                                        <div class="form-group-sm col-4 col-md-3">
                                            <asp:CheckBox runat="server" ID="chk_edita_actos" CssClass="_ml15" Text="Edita actos" />
                                        </div>
                                        <div class="form-group-sm col-4 col-md-3">
                                            <asp:CheckBox runat="server" ID="chk_edita_documentos" CssClass="_ml15" Text="Edita documentos" />
                                        </div>
                                        <div class="form-group-sm col-4 col-md-3">
                                            <asp:CheckBox runat="server" ID="chk_elimina_documentos" CssClass="_ml15" Text="Elimina documentos" />
                                        </div>
                                        <div class="form-group-sm col-4 col-md-3">
                                            <asp:CheckBox runat="server" ID="chk_recibe_prestamos" CssClass="_ml15" Text="Recibe préstamos" />
                                        </div>
                                        <div class="form-group-sm col-4 col-md-3">
                                            <asp:CheckBox runat="server" ID="chk_revisa_gestion" CssClass="_ml15" Text="Revisa gestion" />
                                        </div>
                                        
                                        <%--<div class="form-group-sm col-4 col-md-3">
                                            <asp:Button runat="server" ID="btnResetPassword" CssClass="btn btn-outline-danger btn-sm btnLoad" Visible="false" Text="Resetear Password" OnClick="btnResetPassword_Click" />
                                        </div>--%>
                                    </div>
                                    </div>

                                </asp:Panel>
                            </asp:View>
                        </asp:MultiView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserNavFirst" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserNavBack" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserNavNext" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserNavLast" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserEdit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserDel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="gvUser" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>

                <asp:UpdatePanel runat="server" ID="upUserFoot" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="divSPMessage" id="DivMsgUser"></div>
                        <div class="row-cols-1 navegation">
                            <asp:Panel ID="divUserNavegation" runat="server">
                                <asp:LinkButton ID="btnUserNavFirst" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnUserNavegation_Click">
								<i class="fas fa-angle-double-left"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnUserNavBack" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnUserNavegation_Click">
								<i class="fas fa-angle-left"></i>
                                </asp:LinkButton>
                                <asp:Label runat="server" ID="lblUserCuenta"></asp:Label>
                                <asp:LinkButton ID="btnUserNavNext" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnUserNavegation_Click">
								<i class="fas fa-angle-right"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnUserNavLast" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnUserNavegation_Click">
								<i class="fas fa-angle-double-right"></i>
                                </asp:LinkButton>
                            </asp:Panel>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnUserList" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserEdit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserDel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAccept" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserNavFirst" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserNavBack" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserNavNext" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUserNavLast" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="gvUser" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
    </div>
    </div>
</asp:Content>
