<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" AutoEventWireup="true" 
    CodeBehind="Dashboard.aspx.cs" Inherits="SIDec.Dashboard" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/Indicador/Graphics.ascx" TagName="Graphics" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <link rel="stylesheet" href="./UserControls/Indicador/Indicador.css" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="card-body">
        <asp:UpdatePanel runat="server" ID="upDashboard" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlConsolidate" runat="server">
                    <div class="row">
                        <div class="form-group-sm col-12">
                            <ajax:CalendarExtender ID="calendar1" TargetControlID="txt_mes_visualizacion" Format="yyyy-MM" ClientIDMode="Static" runat="server"
                                DefaultView="Years" OnClientShown="onCalendarShown" OnClientHidden="onCalendarHidden" PopupButtonID="imgStart" />
                            <asp:TextBox runat="server" ID="txt_mes_visualizacion" CssClass="form-control form-control-xs" MaxLength="8" TextMode="Month" AutoPostBack="true" Width="130px" OnTextChanged="txt_mes_visualizacion_TextChanged" Visible="false" ></asp:TextBox>
                        </div>
                        
                        <div class="form-group-sm col-12 col-sm-6 col-xl-3">
                            <uc:Graphics ID="ind_1_1" runat="server" Code="1" Type="bar" Height="300" IndexAxis="y"/>
                        </div>
                        <div class="form-group-sm col-12 col-sm-6 col-xl-3">
                            <uc:Graphics ID="ind_1_2" runat="server"  Code="2" Type="bar" Height="300" />
                        </div>
                    </div>
                </asp:Panel>
                
                <asp:Panel ID="pnlLevel2" runat="server" Visible="false">
                    <div class="row">
                        <div class="form-group-sm col-12">                            
                            <uc:Graphics ID="ind_2" runat="server" Height="600" FontSize="12"/>
                        </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlDetail" Visible="false">
                    <div class="form-group-sm col-12">
                        <div class="row">
                            <div class="card-body" style="border: 1px solid gray">
                               <div class="row title_row">
                                <div class="title_graph">
                                    <asp:Label ID="lbl_ind" runat="server" />
                                </div>
                                <div class="btns_graph btn-group'">
                                    <asp:LinkButton ID="btnMinimize" runat="server" Text="Tabla" CausesValidation="false" OnClick="btnMinimize_Click" ToolTip="Reducir" CssClass="btn">
			                            <i class="fas fa-compress-arrows-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnExcel" runat="server" Text="Exportar Excel" CausesValidation="false" OnClick="btnExcel_Click" ToolTip="Exportar Excel" CssClass="btn">
				                        <i class="far fa-file-excel"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                                <div class="gv-w">
                                    <asp:GridView runat="server" ID="gvDetail" class="gv" AutoGenerateColumns="true" OnRowDataBound="gvDetail_RowDataBound">
                                        <SelectedRowStyle CssClass="gvItemSelected" />
                                        <HeaderStyle CssClass="gvHeader" />
                                        <RowStyle CssClass="gvItem" />
                                        <PagerStyle CssClass="gvPager" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
    <asp:HiddenField ID="hfgvDetalleSV" runat="server" />
    <asp:HiddenField ID="hfgvDetalleSH" runat="server" />

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="./UserControls/Indicador/Indicador.js"></script>

</asp:Content>
