<%@ Page Title="" Language="C#" MasterPageFile="~/Authentic.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="SIDec.Error" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">       
        <br /><br />
		<h3 style="color:red;">Se ha presentado un error</h3>
        <br /><br />
		
        <asp:HyperLink runat="server" ID="btnPreviousPage" CssClass="text-danger" NavigateUrl="~/Error.aspx">Volver</asp:HyperLink>
        
<%--        <asp:LinkButton ID="btnPreviousPage" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false" OnClick="btnPreviousPage_Click()">
            <i class="fas fa-times"></i>&nbsp&nbspCancelar
        </asp:LinkButton>--%>

</asp:Content>

