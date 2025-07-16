<%@ Page Title="" Language="C#" MasterPageFile="~/Authentic.Master" AutoEventWireup="true" CodeBehind="Predios.aspx.cs" Inherits="SIDec.Predios" MaintainScrollPositionOnPostback="true" ViewStateMode="Enabled" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Register Src="~/UserControls/FileUpload.ascx" TagName="FileUpload" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/PrediosDeclarados/Interesado.ascx" TagName="Interesado" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/PrediosDeclarados/Carta.ascx" TagName="Carta" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<script type="text/javascript" src="./UserControls/PrediosDeclarados/Predios.js"></script>
    <div class="divContent">
        <div class="divBuscar">

            <asp:UpdatePanel ID="pnlBuscar" runat="server" DefaultButton="btnBuscar">
                <ContentTemplate>
                    <div class="input-group input-group-sm w350">
                        <asp:TextBox runat="server" ID="txtBuscador" CssClass="form-control" placeholder="Búsqueda por chip, lote o matrícula" MaxLength="20" />
                        <div class="input-group-append">
                            <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnBuscar_Click" />
                        </div>
                    </div>
                    <div class="">
                        <asp:LinkButton ID="btnVolver" runat="server" Visible="false" CssClass="btn" style="right:8%; position:absolute" Text="Volver al proyecto" CausesValidation="false" ToolTip="Volver a proyecto" OnClick="btnVolver_Click">
							<i class="fas fa-arrow-alt-circle-left"></i>
                        </asp:LinkButton>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdateProgress ID="pnlLoading" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="pnlBuscar">
                <ProgressTemplate>
                    <asp:Image runat="server" CssClass="imgCargando" ImageUrl="~/images/icon/cargando.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>

            <%--Valores para scroll de los GV--%>
            <asp:HiddenField runat="server" ID="hfEvtGVPredios" Value="" />
            <asp:HiddenField runat="server" ID="hfEvtGVPrediosDec" Value="" />
            <asp:HiddenField ID="hfGVPrediosSV" runat="server" />
            <asp:HiddenField ID="hfGVPrediosSH" runat="server" />
            <asp:HiddenField ID="hfGVPrediosDecSV" runat="server" />
            <asp:HiddenField ID="hfGVPrediosDecSH" runat="server" />
            <asp:HiddenField ID="hfGVDocumentosSV" runat="server" />
            <asp:HiddenField ID="hfGVDocumentosSH" runat="server" />
            <asp:HiddenField ID="hfGVPrestamosSV" runat="server" />
            <asp:HiddenField ID="hfGVPrestamosSH" runat="server" />
            <asp:HiddenField ID="hfGVPropietariosDecSV" runat="server" />
            <asp:HiddenField ID="hfGVPropietariosDecSH" runat="server" />
            <asp:HiddenField ID="hfGVPrediosPropietariosDecSV" runat="server" />
            <asp:HiddenField ID="hfGVPrediosPropietariosDecSH" runat="server" />
            <asp:HiddenField ID="hfGVPrediosPropietariosSV" runat="server" />
            <asp:HiddenField ID="hfGVPrediosPropietariosSH" runat="server" />
            <asp:HiddenField ID="hfGVPropietariosSV" runat="server" />
            <asp:HiddenField ID="hfGVPropietariosSH" runat="server" />
            <asp:HiddenField ID="hfGVObservacionesSV" runat="server" />
            <asp:HiddenField ID="hfGVObservacionesSH" runat="server" />
            <asp:HiddenField ID="hfGVVisitasSV" runat="server" />
            <asp:HiddenField ID="hfGVVisitasSH" runat="server" />
            <asp:HiddenField ID="hfGVLicenciasSV" runat="server" />
            <asp:HiddenField ID="hfGVLicenciasSH" runat="server" />
            <asp:HiddenField ID="hfGVConceptosSV" runat="server" />
            <asp:HiddenField ID="hfGVConceptosSH" runat="server" />
            <asp:HiddenField ID="hfGVActosAdmSV" runat="server" />
            <asp:HiddenField ID="hfGVActosAdmSH" runat="server" />
            <asp:HiddenField ID="hddProyectoPrediosChip" runat="server" />
        </div>

        <div id="divData">

            <%--********************************************************************************************************************************************************************************--%>
            <%--******************************************************************************       PREDIOS      ******************************************************************************--%>
            <%--********************************************************************************************************************************************************************************--%>
            <div runat="server" class="divSP" onclick="HideMensajeCRUD();">
                <div class="divSPHeader">
                    <asp:UpdatePanel runat="server" ID="upPrediosBtnVistas" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="divSPHeaderTitle">Predios</div>
                            <div class="divSPHeaderButton vh">
                                <asp:LinkButton ID="btnPrediosVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnPrediosVista_Click">
								<i class="fas fa-th"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnPrediosVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnPrediosVista_Click">
								<i class="fas fa-bars"></i>
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="">
                    <asp:UpdatePanel runat="server" ID="upPredios" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="divSPGV">
                                <asp:MultiView runat="server" ID="mvPredios" ActiveViewIndex="0" OnActiveViewChanged="mvPredios_ActiveViewChanged">
                                    <asp:View runat="server" ID="vPrediosGrid">
                                        <asp:GridView ID="gvPredios" CssClass="gv" runat="server" DataKeyNames="chip" AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" AllowPaging="true"
                                            PageSize="100" AllowSorting="true" OnSelectedIndexChanged="gvPredios_SelectedIndexChanged" OnSorting="gvPredios_Sorting" OnRowCreated="gvPredios_RowCreated"
                                            OnPageIndexChanging="gvPredios_PageIndexChanging" OnDataBinding="gvPredios_DataBinding" OnRowDataBound="gvPredios_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="chip_view" HeaderText="CHIP" SortExpression="chip_view" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="matricula" HeaderText="Matrícula" SortExpression="matricula" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cod_lote" HeaderText="Código lote" SortExpression="cod_lote" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="direccion" HeaderText="Dirección" />
                                                <asp:BoundField DataField="nombre_barrio" HeaderText="Barrio" />
                                                <asp:BoundField DataField="nombre_UPZ" HeaderText="UPZ" />
                                                <asp:BoundField DataField="nombre_localidad" HeaderText="Localidad" />
                                                <asp:BoundField DataField="nombre_upl" HeaderText="UPL" />
                                                <asp:BoundField DataField="area_terreno_UAECD" HeaderText="Area UAECD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="area_terreno_folio" HeaderText="Area folio" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="area_construccion" HeaderText="Area construcción" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                            </Columns>
                                            <SelectedRowStyle CssClass="gvItemSelected" />
                                            <HeaderStyle CssClass="gvHeader" />
                                            <RowStyle CssClass="gvItem" />
                                            <PagerStyle CssClass="gvPager" />
                                        </asp:GridView>
                                    </asp:View>

                                    <asp:View runat="server" ID="vPrediosDetalle">
                                        <div class="divEdit">
                                            <label class="lbl1 lblDis">CHIP</label>
                                            <asp:TextBox runat="server" ID="txt_chip" CssClass="txt2 txtDis t-c"></asp:TextBox>
                                            <label class="lbl1 lblDis">Matrícula</label>
                                            <asp:TextBox runat="server" ID="txt_matricula" CssClass="txt2 txtDis t-c"></asp:TextBox>
                                            <label class="lbl1 lblDis">Código Lote</label>
                                            <asp:TextBox runat="server" ID="txt_cod_lote" CssClass="txt2 txtDis t-c"></asp:TextBox>
                                            <label class="lbl1 lblDis">Dirección</label>
                                            <asp:TextBox runat="server" ID="txt_direccion" CssClass="txt2 txtDis w280"></asp:TextBox>
                                            <br />

                                            <asp:Label runat="server" ID="lbl_area_terreno_UAECD" class="lbl1">Area terreno UAECD</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_area_terreno_UAECD" CssClass="txtN2" OnTextChanged="Predios_TextChanged"></asp:TextBox>
                                            <asp:Label runat="server" ID="lbl_area_terreno_folio" class="lbl1">Area terreno folio</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_area_terreno_folio" CssClass="txtN2" OnTextChanged="Predios_TextChanged"></asp:TextBox>
                                            <asp:Label runat="server" ID="lbl_area_construccion" class="lbl1">Area construcción</asp:Label>
                                            <asp:TextBox runat="server" ID="txt_area_construccion" CssClass="txtN2" OnTextChanged="Predios_TextChanged"></asp:TextBox>
                                        </div>
                                    </asp:View>
                                </asp:MultiView>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvPredios" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPrediosVG" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPrediosVD" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnFirstPredios" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnBackPredios" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnNextPredios" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnLastPredios" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPrediosEdit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <asp:UpdatePanel runat="server" ID="upPrediosFoot" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="divSPMessage" id="DivMsgPredios"></div>
                        <div class="divSPFooter">
                            <asp:Panel class="divSPFooter1" ID="divPrediosNavegacion" runat="server">
                                <asp:LinkButton ID="btnFirstPredios" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnPrediosNavegacion_Click">
								<i class="fas fa-angle-double-left"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnBackPredios" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnPrediosNavegacion_Click">
								<i class="fas fa-angle-left"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnNextPredios" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnPrediosNavegacion_Click">
								<i class="fas fa-angle-right"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnLastPredios" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnPrediosNavegacion_Click">
								<i class="fas fa-angle-double-right"></i>
                                </asp:LinkButton>
                            </asp:Panel>

                            <div class="divSPFooter2">
                                <asp:Label runat="server" ID="lblPrediosCuenta"></asp:Label>
                            </div>

                            <div class="divSPFooter3">
                                <asp:LinkButton ID="btnPrediosCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnPrediosCancelar_Click" CausesValidation="false">
								<i class="fas fa-times"></i>&nbspCancelar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnPrediosAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" OnClientClick="return ShowModalPopup('puPredios');">
								<i class="fas fa-check"></i>&nbspAceptar
                                </asp:LinkButton>
                            </div>

                            <div class="divSPFooter4" id="divPrediosAction">
                                <asp:LinkButton ID="btnPrediosEdit" runat="server" CssClass="btn4" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnPrediosAccion_Click">
								<i class="fas fa-pencil-alt"></i>
                                </asp:LinkButton>
                            </div>
                        </div>

                        <!--MODAL POPUP-->
                        <asp:LinkButton ID="lbDummyPredios" runat="server"></asp:LinkButton>
                        <ajaxToolkit:ModalPopupExtender ID="mpePredios" runat="server" PopupControlID="pnlPopupPredios" TargetControlID="lbDummyPredios" CancelControlID="btnHidePredios" BackgroundCssClass="modalBackground" BehaviorID="puPredios"></ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pnlPopupPredios" runat="server" CssClass="modalPopup" Style="display: none">
                            <div class="puHeader">Confirmar acción</div>
                            <div class="puBody">
                                <p>¿Está seguro de continuar con la acción solicitada?</p>
                                <asp:LinkButton ID="btnHidePredios" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClientClick="return HideModalPopup('puPredios');">
                                <i class="fas fa-times"></i>&nbsp&nbspCancelar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnConfirmarPredios" runat="server" Text="" CssClass="btn btn-outline-primary btn-sm ml10" OnClick="btnConfirmar_Click">
                                <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                </asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnPrediosEdit" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <%--********************************************************************************************************************************************************************************--%>
            <%--******************************************************************************     PREDIOS DEC    ******************************************************************************--%>
            <%--********************************************************************************************************************************************************************************--%>
            <div id="divPrediosDec" runat="server" class="divSP" onclick="HideMensajeCRUD();">
                <div class="divSPHeader">
                    <asp:UpdatePanel runat="server" ID="upPrediosDecBtnVistas" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="divSPHeaderTitle">Predios Declarados</div>
                            <div class="divSPHeaderButton vh">
                                <asp:LinkButton ID="btnPrediosDecVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnPrediosDecVista_Click">
								<i class="fas fa-th"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnPrediosDecVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnPrediosDecVista_Click">
								<i class="fas fa-bars"></i>
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="">
                    <asp:UpdatePanel runat="server" ID="upPrediosDec" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="divSPGV">
                                <asp:MultiView runat="server" ID="mvPrediosDec" ActiveViewIndex="0" OnActiveViewChanged="mvPrediosDec_ActiveViewChanged">
                                    <asp:View runat="server" ID="vPrediosDecGrid">
                                        <asp:GridView ID="gvPrediosDec" CssClass="gv w-100" runat="server" DataKeyNames="cod_predio_declarado, cod_usu_responsable, idarchivo" AutoGenerateColumns="False"
                                            ShowHeaderWhenEmpty="false" OnSelectedIndexChanged="gvPrediosDec_SelectedIndexChanged" OnRowDataBound="gvPrediosDec_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="cod_predio_declarado" HeaderText="Cod Predio" ItemStyle-CssClass="t-c" />
                                                <asp:BoundField DataField="chip" HeaderText="CHIP" ItemStyle-CssClass="t-c" />
                                                <asp:BoundField DataField="resolucion_declaratoria" HeaderText="Res. declaratoria" ItemStyle-CssClass="t-c" />
                                                <asp:BoundField DataField="tipo_declaratoria" HeaderText="Tipo declaratoria" HeaderStyle-CssClass="d-n" ItemStyle-CssClass="d-n" />
                                                <asp:BoundField DataField="numero_caja" HeaderText="Número caja" ItemStyle-CssClass="t-c" />
                                                <asp:BoundField DataField="numero_carpetas" HeaderText="Número carpetas" ItemStyle-CssClass="t-c" />
                                                <asp:BoundField DataField="posicion_carpeta" HeaderText="Posición carpeta inicial" ItemStyle-CssClass="t-c" />
                                                <asp:TemplateField HeaderText="Recibe carta de términos" ItemStyle-CssClass="t-c">
                                                    <ItemTemplate><%# Convert.ToString(Eval("recibe_carta_terminos")) == "1" ? "Si" : "" %></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="fecha_resolucion_declaratoria" HeaderText="Fecha declaratoria" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="t-c d-n" HeaderStyle-CssClass="d-n" />
                                                <asp:BoundField DataField="fecha_cumplimiento" HeaderText="Fecha cumplimiento" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="t-c" />
                                                <asp:BoundField DataField="tiempo_cumplimiento" HeaderText="Tiempo cumplimiento" ItemStyle-CssClass="t-c" />
                                                <asp:BoundField DataField="estado_predio_declarado" HeaderText="Estado predio" ItemStyle-CssClass="" />
                                                <asp:BoundField DataField="usu_responsable" HeaderText="Usuario responsable" ItemStyle-CssClass="" />
                                                <asp:BoundField DataField="obs_predio_declarado" HeaderText="Observaciones" ItemStyle-CssClass="w200" />
                                                <asp:BoundField DataField="anos_tiempo_cumplimiento" HeaderText="Tiempo cumplimiento" Visible="false" />
                                            </Columns>
                                            <SelectedRowStyle CssClass="gvItemSelected" />
                                            <HeaderStyle CssClass="gvHeader" />
                                            <RowStyle CssClass="gvItem" />
                                            <PagerStyle CssClass="gvPager" />
                                        </asp:GridView>
                                    </asp:View>

                                    <asp:View runat="server" ID="vPrediosDecDetalle">
                                        <div class="divEdit">
                                            <label class="lbl2 lblDis wa">CHIP</label>
                                            <asp:TextBox runat="server" ID="txt_chip_PrediosDec" CssClass="txt2 txtDis t-c"></asp:TextBox>
                                            <label class="lbl2 lblDis wa">Res. Declaratoria</label>
                                            <asp:TextBox runat="server" ID="txt_resolucion_declaratoria" CssClass="txt2 txtDis t-c"></asp:TextBox>
                                            <label class="lbl1">Número cajas</label>
                                            <asp:TextBox runat="server" ID="txt_numero_caja" CssClass="txtN" TextMode="Number" TabIndex="11" OnTextChanged="PrediosDec_TextChanged"></asp:TextBox>
                                            <label class="lbl1">Número carpetas</label>
                                            <asp:TextBox runat="server" ID="txt_numero_carpetas" CssClass="txtN" TextMode="Number" TabIndex="13" OnTextChanged="PrediosDec_TextChanged"></asp:TextBox>
                                            <label class="lbl1">Posición carpeta</label>
                                            <asp:TextBox runat="server" ID="txt_posicion_carpeta" CssClass="txtN" TextMode="Number" TabIndex="15" OnTextChanged="PrediosDec_TextChanged"></asp:TextBox>
                                            <br />

                                            <asp:CheckBox runat="server" ID="chk_recibe_carta_terminos" CssClass="" TabIndex="17" OnCheckedChanged="PrediosDec_TextChanged" Text="Recibe carta de términos" />
                                            <asp:Label runat="server" ID="lbl_cod_usu_responsable" CssClass="lbl1">Usuario responsable</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlb_cod_usu_responsable" CssClass="txt2 w250" AppendDataBoundItems="true" TabIndex="19" OnTextChanged="PrediosDec_TextChanged">
                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                            </asp:DropDownList>                                           
                                            <br />
                                            <label class="lbl1">Observaciones</label>
                                            <asp:TextBox runat="server" ID="txt_obs_predio_declarado" CssClass="txt2 txtObs1" TabIndex="21"
                                                TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);" OnTextChanged="PrediosDec_TextChanged"></asp:TextBox>
                                        </div>
                                    </asp:View>
                                </asp:MultiView>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvPrediosDec" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnPrediosDecVG" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPrediosDecVD" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnFirstPrediosDec" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnBackPrediosDec" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnNextPrediosDec" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnLastPrediosDec" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnPrediosDecEdit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <asp:UpdatePanel runat="server" ID="upPrediosDecFoot" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="divSPMessage" id="DivMsgPrediosDec"></div>
                        <div class="divSPFooter">
                            <asp:Panel class="divSPFooter1" ID="divPrediosDecNavegacion" runat="server">
                                <%--FOOTER--%>
                                <asp:LinkButton ID="btnFirstPrediosDec" runat="server" CssClass="btn5" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnPrediosDecNavegacion_Click">
								<i class="fas fa-angle-double-left"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnBackPrediosDec" runat="server" CssClass="btn5" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnPrediosDecNavegacion_Click">
								<i class="fas fa-angle-left"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnNextPrediosDec" runat="server" CssClass="btn5" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnPrediosDecNavegacion_Click">
								<i class="fas fa-angle-right"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnLastPrediosDec" runat="server" CssClass="btn5" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnPrediosDecNavegacion_Click">
								<i class="fas fa-angle-double-right"></i>
                                </asp:LinkButton>
                            </asp:Panel>

                            <div class="divSPFooter2">
                                <asp:Label runat="server" ID="lblPrediosDecCuenta"></asp:Label>
                            </div>

                            <div class="divSPFooter3">
                                <asp:LinkButton ID="btnPrediosDecCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnPrediosDecCancelar_Click" CausesValidation="false">
								<i class="fas fa-times"></i>&nbspCancelar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnPrediosDecAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" OnClientClick="return ShowModalPopup('puPrediosDec');">
								<i class="fas fa-check"></i>&nbspAceptar
                                </asp:LinkButton>
                            </div>

                            <div class="divSPFooter4" id="divPrediosDecAction">
                                <asp:LinkButton ID="btnPrediosDecEdit" runat="server" CssClass="btn4" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnPrediosDecAccion_Click">
								<i class="fas fa-pencil-alt"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnPrediosDecFormato" runat="server" CssClass="btn2" CausesValidation="false" ToolTip="Crear formato FO35" OnClick="btnPrediosDecFormato_Click">
								<i class="far fa-file-excel"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnPrediosDecFormato1" runat="server" CssClass="btn2" CausesValidation="false" ToolTip="Crear carta de términos" Enabled="false"> <%--OnClick="btnPrediosDecFormato1_Click"--%>
								<i class="fas fa-file-word"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnCartaExcel" runat="server" style="color: var(--col-s);" CssClass="btn2" Text="Matriz Carta de Seguimientos" CausesValidation="false" CommandName="Excel" CommandArgument="5" ToolTip="Generar matriz" OnClick="btnCartaExcel_Click">
								        <i class="fas fa-file-excel"></i>
                                </asp:LinkButton>
                            </div>
                        </div>

                        <!--MODAL POPUP-->
                        <asp:LinkButton ID="lbDummyPrediosDec" runat="server"></asp:LinkButton>
                        <ajaxToolkit:ModalPopupExtender ID="mpePrediosDec" runat="server" PopupControlID="pnlPopupPrediosDec" TargetControlID="lbDummyPrediosDec" CancelControlID="btnHidePrediosDec" BackgroundCssClass="modalBackground" BehaviorID="puPrediosDec"></ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pnlPopupPrediosDec" runat="server" CssClass="modalPopup" Style="display: none">
                            <div class="puHeader">Confirmar acción</div>
                            <div class="puBody">
                                <p>¿Está seguro de continuar con la acción solicitada?</p>
                                <asp:LinkButton ID="btnHidePrediosDec" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClientClick="return HideModalPopup('puPrediosDec');">
                                <i class="fas fa-times"></i>&nbsp&nbspCancelar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnConfirmarPrediosDec" runat="server" Text="" CssClass="btn btn-outline-primary btn-sm ml10" OnClick="btnConfirmar_Click">
                                <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                </asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnPrediosDecEdit" EventName="Click" />
                        <asp:PostBackTrigger ControlID="btnPrediosDecFormato" />
                        <asp:PostBackTrigger ControlID="btnPrediosDecFormato1" />
                        <asp:PostBackTrigger ControlID="btnCartaExcel" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <div class="divS h20"></div>

            <%--********************************************************************************************************************************************************************************--%>
            <%--******************************************************************************      PESTAÑAS      ******************************************************************************--%>
            <%--********************************************************************************************************************************************************************************--%>
            <asp:UpdatePanel runat="server" ID="upTab" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <ajaxToolkit:TabContainer ID="TabContainer1" runat="server"  Width="1200px" CssClass="tabCont" ActiveTabIndex="0" OnActiveTabChanged="TabContainer1_ActiveTabChanged" AutoPostBack="true">

                        <%--********************************************************************************************************************************************************************************--%>
                        <%--******************************************************************************     DOCUMENTOS     ******************************************************************************--%>
                        <%--********************************************************************************************************************************************************************************--%>
                        <ajaxToolkit:TabPanel runat="server" ID="tpDocumentos" HeaderText="Documentos">
                            <ContentTemplate>
                                <div class="divSP" onclick="HideMensajeCRUD();">
                                    <div class="divSPHeader">
                                        <asp:UpdatePanel runat="server" ID="upDocumentosBtnVistas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPHeaderTitle"></div>
                                                <div class="divSPHeaderButton">
                                                    <asp:LinkButton ID="btnDocumentosVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnDocumentosVista_Click">
													<i class="fas fa-th"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDocumentosVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnDocumentosVista_Click">
													<i class="fas fa-bars"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="">
                                        <asp:UpdatePanel runat="server" ID="upDocumentos" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPGV">
                                                    <asp:MultiView runat="server" ID="mvDocumentos" ActiveViewIndex="0" OnActiveViewChanged="mvDocumentos_ActiveViewChanged">
                                                        <asp:View runat="server" ID="vDocumentosGrid">
                                                            <asp:GridView ID="gvDocumentos" CssClass="gv" runat="server" AutoGenerateColumns="False" DataKeyNames="au_documento" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="500"
                                                                AllowSorting="true" OnSorting="gvDocumentos_Sorting" OnSelectedIndexChanged="gvDocumentos_SelectedIndexChanged" OnPageIndexChanging="gvDocumentos_PageIndexChanging"
                                                                OnRowCreated="gvDocumentos_RowCreated" OnDataBinding="gvDocumentos_DataBinding" OnRowDataBound="gvDocumentos_RowDataBound" OnRowCommand="gvDocumentos_RowCommand">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_documento" HeaderText="Num. documento" Visible="false" />
                                                                    <asp:BoundField DataField="cod_predio_declarado" HeaderText="Cod. predio" Visible="false" />
                                                                    <asp:BoundField DataField="numero_carpeta" HeaderText="Num. carpeta" ItemStyle-CssClass="t-c w30" />
                                                                    <asp:BoundField DataField="orden_documento" HeaderText="Orden Doc." ItemStyle-CssClass="t-c w30" />
                                                                    <asp:BoundField DataField="tipo_documento" HeaderText="Tipo documento" SortExpression="tipo_documento" ItemStyle-CssClass="" HtmlEncode="true" />
                                                                    <asp:BoundField DataField="radicado_documento" HeaderText="Num. radicado" ItemStyle-CssClass="w80" />
                                                                    <asp:BoundField DataField="fecha_radicado_documento" HeaderText="Fecha radicado" SortExpression="fecha_radicado_documento" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="t-c w80" />
                                                                    <asp:BoundField DataField="folios_documento" HeaderText="Folios" ItemStyle-CssClass="t-c w40" />
                                                                    <asp:BoundField DataField="folio_inicial_documento" HeaderText="Folio inicial" SortExpression="folio_inicial_documento" ItemStyle-CssClass="t-r w40" />
                                                                    <asp:BoundField DataField="folio_final_documento" HeaderText="Folio final" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="w40" />
                                                                    <asp:BoundField DataField="obs_documento" HeaderText="Observaciones" ItemStyle-CssClass="" />
                                                                    <asp:BoundField DataField="usuario" HeaderText="Usuario" ItemStyle-CssClass="" />
                                                                    <asp:TemplateField ShowHeader="true" HeaderText="Doc" ItemStyle-CssClass="m0 w30">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnOpenDoc" runat="server" CommandName="OpenDoc" CommandArgument='<%# Container.DisplayIndex %>' ImageUrl="~/images/icon/pdf-icon.png" Visible='<%# (String.IsNullOrEmpty(Eval("cod_usu_scan").ToString())) ? false : true %>' ToolTip="Abrir documento" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </asp:View>

                                                        <asp:View runat="server" ID="vDocumentosDetalle">
                                                            <div class="divEdit1">
                                                                <asp:TextBox runat="server" ID="txt_au_documento" Visible="false"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_tipo_documento" CssClass="lbl2 w120">Tipo documento</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_tipo_documento" CssClass="txt2 w1000" TabIndex="2" MaxLength="200" OnTextChanged="Documentos_TextChanged"></asp:TextBox>
                                                                <br />

                                                                <asp:Label runat="server" ID="lbl_numero_carpeta" CssClass="lbl2 lblDis w120">Número carpeta</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_numero_carpeta" CssClass="txtN txtDis" TextMode="Number" OnTextChanged="Documentos_TextChanged" TabIndex="3" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_orden_documento" CssClass="lbl2 lblDis w130">Orden documento</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_orden_documento" CssClass="txtN txtDis" TextMode="Number" Enabled="False" TabIndex="4"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_folio_inicial_documento" CssClass="lbl2 w90">Folio inicial</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_folio_inicial_documento" CssClass="txtN" TextMode="Number" OnTextChanged="Documentos_folios_TextChanged" TabIndex="5" onkeypress="return SoloEntero(event);" autocomplete="off"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_folios_documento" CssClass="lbl2">Número folios</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_folios_documento" CssClass="txtN" TextMode="Number" OnTextChanged="Documentos_folios_TextChanged" TabIndex="6" onkeypress="return SoloEntero(event);" AutoCompleteType="Disabled"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_folio_final_documento" CssClass="lbl2 lblDis w80">Folio final</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_folio_final_documento" CssClass="txtN txtDis" TextMode="Number" Enabled="False" TabIndex="7"></asp:TextBox>
                                                                <br />

                                                                <asp:Label runat="server" ID="lbl_radicado_documento" CssClass="lbl2 w120">Número radicado</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_radicado_documento" CssClass="txt2 w205" OnTextChanged="Documentos_TextChanged" TabIndex="8"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_fecha_radicado_documento" CssClass="lbl2 w110">Fecha radicado</asp:Label>
                                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_radicado_documento" runat="server" TargetControlID="txt_fecha_radicado_documento" PopupButtonID="txt_fecha_radicado_documento" Format="yyyy-MM-dd" />
                                                                <asp:TextBox runat="server" ID="txt_fecha_radicado_documento" CssClass="txtF" TextMode="Date" OnTextChanged="Documentos_TextChanged" TabIndex="9"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_cod_usu" CssClass="lbl2 w110">Usuario</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_cod_usu" CssClass="txt2 w250" AppendDataBoundItems="true" OnTextChanged="Documentos_TextChanged" TabIndex="10">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />

                                                                <asp:Label runat="server" ID="lbl_obs_documento" CssClass="lbl2 w120">Observaciones</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_obs_documento" CssClass="txt2 txtObs"
                                                                    TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);" OnTextChanged="Documentos_TextChanged" TabIndex="11"></asp:TextBox>
                                                                <br />

                                                                <asp:LinkButton ID="lbSubirDoc" runat="server" CssClass="btn btn-outline-success btn-xs btnLoad" Text="" CausesValidation="false" OnClick="lbSubirDoc_Click">
																<i class="fas fa-upload"></i>&nbspCargar documento
                                                                </asp:LinkButton>
                                                                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="fl w300 fs11 ml5" AllowMultiple="false" Visible="false" />
                                                                <br />
                                                                <br />

                                                                <asp:ValidationSummary runat="server" ID="vsDocumentos" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgDocumentos" EnableClientScript="false" />
                                                                <asp:CompareValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Folio Inicial : El valor debe ser mayor a 0} " Display="Dynamic" ValidationGroup="vgDocumentos" ControlToValidate="txt_folio_inicial_documento" ValueToCompare="0" Operator="GreaterThan" Type="Integer" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Tipo Documento} " Display="Dynamic" ValidationGroup="vgDocumentos" ControlToValidate="txt_tipo_documento" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Folio Inicial} " Display="Dynamic" ValidationGroup="vgDocumentos" ControlToValidate="txt_folio_inicial_documento" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Número Folios} " Display="Dynamic" ValidationGroup="vgDocumentos" ControlToValidate="txt_folios_documento" />
                                                                <asp:CompareValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Número Folios : El valor debe ser mayor a 0} " Display="Dynamic" ValidationGroup="vgDocumentos" ControlToValidate="txt_folios_documento" ValueToCompare="0" Operator="GreaterThan" Type="Integer" />
                                                            </div>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnDocumentosVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnDocumentosVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnFirstDocumentos" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnBackDocumentos" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnNextDocumentos" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLastDocumentos" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnDocumentosEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnDocumentosAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnDocumentosDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnDocumentosReorder" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnDocumentosMoveUp" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnDocumentosMoveDown" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvDocumentos" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <asp:UpdatePanel runat="server" ID="upDocumentosFoot" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:HiddenField runat="server" ID="hfEvtGVDocumentos" Value="" />
                                            <div class="divSPMessage" id="DivMsgDocumentos"></div>
                                            <div class="divSPFooter">
                                                <asp:Panel class="divSPFooter1" ID="divDocumentosNavegacion" runat="server">
                                                    <%--FOOTER--%>
                                                    <asp:LinkButton ID="btnFirstDocumentos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnDocumentosNavegacion_Click">
													<i class="fas fa-angle-double-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnBackDocumentos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnDocumentosNavegacion_Click">
													<i class="fas fa-angle-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnNextDocumentos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnDocumentosNavegacion_Click">
													<i class="fas fa-angle-right"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLastDocumentos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnDocumentosNavegacion_Click">
													<i class="fas fa-angle-double-right"></i>
                                                    </asp:LinkButton>
                                                </asp:Panel>

                                                <div class="divSPFooter2">
                                                    <asp:Label runat="server" ID="lblDocumentosCuenta"></asp:Label>
                                                </div>

                                                <div class="divSPFooter3">
                                                    <asp:LinkButton ID="btnDocumentosCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnDocumentosCancelar_Click" CausesValidation="false">
                            <i class="fas fa-times"></i>&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDocumentosAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" TabIndex="1000" Text="Aceptar" ValidationGroup="vgDocumentos" OnClientClick="if(Page_ClientValidate('vgDocumentos')) return ShowModalPopup('puDocumentos');">
                            <i class="fas fa-check"></i>&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>

                                                <div class="divSPFooter4" id="divDocumentosAction">
                                                    <asp:LinkButton ID="btnDocumentosAdd" runat="server" CssClass="btn4" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnDocumentosAccion_Click">
													<i class="fas fa-plus"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDocumentosEdit" runat="server" CssClass="btn4" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnDocumentosAccion_Click">
													<i class="fas fa-pencil-alt"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDocumentosDel" runat="server" CssClass="btn4" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnDocumentosAccion_Click">
													<i class="far fa-trash-alt"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDocumentosReorder" runat="server" CssClass="btn4" Text="Reordenar" CausesValidation="false" CommandName="Reordenar" CommandArgument="4" ToolTip="Reordenar" OnClick="btnDocumentosAccion_Click">
													<i class="fas fa-sort"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDocumentosMoveUp" runat="server" CssClass="btn4" Text="Mover" CausesValidation="false" CommandName="MoveUp" CommandArgument="5" ToolTip="Mover atrás" OnClick="btnDocumentosAccion_Click">
													<i class="fas fa-caret-up"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDocumentosMoveDown" runat="server" CssClass="btn4" Text="Mover" CausesValidation="false" CommandName="MoveDown" CommandArgument="6" ToolTip="Mover adelante" OnClick="btnDocumentosAccion_Click">
													<i class="fas fa-caret-down"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDocumentosFormato" runat="server" CssClass="btn2" Text="Crear formato" CausesValidation="false" ToolTip="Crear formato F0379" OnClick="btnDocumentosFormato_Click">
													<i class="far fa-file-excel"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>

                                            <!--MODAL POPUP-->
                                            <asp:LinkButton ID="lbDummyDocumentos" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="mpeDocumentos" runat="server" PopupControlID="pnlPopupDocumentos" TargetControlID="lbDummyDocumentos" CancelControlID="btnHideDocumentos" BackgroundCssClass="modalBackground" BehaviorID="puDocumentos"></ajaxToolkit:ModalPopupExtender>
                                            <asp:Panel ID="pnlPopupDocumentos" runat="server" CssClass="modalPopup" Style="display: none">
                                                <div class="puHeader">Confirmar acción</div>
                                                <div class="puBody">
                                                    <p>¿Está seguro de continuar con la acción solicitada?</p>
                                                    <asp:LinkButton ID="btnHideDocumentos" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClientClick="return HideModalPopup('puDocumentos');">
                                                    <i class="fas fa-times"></i>&nbsp&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConfirmarDocumentos" runat="server" Text="" CssClass="btn btn-outline-primary btn-sm ml10" OnClick="btnConfirmar_Click">
                                                    <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>

                                            <!--MODAL ORDER-->
                                            <asp:UpdatePanel ID="upDocumentosReordenar" runat="server" DefaultButton="btnConfirmarDocumentosReordenar">
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="lbDummyDocumentosReordenar" runat="server"></asp:LinkButton>
                                                    <ajaxToolkit:ModalPopupExtender ID="mpeDocumentosReordenar" runat="server" PopupControlID="pnlPopupDocumentosReordenar" TargetControlID="lbDummyDocumentosReordenar" CancelControlID="btnHideDocumentosReordenar" BackgroundCssClass="modalBackground" BehaviorID="puDocumentosReordenar"></ajaxToolkit:ModalPopupExtender>
                                                    <asp:Panel ID="pnlPopupDocumentosReordenar" runat="server" CssClass="modalPopup w500 h270" Style="display: none">
                                                        <div class="puHeader">Confirmar acción</div>
                                                        <div class="puBody">
                                                            <asp:RadioButtonList ID="rbDocumentosOpcion" runat="server" CssClass="w_98a t-l fwn" RepeatDirection="Vertical">
                                                                <asp:ListItem>&nbsp;Mover: mueve el registro seleccionado al folio inicial nuevo</asp:ListItem>
                                                                <asp:ListItem>&nbsp;Desplazar: desplaza desde el registro seleccionado al folio inicial nuevo</asp:ListItem>
                                                                <asp:ListItem>&nbsp;Ajustar: elimina los espacios en la numeración</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <br />
                                                            <asp:Label runat="server" ID="Label41" CssClass="lbl1 ml-10">Folio inicial nuevo</asp:Label>
                                                            <asp:TextBox runat="server" ID="txt_folio_inicial_nuevo" CssClass="txtN" TextMode="Number" TabIndex="2" onkeypress="return SoloEntero(event);" autocomplete="off"></asp:TextBox>
                                                            <br />
                                                            <br />
                                                            <asp:LinkButton ID="btnHideDocumentosReordenar" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClientClick="return HideModalPopup('puDocumentosReordenar');">
                                                            <i class="fas fa-times"></i>&nbsp&nbspCancelar
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnConfirmarDocumentosReordenar" runat="server" CssClass="btn btn-outline-primary btn-sm ml10" ValidationGroup="vgDocumentosOrder" OnClick="btnConfirmar_Click">
                                                            <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                                            </asp:LinkButton>
                                                            <br />
                                                            <br />
                                                            <asp:ValidationSummary runat="server" ID="vsDocumentosOrder" DisplayMode="SingleParagraph" HeaderText="" ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgDocumentosOrder" EnableClientScript="false" />
                                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Acción: Debe escoger una opción} " Display="Dynamic" ValidationGroup="vgDocumentosOrder" ControlToValidate="rbDocumentosOpcion" />
                                                            <asp:CompareValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Folio inicial nuevo : El valor debe ser mayor a 0} " Display="Dynamic" ValidationGroup="vgDocumentosOrder" ControlToValidate="txt_folio_inicial_nuevo" ValueToCompare="0" Operator="GreaterThan" Type="Integer" />
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                            <asp:UpdateProgress ID="uprDocumentosReordenar" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upDocumentosReordenar">
                                                <ProgressTemplate>
                                                    <asp:Image runat="server" CssClass="imgCargando" ImageUrl="~/images/icon/cargando.gif" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>


                                            <!--MODAL ERR NUMERACION-->
                                            <asp:LinkButton ID="lbDummyFolios" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="mpeFolios" runat="server" PopupControlID="pnlPopupFolios" TargetControlID="lbDummyFolios" CancelControlID="btnHideFolios" BackgroundCssClass="modalBackground" BehaviorID="puFolios"></ajaxToolkit:ModalPopupExtender>
                                            <asp:Panel ID="pnlPopupFolios" runat="server" CssClass="modalPopupErr" Style="display: none">
                                                <div class="puHeaderErr">Error de numeración de folios</div>
                                                <div class="puBody">
                                                    <br />
                                                    <asp:Label runat="server" ID="lblMsgErr"></asp:Label>
                                                    <br />
                                                    <br />
                                                    <asp:LinkButton ID="btnHideFolios" runat="server" Text="" CssClass="btn btn-outline-primary btn-sm ml10" OnClientClick="return HideModalPopup('puFolios');">
                                                    <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnDocumentosEdit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnDocumentosAdd" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnDocumentosDel" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnDocumentosReorder" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnDocumentosMoveUp" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnDocumentosMoveDown" EventName="Click" />
                                            <asp:PostBackTrigger ControlID="btnDocumentosFormato" />
                                            <asp:PostBackTrigger ControlID="btnConfirmarDocumentos" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <%--********************************************************************************************************************************************************************************--%>
                        <%--******************************************************************************      PRESTAMOS     ******************************************************************************--%>
                        <%--********************************************************************************************************************************************************************************--%>
                        <ajaxToolkit:TabPanel runat="server" ID="tpPrestamos" HeaderText="Préstamos">
                            <ContentTemplate>
                                <div class="divSP" onclick="HideMensajeCRUD();">
                                    <div class="divSPHeader">
                                        <asp:UpdatePanel runat="server" ID="upPrestamosBtnVistas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPHeaderTitle"></div>
                                                <div class="divSPHeaderButton">
                                                    <asp:LinkButton ID="btnPrestamosVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnPrestamosVista_Click">
													<i class="fas fa-th"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnPrestamosVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnPrestamosVista_Click">
													<i class="fas fa-bars"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="">
                                        <asp:UpdatePanel runat="server" ID="upPrestamos" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPGV">
                                                    <asp:MultiView runat="server" ID="mvPrestamos" ActiveViewIndex="0" OnActiveViewChanged="mvPrestamos_ActiveViewChanged">
                                                        <asp:View runat="server" ID="vPrestamosGrid">
                                                            <asp:GridView ID="gvPrestamos" CssClass="gv" runat="server" AutoGenerateColumns="False" DataKeyNames="au_prestamo,cod_usu_entrega_prestamo,TotalNulos" ShowHeaderWhenEmpty="true" OnSelectedIndexChanged="gvPrestamos_SelectedIndexChanged"
                                                                AllowPaging="true" PageSize="500" OnPageIndexChanging="gvPrestamos_PageIndexChanging" OnRowCreated="gvPrestamos_RowCreated" OnDataBinding="gvPrestamos_DataBinding" OnRowDataBound="gvPrestamos_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_prestamo" Visible="false" />
                                                                    <asp:BoundField DataField="cod_predio_declarado" HeaderText="Cod Predio" Visible="false" />
                                                                    <asp:BoundField DataField="area_solicita_prestamo" HeaderText="Area solicita" />
                                                                    <asp:BoundField DataField="usu_solicita_prestamo" HeaderText="Usuario solicita" />
                                                                    <asp:BoundField DataField="memorando_interno" HeaderText="Memorando" />
                                                                    <asp:BoundField DataField="fecha_entrega_prestamo" HeaderText="Fecha entrega" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="usu_entrega_prestamo" HeaderText="Usuario entrega" HtmlEncode="true" />
                                                                    <asp:BoundField DataField="folios_prestamo" HeaderText="Folios" ItemStyle-HorizontalAlign="Right" />
                                                                    <asp:BoundField DataField="fecha_devolucion_prestamo" HeaderText="Fecha devolución" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="usu_recibe_prestamo" HeaderText="Usuario recibe" />
                                                                    <asp:BoundField DataField="obs_prestamo" HeaderText="Observación" />
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </asp:View>

                                                        <asp:View runat="server" ID="vPrestamosDetalle">
                                                            <div class="divEdit1">
                                                                <asp:TextBox runat="server" ID="txt_au_prestamo" Visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" ID="txt_cod_predio_declarado_prestamo" Visible="false"></asp:TextBox>

                                                                <asp:Label runat="server" ID="lbl_id_area_solicita_prestamo" CssClass="lbl2 w110">Area solicita</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_area_solicita_prestamo" CssClass="txt2 w250 " AppendDataBoundItems="true" OnTextChanged="Prestamos_TextChanged" TabIndex="20" AutoPostBack="true" OnSelectedIndexChanged="ddlb_id_area_solicita_prestamo_SelectedIndexChanged">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="lbl_cod_usu_solicita_prestamo" CssClass="lbl2 w120">Usuario solicita</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_cod_usu_solicita_prestamo" CssClass="txt2 w300 " AppendDataBoundItems="true" OnTextChanged="Prestamos_TextChanged" TabIndex="30">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="lbl_memorando_interno" CssClass="lbl2 w140">Memorando interno</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_memorando_interno" CssClass="txt2 w160 " OnTextChanged="Prestamos_TextChanged" TabIndex="35"></asp:TextBox>
                                                                <br />

                                                                <asp:Label runat="server" ID="lbl_fecha_entrega_prestamo" CssClass="lbl2 w130">Fecha entrega</asp:Label>
                                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_entrega_prestamo" runat="server" TargetControlID="txt_fecha_entrega_prestamo" PopupButtonID="txt_fecha_entrega_prestamo" Format="yyyy-MM-dd" />
                                                                <asp:TextBox runat="server" ID="txt_fecha_entrega_prestamo" CssClass="txtF" TextMode="Date" OnTextChanged="Prestamos_TextChanged" TabIndex="40"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_cod_usu_entrega_prestamo" CssClass="lbl2 w120">Usuario entrega</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_cod_usu_entrega_prestamo" CssClass="txt2 w300 " AppendDataBoundItems="true" OnTextChanged="Prestamos_TextChanged" TabIndex="50">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="lbl_folios_prestamo" CssClass="lbl2 w60">Folios</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_folios_prestamo" CssClass="txtN" TextMode="Number" OnTextChanged="Prestamos_TextChanged" TabIndex="60" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                <br />

                                                                <asp:Label runat="server" ID="lbl_fecha_devolucion_prestamo" CssClass="lbl2 w130">Fecha devolución</asp:Label>
                                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_devolucion_prestamo" runat="server" TargetControlID="txt_fecha_devolucion_prestamo" PopupButtonID="txt_fecha_devolucion_prestamo" Format="yyyy-MM-dd" />
                                                                <asp:TextBox runat="server" ID="txt_fecha_devolucion_prestamo" CssClass="txtF" TextMode="Date" OnTextChanged="Prestamos_TextChanged" TabIndex="70"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_cod_usu_recibe_prestamo" CssClass="lbl2 w120">Usuario recibe</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_cod_usu_recibe_prestamo" CssClass="txt2 w300 " AppendDataBoundItems="true" OnTextChanged="Prestamos_TextChanged" TabIndex="80">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />

                                                                <asp:Label runat="server" ID="lbl_obs_prestamo" CssClass="lbl2 w130">Observación</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_obs_prestamo" CssClass="txt2 txtObs" OnTextChanged="Prestamos_TextChanged" TabIndex="90"
                                                                    TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);"></asp:TextBox>

                                                                <asp:ValidationSummary runat="server" ID="vsPrestamos" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgPrestamos" EnableClientScript="false" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Area solicita} " Display="Dynamic" ValidationGroup="vgPrestamos" ControlToValidate="ddlb_id_area_solicita_prestamo" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Usuario solicita} " Display="Dynamic" ValidationGroup="vgPrestamos" ControlToValidate="ddlb_cod_usu_solicita_prestamo" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Fecha entrega} " Display="Dynamic" ValidationGroup="vgPrestamos" ControlToValidate="txt_fecha_entrega_prestamo" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Usuario entrega} " Display="Dynamic" ValidationGroup="vgPrestamos" ControlToValidate="ddlb_cod_usu_entrega_prestamo" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Folios} " Display="Dynamic" ValidationGroup="vgPrestamos" ControlToValidate="txt_folios_prestamo" />
                                                                <asp:CompareValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Folios : El valor debe ser mayor a 0} " Display="Dynamic" ValidationGroup="vgPrestamos" ControlToValidate="txt_folios_prestamo" ValueToCompare="0" Operator="GreaterThan" Type="Integer" />
                                                                <asp:RangeValidator runat="server" ID="rv_fecha_entrega_prestamo" ValidationGroup="vgPrestamos" ControlToValidate="txt_fecha_entrega_prestamo" />
                                                                <asp:RangeValidator runat="server" ID="rv_fecha_devolucion_prestamo" ValidationGroup="vgPrestamos" ControlToValidate="txt_fecha_devolucion_prestamo" />
                                                                <asp:RequiredFieldValidator runat="server" ID="rfv_fecha_devolucion_prestamo" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Fecha devolución} " Display="Dynamic" ValidationGroup="vgPrestamos" ControlToValidate="txt_fecha_devolucion_prestamo" />
                                                                <asp:RequiredFieldValidator runat="server" ID="rfv_cod_usu_recibe_prestamo" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Usuario recibe} " Display="Dynamic" ValidationGroup="vgPrestamos" ControlToValidate="ddlb_cod_usu_recibe_prestamo" />

                                                            </div>
                                                        </asp:View>

                                                        <asp:View runat="server" ID="vPrestamosDetalle2">
                                                            <div class="divEdit1">
                                                                <div class="fl w410">
                                                                    <asp:Label runat="server" ID="Label38" CssClass="lbl2 w130">Tipo de acción</asp:Label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_id_tipo_prestamo" CssClass="txt2 w150 " AppendDataBoundItems="true" TabIndex="20" AutoPostBack="true" OnSelectedIndexChanged="ddlb_id_tipo_prestamo_SelectedIndexChanged">
                                                                        <asp:ListItem Value="">-- Seleccione</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_id_area_solicita_prestamo2" CssClass="lbl2 w130">Area solicita</asp:Label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_id_area_solicita_prestamo2" CssClass="txt2 w250 " AppendDataBoundItems="true" TabIndex="20" AutoPostBack="true" OnSelectedIndexChanged="ddlb_id_area_solicita_prestamo2_SelectedIndexChanged">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_cod_usu_solicita_prestamo2" CssClass="lbl2 w130">Usuario solicita</asp:Label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_cod_usu_solicita_prestamo2" CssClass="txt2 w250 " AppendDataBoundItems="true" TabIndex="30">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_memorando_interno2" CssClass="lbl2 w130">Memorando</asp:Label>
                                                                    <asp:TextBox runat="server" ID="txt_memorando_interno2" CssClass="txt2 w160 " TabIndex="35"></asp:TextBox>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_fecha_entrega_prestamo2" CssClass="lbl2 w130">Fecha entrega</asp:Label>
                                                                    <ajaxToolkit:CalendarExtender ID="cal_fecha_entrega_prestamo2" runat="server" TargetControlID="txt_fecha_entrega_prestamo2" PopupButtonID="txt_fecha_entrega_prestamo2" Format="yyyy-MM-dd" />
                                                                    <asp:TextBox runat="server" ID="txt_fecha_entrega_prestamo2" CssClass="txtF" TextMode="Date" TabIndex="40"></asp:TextBox>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_cod_usu_entrega_prestamo2" CssClass="lbl2 w130">Usuario entrega</asp:Label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_cod_usu_entrega_prestamo2" CssClass="txt2 w250 " AppendDataBoundItems="true" TabIndex="50">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_fecha_devolucion_prestamo2" CssClass="lbl2 w130">Fecha devolución</asp:Label>
                                                                    <ajaxToolkit:CalendarExtender ID="cal_fecha_devolucion_prestamo2" runat="server" TargetControlID="txt_fecha_devolucion_prestamo2" PopupButtonID="txt_fecha_devolucion_prestamo2" Format="yyyy-MM-dd" />
                                                                    <asp:TextBox runat="server" ID="txt_fecha_devolucion_prestamo2" CssClass="txtF" TextMode="Date" TabIndex="70"></asp:TextBox>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_cod_usu_recibe_prestamo2" CssClass="lbl2 w130">Usuario recibe</asp:Label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_cod_usu_recibe_prestamo2" CssClass="txt2 w250 " AppendDataBoundItems="true" TabIndex="80">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>

                                                                <div class="fl w750 h200 bl">
                                                                    <div class="fl h100 ml10">
                                                                        <label class="lblT1">Predios</label>
                                                                        <br />
                                                                        <br />
                                                                        <label class="lbl3">Filtro</label>
                                                                        <br />
                                                                        <asp:TextBox runat="server" ID="txt_chip_filtro" CssClass="txt2" OnTextChanged="txt_chip_filtro_TextChanged" AutoPostBack="true" TabIndex="59"></asp:TextBox>
                                                                    </div>
                                                                    <div class="fl h200 ml10">
                                                                        <asp:ListBox runat="server" ID="lbx_cod_predio_declarado" CssClass="txt2 w160 h200 mb0" AppendDataBoundItems="true" TabIndex="60" SelectionMode="Multiple"></asp:ListBox>
                                                                    </div>
                                                                    <div class="fl w40 ml10 mt30">
                                                                        <asp:LinkButton ID="lb_cod_predio_declarado" runat="server" CssClass="btn btn-outline-success btn-xs btnAdd w40" Text="" ToolTip="Agregar" CausesValidation="false" OnClick="lb_cod_predio_declarado_Click">
																        <i class="fas fa-chevron-right"></i>
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton ID="lb_cod_predio_declarado_clear" runat="server" CssClass="btn btn-outline-secondary btn-xs btnAdd w40" Text="" ToolTip="Desagregar" CausesValidation="false" OnClick="lb_cod_predio_declarado_clear_Click">
                                                                        <i class="fas fa-chevron-left"></i>
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <div class="fl w300 ml10">
                                                                        <asp:ListBox runat="server" ID="lbx_cod_predio_declarado_add" CssClass="txt2 w160 h200 mb0" AppendDataBoundItems="true" TabIndex="70" SelectionMode="Multiple"></asp:ListBox>
                                                                        <asp:ListBox runat="server" ID="lbx_cod_predio_declarado_add2" CssClass="txt2 w90 h200 mb0" AppendDataBoundItems="true" TabIndex="71" SelectionMode="Multiple"></asp:ListBox>
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <br />
                                                                <div class="divSP h5"></div>

                                                                <asp:Label runat="server" ID="lbl_obs_prestamo2" CssClass="lbl2">Observación</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_obs_prestamo2" CssClass="txt2 txtObs4" TabIndex="90" TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);"></asp:TextBox>

                                                                <asp:ValidationSummary runat="server" ID="vsPrestamos2" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgPrestamos2" EnableClientScript="false" />
                                                                <asp:RequiredFieldValidator runat="server" ID="rfv_id_tipo_prestamo" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Tipo} " Display="Dynamic" ValidationGroup="vgPrestamos2" ControlToValidate="ddlb_id_tipo_prestamo" />
                                                                <asp:RequiredFieldValidator runat="server" ID="rfv_id_area_solicita_prestamo2" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Area solicita} " Display="Dynamic" ValidationGroup="vgPrestamos2" ControlToValidate="ddlb_id_area_solicita_prestamo2" />
                                                                <asp:RequiredFieldValidator runat="server" ID="rfv_cod_usu_solicita_prestamo2" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Usuario solicita} " Display="Dynamic" ValidationGroup="vgPrestamos2" ControlToValidate="ddlb_cod_usu_solicita_prestamo2" />
                                                                <asp:RequiredFieldValidator runat="server" ID="rfv_fecha_entrega_prestamo2" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Fecha entrega} " Display="Dynamic" ValidationGroup="vgPrestamos2" ControlToValidate="txt_fecha_entrega_prestamo2" />
                                                                <asp:RequiredFieldValidator runat="server" ID="rfv_fecha_devolucion_prestamo2" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Fecha devolución} " Display="Dynamic" ValidationGroup="vgPrestamos2" ControlToValidate="txt_fecha_devolucion_prestamo2" />
                                                            </div>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPrestamosVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPrestamosVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnFirstPrestamos" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnBackPrestamos" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnNextPrestamos" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLastPrestamos" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPrestamosEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPrestamosAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPrestamosAdd2" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvPrestamos" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <asp:UpdatePanel runat="server" ID="upPrestamosFoot" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:HiddenField runat="server" ID="hfEvtGVPrestamos" Value="" />
                                            <div class="divSPMessage" id="DivMsgPrestamos"></div>
                                            <div class="divSPFooter">
                                                <asp:Panel class="divSPFooter1" ID="divPrestamosNavegacion" runat="server">
                                                    <%--FOOTER--%>
                                                    <asp:LinkButton ID="btnFirstPrestamos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnPrestamosNavegacion_Click">
													<i class="fas fa-angle-double-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnBackPrestamos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnPrestamosNavegacion_Click">
													<i class="fas fa-angle-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnNextPrestamos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnPrestamosNavegacion_Click">
													<i class="fas fa-angle-right"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLastPrestamos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnPrestamosNavegacion_Click">
													<i class="fas fa-angle-double-right"></i>
                                                    </asp:LinkButton>
                                                </asp:Panel>

                                                <div class="divSPFooter2">
                                                    <asp:Label runat="server" ID="lblPrestamosCuenta"></asp:Label>
                                                </div>

                                                <div class="divSPFooter3">
                                                    <asp:LinkButton ID="btnPrestamosCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnPrestamosCancelar_Click" CausesValidation="false">
													<i class="fas fa-times"></i>&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnPrestamosAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" ValidationGroup="vgPrestamos" OnClientClick="if(Page_ClientValidate('vgPrestamos')) return ShowModalPopup('puPrestamos');">
													<i class="fas fa-check"></i>&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>

                                                <div class="divSPFooter4" id="divPrestamosAction">
                                                    <asp:LinkButton ID="btnPrestamosAdd" runat="server" CssClass="btn4" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnPrestamosAccion_Click">
													<i class="fas fa-plus"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnPrestamosAdd2" runat="server" CssClass="btn4" Text="Agregar" CausesValidation="false" CommandName="Agregar2" CommandArgument="5" ToolTip="Agregar en lote" OnClick="btnPrestamosAccion_Click">
													<i class="fas fa-plus-circle"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnPrestamosEdit" runat="server" CssClass="btn4" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnPrestamosAccion_Click">
													<i class="fas fa-pencil-alt"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>

                                            <!--MODAL POPUP-->
                                            <asp:LinkButton ID="lbDummyPrestamos" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="mpePrestamos" runat="server" PopupControlID="pnlPopupPrestamos" TargetControlID="lbDummyPrestamos" CancelControlID="btnHidePrestamos" BackgroundCssClass="modalBackground" BehaviorID="puPrestamos"></ajaxToolkit:ModalPopupExtender>
                                            <asp:Panel ID="pnlPopupPrestamos" runat="server" CssClass="modalPopup" Style="display: none">
                                                <div class="puHeader">Confirmar acción</div>
                                                <div class="puBody">
                                                    <p>¿Está seguro de continuar con la acción solicitada?</p>
                                                    <asp:LinkButton ID="btnHidePrestamos" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClientClick="return HideModalPopup('puPrestamos');">
                                                    <i class="fas fa-times"></i>&nbsp&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConfirmarPrestamos" runat="server" Text="" CssClass="btn btn-outline-primary btn-sm ml10" OnClick="btnConfirmar_Click">
                                                    <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnPrestamosEdit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPrestamosAdd" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPrestamosAdd2" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <%--********************************************************************************************************************************************************************************--%>
                        <%--******************************************************************************    AFECTACIONES    ******************************************************************************--%>
                        <%--********************************************************************************************************************************************************************************--%>
                        <ajaxToolkit:TabPanel runat="server" ID="tpAfectaciones" HeaderText="Afectaciones">
                            <ContentTemplate>
                                <div class="divSP">
                                    <%--PADRE--%>
                                    <div class="">
                                        <asp:UpdatePanel runat="server" ID="upAfectaciones" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="fl w700 mt10">
                                                    <label class="lbl3 ml195">Alta (m2)</label>
                                                    <label class="lbl3">Media (m2)</label>
                                                    <label class="lbl3">Baja (m2)</label>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk161" Text="Amenaza inundación" CssClass="chk3" Width="180" Enabled="false" />
                                                    <asp:TextBox runat="server" ID="txtAA161" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtAM161" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtAB161" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk162" Text="Amenaza remoción en masa" CssClass="chk3" Width="180" Enabled="false" />
                                                    <asp:TextBox runat="server" ID="txtAA162" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtAM162" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtAB162" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk165" Text="Densidad restringida" CssClass="chk3" Width="180" Enabled="false" />
                                                    <asp:TextBox runat="server" ID="txtAA165" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtAM165" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtAB165" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <br />

                                                    <label class="lbl3 ml195">Area (m2)</label>
                                                    <label class="lbl3 w300">Nombre</label>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk168" Text="Parque" CssClass="chk3" Width="180" Enabled="false" />
                                                    <asp:TextBox runat="server" ID="txtA168" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtObs168" CssClass="txt3 w300 t-l" Enabled="false"></asp:TextBox>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk171" Text="Reserva vial" CssClass="chk3" Width="180" Enabled="false" />
                                                    <asp:TextBox runat="server" ID="txtA171" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtObs171" CssClass="txt3 w300 t-l" Enabled="false"></asp:TextBox>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk174" Text="Sistema de áreas protegidas" CssClass="chk3" Width="180" Enabled="false" />
                                                    <asp:TextBox runat="server" ID="txtA174" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtObs174" CssClass="txt3 w300 t-l" Enabled="false"></asp:TextBox>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk175" Text="ZMPA" CssClass="chk3" Width="180" Enabled="false" />
                                                    <asp:TextBox runat="server" ID="txtA175" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtObs175" CssClass="txt3 w300 t-l" Enabled="false"></asp:TextBox>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk167" Text="Monumento BIC Nacional" CssClass="chk3" Width="180" Enabled="false" />
                                                    <asp:TextBox runat="server" ID="txtObs167" CssClass="txt3 ml105 w300 t-l" Enabled="false"></asp:TextBox>
                                                </div>

                                                <div class="fl w400 mt10">
                                                    <label class="lbl3 ml205">Area (m2)</label>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk166" Text="Influencia aeroportuaria" CssClass="chk3" Width="190" Enabled="false" />
                                                    <asp:TextBox runat="server" ID="txtA166" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk169" Text="Patios SITP" TextAlign="Right" CssClass="chk3" Width="190" Enabled="false" />
                                                    <asp:TextBox runat="server" ID="txtA169" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk170" Text="Recuperación morfológica" CssClass="chk3" Width="190" Enabled="false" />
                                                    <asp:TextBox runat="server" ID="txtA170" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk172" Text="Sector de interés cultural" CssClass="chk3" Width="190" Enabled="false" />
                                                    <asp:TextBox runat="server" ID="txtA172" CssClass="txt3" Enabled="false"></asp:TextBox>
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk163" Text="Bien de interés cultural (BIC)" CssClass="chk3 mt25" Width="190" Enabled="false" />
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk164" Text="Bien colindante con BIC" TextAlign="Right" CssClass="chk3 mt10" Width="190" Enabled="false" />
                                                    <br />

                                                    <asp:CheckBox runat="server" ID="chk173" Text="Servidumbre de alta tensión" TextAlign="Right" CssClass="chk3 mt10" Width="190" Enabled="false" />
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <%--********************************************************************************************************************************************************************************--%>
                        <%--******************************************************************************    PROPIETARIOS    ******************************************************************************--%>
                        <%--********************************************************************************************************************************************************************************--%>
                        <ajaxToolkit:TabPanel runat="server" ID="tpPropietarios" HeaderText="Propietarios">
                            <ContentTemplate>
                                <div class="divSP" onclick="HideMensajeCRUD();">
                                    <!-------------------------------------PREDIOS PROPIETARIOS-------------------------------------------------------------------->
                                    <div class="divSPHeader">
                                        <asp:UpdatePanel runat="server" ID="upPrediosPropietariosBtnVistas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPHeaderTitle"></div>
                                                <div class="divSPHeaderButton">
                                                    <asp:LinkButton ID="btnPrediosPropietariosVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnPrediosPropietariosVista_Click">
														<i class="fas fa-th"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnPrediosPropietariosVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnPrediosPropietariosVista_Click">
														<i class="fas fa-bars"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="">
                                        <asp:UpdatePanel runat="server" ID="upPrediosPropietarios" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPGV">
                                                    <asp:MultiView runat="server" ID="mvPrediosPropietarios" ActiveViewIndex="0" OnActiveViewChanged="mvPrediosPropietarios_ActiveViewChanged">
                                                        <asp:View runat="server" ID="vPrediosPropietariosGrid">
                                                            <asp:GridView ID="gvPrediosPropietarios" CssClass="gv" runat="server" AutoGenerateColumns="False" DataKeyNames="au_predio_propietario" ShowHeaderWhenEmpty="true" OnSelectedIndexChanged="gvPrediosPropietarios_SelectedIndexChanged"
                                                                OnRowDataBound="gvPrediosPropietarios_RowDataBound" AllowSorting="true">
                                                                <Columns>
                                                                    <asp:BoundField DataField="chip" HeaderText="CHIP" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="w100" />
                                                                    <asp:BoundField DataField="fuente_propietario" HeaderText="Fuente" />
                                                                    <asp:BoundField DataField="fecha" HeaderText="Fecha" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="w100" />
                                                                    <asp:BoundField DataField="observacion" HeaderText="Observación" />
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </asp:View>

                                                        <asp:View runat="server" ID="vPrediosPropietariosDetalle">

                                                            <div class="divEdit1">
                                                                <asp:TextBox runat="server" ID="txt_au_predio_propietario" Visible="false"></asp:TextBox>
                                                                <label class="lbl2 lblDis">CHIP</label>
                                                                <asp:TextBox runat="server" ID="txt_chip__predio_propietario" CssClass="txt2 txtDis" TabIndex="1"></asp:TextBox>
                                                                <label class="lbl2 w60">Fuente</label>
                                                                <asp:TextBox runat="server" ID="txt_fuente_propietario" CssClass="txt2 w250" OnTextChanged="PrediosPropietarios_TextChanged" TabIndex="2"></asp:TextBox>
                                                                <label class="lbl2 w60">Fecha</label>
                                                                <ajaxToolkit:CalendarExtender ID="cal_txt_fecha" runat="server" TargetControlID="txt_fecha" PopupButtonID="txt_fecha" Format="yyyy-MM-dd" />
                                                                <asp:TextBox runat="server" ID="txt_fecha" CssClass="txtF" TextMode="Date" OnTextChanged="PrediosPropietarios_TextChanged" TabIndex="5"></asp:TextBox>
                                                                <br />

                                                                <label class="lbl2">Observación</label>
                                                                <asp:TextBox runat="server" ID="txt_observacion__predio_propietario" CssClass="txt2 txtObs1" OnTextChanged="PrediosPropietarios_TextChanged" TabIndex="10"
                                                                    TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);"></asp:TextBox>
                                                                <br />

                                                                <asp:ValidationSummary runat="server" ID="vsPrediosPropietarios" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgPrediosPropietarios" EnableClientScript="false" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Fuente} " Display="Dynamic" ValidationGroup="vgPrediosPropietarios" ControlToValidate="txt_fuente_propietario" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Fecha} " Display="Dynamic" ValidationGroup="vgPrediosPropietarios" ControlToValidate="txt_fecha" />
                                                            </div>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPrediosPropietariosVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPrediosPropietariosVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnFirstPrediosPropietarios" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnBackPrediosPropietarios" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnNextPrediosPropietarios" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLastPrediosPropietarios" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPrediosPropietariosEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPrediosPropietariosAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPrediosPropietariosDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvPrediosPropietarios" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <asp:UpdatePanel runat="server" ID="upPrediosPropietariosFoot" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:HiddenField runat="server" ID="hfEvtGVPrediosPropietarios" Value="" />
                                            <div class="divSPMessage" id="DivMsgPrediosPropietarios"></div>

                                            <div class="divSPFooter">
                                                <asp:Panel class="divSPFooter1" ID="divPrediosPropietariosNavegacion" runat="server">
                                                    <%--FOOTER--%>
                                                    <asp:LinkButton ID="btnFirstPrediosPropietarios" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnPrediosPropietariosNavegacion_Click">
													<i class="fas fa-angle-double-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnBackPrediosPropietarios" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnPrediosPropietariosNavegacion_Click">
													<i class="fas fa-angle-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnNextPrediosPropietarios" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnPrediosPropietariosNavegacion_Click">
													<i class="fas fa-angle-right"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLastPrediosPropietarios" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnPrediosPropietariosNavegacion_Click">
													<i class="fas fa-angle-double-right"></i>
                                                    </asp:LinkButton>
                                                </asp:Panel>

                                                <div class="divSPFooter2">
                                                    <asp:Label runat="server" ID="lblPrediosPropietariosCuenta"></asp:Label>
                                                </div>

                                                <div class="divSPFooter3">
                                                    <asp:LinkButton ID="btnPrediosPropietariosCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnPrediosPropietariosCancelar_Click" CausesValidation="false">
													<i class="fas fa-times"></i>&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnPrediosPropietariosAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" ValidationGroup="vgPrediosPropietarios" OnClientClick="if(Page_ClientValidate('vgPrediosPropietarios')) return ShowModalPopup('puPrediosPropietarios');">
													<i class="fas fa-check"></i>&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>

                                                <div class="divSPFooter4" id="divPrediosPropietariosAction">
                                                    <asp:LinkButton ID="btnPrediosPropietariosAdd" runat="server" CssClass="btn4" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnPrediosPropietariosAccion_Click">
													<i class="fas fa-plus"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnPrediosPropietariosEdit" runat="server" CssClass="btn4" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnPrediosPropietariosAccion_Click">
													<i class="fas fa-pencil-alt"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnPrediosPropietariosDel" runat="server" CssClass="btn4" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnPrediosPropietariosAccion_Click" OnClientClick="LimpiarMensajeWeb();">
													<i class="far fa-trash-alt"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>

                                            <!--MODAL POPUP-->
                                            <asp:LinkButton ID="lbDummyPrediosPropietarios" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="mpePrediosPropietarios" runat="server" PopupControlID="pnlPopupPrediosPropietarios" TargetControlID="lbDummyPrediosPropietarios"
                                                CancelControlID="btnHidePrediosPropietarios" BackgroundCssClass="modalBackground" BehaviorID="puPrediosPropietarios">
                                            </ajaxToolkit:ModalPopupExtender>
                                            <asp:Panel ID="pnlPopupPrediosPropietarios" runat="server" CssClass="modalPopup" Style="display: none">
                                                <div class="puHeader">Confirmar acción</div>
                                                <div class="puBody">
                                                    <p>¿Está seguro de continuar con la acción solicitada?</p>
                                                    <asp:LinkButton ID="btnHidePrediosPropietarios" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClientClick="return HideModalPopup('puPrediosPropietarios');">
                                                    <i class="fas fa-times"></i>&nbsp&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConfirmarPrediosPropietarios" runat="server" Text="" CssClass="btn btn-outline-primary btn-sm ml10" OnClick="btnConfirmar_Click">
                                                    <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnPrediosPropietariosEdit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPrediosPropietariosAdd" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPrediosPropietariosDel" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                                    <!------------------------------------------- PROPIETARIOS-------------------------------------------------------------------->
                                    <div class="divSPHeader">
                                        <asp:UpdatePanel runat="server" ID="upPropietariosBtnVistas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPHeaderTitle"></div>
                                                <div class="divSPHeaderButton">
                                                    <asp:LinkButton ID="btnPropietariosVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnPropietariosVista_Click">
													<i class="fas fa-th"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnPropietariosVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnPropietariosVista_Click">
													<i class="fas fa-bars"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="">
                                        <asp:UpdatePanel runat="server" ID="upPropietarios" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPGV">
                                                    <asp:MultiView runat="server" ID="mvPropietarios" ActiveViewIndex="0" OnActiveViewChanged="mvPropietarios_ActiveViewChanged">
                                                        <asp:View runat="server" ID="vPropietariosGrid">
                                                            <asp:GridView ID="gvPropietarios" CssClass="gv" runat="server" AutoGenerateColumns="False" DataKeyNames="au_propietario" ShowHeaderWhenEmpty="true" OnSelectedIndexChanged="gvPropietarios_SelectedIndexChanged"
                                                                OnRowDataBound="gvPropietarios_RowDataBound" AllowSorting="true">
                                                                <Columns>
                                                                    <asp:BoundField DataField="nombre_propietario" HeaderText="Propietario" SortExpression="nombre_propietario" />
                                                                    <asp:BoundField DataField="tipo_doc_propietario" HeaderText="Tipo doc." />
                                                                    <asp:BoundField DataField="num_doc_propietario" HeaderText="Núm documento" ItemStyle-HorizontalAlign="Right" />
                                                                    <asp:BoundField DataField="telefono_propietario" HeaderText="Teléfono" />
                                                                    <asp:BoundField DataField="direccion_propietario" HeaderText="Dirección" />
                                                                    <asp:BoundField DataField="correo_propietario" HeaderText="Correo" />
                                                                    <asp:BoundField DataField="observacion" HeaderText="Observación" />
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </asp:View>

                                                        <asp:View runat="server" ID="vPropietariosDetalle">
                                                            <div class="divEdit1">
                                                                <asp:TextBox runat="server" ID="txt_au_propietario" Visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" ID="txt_cod_predio_propietario" Visible="false"></asp:TextBox>
                                                                <label class="lbl4 w1100 ml15">Propietario</label>
                                                                <br />

                                                                <label class="lbl2">Nombre</label>
                                                                <asp:TextBox runat="server" ID="txt_nombre_propietario" CssClass="txt2 w995 " OnTextChanged="Propietarios_TextChanged" TabIndex="1"></asp:TextBox>
                                                                <br />

                                                                <label class="lbl2">Tipo doc.</label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_tipo_doc_propietario" CssClass="txt2 w150 " AppendDataBoundItems="true" OnTextChanged="Propietarios_TextChanged" TabIndex="2">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <label class="lbl2 w140">Número documento</label>
                                                                <asp:TextBox runat="server" ID="txt_num_doc_propietario" CssClass="txt2 w185 t-r" OnTextChanged="Propietarios_TextChanged" TabIndex="3"></asp:TextBox>
                                                                <label class="lbl2">Teléfono</label>
                                                                <asp:TextBox runat="server" ID="txt_telefono_propietario" CssClass="txt2 w370 " OnTextChanged="Propietarios_TextChanged" TabIndex="4"></asp:TextBox>
                                                                <br />

                                                                <label class="lbl2">Dirección</label>
                                                                <asp:TextBox runat="server" ID="txt_direccion_propietario" CssClass="txt2 w500 " OnTextChanged="Propietarios_TextChanged" TabIndex="5"></asp:TextBox>
                                                                <label class="lbl2">Correo</label>
                                                                <asp:TextBox runat="server" ID="txt_correo_propietario" CssClass="txt2 w370 " OnTextChanged="Propietarios_TextChanged" TabIndex="6"></asp:TextBox>
                                                                <br />

                                                                <label class="lbl2 w100">Observación</label>
                                                                <asp:TextBox runat="server" ID="txt_observacion__propietario" CssClass="txt2 txtObs" OnTextChanged="Propietarios_TextChanged" TabIndex="21"
                                                                    TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);"></asp:TextBox>
                                                                <br />

                                                                <label class="lbl4 w1100 ml15">Representante</label>
                                                                <br />

                                                                <label class="lbl2">Nombre</label>
                                                                <asp:TextBox runat="server" ID="txt_nombre_representante" CssClass="txt2 w995 " OnTextChanged="Propietarios_TextChanged" TabIndex="11"></asp:TextBox>
                                                                <br />

                                                                <label class="lbl2">Tipo doc.</label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_tipo_doc_representante" CssClass="txt2 w150 " AppendDataBoundItems="true" OnTextChanged="Propietarios_TextChanged" TabIndex="12">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <label class="lbl2 w140">Número documento</label>
                                                                <asp:TextBox runat="server" ID="txt_num_doc_representante" CssClass="txt2 w185 t-r" OnTextChanged="Propietarios_TextChanged" TabIndex="13"></asp:TextBox>
                                                                <label class="lbl2">Teléfono</label>
                                                                <asp:TextBox runat="server" ID="txt_telefono_representante" CssClass="txt2 w370 " OnTextChanged="Propietarios_TextChanged" TabIndex="14"></asp:TextBox>
                                                                <br />

                                                                <label class="lbl2">Dirección</label>
                                                                <asp:TextBox runat="server" ID="txt_direccion_representante" CssClass="txt2 w500 " OnTextChanged="Propietarios_TextChanged" TabIndex="15"></asp:TextBox>
                                                                <label class="lbl2">Correo</label>
                                                                <asp:TextBox runat="server" ID="txt_correo_representante" CssClass="txt2 w370 " OnTextChanged="Propietarios_TextChanged" TabIndex="16"></asp:TextBox>
                                                            </div>
                                                            <asp:ValidationSummary runat="server" ID="vsPropietarios" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgPropietarios" EnableClientScript="false" />
                                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Nombre Propietario} " Display="Dynamic" ValidationGroup="vgPropietarios" ControlToValidate="txt_nombre_propietario" />
                                                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Tipo Documento} " Display="Dynamic" ValidationGroup="vgPropietarios" ControlToValidate="ddlb_id_tipo_doc_propietario" />

                                                        </asp:View>
                                                    </asp:MultiView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPropietariosVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPropietariosVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnFirstPropietarios" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnBackPropietarios" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnNextPropietarios" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLastPropietarios" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPropietariosEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPropietariosAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPropietariosDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvPropietarios" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <asp:UpdatePanel runat="server" ID="upPropietariosFoot" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:HiddenField runat="server" ID="hfEvtGVPropietarios" Value="" />
                                            <div class="divSPMessage" id="DivMsgPropietarios"></div>
                                            <div class="divSPFooter">
                                                <asp:Panel class="divSPFooter1" ID="divPropietariosNavegacion" runat="server">
                                                    <%--FOOTER--%>
                                                    <asp:LinkButton ID="btnFirstPropietarios" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnPropietariosNavegacion_Click">
													<i class="fas fa-angle-double-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnBackPropietarios" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnPropietariosNavegacion_Click">
													<i class="fas fa-angle-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnNextPropietarios" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnPropietariosNavegacion_Click">
													<i class="fas fa-angle-right"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLastPropietarios" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnPropietariosNavegacion_Click">
													<i class="fas fa-angle-double-right"></i>
                                                    </asp:LinkButton>
                                                </asp:Panel>

                                                <div class="divSPFooter2">
                                                    <asp:Label runat="server" ID="lblPropietariosCuenta"></asp:Label>
                                                </div>

                                                <div class="divSPFooter3">
                                                    <asp:LinkButton ID="btnPropietariosCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnPropietariosCancelar_Click" CausesValidation="false">
													<i class="fas fa-times"></i>&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnPropietariosAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" ValidationGroup="vgPropietarios" OnClientClick="if(Page_ClientValidate('vgPropietarios')) return ShowModalPopup('puPropietarios');">
													<i class="fas fa-check"></i>&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>

                                                <div class="divSPFooter4" id="divPropietariosAction">
                                                    <asp:LinkButton ID="btnPropietariosAdd" runat="server" CssClass="btn4" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnPropietariosAccion_Click">
													<i class="fas fa-plus"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnPropietariosEdit" runat="server" CssClass="btn4" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnPropietariosAccion_Click">
													<i class="fas fa-pencil-alt"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnPropietariosDel" runat="server" CssClass="btn4" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnPropietariosAccion_Click" OnClientClick="LimpiarMensajeWeb();">
													<i class="far fa-trash-alt"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>

                                            <!--MODAL POPUP-->
                                            <asp:LinkButton ID="lbDummyPropietarios" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="mpePropietarios" runat="server" PopupControlID="pnlPopupPropietarios" TargetControlID="lbDummyPropietarios"
                                                CancelControlID="btnHidePropietarios" BackgroundCssClass="modalBackground" BehaviorID="puPropietarios">
                                            </ajaxToolkit:ModalPopupExtender>
                                            <asp:Panel ID="pnlPopupPropietarios" runat="server" CssClass="modalPopup" Style="display: none">
                                                <div class="puHeader">Confirmar acción</div>
                                                <div class="puBody">
                                                    <p>¿Está seguro de continuar con la acción solicitada?</p>
                                                    <asp:LinkButton ID="btnHidePropietarios" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClientClick="return HideModalPopup('puPropietarios');">
                                                    <i class="fas fa-times"></i>&nbsp&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConfirmarPropietarios" runat="server" Text="" CssClass="btn btn-outline-primary btn-sm ml10" OnClick="btnConfirmar_Click">
                                                    <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnPropietariosEdit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPropietariosAdd" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPropietariosDel" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <%--********************************************************************************************************************************************************************************--%>
                        <%--******************************************************************************    OBSERVACIONES   ******************************************************************************--%>
                        <%--********************************************************************************************************************************************************************************--%>
                        <ajaxToolkit:TabPanel runat="server" ID="tpObservaciones" HeaderText="Observaciones">
                            <ContentTemplate>
                                <div class="divSP" onclick="HideMensajeCRUD();">
                                    <div class="divSPHeader">
                                        <asp:UpdatePanel runat="server" ID="upObservacionesBtnVistas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPHeaderTitle"></div>
                                                <div class="divSPHeaderButton">
                                                    <asp:LinkButton ID="btnObservacionesVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnObservacionesVista_Click">
													<i class="fas fa-th"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnObservacionesVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnObservacionesVista_Click">
													<i class="fas fa-bars"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="">
                                        <asp:UpdatePanel runat="server" ID="upObservaciones" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPGV">
                                                    <asp:MultiView runat="server" ID="mvObservaciones" ActiveViewIndex="0" OnActiveViewChanged="mvObservaciones_ActiveViewChanged">
                                                        <asp:View runat="server" ID="vObservacionesGrid">
                                                            <asp:GridView ID="gvObservaciones" CssClass="gv" runat="server" AutoGenerateColumns="False" DataKeyNames="au_pd_observacion,cod_usu" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="500"
                                                                AllowSorting="true" OnSorting="gvObservaciones_Sorting" OnSelectedIndexChanged="gvObservaciones_SelectedIndexChanged" OnPageIndexChanging="gvObservaciones_PageIndexChanging"
                                                                OnRowCreated="gvObservaciones_RowCreated" OnDataBinding="gvObservaciones_DataBinding" OnRowDataBound="gvObservaciones_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_pd_observacion" HeaderText="au_pd_observacion" Visible="false" />
                                                                    <asp:BoundField DataField="cod_predio_declarado" HeaderText="Cod. predio" Visible="false" />
                                                                    <asp:BoundField DataField="fecha_observacion" HeaderText="Fecha observación" ItemStyle-CssClass="w100" SortExpression="fecha_observacion" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="estado_predio_obs" HeaderText="Estado predio obs." SortExpression="estado_predio_obs" ItemStyle-CssClass="w250" HtmlEncode="true" />
                                                                    <asp:BoundField DataField="observacion" HeaderText="Observación" />
                                                                    <asp:BoundField DataField="usu" HeaderText="Usuario digita" ItemStyle-CssClass="w200" />
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </asp:View>

                                                        <asp:View runat="server" ID="vObservacionesDetalle">
                                                            <div class="divEdit1">
                                                                <asp:TextBox runat="server" ID="txt_au_pd_observacion" Visible="false"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_fecha_observacion" CssClass="lbl2 lblDis">Fecha obs.</asp:Label>
                                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_observacion" runat="server" TargetControlID="txt_fecha_observacion" PopupButtonID="txt_fecha_observacion" Format="yyyy-MM-dd" />
                                                                <asp:TextBox runat="server" ID="txt_fecha_observacion" CssClass="txtF txtDis" TextMode="Date" OnTextChanged="Observaciones_TextChanged" TabIndex="1"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_estado_predio_obs" CssClass="lbl2 w140">Estado predio obs.</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_estado_predio_obs" CssClass="txt2 w270" AppendDataBoundItems="true" OnTextChanged="Observaciones_TextChanged" TabIndex="10">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="lbl_cod_usu__observacion" CssClass="lbl2 lblDis">Usuario digita</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_cod_usu__observacion" CssClass="txt2 txtDis w250" AppendDataBoundItems="true" TabIndex="20">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />

                                                                <asp:Label runat="server" ID="lbl_observacion" CssClass="lbl2">Observación</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_observacion" CssClass="txt2 txtObs1" TextMode="MultiLine"
                                                                    onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);" OnTextChanged="Observaciones_TextChanged" TabIndex="30"></asp:TextBox>
                                                                <br />

                                                                <asp:ValidationSummary runat="server" ID="vsObservaciones" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgObservaciones" EnableClientScript="false" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Fecha observación} " Display="Dynamic" ValidationGroup="vgObservaciones" ControlToValidate="txt_fecha_observacion" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Estado predio obs.} " Display="Dynamic" ValidationGroup="vgObservaciones" ControlToValidate="ddlb_id_estado_predio_obs" />
                                                            </div>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnObservacionesVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnObservacionesVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnFirstObservaciones" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnBackObservaciones" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnNextObservaciones" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLastObservaciones" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnObservacionesEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnObservacionesAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnObservacionesDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvObservaciones" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <asp:UpdatePanel runat="server" ID="upObservacionesFoot" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:HiddenField runat="server" ID="hfEvtGVObservaciones" Value="" />
                                            <div class="divSPMessage" id="DivMsgObservaciones"></div>
                                            <div class="divSPFooter">
                                                <asp:Panel class="divSPFooter1" ID="divObservacionesNavegacion" runat="server">
                                                    <%--FOOTER--%>
                                                    <asp:LinkButton ID="btnFirstObservaciones" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnObservacionesNavegacion_Click">
													<i class="fas fa-angle-double-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnBackObservaciones" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnObservacionesNavegacion_Click">
													<i class="fas fa-angle-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnNextObservaciones" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnObservacionesNavegacion_Click">
													<i class="fas fa-angle-right"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLastObservaciones" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnObservacionesNavegacion_Click">
													<i class="fas fa-angle-double-right"></i>
                                                    </asp:LinkButton>
                                                </asp:Panel>

                                                <div class="divSPFooter2">
                                                    <asp:Label runat="server" ID="lblObservacionesCuenta"></asp:Label>
                                                </div>

                                                <div class="divSPFooter3">
                                                    <asp:LinkButton ID="btnObservacionesCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnObservacionesCancelar_Click" CausesValidation="false">
                                                    <i class="fas fa-times"></i>&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnObservacionesAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" TabIndex="1000" Text="Aceptar" ValidationGroup="vgObservaciones" OnClientClick="if(Page_ClientValidate('vgObservaciones')) return ShowModalPopup('puObservaciones');">
                                                    <i class="fas fa-check"></i>&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>

                                                <div class="divSPFooter4" id="divObservacionesAction">
                                                    <asp:LinkButton ID="btnObservacionesAdd" runat="server" CssClass="btn4" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnObservacionesAccion_Click">
													<i class="fas fa-plus"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnObservacionesEdit" runat="server" CssClass="btn4" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnObservacionesAccion_Click">
													<i class="fas fa-pencil-alt"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnObservacionesDel" runat="server" CssClass="btn4" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnObservacionesAccion_Click">
													<i class="far fa-trash-alt"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>

                                            <!--MODAL POPUP-->
                                            <asp:LinkButton ID="lbDummyObservaciones" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="mpeObservaciones" runat="server" PopupControlID="pnlPopupObservaciones" TargetControlID="lbDummyObservaciones" CancelControlID="btnHideObservaciones" BackgroundCssClass="modalBackground" BehaviorID="puObservaciones"></ajaxToolkit:ModalPopupExtender>
                                            <asp:Panel ID="pnlPopupObservaciones" runat="server" CssClass="modalPopup" Style="display: none">
                                                <div class="puHeader">Confirmar acción</div>
                                                <div class="puBody">
                                                    <p>¿Está seguro de continuar con la acción solicitada?</p>
                                                    <asp:LinkButton ID="btnHideObservaciones" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClientClick="return HideModalPopup('puObservaciones');">
                                                    <i class="fas fa-times"></i>&nbsp&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConfirmarObservaciones" runat="server" Text="" CssClass="btn btn-outline-primary btn-sm ml10" OnClick="btnConfirmar_Click">
                                                    <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnObservacionesEdit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnObservacionesAdd" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnObservacionesDel" EventName="Click" />
                                            <asp:PostBackTrigger ControlID="btnConfirmarObservaciones" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <%--********************************************************************************************************************************************************************************--%>
                        <%--******************************************************************************       VISITAS      ******************************************************************************--%>
                        <%--********************************************************************************************************************************************************************************--%>
                        <ajaxToolkit:TabPanel runat="server" ID="tpVisitas" HeaderText="Visitas">
                            <ContentTemplate>
                                <div class="divSP" onclick="HideMensajeCRUD();">
                                    <div class="divSPHeader">
                                        <asp:UpdatePanel runat="server" ID="upVisitasBtnVistas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPHeaderTitle"></div>
                                                <div class="divSPHeaderButton">
                                                    <asp:LinkButton ID="btnVisitasVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnVisitasVista_Click">
													<i class="fas fa-th"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnVisitasVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnVisitasVista_Click">
													<i class="fas fa-bars"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="">
                                        <asp:UpdatePanel runat="server" ID="upVisitas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPGV">
                                                    <asp:MultiView runat="server" ID="mvVisitas" ActiveViewIndex="0" OnActiveViewChanged="mvVisitas_ActiveViewChanged">
                                                        <asp:View runat="server" ID="vVisitasGrid">
                                                            <asp:GridView ID="gvVisitas" CssClass="gv" runat="server" AutoGenerateColumns="False" DataKeyNames="au_visita" ShowHeaderWhenEmpty="true" OnSelectedIndexChanged="gvVisitas_SelectedIndexChanged"
                                                                OnRowDataBound="gvVisitas_RowDataBound" AllowSorting="true">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_visita" Visible="false" />
                                                                    <asp:BoundField DataField="cod_predio_declarado" HeaderText="Cod Predio" Visible="false" />
                                                                    <asp:BoundField DataField="tipo_visita" HeaderText="Tipo visita" ItemStyle-CssClass="" />
                                                                    <asp:BoundField DataField="usu_visita" HeaderText="Usuario elabora" ItemStyle-CssClass="" />
                                                                    <asp:BoundField DataField="fecha_visita" SortExpression="fecha_visita" HeaderText="Fecha elabora" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="" />
                                                                    <asp:BoundField DataField="estado_visita" HeaderText="Estado visita" ItemStyle-CssClass="" />
                                                                    <asp:BoundField DataField="obs_visita" HeaderText="Observación" />
                                                                    <asp:BoundField DataField="usu_digita" HeaderText="Usuario digita" ItemStyle-CssClass="w100" />
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </asp:View>

                                                        <asp:View runat="server" ID="vVisitasDetalle">
                                                            <div class="divEdit1">
                                                                <asp:TextBox runat="server" ID="txt_au_visita" Visible="false"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_id_tipo_visita" class="lbl1">Tipo visita</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_tipo_visita" CssClass="txt2 w150" AppendDataBoundItems="true" OnTextChanged="Visitas_TextChanged" TabIndex="5" AutoPostBack="true" OnSelectedIndexChanged="ddlb_id_tipo_visita_SelectedIndexChanged">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="lbl_cod_usu_visita" class="lbl1">Usuario</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_cod_usu_visita" CssClass="txt2 w250" AppendDataBoundItems="true" OnTextChanged="Visitas_TextChanged" TabIndex="10">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="lbl_fecha_visita" class="lbl1">Fecha visita</asp:Label>
                                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_visita" runat="server" TargetControlID="txt_fecha_visita" PopupButtonID="txtFecVisita" Format="yyyy-MM-dd" />
                                                                <asp:TextBox runat="server" ID="txt_fecha_visita" CssClass="txt2 w140 t-c" TextMode="Date" OnTextChanged="Visitas_TextChanged" TabIndex="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_id_estado_visita" class="lbl1">Estado visita</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_estado_visita" CssClass="txt2 w180" AppendDataBoundItems="true" OnTextChanged="Visitas_TextChanged" TabIndex="20">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <div class="divS h10"></div>

                                                                <%--*******************************--%>
                                                                <%--************* t-b *************--%>
                                                                <%--*******************************--%>
                                                                <ajaxToolkit:TabContainer runat="server" ID="TabContainerVisitas" Width="1180px" CssClass="" ActiveTabIndex="0" OnActiveTabChanged="TabContainerVisitas_ActiveTabChanged" AutoPostBack="False">
                                                                    <ajaxToolkit:TabPanel runat="server" ID="tpVisitas1" HeaderText="Formato nuevo">
                                                                        <ContentTemplate>
                                                                            <label class="lblT1 w_99a fs12">Información del predio y su entorno</label>
                                                                            <div class="h110">
                                                                                <div class="fl w150 br">
                                                                                    <label class="lbl3 w120 ml15">Valla de licencias</label>
                                                                                    <asp:CheckBox runat="server" ID="chk_lic_construccion" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="31" Text="Construcción" />
                                                                                    <br />
                                                                                    <asp:CheckBox runat="server" ID="chk_lic_urbanismo" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="32" Text="Urbanismo" />
                                                                                    <br />
                                                                                    <asp:CheckBox runat="server" ID="chk_lic_sin_valla" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="33" Text="Sin valla" />
                                                                                </div>

                                                                                <div class="fl w180">
                                                                                    <asp:Label runat="server" ID="lbl_id_ocupacion_visita" class="lbl3 w150 ml15">Ocupación</asp:Label>
                                                                                    <asp:DropDownList runat="server" ID="ddlb_id_ocupacion_visita" CssClass="txt2 w160 ml10" AppendDataBoundItems="true" OnTextChanged="Visitas_TextChanged" TabIndex="34">
                                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </div>

                                                                                <div class="fl w320 bl">
                                                                                    <label class="lbl3 ml15 w290">Actividad desarrollada</label>
                                                                                    <br />
                                                                                    <asp:CheckBox runat="server" ID="chk_act_viv_unifamiliar" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="41" Text="Viv. unifamiliar" />
                                                                                    <asp:CheckBox runat="server" ID="chk_act_viv_multifamiliar" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="42" Text="Viv. multifamiliar" />
                                                                                    <asp:CheckBox runat="server" ID="chk_act_parqueadero" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="43" Text="Parqueadero" />
                                                                                    <asp:CheckBox runat="server" ID="chk_act_comercio" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="44" Text="Comercio" />
                                                                                    <br />
                                                                                    <asp:Label runat="server" ID="lbl_act_otro" class="lbl8">Otro</asp:Label>
                                                                                    <asp:TextBox runat="server" ID="txt_act_otro" CssClass="txt2 w240" MaxLength="40" OnTextChanged="Visitas_TextChanged" OnCheckedChanged="Visitas_TextChanged" TabIndex="45"></asp:TextBox>
                                                                                </div>

                                                                                <div class="fl w330 bl">
                                                                                    <label class="lbl3 ml15 w290">Entorno</label>
                                                                                    <br />
                                                                                    <asp:CheckBox runat="server" ID="chk_ent_viv_unifamiliar" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="51" Text="Viv. unifamiliar" />
                                                                                    <asp:CheckBox runat="server" ID="chk_ent_viv_multifamiliar" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="52" Text="Viv. multifamiliar" />
                                                                                    <asp:CheckBox runat="server" ID="chk_ent_comercio" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="53" Text="Comercio" />
                                                                                    <asp:CheckBox runat="server" ID="chk_ent_industria" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="54" Text="Industria" />
                                                                                    <br />
                                                                                    <asp:Label runat="server" ID="lbl_ent_otro" class="lbl8">Otro</asp:Label>
                                                                                    <asp:TextBox runat="server" ID="txt_ent_otro" CssClass="txt2 w240" MaxLength="40" OnTextChanged="Visitas_TextChanged" TabIndex="55"></asp:TextBox>
                                                                                </div>

                                                                                <div class="fl w150 bl">
                                                                                    <label class="lbl3 ml15 w120">Accesibilidad</label>
                                                                                    <asp:CheckBox runat="server" ID="chk_acc_via_vehicular" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="61" Text="Vía vehicular" />
                                                                                    <asp:CheckBox runat="server" ID="chk_acc_via_peatonal" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="62" Text="Vía peatonal" />
                                                                                    <asp:CheckBox runat="server" ID="chk_acc_ninguna" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="63" Text="Ninguna" />
                                                                                </div>
                                                                                <br />
                                                                            </div>

                                                                            <label class="lblT1 w_99a fs12">Información técnica del predio (aplica para predios con afectación por inundación o remoción en masa)</label>
                                                                            <div class="h170">
                                                                                <asp:Label runat="server" ID="lbl_id_pendiente_lote" class="lbl2 w120">Pendiente lote</asp:Label>
                                                                                <asp:DropDownList runat="server" ID="ddlb_id_pendiente_lote" CssClass="txt2 w200" AppendDataBoundItems="true" OnTextChanged="Visitas_TextChanged" TabIndex="71">
                                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                                </asp:DropDownList>

                                                                                <asp:Label runat="server" ID="lbl_id_pendiente_ladera" class="lbl2 w120">Pendiente ladera</asp:Label>
                                                                                <asp:DropDownList runat="server" ID="ddlb_id_pendiente_ladera" CssClass="txt2 w200" AppendDataBoundItems="true" OnTextChanged="Visitas_TextChanged" TabIndex="72">
                                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <br />

                                                                                <div class="fl w260 br">
                                                                                    <label class="lbl3 w220 ml15">Cobertura</label>
                                                                                    <asp:CheckBox runat="server" ID="chk_cob_vivienda" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="81" Text="Vivienda" />
                                                                                    <asp:CheckBox runat="server" ID="chk_cob_pastos" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="82" Text="Pastos" />
                                                                                    <asp:CheckBox runat="server" ID="chk_cob_rastrojo" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="83" Text="Rastrojo bajo" />
                                                                                    <asp:CheckBox runat="server" ID="chk_cob_bosque" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="84" Text="Bosque" />
                                                                                    <asp:CheckBox runat="server" ID="chk_cob_sin_cobertura" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="85" Text="Sin cobertura" />
                                                                                    <br />
                                                                                    <asp:Label runat="server" ID="lbl_cob_otro" class="lbl8">Otro</asp:Label>
                                                                                    <asp:TextBox runat="server" ID="txt_cob_otro" CssClass="txt2 w170" MaxLength="40" OnTextChanged="Visitas_TextChanged" TabIndex="86"></asp:TextBox>
                                                                                </div>

                                                                                <div class="fl w300 br">
                                                                                    <label class="lbl3 w260 ml15">Evidencias de inestabilidad</label>
                                                                                    <asp:CheckBox runat="server" ID="chk_inest_fisuras" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="91" Text="Fisuras" />
                                                                                    <asp:CheckBox runat="server" ID="chk_inest_fracturas" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="92" Text="Fracturas" />
                                                                                    <asp:CheckBox runat="server" ID="chk_inest_escarpe" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="93" Text="Escarpe" />
                                                                                    <asp:CheckBox runat="server" ID="chk_inest_corona" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="94" Text="Corona desl." ToolTip="Corona de deslizamiento" />
                                                                                    <asp:CheckBox runat="server" ID="chk_inest_depositos" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="95" Text="Depósitos ML" ToolTip="Depósitos de media ladera" />
                                                                                    <asp:CheckBox runat="server" ID="chk_inest_ninguna" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="96" Text="Ninguna" />
                                                                                    <br />
                                                                                    <asp:Label runat="server" ID="lbl_inest_otro" class="lbl8">Otro</asp:Label>
                                                                                    <asp:TextBox runat="server" ID="txt_inest_otro" CssClass="txt2 w220" MaxLength="40" OnTextChanged="Visitas_TextChanged" TabIndex="97"></asp:TextBox>
                                                                                </div>

                                                                                <div class="fl w300 br">
                                                                                    <label class="lbl3 w270 ml15">Fuentes de aguas superficiales</label>
                                                                                    <asp:CheckBox runat="server" ID="chk_agua_interna" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="101" Text="Interna" />
                                                                                    <asp:CheckBox runat="server" ID="chk_agua_limitrofe" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="102" Text="Limítrofe" />
                                                                                    <asp:CheckBox runat="server" ID="chk_agua_amortiguacion" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="103" Text="Zona amort." ToolTip="Zona amortiguación" />
                                                                                    <asp:CheckBox runat="server" ID="chk_agua_obras" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="104" ToolTip="Obras de control de inundación interna" Text="Obras de control" />
                                                                                    <br />
                                                                                    <asp:Label runat="server" ID="lbl_agua_otro" class="lbl8">Otro</asp:Label>
                                                                                    <asp:TextBox runat="server" ID="txt_agua_otro" CssClass="txt2 w220" MaxLength="40" OnTextChanged="Visitas_TextChanged" TabIndex="105"></asp:TextBox>
                                                                                </div>

                                                                                <div class="fl w300">
                                                                                    <label class="lbl3 w270 ml15">Aspectos geomorfológicos</label>
                                                                                    <asp:CheckBox runat="server" ID="chk_geom_depositos" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="111" Text="Depositos col" ToolTip="Depositos coluviales" />
                                                                                    <asp:CheckBox runat="server" ID="chk_geom_llenos" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="112" Text="Llenos" />
                                                                                    <asp:CheckBox runat="server" ID="chk_geom_escarpes" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="113" Text="Escarpes" />
                                                                                    <asp:CheckBox runat="server" ID="chk_geom_llanuras" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="114" Text="Llanuras aluv." ToolTip="Llanuras aluviales" />
                                                                                    <br />
                                                                                    <asp:Label runat="server" ID="lbl_geom_otro" class="lbl8">Otro</asp:Label>
                                                                                    <asp:TextBox runat="server" ID="txt_geom_otro" CssClass="txt2 w220" MaxLength="40" OnTextChanged="Visitas_TextChanged" TabIndex="115"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </ajaxToolkit:TabPanel>

                                                                    <ajaxToolkit:TabPanel runat="server" ID="tpVisitas2" HeaderText="Formato anterior">
                                                                        <ContentTemplate>
                                                                            <div class="divS h10"></div>

                                                                            <asp:Label runat="server" ID="lbl_id_uso_lote" class="lbl2 w70">Uso lote</asp:Label>
                                                                            <asp:DropDownList runat="server" ID="ddlb_id_uso_lote" CssClass="txt2 w250" AppendDataBoundItems="true" OnTextChanged="Visitas_TextChanged" TabIndex="201">
                                                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:Label runat="server" ID="lbl_uso_entorno" class="lbl2">Uso entorno</asp:Label>
                                                                            <asp:TextBox runat="server" ID="txt_uso_entorno" CssClass="txt2 w500" OnTextChanged="Visitas_TextChanged" TabIndex="202"></asp:TextBox>
                                                                            <br />

                                                                            <asp:Label runat="server" ID="lbl_id_estado_vias_internas" class="lbl2 w140">Estado vías internas</asp:Label>
                                                                            <asp:DropDownList runat="server" ID="ddlb_id_estado_vias_internas" CssClass="txt2 w150" AppendDataBoundItems="true" OnTextChanged="Visitas_TextChanged" TabIndex="203">
                                                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:Label runat="server" ID="lbl_id_estado_vias_perimetrales" class="lbl2 w180">Estado vías perimetrales</asp:Label>
                                                                            <asp:DropDownList runat="server" ID="ddlb_id_estado_vias_perimetrales" CssClass="txt2 w150" AppendDataBoundItems="true" OnTextChanged="Visitas_TextChanged" TabIndex="204">
                                                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:Label runat="server" ID="lbl_tiene_servidumbres" class="lbl2 w150">Tiene servidumbres</asp:Label>
                                                                            <asp:CheckBox runat="server" ID="chk_tiene_servidumbres" CssClass="chk3" Width="200" OnCheckedChanged="Visitas_TextChanged" TabIndex="205" Text="&nbsp;" />
                                                                            <br />

                                                                            <asp:Label runat="server" ID="lbl_construccion_existente_lote" class="lbl2 w180">Construcción existente lote</asp:Label>
                                                                            <asp:TextBox runat="server" ID="txt_construccion_existente_lote" CssClass="txt2 w720" OnTextChanged="Visitas_TextChanged" TabIndex="211"></asp:TextBox>
                                                                            <asp:Label runat="server" ID="lbl_id_estado_construccion_existente_lote" class="lbl2 w60">Estado</asp:Label>
                                                                            <asp:DropDownList runat="server" ID="ddlb_id_estado_construccion_existente_lote" CssClass="txt2 w150" AppendDataBoundItems="true" OnTextChanged="Visitas_TextChanged" TabIndex="212">
                                                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <br />

                                                                            <asp:Label runat="server" ID="lbl_uso_construccion_lote" class="lbl2 w180">Uso construcción lote</asp:Label>
                                                                            <asp:TextBox runat="server" ID="txt_uso_construccion_lote" CssClass="txt2 w950" OnTextChanged="Visitas_TextChanged" TabIndex="213"></asp:TextBox>
                                                                            <br />

                                                                            <asp:Label runat="server" ID="lbl_construccion_existente_entorno" class="lbl2 w205">Construcción existente entorno</asp:Label>
                                                                            <asp:TextBox runat="server" ID="txt_construccion_existente_entorno" CssClass="txt2 w930" OnTextChanged="Visitas_TextChanged" TabIndex="214"></asp:TextBox>
                                                                            <br />

                                                                            <asp:Label runat="server" ID="lbl_estado_consolidacion_entorno" class="lbl2 w200">Estado consolidación entorno</asp:Label>
                                                                            <asp:TextBox runat="server" ID="txt_estado_consolidacion_entorno" CssClass="txt2 w930" OnTextChanged="Visitas_TextChanged" TabIndex="215"></asp:TextBox>
                                                                            <br />

                                                                            <asp:Label runat="server" ID="lblExisteViviendaEntorno" class="lbl2 w200">Existe vivienda en el entorno</asp:Label>
                                                                            <asp:CheckBox runat="server" ID="chk_existe_vivienda_entorno" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="221" Text="&nbsp;" />
                                                                            <asp:Label runat="server" ID="lbl_obs_vivienda_entorno" class="lbl2 w200">Observación vivienda entorno</asp:Label>
                                                                            <asp:TextBox runat="server" ID="txt_obs_vivienda_entorno" CssClass="txt2 w690" OnTextChanged="Visitas_TextChanged" TabIndex="222"></asp:TextBox>
                                                                            <br />

                                                                            <asp:Label runat="server" ID="lblExisteViviendaLote" class="lbl2 w200">Existe vivienda dentro del lote</asp:Label>
                                                                            <asp:CheckBox runat="server" ID="chk_existe_vivienda_lote" CssClass="chk3" OnCheckedChanged="Visitas_TextChanged" TabIndex="223" Text="&nbsp;" />
                                                                            <asp:Label runat="server" ID="lbl_obs_vivienda_lote" class="lbl2 w200">Observación vivienda lote</asp:Label>
                                                                            <asp:TextBox runat="server" ID="txt_obs_vivienda_lote" CssClass="txt2 w690" OnTextChanged="Visitas_TextChanged" TabIndex="224"></asp:TextBox>
                                                                            <br />

                                                                            <asp:Label runat="server" ID="lbl_id_pendiente_topografia" class="lbl2 w150">Pendiente topografía</asp:Label>
                                                                            <asp:DropDownList runat="server" ID="ddlb_id_pendiente_topografia" CssClass="txt2 w150" AppendDataBoundItems="true" OnTextChanged="Visitas_TextChanged" TabIndex="225">
                                                                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                    </ajaxToolkit:TabPanel>
                                                                </ajaxToolkit:TabContainer>
                                                                <%--*******************************--%>
                                                                <%--********** fin t-b ************--%>
                                                                <%--*******************************--%>
                                                                <br />

                                                                <asp:Label runat="server" ID="lbl_nombre_urbanizacion_lote" class="lbl1">Nombre urbanización</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_nombre_urbanizacion_lote" CssClass="txt2 w500" OnTextChanged="Visitas_TextChanged" TabIndex="301"></asp:TextBox>
                                                                <br />

                                                                <asp:Label runat="server" ID="lbl_obs_visita" class="lbl1">Observación</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_obs_visita" CssClass="txt2 txtObs1" TextMode="MultiLine"
                                                                    onKeyDown="MaxLengthText(this,1000);" onKeyUp="MaxLengthText(this,1000);" OnTextChanged="Visitas_TextChanged" TabIndex="302"></asp:TextBox>

                                                                <asp:LinkButton ID="lbSubirFoto" runat="server" CssClass="btn btn-outline-success btn-xs btnLoad" Text="Cargar foto" CausesValidation="false" OnClientClick="verPrediosVisitasFotos();">
																<i class="fas fa-upload"></i>&nbspCargar imágenes
                                                                </asp:LinkButton>
                                                                <br />
                                                                <br />
                                                                <br />

                                                                <asp:ValidationSummary runat="server" ID="vsVisitas" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgVisitas" EnableClientScript="false" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Tipo Visita} " Display="Dynamic" ValidationGroup="vgVisitas" ControlToValidate="ddlb_id_tipo_visita" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Usuario Elabora} " Display="Dynamic" ValidationGroup="vgVisitas" ControlToValidate="ddlb_cod_usu_visita" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Fecha Visita} " Display="Dynamic" ValidationGroup="vgVisitas" ControlToValidate="txt_fecha_visita" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Estado Visita} " Display="Dynamic" ValidationGroup="vgVisitas" ControlToValidate="ddlb_id_estado_visita" />
                                                            </div>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnVisitasVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnVisitasVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnFirstVisitas" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnBackVisitas" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnNextVisitas" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLastVisitas" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnVisitasEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnVisitasAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnVisitasDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvVisitas" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <asp:UpdatePanel runat="server" ID="upVisitasFoot" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:HiddenField runat="server" ID="hfEvtGVVisitas" Value="" />
                                            <div class="divSPMessage" id="DivMsgVisitas"></div>
                                            <div class="divSPFooter">
                                                <asp:Panel class="divSPFooter1" ID="divVisitasNavegacion" runat="server">
                                                    <%--FOOTER--%>
                                                    <asp:LinkButton ID="btnFirstVisitas" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnVisitasNavegacion_Click">
													<i class="fas fa-angle-double-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnBackVisitas" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnVisitasNavegacion_Click">
													<i class="fas fa-angle-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnNextVisitas" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnVisitasNavegacion_Click">
													<i class="fas fa-angle-right"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLastVisitas" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnVisitasNavegacion_Click">
													<i class="fas fa-angle-double-right"></i>
                                                    </asp:LinkButton>
                                                </asp:Panel>

                                                <div class="divSPFooter2">
                                                    <asp:Label runat="server" ID="lblVisitasCuenta"></asp:Label>
                                                </div>

                                                <div class="divSPFooter3">
                                                    <asp:LinkButton ID="btnVisitasCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnVisitasCancelar_Click" OnClientClick="closeWindow();" CausesValidation="false">
													<i class="fas fa-times"></i>&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnVisitasAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" ValidationGroup="vgVisitas" OnClientClick="if(Page_ClientValidate('vgVisitas')) return ShowModalPopup('puVisitas');">
													<i class="fas fa-check"></i>&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>

                                                <div class="divSPFooter4" id="divVisitasAction">
                                                    <asp:LinkButton ID="btnVisitasAdd" runat="server" CssClass="btn4" Text="" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar" OnClick="btnVisitasAccion_Click">
													<i class="fas fa-plus"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnVisitasEdit" runat="server" CssClass="btn4" Text="" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar" OnClick="btnVisitasAccion_Click">
													<i class="fas fa-pencil-alt"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnVisitasDel" runat="server" CssClass="btn4" Text="" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar" OnClick="btnVisitasAccion_Click" OnClientClick="LimpiarMensajeWeb();">
													<i class="far fa-trash-alt"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnVerFotos" runat="server" CssClass="btn2" Text="" CausesValidation="false" ToolTip="Ver fotos" OnClientClick="verPrediosVisitasFotos();"> <%--OnClick="btnVerFotos_Click"--%>
													<i class="far fa-image"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnCrearDocVisita" runat="server" CssClass="btn2" Text="" CausesValidation="false" ToolTip="Crear documento" OnClick="btnCrearDocVisita_Click">
													<i class="far fa-file-word"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnCrearDocVisitaOld" runat="server" CssClass="btn2" Text="" CausesValidation="false" ToolTip="Crear documento - formato anterior" OnClick="btnCrearDocVisitaOld_Click">
													<i class="far fa-file-word"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>

                                            <!--MODAL POPUP-->
                                            <asp:LinkButton ID="lbDummyVisitas" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="mpeVisitas" runat="server" PopupControlID="pnlPopupVisitas" TargetControlID="lbDummyVisitas" CancelControlID="btnHideVisitas" BackgroundCssClass="modalBackground" BehaviorID="puVisitas">
                                            </ajaxToolkit:ModalPopupExtender>
                                            <asp:Panel ID="pnlPopupVisitas" runat="server" CssClass="modalPopup">
                                                <div class="puHeader">Confirmar acción</div>
                                                <div class="puBody">
                                                    <p>¿Está seguro de continuar con la acción solicitada?</p>
                                                    <asp:LinkButton ID="btnHideVisitas" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClientClick="return HideModalPopup('puVisitas');">
                                                    <i class="fas fa-times"></i>&nbsp&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConfirmarVisitas" runat="server" Text="" CssClass="btn btn-outline-primary btn-sm ml10" OnClick="btnConfirmar_Click">
                                                    <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>

                                            <!--MODAL POPUP-->
                                            <asp:LinkButton ID="lbDummyVerFoto" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="mpeVerFoto" runat="server" PopupControlID="pnlPopupVerFoto" TargetControlID="lbDummyVerFoto" CancelControlID="btnHideVerFoto" BackgroundCssClass="modalBackground" BehaviorID="puVerFoto">
                                            </ajaxToolkit:ModalPopupExtender>
                                            <asp:Panel ID="pnlPopupVerFoto" runat="server" CssClass="" Style="display: none">
                                                <div class="popupFoto">
                                                    <div class="btn btn-xs btnNavX">
                                                        <asp:Button ID="btnHideVerFoto" runat="server" Text="X" CssClass="bg-d col-gf" CausesValidation="false" />
                                                    </div>
                                                    <asp:LinkButton ID="btnBackFoto" runat="server" CssClass="btn btn-xs btn-secondary fl btnNav" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnFotoNavegacion_Click">
												    <span aria-hidden="true" class="glyphicon glyphicon-chevron-left mt160 col-p"></span>
                                                    </asp:LinkButton>
                                                    <asp:Image runat="server" ID="imgFoto" CssClass="imgFoto fl" />
                                                    <asp:LinkButton ID="btnNextFoto" runat="server" CssClass="btn btn-xs btn-secondary fr btnNav" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnFotoNavegacion_Click">
													<span aria-hidden="true" class="glyphicon glyphicon-chevron-right mt160 col-p"></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnVisitasEdit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnVisitasAdd" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnVisitasDel" EventName="Click" />
                                            <asp:PostBackTrigger ControlID="btnCrearDocVisita" />
                                            <asp:PostBackTrigger ControlID="btnCrearDocVisitaOld" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <%--********************************************************************************************************************************************************************************--%>
                        <%--******************************************************************************      LICENCIAS     ******************************************************************************--%>
                        <%--********************************************************************************************************************************************************************************--%>
                        <ajaxToolkit:TabPanel runat="server" ID="tpLicencias" HeaderText="Licencias">
                            <ContentTemplate>
                                <div class="divSP" onclick="HideMensajeCRUD();">
                                    <div class="divSPHeader">
                                        <asp:UpdatePanel runat="server" ID="upLicenciasBtnVistas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPHeaderTitle"></div>
                                                <div class="divSPHeaderButton">
                                                    <asp:LinkButton ID="btnLicenciasVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnLicenciasVista_Click">
													<i class="fas fa-th"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLicenciasVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnLicenciasVista_Click">
													<i class="fas fa-bars"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="">
                                        <asp:UpdatePanel runat="server" ID="upLicencias" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPGV">
                                                    <asp:MultiView runat="server" ID="mvLicencias" ActiveViewIndex="0" OnActiveViewChanged="mvLicencias_ActiveViewChanged">
                                                        <asp:View runat="server" ID="vLicenciasGrid">
                                                            <asp:GridView ID="gvLicencias" CssClass="gv" runat="server" AutoGenerateColumns="False" DataKeyNames="au_licencia" ShowHeaderWhenEmpty="true" OnSelectedIndexChanged="gvLicencias_SelectedIndexChanged"
                                                                OnRowDataBound="gvLicencias_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_licencia" Visible="false" />
                                                                    <asp:BoundField DataField="fuente_informacion" HeaderText="Fuente Información" />
                                                                    <asp:BoundField DataField="tipo_licencia" HeaderText="Tipo Licencia" />
                                                                    <asp:BoundField DataField="curador" HeaderText="Curaduría" />
                                                                    <asp:BoundField DataField="numero_licencia" HeaderText="Num. Licencia" />
                                                                    <asp:BoundField DataField="fecha_ejecutoria" HeaderText="Fec. Ejecutoria" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" />
                                                                    <asp:BoundField DataField="termino_vigencia_meses" HeaderText="Vigencia Meses" />
                                                                    <asp:BoundField DataField="nombre_proyecto" HeaderText="Nombre Proyecto" />
                                                                    <asp:BoundField DataField="plano_urbanistico_aprobado" HeaderText="Plano Urb. Aprobado" />
                                                                    <asp:BoundField DataField="area_bruta" HeaderText="Área Bruta" />
                                                                    <asp:BoundField DataField="area_neta" HeaderText="Área Neta" />
                                                                    <asp:BoundField DataField="area_util" HeaderText="Área Util" />
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </asp:View>

                                                        <asp:View runat="server" ID="vLicenciasDetalle">
                                                            <div class="divEdit1">
                                                                <asp:TextBox runat="server" ID="txt_au_licencia" Visible="false"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_id_fuente_informacion" CssClass="lbl2 w140">Fuente información</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_fuente_informacion" CssClass="txt2 w150" AppendDataBoundItems="true" OnTextChanged="Licencias_TextChanged" TabIndex="10">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="lbl_id_tipo_licencia" CssClass="lbl2">Tipo licencia</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_tipo_licencia" CssClass="txt2 w180" AppendDataBoundItems="true" TabIndex="20" OnSelectedIndexChanged="ddlb_id_tipo_licencia_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="lbl_curador" class="lbl2 w80">Curaduría</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_curador" CssClass="txtN w50" OnTextChanged="Licencias_TextChanged" TabIndex="30" TextMode="Number" MaxLength="1" onkeypress="return SoloEnteroRango(event, 1, 5);"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_numero_licencia" CssClass="lbl2 w120">Número licencia</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_numero_licencia" CssClass="txt2 w200" MaxLength="50" OnTextChanged="Licencias_TextChanged" TabIndex="40"></asp:TextBox>
                                                                <br />

                                                                <asp:Label runat="server" ID="lbl_fecha_ejecutoria" CssClass="lbl2 w120">Fecha ejecutoria</asp:Label>
                                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_ejecutoria" runat="server" TargetControlID="txt_fecha_ejecutoria" PopupButtonID="txt_fecha_ejecutoria" Format="yyyy-MM-dd" />
                                                                <asp:TextBox runat="server" ID="txt_fecha_ejecutoria" CssClass="txtF" TextMode="Date" OnTextChanged="Licencias_TextChanged" TabIndex="50"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_termino_vigencia_meses" CssClass="lbl2 w110">Vigencia meses</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_termino_vigencia_meses" CssClass="txtN" OnTextChanged="Licencias_TextChanged" TextMode="Number" TabIndex="60" MaxLength="3" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbl_nombre_proyecto" CssClass="lbl2 w120">Nombre proyecto</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_nombre_proyecto" CssClass="txt2 w500" OnTextChanged="Licencias_TextChanged" TabIndex="70"></asp:TextBox>
                                                                <br />

                                                                <!--Seccion Urbanismo-->
                                                                <div class="fl w370 mt0 ml15 bgc">
                                                                    <label class="lblT1 w340 ml15 mtb10">Urbanismo</label>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_plano_urbanistico_aprobado" CssClass="lbl2 w150" ToolTip="Plano urbanístico aprobado">Plano urb. aprobado</asp:Label>
                                                                    <asp:TextBox runat="server" ID="txt_plano_urbanistico_aprobado" CssClass="txt2 w180 " OnTextChanged="Licencias_TextChanged" TabIndex="80"></asp:TextBox>
                                                                    <br />

                                                                    <label class="lbl3 w80 ml70 mt10">Area (m2)</label>
                                                                    <label class="lbl3 w80 h35 lh16 ml120 mt10">Area cesión (m2)</label>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_area_bruta" CssClass="lbl2 w50">Bruta</asp:Label>
                                                                    <!-- Área Bruta -->
                                                                    <asp:TextBox runat="server" ID="txt_area_bruta" CssClass="txtN2" OnTextChanged="Licencias_TextChanged" TabIndex="81" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lbl_area_cesion_zonas_verdes" CssClass="lbl2">Zonas verdes</asp:Label>
                                                                    <!-- Área Cesión Zonas Verdes -->
                                                                    <asp:TextBox runat="server" ID="txt_area_cesion_zonas_verdes" CssClass="txtN2" OnTextChanged="Licencias_TextChanged" TabIndex="120" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_area_neta" CssClass="lbl2 w50">Neta</asp:Label>
                                                                    <!-- Área Neta -->
                                                                    <asp:TextBox runat="server" ID="txt_area_neta" CssClass="txtN2" OnTextChanged="Licencias_TextChanged" TabIndex="100" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lbl_area_cesion_vias" CssClass="lbl2">Vías</asp:Label>
                                                                    <!-- Área Cesión Vías -->
                                                                    <asp:TextBox runat="server" ID="txt_area_cesion_vias" CssClass="txtN2" OnTextChanged="Licencias_TextChanged" TabIndex="130" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_area_util" CssClass="lbl2 w50">Util</asp:Label>
                                                                    <!-- Área Util -->
                                                                    <asp:TextBox runat="server" ID="txt_area_util" CssClass="txtN2" OnTextChanged="Licencias_TextChanged" TabIndex="110" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lbl_area_cesion_eq_comunal" CssClass="lbl2" ToolTip="Equipamiento comunal">Eq. comunal</asp:Label>
                                                                    <!-- Área Cesión EQ Comunal -->
                                                                    <asp:TextBox runat="server" ID="txt_area_cesion_eq_comunal" CssClass="txtN2" OnTextChanged="Licencias_TextChanged" TabIndex="140" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_porcentaje_ejecucion_urbanismo" CssClass="lbl2 w90 mt15" ToolTip="Porcentaje de ejecución">% Ejecución</asp:Label>
                                                                    <!--	 -->
                                                                    <asp:TextBox runat="server" ID="txt_porcentaje_ejecucion_urbanismo" CssClass="txtN mt15" OnTextChanged="Licencias_TextChanged" TabIndex="145" TextMode="Number" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                    <br />
                                                                </div>
                                                                <!--Seccion Construcción-->
                                                                <div class="fl w750 mt0 bl ml15 bgc">
                                                                    <label class="lblT1 ml15 w700 mtb10">Construcción</label>
                                                                    <br />

                                                                    <asp:Label runat="server" ID="lbl_id_obligacion_VIS" CssClass="lbl2 w130">Obligación VIS</asp:Label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_id_obligacion_VIS" CssClass="txt2 w190" AppendDataBoundItems="true" OnTextChanged="Licencias_TextChanged" TabIndex="150">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:Label runat="server" ID="lbl_id_obligacion_VIP" CssClass="lbl2 w130">Obligación VIP</asp:Label>
                                                                    <asp:DropDownList runat="server" ID="ddlb_id_obligacion_VIP" CssClass="txt2 w200" AppendDataBoundItems="true" OnTextChanged="Licencias_TextChanged" TabIndex="160">
                                                                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <br />

                                                                    <div class="fl w480 mt0">
                                                                        <label class="lbl3 w80 h50 lh16 mt10 ml120">Area terreno (m2)</label>
                                                                        <label class="lbl3 w80 h50 lh16 mt10">Area construida (m2)</label>
                                                                        <asp:Label runat="server" CssClass="lbl3 w80 h35 lh16 mt10" ToolTip="Porcentaje de obligación">% Obligación</asp:Label>
                                                                        <label class="lbl3 w80 mt10">Unidades</label>
                                                                        <br />

                                                                        <asp:Label runat="server" ID="lbl_area_terreno_VIS" CssClass="lbl2 mb5">VIS</asp:Label>
                                                                        <asp:TextBox runat="server" ID="txt_area_terreno_VIS" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="170" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <asp:TextBox runat="server" ID="txt_area_construida_VIS" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="180" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <asp:TextBox runat="server" ID="txt_porcentaje_obligacion_VIS" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="190" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <asp:TextBox runat="server" ID="txt_unidades_vivienda_VIS" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="200" MaxLength="5" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                        <br />

                                                                        <asp:Label runat="server" ID="lbl_area_terreno_no_VIS" CssClass="lbl2 mb5">No VIS</asp:Label>
                                                                        <asp:TextBox runat="server" ID="txt_area_terreno_no_VIS" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="210" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <asp:TextBox runat="server" ID="txt_area_construida_no_VIS" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="220" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <asp:TextBox runat="server" ID="txt_unidades_vivienda_no_VIS" CssClass="txtN2 mb5 ml85" OnTextChanged="Licencias_TextChanged" TabIndex="230" MaxLength="5" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                        <br />

                                                                        <asp:Label runat="server" ID="lbl_area_terreno_VIP" CssClass="lbl2 mb5">VIP</asp:Label>
                                                                        <asp:TextBox runat="server" ID="txt_area_terreno_VIP" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="240" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <asp:TextBox runat="server" ID="txt_area_construida_VIP" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="250" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <asp:TextBox runat="server" ID="txt_porcentaje_obligacion_VIP" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="260" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <asp:TextBox runat="server" ID="txt_unidades_vivienda_VIP" CssClass="txtN2 mb5 w80" OnTextChanged="Licencias_TextChanged" TabIndex="270" MaxLength="5" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                        <br />

                                                                        <asp:Label runat="server" ID="lbl_area_comercio" CssClass="lbl2 mb5">Comercio</asp:Label>
                                                                        <!-- Área Comercio -->
                                                                        <asp:TextBox runat="server" ID="txt_area_comercio" CssClass="txtN2 mb5 ml85" OnTextChanged="Licencias_TextChanged" TabIndex="280" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <br />

                                                                        <asp:Label runat="server" ID="lbl_area_oficina" CssClass="lbl2 mb5">Oficina</asp:Label>
                                                                        <!-- Área Oficina -->
                                                                        <asp:TextBox runat="server" ID="txt_area_oficina" CssClass="txtN2 mb5 ml85" OnTextChanged="Licencias_TextChanged" TabIndex="290" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <br />

                                                                        <asp:Label runat="server" ID="lbl_area_institucional" CssClass="lbl2 mb5">Institucional</asp:Label>
                                                                        <!-- Área Institucional -->
                                                                        <asp:TextBox runat="server" ID="txt_area_institucional" CssClass="txtN2 mb5 ml85" OnTextChanged="Licencias_TextChanged" TabIndex="300" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <br />

                                                                        <asp:Label runat="server" ID="lbl_area_industria" CssClass="lbl2 mb5">Industria</asp:Label>
                                                                        <!-- Área Industria -->
                                                                        <asp:TextBox runat="server" ID="txt_area_industria" CssClass="txtN2 mb5 ml85" OnTextChanged="Licencias_TextChanged" TabIndex="310" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <br />
                                                                    </div>

                                                                    <div class="fl w240 mt0 bl">
                                                                        <label class="lbl4 mt10 ml15 w205">Areas Proyecto (m2)</label>
                                                                        <asp:Label runat="server" ID="lbl_area_lote" CssClass="lbl2 w120 mb5">Lote</asp:Label>
                                                                        <asp:TextBox runat="server" ID="txt_area_lote" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="320" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label runat="server" ID="lbl_area_sotano" CssClass="lbl2 w120 mb5">Sótano</asp:Label>
                                                                        <asp:TextBox runat="server" ID="txt_area_sotano" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="330" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label runat="server" ID="lbl_area_semisotano" CssClass="lbl2 w120 mb5">Semisótano</asp:Label>
                                                                        <asp:TextBox runat="server" ID="txt_area_semisotano" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="340" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label runat="server" ID="lbl_area_primer_piso" CssClass="lbl2 w120 mb5">Primer piso</asp:Label>
                                                                        <asp:TextBox runat="server" ID="txt_area_primer_piso" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="350" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label runat="server" ID="lbl_area_pisos_restantes" CssClass="lbl2 w120 mb5">Pisos restantes</asp:Label>
                                                                        <asp:TextBox runat="server" ID="txt_area_pisos_restantes" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="360" MaxLength="10" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label runat="server" ID="lbl_area_construida_total" CssClass="lbl2 lblDis w120">Construida total</asp:Label>
                                                                        <asp:TextBox runat="server" ID="txt_area_construida_total" CssClass="txtN2 mb5 txtDis" OnTextChanged="Licencias_TextChanged" TabIndex="370" MaxLength="365" Enabled="false" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label runat="server" ID="lbl_area_libre_primer_piso" CssClass="lbl2 lblDis w120 mb5">Libre primer piso</asp:Label>
                                                                        <asp:TextBox runat="server" ID="txt_area_libre_primer_piso" CssClass="txtN2 mb5 txtDis" OnTextChanged="Licencias_TextChanged" TabIndex="380" MaxLength="10" Enabled="false" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label runat="server" ID="lbl_porcentaje_ejecucion_construccion" CssClass="lbl2 w120 mb5" ToolTip="Porcentaje de ejecución">% Ejecución</asp:Label>
                                                                        <asp:TextBox runat="server" ID="txt_porcentaje_ejecucion_construccion" CssClass="txtN2 mb5" OnTextChanged="Licencias_TextChanged" TabIndex="390" TextMode="Number" MaxLength="5" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="divS h10"></div>
                                                                <asp:ValidationSummary runat="server" ID="vsLicencias" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgLicencias" EnableClientScript="false" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Fuente Información} " Display="Dynamic" ValidationGroup="vgLicencias" ControlToValidate="ddlb_id_fuente_informacion" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Tipo Licencia} " Display="Dynamic" ValidationGroup="vgLicencias" ControlToValidate="ddlb_id_tipo_licencia" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Número Licencia} " Display="Dynamic" ValidationGroup="vgLicencias" ControlToValidate="txt_numero_licencia" />
                                                                <asp:RangeValidator ID="rv_curador" runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Curaduría : El valor debe estar entre 1 y 6} " Display="Dynamic" ValidationGroup="vgLicencias" ControlToValidate="txt_curador" MinimumValue="1" MaximumValue="6" Type="Integer" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Fecha Ejecutoria} " Display="Dynamic" ValidationGroup="vgLicencias" ControlToValidate="txt_fecha_ejecutoria" />
                                                                <asp:RequiredFieldValidator ID="rfv_termino_vigencia_meses" runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Vigencia meses} " Display="Dynamic" ValidationGroup="vgLicencias" ControlToValidate="txt_termino_vigencia_meses" />
                                                                <asp:RangeValidator ID="rv_termino_vigencia_meses" runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Vigencia meses : El valor debe estar entre 1 y 100} " Display="Dynamic" ValidationGroup="vgLicencias" ControlToValidate="txt_termino_vigencia_meses" MinimumValue="1" MaximumValue="100" Type="Integer" />
                                                            </div>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLicenciasVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLicenciasVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnFirstLicencias" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnBackLicencias" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnNextLicencias" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLastLicencias" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLicenciasEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLicenciasAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLicenciasDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvLicencias" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlb_id_tipo_licencia" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <asp:UpdatePanel runat="server" ID="upLicenciasFoot" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:HiddenField runat="server" ID="hfEvtGVLicencias" Value="" />
                                            <div class="divSPMessage" id="DivMsgLicencias"></div>
                                            <div class="divSPFooter">
                                                <asp:Panel class="divSPFooter1" ID="divLicenciasNavegacion" runat="server">
                                                    <%--FOOTER--%>
                                                    <asp:LinkButton ID="btnFirstLicencias" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnLicenciasNavegacion_Click">
													<i class="fas fa-angle-double-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnBackLicencias" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnLicenciasNavegacion_Click">
													<i class="fas fa-angle-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnNextLicencias" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnLicenciasNavegacion_Click">
													<i class="fas fa-angle-right"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLastLicencias" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnLicenciasNavegacion_Click">
													<i class="fas fa-angle-double-right"></i>
                                                    </asp:LinkButton>
                                                </asp:Panel>

                                                <div class="divSPFooter2">
                                                    <asp:Label runat="server" ID="lblLicenciasCuenta"></asp:Label>
                                                </div>

                                                <div class="divSPFooter3">
                                                    <asp:LinkButton ID="btnLicenciasCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnLicenciasCancelar_Click" CausesValidation="false">
													<i class="fas fa-times"></i>&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLicenciasAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" ValidationGroup="vgLicencias" OnClientClick="if(Page_ClientValidate('vgLicencias')) return ShowModalPopup('puLicencias');">
													<i class="fas fa-check"></i>&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>

                                                <div class="divSPFooter4" id="divLicenciasAction">
                                                    <asp:LinkButton ID="btnLicenciasAdd" runat="server" CssClass="btn4" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnLicenciasAccion_Click">
													<i class="fas fa-plus"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLicenciasEdit" runat="server" CssClass="btn4" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnLicenciasAccion_Click">
													<i class="fas fa-pencil-alt"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLicenciasDel" runat="server" CssClass="btn4" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnLicenciasAccion_Click" OnClientClick="LimpiarMensajeWeb();">
													<i class="far fa-trash-alt"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>

                                            <!--MODAL POPUP-->
                                            <asp:LinkButton ID="lbDummyLicencias" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="mpeLicencias" runat="server" PopupControlID="pnlPopupLicencias" TargetControlID="lbDummyLicencias" CancelControlID="btnHideLicencias" BackgroundCssClass="modalBackground" BehaviorID="puLicencias"></ajaxToolkit:ModalPopupExtender>
                                            <asp:Panel ID="pnlPopupLicencias" runat="server" CssClass="modalPopup" Style="display: none">
                                                <div class="puHeader">Confirmar acción</div>
                                                <div class="puBody">
                                                    <p>¿Está seguro de continuar con la acción solicitada?</p>
                                                    <asp:LinkButton ID="btnHideLicencias" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClientClick="return HideModalPopup('puLicencias');">
                                                    <i class="fas fa-times"></i>&nbsp&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConfirmarLicencias" runat="server" Text="" CssClass="btn btn-outline-primary btn-sm ml10" OnClick="btnConfirmar_Click">
                                                    <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnLicenciasEdit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnLicenciasAdd" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnLicenciasDel" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <%--********************************************************************************************************************************************************************************--%>
                        <%--******************************************************************************      CONCEPTOS     ******************************************************************************--%>
                        <%--********************************************************************************************************************************************************************************--%>
                        <ajaxToolkit:TabPanel runat="server" ID="tpConceptos" HeaderText="Conceptos">
                            <ContentTemplate>
                                <div class="divSP" onclick="HideMensajeCRUD();">
                                    <div class="divSPHeader">
                                        <asp:UpdatePanel runat="server" ID="upConceptosBtnVistas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPHeaderTitle"></div>
                                                <div class="divSPHeaderButton">
                                                    <asp:LinkButton ID="btnConceptosVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnConceptosVista_Click">
													<i class="fas fa-th"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConceptosVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnConceptosVista_Click">
													<i class="fas fa-bars"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="">
                                        <asp:UpdatePanel runat="server" ID="upConceptos" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPGV">
                                                    <asp:MultiView runat="server" ID="mvConceptos" ActiveViewIndex="0" OnActiveViewChanged="mvConceptos_ActiveViewChanged">
                                                        <asp:View runat="server" ID="vConceptosGrid">
                                                            <asp:GridView ID="gvConceptos" CssClass="gv" runat="server" AutoGenerateColumns="False" DataKeyNames="au_concepto" ShowHeaderWhenEmpty="true" OnSelectedIndexChanged="gvConceptos_SelectedIndexChanged"
                                                                OnRowDataBound="gvConceptos_RowDataBound" AllowSorting="true">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_concepto" Visible="false" />
                                                                    <asp:BoundField DataField="cod_predio_declarado" HeaderText="Cod Predio" Visible="false" />
                                                                    <asp:BoundField DataField="tipo_concepto" HeaderText="Tipo concepto" ItemStyle-CssClass="" />
                                                                    <asp:BoundField DataField="usu_concepto" HeaderText="Usuario elabora" ItemStyle-CssClass="" />
                                                                    <asp:BoundField DataField="fecha_concepto" SortExpression="fecha_concepto" HeaderText="Fecha elabora" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="estado_concepto" HeaderText="Estado concepto" ItemStyle-CssClass="" />
                                                                    <asp:BoundField DataField="usu_concepto_revisa" HeaderText="Usuario revisa" ItemStyle-CssClass="" />
                                                                    <asp:BoundField DataField="usu_concepto_aprueba" HeaderText="Usuario aprueba" ItemStyle-CssClass="" />
                                                                    <asp:BoundField DataField="usu_digita" HeaderText="Usuario digita" ItemStyle-CssClass="" />
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </asp:View>

                                                        <asp:View runat="server" ID="vConceptosDetalle">
                                                            <div class="divEdit1">
                                                                <asp:TextBox runat="server" ID="txt_au_concepto" Visible="false"></asp:TextBox>
                                                                <label class="lbl2 w130">Tipo concepto</label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_tipo_concepto" CssClass="txt2 w520 " AppendDataBoundItems="true" OnTextChanged="Conceptos_TextChanged" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlb_id_tipo_concepto_SelectedIndexChanged">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <label class="lbl1">Usuario elabora</label>
                                                                <asp:DropDownList runat="server" ID="ddlb_cod_usu_concepto" CssClass="txt2 w300 " AppendDataBoundItems="true" OnTextChanged="Conceptos_TextChanged" TabIndex="2">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />

                                                                <label class="lbl2 w130">Fecha elaboración</label>
                                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_concepto" runat="server" TargetControlID="txt_fecha_concepto" PopupButtonID="txt_fecha_concepto" Format="yyyy-MM-dd" />
                                                                <asp:TextBox runat="server" ID="txt_fecha_concepto" CssClass="txtF txtDis" TextMode="Date" onkeydown="return false;" onkeypress="return false;" onpaste="return false;" OnTextChanged="txt_fecha_concepto_TextChanged" AutoPostBack="true" TabIndex="3"></asp:TextBox>
                                                                <%--  --%>
                                                                <label class="lbl1">Estado concepto</label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_estado_concepto" CssClass="txt2 w250 " AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="Conceptos_TextChanged" TabIndex="4">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />

                                                                <%--*******************************--%>
                                                                <%--************* t-b *************--%>
                                                                <%--*******************************--%>
                                                                <ajaxToolkit:TabContainer runat="server" ID="TabContainerConceptos" Width="1150px" CssClass="ml15" ActiveTabIndex="0" OnActiveTabChanged="TabContainerConceptos_ActiveTabChanged" AutoPostBack="False">
                                                                    <ajaxToolkit:TabPanel runat="server" ID="tpConceptos1" HeaderText="General">
                                                                        <ContentTemplate>
                                                                            <label class="lbl2 ml5">Objeto</label>
                                                                            <asp:TextBox runat="server" ID="txt_objeto" CssClass="txt2 w1000 mw1000 h70 " OnTextChanged="Conceptos_TextChanged" TabIndex="11"
                                                                                TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);"></asp:TextBox>
                                                                            <br />
                                                                            <label class="lbl2 ml5">Antecedentes</label>
                                                                            <asp:TextBox runat="server" ID="txt_antecedentes" CssClass="txt2 w1000 mw1000 h200 " OnTextChanged="Conceptos_TextChanged" TabIndex="13"
                                                                                TextMode="MultiLine" onKeyDown="MaxLengthText(this,2000);" onKeyUp="MaxLengthText(this,2000);"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                    </ajaxToolkit:TabPanel>

                                                                    <ajaxToolkit:TabPanel runat="server" ID="tpConceptos2" HeaderText="Argumentos">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox runat="server" ID="txt_argumentos" CssClass="txt2 w1110 mw1110 h300 " OnTextChanged="Conceptos_TextChanged" TabIndex="15"
                                                                                TextMode="MultiLine" onKeyDown="MaxLengthText(this,4000);" onKeyUp="MaxLengthText(this,4000);"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                    </ajaxToolkit:TabPanel>

                                                                    <ajaxToolkit:TabPanel runat="server" ID="tpConceptos3" HeaderText="Soportes">
                                                                        <ContentTemplate>
                                                                            <div class="fl mr5">
                                                                                <asp:Label runat="server" ID="Label23" class="lbl2 w90 h35 lh16 ml0">Inf. técnico de visita</asp:Label>
                                                                                <asp:CheckBox runat="server" ID="chk_sd_1" CssClass="chk4" TabIndex="85" AutoPostBack="true" OnCheckedChanged="sd_Changed" Text="&nbsp;" />
                                                                                <ajaxToolkit:CalendarExtender ID="cal_sd_1_fecha" runat="server" TargetControlID="txt_sd_1_fecha" PopupButtonID="txt_sd_1_fecha" Format="yyyy-MM-dd" />
                                                                                <asp:TextBox runat="server" ID="txt_sd_1_fecha" CssClass="txtF vat" OnKeyPress="return false;" TextMode="Date" AutoPostBack="True" OnTextChanged="sd_Changed" OnClientCheck=""></asp:TextBox>
                                                                            </div>
                                                                            <div class="fl w865">
                                                                                <asp:TextBox runat="server" ID="txt_sd_1_t" CssClass="txt2 txtObs2" TextMode="MultiLine"
                                                                                    onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);" OnTextChanged="Conceptos_TextChanged" TabIndex="3"></asp:TextBox>
                                                                            </div>
                                                                            <br />

                                                                            <div class="fl mr5">
                                                                                <asp:Label runat="server" ID="Label13" class="lbl2 w90 h35 lh16 ml0">Certificado catastral</asp:Label>
                                                                                <asp:CheckBox runat="server" ID="chk_sd_2" CssClass="chk4" TabIndex="85" AutoPostBack="true" OnCheckedChanged="sd_Changed" Text="&nbsp;" />
                                                                                <ajaxToolkit:CalendarExtender ID="cal_sd_2_fecha" runat="server" TargetControlID="txt_sd_2_fecha" PopupButtonID="txt_sd_2_fecha" Format="yyyy-MM-dd" />
                                                                                <asp:TextBox runat="server" ID="txt_sd_2_fecha" CssClass="txtF vat" OnKeyPress="return false;" TextMode="Date" TabIndex="3" AutoPostBack="true" OnTextChanged="sd_Changed"></asp:TextBox>
                                                                                <asp:Label runat="server" ID="lbl_id_origen_certificado_catastral" CssClass="lbl2 w60 ml0">Origen</asp:Label>
                                                                                <asp:DropDownList runat="server" ID="ddlb_id_origen_certificado_catastral" CssClass="txt4 w70 vat" AppendDataBoundItems="true" OnTextChanged="Conceptos_TextChanged" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="sd_Changed">
                                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <div class="fl w725">
                                                                                <asp:TextBox runat="server" ID="txt_sd_2_t" CssClass="txt2 txtObs3" TextMode="MultiLine"
                                                                                    onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);" OnTextChanged="Conceptos_TextChanged" TabIndex="3"></asp:TextBox>
                                                                            </div>
                                                                            <br />

                                                                            <div class="fl mr5">
                                                                                <asp:Label runat="server" ID="Label17" class="lbl2 w90 h35 lh16 ml0">Matrícula inmobiliaria</asp:Label>
                                                                                <asp:CheckBox runat="server" ID="chk_sd_3" CssClass="chk4" TabIndex="85" AutoPostBack="true" OnCheckedChanged="sd_Changed" Text="&nbsp;" />
                                                                                <ajaxToolkit:CalendarExtender ID="cal_sd_3_fecha" runat="server" TargetControlID="txt_sd_3_fecha" PopupButtonID="txt_sd_3_fecha" Format="yyyy-MM-dd" />
                                                                                <asp:TextBox runat="server" ID="txt_sd_3_fecha" CssClass="txtF vat" OnKeyPress="return false;" TextMode="Date" TabIndex="3" AutoPostBack="true" OnTextChanged="sd_Changed"></asp:TextBox>
                                                                                <asp:Label runat="server" ID="lbl_id_origen_matricula" CssClass="lbl2 w60 ml0">Origen</asp:Label>
                                                                                <asp:DropDownList runat="server" ID="ddlb_id_origen_matricula" CssClass="txt2 w70 vat" AppendDataBoundItems="true" OnTextChanged="Conceptos_TextChanged" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="sd_Changed">
                                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <div class="fl w660">
                                                                                <asp:TextBox runat="server" ID="txt_sd_3_t" CssClass="txt2 txtObs3" TextMode="MultiLine"
                                                                                    onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);" OnTextChanged="Conceptos_TextChanged" TabIndex="3"></asp:TextBox>
                                                                            </div>
                                                                            <br />

                                                                            <div class="fl mr5">
                                                                                <asp:Label runat="server" ID="Label18" class="lbl2 w90 h35 lh16 ml0">Consulta licencias</asp:Label>
                                                                                <asp:CheckBox runat="server" ID="chk_sd_4" CssClass="chk4" TabIndex="85" AutoPostBack="true" OnCheckedChanged="sd_Changed" Text="&nbsp;" />
                                                                                <ajaxToolkit:CalendarExtender ID="cal_sd_4_fecha" runat="server" TargetControlID="txt_sd_4_fecha" PopupButtonID="txt_sd_4_fecha" Format="yyyy-MM-dd" />
                                                                                <asp:TextBox runat="server" ID="txt_sd_4_fecha" CssClass="txtF vat" OnKeyPress="return false;" TextMode="Date" TabIndex="3" AutoPostBack="true" OnTextChanged="sd_Changed"></asp:TextBox>
                                                                            </div>
                                                                            <div class="fl w865">
                                                                                <asp:TextBox runat="server" ID="txt_sd_4_t" CssClass="txt2 txtObs2" TextMode="MultiLine"
                                                                                    onKeyDown="MaxLengthText(this,1000);" onKeyUp="MaxLengthText(this,1000);" OnTextChanged="Conceptos_TextChanged" TabIndex="3"></asp:TextBox>
                                                                            </div>
                                                                            <br />

                                                                            <div class="fl mr5">
                                                                                <asp:Label runat="server" ID="Label30" class="lbl2 w90 ml0" ToolTip="Manual de procedimientos">Manual Pr.</asp:Label>
                                                                                <asp:CheckBox runat="server" ID="chk_sd_5" CssClass="chk4" TabIndex="85" AutoPostBack="true" OnCheckedChanged="sd_Changed" Text="&nbsp;" />
                                                                            </div>
                                                                            <div class="fl w1000">
                                                                                <asp:TextBox runat="server" ID="txt_sd_5_t" CssClass="txt2 txtObs" TextMode="MultiLine"
                                                                                    onKeyDown="MaxLengthText(this,1000);" onKeyUp="MaxLengthText(this,1000);" OnTextChanged="Conceptos_TextChanged" TabIndex="3"></asp:TextBox>
                                                                            </div>
                                                                            <br />

                                                                            <asp:Label runat="server" ID="Label8" class="lbl2 w90 ml0">Otros</asp:Label>
                                                                            <asp:TextBox runat="server" ID="txt_sd_otros" CssClass="txt2 txtObs" OnTextChanged="Conceptos_TextChanged" TabIndex="17"
                                                                                TextMode="MultiLine" onKeyDown="MaxLengthText(this,4000);" onKeyUp="MaxLengthText(this,4000);"></asp:TextBox>
                                                                            <asp:TextBox runat="server" ID="txt_soportes" CssClass="txt2 txtObs" OnTextChanged="Conceptos_TextChanged" TabIndex="17"
                                                                                TextMode="MultiLine" onKeyDown="MaxLengthText(this,2000);" onKeyUp="MaxLengthText(this,2000);" Visible="false"></asp:TextBox>
                                                                            <br />

                                                                        </ContentTemplate>
                                                                    </ajaxToolkit:TabPanel>

                                                                    <ajaxToolkit:TabPanel runat="server" ID="tpConceptos4" HeaderText="Consideraciones">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox runat="server" ID="txt_consideraciones" CssClass="txt2 w1110 mw1110 h300" OnTextChanged="Conceptos_TextChanged" TabIndex="19" TextMode="MultiLine"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                    </ajaxToolkit:TabPanel>

                                                                    <ajaxToolkit:TabPanel runat="server" ID="tpConceptos5" HeaderText="Conclusiones">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox runat="server" ID="txt_conclusiones" CssClass="txt2 w1110 mw1110 h300" OnTextChanged="Conceptos_TextChanged" TabIndex="21" TextMode="MultiLine"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                    </ajaxToolkit:TabPanel>
                                                                </ajaxToolkit:TabContainer>
                                                                <%--*******************************--%>
                                                                <%--********** fin t-b ************--%>
                                                                <%--*******************************--%>
                                                                <br />

                                                                <label class="lbl2">Observaciones</label>
                                                                <asp:TextBox runat="server" ID="txt_obs_concepto" CssClass="txt2 txtObs1" OnTextChanged="Conceptos_TextChanged" TabIndex="23"
                                                                    TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);" MaxLength="500"></asp:TextBox>
                                                                <br />

                                                                <label class="lbl2">Revisa</label>
                                                                <asp:DropDownList runat="server" ID="ddlb_cod_usu_concepto_revisa" CssClass="txt2 w300" OnTextChanged="Conceptos_TextChanged" AppendDataBoundItems="true" TabIndex="31">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <label class="lbl1">Aprueba</label>
                                                                <asp:DropDownList runat="server" ID="ddlb_cod_usu_concepto_aprueba" CssClass="txt2 w300" OnTextChanged="Conceptos_TextChanged" AppendDataBoundItems="true" TabIndex="33">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>

                                                                <asp:HiddenField runat="server" ID="hf_tipo_declaratoria" Value="" />
                                                                <asp:HiddenField runat="server" ID="hf_resolucion_declaratoria" Value="" />
                                                                <asp:HiddenField runat="server" ID="hf_ano_resolucion_declaratoria" Value="" />
                                                                <br />

                                                                <asp:ValidationSummary runat="server" ID="vsConceptos" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgConceptos" EnableClientScript="false" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Tipo Concepto} " Display="Dynamic" ValidationGroup="vgConceptos" ControlToValidate="ddlb_id_tipo_concepto" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Usuario Elabora} " Display="Dynamic" ValidationGroup="vgConceptos" ControlToValidate="ddlb_cod_usu_concepto" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Fecha Elaboración} " Display="Dynamic" ValidationGroup="vgConceptos" ControlToValidate="txt_fecha_concepto" />
                                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Estado Concepto} " Display="Dynamic" ValidationGroup="vgConceptos" ControlToValidate="ddlb_id_estado_concepto" />
                                                                <asp:RegularExpressionValidator ID="rev_obs_concepto" runat="server" ValidationGroup="vgConceptos" ControlToValidate="txt_obs_concepto" />
                                                                <asp:RangeValidator runat="server" ID="rv_fecha_concepto" ValidationGroup="vgConceptos" ControlToValidate="txt_fecha_concepto" />
                                                            </div>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnConceptosVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnConceptosVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnFirstConceptos" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnBackConceptos" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnNextConceptos" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLastConceptos" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnConceptosEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnConceptosAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnConceptosDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvConceptos" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <asp:UpdatePanel runat="server" ID="upConceptosFoot" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:HiddenField runat="server" ID="hfEvtGVConceptos" Value="" />
                                            <div class="divSPMessage" id="DivMsgConceptos"></div>
                                            <div class="divSPFooter">
                                                <asp:Panel class="divSPFooter1" ID="divConceptosNavegacion" runat="server">
                                                    <%--FOOTER--%>
                                                    <asp:LinkButton ID="btnFirstConceptos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnConceptosNavegacion_Click">
													<i class="fas fa-angle-double-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnBackConceptos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnConceptosNavegacion_Click">
													<i class="fas fa-angle-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnNextConceptos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnConceptosNavegacion_Click">
													<i class="fas fa-angle-right"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLastConceptos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnConceptosNavegacion_Click">
													<i class="fas fa-angle-double-right"></i>
                                                    </asp:LinkButton>
                                                </asp:Panel>

                                                <div class="divSPFooter2">
                                                    <asp:Label runat="server" ID="lblConceptosCuenta"></asp:Label>
                                                </div>

                                                <div class="divSPFooter3">
                                                    <asp:LinkButton ID="btnConceptosCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnConceptosCancelar_Click" CausesValidation="false">
													<i class="fas fa-times"></i>&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConceptosAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" ValidationGroup="vgConceptos" OnClientClick="if(Page_ClientValidate('vgConceptos')) return ShowModalPopup('puConceptos');">
													<i class="fas fa-check"></i>&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>

                                                <div class="divSPFooter4" id="divConceptosAction">
                                                    <asp:LinkButton ID="btnConceptosAdd" runat="server" CssClass="btn4" Text="" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnConceptosAccion_Click">
													<i class="fas fa-plus"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConceptosEdit" runat="server" CssClass="btn4" Text="" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnConceptosAccion_Click">
													<i class="fas fa-pencil-alt"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConceptosDel" runat="server" CssClass="btn4" Text="" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnConceptosAccion_Click" OnClientClick="LimpiarMensajeWeb();">
													<i class="far fa-trash-alt"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnCrearDocConcepto" runat="server" CssClass="btn2" Text="" CausesValidation="false" ToolTip="Crear documento" OnClick="btnCrearDocConcepto_Click">
													<i class="far fa-file-word"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>

                                            <!--MODAL POPUP-->
                                            <asp:LinkButton ID="lbDummyConceptos" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="mpeConceptos" runat="server" PopupControlID="pnlPopupConceptos" TargetControlID="lbDummyConceptos"
                                                CancelControlID="btnHideConceptos" BackgroundCssClass="modalBackground" BehaviorID="puConceptos">
                                            </ajaxToolkit:ModalPopupExtender>
                                            <asp:Panel ID="pnlPopupConceptos" runat="server" CssClass="modalPopup" Style="display: none">
                                                <div class="puHeader">Confirmar acción</div>
                                                <div class="puBody">
                                                    <p>¿Está seguro de continuar con la acción solicitada?</p>
                                                    <asp:LinkButton ID="btnHideConceptos" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClientClick="return HideModalPopup('puConceptos');">
                                                    <i class="fas fa-times"></i>&nbsp&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConfirmarConceptos" runat="server" Text="" CssClass="btn btn-outline-primary btn-sm ml10" OnClick="btnConfirmar_Click">
                                                    <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnConceptosEdit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnConceptosAdd" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnConceptosDel" EventName="Click" />
                                            <asp:PostBackTrigger ControlID="btnCrearDocConcepto" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <%--********************************************************************************************************************************************************************************--%>
                        <%--******************************************************************************      ACTOS ADM     ******************************************************************************--%>
                        <%--********************************************************************************************************************************************************************************--%>
                        <ajaxToolkit:TabPanel runat="server" ID="tpActAdmin" HeaderText="Actos Administrativos">
                            <ContentTemplate>
                                <div class="divSP" onclick="HideMensajeCRUD();">
                                    <div class="divSPHeader">
                                        <asp:UpdatePanel runat="server" ID="upActosAdmBtnVistas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPHeaderTitle"></div>
                                                <div class="divSPHeaderButton">
                                                    <asp:LinkButton ID="btnActosAdmVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnActosAdmVista_Click">
													<i class="fas fa-th"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnActosAdmVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnActosAdmVista_Click">
													<i class="fas fa-bars"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="">
                                        <asp:UpdatePanel runat="server" ID="upActosAdm" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="divSPGV">
                                                    <asp:MultiView runat="server" ID="mvActosAdm" ActiveViewIndex="0" OnActiveViewChanged="mvActosAdm_ActiveViewChanged">
                                                        <asp:View runat="server" ID="vActosAdmGrid">
                                                            <asp:GridView ID="gvActosAdm" CssClass="gv" runat="server" AutoGenerateColumns="False" DataKeyNames="au_acto_administrativo" ShowHeaderWhenEmpty="true" OnSelectedIndexChanged="gvActosAdm_SelectedIndexChanged"
                                                                OnRowDataBound="gvActosAdm_RowDataBound" OnRowCommand="gvActosAdm_RowCommand" AllowSorting="true">
                                                                <Columns>
                                                                    <asp:BoundField DataField="au_acto_administrativo" Visible="false" />
                                                                    <asp:BoundField DataField="tipo_acto" HeaderText="Tipo acto" />
                                                                    <asp:BoundField DataField="numero_acto" HeaderText="Número acto" />
                                                                    <asp:BoundField DataField="fecha_acto" SortExpression="fecha_acto" HeaderText="Fecha acto" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="w80 t-c" />
                                                                    <asp:BoundField DataField="estado_predio_declarado" HeaderText="Estado predio" />
                                                                    <asp:BoundField DataField="fecha_notificacion_acto" SortExpression="fecha_notificacion_acto" HeaderText="Fecha notificación" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="w80 t-c" />
                                                                    <asp:BoundField DataField="fecha_ejecutoria_acto" SortExpression="fecha_ejecutoria_acto" HeaderText="Fecha ejecutoria" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="w80 t-c" />
                                                                    <asp:BoundField DataField="causal_acto" HeaderText="Causal exclusión" />
                                                                    <asp:BoundField DataField="suspension_meses" HeaderText="Susp. meses" ItemStyle-CssClass="w50 t-c" />
                                                                    <asp:BoundField DataField="suspension_dias" HeaderText="Susp. días" ItemStyle-CssClass="w50 t-c" />
                                                                    <asp:BoundField DataField="fecha_vencimiento_acto" SortExpression="fecha_vencimiento_acto" HeaderText="Fecha vencimiento" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="w80 t-c" />
                                                                    <asp:BoundField DataField="obs_acto" HeaderText="Observación" />
                                                                    <asp:BoundField DataField="usu_digita" HeaderText="Usuario digita" />
                                                                    <asp:BoundField DataField="es_inicial" HeaderText="es_inicial" HeaderStyle-CssClass="d-n" ItemStyle-CssClass="d-n" />
                                                                    <asp:TemplateField ShowHeader="true" HeaderText="Doc" ItemStyle-CssClass="m0 w30">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton runat="server" CommandName="OpenDoc" CommandArgument='<%# Container.DisplayIndex %>' ImageUrl="~/images/icon/pdf-icon.png" Visible='<%# (string)Eval("tipo_acto") == "Resolución" ? true : false %>' ToolTip="Abrir documento" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="gvItemSelected" />
                                                                <HeaderStyle CssClass="gvHeader" />
                                                                <RowStyle CssClass="gvItem" />
                                                                <PagerStyle CssClass="gvPager" />
                                                            </asp:GridView>
                                                        </asp:View>

                                                        <asp:View runat="server" ID="vActosAdmDetalle">
                                                            <div class="divEdit1">
                                                                <asp:TextBox runat="server" ID="txt_au_acto_administrativo" Visible="false"></asp:TextBox>

                                                                <asp:Label runat="server" ID="lblidtipoacto" CssClass="lbl2">Tipo acto</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_tipo_acto" CssClass="txt2 w220" AppendDataBoundItems="true" OnTextChanged="ActosAdm_TextChanged" TabIndex="1">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>

                                                                <asp:Label runat="server" ID="lblnumeroacto" CssClass="lbl1">Número acto</asp:Label>
                                                                <asp:TextBox runat="server" ID="txt_numero_acto" CssClass="txt2 w150" OnTextChanged="ActosAdm_TextChanged" TabIndex="2"></asp:TextBox>

                                                                <asp:Label runat="server" ID="lblfechaacto" CssClass="lbl1">Fecha acto</asp:Label>
                                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_acto" runat="server" TargetControlID="txt_fecha_acto" PopupButtonID="txt_fecha_acto" Format="yyyy-MM-dd" />
                                                                <asp:TextBox runat="server" ID="txt_fecha_acto" CssClass="txtF" TextMode="Date" OnTextChanged="ActosAdm_TextChanged" TabIndex="3"></asp:TextBox>
                                                                <br />

                                                                <asp:Label runat="server" ID="lblidestadoprediodeclarado" CssClass="lbl2">Estado predio</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_estado_predio_declarado" CssClass="txt2 w220" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlb_id_estado_predio_declarado_SelectedIndexChanged" OnTextChanged="ActosAdm_TextChanged" TabIndex="4">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>

                                                                <div runat="server" id="divActosSuspension" class="d-i">
                                                                    <label class="lbl1">Tiempo de suspensión / interrupción</label>
                                                                    <label class="lbl1">Meses</label>
                                                                    <asp:TextBox runat="server" ID="txt_suspension_meses" CssClass="txtN w50" OnTextChanged="txt_suspension_meses_TextChanged" AutoPostBack="true" TabIndex="6" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                    <label class="lbl1">Días</label>
                                                                    <asp:TextBox runat="server" ID="txt_suspension_dias" CssClass="txtN w50" OnTextChanged="txt_suspension_meses_TextChanged" AutoPostBack="true" TabIndex="8" onkeypress="return SoloEntero(event);"></asp:TextBox>
                                                                </div>
                                                                <br />

                                                                <label class="lbl2 w130">Fecha notificación</label>
                                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_notificacion_acto" runat="server" TargetControlID="txt_fecha_notificacion_acto" PopupButtonID="txt_fecha_notificacion_acto" Format="yyyy-MM-dd" />
                                                                <asp:TextBox runat="server" ID="txt_fecha_notificacion_acto" CssClass="txtF" TextMode="Date" OnTextChanged="ActosAdm_TextChanged" TabIndex="20"></asp:TextBox>

                                                                <label class="lbl1">Fecha ejecutoria</label>
                                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_ejecutoria_acto" runat="server" TargetControlID="txt_fecha_ejecutoria_acto" PopupButtonID="txt_fecha_ejecutoria_acto" Format="yyyy-MM-dd" />
                                                                <asp:TextBox runat="server" ID="txt_fecha_ejecutoria_acto" CssClass="txtF" TextMode="Date" OnTextChanged="ActosAdm_TextChanged" TabIndex="30"></asp:TextBox>

                                                                <label class="lbl1 lblDis">Fecha vencimiento</label>
                                                                <ajaxToolkit:CalendarExtender ID="cal_fecha_vencimiento_acto" runat="server" TargetControlID="txt_fecha_vencimiento_acto" PopupButtonID="txt_fecha_vencimiento_acto" Format="yyyy-MM-dd" />
                                                                <asp:TextBox runat="server" ID="txt_fecha_vencimiento_acto" CssClass="txtF txtDis" TextMode="Date" OnTextChanged="ActosAdm_TextChanged" TabIndex="40"></asp:TextBox>
                                                                <br />

                                                                <asp:Label runat="server" ID="Label42" CssClass="lbl2 w130">Causal exclusión</asp:Label>
                                                                <asp:DropDownList runat="server" ID="ddlb_id_causal_acto" CssClass="txt2 w700" AppendDataBoundItems="true" OnTextChanged="ActosAdm_TextChanged" TabIndex="5">
                                                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />

                                                                <label class="lbl2">Observación</label>
                                                                <asp:TextBox runat="server" ID="txt_obs_acto" CssClass="txt2 txtObs1"
                                                                    TextMode="MultiLine" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);" OnTextChanged="ActosAdm_TextChanged" TabIndex="50"></asp:TextBox>
                                                                <br />

                                                                <asp:ValidationSummary runat="server" ID="vsActosAdm" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgActosAdm" EnableClientScript="false" />
                                                                <asp:RequiredFieldValidator ID="rfv_suspension_meses" runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Suspension meses} " Display="Dynamic" ValidationGroup="vgActosAdm" ControlToValidate="txt_suspension_meses" />
                                                                <asp:RequiredFieldValidator ID="rfv_suspension_dias" runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Suspension días} " Display="Dynamic" ValidationGroup="vgActosAdm" ControlToValidate="txt_suspension_dias" />
                                                                <asp:CompareValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {ND No es un estado permitido.} " Display="Dynamic" ControlToValidate="ddlb_id_estado_predio_declarado" ValueToCompare="43" ValidationGroup="vgActosAdm" Operator="NotEqual" Type="String"></asp:CompareValidator>
                                                                <asp:RangeValidator ID="rv_suspension_meses" runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Suspension meses : El valor debe estar entre 0 y 100} " Display="Dynamic" ValidationGroup="vgActosAdm" ControlToValidate="txt_suspension_meses" MinimumValue="0" MaximumValue="100" Type="Integer" />
                                                                <asp:RangeValidator ID="rv_suspension_dias" runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Suspension días : El valor debe estar entre 0 y 30} " Display="Dynamic" ValidationGroup="vgActosAdm" ControlToValidate="txt_suspension_dias" MinimumValue="0" MaximumValue="30" Type="Integer" />
                                                                <asp:RangeValidator ID="rv_fecha_acto" runat="server" ValidationGroup="vgActosAdm" ControlToValidate="txt_fecha_acto" />
                                                                <asp:RangeValidator ID="rv_fecha_notificacion_acto" runat="server" ValidationGroup="vgActosAdm" ControlToValidate="txt_fecha_notificacion_acto" />
                                                                <asp:RangeValidator ID="rv_fecha_ejecutoria_acto" runat="server" ValidationGroup="vgActosAdm" ControlToValidate="txt_fecha_ejecutoria_acto" />
                                                            </div>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnActosAdmVG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnActosAdmVD" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnFirstActosAdm" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnBackActosAdm" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnNextActosAdm" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLastActosAdm" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnActosAdmEdit" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnActosAdmAdd" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnActosAdmDel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvActosAdm" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <asp:UpdatePanel runat="server" ID="upActosAdmFoot" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:HiddenField runat="server" ID="hfEvtGVActosAdm" Value="" />
                                            <div class="divSPMessage" id="DivMsgActosAdm"></div>
                                            <div class="divSPFooter">
                                                <asp:Panel class="divSPFooter1" ID="divActosAdmNavegacion" runat="server">
                                                    <asp:LinkButton ID="btnFirstActosAdm" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnActosAdmNavegacion_Click">
													<i class="fas fa-angle-double-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnBackActosAdm" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnActosAdmNavegacion_Click">
													<i class="fas fa-angle-left"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnNextActosAdm" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnActosAdmNavegacion_Click">
													<i class="fas fa-angle-right"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnLastActosAdm" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnActosAdmNavegacion_Click">
													<i class="fas fa-angle-double-right"></i>
                                                    </asp:LinkButton>
                                                </asp:Panel>

                                                <div class="divSPFooter2">
                                                    <asp:Label runat="server" ID="lblActosAdmCuenta"></asp:Label>
                                                </div>

                                                <div class="divSPFooter3">
                                                    <asp:LinkButton ID="btnActosAdmCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnActosAdmCancelar_Click" CausesValidation="false">
													<i class="fas fa-times"></i>&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnActosAdmAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" ValidationGroup="vgActosAdm" OnClientClick="if(Page_ClientValidate('vgActosAdm')) return ShowModalPopup('puActosAdm');">
													<i class="fas fa-check"></i>&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>

                                                <div class="divSPFooter4" id="divActosAdmAction">
                                                    <asp:LinkButton ID="btnActosAdmAdd" runat="server" CssClass="btn4" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnActosAdmAccion_Click">
													<i class="fas fa-plus"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnActosAdmEdit" runat="server" CssClass="btn4" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnActosAdmAccion_Click">
													<i class="fas fa-pencil-alt"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnActosAdmDel" runat="server" CssClass="btn4" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnActosAdmAccion_Click" OnClientClick="LimpiarMensajeWeb();">
													<i class="far fa-trash-alt"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>

                                            <!--MODAL POPUP-->
                                            <asp:LinkButton ID="lbDummyActosAdm" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="mpeActosAdm" runat="server" PopupControlID="pnlPopupActosAdm" TargetControlID="lbDummyActosAdm"
                                                CancelControlID="btnHideActosAdm" BackgroundCssClass="modalBackground" BehaviorID="puActosAdm">
                                            </ajaxToolkit:ModalPopupExtender>
                                            <asp:Panel ID="pnlPopupActosAdm" runat="server" CssClass="modalPopup" Style="display: none">
                                                <div class="puHeader">Confirmar acción</div>
                                                <div class="puBody">
                                                    <p>¿Está seguro de continuar con la acción solicitada?</p>
                                                    <asp:LinkButton ID="btnHideActosAdm" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClientClick="return HideModalPopup('puActosAdm');">
                                                    <i class="fas fa-times"></i>&nbsp&nbspCancelar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnConfirmarActosAdm" runat="server" Text="" CssClass="btn btn-outline-primary btn-sm ml10" OnClick="btnConfirmar_Click">
                                                    <i class="fas fa-check"></i>&nbsp&nbspAceptar
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnActosAdmEdit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnActosAdmAdd" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnActosAdmDel" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <%--********************************************************************************************************************************************************************************--%>
                        <%--******************************************************************************      FICHA PREDIAL     ******************************************************************************--%>
                        <%--********************************************************************************************************************************************************************************--%>
                        <ajaxToolkit:TabPanel runat="server" ID="tpFichaPredial" HeaderText="Ficha Predial">
                            <ContentTemplate>
                                <div class="divSP" onclick="HideMensajeCRUD();">
                                    <div class="divSPMessageOK alert alert-warning">
                                        <i class="fas fa-exclamation-triangle"></i>&nbsp; Recuerde que esta información es histórica. Si requiere ver el estado actual de la información geográfica, por favor remitirse a la cartografía oficial vigente. 
                                    </div>
                                    <asp:UpdatePanel runat="server" ID="upPlanesPFileUpload" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="hdd_idactor" runat="server" Value="0" />
                                            <uc:FileUpload ID="ucFileUpload" runat="server" ControlID="PlanParcial.Documentos" Extensions="pdf" OnUserControlException="ucFileUpload_UserControlException"
                                                OnViewDoc="ucFileUpload_ViewDoc" Multiple="True" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <%--********************************************************************************************************************************************************************************--%>
                        <%--******************************************************************************      INTERESADOS     ******************************************************************************--%>
                        <%--********************************************************************************************************************************************************************************--%>
                        <ajaxToolkit:TabPanel runat="server" ID="tpInteresados" HeaderText="Datos contacto">
                            <ContentTemplate>
                                <div class="divSP" onclick="HideMensajeCRUD();">
                                    <asp:UpdatePanel runat="server" ID="upInteresados" UpdateMode="Always">
                                        <ContentTemplate>
                                            <uc:Interesado ID="ucInteresado" runat="server" ControlID="PrediosD.Interesados"/>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>


                        <%--********************************************************************************************************************************************************************************--%>
                        <%--******************************************************************************      CARTAS SEGUIMIENTOS     ******************************************************************************--%>
                        <%--********************************************************************************************************************************************************************************--%>
                        <ajaxToolkit:TabPanel runat="server" ID="tbCartas" HeaderText="Cartas Seguimientos">
                            <ContentTemplate>
                                <div class="divSP" onclick="HideMensajeCRUD();">
                                    <asp:UpdatePanel runat="server" ID="upCartas" UpdateMode="Always">
                                        <ContentTemplate>
                                            <uc:Carta ID="ucCarta" runat="server" ControlID="PrediosD.CartaSeguimiento"/>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                    </ajaxToolkit:TabContainer>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <%--********************************************************************************************************************************************************************************--%>
    <%--******************************************************************************       SCRIPTS      ******************************************************************************--%>
    <%--********************************************************************************************************************************************************************************--%>
    <script>
        function pageLoad() {
            //*****************************************LECTURA PARAMETROS CONFIGURACION
            var Ver = '<%=((NameValueCollection)ConfigurationManager.GetSection("PARAMETRIZACION/BotonesAccion"))["Documentos"].ToString()%>';
        if (Ver == "False") {
            if (document.getElementById('divDocumentosAction') != null)
                document.getElementById('divDocumentosAction').style.display = 'none';
        }

        var Ver = '<%=((NameValueCollection)ConfigurationManager.GetSection("PARAMETRIZACION/BotonesAccion"))["Prestamos"].ToString()%>';
        if (Ver == "False") {
            if (document.getElementById('divPrestamosAction') != null)
                document.getElementById('divPrestamosAction').style.display = 'none';
        }

        var Ver = '<%=((NameValueCollection)ConfigurationManager.GetSection("PARAMETRIZACION/BotonesAccion"))["PrediosPropietarios"].ToString()%>';
        if (Ver == "False") {
            if (document.getElementById('divPrediosPropietariosAction') != null)
                document.getElementById('divPrediosPropietariosAction').style.display = 'none';
        }

        var Ver = '<%=((NameValueCollection)ConfigurationManager.GetSection("PARAMETRIZACION/BotonesAccion"))["Propietarios"].ToString()%>';
        if (Ver == "False") {
            if (document.getElementById('divPropietariosAction') != null)
                document.getElementById('divPropietariosAction').style.display = 'none';
        }

        var Ver = '<%=((NameValueCollection)ConfigurationManager.GetSection("PARAMETRIZACION/BotonesAccion"))["Visitas"].ToString()%>';
        if (Ver == "False") {
            if (document.getElementById('divObservacionesAction') != null)
                document.getElementById('divObservacionesAction').style.display = 'none';
        }

        var Ver = '<%=((NameValueCollection)ConfigurationManager.GetSection("PARAMETRIZACION/BotonesAccion"))["Visitas"].ToString()%>';
        if (Ver == "False") {
            if (document.getElementById('divVisitasAction') != null)
                document.getElementById('divVisitasAction').style.display = 'none';
        }

        var Ver = '<%=((NameValueCollection)ConfigurationManager.GetSection("PARAMETRIZACION/BotonesAccion"))["Conceptos"].ToString()%>';
        if (Ver == "False") {
            if (document.getElementById('divConceptosAction') != null)
                document.getElementById('divConceptosAction').style.display = 'none';
        }

        var Ver = '<%=((NameValueCollection)ConfigurationManager.GetSection("PARAMETRIZACION/BotonesAccion"))["ActosAdm"].ToString()%>';
        if (Ver == "False") {
            if (document.getElementById('divActosAdmAction') != null)
                document.getElementById('divActosAdmAction').style.display = 'none';
        }

        //*****************************************ESTILO GRIDVIEWS
        $('#<%=gvPredios.ClientID%>').gridviewScroll({
            width: 1198,
            height: 200,
            railcolor: gvValores("railcolor"),
            barcolor: gvValores("barcolor"),
            barhovercolor: gvValores("barhovercolor"),
            bgcolor: gvValores("bgcolor"),
            varrowtopimg: gvValores("varrowtopimg"),
            varrowbottomimg: gvValores("varrowbottomimg"),
            harrowleftimg: gvValores("harrowleftimg"),
            harrowrightimg: gvValores("harrowrightimg"),
            freezesize: 2,
            arrowsize: 16,
            headerrowcount: 1,
            railsize: 16,
            barsize: 12,

            startVertical: $("#<%=hfGVPrediosSV.ClientID%>").val(),
            startHorizontal: $("#<%=hfGVPrediosSH.ClientID%>").val(),
            onScrollVertical: function (delta) {
                $("#<%=hfGVPrediosSV.ClientID%>").val(delta);
            },
            onScrollHorizontal: function (delta) {
                $("#<%=hfGVPrediosSH.ClientID%>").val(delta);
            }
        });

        $('#<%=gvPrediosDec.ClientID%>').gridviewScroll({
            width: 1198,
            height: 100,
            railcolor: gvValores("railcolor"),
            barcolor: gvValores("barcolor"),
            barhovercolor: gvValores("barhovercolor"),
            bgcolor: gvValores("bgcolor"),
            varrowtopimg: gvValores("varrowtopimg"),
            varrowbottomimg: gvValores("varrowbottomimg"),
            harrowleftimg: gvValores("harrowleftimg"),
            harrowrightimg: gvValores("harrowrightimg"),
            freezesize: 1,
            arrowsize: 16,
            headerrowcount: 1,
            railsize: 16,
            barsize: 12,

            startVertical: $("#<%=hfGVPrediosDecSV.ClientID%>").val(),
            startHorizontal: $("#<%=hfGVPrediosDecSH.ClientID%>").val(),
            onScrollVertical: function (delta) {
                $("#<%=hfGVPrediosDecSV.ClientID%>").val(delta);
            },
            onScrollHorizontal: function (delta) {
                $("#<%=hfGVPrediosDecSH.ClientID%>").val(delta);
            }
        });

        $('#<%=gvDocumentos.ClientID%>').gridviewScroll({
            width: 1180,
            height: 330,
            railcolor: gvValores("railcolor"),
            barcolor: gvValores("barcolor"),
            barhovercolor: gvValores("barhovercolor"),
            bgcolor: gvValores("bgcolor"),
            varrowtopimg: gvValores("varrowtopimg"),
            varrowbottomimg: gvValores("varrowbottomimg"),
            harrowleftimg: gvValores("harrowleftimg"),
            harrowrightimg: gvValores("harrowrightimg"),
            freezesize: 1,
            arrowsize: 16,
            headerrowcount: 1,
            railsize: 16,
            barsize: 12,

            startVertical: $("#<%=hfGVDocumentosSV.ClientID%>").val(),
            startHorizontal: $("#<%=hfGVDocumentosSH.ClientID%>").val(),
            onScrollVertical: function (delta) {
                $("#<%=hfGVDocumentosSV.ClientID%>").val(delta);
            },
            onScrollHorizontal: function (delta) {
                $("#<%=hfGVDocumentosSH.ClientID%>").val(delta);
            }
        });

        $('#<%=gvPrestamos.ClientID%>').gridviewScroll({
            width: 1180,
            height: 250,
            railcolor: gvValores("railcolor"),
            barcolor: gvValores("barcolor"),
            barhovercolor: gvValores("barhovercolor"),
            bgcolor: gvValores("bgcolor"),
            varrowtopimg: gvValores("varrowtopimg"),
            varrowbottomimg: gvValores("varrowbottomimg"),
            harrowleftimg: gvValores("harrowleftimg"),
            harrowrightimg: gvValores("harrowrightimg"),
            freezesize: 1,
            arrowsize: 16,
            headerrowcount: 1,
            railsize: 16,
            barsize: 12,

            startVertical: $("#<%=hfGVPrestamosSV.ClientID%>").val(),
            startHorizontal: $("#<%=hfGVPrestamosSH.ClientID%>").val(),
            onScrollVertical: function (delta) {
                $("#<%=hfGVPrestamosSV.ClientID%>").val(delta);
            },
            onScrollHorizontal: function (delta) {
                $("#<%=hfGVPrestamosSH.ClientID%>").val(delta);
            }
        });

        $('#<%=gvPrediosPropietarios.ClientID%>').gridviewScroll({
            width: 1180,
            height: 200,
            railcolor: gvValores("railcolor"),
            barcolor: gvValores("barcolor"),
            barhovercolor: gvValores("barhovercolor"),
            bgcolor: gvValores("bgcolor"),
            varrowtopimg: gvValores("varrowtopimg"),
            varrowbottomimg: gvValores("varrowbottomimg"),
            harrowleftimg: gvValores("harrowleftimg"),
            harrowrightimg: gvValores("harrowrightimg"),
            freezesize: 1,
            arrowsize: 16,
            headerrowcount: 1,
            railsize: 16,
            barsize: 12,

            startVertical: $("#<%=hfGVPrediosPropietariosSV.ClientID%>").val(),
            startHorizontal: $("#<%=hfGVPrediosPropietariosSH.ClientID%>").val(),
            onScrollVertical: function (delta) {
                $("#<%=hfGVPrediosPropietariosSV.ClientID%>").val(delta);
            },
            onScrollHorizontal: function (delta) {
                $("#<%=hfGVPrediosPropietariosSH.ClientID%>").val(delta);
            }
        });

        $('#<%=gvPropietarios.ClientID%>').gridviewScroll({
            width: 1180,
            height: 200,
            railcolor: gvValores("railcolor"),
            barcolor: gvValores("barcolor"),
            barhovercolor: gvValores("barhovercolor"),
            bgcolor: gvValores("bgcolor"),
            varrowtopimg: gvValores("varrowtopimg"),
            varrowbottomimg: gvValores("varrowbottomimg"),
            harrowleftimg: gvValores("harrowleftimg"),
            harrowrightimg: gvValores("harrowrightimg"),
            freezesize: 1,
            arrowsize: 16,
            headerrowcount: 1,
            railsize: 16,
            barsize: 12,

            startVertical: $("#<%=hfGVPropietariosSV.ClientID%>").val(),
            startHorizontal: $("#<%=hfGVPropietariosSH.ClientID%>").val(),
            onScrollVertical: function (delta) {
                $("#<%=hfGVPropietariosSV.ClientID%>").val(delta);
            },
            onScrollHorizontal: function (delta) {
                $("#<%=hfGVPropietariosSH.ClientID%>").val(delta);
            }
        });

        $('#<%=gvObservaciones.ClientID%>').gridviewScroll({
            width: 1180,
            height: 250,
            railcolor: gvValores("railcolor"),
            barcolor: gvValores("barcolor"),
            barhovercolor: gvValores("barhovercolor"),
            bgcolor: gvValores("bgcolor"),
            varrowtopimg: gvValores("varrowtopimg"),
            varrowbottomimg: gvValores("varrowbottomimg"),
            harrowleftimg: gvValores("harrowleftimg"),
            harrowrightimg: gvValores("harrowrightimg"),
            freezesize: 1,
            arrowsize: 16,
            headerrowcount: 1,
            railsize: 16,
            barsize: 12,

            startVertical: $("#<%=hfGVObservacionesSV.ClientID%>").val(),
            startHorizontal: $("#<%=hfGVObservacionesSH.ClientID%>").val(),
            onScrollVertical: function (delta) {
                $("#<%=hfGVObservacionesSV.ClientID%>").val(delta);
            },
            onScrollHorizontal: function (delta) {
                $("#<%=hfGVObservacionesSH.ClientID%>").val(delta);
            }
        });

        $('#<%=gvVisitas.ClientID%>').gridviewScroll({
            width: 1180,
            height: 560,
            railcolor: gvValores("railcolor"),
            barcolor: gvValores("barcolor"),
            barhovercolor: gvValores("barhovercolor"),
            bgcolor: gvValores("bgcolor"),
            varrowtopimg: gvValores("varrowtopimg"),
            varrowbottomimg: gvValores("varrowbottomimg"),
            harrowleftimg: gvValores("harrowleftimg"),
            harrowrightimg: gvValores("harrowrightimg"),
            freezesize: 1,
            arrowsize: 16,
            headerrowcount: 1,
            railsize: 16,
            barsize: 12,

            startVertical: $("#<%=hfGVVisitasSV.ClientID%>").val(),
            startHorizontal: $("#<%=hfGVVisitasSH.ClientID%>").val(),
            onScrollVertical: function (delta) {
                $("#<%=hfGVVisitasSV.ClientID%>").val(delta);
            },
            onScrollHorizontal: function (delta) {
                $("#<%=hfGVVisitasSH.ClientID%>").val(delta);
            }
        });

        $('#<%=gvLicencias.ClientID%>').gridviewScroll({
            width: 1180,
            height: 380,
            railcolor: gvValores("railcolor"),
            barcolor: gvValores("barcolor"),
            barhovercolor: gvValores("barhovercolor"),
            bgcolor: gvValores("bgcolor"),
            varrowtopimg: gvValores("varrowtopimg"),
            varrowbottomimg: gvValores("varrowbottomimg"),
            harrowleftimg: gvValores("harrowleftimg"),
            harrowrightimg: gvValores("harrowrightimg"),
            freezesize: 1,
            arrowsize: 16,
            headerrowcount: 1,
            railsize: 16,
            barsize: 12,

            startVertical: $("#<%=hfGVLicenciasSV.ClientID%>").val(),
            startHorizontal: $("#<%=hfGVLicenciasSH.ClientID%>").val(),
            onScrollVertical: function (delta) {
                $("#<%=hfGVLicenciasSV.ClientID%>").val(delta);
            },
            onScrollHorizontal: function (delta) {
                $("#<%=hfGVLicenciasSH.ClientID%>").val(delta);
            }
        });

        $('#<%=gvConceptos.ClientID%>').gridviewScroll({
            width: 1180,
            height: 570,
            railcolor: gvValores("railcolor"),
            barcolor: gvValores("barcolor"),
            barhovercolor: gvValores("barhovercolor"),
            bgcolor: gvValores("bgcolor"),
            varrowtopimg: gvValores("varrowtopimg"),
            varrowbottomimg: gvValores("varrowbottomimg"),
            harrowleftimg: gvValores("harrowleftimg"),
            harrowrightimg: gvValores("harrowrightimg"),
            freezesize: 1,
            arrowsize: 16,
            headerrowcount: 1,
            railsize: 16,
            barsize: 12,

            startVertical: $("#<%=hfGVConceptosSV.ClientID%>").val(),
            startHorizontal: $("#<%=hfGVConceptosSH.ClientID%>").val(),
            onScrollVertical: function (delta) {
                $("#<%=hfGVConceptosSV.ClientID%>").val(delta);
            },
            onScrollHorizontal: function (delta) {
                $("#<%=hfGVConceptosSH.ClientID%>").val(delta);
            }
        });

        $('#<%=gvActosAdm.ClientID%>').gridviewScroll({
            width: 1180,
            height: 330,
            railcolor: gvValores("railcolor"),
            barcolor: gvValores("barcolor"),
            barhovercolor: gvValores("barhovercolor"),
            bgcolor: gvValores("bgcolor"),
            varrowtopimg: gvValores("varrowtopimg"),
            varrowbottomimg: gvValores("varrowbottomimg"),
            harrowleftimg: gvValores("harrowleftimg"),
            harrowrightimg: gvValores("harrowrightimg"),
            freezesize: 1,
            arrowsize: 16,
            headerrowcount: 1,
            railsize: 16,
            barsize: 12,

            startVertical: $("#<%=hfGVActosAdmSV.ClientID%>").val(),
            startHorizontal: $("#<%=hfGVActosAdmSH.ClientID%>").val(),
            onScrollVertical: function (delta) {
                $("#<%=hfGVActosAdmSV.ClientID%>").val(delta);
            },
            onScrollHorizontal: function (delta) {
                $("#<%=hfGVActosAdmSH.ClientID%>").val(delta);
            }
        });

            //$(function () {
            //    $('input:text:first').focus();
            //    var $inp = $('.tabCtrl');
            //    $inp.bind('keydown', function (e) {
            //        var key = e.which;
            //        if (key == 13) {
            //            e.preventDefault();
            //            var nxtIdx = $inp.index(this) + 1;
            //            $(".tabCtrl:eq(" + nxtIdx + ")").focus();
            //        }
            //    });
            //});
        }

        function HideMensajeCRUD() {
            document.getElementById('DivMsgDocumentos').style.display = "none";
            document.getElementById('DivMsgPrestamos').style.display = "none";
            document.getElementById('DivMsgPrediosPropietarios').style.display = "none";
            document.getElementById('DivMsgPropietarios').style.display = "none";
            document.getElementById('DivMsgObservaciones').style.display = "none";
            document.getElementById('DivMsgVisitas').style.display = "none";
            document.getElementById('DivMsgLicencias').style.display = "none";
            document.getElementById('DivMsgConceptos').style.display = "none";
            document.getElementById('DivMsgActosAdm').style.display = "none";
        }

        function MensajeCRUD(mensaje, tipo, divMsg) {
            var pMensajeCRUD = document.getElementById(divMsg);
            pMensajeCRUD.innerHTML = mensaje;
            pMensajeCRUD.style.visibility = "visible";
            switch (tipo) {
                case 0: //info
                    pMensajeCRUD.className = "divSPMessageOK alert-info";
                    break;
                case 1: //success
                    pMensajeCRUD.className = "divSPMessageOK alert-success";
                    break;
                case 2: //warning
                    pMensajeCRUD.className = "divSPMessageOK alert-warning";
                    break;
                case 3: //danger
                    pMensajeCRUD.className = "divSPMessageOK alert-danger";
                    break;
                default: //Ocultar
                    pMensajeCRUD.className = "";
                    pMensajeCRUD.innerHTML = '';
                    pMensajeCRUD.style.visibility = "hidden";
                    break;
            }
        }

        function ShowModalPopup(objPanel) {
            $find(objPanel).show();
            return false;
        }

        function HideModalPopup(objPanel) {
            $find(objPanel).hide();
            return false;
        }

        var MaxWinParams = [
            'height=' + screen.height,
            'width=' + screen.width,
            'fullscreen=yes'
        ].join(',');

        function loadMapa(cx, cy) {
            var ruta = '<%=((NameValueCollection)ConfigurationManager.GetSection("PARAMETRIZACION/mapa"))["url"].ToString()%>' + cy + ',' + cx;
            var winMap = window.open(ruta, 'myMapwindow', MaxWinParams);
        }

        function verOpcFotos() {
            var FotoWin = window.open("fotos.aspx", "_blank", "menubar=0;toolbar=0,scrollbars=0,resizable=0,titlebar=0,width=1140,height=690");
            FotoWin.moveTo(0, 0);
        }

        function verPrediosVisitasFotos() {
            var width = 910;
            var height = 900;
            var left = (screen.width / 2) - (width / 2);
            var top = (screen.height / 2) - (height / 2);
            return window.open("PrediosVisitasFotos.aspx", "PrediosVisitasFotos", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + width + ', height=' + height + ', top=' + top + ', left=' + left);
        }

        function closeWindow() {
            window.close("PrediosVisitasFotos.aspx")
        }

    </script>

</asp:Content>
