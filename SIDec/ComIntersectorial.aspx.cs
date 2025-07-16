using GLOBAL.CONST;
using GLOBAL.PERMISOS;
using GLOBAL.VAR;
using System;
using System.Web.UI;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec
{
    public partial class ComIntersectorial : Page
    {
        #region Propiedades

        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();
        #endregion



        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            btnConfirmarComIntersectorial.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarComIntersectorial.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarComIntersectorial, "Click") + "; return false;");

            if (!IsPostBack)
            {
                Page.Form.Enctype = "multipart/form-data";
                ViewState["CriterioBuscar"] = "";
                ViewState["AccionFinal"] = "";

                ViewState["IndexComIntersectorial"] = "0";

                ViewState["SortExpComIntersectorial"] = "codigo";
                ViewState["SortDirComIntersectorial"] = "ASC";

                txtBuscar.Focus();
                Load_ComIntersectorial();
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string filter = txtBuscar.Text.Trim() == "" ? "%" : txtBuscar.Text.Trim();
            ViewState["CriterioBuscar"] = filter;
            Load_ComIntersectorial();
        }
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            bool permiso = false;
            switch (ViewState["AccionFinal"].ToString())
            {
                case "_Edit":
                    if ((Session["ComIntersectorial.idproyecto"] ?? 0).ToString() == "0" || (Session["ComIntersectorial.idproyecto"] ?? 0).ToString() == "")
                    {
                        if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.EDITAR, true, false)) return;
                    }
                    ViewDetail();
                    Session["ReloadXFU"] = "1";
                    Load_ComIntersectorial();
                    (Master as AuthenticNew).fReload();
                    return;
                case "_Add":
                    if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.INSERTAR, true, false)) return;

                    ViewDetail();

                    Session["ReloadXFU"] = "1";
                    Load_ComIntersectorial();
                    (Master as AuthenticNew).fReload();
                    return;
                case "_Delete":
                    if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.ELIMINAR, true)) return;
                    permiso = true;
                    break;
            }
            if (!permiso)
            {
                oBasic.AlertMain(msgMain, clConstantes.MSG_ERR_PERMISO, "danger");
            }
            Load_ComIntersectorial();
            oBasic.FixPanel(divData, "ComIntersectorial", 0);

            Session["ReloadXFU"] = "1";
            (Master as AuthenticNew).fReload();
        }
        protected void MessageBox_Accept(string key)
        {
            try
            {
                switch (key)
                {
                    case "Delete":
                        oBasic.AlertMain(msgMain, clConstantes.MSG_ERR_PERMISO, "danger");
                        Load_ComIntersectorial();
                        oBasic.FixPanel(divData, "ComIntersectorial", 0);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }
        protected void ucFolios_ViewDoc(object sender)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
        }
        #endregion



        #region Métodos Privados
        private void Load_ComIntersectorial()
        {
            if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.CONSULTAR, false, false)) return;

            string usuario = (string)((oPermisos.TienePermisosAccion(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.CONSULTAR, "", oVar.prUserCod.ToString())) ? "" : oVar.prUserCod.ToString());
            LoadDetail();
        }
        private void LoadDetail()
        {
            ucFolios.CodUsuario = oVar.prUserCod.ToString();
            ucFolios.Filter = ViewState["CriterioBuscar"].ToString();
            ucFolios.LoadControl();
            oBasic.FixPanel(divData, "FichaProyecto", 0, pList: (hdd_Proyecto_ComIntersectorial_Id.Value == ""), pAdd: (hdd_Proyecto_ComIntersectorial_Id.Value == ""));
            upProyectosSection.Update();
        }


        private bool ValidateAccess(string section, string action, bool validateResponsible = false, bool requeridedResponsible = true)
        {
            string cod_usu_responsable = oVar.prUserCod.ToString();
            string message = oPermisos.ValidateAccess(section, action, cod_usu_responsable, validateResponsible, requeridedResponsible);

            if (message == "")
                return true;

            MessageInfo.ShowMessage(message);
            return false;
        }
        private void ViewDetail()
        {
            if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.CONSULTAR, false, false)) return;

            LoadDetail();
            oBasic.FixPanel(divData, "ComIntersectorial", 1);

            oBasic.FixPanel(divData, "FichaProyecto", 0, pList: (hdd_Proyecto_ComIntersectorial_Id.Value == ""), pAdd: (hdd_Proyecto_ComIntersectorial_Id.Value == ""));
        }
        #endregion
    }
}