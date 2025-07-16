<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Carta.ascx.cs" Inherits="SIDec.UserControls.PrediosDeclarados.Carta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/SliderImages.ascx" TagName="SliderImages" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>
<script type="text/javascript" src="Predios.js"></script>
<script type="text/javascript" src="./UserControls/PrediosDeclarados/Predios.js"></script>

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc1:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc1:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upCarta" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hddReferenceID" runat="server" Value="0" />
        <asp:HiddenField ID="hddId" runat="server" Value="UserControlCarta" />
        <asp:HiddenField ID="hddIdReferenceType" runat="server" Value="0" />
        <asp:HiddenField ID="hddCartaPrimary" runat="server" Value="0" />


        <%--************************************************** Alert Msg Main **************************************************************--%>
        <asp:UpdatePanel runat="server" ID="upCartaFoot" UpdateMode="Conditional">
            <ContentTemplate>

                <div id="divData" runat="server" class="dvRelative" style="right: 14px;">
                    <div class="row">
                        <asp:UpdatePanel ID="upMsgMain" runat="server" UpdateMode="Conditional" class="alert-main msgusercontrol">
                            <ContentTemplate>
                                <div runat="server" id="msgCartas" class="alert d-none" role="alert"></div>
                                <div runat="server" id="msgCartasMain" class="d-none" role="alert">
                                    <span runat="server" id="msgMainText"></span>
                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pCartaExecAction" runat="server">
                            <div class="col-auto card-header-buttons" style="padding-top: .75rem; padding-bottom: .25rem;">
                                <div class="btn-group flex-wrap">
                                    <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgCarta" OnClick="btnAccept_Click">
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
                        <asp:Panel ID="pCartaAction" runat="server">
                            <div class="col-auto ml-auto card-header-buttons">
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnCartaList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnCartaAccion_Click">
										<i class="fas fa-th-list"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnCartaAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnCartaAccion_Click">
										<i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnCartaEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnCartaAccion_Click">
										<i class="fas fa-pencil-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnCartaDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnCartaAccion_Click">
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
                <asp:AsyncPostBackTrigger ControlID="btnCartaList" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCartaAdd" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCartaEdit" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCartaDel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="gvCartas" />
            </Triggers>
        </asp:UpdatePanel>


        <div class="uc-section-shade">
            <asp:Panel ID="pnlPBCartaActions" runat="server" Width="100%">
                <div runat="server" id="dvCartaActions" class="ml0 mtb10 d-i">
                    <div class="card form-group mt-2">
                        <div class="subcard-body">
                            <div class="card-header-main text-center">
                                <ul class="nav nav-pills border-bottom-0 mr-auto">
                                    <li class="nav-item">
                                        <a id="tab_1" class="nav-link active" href="#" data-original-title="" title="">Cartas de Seguimiento</a>
                                    </li>
                                    <li class="nav-item">
                                        <a id="tab_2" class="nav-link" href="#" data-original-title="" title="">Noitificaciones</a>
                                    </li>
                                    <li class="nav-item">
                                        <a id="tab_3" class="nav-link" href="#" data-original-title="" title="">Constancia Ejecutoria</a>
                                    </li>
                                </ul>
                            </div>
                            <div class="card-body bg-light " runat="server" id="mvProyectosSection">
                                <div id="tab-1">
                                    <asp:HiddenField runat="server" ID="hdd_idcarta" Value="0" />
                                    <div class="row">
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblprof_busqueda_info" runat="server">Profesional de búsqueda de información</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_prof_busqueda_info" CssClass="form-control form-control-xs" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblprof_elab_carta" runat="server">Profesional elaboración de carta</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_prof_elab_carta" CssClass="form-control form-control-xs" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblprof_revision" runat="server">Profesional revisión</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_prof_revision" CssClass="form-control form-control-xs" />
                                        </div>                                        
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblprof_elab_carta_par" runat="server">Profesional par elaboración de carta</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_prof_elab_carta_par" CssClass="form-control form-control-xs" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblprof_revision_par" runat="server">Profesional par revisión</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_prof_revision_par" CssClass="form-control form-control-xs" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblenvio_carta" runat="server">Envío de carta</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_envio_carta" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_envio_carta" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_envio_carta" FilterMode="ValidChars" />
                                        </div>


                                        <%--------------------------------------------------------------------------------------------------------------------------------------%>
                                        <div class="uc-center form-group-sm col-12">
                                            <label class="section-title fwb">
                                                Envío de la carta de seguimiento<label>
                                        </div>
                                        <div class="form-group-sm col-4 col-sm-2">
                                            <asp:Label ID="lblradicado_salida" runat="server">No. del radicado de salida</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_radicado_salida" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_radicado_salida" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_radicado_salida" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_radicado_salida" runat="server">Fecha del radicado de salida</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_radicado_salida" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_radicado_salida" Display="Dynamic">
                                                    <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server" />
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_radicado_salida" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_radicado_salida">
                                                    <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server" />
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_radicado_salida" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_radicado_salida" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_radicado_salida" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-12 col-sm-8">
                                            <asp:Label ID="lblobs_rev_info" runat="server">Observaciones revisión de información</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_obs_rev_info" CssClass="form-control form-control-xs" MaxLength="250"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_obs_rev_info" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_obs_rev_info" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblnombre_receptor" runat="server">Nombre del receptor</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_nombre_receptor" CssClass="form-control form-control-xs" MaxLength="120"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_nombre_receptor" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_nombre_receptor" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_entrega" runat="server">Fecha de entrega</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_entrega" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_entrega" Display="Dynamic">
                                                    <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server" />
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_entrega" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_entrega">
                                                    <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server" />
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_entrega" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_entrega" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_entrega" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblmotivo_devolucion" runat="server">Motivo de devolución</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_motivo_devolucion" CssClass="form-control form-control-xs" MaxLength="250"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_motivo_devolucion" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_motivo_devolucion" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_devolucion" runat="server">Fecha de devolución</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_devolucion" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_devolucion" Display="Dynamic">
                                                    <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server" />
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_devolucion" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_devolucion">
                                                    <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server" />
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_devolucion" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_devolucion" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_devolucion" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>

                                        <%------------------------------------------------------------------------------------------------------------%>
                                        <div class="uc-center form-group-sm col-12">
                                            <label class="section-title fwb">Manifestaciones de los propietarios frente a las cartad de seguimiento</label>
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblradicado_entrada" runat="server">No. del radicado de entrada</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_radicado_entrada" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_radicado_entrada" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_radicado_entrada" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_manifestacion" runat="server">Fecha de manifestación</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_manifestacion" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_manifestacion" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server" />
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_manifestacion" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_manifestacion">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server" />
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_manifestacion" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_manifestacion" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_manifestacion" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblresumen_manifestacion" runat="server">Tipo manifestación</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_resumen_manifestacion" CssClass="form-control form-control-xs">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                <asp:ListItem>Sin trámite alguno</asp:ListItem>
                                                <asp:ListItem>Trámite de saneamiento y sin solicitud de licencia</asp:ListItem>
                                                <asp:ListItem>Solicitud de licencia ante Curaduría</asp:ListItem>
                                                <asp:ListItem>Licencia aprobada por Curaduría Urbana.</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lbltipo_licencia" runat="server">Tipo de licencia</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_tipo_licencia" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_tipo_licencia" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_tipo_licencia" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblradicado_licencia" runat="server">No. de radicado de la licencia</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_radicado_licencia" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_radicado_licencia" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_radicado_licencia" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_licencia" runat="server">Fecha de la licencia</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_licencia" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_licencia" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server" />
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_licencia" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_licencia">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server" />
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_licencia" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_licencia" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_licencia" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-8">
                                            <asp:Label ID="lblobs_adicionales" runat="server">Observaciones adicionales</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_obs_adicionales" CssClass="form-control form-control-xs" MaxLength="250"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_obs_adicionales" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_obs_adicionales" FilterMode="ValidChars" />
                                        </div>



                                        <%--------------------------------------------------------------------------------------------------------------------------------------%>
                                        <div class="uc-center form-group-sm col-12">
                                            <label class="section-title fwb">Respuesta de la SDHT a las manifestaciones de los propietarios </label>
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblprof_elab_respuesta" runat="server">Profesional elaboración de respuesta</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_prof_elab_respuesta" CssClass="form-control form-control-xs" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblprof_rev_respuesta" runat="server">Profesional revisión de respuesta</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddl_prof_rev_respuesta" CssClass="form-control form-control-xs" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblresumen_respuesta_sdht" runat="server">Resumen respuesta de la SDHT</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_resumen_respuesta_sdht" CssClass="form-control form-control-xs" MaxLength="250"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_resumen_respuesta_sdht" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_resumen_respuesta_sdht" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblradicado_respuesta_salida" runat="server">No. radicado salida</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_radicado_respuesta_salida" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_radicado_respuesta_salida" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_radicado_respuesta_salida" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_respuesta" runat="server">Fecha de respuesta</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_respuesta" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_respuesta" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server" />
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_respuesta" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_respuesta">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server" />
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_respuesta" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_respuesta" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_respuesta" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblnombre_receptor_respuesta" runat="server">Nombre del receptor de respuesta</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_nombre_receptor_respuesta" CssClass="form-control form-control-xs" MaxLength="250"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_nombre_receptor_respuesta" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_nombre_receptor_respuesta" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_entrega_respuesta" runat="server">Fecha de entrega de respuesta</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_entrega_respuesta" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_entrega_respuesta" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server" />
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_entrega_respuesta" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_entrega_respuesta">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server" />
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_entrega_respuesta" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_entrega_respuesta" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_entrega_respuesta" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblmotivo_devolucion_respuesta" runat="server">Motivo de devolución de respuesta</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_motivo_devolucion_respuesta" CssClass="form-control form-control-xs" MaxLength="100"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_motivo_devolucion_respuesta" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_motivo_devolucion_respuesta" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_devolucion_respuesta" runat="server">Fecha devolución</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_devolucion_respuesta" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_devolucion_respuesta" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server" />
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_devolucion_respuesta" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_devolucion_respuesta">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server" />
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_devolucion_respuesta" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_devolucion_respuesta" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_devolucion_respuesta" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>


                                    </div>
                                </div>
                                <div id="tab-2" class="hidden">
                                    <div class="row">
                                        <div class="uc-center form-group-sm col-12">
                                            <label class="section-title fwb">Primera notificación </label>
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_primera_notif" runat="server">Fecha de notificación</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_primera_notif" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_primera_notif" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_primera_notif" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_primera_notif">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_primera_notif" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_primera_notif" PopupButtonID="txt_fecha_primera_notif" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_primera_notif" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lbltipo_notif_primera" runat="server">Tipo de notificación</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_tipo_notif_primera" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_tipo_notif_primera" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_tipo_notif_primera" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblnum_propietarios_notif1" runat="server">No. de propietarios</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_num_propietarios_notif1" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_num_propietarios_notif1" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_num_propietarios_notif1" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-6">
                                            <asp:Label ID="lblnombre_prop_notif1" runat="server">Nombre del propietario notificado</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_nombre_prop_notif1" CssClass="form-control form-control-xs" MaxLength="120"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_nombre_prop_notif1" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_nombre_prop_notif1" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lbldir_corresp_fisica1" runat="server">Dirección física</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_dir_corresp_fisica1" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_dir_corresp_fisica1" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_dir_corresp_fisica1" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lbldir_corresp_electronica1" runat="server">Dirección electrónica</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_dir_corresp_electronica1" CssClass="form-control form-control-xs" MaxLength="150"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_dir_corresp_electronica1" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ@._," TargetControlID="txt_dir_corresp_electronica1" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_recurso1" runat="server">Fecha recurso de reposición</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_recurso1" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_recurso1" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_recurso1" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_recurso1">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_recurso1" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_recurso1" PopupButtonID="txt_fecha_recurso1" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_recurso1" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblradicado_recurso1" runat="server">Radicado del recurso</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_radicado_recurso1" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_radicado_recurso1" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_radicado_recurso1" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_resol_recurso1" runat="server">Fecha resolución recurso</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_resol_recurso1" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_resol_recurso1" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_resol_recurso1" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_resol_recurso1">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_resol_recurso1" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_resol_recurso1" PopupButtonID="txt_fecha_resol_recurso1" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_resol_recurso1" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblnum_acto_admin1" runat="server">No. de acto administrativo</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_num_acto_admin1" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_num_acto_admin1" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_num_acto_admin1" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_notif_resol1" runat="server">Fecha notificación resolución</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_notif_resol1" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_notif_resol1" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_notif_resol1" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_notif_resol1">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_notif_resol1" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_notif_resol1" PopupButtonID="txt_fecha_notif_resol1" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_notif_resol1" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lbltipo_notif_resol1" runat="server">Tipo de notificación resolución</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_tipo_notif_resol1" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_tipo_notif_resol1" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_tipo_notif_resol1" FilterMode="ValidChars" />
                                        </div>
                                    </div>



                                    <div class="row">
                                        <div class="uc-center form-group-sm col-12">
                                            <label class="section-title fwb">Segunda notificación </label>
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_segunda_notif" runat="server">Fecha de notificación</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_segunda_notif" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_segunda_notif" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_segunda_notif" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_segunda_notif">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_segunda_notif" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_segunda_notif" PopupButtonID="txt_fecha_segunda_notif" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_segunda_notif" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lbltipo_notif_segunda" runat="server">Tipo de notificación</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_tipo_notif_segunda" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_tipo_notif_segunda" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_tipo_notif_segunda" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblnum_propietarios_notif2" runat="server">No. de propietarios</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_num_propietarios_notif2" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_num_propietarios_notif2" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_num_propietarios_notif2" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-6">
                                            <asp:Label ID="lblnombre_prop_notif2" runat="server">Nombre del propietario notificado</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_nombre_prop_notif2" CssClass="form-control form-control-xs" MaxLength="120"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_nombre_prop_notif2" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_nombre_prop_notif2" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lbldir_corresp_fisica2" runat="server">Dirección física</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_dir_corresp_fisica2" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_dir_corresp_fisica2" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_dir_corresp_fisica2" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lbldir_corresp_electronica2" runat="server">Dirección electrónica</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_dir_corresp_electronica2" CssClass="form-control form-control-xs" MaxLength="150"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_dir_corresp_electronica2" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ@._," TargetControlID="txt_dir_corresp_electronica2" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_recurso2" runat="server">Fecha recurso de reposición</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_recurso2" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_recurso2" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_recurso2" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_recurso2">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_recurso2" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_recurso2" PopupButtonID="txt_fecha_recurso2" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_recurso2" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblradicado_recurso2" runat="server">Radicado del recurso</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_radicado_recurso2" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_radicado_recurso2" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_radicado_recurso2" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_resol_recurso2" runat="server">Fecha resolución recurso</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_resol_recurso2" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_resol_recurso2" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_resol_recurso2" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_resol_recurso2">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_resol_recurso2" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_resol_recurso2" PopupButtonID="txt_fecha_resol_recurso2" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_resol_recurso2" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblnum_acto_admin2" runat="server">No. de acto administrativo</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_num_acto_admin2" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_num_acto_admin2" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_num_acto_admin2" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_notif_resol2" runat="server">Fecha notificación resolución</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_notif_resol2" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_notif_resol2" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_notif_resol2" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_notif_resol2">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_notif_resol2" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_notif_resol2" PopupButtonID="txt_fecha_notif_resol2" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_notif_resol2" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lbltipo_notif_resol2" runat="server">Tipo de notificación resolución</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_tipo_notif_resol2" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_tipo_notif_resol2" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_tipo_notif_resol2" FilterMode="ValidChars" />
                                        </div>
                                    </div>



                                    <div class="row">
                                        <div class="uc-center form-group-sm col-12">
                                            <label class="section-title fwb">
                                                Tercera notificación
                                            </label>
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_resol_recurso3" runat="server">Fecha resolución recurso</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_resol_recurso3" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_resol_recurso3" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_resol_recurso3" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_resol_recurso3">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_resol_recurso3" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_resol_recurso3" PopupButtonID="txt_fecha_resol_recurso3" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_resol_recurso3" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblnum_acto_admin3" runat="server">No. de acto administrativo</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_num_acto_admin3" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_num_acto_admin3" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_num_acto_admin3" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblfecha_notif_resol3" runat="server">Fecha notificación resolución</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_notif_resol3" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_notif_resol3" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_notif_resol3" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_notif_resol3">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_notif_resol3" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_notif_resol3" PopupButtonID="txt_fecha_notif_resol3" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_notif_resol3" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lbltipo_notif_resol3" runat="server">Tipo de notificación resolución</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_tipo_notif_resol3" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_tipo_notif_resol3" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_tipo_notif_resol3" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-2">
                                            <asp:Label ID="lblnum_propietarios_notif3" runat="server">No. de propietarios</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_num_propietarios_notif3" CssClass="form-control form-control-xs" MaxLength="20"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_num_propietarios_notif3" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_num_propietarios_notif3" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lblnombre_prop_notif3" runat="server">Nombre del propietario notificado</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_nombre_prop_notif3" CssClass="form-control form-control-xs" MaxLength="120"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_nombre_prop_notif3" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ" TargetControlID="txt_nombre_prop_notif3" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lbldir_corresp_fisica3" runat="server">Dirección física</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_dir_corresp_fisica3" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_dir_corresp_fisica3" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ.,; " TargetControlID="txt_dir_corresp_fisica3" FilterMode="ValidChars" />
                                        </div>
                                        <div class="form-group-sm col-4">
                                            <asp:Label ID="lbldir_corresp_electronica3" runat="server">Dirección electrónica</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_dir_corresp_electronica3" CssClass="form-control form-control-xs" MaxLength="150"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftb_dir_corresp_electronica3" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom" ValidChars="áéíóúÁÉÍÓÚñÑ@._," TargetControlID="txt_dir_corresp_electronica3" FilterMode="ValidChars" />
                                        </div>
                                    </div>
                                </div>
                                <div id="tab-3" class="hidden">
                                    <div class="row">
                                        <div class="uc-center form-group-sm col-12">
                                            <label class="section-title fwb"></label>
                                        </div>
                                        <div class="form-group-sm col-3">
                                            <asp:Label ID="lblfecha_firmeza_ejec1" runat="server">Fecha de firmeza (ejecutoria 1)</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_firmeza_ejec1" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_firmeza_ejec1" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_firmeza_ejec1" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_firmeza_ejec1">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_firmeza_ejec1" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_firmeza_ejec1" PopupButtonID="txt_fecha_firmeza_ejec1" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_firmeza_ejec1" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>

                                        <div class="form-group-sm col-3">
                                            <asp:Label ID="lblfecha_expedicion_ejec1" runat="server">Fecha de expedición (ejecutoria 1)</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_expedicion_ejec1" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_expedicion_ejec1" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_expedicion_ejec1" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_expedicion_ejec1">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_expedicion_ejec1" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_expedicion_ejec1" PopupButtonID="txt_fecha_expedicion_ejec1" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_expedicion_ejec1" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                    </div>    
                                    <div class="row">
                                        <div class="form-group-sm col-3">
                                            <asp:Label ID="lblfecha_firmeza_ejec2" runat="server">Fecha de firmeza (ejecutoria 2)</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_firmeza_ejec2" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_firmeza_ejec2" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_firmeza_ejec2" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_firmeza_ejec2">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_firmeza_ejec2" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_firmeza_ejec2" PopupButtonID="txt_fecha_firmeza_ejec2" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_firmeza_ejec2" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="form-group-sm col-3">
                                            <asp:Label ID="lblfecha_expedicion_ejec2" runat="server">Fecha de expedición (ejecutoria 2)</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_expedicion_ejec2" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_expedicion_ejec2" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_expedicion_ejec2" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_expedicion_ejec2">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_expedicion_ejec2" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_expedicion_ejec2" PopupButtonID="txt_fecha_expedicion_ejec2" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_expedicion_ejec2" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>
                                    </div>    
                                    <div class="row">
                                        <div class="form-group-sm col-3">
                                            <asp:Label ID="lblfecha_firmeza_ejec3" runat="server">Fecha de firmeza (ejecutoria 3)</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_firmeza_ejec3" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_firmeza_ejec3" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_firmeza_ejec3" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_firmeza_ejec3">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_firmeza_ejec3" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_firmeza_ejec3" PopupButtonID="txt_fecha_firmeza_ejec3" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_firmeza_ejec3" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>

                                        <div class="form-group-sm col-3">
                                            <asp:Label ID="lblfecha_expedicion_ejec3" runat="server">Fecha de expedición (ejecutoria 3)</asp:Label>
                                            <asp:RegularExpressionValidator ID="rev_fecha_expedicion_ejec3" runat="server" ValidationGroup="vgPrediosCartas" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txt_fecha_expedicion_ejec3" Display="Dynamic">
                                                <uc:ToolTip width="150px" ToolTip="Fecha inválida" runat="server"/>
                                            </asp:RegularExpressionValidator>
                                            <asp:RangeValidator runat="server" ID="rv_fecha_expedicion_ejec3" ValidationGroup="vgPrediosCartas" ControlToValidate="txt_fecha_expedicion_ejec3">
                                                <uc:ToolTip width="160px" ToolTip="Fecha inválida (válido del año 2008 a hoy)" runat="server"/>
                                            </asp:RangeValidator>
                                            <ajax:CalendarExtender ID="cal_fecha_expedicion_ejec3" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_expedicion_ejec3" PopupButtonID="txt_fecha_expedicion_ejec3" Format="yyyy-MM-dd" />
                                            <asp:TextBox runat="server" ID="txt_fecha_expedicion_ejec3" CssClass="form-control form-control-xs" TextMode="Date"></asp:TextBox>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="gv-w">
                        <asp:GridView ID="gvCartas" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="id_carta_terminos" EmptyDataText="No hay registros asociados"
                            AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" AllowSorting="true" OnRowCommand="gvCartas_RowCommand" OnRowDataBound="gvCartas_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="envio_carta" HeaderText="Envío carta" />
                                <asp:BoundField DataField="radicado_salida" HeaderText="Radicado salida" />
                                <asp:BoundField DataField="fecha_radicado_salida" HeaderText="Fecha radicado salida" />
                                <asp:BoundField DataField="profesional_elab_carta" HeaderText="Profesional elabora carta" />
                                <asp:BoundField DataField="profesional_revision" HeaderText="Profesional revisa" />
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
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnAccept" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnCartaList" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnCartaAdd" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnCartaEdit" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnCartaDel" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="gvCartas" />
    </Triggers>
</asp:UpdatePanel>

