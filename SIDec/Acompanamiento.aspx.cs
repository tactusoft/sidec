using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIDec
{
    public partial class Acompanamiento : System.Web.UI.Page
    {
        private readonly clBasic oBasic = new clBasic();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["Retorno.Proyecto.Page"] ?? "").ToString() != "")
                {
                    ucProyecto.Filter = txtBuscar.Text = (Session["Retorno.ucProyecto.filter"] ?? "").ToString().Replace("%", "");
                    ucProyecto.IdProyecto = Session["Retorno.ucProyecto.ID"].ToString();
                    ucProyecto.ViewType = Session["Retorno.ucProyecto.ViewType"].ToString();
                    Session["Retorno.ucProyecto.ID"] = null;
                    Session["Retorno.ucProyecto.ViewType"] = null;
                    Session["Retorno.ucProyecto.filter"] = null;
                    Session["Retorno.Proyecto.Page"] = null;
                }
                else
                {
                    ucProyecto.Filter = "";
                    if (Session["Acompanamiento.IdProyecto"] != null)
                    {
                        ucProyecto.IdProyecto = Session["Acompanamiento.IdProyecto"].ToString();
                        ucProyecto.ViewType = "1";
                        Session["Acompanamiento.IdProyecto"] = null;
                    }
                }

                ucProyecto.LoadGrid();
                LoadChildren();
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ucProyecto.Filter = txtBuscar.Text;
            ucProyecto.LoadGrid();

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
        protected void ucLicencias_ViewDoc(object sender)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
        }
        protected void ucProyecto_SelectedProyecto(object sender)
        {
            LoadChildren();
            upProyectosSection.Update();
        }
        protected void ucPredios_GoPredios(object sender)
        {
            Session["Retorno.ucProyecto.ID"] = ucProyecto.IdProyecto;
            Session["Retorno.ucProyecto.ViewType"] = ucProyecto.ViewType;
            Session["Retorno.ucProyecto.filter"] = ucProyecto.Filter;
            Session["Retorno.ucProyecto.chip"] = ucPredios.Chip;

            Session["Retorno.Predios.Origen"] = "acompanamiento";
            Session["Proyecto.Predios.chip"] = ucPredios.Chip;
            Response.Redirect("Predios");
        }



        private void EnableButtons()
        {
            upProyectos.Update();
        }
        private void LoadChildren()
        {

            //ucLicencias.Filter = "";
            //ucPredios.IdProyecto = ucProyecto.IdProyecto;
            //ucLicencias.LoadGrid();

            if (Int32.TryParse(ucProyecto.IdProyecto, out int IdProyecto))
            {
                ucPredios.ProyectoID = IdProyecto;
                ucPredios.Chip = (Session["Retorno.ucProyecto.chip"] ?? "").ToString();
                Session["Retorno.ucProyecto.chip"] = null;
                ucPredios.ResponsibleUserCode = ucProyecto.ResponsibleUserCode;
                ucPredios.LoadControl();

                ucLicencias.ProyectoID = IdProyecto;
                ucLicencias.ResponsibleUserCode = ucProyecto.ResponsibleUserCode;
                ucLicencias.LoadControl();

                ucResponsables.ReferenceID = IdProyecto;
                ucResponsables.ActorID = Convert.ToInt32(ucProyecto.IdActor);
                ucResponsables.ResponsibleUserCode = ucProyecto.ResponsibleUserCode;
                ucResponsables.LoadGrid();
            }
        }
    }
}