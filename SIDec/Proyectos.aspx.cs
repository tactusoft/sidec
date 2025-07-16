using GLOBAL.VAR;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using tipoArchivo = GLOBAL.CONST.clConstantes.TipoArchivo;

namespace SIDec
{
    public partial class Proyectos : Page
    {
        private readonly clBasic oBasic = new clBasic();
        private readonly clGlobalVar oVar = new clGlobalVar();


        #region events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Form.Enctype = "multipart/form-data";

                ViewState["CriterioBuscar"] = "";
                ViewState["AccionFinal"] = "";

                ViewState["SortExpProyectosPredios"] = "chip";
                ViewState["SortExpProyectosCartas"] = "fecha_radicado_manifestacion_interes";
                ViewState["SortExpProyectosLicencias"] = "origen";
                ViewState["SortExpProyectosActores"] = "documento";
                ProyectosLoad();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ucProyecto.Filter = txtBuscar.Text;
            ucProyecto.LoadControl();

            LoadChildren();
        }
        protected void btnProyectosSection_Click(object sender, EventArgs e)
        {
            int oldIndex = mvProyectosSection.ActiveViewIndex;
            int newIndex = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            string oldID = "lbProyectosSection_" + oldIndex.ToString();
            string newID = "lbProyectosSection_" + newIndex.ToString();
            LinkButton lbOld = (LinkButton)ulProyectosSection.FindControl(oldID);
            LinkButton lbNew = (LinkButton)ulProyectosSection.FindControl(newID);
            oBasic.ActiveNav(mvProyectosSection, lbOld, lbNew, newIndex);

            EnableButtons();
            //Session["ReloadXFU"] = "1";
            //(Master as AuthenticNew).fReload();
            upProyectosSection.Update();
        }
        protected void uc_ViewDoc(object sender)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
        }
        protected void ucPredios_GoPredios(object sender)
        {
            Session["Retorno.ucProyecto.ID"] = ucProyecto.IdProyecto;
            Session["Retorno.ucProyecto.ViewType"] = ucProyecto.ViewType;
            Session["Retorno.ucProyecto.filter"] = ucProyecto.Filter;
            Session["Retorno.ucProyecto.chip"] = ucPredios.Chip;

            Session["Retorno.Predios.Origen"] = "proyectos";
            Session["Proyecto.Predios.chip"] = ucPredios.Chip;
            Response.Redirect("Predios");
        }
        protected void ucProyecto_SelectedProyecto(object sender)
        {
            LoadChildren();
            upProyectosSection.Update();
        }
        protected void ucVisita_UserControlException(object sender, Exception ex)
        {
            MessageInfo.ShowMessage("Se ha presentado un error en el sistema: " + ex.Message);
        }
        protected void ucVisita_ViewDoc(object sender)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
        }
        #endregion


        #region Method
        private void EnableButtons()
        {
            upProyectos.Update();
        }
        private void LoadChildren()
        {
            if (int.TryParse(ucProyecto.IdProyecto, out int IdProyecto))
            {
                ucPredios.ProyectoID = IdProyecto;
                ucPredios.Chip = (Session["Retorno.ucProyecto.chip"] ?? "").ToString();
                Session["Retorno.ucProyecto.chip"] = null;
                ucPredios.ResponsibleUserCode = ucProyecto.ResponsibleUserCode;
                ucPredios.LoadControl();

                ucCartas.ReferenceID = IdProyecto;
                ucCartas.ResponsibleUserCode = ucProyecto.ResponsibleUserCode;
                ucCartas.LoadGrid();

                ucLicencias.ProyectoID = IdProyecto;
                ucLicencias.ResponsibleUserCode = ucProyecto.ResponsibleUserCode;
                ucLicencias.LoadControl();

                ucResponsables.ReferenceID = IdProyecto;
                ucResponsables.ActorID = Convert.ToInt32(ucProyecto.IdActor);
                ucResponsables.ResponsibleUserCode = ucProyecto.ResponsibleUserCode;
                ucResponsables.LoadGrid();

                ucVisitas.ReferenceID = IdProyecto;
                ucVisitas.ResponsibleUserCode = ucProyecto.ResponsibleUserCode;
                ucVisitas.FilePath = oVar.prPathDocumentosProyectos.ToString();
                ucVisitas.Prefix = tipoArchivo.IMG_VPA;
                ucVisitas.LoadControl();
            }
        }
        private void ProyectosLoad()
        {
            if ((Session["Retorno.Proyecto.Page"] ?? "").ToString() != "")
            {
                ucProyecto.Filter = txtBuscar.Text = (Session["Retorno.ucProyecto.filter"] ?? "").ToString().Replace("%", "");
                ucProyecto.IdProyecto = Session["Retorno.ucProyecto.ID"].ToString();
                ucProyecto.ViewType = Session["Retorno.ucProyecto.ViewType"].ToString();
                Session["Retorno.ucProyecto.ViewType"] = null;
                Session["Retorno.ucProyecto.filter"] = null;
                Session["Proyecto.Banco.Id"] = null;
            }
            else
            {
                ucProyecto.Filter = "";
                if (Session["Proyecto.IdProyecto"] != null)
                {
                    ucProyecto.IdProyecto = Session["Proyecto.IdProyecto"].ToString();
                    ucProyecto.ViewType = "1";
                    Session["Proyecto.IdProyecto"] = null;
                }
            }

            ucProyecto.LoadControl();
            LoadChildren();
        }
        #endregion
    }
}