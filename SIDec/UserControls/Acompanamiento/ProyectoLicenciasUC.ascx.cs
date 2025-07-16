using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec.UserControls.Acompanamiento
{
    public partial class ProyectoLicenciasUC : UserControl
    {

        private readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL(); 
        private readonly PROYECTOSLICENCIAS_DAL oProyectoLicencias = new PROYECTOSLICENCIAS_DAL();

        private readonly clBasic oBasic = new clBasic();
        private readonly clFile oFile = new clFile();
        private readonly clLog oLog = new clLog();
        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clPermisos oPermisos = new clPermisos();

        private const string _SOURCEPAGE = "ProyectoLicenciasUC";

        public delegate void OnUserControlExceptionEventHandler(object sender, Exception ex);
        public event OnUserControlExceptionEventHandler UserControlException;

        public delegate void OnViewDocEventHandler(object sender);
        public event OnViewDocEventHandler ViewDoc;

        #region Propiedades
        /// <summary>
        /// Identificador del registro del ProyectoLicencia
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
                return (Session[ControlID + ".ProyectoLicencias.Enabled"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".ProyectoLicencias.Enabled"] = value ? "1" : "0";
            }
        }
        /// <summary>
        /// Identificacion del proyecto al que se asocian los proyectosProyectoLicencias
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
        /// Identificador del registro del ProyectoLicencia
        /// </summary>
        public int ProyectoLicenciaID
        {
            get
            {
                return (Int32.TryParse(hddProyectoLicenciaPrimary.Value, out int id) ? id : 0);
            }
            set
            {
                hddProyectoLicenciaPrimary.Value = value.ToString();
                hdd_au_proyecto_licencia.Value = hddProyectoLicenciaPrimary.Value;
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
            RegisterScript();
            if (!IsPostBack)
            {
                LoadDropDowns();
                Initialize();
            }
            ViewControls();
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            switch (ViewState["ST" + ControlID])
            {
                case "Detail":
                    if (hdd_au_proyecto_licencia.Value != "0")
                    {
                        MessageBox1.ShowConfirmation("Detail", "¿Está seguro de actualizar la información?", type: "warning");
                    }
                    else
                        MessageBox1.ShowConfirmation("Detail", "¿Está seguro de continuar con la acción solicitada?");
                    break;
            }
        }
        protected void btnProyectoLicenciaAdd_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgProyectoLicenciaMain, "", "0");
            ViewAdd();
        }
        protected void btnProyectoLicenciaAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgProyectoLicenciaMain, "", "0");

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    LoadGrid();
                    break;
                case "Editar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.EDITAR)) return;
                    Enabled = true;
                    break;
                case "Agregar":
                    ViewAdd();
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.ELIMINAR)) return;

                    MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar este registro?", type: "danger");
                    return;
            }
            ViewControls();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgProyectoLicenciaMain, "", "0");
            ViewState["ST" + ControlID] = "Grid";

            Enabled = false;
            ViewControls();
        }
        protected void gvProyectoLicencia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                oBasic.AlertMain(msgProyectoLicenciaMain, "", "0");
                if (rowIndex >= gvProyectoLicencia.Rows.Count)
                    rowIndex = 0;
                hdd_idorigen.Value = rowIndex >= 0 ? gvProyectoLicencia.DataKeys[rowIndex]["idorigen"].ToString() : "0";
                hdd_au_proyecto_licencia.Value = rowIndex >= 0 ? gvProyectoLicencia.DataKeys[rowIndex]["au_proyecto_licencia"].ToString() : "0";

                switch (e.CommandName)
                {
                    case "Select":
                        ViewState["ST" + ControlID] = "Grid";
                        break;
                    case "_Detail":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.CONSULTAR, false, false)) return;

                        ViewState["ST" + ControlID] = "Detail";
                        Enabled = false;
                        LoadDetail();
                        break;
                    case "_Delete":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.ELIMINAR, true, false)) return;

                        ViewState["ST" + ControlID] = "Detail";
                        Enabled = false;
                        LoadDetail();
                        MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar este registro?", type: "danger");
                        break;
                    case "_OpenFile":
                        hdd_ruta_licencia.Value = gvProyectoLicencia.DataKeys[rowIndex]["ruta_licencia"].ToString();
                        ViewDocument();
                        break;
                    case "_Edit":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.EDITAR, true)) return;

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
        protected void gvProyectoLicencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvProyectoLicencia.DataKeys[e.Row.DataItemIndex]["idorigen"].ToString() != "1")
                {
                    e.Row.ForeColor = System.Drawing.Color.FromArgb(55, 55, 55);
                    e.Row.BackColor = System.Drawing.Color.FromArgb(241, 242, 243);
                    e.Row.Font.Italic = true;
                }
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvProyectoLicencia, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvProyectoLicencia_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProyectoLicencia.PageIndex = e.NewPageIndex;
            LoadGrid();
            ViewState["IndexProyectoLicencia"] = ((gvProyectoLicencia.PageSize * gvProyectoLicencia.PageIndex) + gvProyectoLicencia.PageIndex - 1).ToString();
        }
        protected void lblPdfLicense_Click(object sender, EventArgs e)
        {
            ViewDocument();
        }
        protected void MessageBox_Accept(string key)
        {
            try
            {
                string strResult = "";
                switch (key)
                {
                    case "Detail":
                        strResult = SaveProyectoLicencia();
                        break;
                    case "Delete":
                        strResult = DeleteProyectoLicencia();
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
        private string DeleteProyectoLicencia()
        {
            hdd_au_proyecto_licencia.Value = hdd_au_proyecto_licencia.Value == "" ? "0" : hdd_au_proyecto_licencia.Value;

            string strResult = oProyectoLicencias.sp_d_proyecto_licencia(hdd_au_proyecto_licencia.Value);
            if (oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d"))
            {
                ViewState["ST" + ControlID] = "Grid";
                LoadControl();
                ViewControls();
                upProyectoLicenciaFoot.Update();
            }

            return strResult;
        }
        private void Initialize()
        {
            cal_fecha_ejecutoria.StartDate = new DateTime(2008, 1, 1);
            rvfecha_ejecutoria.MinimumValue = (new DateTime(2008, 1, 1)).ToString("yyyy-MM-dd");
            cal_fecha_ejecutoria.EndDate = DateTime.Today;
            rvfecha_ejecutoria.MaximumValue = (DateTime.Today).ToString("yyyy-MM-dd");
        }
        private void LoadDetail()
        {
            if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.CONSULTAR)) return;

            oBasic.fClearControls(pnlProyectoLicencia);
            hdd_au_proyecto_licencia.Value = hdd_au_proyecto_licencia.Value.Trim() == "" ? "0" : hdd_au_proyecto_licencia.Value;
            if (hdd_au_proyecto_licencia.Value != "0")
            {
                DataSet dSet = oProyectoLicencias.sp_s_proyectos_licencia(hdd_idorigen.Value, hdd_au_proyecto_licencia.Value); 
                DataRow dRow = dSet.Tables[0].Rows.Count > 0 ? dSet.Tables[0].Rows[0] : dSet.Tables[0].NewRow();

                oBasic.fValueControls(pnlProyectoLicencia, dRow);
            }
            lblPdfLicense.Visible = hdd_ruta_licencia.Value != "";
            lblInfoFileLicense.Text = "";
            hdd_au_proyecto_licencia.Value = hdd_au_proyecto_licencia.Value.Trim() == "" ? "0" : hdd_au_proyecto_licencia.Value;
        }
        private void LoadDropDowns()
        {
            LoadDropDownIdentidad(ddlb_id_fuente_informacion, "15");
            LoadDropDownIdentidad(ddl_id_tipo_licencia, "16");
            LoadDropDownIdentidad(ddlb_id_obligacion_VIS, "17");
            LoadDropDownIdentidad(ddlb_id_obligacion_VIP, "18");
        }
        private void LoadDropDownIdentidad(DropDownList ddl, string id)
        {
            DataSet ds = oIdentidades.sp_s_identidad_id_categoria(id);
            ddl.DataSource = ds;
            ddl.DataTextField = "nombre_identidad";
            ddl.DataValueField = "id_identidad";
            ddl.DataBind();
        }
        private void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.CONSULTAR, false, false)) return;

            ViewState["ST" + ControlID] = "Grid";
            gvProyectoLicencia.DataSource = oProyectoLicencias.sp_s_proyectos_licencias_cod_proyecto(ProyectoID.ToString());
            gvProyectoLicencia.DataBind();

            oBasic.FixPanel(divData, "ProyectoLicencia", 0, pAdd: false, pEdit: false, pDelete: false);
        }
        private void RegisterScript()
        {
            Page.Form.Enctype = "multipart/form-data";
            lblLoadLicense.Attributes.Add("onclick", "$(\"input[ID*='" + fuLoadLicense.ClientID + "']\").click();return false;");
        }
        private string SaveProyectoLicencia()
        {
            string strResult;
            hdd_au_proyecto_licencia.Value = hdd_au_proyecto_licencia.Value == "" ? "0" : hdd_au_proyecto_licencia.Value;
            ViewState["ST" + ControlID] = "Grid";

            var fileName = "";
            if (fuLoadLicense.HasFile)
            {
                string contentType = fuLoadLicense.ContentType;
                if (contentType == "application/pdf")
                {
                    fileName = "LCPY_" + ProyectoID.ToString() + "_" + Guid.NewGuid() + Path.GetExtension(fuLoadLicense.FileName);
                    var pathFile = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);
                    try
                    {
                        fuLoadLicense.SaveAs(pathFile);
                    }
                    catch (Exception e)
                    {
                        oLog.RegistrarLogError("Error subiendo archivo " + e.Message + ":" + fuLoadLicense.FileName + ":::" + fuLoadLicense.PostedFile.ContentLength, _SOURCEPAGE, "SaveProyectoLicencia");
                    }
                }
            }

            if (hdd_au_proyecto_licencia.Value == "0")
            {
                strResult = oProyectoLicencias.sp_i_proyecto_licencia(
                    ProyectoID.ToString(),
                    oBasic.fInt(ddlb_id_fuente_informacion),
                    oBasic.fInt(ddl_id_tipo_licencia),
                    txt_licencia.Text,
                    oBasic.fInt(ddl_curador),
                    txt_fecha_ejecutoria.Text,
                    oBasic.fInt(txt_termino_vigencia_meses),
                    txt_plano_urbanistico_aprobado.Text,
                    txt_nombreproyecto.Text,
                    oBasic.fDec(txt_area_bruta__licencia),
                    oBasic.fDec(txt_area_neta),
                    oBasic.fDec(txt_area_util__licencia),
                    oBasic.fDec(txt_area_cesion_zonas_verdes),
                    oBasic.fDec(txt_area_cesion_vias),
                    oBasic.fDec(txt_area_cesion_eq_comunal),
                    oBasic.fInt(txt_porcentaje_ejecucion_urbanismo),
                    oBasic.fInt(ddlb_id_obligacion_VIS),
                    oBasic.fInt(ddlb_id_obligacion_VIP),
                    oBasic.fDec(txt_area_terreno_VIS),
                    oBasic.fDec(txt_area_terreno_no_VIS),
                    oBasic.fDec(txt_area_terreno_VIP),
                    oBasic.fDec(txt_area_construida_VIS),
                    oBasic.fDec(txt_area_construida_no_VIS),
                    oBasic.fDec(txt_area_construida_VIP),
                    oBasic.fInt(txt_porcentaje_obligacion_VIS),
                    oBasic.fInt(txt_porcentaje_obligacion_VIP),
                    oBasic.fInt(txt_unidades_vivienda_VIS),
                    oBasic.fInt(txt_unidades_vivienda_no_VIS),
                    oBasic.fInt(txt_unidades_vivienda_VIP),
                    oBasic.fDec(txt_area_comercio),
                    oBasic.fDec(txt_area_oficina),
                    oBasic.fDec(txt_area_institucional),
                    oBasic.fDec(txt_area_industria),
                    oBasic.fDec(txt_area_lote),
                    oBasic.fDec(txt_area_sotano),
                    oBasic.fDec(txt_area_semisotano),
                    oBasic.fDec(txt_area_primer_piso),
                    oBasic.fDec(txt_area_pisos_restantes),
                    oBasic.fDec(txt_area_construida_total),
                    oBasic.fDec(txt_area_libre_primer_piso),
                    oBasic.fInt(txt_porcentaje_ejecucion_construccion),
                    txt_observacion__licencia.Text,
                    fileName
                );
                if (oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i"))
                {
                    hdd_au_proyecto_licencia.Value = hdd_au_proyecto_licencia.Value == "0" ? (strResult.Split(':'))[1] : hdd_au_proyecto_licencia.Value;
                }
            }
            else
            {
                strResult = oProyectoLicencias.sp_u_proyecto_licencia(
                    hdd_au_proyecto_licencia.Value,
                    oBasic.fInt(ddlb_id_fuente_informacion),
                    oBasic.fInt(ddl_id_tipo_licencia),
                    txt_licencia.Text,
                    oBasic.fInt(ddl_curador),
                    txt_fecha_ejecutoria.Text,
                    oBasic.fInt(txt_termino_vigencia_meses),
                    txt_plano_urbanistico_aprobado.Text,
                    txt_nombreproyecto.Text,
                    oBasic.fDec(txt_area_bruta__licencia),
                    oBasic.fDec(txt_area_neta),
                    oBasic.fDec(txt_area_util__licencia),
                    oBasic.fDec(txt_area_cesion_zonas_verdes),
                    oBasic.fDec(txt_area_cesion_vias),
                    oBasic.fDec(txt_area_cesion_eq_comunal),
                    oBasic.fInt(txt_porcentaje_ejecucion_urbanismo),
                    oBasic.fInt(ddlb_id_obligacion_VIS),
                    oBasic.fInt(ddlb_id_obligacion_VIP),
                    oBasic.fDec(txt_area_terreno_VIS),
                    oBasic.fDec(txt_area_terreno_no_VIS),
                    oBasic.fDec(txt_area_terreno_VIP),
                    oBasic.fDec(txt_area_construida_VIS),
                    oBasic.fDec(txt_area_construida_no_VIS),
                    oBasic.fDec(txt_area_construida_VIP),
                    oBasic.fInt(txt_porcentaje_obligacion_VIS),
                    oBasic.fInt(txt_porcentaje_obligacion_VIP),
                    oBasic.fInt(txt_unidades_vivienda_VIS),
                    oBasic.fInt(txt_unidades_vivienda_no_VIS),
                    oBasic.fInt(txt_unidades_vivienda_VIP),
                    oBasic.fDec(txt_area_comercio),
                    oBasic.fDec(txt_area_oficina),
                    oBasic.fDec(txt_area_institucional),
                    oBasic.fDec(txt_area_industria),
                    oBasic.fDec(txt_area_lote),
                    oBasic.fDec(txt_area_sotano),
                    oBasic.fDec(txt_area_semisotano),
                    oBasic.fDec(txt_area_primer_piso),
                    oBasic.fDec(txt_area_pisos_restantes),
                    oBasic.fDec(txt_area_construida_total),
                    oBasic.fDec(txt_area_libre_primer_piso),
                    oBasic.fInt(txt_porcentaje_ejecucion_construccion),
                    txt_observacion__licencia.Text,
                    fileName
                );
            }

            Enabled = false;

            oBasic.EnableControls(pnlProyectoLicencia,Enabled,true);
            LoadControl();
            ViewControls();
            upProyectoLicenciaFoot.Update();
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
            if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.INSERTAR)) return;

            ViewState["ST" + ControlID] = "Detail";
            hdd_au_proyecto_licencia.Value = "0";
            Enabled = true;
            LoadControl();
            lblPdfLicense.Visible = false;
            lblInfoFileLicense.Text = "";

            oBasic.FixPanel(divData, "ProyectoLicencia", 2);
            oBasic.EnableControls(pnlProyectoLicencia, Enabled, true);
        }
        private void ViewControls()
        {
            ViewState["ST" + ControlID] = ViewState["ST" + ControlID] ?? "Grid";

            pnlGrid.Visible = false;
            pnlDetail.Visible = false;

            switch (ViewState["ST" + ControlID].ToString())
            {
                case "Grid":
                    ViewState["IndexProyectoLicencia"] = ViewState["IndexProyectoLicencia"] ?? "0";
                    ViewState["SortExpProyectoLicencia"] = ViewState["SortExpProyectoLicencia"] ?? "ProyectoLicencia_inicial";
                    ViewState["SortDirProyectoLicencia"] = ViewState["SortDirProyectoLicencia"] ?? "DESC";
                    pnlGrid.Visible = true;
                    oBasic.FixPanel(divData, "ProyectoLicencia", 0, pList: false, pAdd: false, pEdit: false, pDelete: false);
                    upProyectoLicencia.Update();
                    break;

                case "Detail":
                    pnlDetail.Visible = true;
                    oBasic.EnableControls(pnlProyectoLicencia, Enabled, true);
                    lblPdfLicense.Enabled = true;
                    lblLoadLicense.Visible = Enabled;
                    oBasic.FixPanel(divData, "ProyectoLicencia", Enabled ? 2 : 0, pList: true, pEdit: hdd_idorigen.Value == "1", pDelete: hdd_idorigen.Value == "1");
                    upDetail.Update();
                    break;
            }
        }
        private void ViewDocument()
        {
            string fileName = hdd_ruta_licencia.Value;
            string pathFile = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);
            oFile.GetPath(pathFile);
            ViewDoc?.Invoke(this);
        }

        #endregion
    }
}