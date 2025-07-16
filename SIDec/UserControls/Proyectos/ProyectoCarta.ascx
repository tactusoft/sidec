<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProyectoCarta.ascx.cs" Inherits="SIDec.UserControls.Particular.ProyectoCarta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/SliderImages.ascx" TagName="SliderImages" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>
<script type="text/javascript" src="./UserControls/Proyectos/Proyectos.js"></script>

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc1:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc1:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upProyectoCarta" runat="server" UpdateMode="Conditional">
    <ContentTemplate>


        <asp:HiddenField ID="hddReferenceID" runat="server" Value="0" />
        <asp:HiddenField ID="hddId" runat="server" Value="UserControlProyectoCarta" />
        <asp:HiddenField ID="hddIdReferenceType" runat="server" Value="0" />
        <asp:HiddenField ID="hddProyectoCartaPrimary" runat="server" Value="0" />
        <%--************************************************** Alert Msg Main **************************************************************--%>
        <asp:UpdatePanel runat="server" ID="upProyectoCartaFoot" UpdateMode="Conditional">
            <ContentTemplate>

                <div id="divData" runat="server" class="main-section">
                    <div class="row">
                        <asp:UpdatePanel ID="upMsgMain" runat="server" UpdateMode="Conditional" class="alert-main msgusercontrol">
                            <ContentTemplate>
                                <div runat="server" id="msgProyectoCartaes" class="alert d-none" role="alert"></div>
                                <div runat="server" id="msgProyectoCartaesMain" class="d-none" role="alert">
                                    <span runat="server" id="msgMainText"></span>
                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pProyectoCartaExecAction" runat="server">
                            <div class="col-auto card-header-buttons" style="padding-top: .75rem; padding-bottom: .25rem;">
                                <div class="btn-group flex-wrap">
                                    <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgProyectoCarta" OnClick="btnAccept_Click">
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
                        <asp:Panel ID="pProyectoCartaAction" runat="server">
                            <div class="col-auto ml-auto card-header-buttons">
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnProyectoCartaList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnProyectoCartaAccion_Click">
										                                            <i class="fas fa-th-list"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnProyectoCartaAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnProyectoCartaAccion_Click">
										                                            <i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnProyectoCartaEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnProyectoCartaAccion_Click">
										                                            <i class="fas fa-pencil-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnProyectoCartaDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnProyectoCartaAccion_Click">
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
                <asp:AsyncPostBackTrigger ControlID="btnProyectoCartaList" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoCartaAdd" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoCartaEdit" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnProyectoCartaDel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="gvProyectoCartas" />
            </Triggers>
        </asp:UpdatePanel>


        <asp:Panel ID="pnlPBProyectoCartaActions" runat="server" Width="100%">
            <div runat="server" id="dvProyectoCartaActions" class="ml0 mtb10 d-i">
                <div class="form-group mt-2">
                    <asp:HiddenField runat="server" ID="hdd_idproyecto_carta" Value="0" />
                    <div class="div_auditoria">
                        <asp:Label ID="lbl_fec_auditoria_proyecto_carta" runat="server" />
                    </div>
                    <div class="row row first-row row-cols-1 row-cols-sm-2 row-cols-md-4 row-cols-xl-6">
                        <asp:TextBox runat="server" ID="txt_au_proyecto_carta" Enabled="false" Visible="false"></asp:TextBox>
                        <asp:TextBox runat="server" ID="txt_cod_proyecto__carta" Enabled="false" Visible="false"></asp:TextBox>
                        <div class="form-group-sm col">
                            <label for="txt_radicado_manifestacion_interes" class="">Radicado manifestación interés</label>
                            <asp:TextBox runat="server" ID="txt_radicado_manifestacion_interes" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col">
                            <label for="txt_fecha_radicado_manifestacion_interes" class="">Fecha radicado m.i.</label>
                            <asp:RegularExpressionValidator ID="revfecha_radicado_manifestacion_interes" runat="server" ValidationGroup="vgProyectosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_radicado_manifestacion_interes" Display="Dynamic">
                                                                        <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                            </asp:RegularExpressionValidator>
                            <asp:RangeValidator runat="server" ID="rvfecha_radicado_manifestacion_interes" ValidationGroup="vgProyectosCartas" ControlToValidate="txt_fecha_radicado_manifestacion_interes">
                                                                        <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                            </asp:RangeValidator>
                            <ajax:CalendarExtender ID="cal_fecha_radicado_manifestacion_interes" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_radicado_manifestacion_interes" PopupButtonID="txt_fecha_radicado_manifestacion_interes" Format="yyyy-MM-dd" />
                            <asp:TextBox runat="server" ID="txt_fecha_radicado_manifestacion_interes" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col">
                            <label for="txt_radicado_carta_intencion" class="">Radicado carta intención</label>
                            <asp:TextBox runat="server" ID="txt_radicado_carta_intencion" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col">
                            <label for="txt_fecha_radicado_carta_intencion" class="">Fecha radicado c.i.</label>
                            <asp:RegularExpressionValidator ID="revfecha_radicado_carta_intencion" runat="server" ValidationGroup="vgProyectosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_radicado_carta_intencion" Display="Dynamic">
                                                                        <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                            </asp:RegularExpressionValidator>
                            <asp:RangeValidator runat="server" ID="rvfecha_radicado_carta_intencion" ValidationGroup="vgProyectosCartas" ControlToValidate="txt_fecha_radicado_carta_intencion">
                                                                        <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                            </asp:RangeValidator>
                            <ajax:CalendarExtender ID="cal_fecha_radicado_carta_intencion" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_radicado_carta_intencion" PopupButtonID="txt_fecha_radicado_carta_intencion" Format="yyyy-MM-dd" />
                            <asp:TextBox runat="server" ID="txt_fecha_radicado_carta_intencion" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col">
                            <label for="txt_radicado_otrosi" class="">Radicado otrosí</label>
                            <asp:TextBox runat="server" ID="txt_radicado_otrosi" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col">
                            <label for="txt_fecha_radicado_otrosi" class="">Fecha radicado otrosí</label>
                            <asp:RegularExpressionValidator ID="revfecha_radicado_otrosi" runat="server" ValidationGroup="vgProyectosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_radicado_otrosi" Display="Dynamic">
                                                                        <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                            </asp:RegularExpressionValidator>
                            <asp:RangeValidator runat="server" ID="rvfecha_radicado_otrosi" ValidationGroup="vgProyectosCartas" ControlToValidate="txt_fecha_radicado_otrosi">
                                                                        <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                            </asp:RangeValidator>
                            <ajax:CalendarExtender ID="cal_fecha_radicado_otrosi" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_radicado_otrosi" PopupButtonID="txt_fecha_radicado_otrosi" Format="yyyy-MM-dd" />
                            <asp:TextBox runat="server" ID="txt_fecha_radicado_otrosi" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col">
                            <label for="ddl_id_documento_constitucion_proyecto" class="">Documento constitución</label>
                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgProyectosCartas" ControlToValidate="ddl_id_documento_constitucion_proyecto">
                                                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                            <asp:DropDownList runat="server" ID="ddl_id_documento_constitucion_proyecto" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group-sm col">
                            <label for="txt_tiempo_desarrollo_proyecto" class="">Tiempo desarrollo en meses</label>
                            <asp:TextBox runat="server" ID="txt_meses_desarrollo" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col">
                            <label for="txt_unidad_gestion_aplica_proyecto" class="">Unidad de gestión que aplica</label>
                            <asp:TextBox runat="server" ID="txt_unidad_gestion_aplica_proyecto" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col">
                            <label for="txt_etapa_aplica_proyecto" class="">Etapa en la que aplica</label>
                            <asp:TextBox runat="server" ID="txt_etapa_aplica_proyecto" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col">
                            <label for="txt_area_util__carta" class="">Área útil</label>
                            <div class="input-group input-group-xs">
                                <asp:TextBox runat="server" ID="txt_area_util__carta" CssClass="form-control form-control-xs" MaxLength="13" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                <div class="input-group-append">
                                    <span class="input-group-text">m<sup>2</sup></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group-sm col">
                            <label for="txt_area_minima_vivienda" class="">Área mínima vivienda</label>
                            <asp:TextBox runat="server" ID="txt_area_minima_vivienda" CssClass="form-control form-control-xs" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col col-md-3 col-lg-3 col-xl-4">
                            <label for="txt_localizacion_proyecto" class="">Localización proyecto</label>
                            <asp:TextBox runat="server" ID="txt_localizacion_proyecto" CssClass="form-control form-control-xs"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col col-md-3 col-lg-3 col-xl-2 va-m">
                            <asp:Label ID="lblcarta_intencion_firmada" runat="server" />
                            <asp:CheckBox runat="server" ID="chk_carta_intencion_firmada" CssClass="" Text="Carta intención firmada" />
                        </div>
                        <div class="form-group-sm col">
                            <label for="txt_fecha_firma" class="">Fecha de firma</label>
                            <asp:RegularExpressionValidator ID="rev_fecha_firma" runat="server" ValidationGroup="vgProyectosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_firma" Display="Dynamic">
                                                                        <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                            </asp:RegularExpressionValidator>
                            <asp:RangeValidator runat="server" ID="rvfecha_firma" ValidationGroup="vgProyectosCartas" ControlToValidate="txt_fecha_firma">
                                                                        <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                            </asp:RangeValidator>
                            <br />
                            <ajax:CalendarExtender ID="cal_fecha_firma" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_firma" PopupButtonID="txt_fecha_firma" Format="yyyy-MM-dd" />
                            <asp:TextBox runat="server" ID="txt_fecha_firma" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                        </div>

                        <div class="form-group-sm col col-sm-6 col-md-3 col-xl-4 va-m">
                            <div style="float: left; min-width: 50px;">
                                <asp:LinkButton ID="lblPdfCarta" runat="server" CssClass="btn btn-danger btn-sm" Text="" CausesValidation="false" OnClick="lblPdfCarta_Click" ToolTip="Ver documento">
						                    <i class="fas fa-file-pdf"></i>
                                </asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="lbLLoadCarta" runat="server" CssClass="btn btn-success btn-sm btnLoad" Text="" CausesValidation="false">
											<i class="fas fa-upload"></i>
                                        </asp:LinkButton>&nbsp;
                            </div>
                            <div class="labelLimited" style="padding-left: 10px;">
                                <asp:HiddenField ID="hdd_ruta_carta" runat="server" Value="" />
                                <asp:Label ID="lblInfoFileCarta" runat="server" />
                                <asp:TextBox ID="txt_ruta_carta" runat="server" Style="display: none" />
                                <asp:Label ID="lblErrorFileCarta" runat="server" CssClass="error" />
                                <ajax:AsyncFileUpload ID="fuLoadCarta" runat="server" onchange="infoFileCarta();" Style="display: none" PersistFile="true"
                                    CompleteBackColor="Transparent" ErrorBackColor="Red" />
                            </div>
                        </div>
                    </div>
                    <div class="card form-group bg-light mt-2">
                        <div class="card-header">Unidades comprometidas</div>
                        <div class="card-body">
                            <div class="row row-cols-1 row-cols-sm-3 row-cols-md-6 row-cols-lg-6">
                                <div class="form-group-sm col">
                                    <label for="txt_UP_VIP__carta" class="">VIP</label>
                                    <asp:TextBox runat="server" ID="txt_UP_VIP__carta" CssClass="form-control form-control-xs" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                </div>
                                <div class="form-group-sm col">
                                    <label for="txt_UP_VIS__carta" class="">VIS</label>
                                    <asp:TextBox runat="server" ID="txt_UP_VIS__carta" CssClass="form-control form-control-xs" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                </div>
                                <div class="form-group-sm col">
                                    <label for="txt_UP_E3" class="">Estrato 3</label>
                                    <asp:TextBox runat="server" ID="txt_UP_E3" CssClass="form-control form-control-xs" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                </div>
                                <div class="form-group-sm col">
                                    <label for="txt_UP_E4" class="">Estrato 4</label>
                                    <asp:TextBox runat="server" ID="txt_UP_E4" CssClass="form-control form-control-xs" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                </div>
                                <div class="form-group-sm col">
                                    <label for="txt_UP_E5" class="">Estrato 5</label>
                                    <asp:TextBox runat="server" ID="txt_UP_E5" CssClass="form-control form-control-xs" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                </div>
                                <div class="form-group-sm col">
                                    <label for="txt_UP_E6" class="">Estrato 6</label>
                                    <asp:TextBox runat="server" ID="txt_UP_E6" CssClass="form-control form-control-xs" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="row">
                        <div class="form-group-sm col">
                            <label for="txt_observacion__carta" class="">Observaciones</label>
                            <asp:TextBox runat="server" ID="txt_observacion__carta" CssClass="form-control form-control-xs"
                                TextMode="MultiLine" onKeyDown="MaxLengthText(this,2000);" onKeyUp="MaxLengthText(this,2000);" MaxLength="2000"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="gv-w">
                    <asp:GridView ID="gvProyectoCartas" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" AutoGenerateColumns="False" SAllowSorting="true"
                        DataKeyNames="au_proyecto_carta,carta_intencion_firmada,meses_desarrollo,fecha_firma,ruta_carta,proy_en_proceso" EmptyDataText="No hay registros asociados" howHeaderWhenEmpty="true"
                        OnRowCommand="gvProyectoCartas_RowCommand" OnRowDataBound="gvProyectoCartas_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="au_proyecto_carta" HeaderText="au_proyecto_carta" Visible="false" />
                            <asp:BoundField DataField="radicado_manifestacion_interes" HeaderText="Radicado manifestación interés" />
                            <asp:BoundField DataField="fecha_radicado_manifestacion_interes" HeaderText="Fecha radicado manifestación interés" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-CssClass="t-c w120" />
                            <asp:BoundField DataField="radicado_carta_intencion" HeaderText="Radicado carta interés" />
                            <asp:BoundField DataField="fecha_radicado_carta_intencion" HeaderText="Fecha radicado carta intención" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-CssClass="t-c w120" />
                            <asp:BoundField DataField="documento_constitucion_proyecto" HeaderText="Documento constitución proyecto" />
                            <asp:TemplateField HeaderText="Carta intención firmada" ItemStyle-CssClass="t-c w100">
                                <ItemTemplate><%# Convert.ToString(Eval("carta_intencion_firmada")) == "1" ? "Si" : "" %></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="fecha_firma" HeaderText="Fecha firma" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-CssClass="t-c w120" />
                            <asp:TemplateField HeaderText="Tiempo de desarrollo en meses" ItemStyle-CssClass="t-c w100">
                                <ItemTemplate>
                                    <asp:Label ID="lblSemaforo" runat="server" Style="min-width: 75px;"><i class="fas fa-circle"></i></asp:Label>
                                    <asp:Label ID="lblDiff" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="area_util" HeaderText="Área útil (m2)" DataFormatString="{0:N2}" ItemStyle-CssClass="t-r" />
                            <asp:BoundField DataField="area_minima_vivienda" HeaderText="Área mínima vivienda" />
                            <asp:BoundField DataField="UP_VIP" HeaderText="Unidades VIP" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                            <asp:BoundField DataField="UP_VIS" HeaderText="Unidades VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="t-r" />
                            <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                                <HeaderTemplate>
                                    <div class="btn-group">
                                        <asp:LinkButton ID="btnCartaAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnCartaAdd_Click">
										                        <i class="fas fa-plus"></i>
                                        </asp:LinkButton>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn btn-grid-detail" Text="Detalle" CausesValidation="false" CommandName="_Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Visualizar registro"> 
										        <i class="fas fa-info-circle"></i>
                                        </asp:LinkButton>
                                        <asp:ImageButton runat="server" CommandName="OpenFile" CommandArgument='<%# Container.DisplayIndex %>' ImageUrl="~/images/icon/pdf-icon.png" Visible='<%# (String.IsNullOrEmpty(Eval("ruta_carta").ToString())) ? false : true %>' ToolTip="Abrir documento" />
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

