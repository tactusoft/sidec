using GLOBAL.DAL;
using GLOBAL.PERMISOS;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec.UserControls.Acompanamiento
{
    public partial class ProyectoPrediosUC : UserControl
    {
        private readonly PROYECTOSPREDIOS_DAL oProyectosPredios = new PROYECTOSPREDIOS_DAL();

        private readonly clBasic oBasic = new clBasic();
        private readonly clPermisos oPermisos = new clPermisos();

        private const string _SOURCEPAGE = "ProyectoPrediosUC";

        public delegate void OnUserControlExceptionEventHandler(object sender, Exception ex);
        public event OnUserControlExceptionEventHandler UserControlException;

        public delegate void OnGoPredioEventHandler(object sender);
        public event OnGoPredioEventHandler GoPredio;

        #region Propiedades
        /// <summary>
        /// Identificador del registro del proyectopredio
        /// </summary>
        public string Chip
        {
            get
            {
                return txt_chip.Text;
            }
            set
            {
                txt_chip.Text = value;
            }
        }
        public string ControlID
        {
            get
            {
                return (hddId.Value);
            }
            set
            {
                hddId.Value = value;
            }
        }
        public bool Enabled
        {
            get
            {
                return (Session[ControlID + ".ProyectoPredios.Enabled"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".ProyectoPredios.Enabled"] = value ? "1" : "0";
            }
        }
        /// <summary>
        /// Identificacion del proyecto al que se asocian los proyectosProyectoPredios
        /// </summary>
        public int ProyectoID
        {
            get
            {
                return (Int32.TryParse(hddIdProyecto.Value, out int id) ? id : 0);
            }
            set
            {
                hddIdProyecto.Value = value.ToString();
                LoadGrid();
            }
        }
        /// <summary>
        /// Identificador del registro del proyectopredio
        /// </summary>
        public int ProyectoPredioID
        {
            get
            {
                return (Int32.TryParse(hddProyectoPredioPrimary.Value, out int id) ? id : 0);
            }
            set
            {
                hddProyectoPredioPrimary.Value = value.ToString();
                hdd_au_proyecto_predio.Value = hddProyectoPredioPrimary.Value;
            }
        }
        /// <summary>
        /// Esta propiedad solo se debe asignar para asegurar que el contenedor de control requiera un usuario responsable
        /// </summary>
        public string ResponsibleUserCode
        {
            get
            {
                return (Session[ControlID + ".VisitasSitio.ResponsibleUserCode"] ?? "NoRequired").ToString();
            }
            set
            {
                Session[ControlID + ".VisitasSitio.ResponsibleUserCode"] = value;
            }
        }
        #endregion



        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
            ViewControls();
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            switch (ViewState["ST" + ControlID])
            {
                case "Detail":
                    if (hdd_au_proyecto_predio.Value != "0")
                    {
                        MessageBox1.ShowConfirmation("Detail", "¿Está seguro de actualizar la información?", type: "warning");
                    }
                    else
                        MessageBox1.ShowConfirmation("Detail", "¿Está seguro de continuar con la acción solicitada?");
                    break;
            }
        }
        protected void btnGoPredio_Click(object sender, EventArgs e)
        {
            GoPredio?.Invoke(this);
        }
        protected void btnProyectoPredioAdd_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgProyectoPredioMain, "", "0");
            ViewAdd();
        }
        protected void btnProyectoPredioAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgProyectoPredioMain, "", "0");

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    LoadGrid();
                    break;
                case "Editar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.EDITAR)) return;
                    Enabled = true;
                    break;
                case "Agregar":
                    ViewAdd();
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.ELIMINAR)) return;

                    MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar este registro?", type: "danger");
                    return;
            }
            ViewControls();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgProyectoPredioMain, "", "0");
            ViewState["ST" + ControlID] = "Grid";

            Enabled = false;
            ViewControls();
        }
        protected void gvProyectoPredio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                oBasic.AlertMain(msgProyectoPredioMain, "", "0");
                if (rowIndex >= gvProyectoPredio.Rows.Count)
                    rowIndex = 0;
                hdd_au_proyecto_predio.Value = rowIndex >= 0 ? gvProyectoPredio.DataKeys[rowIndex]["au_proyecto_predio"].ToString() : "0";

                switch (e.CommandName)
                {
                    case "Select":
                        ViewState["ST" + ControlID] = "Grid";
                        break;
                    case "_Detail":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.CONSULTAR, false, false)) return;

                        ViewState["ST" + ControlID] = "Detail";
                        Enabled = false;
                        LoadDetail();
                        break;
                    case "_Delete":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.ELIMINAR, true, false)) return;

                        ViewState["ST" + ControlID] = "Detail";
                        Enabled = false;
                        LoadDetail();
                        MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar este registro?", type: "danger");
                        break;
                    case "_Edit":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.EDITAR, true)) return;

                        ViewState["ST" + ControlID] = "Detail";
                        Enabled = true;
                        LoadDetail();
                        break;
                    default:
                        return;
                }
            }
            ViewControls();
        }
        protected void gvProyectoPredio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvProyectoPredio, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvProyectoPredio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProyectoPredio.PageIndex = e.NewPageIndex;
            LoadGrid();
            ViewState["IndexProyectoPredio"] = ((gvProyectoPredio.PageSize * gvProyectoPredio.PageIndex) + gvProyectoPredio.PageIndex - 1).ToString();
        }
        protected void MessageBox_Accept(string key)
        {
            try
            {
                string strResult = "";
                switch (key)
                {
                    case "Detail":
                        strResult = SaveProyectoPredio();
                        break;
                    case "Delete":
                        strResult = DeleteProyectoPredio();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                UserControlException?.Invoke(this, e);
            }
        }
        protected void txt_chip_TextChanged(object sender, EventArgs e)
        {
            var aux_chip = txt_chip.Text;
            var aux_idproypredio = hdd_au_proyecto_predio.Value;
            var aux_cumple_funcion = chk_cumple_funcion_social.Checked;
            var aux_descripcion = txt_observacion__predio.Text;

            if (txt_chip.Text != "")
            {
                DataSet dsPredio = oProyectosPredios.sp_s_predio_chip_buscar(txt_chip.Text, ProyectoID.ToString());

                if (dsPredio.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = dsPredio.Tables[0].Rows[0];
                    var id = dRow["au_proyecto_predio"].ToString();
                    if (id != hdd_au_proyecto_predio.Value && id != "")
                    {
                        txt_chip.Text = "";
                        MessageInfo.ShowMessage("El CHIP ingresado ya corresponde al proyecto");
                        return;
                    }
                    oBasic.fValueControls(pnlProyectoPredio, dRow);
                    hdd_au_proyecto_predio.Value = aux_idproypredio;
                    Enabled = true;
                    ViewControls();
                    return;
                }
            }
            oBasic.fClearControls(pnlProyectoPredio);
            txt_chip.Text = aux_chip;
            hdd_au_proyecto_predio.Value = aux_idproypredio;
            chk_cumple_funcion_social.Checked = aux_cumple_funcion;
            txt_observacion__predio.Text = aux_descripcion;
            Enabled = true;
            ViewControls();
        }
        #endregion



        #region "Métodos público"
        public void LoadControl()
        {
            ViewState["ST" + ControlID] = ViewState["ST" + ControlID] ?? "Grid";
            switch (ViewState["ST" + ControlID].ToString())
            {
                case "Grid":
                    LoadGrid();
                    break;
                case "Detail":
                    LoadDetail();
                    break;
            }

            ViewControls();
        }
        #endregion



        #region Métodos privados
        private string DeleteProyectoPredio()
        {
            hdd_au_proyecto_predio.Value = hdd_au_proyecto_predio.Value == "" ? "0" : hdd_au_proyecto_predio.Value;

            string strResult = oProyectosPredios.sp_d_proyecto_predio(hdd_au_proyecto_predio.Value);
            if (oBasic.AlertUserControl(msgProyectoPredio, msgProyectoPredioMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d"))
            {
                ViewState["ST" + ControlID] = "Grid";
                LoadControl();
                ViewControls();
                upProyectoPredioFoot.Update();
            }

            return strResult;
        }
        private void EnabledControls()
        {
            txt_chip.Enabled = Enabled;
            chk_cumple_funcion_social.Enabled = Enabled;
            txt_observacion__predio.Enabled = Enabled;

            txt_matricula.Enabled = (txt_declaratoria.Text == "") && Enabled;
            txt_direccion.Enabled = (txt_declaratoria.Text == "") && Enabled;
            btnGoPredio.Visible = !(txt_declaratoria.Text == "");
        }
        private void LoadDetail()
        {
            if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.CONSULTAR)) return;

            oBasic.fClearControls(pnlProyectoPredio);
            hdd_au_proyecto_predio.Value = hdd_au_proyecto_predio.Value.Trim() == "" ? "0" : hdd_au_proyecto_predio.Value;
            if (hdd_au_proyecto_predio.Value != "0")
            {
                DataSet dSet = oProyectosPredios.sp_s_proyecto_predio(hdd_au_proyecto_predio.Value);
                DataRow dRow = dSet.Tables[0].Rows.Count > 0 ? dSet.Tables[0].Rows[0] : dSet.Tables[0].NewRow();

                oBasic.fValueControls(pnlProyectoPredio, dRow);
            }
            hdd_au_proyecto_predio.Value = hdd_au_proyecto_predio.Value.Trim() == "" ? "0" : hdd_au_proyecto_predio.Value;
        }
        private void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.CONSULTAR, false, false)) return;

            ViewState["ST" + ControlID] = "Grid";
            gvProyectoPredio.DataSource = oProyectosPredios.sp_s_proyectos_predios_cod_proyecto(ProyectoID.ToString());
            gvProyectoPredio.DataBind();

            if(Chip != null && Chip != "")
            {
                foreach (GridViewRow row in gvProyectoPredio.Rows)
                {
                    if (gvProyectoPredio.DataKeys[row.RowIndex]["chip"].ToString() == Chip)
                        gvProyectoPredio.SelectedIndex = row.RowIndex;
                }
            }

            oBasic.FixPanel(divData, "ProyectoPredio", 0, pAdd: false, pEdit: false, pDelete: false);
        }
        private string SaveProyectoPredio()
        {
            string strResult;
            hdd_au_proyecto_predio.Value = hdd_au_proyecto_predio.Value == "" ? "0" : hdd_au_proyecto_predio.Value;
            ViewState["ST" + ControlID] = "Grid";

            if (hdd_au_proyecto_predio.Value == "0")
            {
                strResult = oProyectosPredios.sp_i_proyecto_predio( ProyectoID.ToString(), txt_chip.Text, txt_matricula.Text, txt_direccion.Text,
                                        chk_cumple_funcion_social.Checked, txt_observacion__predio.Text);
                if (oBasic.AlertUserControl(msgProyectoPredio, msgProyectoPredioMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i"))
                {
                    hdd_au_proyecto_predio.Value = hdd_au_proyecto_predio.Value == "0" ? (strResult.Split(':'))[1] : hdd_au_proyecto_predio.Value;
                }
            }
            else
            {
                strResult = oProyectosPredios.sp_u_proyecto_predio(hdd_au_proyecto_predio.Value, txt_chip.Text, txt_matricula.Text, txt_direccion.Text,
                                    chk_cumple_funcion_social.Checked, txt_observacion__predio.Text );
                oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u");
            }

            Enabled = false;
            EnabledControls();
            LoadControl();
            ViewControls();
            upProyectoPredioFoot.Update();
            upDetail.Update();

            return strResult;
        }
        private bool ValidateAccess(string section, string action, bool validateResponsible = false, bool requeridedResponsible = false)
        {
            string message = oPermisos.ValidateAccess(section, action, ResponsibleUserCode, validateResponsible, requeridedResponsible);

            if (message == "")
                return true;

            MessageInfo.ShowMessage(message);
            return false;
        }
        private void ViewAdd()
        {
            if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.INSERTAR)) return;

            ViewState["ST" + ControlID] = "Detail";
            hdd_au_proyecto_predio.Value = "0";
            Enabled = true;
            LoadControl();

            oBasic.FixPanel(divData, "ProyectoPredio", 2);
            EnabledControls();
        }
        private void ViewControls()
        {
            ViewState["ST" + ControlID] = ViewState["ST" + ControlID] ?? "Grid";

            pnlGrid.Visible = false;
            pnlDetail.Visible = false;

            switch (ViewState["ST" + ControlID].ToString())
            {
                case "Grid":
                    ViewState["IndexProyectoPredio"] = ViewState["IndexProyectoPredio"] ?? "0";
                    ViewState["SortExpProyectoPredio"] = ViewState["SortExpProyectoPredio"] ?? "ProyectoPredio_inicial";
                    ViewState["SortDirProyectoPredio"] = ViewState["SortDirProyectoPredio"] ?? "DESC";
                    pnlGrid.Visible = true;
                    oBasic.FixPanel(divData, "ProyectoPredio", 0, pList: false, pAdd: false, pEdit: false, pDelete: false);
                    upProyectoPredio.Update();
                    break;

                case "Detail":
                    pnlDetail.Visible = true;
                    EnabledControls();
                    oBasic.FixPanel(divData, "ProyectoPredio", Enabled ? 2 : 0, pList: true);
                    upDetail.Update();
                    break;
            }
        }

        #endregion
    }
}