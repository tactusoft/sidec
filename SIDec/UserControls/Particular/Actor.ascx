<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Actor.ascx.cs" Inherits="SIDec.UserControls.Particular.Actor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/SliderImages.ascx" TagName="SliderImages" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc1:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc1:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>



<asp:UpdatePanel ID="upActor" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:HiddenField ID="hddReferenceID" runat="server" Value="0" />
        <asp:HiddenField ID="hddId" runat="server" Value="UserControlActor" />
        <asp:HiddenField ID="hddIdReferenceType" runat="server" Value="0" />
        <asp:HiddenField ID="hddActorPrimary" runat="server" Value="0" />

        <%--************************************************** Alert Msg Main **************************************************************--%>
        <asp:UpdatePanel runat="server" ID="upActorFoot" UpdateMode="Conditional">
            <ContentTemplate>

                <div id="divData" runat="server" class="dvRelative">
                    <div class="row">
                        <asp:UpdatePanel ID="upMsgMain" runat="server" UpdateMode="Conditional" class="alert-main msgusercontrol">
                            <ContentTemplate>
                                <div runat="server" id="msgActores" class="alert d-none" role="alert"></div>
                                <div runat="server" id="msgActoresMain" class="d-none" role="alert">
                                    <span runat="server" id="msgMainText"></span>
                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pActorExecAction" runat="server">
                            <div class="col-auto card-header-buttons" style="padding-top: .75rem; padding-bottom: .25rem;">
                                <div class="btn-group flex-wrap">
                                    <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgActor" OnClick="btnAccept_Click">
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
                            <asp:Panel ID="pActorAction" runat="server">
                        <div class="col-auto ml-auto card-header-buttons">
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnActorList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnActorAccion_Click">
										                                            <i class="fas fa-th-list"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnActorAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnActorAccion_Click">
										                                            <i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnActorEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnActorAccion_Click">
										                                            <i class="fas fa-pencil-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnActorDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnActorAccion_Click">
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
                <asp:AsyncPostBackTrigger ControlID="btnActorList" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnActorAdd" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnActorEdit" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnActorDel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="gvActors" />
            </Triggers>
        </asp:UpdatePanel>


        <asp:Panel ID="pnlPBActorActions" runat="server" Width="100%">
            <div runat="server" id="dvActorActions" class="ml0 mtb10 d-i">
                <div class="card form-group bg-light mt-2">
                    <div class="card-body">
                        <asp:HiddenField runat="server" ID="hdd_idpersona" Value="0" />
                        <asp:HiddenField runat="server" ID="hdd_idactor" Value="0" />

                        <div class="row row-cols-12 ">
                            <div class="form-group-sm col-6 col-sm-4">
                                <label for="ddl_idtipo_actor" class="">Tipo responsable </label>
                                <asp:RequiredFieldValidator ID="rfv_idtipo_actor" runat="server" SetFocusOnError="true" ForeColor="Red" Display="Dynamic"
                                    ValidationGroup="vgActor" ControlToValidate="ddl_idtipo_actor" CssClass="invalid-feedback">
                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                </asp:RequiredFieldValidator>
                                <asp:DropDownList runat="server" ID="ddl_idtipo_actor" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                </asp:DropDownList>

                            </div>
                            <div class="form-group-sm col-3 col-sm-4 col-md-5 col-lg-6 div_auditoria">
                                <asp:Label ID="lbl_fec_auditoria_actor" runat="server" /></div>
                        </div>
                        <div class="row row-cols-12 ">
                            <div class="form-group-sm col-md-2 col-sm-6 col-12 ">
                                <label for="ddl_idtipo_documento" class="">Tipo documento</label>
                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ForeColor="Red" Display="Dynamic"
                                    ValidationGroup="vgActor" ControlToValidate="ddl_idtipo_documento">
                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                </asp:RequiredFieldValidator>
                                <asp:DropDownList runat="server" ID="ddl_idtipo_documento" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group-sm col-md-2 col-sm-6 col-12 ">
                                <label for="txt_documento" class="">No. documento</label>
                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ForeColor="Red" Display="Dynamic"
                                    ValidationGroup="vgActor" ControlToValidate="txt_documento">
                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                </asp:RequiredFieldValidator>
                                <asp:TextBox runat="server" ID="txt_documento" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbdocumento" runat="server" FilterType="Numbers, Custom" ValidChars="-" TargetControlID="txt_documento" />
                            </div>
                            <div class="form-group-sm col-md-5 col-sm-6 col-12 ">
                                <label for="txt_nombre" class="">Nombre</label>
                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" ForeColor="Red" Display="Dynamic"
                                    ValidationGroup="vgActor" ControlToValidate="txt_nombre">
                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                </asp:RequiredFieldValidator>
                                <asp:TextBox runat="server" ID="txt_nombre" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbnombre" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" InvalidChars="<>" TargetControlID="txt_nombre" FilterMode="InvalidChars" />
                            </div>
                            <div class="form-group-sm col-md-3 col-sm-6 col-12 ">
                                <label for="txt_telefono" class="">Teléfono</label>
                                <asp:TextBox runat="server" ID="txt_telefono" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbtelefono" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars=" -/.,;_" FilterMode="ValidChars" TargetControlID="txt_telefono" />
                            </div>
                            <div class="form-group-sm col-sm-6 col-12 ">
                                <label for="txta_direccion" class="">Dirección</label>
                                <asp:TextBox runat="server" ID="txta_direccion" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbdireccion" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" InvalidChars="<>" TargetControlID="txta_direccion" FilterMode="InvalidChars" />
                            </div>
                            <div class="form-group-sm col-sm-6 col-12 ">
                                <label for="txt_correo" class="">Correo</label>
                                <asp:RegularExpressionValidator ID="revcorreo" runat="server" SetFocusOnError="True" CssClass="invalid-feedback" Display="Dynamic"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="vgActor" ControlToValidate="txt_correo">
                                        <uc:ToolTip width="130px" ToolTip="Correo inválido" runat="server"/>
                                </asp:RegularExpressionValidator>
                                <asp:TextBox runat="server" ID="txt_correo" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbcorreo" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" InvalidChars="<>" TargetControlID="txt_correo" FilterMode="InvalidChars" />
                            </div>
                        </div>
                    </div>
                    <div class="card-header">Representante</div>
                    <div class="card-body">
                        <asp:HiddenField runat="server" ID="hdd_idpersona_representante" Value="0" />

                        <div class="row row-cols-12 ">
                            <div class="form-group-sm col-md-2 col-sm-6 col-12 ">
                                <label for="ddl_idtipo_documento_rep" class="">Tipo documento</label>
                                <asp:DropDownList runat="server" ID="ddl_idtipo_documento_rep" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group-sm col-md-2 col-sm-6 col-12 ">
                                <label for="txt_documento_rep" class="">No. documento</label>
                                <asp:TextBox runat="server" ID="txt_documento_rep" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbdocumentorep" runat="server" FilterType="Numbers, Custom" ValidChars="-" FilterMode="ValidChars" TargetControlID="txt_documento_rep" />
                            </div>
                            <div class="form-group-sm col-md-5 col-sm-6 col-12 ">
                                <label for="txt_nombre_rep" class="">Nombre</label>
                                <asp:TextBox runat="server" ID="txt_nombre_rep" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbnombrerep" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" InvalidChars="<>" FilterMode="InvalidChars" TargetControlID="txt_nombre_rep" />
                            </div>
                            <div class="form-group-sm col-md-3 col-sm-6 col-12 ">
                                <label for="txt_telefono_rep" class="">Teléfono</label>
                                <asp:TextBox runat="server" ID="txt_telefono_rep" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbtelefonorep" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars=" -/.,;_" FilterMode="ValidChars" TargetControlID="txt_telefono_rep" />
                            </div>
                            <div class="form-group-sm col-sm-6 col-12 ">
                                <label for="txt_direccion_rep" class="">Dirección</label>
                                <asp:TextBox runat="server" ID="txt_direccion_rep" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbdireccionrep" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" InvalidChars="<>" TargetControlID="txt_direccion_rep" FilterMode="InvalidChars" />
                            </div>
                            <div class="form-group-sm col-sm-6 col-12 ">
                                <label for="txt_correo_rep" class="">Correo</label>
                                <asp:RegularExpressionValidator ID="revcorreorep" runat="server" SetFocusOnError="True" CssClass="invalid-feedback" Display="Dynamic"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="vgActor" ControlToValidate="txt_correo_rep">
                                            <uc:ToolTip width="130px" ToolTip="Correo inválido" runat="server"/>
                                </asp:RegularExpressionValidator>
                                <asp:TextBox runat="server" ID="txt_correo_rep" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbcorreorep" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" InvalidChars="<>" TargetControlID="txt_correo_rep" FilterMode="InvalidChars" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="gv-w">
                    <asp:GridView ID="gvActors" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="idactor, nombre" EmptyDataText="No hay registros asociados"
                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" AllowSorting="true" OnRowCommand="gvActors_RowCommand" OnRowDataBound="gvActors_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="tipo_actor" HeaderText="Responsable" SortExpression="tipo_actor" />
                            <asp:BoundField DataField="nombre" HeaderText="Nombre" SortExpression="nombre" />
                            <asp:BoundField DataField="tip_doc" HeaderText="Tipo doc." />
                            <asp:BoundField DataField="documento" HeaderText="Núm documento" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="telefono" HeaderText="Teléfono" />
                            <asp:BoundField DataField="direccion" HeaderText="Dirección" />
                            <asp:BoundField DataField="correo" HeaderText="Correo" />
                            <asp:BoundField DataField="nombre_rep" HeaderText="Representante" />
                            <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                                <HeaderTemplate>
                                    <div class="btn-group">
                                        <asp:LinkButton ID="btnActorAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnActorAdd_Click">
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
    </ContentTemplate>
</asp:UpdatePanel>

