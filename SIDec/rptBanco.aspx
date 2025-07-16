<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" AutoEventWireup="true" CodeBehind="rptBanco.aspx.cs" Inherits="SIDec.rptBanco" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
            <uc:MessageBox ID="MessageInfo" runat="server" Width="100%" />
    <asp:UpdatePanel ID="upReport" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="">
                <div class="card-header bg-light text-primary">
                    <div class="card-header card-header-main">
                        <div class="row">
                            <div class="col-6 col-sm-7 col-lg-9 col-xl-10">
                                <h4>Reporte Banco de Proyectos</h4>
                            </div>
                            <div class="col-6 col-sm-5 col-lg-3 col-xl-2 ml-auto row">
                                <div class="col-5">
                                    <asp:CheckBoxList runat="server" ID="chk_sections" CssClass="fwb ml-auto noBorder" ToolTip="Lista: para ver la pestaña de «Proyectos Incorporados» ó Ficha: para ver el seguimiento de cada proyecto">
                                        <asp:ListItem Value="1" Selected="True">Lista</asp:ListItem>
                                        <asp:ListItem Value="2">Fichas</asp:ListItem>
                                    </asp:CheckBoxList>
                                </div>
                                <div class="col-7">
                                    <asp:UpdatePanel ID="pnlEjecutar" runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="btnExportar" runat="server" CssClass="btn btn-outline-success" Text="Exportar" CausesValidation="false" ToolTip="" OnClick="btnExportar_Click">
                                    <i class="far fa-file-excel"></i>&nbsp;&nbsp;Exportar
                                            </asp:LinkButton>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdateProgress ID="pnlLoading" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="pnlEjecutar">
                                        <ProgressTemplate>
                                            <asp:Image runat="server" CssClass="imgCargando" ImageUrl="~/images/icon/cargando.gif" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="mt-3">
                    <div class="card-body">
                        <div class="gv-w gv-scroll">
                            <asp:GridView ID="gvBanco" CssClass="gv" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                DataKeyNames="idbanco,activo" AllowPaging="false" style="min-width: 1200px; max-height: 500px;">
                                <Columns>
                                    <asp:BoundField DataField="Codigo" HeaderText="ID" />
                                    <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="estado_proyecto" HeaderText="Estado Proyecto" />
                                    <asp:BoundField DataField="localidad" HeaderText="Localidad" />
                                    <asp:BoundField DataField="area_bruta" HeaderText="Área bruta" DataFormatString="{0:N2}" ItemStyle-CssClass="w75 t-r" />
                                    <asp:BoundField DataField="area_neta" HeaderText="Área neta" DataFormatString="{0:N2}" ItemStyle-CssClass="w75 t-r" />
                                    <asp:BoundField DataField="suelo_util" HeaderText="Suelo útil" DataFormatString="{0:N2}" ItemStyle-CssClass="w75 t-r" />
                                    <asp:BoundField DataField="viviendas_vip" HeaderText="VIP" DataFormatString="{0:N0}" ItemStyle-CssClass="w50 t-r" />
                                    <asp:BoundField DataField="viviendas_vis" HeaderText="VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="w50 t-r" />
                                    <asp:BoundField DataField="viviendas_novip" HeaderText="No VIS" DataFormatString="{0:N0}" ItemStyle-CssClass="w50 t-r" />
                                    <asp:BoundField DataField="totalviviendas" HeaderText="Total viviendas" DataFormatString="{0:N0}" ItemStyle-CssClass="w50 t-r" />
                                    <asp:BoundField DataField="cesion_parque" HeaderText="Cesión parque" DataFormatString="{0:N2}" ItemStyle-CssClass="w75 t-r" />
                                    <asp:BoundField DataField="cesion_equipamiento" HeaderText="Cesion Equipamiento" DataFormatString="{0:N2}" ItemStyle-CssClass="w75 t-r" />

                                </Columns>
                                <SelectedRowStyle CssClass="gvItemSelected" />
                                <HeaderStyle CssClass="gvHeader" />
                                <RowStyle CssClass="gvItem" />
                                <PagerStyle CssClass="gvPager" />
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="card-footer mt-2">
                        <div class="row">
                            <div class="col-auto va-m mr-auto">
                                <asp:Label runat="server" ID="lbl_total_0" CssClass=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="MessageInfo" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
