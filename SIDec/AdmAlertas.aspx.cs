using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using SigesTO;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec
{
    public partial class AdmAlertas : Page
    {
        private readonly RangoEjecucion_DAL oRango = new RangoEjecucion_DAL();
        private readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();

        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clUtil oUtil = new clUtil();
        private readonly clLog oLog = new clLog();
        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();

        private const string _SOURCEPAGE = "Admin";

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                LoadGrid();

                txtBuscar.Focus();
            }
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            MessageBox1.ShowConfirmation("Editar", "¿Está seguro de actualizar la información?", type: "warning");
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["Admin.CriterioBuscar"] = txtBuscar.Text.Trim();
            LoadGrid();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadGrid();
            gridSelect();
        }
        protected void btnAdminAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgMain, "", "0");
            upMsgMain.Update();

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    LoadGrid();
                    gridSelect();
                    break;
                case "Editar":
                    ViewEdit();
                    break;
                default:
                    return;
            }
        }
        protected void btnAdminNavegation_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexAdmin"]) - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexAdmin"]) + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSUsuarios).Tables[0].Rows.Count - 1;
                    break;
            }
            gvAdmin.PageIndex = (index - (index % gvAdmin.PageSize)) / gvAdmin.PageSize;
            gvAdmin.DataSource = ((DataSet)oVar.prDSUsuarios).Tables[0];
            gvAdmin.DataBind();
            gvAdmin.SelectedIndex = index % gvAdmin.PageSize;
            ViewState["IndexAdmin"] = index.ToString();

            hdd_idrango_ejecucion.Value = index >= 0 ? gvAdmin.DataKeys[gvAdmin.SelectedIndex]["idrango_ejecucion"].ToString() : "0";

            LoadDetail();
            gv_SelectedIndexChanged(gvAdmin);
        }
        protected void gvAdmin_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAdmin.PageIndex = e.NewPageIndex;
            LoadGrid();
            if (gvAdmin.SelectedIndex > gvAdmin.Rows.Count)
                gvAdmin.SelectedIndex = gvAdmin.Rows.Count - 1;
            ViewState["IndexAdmin"] = ((gvAdmin.PageSize * gvAdmin.PageIndex) + gvAdmin.SelectedIndex);
        }
        protected void gvAdmin_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            oBasic.AlertMain(msgMain, "", "0");
            upMsgMain.Update();

            if (e.CommandName != "_Detail")
                return;

            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex > -1)
                {
                    if (rowIndex >= gvAdmin.Rows.Count)
                        rowIndex = 0;
                    gvAdmin.SelectedIndex = rowIndex;
                    gv_SelectedIndexChanged(gvAdmin);
                    hdd_idrango_ejecucion.Value = rowIndex >= 0 ? gvAdmin.DataKeys[rowIndex]["idrango_ejecucion"].ToString() : "0";

                    switch (e.CommandName)
                    {
                        case "_Detail":
                            ViewDetail();
                            break;
                        default:
                            return;
                    }
                }
            }
        }
        protected void gvAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvAdmin, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvAdmin);
        }
        protected void MessageBox_Accept(string key)
        {
            try
            {
                switch (key)
                {
                    case "Editar":
                        Edit();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion



        #region Methods
        private void Edit()
        {
            RangoEjecucionTO rango = new RangoEjecucionTO()
            {
                IdRangoEjecucionTO = Convert.ToInt32(hdd_idrango_ejecucion.Value),
                DiasLimite = Convert.ToInt32(txt_dias_limite.Text),
                PorcentajeLimite = Convert.ToDouble(txt_vporcentaje_limite.Text),
                DiasLimiteCritico = Convert.ToInt32(txt_dias_limite_critico.Text),
                PorcentajeLimiteCritico = Convert.ToDouble(txt_vporcentaje_limite_critico.Text)
            };
            string strResult = oRango.sp_u_rango_ejecucion(rango);

            if (oBasic.AlertUserControl(msgAdmin, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u"))
            {

                oBasic.SPOk(msgMain, msgAdmin, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                LoadGrid();
            }
            else
            {
                oBasic.SPError(msgAdmin, msgMain, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResult);
                MessageInfo.ShowMessage("No fue posible actualizar rango de ejecución.", type: "danger");
            }
            upMsgMain.Update();
        }
        private bool Enabled()
        {
            return ViewState["Admin.Enabled"].ToString() == "1";
        }
        private void gridSelect()
        {
            int index = Convert.ToInt16(ViewState["IndexAdmin"]);
            gvAdmin.PageIndex = (index - (index % gvAdmin.PageSize)) / gvAdmin.PageSize;
            gvAdmin.DataSource = ((DataSet)oVar.prDSUsuarios).Tables[0];
            gvAdmin.DataBind();
            gvAdmin.SelectedIndex = index % gvAdmin.PageSize;
        }
        private void gv_SelectedIndexChanged(GridView gv)
        {
            string modulo = gv.ID.Substring(2);
            if (gvAdmin.SelectedIndex > gvAdmin.Rows.Count)
                gvAdmin.SelectedIndex = gvAdmin.Rows.Count - 1;
            ViewState["Index" + modulo] = ((gv.PageIndex * gv.PageSize) + gv.SelectedIndex).ToString();

            try
            {
                UpdatePanel up = (UpdatePanel)divData.FindControl("up" + modulo + "Foot");
                oBasic.LblRegistros(up, ((DataSet)oVar.prDSUsuarios).Tables[0].Rows.Count, Convert.ToInt32(ViewState["Index" + modulo] ?? "0"));
            }
            catch { }
        }
        private void Initialize()
        {
            ViewState["Admin.Enabled"] = "0";
            ViewState["Admin.CriterioBuscar"] = "";

            ViewState["IndexAdmin"] = "0";
        }
        private void LoadDetail()
        {
            oBasic.fClearControls(pnlDetail);


            DataSet dSet = oRango.sp_s_rango_ejecucion(hdd_idrango_ejecucion.Value);
            if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
            {
                DataRow dRow = dSet.Tables[0].Rows[0];
                oBasic.fValueControls(pnlDetail, dRow);
                mvAdmin.ActiveViewIndex = 1;
            }            

            pnlAdminNavegation.Visible = divAdminNavegation.Visible = true;
            oBasic.EnableControls(pnlDetail, Enabled(), true);
            txt_tipo.Enabled = false;
            txt_nombre.Enabled = false;
            oBasic.FixPanel(divData, "Admin", Enabled() ? 2 : 1, pList: true);
            oBasic.EnableCtl(lblAdminCuenta, true, true);
            int Indice = Convert.ToInt16(ViewState["IndexAdmin"]);
            oBasic.LblRegistros(upAdminFoot, ((DataSet)oVar.prDSUsuarios).Tables[0].Rows.Count, Indice);
        }
        private void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.ADMINISTRACION_ALERTA, cnsAction.CONSULTAR)) return;
            oVar.prDSUsuarios = oRango.sp_s_rangos_ejecucion(rbl_tipo.SelectedValue, ViewState["Admin.CriterioBuscar"].ToString());

            gvAdmin.DataSource = ((DataSet)(oVar.prDSUsuarios));
            gvAdmin.DataBind();

            if (gvAdmin.Rows.Count == 0)
            {
                lblAdminCuenta.Text = "";
            }
            else
            {
                gvAdmin.SelectedIndex = gvAdmin.SelectedIndex == -1 ? 0 : gvAdmin.SelectedIndex;
            }
            gv_SelectedIndexChanged(gvAdmin);
            oBasic.FixPanel(divData, "Admin", 0, pAdd: false, pEdit: false, pDelete: false);
            mvAdmin.ActiveViewIndex = 0;
            pnlAdminNavegation.Visible = divAdminNavegation.Visible = false;
            upAdmin.Update();
            upAdminNavegation.Update();
        }
        private bool ValidateAccess(string section, string action)
        {
            string message = oPermisos.ValidateAccess(section, action, oVar.prUserCod.ToString(), false, false);

            if (message == "")
                return true;

            MessageInfo.ShowMessage(message);
            return false;
        }
        private void ViewDetail()
        {
            if (!ValidateAccess(cnsSection.ADMINISTRACION_ALERTA, cnsAction.CONSULTAR)) return;

            ViewState["Admin.Enabled"] = "0";
            LoadDetail();
        }
        private void ViewEdit()
        {
            if (!ValidateAccess(cnsSection.ADMINISTRACION_ALERTA, cnsAction.EDITAR)) return;

            ViewState["Admin.Enabled"] = "1";
            LoadDetail();
        }
        #endregion

        protected void rbl_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
    }
}