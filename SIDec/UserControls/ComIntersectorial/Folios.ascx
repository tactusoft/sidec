<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Folios.ascx.cs" Inherits="SIDec.UserControls.ComIntersectorial.Folios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/FileUpload.ascx" TagName="FileUpload" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>
    <script type="text/javascript" src="./UserControls/ComIntersectorial/ComIntersectorial.js"></script>

<asp:HiddenField ID="hddId" runat="server" Value="UserControlFolios" />

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>


<asp:UpdatePanel ID="upFolio" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:UpdatePanel runat="server" ID="upFolioFoot" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divData" runat="server">
                    <div class="row">
                        <asp:UpdatePanel runat="server" ID="upFolioMsg" UpdateMode="Conditional" class="alert-main msgusercontrol">
                            <ContentTemplate>
                                <div runat="server" id="msgFolio" class="alert d-none" role="alert"></div>
                                <div runat="server" id="msgFolioMain" class="d-none" role="alert">
                                    <span runat="server" id="msgMainText"></span>
                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <asp:ValidationSummary runat="server" ID="vsFolio" DisplayMode="SingleParagraph" CssClass="invalid-feedback" HeaderText="Falta informar: " ShowSummary="true" ShowValidationErrors="true" ValidationGroup="vgFolio" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pFolioExecAction" runat="server">
                            <div class="col-auto card-header-buttons" >
                                <div class="btn-group flex-wrap">
                                    <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgFolio" OnClick="btnAccept_Click">
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
                        <asp:Panel ID="pFolioAction" runat="server">
                            <div class="col-auto ml-auto card-header-buttons">
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnFolioList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnFolioAccion_Click">
							            <i class="fas fa-th-list"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnFolioAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnFolioAccion_Click">
								        <i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnFolioEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnFolioAccion_Click">
								        <i class="fas fa-pencil-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnFolioDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnFolioAccion_Click">
								        <i class="far fa-trash-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnFichaExcel" runat="server" CssClass="btn" Text="Ficha" CausesValidation="false" CommandName="Excel" CommandArgument="4" ToolTip="Generar ficha" OnClick="btnFichaExcel_Click">
									    <i class="far fa-file-excel"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnFolioList" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnFolioAdd" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnFolioEdit" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnFolioDel" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnFichaExcel" />
                <asp:AsyncPostBackTrigger ControlID="gvFolio" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:Panel ID="pnlGrid" runat="server">
             <div class="form-group-sm col-6 col-md-4 col-lg-3 col-xl-2">
                <asp:label ID="lblfec_radicacion" runat="server" class="lblBasic">Año</asp:label>
               <asp:DropDownList ID="ddlAnio" runat="server" OnSelectedIndexChanged="ddlAnio_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control form-control-xs"></asp:DropDownList>
            </div>
            <div class="gv-w">
                <asp:GridView ID="gvFolio" CssClass="gv" runat="server" PageSize="50" AllowPaging="true" DataKeyNames="idfolio, ruta_archivo" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
                    OnRowDataBound="gvFolio_RowDataBound" OnPageIndexChanging="gvFolio_PageIndexChanging" OnRowCommand="gvFolio_RowCommand" EmptyDataText="No se encontraron registros">
                    <Columns>
                        <asp:BoundField DataField="fecha_evento" HeaderText="Fecha" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-CssClass="t-c w120" />
                        <asp:BoundField DataField="carpeta" HeaderText="Carpeta" ItemStyle-CssClass="t-c" />
                        <asp:BoundField DataField="orden" HeaderText="Orden" ItemStyle-CssClass="t-c" />
                        <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="radicado" HeaderText="Radicado" />
                        <asp:BoundField DataField="fecha_radicado" HeaderText="Fec. radicado" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-CssClass="t-c" />
                        <asp:BoundField DataField="folio_inicial" HeaderText="Folio inicial" ItemStyle-CssClass="t-c" />
                        <asp:BoundField DataField="folio_final" HeaderText="Folio final" ItemStyle-CssClass="t-c" />
                        <asp:BoundField DataField="folios" HeaderText="Folios" ItemStyle-CssClass="t-c" />

                        <asp:TemplateField ShowHeader="true" ItemStyle-CssClass="t-c w40">
                            <%--<HeaderTemplate>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnFolioAdd" runat="server" CssClass="btn btn-grid-add" Text="Agregar" CausesValidation="false" CommandName="_Add" CommandArgument="-1" ToolTip="Agregar registro" OnClick="btnFolioAdd_Click">
										<i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                </div>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <div class="btn-group">
                                    <asp:ImageButton runat="server" CommandName="_OpenFile" CommandArgument='<%# Container.DisplayIndex %>' ImageUrl="~/images/icon/pdf-icon.png" CssClass="btn"
                                                                Visible='<%# (String.IsNullOrEmpty(Eval("ruta_archivo").ToString())) ? false : true %>' ToolTip="Abrir documento" />
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
                <asp:Panel ID="pnlFolio" runat="server" Width="100%">
                    <asp:HiddenField ID="hdd_idfolio" runat="server" Value="0" />
                    <div class="row first-row">

                        <div class="form-group-sm col-4 col-sm-3 col-lg-2">
                            <asp:Label ID="lblFechaEvento" runat="server" class="lblBasic" ToolTip="Fecha comité">Fecha evento</asp:Label>
                            <asp:CustomValidator ID="cv_fecha_evento" runat="server" ClientValidationFunction="DateClientValidate" Display="Dynamic" ValidateEmptyText="true" ControlToValidate="txt_fecha_evento">
                                <uc:ToolTip width="150px" ToolTip="Fecha con formato inválido" runat="server"/>
                            </asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="rfvfecha_evento" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                                ValidationGroup="vgFolio" ControlToValidate="txt_fecha_evento">
                                        <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                            <asp:RangeValidator runat="server" ID="rvfecha_evento" ValidationGroup="vgFolio" ControlToValidate="txt_fecha_evento" CssClass="invalid-feedback">
                                <uc:ToolTip width="130px" ToolTip="Fecha inválida" runat="server"/>
                            </asp:RangeValidator>
                            <ajax:CalendarExtender ID="cefecha_evento" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_evento" PopupButtonID="txt_fecha_evento" Format="yyyy-MM-dd" />
                            <asp:TextBox runat="server" ID="txt_fecha_evento" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8" onchange="onchangetime(1000)" ></asp:TextBox>
                            <asp:Button ID="btnfechaevento" runat="server" CssClass="hidden" OnClick="txt_fecha_evento_TextChanged"></asp:Button><%-- --%>
                        </div>
                        <div class="form-group-sm col-8 col-sm-9 col-lg-10">
                            <asp:Label ID="lblnombre" runat="server" class="lblBasic">Nombre</asp:Label>
                            <asp:RequiredFieldValidator ID="rfvnombre" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgFolio" ControlToValidate="txt_nombre">
                                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" ID="txt_nombre" CssClass="form-control form-control-xs" MaxLength="200"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group-sm col-12 col-lg-6">
                            <div class="row">
                                <div class="form-group-sm col-3">
                                    <asp:Label ID="lblcarpeta" runat="server" class="lblBasic">Carpeta</asp:Label>
                                    <asp:RequiredFieldValidator ID="rfvcarpeta" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                                        ValidationGroup="vgFolio" ControlToValidate="txt_carpeta">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                    </asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" ID="txt_carpeta" CssClass="form-control form-control-xs" MaxLength="15"></asp:TextBox>
                                </div>
                                <div class="form-group-sm col-3">
                                    <asp:Label ID="lblfolio_inicial" runat="server" class="lblBasic">Folio inicial</asp:Label>
                                    <asp:RequiredFieldValidator ID="rfvfolio_inicial" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                                        ValidationGroup="vgFolio" ControlToValidate="txt_folio_inicial">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                    </asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" ID="txt_folio_inicial" CssClass="form-control form-control-xs" onchange="totalFolios();"></asp:TextBox>
                                </div>
                                <div class="form-group-sm col-3">
                                    <asp:Label ID="lblfolios" runat="server" class="lblBasic">Número folios</asp:Label>
                                    <asp:RequiredFieldValidator ID="rfvfolios" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                                        ValidationGroup="vgFolio" ControlToValidate="txt_folios" InitialValue="0">
                                                <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                                    </asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" ID="txt_folios" CssClass="form-control form-control-xs" onchange="totalFolios();"></asp:TextBox>
                                </div>
                                <div class="form-group-sm col-3">
                                    <asp:Label ID="lblfolio_final" runat="server" class="lblBasic">Folio final</asp:Label>
                                    <asp:TextBox runat="server" ID="txt_folio_final" CssClass="form-control form-control-xs" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group-sm col-4 col-sm-3 col-lg-2">
                            <asp:Label ID="lbl_fecinicio" runat="server" class="lblBasic" ToolTip="Fecha de radicado">Fecha de radicado</asp:Label>
                            <asp:CustomValidator ID="cv_fecha_radicado" runat="server" ClientValidationFunction="DateClientValidate" Display="Dynamic" ValidateEmptyText="true" ControlToValidate="txt_fecha_radicado">
                                            <uc:ToolTip width="150px" ToolTip="Fecha con formato inválido" runat="server"/>
                            </asp:CustomValidator>
                            <asp:RangeValidator runat="server" ID="rvfecha_radicado" ValidationGroup="vgFolio" ControlToValidate="txt_fecha_radicado" CssClass="invalid-feedback">
                                                    <uc:ToolTip width="130px" ToolTip="Fecha inválida" runat="server"/>
                            </asp:RangeValidator>
                            <ajax:CalendarExtender ID="cefecha_radicado" runat="server" FirstDayOfWeek="Sunday" TargetControlID="txt_fecha_radicado" PopupButtonID="txt_fecha_radicado" Format="yyyy-MM-dd" />
                            <asp:TextBox runat="server" ID="txt_fecha_radicado" CssClass="form-control form-control-xs" TextMode="Date" MaxLength="8"></asp:TextBox>
                        </div>
                        <div class="form-group-sm col-5 col-sm-6 col-md-6 col-lg-2">
                            <asp:Label ID="lblradicado" runat="server" class="lblBasic">Radicado</asp:Label>
                            <asp:TextBox runat="server" ID="txt_radicado" CssClass="form-control form-control-xs" MaxLength="50"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbradicado" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                TargetControlID="txt_radicado" ValidChars="-/" FilterMode="ValidChars" />
                        </div>

                        <div class="form-group-sm col-3 col-lg-2">
                            <asp:label Id="lblarchivo" runat="server" class="lblBasic">Documento</asp:label>
                            <asp:RequiredFieldValidator ID="rfv_InfoFile" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgFolio" ControlToValidate="txt_ruta_archivo">
                                <uc:ToolTip width="130px" ToolTip="Archivo requerido" runat="server"/>
                            </asp:RequiredFieldValidator>
                            <br />
                            <div style="float: left; min-width: 50px;">
                                <asp:LinkButton ID="lblPdfProject" runat="server" CssClass="btn btn-danger btn-sm" Text="" CausesValidation="false" OnClick="lblPdfProject_Click" ToolTip="Ver documento">
						            <i class="fas fa-file-pdf"></i>
                                </asp:LinkButton>&nbsp;
                                <asp:LinkButton ID="lbLoadProject" runat="server" CssClass="btn btn-success btn-sm" Text="" CausesValidation="false" ToolTip="Cargar archivo">
						            <i class="fas fa-upload"></i>
                                </asp:LinkButton>
                            </div>
                            <div class="labelLimited" style="padding-left: 10px; height: 20px">
                                <asp:HiddenField ID="hdd_idarchivo" runat="server" Value="0"/>
                                <asp:HiddenField ID="hdd_ruta_archivo" runat="server" />
                                <asp:Label ID="lblInfoFileProject" runat="server" />
                                <asp:TextBox ID="txt_ruta_archivo" runat="server" Style="display: none" />
                                <asp:Label ID="lblErrorFileProject" runat="server" CssClass="error" />
                                <ajax:AsyncFileUpload ID="fuLoadProject" runat="server" onchange="infoFileProject();" Style="display: none" PersistFile="true"
                                    CompleteBackColor="Transparent" ErrorBackColor="Red" />
                            </div>
                        </div>

                        <div class="form-group-sm col-12">
                            <asp:Label ID="lblobservaciones" runat="server" class="lblBasic">Observaciones</asp:Label>
                            <asp:TextBox runat="server" ID="txt_observaciones" CssClass="form-control form-control-xs"
                                TextMode="MultiLine" MaxLength="2000" onKeyDown="MaxLengthText(this,500);" onKeyUp="MaxLengthText(this,500);"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
                <asp:UpdatePanel runat="server" ID="upAnexos" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlAnexos" runat="server" Visible="false" Width="100%">
                            <div class="col-12">
                                <div class="card-user-control subcontrol">
                                    <label class="section-title fwb">Anexos</label>
                                    <uc:FileUpload ID="ucAnexos" ControlID="fuAnexos" Extensions="jpg,png,jpeg,pdf,doc,docx,xls,xlsx,doc,docx,ppt,pptx" 
                                        Multiple="true" runat="server" OnUserControlException="ucAnexos_UserControlException" OnViewDoc="ucAnexos_ViewDoc" />
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
