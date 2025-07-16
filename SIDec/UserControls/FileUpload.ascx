<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.ascx.cs" Inherits="SIDec.UserControls.FileUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>

<asp:HiddenField ID="hddId" runat="server" Value="UserControlFileUpload" />
<asp:HiddenField ID="hddArchivoPrimary" runat="server" Value="0" />
<asp:HiddenField ID="hddReferenceID" runat="server" Value="UserControlREFFileUpload" />

<asp:UpdatePanel runat="server" ID="upMessages" UpdateMode="Always">
    <ContentTemplate>
        <uc:MessageBox ID="MessageBox1" runat="server" Width="100%" OnAccept="MessageBox_Accept" />
        <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    </ContentTemplate>
</asp:UpdatePanel>


<asp:UpdatePanel ID="upFileUpload" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel runat="server" ID="upFileUploadFoot" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divData" runat="server" class="dvRelative">
                    <div class="row">
                        <asp:UpdatePanel runat="server" ID="upFileUploadMsg" UpdateMode="Conditional" class="alert-main msgusercontrol">
                            <ContentTemplate>
                                <div runat="server" id="msgFileUpload" class="alert d-none" role="alert"></div>
                                <div runat="server" id="msgFileUploadMain" class="d-none" role="alert">
                                    <span runat="server" id="msgMainText"></span>
                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pFileUploadExecAction" runat="server">
                            <div class="col-auto card-header-buttons p0" style="padding-top: .75rem; padding-bottom: .25rem;">
                                <div class="btn-group flex-wrap">
                                    <asp:LinkButton ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" ValidationGroup="vgFileUpload" OnClick="btnAccept_Click">
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
                        <asp:Panel ID="pFileUploadAction" runat="server">
                            <div class="col-auto ml-auto card-header-buttons">
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnFileUploadList" runat="server" disabled="" CssClass="btn" Text="Tabla" CausesValidation="false" CommandName="Listar" CommandArgument="4" ToolTip="Listado" OnClick="btnFileUploadAccion_Click">
										                                            <i class="fas fa-th-list"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnFileUploadAdd" runat="server" disabled="" CssClass="btn" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnFileUploadAccion_Click">
										                                            <i class="fas fa-plus"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnFileUploadEdit" runat="server" CssClass="btn" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnFileUploadAccion_Click">
										                                            <i class="fas fa-pencil-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnFileUploadDel" runat="server" CssClass="btn" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnFileUploadAccion_Click">
										                                            <i class="far fa-trash-alt"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnFileUploadList" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnFileUploadAdd" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnFileUploadEdit" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnFileUploadDel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="gvArchivos" />
            </Triggers>
        </asp:UpdatePanel>



        <asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Panel ID="pnlDetail" runat="server" Visible="false">
                    <div class="card form-group bg-light mt-2">
                        <div class="card-body">
                            <div class="row">
                                <div class="form-group-sm col-12 col-md-4">
                                    <asp:HiddenField ID="hdd_idarchivo" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdd_ruta" runat="server" Value="" />
                                    <label class="lblBasic">Documento</label>
                                    <asp:RequiredFieldValidator ID="rfv_InfoFile" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic" ValidationGroup="vgFileUpload" ControlToValidate="txtInfo">
                                            <uc:ToolTip width="130px" ToolTip="Archivo requerido" runat="server"/>
                                    </asp:RequiredFieldValidator><br />
                                    <div style="float: left; min-width: 50px;">
                                        <asp:LinkButton ID="lbPdf" runat="server" CssClass="btn btn-danger btn-sm" Text="" CausesValidation="false" ToolTip="Ver documento" OnClick="lbPdf_Click">
						                        <i class="fas fa-file-pdf"></i>
                                        </asp:LinkButton>&nbsp;
                                            <asp:LinkButton ID="lbLoad" runat="server" CssClass="btn btn-success btn-sm" Text="" CausesValidation="false" ToolTip="Cargar archivo">
						                        <i class="fas fa-upload"></i>
                                            </asp:LinkButton>
                                    </div>
                                    <div class="labelLimited" style="padding-left: 10px; height: 20px">
                                        <asp:Label ID="lbl_nombre" runat="server" />
                                        <asp:TextBox ID="txtInfo" runat="server" Style="display: none" />
                                        <asp:Label ID="lblError" runat="server" CssClass="error" />
                                        <ajax:AsyncFileUpload ID="fuLoad" runat="server" Style="display: none" PersistFile="true" CompleteBackColor="Transparent" ErrorBackColor="Red" />
                                        <%-- OnUploadedComplete="FileUploadComplete"--%>
                                    </div>
                                </div>

                                <div class="form-group-sm col-12 col-md-5">
                                    <label class="lblBasic">Descripción</label>
                                    <asp:TextBox runat="server" ID="txt_descripcion" CssClass="form-control form-control-xs" Enabled="false" MaxLength="200"></asp:TextBox>
                                </div>
                                <div class="form-group-sm col-3 col-sm-4 col-md-5 col-lg-6 div_auditoria">
                                    <asp:Label ID="lbl_fec_auditoria_archivo" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>



        <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                    <div class="card-body">
                        <div class="row">
                            <div class="gv-w">
                                <asp:GridView ID="gvArchivos" CssClass="gv" runat="server" PageSize="500" AllowPaging="false" DataKeyNames="idarchivo,ruta" EmptyDataText="No hay registros asociados"
                                    AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" AllowSorting="true" OnRowCommand="gvArchivos_RowCommand" OnRowDataBound="gvArchivos_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                                        <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                                        <asp:BoundField DataField="fechacarga" HeaderText="Fecha carga" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-CssClass="t-c w120" />
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
                                                    <asp:LinkButton ID="btnPdf" runat="server" CssClass="btn btn-grid-danger" CommandName="_OpenFile" CommandArgument='<%# Container.DisplayIndex %>' ImageUrl="~/images/icon/pdf-icon.png" 
                                                        Visible='<%# (String.IsNullOrEmpty(Eval("ruta").ToString())) ? false : true %>' ToolTip="Abrir documento" />

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
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

    </ContentTemplate>
</asp:UpdatePanel>

