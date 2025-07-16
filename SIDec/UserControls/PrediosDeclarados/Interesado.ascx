 <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Interesado.ascx.cs" Inherits="SIDec.UserControls.PrediosDeclarados.Interesado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/SliderImages.ascx" TagName="SliderImages" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>

<asp:HiddenField ID="hddReferenceID" runat="server" Value="0" />
<asp:HiddenField ID="hddId" runat="server" Value="UserControlInteresado" />
<asp:HiddenField ID="hddIdReferenceType" runat="server" Value="0" />
<asp:HiddenField ID="hddInteresadoPrimary" runat="server" Value="0" />

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc1:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc1:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upInteresado" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <%--************************************************** Alert Msg Main **************************************************************--%>
        <asp:UpdatePanel runat="server" ID="upInteresadoFoot" UpdateMode="Conditional">
            <ContentTemplate>

                <div id="divData" runat="server" class="dvRelative">
                    <div class="row">
                        <asp:UpdatePanel ID="upMsgMain" runat="server" UpdateMode="Conditional" class="alert-main msgusercontrol">
                            <ContentTemplate>
                                <div runat="server" id="msgInteresadoes" class="alert d-none" role="alert"></div>
                                <div runat="server" id="msgInteresadoesMain" class="d-none" role="alert">
                                    <span runat="server" id="msgMainText"></span>
                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pInteresadoExecAction" runat="server">
                            <div class="col-auto card-header-buttons" style="padding-top: .75rem; padding-bottom: .25rem;">
                                <div class="btn-group flex-wrap">
                                    <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgInteresado" OnClick="btnAccept_Click">
										                                            <i class="fas fa-check"></i>&nbspAceptar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-secondary btn-sm" CommandArgument="0" OnClick="btnCancel_Click" CausesValidation="false">
										                                            <i class="fas fa-times"></i>&nbspCancelar
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>

                    <div class="row">
                            <asp:Panel ID="pInteresadoAction" runat="server">
                        <div class="col-auto ml-auto card-header-buttons">
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnInteresadoList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnInteresadoAccion_Click">
										                                            <i class="fas fa-th-list"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnInteresadoAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnInteresadoAccion_Click">
										                                            <i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnInteresadoEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnInteresadoAccion_Click">
										                                            <i class="fas fa-pencil-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnInteresadoDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnInteresadoAccion_Click">
										                                            <i class="far fa-trash-alt"></i>
                                    </asp:LinkButton>
                                </div>
                        </div>
                            </asp:Panel>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAccept" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnInteresadoList" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnInteresadoAdd" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnInteresadoEdit" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnInteresadoDel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="gvInteresados" />
            </Triggers>
        </asp:UpdatePanel>


        <div class="uc-section-shade">
            <asp:Panel ID="pnlPBInteresadoActions" runat="server" Width="100%">
                <div runat="server" id="dvInteresadoActions" class="ml0 mtb10 d-i">
                    <div class="card form-group bg-light mt-2">
                        <div class="card-body">
                            <asp:HiddenField runat="server" ID="hdd_idinteresado" Value="0" />

                                <div class="form-group-sm col-3 col-sm-4 col-md-5 col-lg-6 div_auditoria">
                                    <asp:Label ID="lbl_fec_auditoria_interesado" runat="server" /></div>
                            <div class="row">
                                <div class="form-group-sm col-4 col-md-3">
                                    <label for="ddl_idtipo_interesado" class="">En calidad de</label>
                                    <asp:RequiredFieldValidator id="rfv_idtipo_interesado" runat="server" SetFocusOnError="true" ForeColor="Red" Display="Dynamic"
                                        ValidationGroup="vgInteresado" ControlToValidate="ddl_idtipo_interesado" CssClass="invalid-feedback" >
                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                    </asp:RequiredFieldValidator>
                                    <asp:DropDownList runat="server" ID="ddl_idtipo_interesado" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group-sm col-4 col-md-2">
                                    <label for="txt_documento" class="">No. documento</label>
                                    <asp:TextBox runat="server" ID="txt_documento" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbdocumento" runat="server" FilterType="Numbers, Custom" ValidChars="-" TargetControlID="txt_documento" />
                                </div>
                                <div class="form-group-sm col-12 col-sm-6 col-md-5">
                                    <label for="txt_nombre" class="">Nombre</label>
                                    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ForeColor="Red" Display="Dynamic"
                                        ValidationGroup="vgInteresado" ControlToValidate="txt_nombre">
                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                    </asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" ID="txt_nombre" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbnombre" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ " TargetControlID="txt_nombre" FilterMode="ValidChars" />
                                </div>
                                <div class="form-group-sm col-12 col-sm-6 col-md-3">
                                    <label for="txt_telefono" class="">Teléfono</label>
                                    <asp:TextBox runat="server" ID="txt_telefono" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbtelefono" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars=" -/.,;_" FilterMode="ValidChars" TargetControlID="txt_telefono" />
                                </div>
                                <div class="form-group-sm col-12 col-sm-6 col-md-5">
                                    <label for="txt_direccion" class="">Dirección</label>
                                    <asp:TextBox runat="server" ID="txt_direccion" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbdireccion" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" InvalidChars="<>" TargetControlID="txt_direccion" FilterMode="InvalidChars" />
                                </div>
                                <div class="form-group-sm col-12 col-sm-6 col-md-4">
                                    <label for="txt_correo" class="">Correo</label>
                                    <asp:RegularExpressionValidator ID="revcorreo" runat="server" SetFocusOnError="True" CssClass="invalid-feedback" Display="Dynamic"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="vgInteresado" ControlToValidate="txt_correo" >
                                        <uc:ToolTip width="130px" ToolTip="Correo inválido" runat="server"/>
                                    </asp:RegularExpressionValidator>
                                    <asp:TextBox runat="server" ID="txt_correo" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbcorreo" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" InvalidChars="<>" TargetControlID="txt_correo" FilterMode="InvalidChars" />
                                </div>
                                <div class="form-group-sm col-12 ">
                                    <label for="txt_direccion" class="">Otros datos de contacto</label>
                                    <asp:TextBox runat="server" ID="txt_otro" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbotro" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" InvalidChars="<>" TargetControlID="txt_otro" FilterMode="InvalidChars" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="gv-w">
                        <asp:GridView ID="gvInteresados" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="idinteresado, nombre" EmptyDataText="No hay registros asociados"
                            AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" AllowSorting="true" OnRowCommand="gvInteresados_RowCommand" OnRowDataBound="gvInteresados_RowDataBound" >
                            <Columns>
                                <asp:BoundField DataField="tipo_interesado" HeaderText="En calidad de"/>
                                <asp:BoundField DataField="documento" HeaderText="No. documento"/> 
                                <asp:BoundField DataField="nombre" HeaderText="Nombre"/>
                                <asp:BoundField DataField="telefono" HeaderText="Teléfono" />
                                <asp:BoundField DataField="direccion" HeaderText="Dirección" />
                                <asp:BoundField DataField="correo" HeaderText="Correo" />
                                <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                                    <HeaderTemplate>
                                        <div class="btn-group">
                                            <asp:LinkButton ID="btnInteresadoAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnInteresadoAdd_Click">
										                        <i class="fas fa-plus"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </HeaderTemplate>
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
                            <EmptyDataRowStyle CssClass="gvEmpty" /> 
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

