<%@ Page Title="Recuperar Contraseña" Language="C#" MasterPageFile="~/Basic.Master" AutoEventWireup="true" CodeBehind="Recuperar.aspx.cs" Inherits="SIDec.Account.Recuperar" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="row" role="main">
    <div class="col-12">
			<h5>¿Olvidó su contraseña?</h5>
			<br />
			<p>Por favor envíe un correo electrónico a <a href="mailto:wilmer.alvarez@habitatbogota.gov.co">wilmer.alvarez@habitatbogota.gov.co</a> solicitando el restablecimiento de su contraseña.</p>
			<br />
			<p class="fr">
				<asp:HyperLink runat="server" ID="Regresar" CssClass="btn btn-outline-primary" ViewStateMode="Disabled">Iniciar sesión</asp:HyperLink>
			</p>
		</div>
	</div>
</asp:Content>
