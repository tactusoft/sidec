<%@ Page Title="" Language="C#" MasterPageFile="~/Basic.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SIDec.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <div class="row" role="main">
    <div class="col-12">
			<asp:MultiView runat="server" ID="mvLogin" ActiveViewIndex="0" EnableViewState="true">
				<asp:View runat="server" ID="vLogin">
					<div id="loginForm" class="">						
						<h4>Inicio de sesión</h4>
						<br />
            <div class="row row-cols-1">
							<div class="form-group col">
                <%--<label for="txtUserName" class="">Usuario</label>--%>
                <div class="input-group">
                  <div class="input-group-prepend">
                    <span class=" input-group-text bg-primary text-white border-0"><i class="fas fa-user"></i></span>
                  </div>
                  <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control" TabIndex="5" placeholder="Usuario"></asp:TextBox>
                </div>
								<asp:RequiredFieldValidator runat="server" ControlToValidate="txtUserName" Display="None" ValidationGroup="vgLogin" />
							</div>

							<div class="form-group col">
                <%--<label for="txtPassword" class="">Contraseña</label>--%>
                <div class="input-group">
                  <div class="input-group-prepend">
                    <span class="input-group-text bg-primary text-white border-0"><i class="fas fa-key"></i></span>
                  </div>
                  <asp:TextBox runat="server" ID="txtPassword" CssClass="form-control" TextMode="Password" placeholder="Contraseña"></asp:TextBox>
                </div>
								<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" Display="None" ValidationGroup="vgLogin" />
							</div>

							<div class="form-group col">
								<asp:ValidationSummary DisplayMode="SingleParagraph" runat="server" HeaderText="Ingrese usuario y contraseña por favor" CssClass="text-danger" ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgLogin" />
							</div>

							<div class="form-group col">
								<asp:Button runat="server" OnClick="LogIn" Text="Iniciar sesión" CssClass="btn btn-outline-primary btn-block" ValidationGroup="vgLogin" />
							</div>

							<p class="text-right col">
								<asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" CssClass="" ViewStateMode="Disabled">¿Olvidó su contraseña?</asp:HyperLink>
							</p>
						</div>
					</div>
				</asp:View>

				<asp:View runat="server" ID="vCambiar">
					<asp:HiddenField runat="server" ID="hfOP" />
					<asp:HiddenField runat="server" ID="hfDB" />
					<div id="CambiarForm" class="">
						<h4>Cambio de contraseña</h4>
						<br />
            <div class="row row-cols-1">
							<div class="form-group col">
                <%--<label for="txtNewPassword" class="">Contraseña nueva</label>--%>
                <div class="input-group">
                  <div class="input-group-prepend">
                    <span class=" input-group-text bg-primary text-white border-0"><i class="fas fa-key"></i></span>
                  </div>
                  <asp:TextBox runat="server" ID="txtNewPassword" CssClass="form-control" TextMode="Password" TabIndex="5" placeholder="Contraseña nueva"></asp:TextBox>
                </div>
								<asp:RequiredFieldValidator runat="server" ControlToValidate="txtNewPassword" SetFocusOnError="true" CssClass="text-danger" ErrorMessage="" ValidationGroup="vgPass" />
							</div>

							<div class="form-group col">
                <%--<label for="txtConfirmNewPassword" class="">Confirmación contraseña</label>--%>
                <div class="input-group">
                  <div class="input-group-prepend">
                    <span class=" input-group-text bg-primary text-white border-0"><i class="fas fa-key"></i></span>
                  </div>
                  <asp:TextBox runat="server" ID="txtConfirmNewPassword" CssClass="form-control" TextMode="Password" TabIndex="5" placeholder="Confirmación contraseña"></asp:TextBox>
                </div>
								<asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmNewPassword" SetFocusOnError="true" CssClass="text-danger" Display="Dynamic" ErrorMessage="" ValidationGroup="vgPass" />
								<asp:CompareValidator ID="ComparePW" runat="server" ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmNewPassword" SetFocusOnError="true" CssClass="text-danger" Display="Dynamic" ErrorMessage="" ValidationGroup="vgPass" />
							</div>

							<div class="form-group">
								<asp:ValidationSummary DisplayMode="SingleParagraph" runat="server" HeaderText="La contraseña y la confirmación no coinciden" ShowMessageBox="false" CssClass="text-danger" ShowSummary="true" ValidationGroup="vgPass" />
							</div>
							<div class="form-group">
								<asp:Button runat="server" ID="CambiarPW" OnClick="CambiarPW_Click" Text="Cambiar contraseña" CssClass="btn btn-outline-primary btn-block" ValidationGroup="vgPass" />
							</div>
					</div>
					</div>
				</asp:View>

				<asp:View runat="server" ID="vCambiarAntiguo">
					<asp:HiddenField runat="server" ID="HiddenField1" />
					<asp:HiddenField runat="server" ID="HiddenField2" />
					<h4>Cambio de contraseña</h4>
					<br />
          <div class="row row-cols-1">
						<div class="form-group col">
              <%--<label for="txtPWAntiguo" class="">Contraseña actual</label>--%>
              <div class="input-group">
                <div class="input-group-prepend">
                  <span class=" input-group-text bg-primary text-white border-0"><i class="fas fa-lock"></i></span>
                </div>
                <asp:TextBox runat="server" ID="txtPWAntiguo" CssClass="form-control" TextMode="Password" TabIndex="5" placeholder="Contraseña actual"></asp:TextBox>
              </div>
							<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPWAntiguo" SetFocusOnError="true" CssClass="text-danger" ErrorMessage="" ValidationGroup="vgCambioOri" />
						</div>

						<div class="form-group col">
              <%--<label for="txtPWNuevo" class="">Contraseña nueva</label>--%>
              <div class="input-group">
                <div class="input-group-prepend">
                  <span class=" input-group-text bg-primary text-white border-0"><i class="fas fa-key"></i></span>
                </div>
                <asp:TextBox runat="server" ID="txtPWNuevo" CssClass="form-control" TextMode="Password" TabIndex="5" placeholder="Contraseña nueva"></asp:TextBox>
              </div>
							<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPWNuevo" SetFocusOnError="true" CssClass="text-danger" ErrorMessage="" ValidationGroup="vgCambioOri" />
						</div>

						<div class="form-group col">
              <%--<label for="txtPWNuevoConfirmar" class="">Confirmación contraseña</label>--%>
              <div class="input-group">
                <div class="input-group-prepend">
                  <span class=" input-group-text bg-primary text-white border-0"><i class="fas fa-key"></i></span>
                </div>
                <asp:TextBox runat="server" ID="txtPWNuevoConfirmar" CssClass="form-control" TextMode="Password" TabIndex="5" placeholder="Confirmación contraseña"></asp:TextBox>
              </div>
							<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPWNuevoConfirmar" SetFocusOnError="true" CssClass="text-danger" ErrorMessage="" ValidationGroup="vgCambioOri" />
							<asp:CompareValidator ID="cvPW" runat="server" ControlToCompare="txtPWNuevoConfirmar" ControlToValidate="txtPWNuevo" SetFocusOnError="true" CssClass="text-danger" Display="Dynamic" ErrorMessage="" ValidationGroup="vgCambioOri" />
						</div>

						<div class="form-group">
							<asp:ValidationSummary DisplayMode="SingleParagraph" runat="server" HeaderText="La contraseña nueva y la confirmación no coinciden" CssClass="text-danger" ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgCambioOri" />
						</div>
						
            <div class="col-12">
              <div class="row">
								<div class="form-group col">
									<asp:Button runat="server" ID="btnCancelarPW" OnClick="btnCancelarPW_Click" Text="Cancelar" CssClass="btn btn-outline-secondary btn-block" CausesValidation="false" />
								</div>
									
								<div class="form-group col">
									<asp:Button runat="server" ID="btnCambiarPWOriginal" OnClick="btnCambiarPWOriginal_Click" Text="Aceptar" CssClass="btn btn-outline-primary btn-block" ValidationGroup="vgCambioOri" />
								</div>
							</div>
						</div>
					</div>
				</asp:View>
			</asp:MultiView>
		</div>
	</div>
</asp:Content>