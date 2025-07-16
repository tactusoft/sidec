<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Activity.ascx.cs" Inherits="SIDec.UserControls.Particular.Activity" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/ListBox.ascx" TagName="ListBox" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>


<asp:UpdatePanel runat="server" ID="upActivityFoot" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divData" runat="server">
            <div class="row">
                <asp:UpdatePanel runat="server" ID="upActivityMsg" UpdateMode="Conditional" class="alert-card card-header-message uc-div-message">
                    <ContentTemplate>
                        <div runat="server" id="msgActivity" class="alert d-none" role="alert"></div>
                        <div runat="server" id="msgActivityMain" class="d-none" role="alert">
                            <span runat="server" id="msgMainText"></span>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <asp:ValidationSummary runat="server" ID="vsActivity" DisplayMode="SingleParagraph" CssClass="invalid-feedback" HeaderText="Falta informar: " ShowSummary="true" ShowValidationErrors="true" ValidationGroup="vgActivity" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="row">
                <div class="col-auto card-header-buttons" style="padding-top: .75rem; padding-bottom: .25rem;">
                    <asp:Panel ID="pActivityExecAction" runat="server">
                        <div class="btn-group flex-wrap">
                            <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgActivity" OnClick="btnAccept_Click">
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
                <div class="col-auto ml-auto card-header-buttons">
                    <asp:Panel ID="pActivityAction" runat="server">
                        <div class="btn-group">
                            <asp:LinkButton ID="btnActivityList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnActivityAccion_Click">
							    <i class="fas fa-th-list"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnActivityAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnActivityAccion_Click">
								 <i class="fas fa-plus"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnActivityEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnActivityAccion_Click">
							    <i class="fas fa-pencil-alt"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnActivityDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnActivityAccion_Click">
							    <i class="far fa-trash-alt"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnActivityExcel" runat="server" CssClass="btn" Text="Ficha" CausesValidation="false" CommandName="Excel" CommandArgument="5" ToolTip="Generar matriz" OnClick="btnActivityAccion_Click">
								<i class="far fa-file-excel"></i>
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnActivityList" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnActivityAdd" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnActivityEdit" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnActivityDel" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="gvActivity" />
        <asp:PostBackTrigger ControlID="btnActivityExcel" />
    </Triggers>
</asp:UpdatePanel>



