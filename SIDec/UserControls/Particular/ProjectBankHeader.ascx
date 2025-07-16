<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectBankHeader.ascx.cs" Inherits="SIDec.UserControls.Particular.ProjectBankHeader" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/SliderImages.ascx" TagName="SliderImages" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Particular/Actor.ascx" TagName="Actor" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ListBox.ascx" TagName="ListBox" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MultiSelectGrid.ascx" TagName="MultiSelectGrid" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>

<asp:HiddenField ID="hddIdBanco" runat="server" Value="0" />
<asp:HiddenField ID="hddEnabled" runat="server" Value="0" />
<asp:HiddenField ID="hddId" runat="server" Value="UserControlProjectBankHeader" />

<asp:UpdatePanel ID="upPBHeader" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlPBDetail" runat="server">
            <div class="row">
                <div class="col-sm-12 text-primary">
                    <label class="fs10 fwb">DATOS DEL PROYECTO</label>
                </div>
            </div>
            <div class="row">
                <div class="form-group-sm col-12 col-sm-5 col-md-4 col-xl-3">
                    <div class="row">
                        <div class="col-5 col-sm-4">
                            <div class="form-group-sm col">
                                <label class="lblBasic">ID</label>
                                <asp:TextBox runat="server" ID="txt_codigo" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-7 col-sm-8">
                            <div class="form-group-sm col">
                                <label class="lblBasic">Tipo de proyecto</label>
                                <asp:TextBox runat="server" ID="txt_tipo_proyecto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group-sm col-12 col-sm-7 col-md-8 col-xl-6">
                    <label class="lblBasic">Proyecto</label>
                    <asp:TextBox runat="server" ID="txt_nombre" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3">
                    <label class="lblBasic">Estado del proyecto</label>
                    <asp:TextBox runat="server" ID="txt_estado_proyecto" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3">
                    <label class="lblBasic">Tratamiento</label>
                    <asp:TextBox runat="server" ID="txt_tratamiento" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3">
                    <label class="lblBasic">Instrumento planificación</label>
                    <asp:TextBox runat="server" ID="txt_instrumento" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3" runat="server">
                    <label class="lblBasic">Localidad</label>
                    <asp:TextBox runat="server" ID="txt_Localidad" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3" runat="server">
                    <label class="lblBasic">UPZ</label>
                    <asp:TextBox runat="server" ID="txt_upz" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3">
                    <label class="lblBasic">Vinculación SDHT</label>
                    <asp:TextBox runat="server" ID="txt_vinculacion" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-6 col-md-3 col-lg-3 col-xl-2">
                    <label for="txt_fec_inicio_ventas" class="lblBasic">Inicio de ventas</label>
                    <asp:TextBox runat="server" ID="txt_fec_inicio_ventas" CssClass="form-control form-control-xs" TextMode="Date" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-6 col-md-3 col-lg-3 col-xl-2">
                    <label for="txt_fec_inicio_construccion" class="lblBasic">Inicio de construcción</label>
                    <asp:TextBox runat="server" ID="txt_fec_inicio_construccion" CssClass="form-control form-control-xs" TextMode="Date" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-xl-5">
                    <label for="txt_responsable" class="lblBasic">Responsable</label>
                    <asp:TextBox runat="server" ID="txt_responsable" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12">
                    <label class="lblBasic">Descripción del proyecto</label>
                    <asp:TextBox runat="server" ID="txt_descripcion" CssClass="form-control form-control-xs" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group-sm col-12" id="dvObservacionesPA" runat="server">
                    <label class="lblBasic">Observaciones del proyecto asociativo</label>
                    <asp:TextBox runat="server" ID="txt_observacion" CssClass="form-control form-control-xs" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                </div>
            </div>
            <div class="form-group-sm col-12 ">
                <div class="row">
                    <div class="col-12 text-primary">
                        <label class="fs10 fwb">INDICADORES DE IMPACTO</label>
                    </div>
                    <div class="row row-cols-md-5">
                        <div class="form-group-sm col-4">
                            <label class="lblBasic">Área bruta</label>
                            <div class="input-group input-group-xs">
                                <asp:TextBox runat="server" ID="txt_area_bruta" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                <div class="input-group-append"><span class="input-group-text">m<sup>2</sup></span></div>
                            </div>
                        </div>
                        <div class="form-group-sm col-4">
                            <label class="lblBasic">Área neta</label>
                            <div class="input-group input-group-xs">
                                <asp:TextBox runat="server" ID="txt_area_neta" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                <div class="input-group-append"><span class="input-group-text">m<sup>2</sup></span></div>
                            </div>
                        </div>
                        <div class="form-group-sm col-4">
                            <label class="lblBasic">Suelo útil</label>
                            <div class="input-group input-group-xs">
                                <asp:TextBox runat="server" ID="txt_area_suelo_util" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                <div class="input-group-append"><span class="input-group-text">m<sup>2</sup></span></div>
                            </div>
                        </div>
                        <div class="form-group-sm col-6">
                            <label class="lblBasic">Cesión tipo parque</label>
                            <div class="input-group input-group-xs">
                                <asp:TextBox runat="server" ID="txt_cesion_parque" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                <div class="input-group-append"><span class="input-group-text">m<sup>2</sup></span></div>
                            </div>
                        </div>
                        <div class="form-group-sm col-6">
                            <label class="lblBasic">Cesión equipamiento</label>
                            <div class="input-group input-group-xs">
                                <asp:TextBox runat="server" ID="txt_cesion_equipamiento" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                                <div class="input-group-append"><span class="input-group-text">m<sup>2</sup></span></div>
                            </div>
                        </div>
                        <div class="form-group-sm col-4">
                            <label class="lblBasic">Viviendas VIP</label>
                            <asp:TextBox runat="server" ID="txt_viviendas_vip" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col-4">
                            <label class="lblBasic">Viviendas VIS</label>
                            <asp:TextBox runat="server" ID="txt_viviendas_vis" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col-4">
                            <label class="lblBasic">Viviendas no VIS - VIP</label>
                            <asp:TextBox runat="server" ID="txt_viviendas_novip" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col-6">
                            <label class="lblBasic">Total de viviendas</label>
                            <asp:TextBox runat="server" ID="txt_totalviviendas" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col-6">
                            <label class="lblBasic">Población beneficiaria</label>
                            <asp:TextBox runat="server" ID="txt_poblacion_beneficiaria" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group-sm col-6">
                        <label class="lblBasic">Entidades</label>
                        <asp:TextBox runat="server" ID="txtEntidades" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="form-group-sm col-6">
                        <label class="lblBasic">Representantes</label>
                        <asp:TextBox runat="server" ID="txtRepresentante" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdd_idProyecto" runat="server" Value="0" />
        </asp:Panel>
        <asp:Panel ID="pnlPBSliderImages" runat="server" Width="100%">
            <div class="row row-cols-12 print_nobreak">
                <div class="form-group-sm col-12 col-sm-5 col-md-4 col-lg-3 b-g img-md" style="text-align: center">
                    <uc:SliderImages ID="siCarousel" runat="server" />
                </div>
                <div class="form-group-sm col-12 col-sm-7 col-md-8 col-lg-9">
                    <div style="position: relative;" class="gantt scroll2" id="GanttChartDIV" runat="server"></div>
                    <br />
                </div>
            </div>
        </asp:Panel>



        <asp:Panel ID="pnlPBEdition" runat="server">
            <div class="row">
                <div class="col-sm-12 text-primary">
                    <label class="fs10 fwb">DATOS DEL PROYECTO</label>
                </div>
            </div>
            <div class="row">
                <div class="form-group-sm col-12 col-sm-5 col-md-4 col-xl-3">
                    <div class="row">
                        <div class="form-group-sm col-5 col-sm-4">
                            <asp:HiddenField ID="hdd_idArchivo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdd_idactor" runat="server" Value="0" />
                            <label for="txte_codigo" class="lblBasic">ID</label>
                            <asp:TextBox runat="server" ID="txte_codigo" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col-7 col-sm-8">
                            <label for="ddl_idtipo_proyecto" class="lblBasic">Tipo de proyecto</label>
                            <asp:RequiredFieldValidator ID="rfv_idtipo_proyecto" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgFichaProyecto" ControlToValidate="ddl_idtipo_proyecto">
                                    <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                            <asp:DropDownList runat="server" ID="ddl_idtipo_proyecto" CssClass="form-control form-control-xs" AppendDataBoundItems="true" AutoPostBack="true">
                                <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group-sm col-12 col-sm-7 col-md-8 col-xl-6">
                    <label class="lblBasic">Proyecto</label>
                    <asp:RequiredFieldValidator ID="rfv_nombre" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgFichaProyecto" ControlToValidate="txte_nombre">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="txte_nombre" CssClass="form-control form-control-xs" Enabled="false" MaxLength="100"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbnombre" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" FilterMode="InvalidChars" InvalidChars="<>" TargetControlID="txte_nombre" />
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3">
                    <label for="ddl_idestado_proyecto" class="lblBasic">Estado proyecto</label>
                    <asp:RequiredFieldValidator ID="rfv_idestado_proyecto" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgFichaProyecto" ControlToValidate="ddl_idestado_proyecto">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                    </asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="ddl_idestado_proyecto" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3">
                    <label for="ddl_idtratamiento" class="lblBasic">Tratamiento</label>
                    <asp:DropDownList runat="server" ID="ddl_idtratamiento" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3">
                    <label for="ddl_idinstrumento" class="lblBasic">Instrumento planificación</label>
                    <asp:DropDownList runat="server" ID="ddl_idinstrumento" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3" runat="server">
                    <label for="ddl_idlocalidad" class="lblBasic">Localidad</label>
                    <asp:DropDownList runat="server" ID="ddl_idlocalidad" CssClass="form-control form-control-xs" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddl_idlocalidad_SelectedIndexChanged">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3" runat="server">
                    <label for="ddl_idupz" class="lblBasic">UPZ</label>
                    <asp:DropDownList runat="server" ID="ddl_idupz" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12 col-sm-6 col-md-4 col-xl-3">
                    <label for="ddl_idvinculacion" class="lblBasic">Vinculación SDHT</label>
                    <asp:DropDownList runat="server" ID="ddl_idvinculacion" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                        <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-6 col-md-3 col-lg-3 col-xl-2">
                    <label for="txte_fec_inicio_ventas" class="lblBasic">Inicio de ventas</label>
                    <asp:RegularExpressionValidator ID="rev_fec_inicio_ventas" runat="server" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txte_fec_inicio_ventas" Display="Dynamic">
                            <uc:ToolTip width="130px" ToolTip="Formato inválido" runat="server" />
                    </asp:RegularExpressionValidator>
                    <asp:RangeValidator runat="server" ID="rv_fec_inicio_ventas" ValidationGroup="vgFichaProyecto" ControlToValidate="txte_fec_inicio_ventas">
                            <uc:ToolTip width="130px" ToolTip="Fecha inválida" runat="server"/>
                    </asp:RangeValidator>
                    <ajax:CalendarExtender ID="ce_fec_inicio_ventas" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txte_fec_inicio_ventas" PopupButtonID="txte_fec_inicio_ventas" Format="yyyy-MM-dd" />
                    <asp:TextBox runat="server" ID="txte_fec_inicio_ventas" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8"></asp:TextBox>
                </div>
                <div class="form-group-sm col-6 col-md-3 col-lg-3 col-xl-2">
                    <label for="txte_fec_inicio_construccion" class="lblBasic">Inicio de construcción</label>
                    <asp:RegularExpressionValidator ID="rev_fec_inicio_construccion" runat="server" ValidationExpression="^(\d{4})([\-])(\d{2})([\-])(\d{2})$" ControlToValidate="txte_fec_inicio_construccion" Display="Dynamic" ValidationGroup="vgFichaProyecto">
                            <uc:ToolTip width="180px" ToolTip="Formato inválido" runat="server" />
                    </asp:RegularExpressionValidator>
                    <asp:RangeValidator runat="server" ID="rv_fec_inicio_construccion" ValidationGroup="vgFichaProyecto" ControlToValidate="txte_fec_inicio_construccion">
                            <uc:ToolTip width="130px" ToolTip="Rango de fecha inválido" runat="server"/>
                    </asp:RangeValidator>
                    <ajax:CalendarExtender ID="cefec_inicio_construccion" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txte_fec_inicio_construccion" PopupButtonID="txte_fec_inicio_construccion" Format="yyyy-MM-dd" />
                    <asp:TextBox runat="server" ID="txte_fec_inicio_construccion" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8"></asp:TextBox>
                </div>
                
                <div class="form-group-sm col-12 col-sm-6 col-xl-5">
                    <label for="ddl_responsable" class="lblBasic">Responsable</label>
                    <asp:DropDownList runat="server" ID="ddl_cod_usu_responsable" CssClass="form-control form-control-xs" AppendDataBoundItems="true">
                        <asp:ListItem Value="0">-- Seleccione opción</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group-sm col-12">
                    <label for="txte_descripcion" class="lblBasic">Descripción del proyecto</label>
                    <asp:TextBox runat="server" ID="txte_descripcion" CssClass="form-control form-control-xs" 
                        MaxLength="1000" TextMode="MultiLine" onKeyDown="MaxLengthText(this,1000);" onKeyUp="MaxLengthText(this,1000);" ></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbdescripcion" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" FilterMode="InvalidChars" InvalidChars="<>" TargetControlID="txte_descripcion" />
                </div>
            </div>
            <div class="form-group-sm col-12 ">
                <div class="row">
                    <div class="col-12 text-primary">
                        <label class="fs10 fwb">INDICADORES DE IMPACTO</label>
                    </div>

                    <div class="row row-cols-md-5">
                        <div class="form-group-sm col-4">
                            <label for="txte_area_bruta" class="lblBasic">Área bruta</label>
                            <div class="input-group input-group-xs">
                                <asp:TextBox runat="server" ID="txte_area_bruta" CssClass="form-control form-control-xs" MaxLength="20" onfocus="cleanValueZero(this)"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbarea_bruta" runat="server" FilterType="Numbers,Custom" ValidChars=".," TargetControlID="txte_area_bruta" />
                                <div class="input-group-append"><span class="input-group-text">m<sup>2</sup></span></div>
                            </div>
                        </div>
                        <div class="form-group-sm col-4">
                            <label for="txte_area_neta" class="lblBasic">Área neta</label>
                            <div class="input-group input-group-xs">
                                <asp:TextBox runat="server" ID="txte_area_neta" CssClass="form-control form-control-xs" MaxLength="20" onfocus="cleanValueZero(this)" onkeypress="return SoloDecimal(event);"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbarea_neta" runat="server" FilterType="Numbers,Custom" ValidChars=".," TargetControlID="txte_area_neta" />
                                <div class="input-group-append"><span class="input-group-text">m<sup>2</sup></span></div>
                            </div>
                        </div>
                        <div class="form-group-sm col-4">
                            <label for="txte_area_suelo_util" class="lblBasic">Suelo útil</label>
                            <div class="input-group input-group-xs">
                                <asp:TextBox runat="server" ID="txte_area_suelo_util" CssClass="form-control form-control-xs" MaxLength="20" onfocus="cleanValueZero(this)"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbarea_suelo_util" runat="server" FilterType="Numbers,Custom" ValidChars=".," TargetControlID="txte_area_suelo_util" />
                                <div class="input-group-append"><span class="input-group-text">m<sup>2</sup></span></div>
                            </div>
                        </div>
                        <div class="form-group-sm col-6">
                            <label for="txte_cesion_parque" class="lblBasic">Cesión tipo parque</label>
                            <div class="input-group input-group-xs">
                                <asp:TextBox runat="server" ID="txte_cesion_parque" CssClass="form-control form-control-xs" MaxLength="20" onfocus="cleanValueZero(this)"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbcesion_parque" runat="server" FilterType="Numbers,Custom" ValidChars=".," TargetControlID="txte_cesion_parque" />
                                <div class="input-group-append"><span class="input-group-text">m<sup>2</sup></span></div>
                            </div>
                        </div>
                        <div class="form-group-sm col-6">
                            <label for="txte_cesion_equipamiento" class="lblBasic">Cesión equipamiento</label>
                            <div class="input-group input-group-xs">
                                <asp:TextBox runat="server" ID="txte_cesion_equipamiento" CssClass="form-control form-control-xs" MaxLength="20" onfocus="cleanValueZero(this)"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbcesion_equipamiento" runat="server" FilterType="Numbers,Custom" ValidChars=".," TargetControlID="txte_cesion_equipamiento" />
                                <div class="input-group-append"><span class="input-group-text">m<sup>2</sup></span></div>
                            </div>
                        </div>
                        <div class="form-group-sm col-4">
                            <label for="txte_viviendas_vip" class="lblBasic">Viviendas VIP</label>
                            <asp:TextBox runat="server" ID="txte_viviendas_vip" CssClass="form-control form-control-xs" MaxLength="20" onfocus="cleanValueZero(this)"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbviviendas_vip" runat="server" FilterType="Numbers,Custom" ValidChars=".," TargetControlID="txte_viviendas_vip" />
                        </div>
                        <div class="form-group-sm col-4">
                            <label for="txte_viviendas_vis" class="lblBasic">Viviendas VIS</label>
                            <asp:TextBox runat="server" ID="txte_viviendas_vis" CssClass="form-control form-control-xs" MaxLength="20" onfocus="cleanValueZero(this)"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbviviendas_vis" runat="server" FilterType="Numbers,Custom" ValidChars=".," TargetControlID="txte_viviendas_vis" />
                        </div>
                        <div class="form-group-sm col-4">
                            <label for="txte_viviendas_novip" class="lblBasic">Viviendas no VIS - VIP</label>
                            <asp:TextBox runat="server" ID="txte_viviendas_novip" CssClass="form-control form-control-xs" MaxLength="20" onfocus="cleanValueZero(this)"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbviviendas_novip" runat="server" FilterType="Numbers,Custom" ValidChars=".," TargetControlID="txte_viviendas_novip" />
                        </div>
                        <div class="form-group-sm col-6">
                            <label for="txte_poblacion_beneficiaria" class="lblBasic">Población beneficiaria</label>
                            <asp:TextBox runat="server" ID="txte_poblacion_beneficiaria" CssClass="form-control form-control-xs" MaxLength="20" onfocus="cleanValueZero(this)"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbpoblacion_beneficiaria" runat="server" FilterType="Numbers,Custom" ValidChars=".," TargetControlID="txte_poblacion_beneficiaria" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group-sm col-12 ">
                <div class="row">
                    <div class="form-group-sm col-12 col-md-6">
                        <uc:ListBox ID="ddlEntidad" runat="server" Label="Entidad responsable" ControlID="ProjectBankHeaderEntidad" Enabled="true" />
                    </div>
                    <div class="form-group-sm col-12 col-md-6" id="dvRepresentante" runat="server" visible="false">
                        <label class="lblBasic">Representantes</label>
                        <asp:TextBox runat="server" ID="txteRepresentante" CssClass="form-control form-control-xs" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />


        <asp:Panel ID="pnlAnexos" runat="server">
            <div class="row">
                <div class="col-sm-12">
                    <ul class="nav nav-tabs nav-pills nav-justified" runat="server" id="ulPBSubHeader">
                        <li class="nav-item nav-item-p">
                            <asp:LinkButton ID="lblPBSubHeader_0" runat="server" CssClass="nav-link active" CommandArgument="0" CausesValidation="false" OnClick="btnPBSubHeader_Click">Imágenes</asp:LinkButton>
                        </li>
                        <li class="nav-item nav-item-p">
                            <asp:LinkButton ID="lblPBSubHeader_1" runat="server" CssClass="nav-link" CommandArgument="1" CausesValidation="false" OnClick="btnPBSubHeader_Click">Promotor / Desarrollador / Constructor</asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </div>

            <div style="padding: 10px; background-color: #fafafa;">
                <asp:MultiView ID="mvPBSubHeader" runat="server" ActiveViewIndex="0">
                    <%------------------------- Imágenes -------------------------%>
                    <asp:View ID="TabImages" runat="server">
                        <asp:Panel ID="pnlPBImageActions" runat="server" Width="100%">
                            <div runat="server" id="dvImageActions" class="ml0 mtb10 d-i">
                                <div class="row" style="background-color: #f2f2f2;">
                                    <div class="col-12 col-md-4">
                                        <div class="form-group-sm col">

                                            <asp:UpdatePanel ID="upImage" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:HiddenField ID="hdd_IdArchivoList" runat="server" Value="0" />
                                                    <label for="afuImage" class="lblBasic">Archivo</label>
                                                    <span id="ttNoImage" runat="server" visible="false">
                                                        <uc:Tooltip width="200px" ToolTip="Solo soporta imágenes de tipo jpg, gif y png" runat="server" />
                                                    </span>
                                                    <span id="ttFileRequired" runat="server" visible="false">
                                                        <uc:Tooltip width="250px" ToolTip="Debe seleccionar un archivo" runat="server" />
                                                    </span>
                                                    <asp:Label ID="lblPreviousImage" runat="server" Text="" CssClass="lblBasic fwb" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="afuImage" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <ajax:AsyncFileUpload ID="afuImage" runat="server" PersistFile="true" CompleteBackColor="Transparent" OnUploadedComplete="afuImage_UploadedComplete" ErrorBackColor="Red" />
                                        </div>
                                    </div>
                                    <div class="col-12 col-md-4">
                                        <div class="form-group-sm col">
                                            <label for="txt_DescripcionImagen" class="lblBasic">Descripción</label>
                                            <asp:TextBox runat="server" ID="txt_DescripcionImagen" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-12 col-md-4">
                                        <div class="form-group-sm col">
                                            <br>
                                            <br></br>
                                            <asp:LinkButton ID="btnAccept" runat="server" CausesValidation="true" CssClass="btn btn-outline-primary" OnClick="btnAccept_Click" Text="Aceptar" ValidationGroup="vgImagenCabecera">
                                                <i class="fas fa-check"></i>&nbsp;&nbsp;Aceptar
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CssClass="btn btn-outline-secondary" OnClick="btnCancel_Click">
                                                <i class="fas fa-times"></i>&nbsp;&nbsp;Cancelar
                                            </asp:LinkButton>
                                            </br>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />

                            <div class="row">
                                <div class="gv-w">
                                    <asp:GridView ID="gvPBImages" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="idarchivo,nombre,descripcion"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" AllowSorting="true" OnRowCommand="gvPBImages_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                            <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                                            <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                                                <HeaderTemplate>
                                                    <div class="btn-group">
                                                        <asp:LinkButton ID="btnPBImagesAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnPBImagesAdd_Click">
										                        <i class="fas fa-plus"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="btn-group">
                                                        <asp:LinkButton ID="btnPBImageEdit" runat="server" CssClass="btn btn-grid-edit" Text="Editar" CausesValidation="false" CommandName="_Edit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Editar registro"> 
										                        <i class="fas fa-pencil-alt"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnPBImageDelete" runat="server" CssClass="btn btn-grid-delete" Text="Eliminar" CausesValidation="false" CommandName="_Delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Eliminar registro"> 
										                        <i class="fas fa-trash-alt"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="gvItemSelected" />
                                        <HeaderStyle CssClass="gvHeaderSm" />
                                        <RowStyle CssClass="gvItem" />
                                        <PagerStyle CssClass="gvPager" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:Panel>
                    </asp:View>
                    <%------------------------- Responsables -------------------------%>
                    <asp:View ID="TabRepresentative" runat="server">
                        <%--<div class="card-user-control">--%>
                            <uc:Actor ID="ucRepresentative" runat="server" ReferenceTypeID="1" ControlID="ResponsableBanco" />
                        <%--</div>--%>
                    </asp:View>
                    <%------------------------- Entidades Responsables -------------------------%>
                    <%-- <asp:View ID="Tab3" runat="server">
                    </asp:View>--%>
                </asp:MultiView>
            </div>
        </asp:Panel>


    </ContentTemplate>
</asp:UpdatePanel>

