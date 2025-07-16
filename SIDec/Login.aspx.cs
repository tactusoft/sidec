using GLOBAL.AUTENTICACION;
using GLOBAL.CONST;
using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using System;

namespace SIDec
{
    public partial class Login : System.Web.UI.Page
	{
		clLog oLog = new clLog();
		clGlobalVar oVar = new clGlobalVar();
		clUtil oUtil = new clUtil();

		USUARIOS_DAL oUsuarios = new USUARIOS_DAL();

		private string _MSGNOAUT = "No fue posible verificar su identidad. Si el inconveniente persiste por favor contacte al administrador";
		private const string _MSGLOGNOAUT = "Intento fallido: Usuario {0} no autenticado.";
		private const string _MSGLOGAUT = "Login exitoso: Usuario {0} autenticado.";
		private const string _MSGLOGCAMBIARPW = "Usuario {0} debe realizar el cambio de contraseña.";
		private const string _MSGLOGCAMBIOPWOK = "La contraseña para el usuario {0} fue cambiada exitosamente.";
		private const string _MSGLOGCAMBIOPWERR = "No fue posible cambiar la contraseña. Si el inconveniente persiste por favor contacte al administrador";

		protected void Page_Load(object sender, EventArgs e)
		{
			ForgotPasswordHyperLink.NavigateUrl = "Recuperar";
			if (Request.QueryString["o"] != null)
			{
				if (Request.QueryString["o"].ToString() == "0")
				{
					mvLogin.ActiveViewIndex = 2;
				}
			}
		}

		protected void btnCambiarPWOriginal_Click(object sender, EventArgs e)
		{
			fCambiarPassword(txtPWAntiguo.Text, txtPWNuevo.Text);
		}

		protected void btnCancelarPW_Click(object sender, EventArgs e)
		{
			Response.Redirect("default");
		}

		protected void CambiarPW_Click(object sender, EventArgs e)
		{
			fCambiarPassword(hfOP.Value, txtNewPassword.Text);
		}

		protected void LogIn(object sender, EventArgs e)
		{
			if (IsValid)
			{
				clAutentica oAutenticar = new GLOBAL.AUTENTICACION.clAutentica();
				int iResult = oAutenticar.Autenticacion(txtUserName.Text, txtPassword.Text);
				if (iResult == (int)clConstantes.Autenticacion.Autenticado)
				{
					oLog.RegistrarLogInfo("Login", "LogIn", string.Format(_MSGLOGAUT, txtUserName.Text));
					if ((bool)oVar.prUserCambiarPW)
					{
						hfOP.Value = txtPassword.Text;
						oLog.RegistrarLogInfo("Login", "LogIn", string.Format(_MSGLOGCAMBIARPW, txtUserName.Text));
						mvLogin.ActiveViewIndex = 1;
						txtNewPassword.Attributes.Add("value", "");
					}
					else
						Response.Redirect("default");
				}
				else    //Usuario No Autenticado
				{
					oLog.RegistrarLogInfo("Login", "LogIn", string.Format(_MSGLOGNOAUT, txtUserName.Text));
					fMensaje(_MSGNOAUT, (int)clConstantes.NivelMensaje.Error);
				}
			}
		}

		private void fCambiarPassword(string pwOri, string pwNew)
		{
			if (oUtil.DBOperationResult(oUsuarios.sp_u_usuario_password(oVar.prUser.ToString(), pwNew, pwOri, false)))
				fMensaje(string.Format(_MSGLOGCAMBIOPWOK, oVar.prUser.ToString()), (int)clConstantes.NivelMensaje.Exitoso);
			else
				fMensaje(string.Format(_MSGLOGCAMBIOPWERR, oVar.prUser.ToString()), (int)clConstantes.NivelMensaje.Error);
			Response.Redirect("login");
		}

		private void fMensaje(string Mensaje, int Estado)
		{
			(this.Master as Basic).Mensaje(Mensaje, Estado);
		}
	}
}