<asp:UpdatePanel ID="upActivity" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hddId" runat="server" Value="UserControlActivity" />
        <asp:HiddenField ID="hddIdBanco" runat="server" Value="0" />
        <asp:Panel ID="pnlGrid" runat="server">
            <div>
                <asp:CheckBox runat="server" ID="chk_activas" Text=" Solo activas" CssClass="fwb" Checked="true" OnCheckedChanged="chk_activas_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
            </div>
            <div class="gv-w" style="overflow-x: scroll">
                <asp:GridView ID="gvActivity" CssClass="gv" runat="server" PageSize="20" AllowPaging="true" AutoGenerateColumns="false"
                    DataKeyNames="idbanco_actividad,activo,dias_disponibles,dias_en_tramite,fec_finalizacion,semaforo,porcentajeesperado" ShowHeaderWhenEmpty="true" AllowSorting="true"
                    OnRowDataBound="gvActivity_RowDataBound" OnPageIndexChanging="gvActivity_PageIndexChanging" OnRowCommand="gvActivity_RowCommand">
                    <Columns>
                        <asp:TemplateField ShowHeader="true" HeaderText="Clave" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <div class="text-primary fs12 fwb">
                                    <asp:Label ID="lblClave" runat="server" Visible='<%# Eval("clave").ToString()=="1"%>'><i class="fas fa-check"></i></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true" HeaderText="Fase">
                            <ItemTemplate>
                                <div class="gv-td-tl">
                                    <asp:Label ID="lblEstado" runat="server" Text='<%# Eval("estado_actividad").ToString()%>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true" HeaderText="Actividad">
                            <ItemTemplate>
                                <div class="gv-td-tl">
                                    <asp:Label ID="lblActividad" runat="server" Text='<%# Eval("nombre").ToString()%>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="entidad" HeaderText="Entidad" />
                        <asp:BoundField DataField="fec_inicio" HeaderText="Fec. inicio trámite del promotor" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="fec_culminacion" HeaderText="Fec. estimada culminación trámite" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="dias_en_tramite" HeaderText="Días en trámite" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="fec_finalizacion" HeaderText="Fec. finalización trámite" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField ShowHeader="true" HeaderText="Semáforo" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="mw80">
                            <ItemTemplate>
                                <asp:Label ID="lblSemaforo" runat="server" Style="min-width: 75px;"><i class="fas fa-circle fa-stack-2x"></i></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="porccompletado" HeaderText="% Completado" ItemStyle-HorizontalAlign="Right" />

                        <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                            <HeaderTemplate>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnActivityAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="_Add" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnActivityAdd_Click">
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

        <asp:Panel ID="pnlDetail" runat="server" Visible="false">
            <div class="row">
                <div class="form-group-sm col-12 col-md-3">
                    <asp:HiddenField ID="hdd_idbanco_actividad" runat="server" Value="0" />
                    <label class="lblBasic">Fase</label>
                    <asp:RequiredFieldValidator ID="rfv_estado_actividad" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                        ValidationGroup="vgActivity" ControlToValidate="ddl_idestado_actividad">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="ddl_idestado_actividad" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12 col-md-7">
                    <label class="lblBasic">Nombre actividad</label>
                    <asp:RequiredFieldValidator ID="rfv_actividad" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                        ValidationGroup="vgActivity" ControlToValidate="txt_actividad">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="txt_actividad" CssClass="form-control form-control-xs" MaxLength="400"></asp:TextBox>
                </div>

                <div class="form-group-sm col-6 col-sm-4 col-md-1">
                    <br />
                    <asp:CheckBox runat="server" ID="chk_Clave" Text=" Clave" CssClass="" Checked="true" />
                </div>

                <div class="form-group-sm col-6 col-sm-4 col-md-1">
                    <br />
                    <asp:CheckBox runat="server" ID="chk_activo" Text=" Activo" CssClass="" Checked="true" />
                </div>
                <div class="col-6 col-sm-4 col-md-3 col-lg-3">
                    <div class="form-group-sm col">
                        <div style="width: 80%; float: left;" class="text-truncate">
                            <asp:Label ID="lbl_fecinicio" runat="server" class="lblBasic" ToolTip="Fecha de inicio trámite radicación del promotor">Fecha de inicio trámite radicación del promotor</asp:Label>
                        </div>
                        <div style="right: 0px; position: relative;">
                            <asp:RegularExpressionValidator ID="rev_fec_inicio" runat="server" ValidationGroup="vgActivity" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fec_inicio" Display="Dynamic">
                            <uc:ToolTip width="150px" ToolTip="Fecha con formato inválido" runat="server"/>
                            </asp:RegularExpressionValidator>
                            <asp:RangeValidator runat="server" ID="rv_fec_inicio" ValidationGroup="vgActivity" ControlToValidate="txt_fec_inicio">
                            <uc:ToolTip width="130px" ToolTip="Fecha inválida" runat="server"/>
                            </asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="rfv_fec_inicio" runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgActivity" ControlToValidate="txt_fec_inicio">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                        </div>
                        <ajax:CalendarExtender ID="ce_fec_inicio" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fec_inicio" PopupButtonID="txt_fec_inicio" Format="yyyy-MM-dd" />
                        <asp:TextBox runat="server" ID="txt_fec_inicio" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group-sm col-6 col-sm-4 col-md-3">
                    <div style="width: 80%; float: left;" class="text-truncate">
                        <asp:Label ID="lbl_fecculminacion" runat="server" class="lblBasic" ToolTip="Fecha estimada culminación trámite entidad competente">Fecha estimada culminación trámite entidad competente</asp:Label>
                    </div>
                    <div style="right: 0px; position: relative;">
                        <asp:RegularExpressionValidator ID="rev_fec_culminacion" runat="server" ValidationGroup="vgActivity" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fec_culminacion" Display="Dynamic">
                            <uc:ToolTip width="150px" ToolTip="Fecha con formato inválido" runat="server"/>
                        </asp:RegularExpressionValidator>
                        <asp:RangeValidator runat="server" ID="rv_fec_culminacion" ValidationGroup="vgActivity" ControlToValidate="txt_fec_culminacion">
                            <uc:ToolTip width="130px" ToolTip="Fecha inválida" runat="server"/>
                        </asp:RangeValidator>
                        <asp:RequiredFieldValidator ID="rfv_fec_culminacion" runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgActivity" ControlToValidate="txt_fec_culminacion">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                        </asp:RequiredFieldValidator>
                    </div>
                    <ajax:CalendarExtender ID="ce_fec_culminacion" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fec_culminacion" PopupButtonID="txt_fec_culminacion" Format="yyyy-MM-dd" />
                    <asp:TextBox runat="server" ID="txt_fec_culminacion" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8"></asp:TextBox>
                </div>
                <div class="col-6 col-sm-4 col-md-3">
                    <div class="form-group-sm col">
                        <div style="width: 80%; float: left;" class="text-truncate">
                            <asp:Label ID="lbl_fecfinalizacion" runat="server" class="lblBasic" ToolTip="Fecha finalizacion trámite / gestión">Fecha finalizacion trámite / gestión</asp:Label>
                        </div>
                        <div style="right: 0px; position: relative;">
                            <asp:RegularExpressionValidator ID="rev_fec_finalizacion" runat="server" ValidationGroup="vgActivity" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fec_finalizacion" Display="Dynamic">
                            <uc:ToolTip width="150px" ToolTip="Fecha con formato inválido" runat="server"/>
                            </asp:RegularExpressionValidator>
                            <asp:RangeValidator runat="server" ID="rv_fec_finalizacion" ValidationGroup="vgActivity" ControlToValidate="txt_fec_finalizacion">
                            <uc:ToolTip width="130px" ToolTip="Fecha inválida" runat="server"/>
                            </asp:RangeValidator>
                        </div>
                        <ajax:CalendarExtender ID="ce_fec_finalizacion" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fec_finalizacion" PopupButtonID="txt_fec_finalizacion" Format="yyyy-MM-dd" />
                        <asp:TextBox runat="server" ID="txt_fec_finalizacion" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group-sm col-6 col-sm-4 col-md-3">
                    <label class="lblBasic">% Completado</label>
                    <asp:RequiredFieldValidator ID="rfv_porc_completado" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                        ValidationGroup="vgActivity" ControlToValidate="txt_porccompletado">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator runat="server" ID="rv_porccompletado" ValidationGroup="vgActivity" ControlToValidate="txt_porccompletado" MinimumValue="0" MaximumValue="100" Type="Double">
                            <uc:ToolTip width="150px" ToolTip="Porcentaje inválido" runat="server"/>
                    </asp:RangeValidator>
                    <asp:TextBox runat="server" ID="txt_porccompletado" CssClass="form-control form-control-xs" MaxLength="6">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftb_porccompletado" runat="server" FilterMode="ValidChars" FilterType="Numbers, Custom" ValidChars="," TargetControlID="txt_porccompletado" />
                </div>
                <div class="form-group-sm col-12 col-md-4 col-lg-3">
                    <label class="lblBasic">Entidad</label>
                    <asp:RequiredFieldValidator ID="rfvEntidad" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                        ValidationGroup="vgActivity" ControlToValidate="ddl_identidad">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="ddl_idEntidad" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12 col-md-4 col-lg-5">
                    <label class="lblBasic">Área dependencia</label>
                    <asp:TextBox runat="server" ID="txt_dependencia" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-sm-8 col-md-4 col-lg-2">
                    <label class="lblBasic">Radicado</label>
                    <asp:TextBox runat="server" ID="txt_radicado" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                </div>
                <div class="form-group-sm col-6 col-sm-4 col-md-3 col-lg-2">
                    <div style="width: 80%; float: left;" class="text-truncate">
                        <asp:Label ID="lblradicado" runat="server" class="lblBasic" ToolTip="Fecha de radicado">Fecha de radicado</asp:Label>
                    </div>
                    <div style="right: 0px; position: relative;">
                        <asp:RegularExpressionValidator ID="revfecradicado" runat="server" ValidationGroup="vgActivity" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_radicado" Display="Dynamic">
                            <uc:ToolTip width="150px" ToolTip="Fecha con formato inválido" runat="server"/>
                        </asp:RegularExpressionValidator>
                        <asp:RangeValidator runat="server" ID="rvfecharadicado" ValidationGroup="vgActivity" ControlToValidate="txt_fecha_radicado">
                            <uc:ToolTip width="130px" ToolTip="Fecha inválida" runat="server"/>
                        </asp:RangeValidator>
                    </div>
                    

                    <asp:HiddenField runat="server" ID="hdd_fec_finalizacion"></asp:HiddenField>
                    <asp:HiddenField runat="server" ID="hdd_fec_culminacion"></asp:HiddenField>
                    <asp:HiddenField runat="server" ID="hdd_fec_inicio"></asp:HiddenField>
                    <ajax:CalendarExtender ID="cefecharadicado" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_radicado" PopupButtonID="txt_fecha_radicado" Format="yyyy-MM-dd" />
                    <asp:TextBox runat="server" ID="txt_fecha_radicado" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8"></asp:TextBox>

                </div>
                <div class="form-group-sm col-12 col-md-4 col-lg-6">
                    <label class="lblBasic">Contacto / Profesional a cargo</label>
                    <asp:TextBox runat="server" ID="txt_encargado" CssClass="form-control form-control-xs" MaxLength="400"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-md-5 col-lg-6">
                    <label class="lblBasic">Datos de contacto</label>
                    <asp:TextBox runat="server" ID="txt_datos_contacto" CssClass="form-control form-control-xs" MaxLength="1000"></asp:TextBox>
                </div>

                <div class="form-group-sm col-12 col-md-6">
                    <label class="lblBasic">Solicitud</label>
                    <asp:TextBox runat="server" ID="txt_solicitud" CssClass="form-control form-control-xs r2" MaxLength="1000"
                        TextMode="MultiLine" Enabled="false" Rows="2" onKeyDown="MaxLengthText(this,1000);" onKeyUp="MaxLengthText(this,1000);"></asp:TextBox>
                </div>                
                <div class="form-group-sm col-12 col-md-6">                    
                   <uc:ListBox ID="ddl_Entidad" runat="server" Label="Relacionamiento" ValidationGroup="vgActivity" />
                </div>
                <div class="form-group-sm col-12 col-md-6">
                    <label class="lblBasic">Problemática</label>
                    <asp:TextBox runat="server" ID="txt_problematica" CssClass="form-control form-control-xs" MaxLength="4000"
                        TextMode="MultiLine" Enabled="false" Rows="4" onKeyDown="MaxLengthText(this,4000);" onKeyUp="MaxLengthText(this,4000);"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-md-6">
                    <label class="lblBasic">Impacto</label>
                    <asp:TextBox runat="server" ID="txt_impacto" CssClass="form-control form-control-xs " MaxLength="4000"
                        TextMode="MultiLine" Enabled="false" Rows="4" onKeyDown="MaxLengthText(this,4000);" onKeyUp="MaxLengthText(this,4000);"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-md-6">
                    <label class="lblBasic">Gestión adelantada por SGS</label>
                    <asp:TextBox runat="server" ID="txt_gestion_sgs" CssClass="form-control form-control-xs r3" MaxLength="2000"
                        TextMode="MultiLine" Enabled="false" Rows="3" onKeyDown="MaxLengthText(this,4000);" onKeyUp="MaxLengthText(this,2000);"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-md-6">
                    <label class="lblBasic">Gestión que se propone debe adelantarse</label>
                    <asp:TextBox runat="server" ID="txt_gestion_adelantar" CssClass="form-control form-control-xs r3" MaxLength="2000"
                        TextMode="MultiLine" Enabled="false" Rows="3" onKeyDown="MaxLengthText(this,4000);" onKeyUp="MaxLengthText(this,2000);"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
