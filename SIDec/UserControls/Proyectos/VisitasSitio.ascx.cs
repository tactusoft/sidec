using GLOBAL.DAL;
using GLOBAL.PERMISOS;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;
using tipoArchivo = GLOBAL.CONST.clConstantes.TipoArchivo;

namespace SIDec.UserControls.Proyectos
{
    public partial class VisitasSitio : UserControl
    {
        private readonly VISITAS_SITIO_DAL oVisitasSitio = new VISITAS_SITIO_DAL();

        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();

        private const string _SOURCEPAGE = "VisitasSitio";

        public delegate void OnSaveEventHandler(object sender);
        public event OnSaveEventHandler Save;

        public delegate void OnUserControlExceptionEventHandler(object sender, Exception ex);
        public event OnUserControlExceptionEventHandler UserControlException;

        public delegate void OnViewDocEventHandler(object sender);
        public event OnViewDocEventHandler ViewDoc;



        #region Propiedades
        /// <summary>
        /// Identificador del instancia del control para evitar solapar las variables de sesión
        /// </summary>
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
                return (Session[ControlID + ".VisitasSitio.Enabled"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".VisitasSitio.Enabled"] = value ? "1" : "0";
            }
        }
        /// <summary>
        /// Ruta de la carpeta donde se guardan los archivos de las evidencias de las visitas
        /// </summary>
        public string FilePath
        {
            get
            {
                return Session[ControlID + ".VisitasSitio.FilePath"].ToString();
            }
            set
            {
                Session[ControlID + ".VisitasSitio.FilePath"] = value;
            }
        }
        /// <summary>
        /// Prefijo con el que se almacena los archivos de las evidencias
        /// </summary>
        public tipoArchivo Prefix
        {
            get
            {
                return (tipoArchivo)(Session[ControlID + ".VisitasSitio.Prefix"] ?? tipoArchivo.ND);
            }
            set
            {
                Session[ControlID + ".VisitasSitio.Prefix"] = value;
            }
        }
        /// <summary>
        /// Id del registro de la tabla con la que se relacionan los archivos
        /// </summary>
        public int ReferenceID
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
        /// <summary>
         /// Identificador del registro de la visita 
         /// </summary>
        public int VisitaID
        {
            get
            {
                return (Int32.TryParse(hddVisitaPrimary.Value, out int id) ? id : 0);
            }
            set
            {
                hddVisitaPrimary.Value = value.ToString();
                hdd_idvisita_sitio.Value = hddVisitaPrimary.Value;
            }
        }
        #endregion



        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }

            ViewControls();
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            switch (ViewState["ST" + ControlID])
            {
                case "Detail":
                    if (hdd_idvisita_sitio.Value != "0")
                    {
                        MessageBox1.ShowConfirmation("Detail", "¿Está seguro de actualizar la información?", type: "warning");
                    }
                    else
                        MessageBox1.ShowConfirmation("Detail", "¿Está seguro de continuar con la acción solicitada?");
                    break;
            }
        }
        protected void btnVisitasSitioAdd_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgVisitasSitioMain, "", "0");
            ViewAdd();
        }
        protected void btnVisitasSitioAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgVisitasSitioMain, "", "0");

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    LoadGrid();
                    break;
                case "Editar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_VISITAS, cnsAction.EDITAR, true)) return;

                    Enabled = true;
                    break;
                case "Agregar":
                    ViewAdd();
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_VISITAS, cnsAction.ELIMINAR, true, false)) return;

                    MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar este registro?", type: "danger");
                    return;
            }
            ViewControls();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgVisitasSitioMain, "", "0");

            Enabled = false;
            hdd_idvisita_sitio.Value = VisitaID.ToString();
            if (VisitaID == 0)
                ViewState["ST" + ControlID] = "Grid";
            else
                ViewState["ST" + ControlID] = "Detail";
            LoadDetail();
            ViewControls();

        }
        protected void gvVisitasSitio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                oBasic.AlertMain(msgVisitasSitioMain, "", "0");
                if (rowIndex >= gvVisitasSitio.Rows.Count)
                    rowIndex = 0;
                hdd_idvisita_sitio.Value = rowIndex >= 0 ? gvVisitasSitio.DataKeys[rowIndex]["idvisita_sitio"].ToString() : "0";

                switch (e.CommandName)
                {
                    case "Select":
                        ViewState["ST" + ControlID] = "Grid";
                        break;
                    case "_Detail":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_VISITAS, cnsAction.CONSULTAR, false, false)) return;
                        
                        ViewState["ST" + ControlID] = "Detail";
                        Enabled = false;
                        LoadDetail();
                        break;
                    default:
                        return;
                }
            }
            ViewControls();
        }
        protected void gvVisitasSitio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvVisitasSitio, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvVisitasSitio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVisitasSitio.PageIndex = e.NewPageIndex;
            LoadGrid();
            ViewState["IndexVisitasSitio"] = ((gvVisitasSitio.PageSize * gvVisitasSitio.PageIndex) + gvVisitasSitio.PageIndex - 1).ToString();
        }
        protected void MessageBox_Accept(string key)
        {
            try
            {
                string strResult = "";
                switch (key)
                {
                    case "Detail":
                        strResult = SaveVisitaSitio();
                        Save?.Invoke(this);
                        break;
                    case "Delete":
                        strResult = DeleteVisitaSitio();
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
        protected void ucImagenes_UserControlException(object sender, Exception ex)
        {
            UserControlException?.Invoke(sender, ex);
        }
        protected void ucImagenes_ViewDoc(object sender)
        {
            ViewDoc?.Invoke(this);
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
        private string DeleteVisitaSitio()
        {
            hdd_idvisita_sitio.Value = hdd_idvisita_sitio.Value == "" ? "0" : hdd_idvisita_sitio.Value;

            string strResult = oVisitasSitio.sp_d_visita_sitio(hdd_idvisita_sitio.Value);
            if (oBasic.AlertUserControl(msgVisitasSitio, msgVisitasSitioMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d"))
            {
                ViewState["ST" + ControlID] = "Grid";
                LoadControl();
                ViewControls();
                upVisitasSitioFoot.Update();
            }

            return strResult;
        }
        private void Initialize()
        {
            ViewState["IndexVisitasSitio"] = "0";
            ViewState["SortExpVisitasSitio"] = "fec_inicio";
            ViewState["SortDirVisitasSitio"] = "ASC";
            ViewState["ST" + ControlID] = "Grid";

            ce_fecha_visita.StartDate = new DateTime(2000, 1, 1);
            rv_fecha_visita.MinimumValue = (new DateTime(2000, 1, 1)).ToString("yyyy-MM-dd");
            ce_fecha_visita.EndDate = DateTime.Today;
            rv_fecha_visita.MaximumValue = (DateTime.Today).ToString("yyyy-MM-dd");
        }
        private void LoadDetail()
        {
            if (!ValidateAccess(cnsSection.PROY_ASOC_VISITAS, cnsAction.CONSULTAR, false, false)) return;

            oBasic.fClearControls(pnlVisita_sitio);
            hdd_idvisita_sitio.Value = hdd_idvisita_sitio.Value.Trim() == "" ? "0" : hdd_idvisita_sitio.Value;
            if (hdd_idvisita_sitio.Value != "0")
            {
                DataSet dSet = oVisitasSitio.sp_s_visita_sitio_consultar(hdd_idvisita_sitio.Value);
                DataRow dRow = dSet.Tables[0].Rows[0];

                oBasic.fValueControls(pnlVisita_sitio, dRow);

                ucImagenes.SectionPermission = cnsSection.PROY_ASOC_VISITAS;
                ucImagenes.Prefix = this.Prefix;
                ucImagenes.FilePath = this.FilePath;
                ucImagenes.ReferenceID = Convert.ToInt32(hdd_idvisita_sitio.Value);
                ucImagenes.ArchivoID = Convert.ToInt32(("0" + (dRow["idarchivo"] ?? "-1")).ToString());
                ucImagenes.Enabled = true;
                ucImagenes.LoadGrid();

                upImagenes.Update();
            }
        }
        private void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.PROY_ASOC_VISITAS, cnsAction.CONSULTAR, false, false)) return;

            ViewState["ST" + ControlID] = "Grid";
            gvVisitasSitio.DataSource = oVisitasSitio.sp_s_visitas_sitio(ReferenceID.ToString());
            gvVisitasSitio.DataBind();

            oBasic.FixPanel(divData, "VisitasSitio", 0, pAdd: false, pEdit: false, pDelete: false);
        }
        private string SaveVisitaSitio()
        {
            string strResult;
            hdd_idvisita_sitio.Value = hdd_idvisita_sitio.Value == "" ? "0" : hdd_idvisita_sitio.Value;
            int idarchivo = 0;
            ViewState["ST" + ControlID] = "Grid";

            if (hdd_idvisita_sitio.Value == "0")
            {
                strResult = oVisitasSitio.sp_i_visita_sitio(ReferenceID.ToString(), oBasic.fDateTime(txt_fecha_visita), txt_observaciones.Text.Trim(), idarchivo);
                if (oBasic.AlertUserControl(msgVisitasSitio, msgVisitasSitioMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i"))
                {
                    hdd_idvisita_sitio.Value = hdd_idvisita_sitio.Value == "0" ? (strResult.Split(':'))[1] : hdd_idvisita_sitio.Value;
                    ViewState["ST" + ControlID] = "Detail";
                }
            }
            else
            {
                strResult = oVisitasSitio.sp_u_visita_sitio(hdd_idvisita_sitio.Value, oBasic.fDateTime(txt_fecha_visita), txt_observaciones.Text.Trim());
                oBasic.AlertUserControl(msgVisitasSitio, msgVisitasSitioMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u");
            }

            Enabled = false;
            
            oBasic.EnableControls(pnlVisita_sitio, false, true);
            LoadControl();
            ViewControls();
            upVisitasSitioFoot.Update();
            upDetail.Update();

            return strResult;
        }
        private bool ValidateAccess(string section, string action, bool validateResponsible = false, bool requeridedResponsible = true)
        {
            string message = oPermisos.ValidateAccess(section, action, ResponsibleUserCode, validateResponsible, requeridedResponsible);

            if (message == "")
                return true;

            MessageInfo.ShowMessage(message);
            return false;
        }
        private void ViewAdd()
        {
            if (!ValidateAccess(cnsSection.PROY_ASOC_VISITAS, cnsAction.INSERTAR, true)) return;

            ViewState["ST" + ControlID] = "Detail";
            hdd_idvisita_sitio.Value = "0";
            Enabled = true;
            LoadControl();

            oBasic.FixPanel(divData, "VisitasSitio", 2);
            oBasic.EnableControls(pnlVisita_sitio, true, true);
            pnlImagenes.Visible = false;
            upImagenes.Update();
        }
        private void ViewControls()
        {
            ViewState["ST" + ControlID] = ViewState["ST" + ControlID] ?? "Grid";

            pnlGrid.Visible = false;
            pnlDetail.Visible = false;
            pnlImagenes.Visible = false;

            switch (ViewState["ST" + ControlID].ToString())
            {
                case "Grid":
                    ViewState["IndexVisitasSitio"] = ViewState["IndexVisitasSitio"] ?? "0";
                    ViewState["SortExpVisitasSitio"] = ViewState["SortExpVisitasSitio"] ?? "fec_seguimiento";
                    ViewState["SortDirVisitasSitio"] = ViewState["SortDirVisitasSitio"] ?? "ASC";
                    pnlGrid.Visible = true;
                    oBasic.FixPanel(divData, "VisitasSitio", 0, pList: false, pAdd: false, pEdit: false, pDelete: false);
                    upVisitasSitio.Update();
                    break;

                case "Detail":
                    pnlDetail.Visible = true;
                    pnlImagenes.Visible = true && hdd_idvisita_sitio.Value != "0";
                    oBasic.EnableControls(pnlVisita_sitio, Enabled, true);
                    oBasic.FixPanel(divData, "VisitasSitio", Enabled ? 2 : 0, pList: true);

                    upDetail.Update();
                    break;
            }
        }
        #endregion
    }
}