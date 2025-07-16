using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SIDec
{
    public partial class AuthenticNew : System.Web.UI.MasterPage
    {
        readonly clGlobalVar oVar = new clGlobalVar();
        readonly clLog oLog = new clLog();
        readonly clPermisos oPermisosUsuario = new clPermisos();
        readonly clUtil oUtil = new clUtil();

        readonly MENUGLOBAL_DAL oMenu = new MENUGLOBAL_DAL();
        readonly PERMISOS_DAL oPermisos = new PERMISOS_DAL();

        private const string _MSGLOGOUT = "Logout exitoso: Usuario {0} cierra sesion.";
        private const string _MSGNOAUTORIZADO = "Intento de acceso sin login o sesión expirada.";
        private const string _MSGNOAUTORIZADOAUTENTICADO = "No tiene los permisos para acceder al modulo {0}.";

        private const string _SOURCEPAGE = "Autentic";
        private const string _PATHLOGOUT = "Login";
        private const string _PATHDEFAULT = "Default";

        string PagesPermisos = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterScript();
            if (oVar.prUser == null || string.IsNullOrEmpty(oVar.prUser.ToString()))
                fLogout(Request.Url.Segments[1]);

            if (!IsPostBack)
            {
                lblUser.Attributes.Add("onClick", "return false;");
                fCrearPermisos();
                fCrearMenu();
                oVar.prPermisosMenu = PagesPermisos;
                lblUser.Text = oVar.prUserName.ToString();
            }

            string AbsolutePath = Request.Url.AbsolutePath;
            string pagina = AbsolutePath.Substring(AbsolutePath.LastIndexOf("/") + 1);

            if (pagina.ToLower() != "default" && !hasAuthorization(pagina.ToLower()))
                fRedirectDefault(Request.Url.AbsolutePath+"("+pagina + ")|"+ oVar.prPermisosMenu);

            if (pagina.ToLower() == "default")
            {
                string[] permisos = oVar.prPermisosMenu.ToString().Split(';');
                if (permisos.Length == 2 && permisos[0].Trim() != "")
                {
                    Response.Redirect(AbsolutePath.Substring(0, AbsolutePath.LastIndexOf("/") + 1) + permisos[0]);
                }
            } 
        }
        private void RegisterScript()
        {
            string key = "AuthenticNew.HideTooltip";
            StringBuilder scriptSlider = new StringBuilder();
            scriptSlider.Append("<script type='text/javascript'> ");
            scriptSlider.Append("   $('[id^=tooltip]').tooltip('hide'); ");
            scriptSlider.Append(" </script> ");

            if (!Page.ClientScript.IsStartupScriptRegistered(key))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
            }

        }

        private bool hasAuthorization(string page)
        {
            bool option = false;
            foreach(string item in oVar.prPermisosMenu.ToString().Split(';'))
            {
                if (item == page)
                    option = true;
            }

            return option;
        }

        protected void Page_init(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)oVar.prUser))
            {
                fLogout(_MSGNOAUTORIZADO);
            }
        }

        protected void lbSalir_Click(object sender, EventArgs e)
        {
            fLogout(string.Format(_MSGLOGOUT, oVar.prUser.ToString()));
        }

        protected void lbCambiar_Click(object sender, EventArgs e)
        {
            Response.Redirect(_PATHLOGOUT + "?o=0");
        }

        private void fLogout(string strMensaje)
        {
            oLog.RegistrarLogInfo(_SOURCEPAGE, "Logout", strMensaje);
            oUtil.ClearSession();
            Response.Redirect(_PATHLOGOUT);
        }

        #region CREAR MENU
        private void fCrearMenu()
        {
            DataTable dtMenuItems = new DataTable();
            dtMenuItems = oMenu.sp_s_menu();
            foreach (DataRow drMenuItem in dtMenuItems.Rows)
            {
                if (drMenuItem["cod_menu"].Equals(drMenuItem["padre_id"]))
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    menubar.Controls.Add(li);

                    if (string.IsNullOrEmpty(drMenuItem["URL_menu"].ToString()))  //Tiene SUBMENU
                    {
                        if (oPermisosUsuario.bPermisoMenu(drMenuItem["cod_menu"].ToString()))
                        {
                            li.Attributes.Add("class", "nav-item dropdown");
                            HtmlGenericControl anchor = new HtmlGenericControl("a");
                            anchor.Attributes.Add("class", "nav-link dropdown-toggle");
                            anchor.Attributes.Add("data-toggle", "dropdown");
                            anchor.Attributes.Add("href", "#");
                            anchor.InnerText = drMenuItem["nombre_menu"].ToString();
                            if (drMenuItem["visible"].ToString() == "1") li.Controls.Add(anchor);

                            HtmlGenericControl div = new HtmlGenericControl("div");
                            div.Attributes.Add("class", "dropdown-menu fs12");
                            if (drMenuItem["visible"].ToString() == "1") li.Controls.Add(div);
                            AddSubMenuItem(div, dtMenuItems, drMenuItem["padre_id"].ToString());
                        }
                    }
                    else
                    {
                        if (oPermisosUsuario.bPermisoMenu(drMenuItem["cod_menu"].ToString()))
                        {
                            string url = drMenuItem["URL_menu"].ToString() + (drMenuItem["atributos"].ToString().Length > 1 ? "?" + drMenuItem["atributos"].ToString() : "");
                            li.Attributes.Add("class", "nav-item");
                            HtmlGenericControl anchor = new HtmlGenericControl("a");
                            anchor.Attributes.Add("class", "nav-link");
                            anchor.Attributes.Add("href", url);
                            PagesPermisos = PagesPermisos + drMenuItem["URL_menu"].ToString().ToLower() + ";";
                            anchor.InnerText = drMenuItem["nombre_menu"].ToString();
                            if (drMenuItem["visible"].ToString() == "1") li.Controls.Add(anchor);
                            AddSubMenuItem(new HtmlGenericControl("div"), dtMenuItems, drMenuItem["padre_id"].ToString());
                        }
                    }
                }
            }
        }

        private void AddSubMenuItem(HtmlGenericControl div, DataTable dtMenuItems, string IdPadre)
        {
            //recorremos cada elemento del datatable para poder determinar cuales son elementos hijos
            foreach (DataRow drMenuItem in dtMenuItems.Rows)
            {
                if (drMenuItem["padre_id"].ToString().Equals(IdPadre) && !(drMenuItem["cod_menu"].Equals(drMenuItem["padre_id"])))
                {
                    if (oPermisosUsuario.bPermisoMenu(drMenuItem["cod_menu"].ToString()))
                    {
                        HtmlGenericControl anchorsubmenu = new HtmlGenericControl("a");
                        anchorsubmenu.Attributes.Add("class", "dropdown-item");
                        string url = drMenuItem["URL_menu"].ToString() + (drMenuItem["atributos"].ToString().Length > 1 ? "?" + drMenuItem["atributos"].ToString() : "");
                        anchorsubmenu.Attributes.Add("href", url);
                        anchorsubmenu.Attributes.Add("class", "dropdown-item");
                        PagesPermisos = PagesPermisos + drMenuItem["URL_menu"].ToString().ToLower() + ";";
                        anchorsubmenu.InnerText = drMenuItem["nombre_menu"].ToString();
                        if (drMenuItem["visible"].ToString() == "1") div.Controls.Add(anchorsubmenu);
                    }
                }
            }
        }
        #endregion

        private void fCrearPermisos()
        {
            DataSet odsPermisos = oPermisos.sp_s_permisos_cod_usuario(oVar.prUserCod.ToString());
            oVar.prPermisosUsuario = odsPermisos;
        }

        private void fRedirectDefault(string Pagina)
        {
            oLog.RegistrarLogInfo(_SOURCEPAGE, "RedirectDefault", string.Format(_MSGNOAUTORIZADOAUTENTICADO, Pagina));
            Response.Redirect(_PATHDEFAULT);
            Mensaje(string.Format(_MSGNOAUTORIZADOAUTENTICADO, Pagina), (int)GLOBAL.CONST.clConstantes.NivelMensaje.Error);
        }

        public void fReload()
        {
            string Reload = Session["ReloadXFU"] == null ? "" : "1";
            if (Reload == "1")
            {
                fCrearPermisos();
                fCrearMenu();
            }
            Session["ReloadXFU"] = null;
        }

        public void Mensaje(string Mensaje, int NivelMensaje)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "MensajeWeb('" + Mensaje + "'," + NivelMensaje + ");", true);
        }

        private static Control fGetControl(Control RootControl, string ControlIdBuscar)
        {
            if (RootControl.ID == ControlIdBuscar)
                return RootControl;
            foreach (Control Ctl in RootControl.Controls)
            {
                Control FoundCtl = fGetControl(Ctl, ControlIdBuscar);
                if (FoundCtl != null)
                    return FoundCtl;
            }
            return null;
        }

        public void EstadoBoton(bool Habilitado, string lbBotonID)
        {
            LinkButton lbContent = (LinkButton)fGetControl(ContentPlaceHolder1, lbBotonID);
            if (lbContent != null)
            {
                lbContent.Enabled = Habilitado;
                if (Habilitado)
                {
                    lbContent.Style.Remove("color");
                    lbContent.Style.Add("cursor", "pointer");
                }
                else
                {
                    lbContent.Style.Add("color", "#aaa");
                    lbContent.Style.Add("cursor", "not-allowed");
                }
            }
        }

        protected void lbManual_Click(object sender, EventArgs e)
        {
            oVar.prFullPathDoc = oVar.prDocAyuda.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
            Session["ReloadXFU"] = "1";
            fReload();
        }

        protected void lbGeosidec_Click(object sender, EventArgs e)
        {
            string pathBase = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            string pageurl = pathBase + "/geosidec/";
            Response.Write("<script> window.open('" + pageurl + "','_blank'); </script>");
            Session["ReloadXFU"] = "1";
            fReload();
        }
    }
}