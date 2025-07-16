<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ProyectoPrediosUC.ascx.cs" Inherits="SIDec.UserControls.Acompanamiento.ProyectoPrediosUC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>
    <script type="text/javascript" src="./UserControls/ComIntersectorial/ComIntersectorial.js"></script>

<asp:HiddenField ID="hddProyectoPredioPrimary" runat="server" Value="0" />
<asp:HiddenField ID="hddId" runat="server" Value="UserControlProyectoPredios" />
<asp:HiddenField ID="hddIdProyecto" runat="server" Value="0" />

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>


<asp:UpdatePanel ID="upProyectoPredio" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:UpdatePanel runat="server" ID="upProyectoPredioFoot" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divData" runat="server" class="main-section">
                    <div class="row">
                        <asp:UpdatePanel runat="server" ID="upProyectoPredioMsg" UpdateMode="Conditional" class="alert-main msgusercontrol">
                            <ContentTemplate>
                                <div runat="server" id="msgProyectoPredio" class="alert d-none" role="alert"></div>
                                <div runat="server" id="msgProyectoPredioMain" class="d-none" role="alert">
                                    <span runat="server" id="msgMainText"></span>
                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <asp:ValidationSummary runat="server" ID="vsProyectoPredio" DisplayMode="SingleParagraph" CssClass="invalid-feedback" HeaderText="Falta informar: " ShowSummary="true" ShowValidationErrors="true" ValidationGroup="vgProyectoPredio" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pProyectoPredioExecAction" runat="server">
                            <div class="col-auto card-header-buttons" >
                                <div class="btn-group flex-wrap">
                                    <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgProyectoPredio" OnClick="btnAccept_Click">
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
                        <asp:Panel ID="pProyectoPredioAction" runat="server">
                            <div class="col-auto ml-auto card-header-buttons">
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnProyectoPredioList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnProyectoPredioAccion_Click">
							            <i class="fas fa-th-list"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnProyectoPredioAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnProyectoPredioAccion_Click">
								        <i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnProyectoPredioEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnProyectoPredioAccion_Click">
								        <i class="fas fa-pencil-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnProyectoPredioDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnProyectoPredioAccion_Click">
								        <i class="far fa-trash-alt"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProyectoPredioList" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoPredioAdd" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoPredioEdit" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoPredioDel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="gvProyectoPredio" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:Panel ID="pnlGrid" runat="server">
            <div class="gv-w">
                <asp:GridView ID="gvProyectoPredio" CssClass="gv" runat="server" PageSize="500" AllowPaging="true" DataKeyNames="au_proyecto_predio,chip" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
                    OnRowDataBound="gvProyectoPredio_RowDataBound" OnPageIndexChanging="gvProyectoPredio_PageIndexChanging" OnRowCommand="gvProyectoPredio_RowCommand" EmptyDataText="No se encontraron registros">
                    <Columns>
                        <asp:BoundField DataField="chip" HeaderText="Chip" ItemStyle-CssClass="t-c" />
                        <asp:BoundField DataField="matricula" HeaderText="Matrícula Inm." ItemStyle-CssClass="t-c" />
                        <asp:TemplateField HeaderText="Es predio declarado" ItemStyle-CssClass="t-c">
                            <ItemTemplate><%# Convert.ToString(Eval("declaratoria")) != "" ? "Si" : "No" %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cumple función social" ItemStyle-CssClass="t-c">
                            <ItemTemplate><%# Convert.ToString(Eval("cumple_funcion_social")) != "0" ? "Si" : "No" %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="declaratoria" HeaderText="Declaratoria" />
                        <asp:BoundField DataField="tipo_declaratoria" HeaderText="Tipo declaratoria" />
                        <asp:BoundField DataField="estado_predio_declarado" HeaderText="Estado predio" />
                        <asp:BoundField DataField="causal_acto" HeaderText="Causal acto" />
                        <asp:BoundField DataField="numero_ultimo_acto" HeaderText="Número último acto" />
                        <asp:BoundField DataField="fecha_ultimo_acto" HeaderText="Fecha último acto" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-CssClass="t-c w120" />
                        <asp:BoundField DataField="estado_ultimo_acto" HeaderText="Estado predio último acto" />

                        <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                            <HeaderTemplate>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnProyectoPredioAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="_Add" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnProyectoPredioAdd_Click">
										<i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn btn-grid-detail" Text="Detalle" CausesValidation="false" CommandName="_Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Visualizar registro"> 
									    <i class="fas fa-info-circle"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-grid-edit" Text="Editar" CausesValidation="false" CommandName="_Edit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Editar registro" Visible="false"> 
										<i class="fas fa-pencil-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-grid-delete" Text="Eliminar" CausesValidation="false" CommandName="_Delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Eliminar registro" Visible="false"> 
										<i class="fas fa-trash-alt"></i>
                                    </asp:LinkButton>                                 
                                </div>
                                </itemtemplate>
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle CssClass="gvItemSelected" />
                    <HeaderStyle CssClass="gvHeaderSm" />
                    <RowStyle CssClass="gvItemSm" />
                    <PagerStyle CssClass="gvPager" />
                </asp:GridView>
            </div>
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>


<asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:Panel ID="pnlDetail" runat="server" Visible="false" Width="100%">
            <div class="form-group">
                <asp:Panel ID="pnlProyectoPredio" runat="server" Width="100%">
                    <asp:HiddenField ID="hdd_au_proyecto_predio" runat="server" Value="0" />
                    <div class="row first-row  row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4">
                        <div class="form-group-sm col">
                            <asp:label Id="lblchip" runat="server" style="float:left;">CHIP  <i class="fas fa-search"></i></asp:label>
                            <asp:LinkButton ID="btnGoPredio" runat="server" CssClass="btn btn-outline-warning btn-xs btnLoad" Text="" CausesValidation="false" OnClick="btnGoPredio_Click"  ToolTip="Ir al predio" > 
							    <i class="fas fa-eye"></i> 
                            </asp:LinkButton>
                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectoPredio" ControlToValidate="txt_chip">
                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" ID="txt_chip" CssClass="form-control form-control-xs" OnTextChanged="txt_chip_TextChanged" MaxLength="11" AutoPostBack="true"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbchips" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Numbers" TargetControlID="txt_chip"/>
                        </div>
                        <div class="form-group-sm col">
                            <asp:Label runat="server" ID="lbl_txt_matricula" class="">Matrícula inmobiliaria</asp:Label>
                            <asp:TextBox runat="server" ID="txt_matricula" CssClass="form-control form-control-xs" placeholder="50X-00000000" MaxLength="12"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbmatricula" runat="server" FilterType="Custom,Numbers" ValidChars="csnCSN-" FilterMode="ValidChars" TargetControlID="txt_matricula"/>
                        </div>

                        <div class="form-group-sm col">
                            <asp:Label runat="server" ID="lbl_txt_direccion" class="">Dirección</asp:Label>
                            <asp:TextBox runat="server" ID="txt_direccion" CssClass="form-control form-control-xs" MaxLength="100"></asp:TextBox>
                        </div>

                        <div class="form-group-sm col">
                            <asp:Label runat="server" ID="lbl_txt_declaratoria" class="">Declaratoria</asp:Label>
                            <asp:TextBox runat="server" ID="txt_declaratoria" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="form-group-sm col va-m">
                            <asp:CheckBox runat="server" ID="chk_cumple_funcion_social" CssClass="" Text="Cumplió función social"/>
                        </div>

                        <div class="form-group-sm col">
                            <asp:Label runat="server" ID="lbl_txt_tipo_declaratoria" class="">Tipo declaratoria</asp:Label>
                            <asp:TextBox runat="server" ID="txt_tipo_declaratoria" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="form-group-sm col">
                            <asp:Label runat="server" ID="lbl_txt_estado_predio_declarado" class="">Estado predio</asp:Label>
                            <asp:TextBox runat="server" ID="txt_estado_predio_declarado" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="form-group-sm col">
                            <asp:Label runat="server" ID="lbl_txt_causal_acto" class="">Causal acto</asp:Label>
                            <asp:TextBox runat="server" ID="txt_causal_acto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="form-group-sm col">
                            <asp:Label runat="server" ID="lbl_txt_numero_ultimo_acto" class="">Número último acto</asp:Label>
                            <asp:TextBox runat="server" ID="txt_numero_ultimo_acto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="form-group-sm col">
                            <asp:Label runat="server" ID="lbl_txt_fecha_ultimo_acto" class="">Fecha último acto</asp:Label>
                            <asp:TextBox runat="server" ID="txt_fecha_ultimo_acto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="form-group-sm col">
                            <asp:Label runat="server" ID="lbl_txt_fecha_ejecutoria_ultimo_acto" class="">Fecha ejecutoria último acto</asp:Label>
                            <asp:TextBox runat="server" ID="txt_fecha_ejecutoria_ultimo_acto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="form-group-sm col">
                            <asp:Label runat="server" ID="lbl_txt_estado_ultimo_acto" class="">Estado último acto</asp:Label>
                            <asp:TextBox runat="server" ID="txt_estado_ultimo_acto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="form-group-sm col">
                            <asp:Label runat="server" ID="lbl_txt_fecha_vencimiento_ultimo_acto" class="">Fecha vencimiento último acto</asp:Label>
                            <asp:TextBox runat="server" ID="txt_fecha_vencimiento_ultimo_acto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group-sm col">
                            <asp:Label runat="server" ID="lbl_txt_observacion__predio" class="">Observaciones</asp:Label>
                            <asp:TextBox runat="server" ID="txt_observacion__predio" CssClass="form-control form-control-xs" 
                                TextMode="MultiLine" MaxLength="2000"  onKeyDown="MaxLengthText(this,2000);" onKeyUp="MaxLengthText(this,2000);" ></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
