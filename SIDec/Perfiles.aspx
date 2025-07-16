<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" AutoEventWireup="true" CodeBehind="Perfiles.aspx.cs" Inherits="SIDec.Perfiles" EnableEventValidation="false" %>

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
                            <asp:Label ID="lblTitle" runat="server" Text="Perfiles"></asp:Label></h4>
                    </div>
                    <div class="col-sm-6">
                        <asp:UpdatePanel ID="pnlBuscar" runat="server" DefaultButton="btnBuscar">
                            <ContentTemplate>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control" placeholder="Búsqueda por perfil o descripción" />
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
                    <div runat="server" id="msgProfile" class="alert " ></div><%--role="alert"--%>
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
            <asp:UpdatePanel runat="server" ID="upProfileNavegation" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField ID="hddID" runat="server" Value="0" />
                    <asp:Panel runat="server" ID="pnlProfileNavegation">
                        <div id="divData" runat="server" style="top: 45px;">

                            <div class="row">
                                <div class="col-auto card-header-buttons" style="padding-top: .75rem; padding-bottom: .25rem; top: 45px;">
                                    <asp:Panel ID="pProfileExecAction" runat="server">
                                        <div class="btn-group flex-wrap">
                                            <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgProfile" OnClick="btnAccept_Click">
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
                                    <asp:Panel ID="pProfileAction" runat="server">
                                        <div class="btn-group">
                                            <asp:LinkButton ID="btnProfileList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnProfileAccion_Click">
										        <i class="fas fa-th-list"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProfileAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnProfileAccion_Click">
										        <i class="fas fa-plus"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProfileEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnProfileAccion_Click">
										        <i class="fas fa-pencil-alt"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnProfileDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Delete" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnProfileAccion_Click">
										        <i class="fas fa-trash-alt"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnProfileList" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnProfileAdd" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnProfileEdit" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnProfileDel" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="gvProfile" />
                </Triggers>
            </asp:UpdatePanel>

            <div class="card-body">

                <asp:UpdatePanel runat="server" ID="upProfile" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:MultiView runat="server" ID="mvProfile" ActiveViewIndex="0">
                            <asp:View runat="server" ID="vProfileGrid">
                                <div class="gv-w gv-scroll-xl">
                                    <asp:GridView ID="gvProfile" CssClass="gv" runat="server" PageSize="20" AllowPaging="true"
                                        DataKeyNames="cod_perfil,nombre_perfil" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvProfile_PageIndexChanging"
                                        AllowSorting="true" OnSorting="gvProfile_Sorting" OnSelectedIndexChanged="gvProfile_SelectedIndexChanged"
                                        OnRowDataBound="gvProfile_RowDataBound" OnRowCreated="gv_RowCreated" OnRowCommand="gvProfile_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="cod_perfil" HeaderText="Código" SortExpression="cod_perfil" />
                                            <asp:BoundField DataField="nombre_perfil" HeaderText="Perfil" SortExpression="nombre_perfil" />
                                            <asp:BoundField DataField="desc_perfil" HeaderText="Descripción" />
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
                                                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-grid-delete" Text="Eliminar" CausesValidation="false" CommandName="_Delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Eliminar registro"> 
										                    <i class="fas fa-trash-alt"></i>
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

                            <asp:View runat="server" ID="vProfileDetalle">
                                <asp:Panel ID="pnlDetail" runat="server">
                                    <div class="row" style="margin-top: 22px;">
                                        <div class="form-group-sm col-4 col-md-1">
                                            <label for="txt_cod_usuario" class="">Código</label>
                                            <asp:TextBox runat="server" ID="txt_cod_perfil" CssClass="form-control form-control-xs"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-8 col-md-4 col-lg-3">
                                            <label for="txt_nombre_perfil" class="">Perfil</label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProfile" ControlToValidate="txt_nombre_perfil">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox runat="server" ID="txt_nombre_perfil" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender runat="server" ID="fte_txtperfil" FilterMode="ValidChars" FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom" TargetControlID="txt_nombre_perfil" ValidChars=". -+" />
                                        </div>
                                        <div class="form-group-sm col-12 col-md-7 col-lg-8">
                                            <label for="txt_desc_perfil" class="">Descripción</label>
                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProfile" ControlToValidate="txt_desc_perfil">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox runat="server" ID="txt_desc_perfil" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender1" FilterMode="InvalidChars" FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom" TargetControlID="txt_nombre_perfil" InvalidChars="<>" />
                                        </div>
                                    </div>

                                </asp:Panel>
                                <div class="form-group-sm col-12">
                                    <div class="gv-w gv-scroll-sm">
                                        <label for="cbl_cod_perfil" class="fwb">Permisos</label>
                                        <asp:GridView ID="gvPermissions" runat="server" CssClass="gv" OnRowDataBound="gvPermissions_RowDataBound"
                                            DataKeyNames="tipo_permiso,objeto_permiso,consultar,insertar,modificar,eliminar" AutoGenerateColumns="false" AllowSorting="false">
                                            <Columns>
                                                <asp:BoundField DataField="tipo" HeaderText="Tipo" ItemStyle-CssClass="w70" />
                                                <asp:BoundField DataField="nombre" HeaderText="Menú" />
                                                <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w100" HeaderText="Consulta">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlRead" runat="server" SelectedValue='<%# Bind("consultar") %>'>
                                                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Total"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w100" HeaderText="Inserción">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlCreate" runat="server" SelectedValue='<%# Bind("insertar") %>'>
                                                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Total"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w100" HeaderText="Actualización">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlUpdate" runat="server" SelectedValue='<%# Bind("modificar") %>'>
                                                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Total"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w100" HeaderText="Eliminación">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlDelete" runat="server" SelectedValue='<%# Bind("eliminar") %>'>
                                                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Total"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="gvHeader" />
                                            <RowStyle CssClass="gvItem" />
                                            <PagerStyle CssClass="gvPager" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileNavFirst" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileNavBack" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileNavNext" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileNavLast" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileEdit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileDel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="gvProfile" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>

                <asp:UpdatePanel runat="server" ID="upProfileFoot" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="divSPMessage" id="DivMsgProfile"></div>
                        <div class="row-cols-1 navegation">
                            <asp:Panel ID="divProfileNavegation" runat="server">
                                <asp:LinkButton ID="btnProfileNavFirst" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnProfileNavegation_Click">
								<i class="fas fa-angle-double-left"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnProfileNavBack" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnProfileNavegation_Click">
								<i class="fas fa-angle-left"></i>
                                </asp:LinkButton>
                                <asp:Label runat="server" ID="lblProfileCuenta"></asp:Label>
                                <asp:LinkButton ID="btnProfileNavNext" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnProfileNavegation_Click">
								<i class="fas fa-angle-right"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnProfileNavLast" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnProfileNavegation_Click">
								<i class="fas fa-angle-double-right"></i>
                                </asp:LinkButton>
                            </asp:Panel>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnProfileList" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileEdit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileDel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAccept" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileNavFirst" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileNavBack" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileNavNext" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnProfileNavLast" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="gvProfile" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
    </div>
    </div>
</asp:Content>
