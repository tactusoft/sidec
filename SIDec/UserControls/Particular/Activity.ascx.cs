using GLOBAL.CONST;
using GLOBAL.DAL;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec.UserControls.Particular
{
    public partial class Activity : UserControl
    {
        private readonly BANCOACTIVIDADES_DAL oActividades = new BANCOACTIVIDADES_DAL();
        private readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
        private readonly BancoActividadEntidades_DAL oActividadEntidades = new BancoActividadEntidades_DAL();

        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();
        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clUtil oUtil = new clUtil();


        private const string _SOURCEPAGE = "Activity";
        private const int NUM_COLUMN_SEMAPHORE = 8;

        public delegate void OnListEventHandler(object sender);
        public event OnListEventHandler ToList;

        public delegate void OnSaveEventHandler(object sender);
        public event OnSaveEventHandler Save;

        #region Propiedades
        public string CodUsuario
        {
            get
            {
                return (Session[ControlID + ".Activity.CodUsuario"] ?? "").ToString();
            }
            set
            {
                Session[ControlID + ".Activity.CodUsuario"] = value;
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
                ddl_Entidad.ControlID = value + ".ActivityListBox";
            }
        }
        public bool Enabled
        {
            get
            {
                return (Session[ControlID + ".Activity.Enabled"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".Activity.Enabled"] = value ? "1" : "0";
            }
        }
        public int IdBanco
        {
            get
            {
                return (Int32.TryParse(hddIdBanco.Value, out int id) ? id : 0);
            }
            set
            {
                hddIdBanco.Value = value.ToString();
                LoadGrid();
            }
        }
        public string Section
        {
            get
            {
                return (Session[ControlID + ".Activity.Section"] ?? "").ToString();
            }
            set
            {
                Session[ControlID + ".Activity.Section"] = value;
            }
        }
        public string TipoProyecto
        {
            set
            {
                LoadDropDowns(value);
            }
        }
        #endregion



        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                LoadDropDowns();
            }

            ViewControls();
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            switch (ViewState["ST" + ControlID])
            {
                case "Detail":
                    if (hdd_idbanco_actividad.Value != "0")
                    {
                        MessageBox1.ShowConfirmation("Detail", "¿Está seguro de actualizar la información?", type: "warning");
                    }
                    else
                        MessageBox1.ShowConfirmation("Detail", "¿Está seguro de continuar con la acción solicitada?");
                    break;
            }
        }
        protected void btnActivityAdd_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgActivityMain, "", "0");
            ViewAdd();
        }
        protected void btnActivityAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgActivityMain, "", "0");

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    if (ViewState["ST" + ControlID].ToString() == "Grid")
                        ToList?.Invoke(this);
                    else
                        LoadGrid();
                    break;
                case "Editar":
                    if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.EDITAR, true)) return;

                    Enabled = true;
                    oBasic.AlertSection(msgActivity, clConstantes.MSG_U, "warning");
                    break;
                case "Agregar":
                    oBasic.AlertMain(msgActivityMain, "", "0");
                    ViewAdd();
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.ELIMINAR, true, false)) return;

                    MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar este registro?", type: "danger");
                    oBasic.AlertSection(msgActivity, clConstantes.MSG_D, "danger");
                    return;
                case "Excel":
                    oBasic.AlertMain(msgActivityMain, "", "0");
                    LoadReportUcActivity();
                    break;
            }
            ViewControls();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgActivityMain, "", "0");
            ViewState["ST" + ControlID] = "Grid";

            Enabled = false;
            ViewControls();
        }
        protected void chk_activas_CheckedChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
        protected void gvActivity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvActivity.Rows.Count)
                    rowIndex = 0;
                hdd_idbanco_actividad.Value = rowIndex >= 0 ? gvActivity.DataKeys[rowIndex]["idbanco_actividad"].ToString() : "0";

                switch (e.CommandName)
                {
                    case "Select":
                        ViewState["ST" + ControlID] = "Grid";
                        break;
                    case "_Detail":
                        if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.CONSULTAR, false, false)) return;

                        ViewState["ST" + ControlID] = "Detail";
                        oBasic.AlertMain(msgActivityMain, "", "0");
                        Enabled = false;
                        LoadDetail();
                        break;
                    case "_Delete":
                        if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.ELIMINAR, true, false)) return;

                        ViewState["ST" + ControlID] = "Detail";
                        oBasic.AlertMain(msgActivityMain, "", "0");
                        Enabled = false;
                        LoadDetail();
                        MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar esta actividad?", type: "danger");
                        oBasic.AlertSection(msgActivity, clConstantes.MSG_D, "danger");
                        break;
                    case "_Edit":
                        if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.EDITAR, true)) return;

                        ViewState["ST" + ControlID] = "Detail";
                        oBasic.AlertMain(msgActivityMain, "", "0");
                        Enabled = true;
                        LoadDetail();
                        oBasic.AlertSection(msgActivity, clConstantes.MSG_U, "warning");
                        break;
                    default:
                        return;
                }
            }
            ViewControls();
        }
        protected void gvActivity_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvActivity.DataKeys[e.Row.DataItemIndex % gvActivity.PageSize]["activo"].ToString() != "1")
                {
                    e.Row.ForeColor = System.Drawing.Color.FromArgb(55, 55, 55);
                    e.Row.BackColor = System.Drawing.Color.FromArgb(241, 242, 243);
                    e.Row.Font.Italic = true;
                }

                TableCell tc = e.Row.Cells[NUM_COLUMN_SEMAPHORE];

                int dias_disponibles = Convert.ToInt32(gvActivity.DataKeys[e.Row.DataItemIndex % gvActivity.PageSize]["dias_disponibles"].ToString());
                double porcentajeesperado = Convert.ToDouble(gvActivity.DataKeys[e.Row.DataItemIndex % gvActivity.PageSize]["porcentajeesperado"].ToString());
                int semaforo = Convert.ToInt32(gvActivity.DataKeys[e.Row.DataItemIndex % gvActivity.PageSize]["semaforo"].ToString());
                if (Math.Abs(semaforo) == 2)
                    if (semaforo == 2)
                    {
                        tc.BackColor = System.Drawing.Color.LimeGreen;
                        tc.ForeColor = System.Drawing.Color.Black;
                        (tc.FindControl("lblSemaforo") as Label).Text = "OK, Trámite en términos";
                    }
                    else
                    {
                        tc.BackColor = System.Drawing.Color.DeepSkyBlue;
                        tc.ForeColor = System.Drawing.Color.Black;
                        (tc.FindControl("lblSemaforo") as Label).Text = "OK, Trámite fuera de términos";
                    }
                else
                {
                    if (semaforo == -1)
                        tc.ForeColor = System.Drawing.Color.Red;
                    else if (semaforo == 0)
                        tc.ForeColor = System.Drawing.Color.Yellow;
                    else
                        tc.ForeColor = System.Drawing.Color.Green;

                    (tc.FindControl("lblSemaforo") as Label).Text = "<i class='fas fa-circle'></i> " + dias_disponibles.ToString("N0") + (dias_disponibles == 1 || dias_disponibles == -1 ? " día" : " días");
                    if(porcentajeesperado > 0) (tc.FindControl("lblSemaforo") as Label).ToolTip = "Avance esperado del " + porcentajeesperado.ToString("N2") + " %";
                }
                

                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvActivity, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActivity.PageIndex = e.NewPageIndex;
            LoadGrid();
            ViewState["IndexActivity"] = ((gvActivity.PageSize * gvActivity.PageIndex) + gvActivity.PageIndex - 1).ToString();
        }
        protected void MessageBox_Accept(string key)
        {
            try
            {
                string strResult = "";
                switch (key)
                {
                    case "Detail":
                        strResult = Save_Actividad();
                        Save?.Invoke(this);
                        break;
                    case "Delete":
                        strResult = Delete_Actividad();
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
        private string Delete_Actividad()
        {
            hdd_idbanco_actividad.Value = hdd_idbanco_actividad.Value == "" ? "0" : hdd_idbanco_actividad.Value;

            string strResult = oActividades.sp_d_bancoactividades(hdd_idbanco_actividad.Value);
            if (oBasic.AlertUserControl(msgActivity, msgActivityMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d"))
            {
                ViewState["ST" + ControlID] = "Grid";
                LoadControl();
                ViewControls();
                upActivityFoot.Update();
            }

            return strResult;
        }
        private void Initialize()
        {
            ViewState["IndexActivity"] = "0";
            ViewState["SortExpActivity"] = "fec_inicio";
            ViewState["SortDirActivity"] = "ASC";
            ViewState["ST" + ControlID] = "Grid";

            cefecharadicado.StartDate = ce_fec_inicio.StartDate = ce_fec_culminacion.StartDate = ce_fec_finalizacion.StartDate = new DateTime(2000, 1, 1);
            rvfecharadicado.MinimumValue = rv_fec_inicio.MinimumValue = rv_fec_culminacion.MinimumValue = rv_fec_finalizacion.MinimumValue = (new DateTime(2000, 1, 1)).ToString("yyyy-MM-dd");
            cefecharadicado.EndDate = ce_fec_inicio.EndDate = ce_fec_culminacion.EndDate = ce_fec_finalizacion.EndDate = DateTime.Today.AddYears(50);
            rvfecharadicado.MaximumValue = rv_fec_inicio.MaximumValue = rv_fec_culminacion.MaximumValue = rv_fec_finalizacion.MaximumValue = (DateTime.Today.AddYears(50)).ToString("yyyy-MM-dd");

            Load_Entidad();
        }
        private void LoadDetail()
        {
            if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.CONSULTAR, false, false)) return;

            oBasic.fClearControls(pnlDetail);
            chk_activo.Checked = true;
            hdd_fec_inicio.Value = hdd_fec_culminacion.Value = hdd_fec_finalizacion.Value = "";
            txt_fec_inicio.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txt_porccompletado.Text = "0";

            hdd_idbanco_actividad.Value = hdd_idbanco_actividad.Value.Trim() == "" ? "0" : hdd_idbanco_actividad.Value;
            if (hdd_idbanco_actividad.Value != "0")
            {
                DataSet dSet = oActividades.sp_s_bancoactividad_consultar(hdd_idbanco_actividad.Value);
                ddl_Entidad.SetSelectedValues(null);
                if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = dSet.Tables[0].Rows[0];

                    oBasic.fValueControls(pnlDetail, dRow);
                    hdd_fec_inicio.Value = oUtil.ConvertToFechaDetalle(hdd_fec_inicio.Value);
                    hdd_fec_culminacion.Value = oUtil.ConvertToFechaDetalle(hdd_fec_culminacion.Value);
                    hdd_fec_finalizacion.Value = oUtil.ConvertToFechaDetalle(hdd_fec_finalizacion.Value);

                    Load_Entidad();
                }
            }
        }
        private void LoadDropDowns(string tipo = "")
        {
            LoadDropDownIdentidad(ddl_idEntidad, "75"); 
            if (tipo == "PE")
                LoadDropDownIdentidad(ddl_idestado_actividad, "55"); 
            else
                LoadDropDownIdentidad(ddl_idestado_actividad, "55", "3"); //3-ignore las opciones exclusivas de Proyectos estratégicos
        }
        private void LoadDropDownIdentidad(DropDownList ddl, string id, string filterIgnore = "")
        {
            DataView dv = oIdentidades.sp_s_identidad_id_categoria(id).Tables[0].DefaultView;
            dv.RowFilter = filterIgnore == "" ? "" : "opcion_identidad is null or opcion_identidad not in (" + filterIgnore + ")";
            ddl.Items.Clear();
            ddl.DataSource = dv;
            ddl.DataTextField = "nombre_identidad";
            ddl.DataValueField = "id_identidad";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("-- Seleccione opción"));
        }
        private void Load_Entidad()
        {
            DataSet ds = oIdentidades.sp_s_identidad_id_categoria("75");
            ddl_Entidad.LoadListBox(ds);
            string entidades = string.Empty;

            DataSet dsEntidad = oActividadEntidades.sp_s_bancoactividad_entidades(hdd_idbanco_actividad.Value);
            if (dsEntidad.Tables.Count > 0)
            {
                DataTable dt = dsEntidad.Tables[0];
                List<string> idEntidades = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    idEntidades.Add(dr["identidad"].ToString());
                    entidades += ", " + dr["entidad"].ToString();
                }
                ddl_Entidad.SetSelectedValues(idEntidades);
            }
        }
        private void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.CONSULTAR, false, false)) return;

            ViewState["ST" + ControlID] = "Grid";
            bool? activos = null;
            if (chk_activas.Checked) activos = true;
            gvActivity.DataSource = oActividades.sp_s_bancoactividades_listar(IdBanco, p_activo: activos);
            gvActivity.DataBind();
        }
        private void LoadReportUcActivity()
        {
            string FORMAT_TRAMITE = "Plantilla_Tramites.xlsx";
            string _plantilla_bancos = oVar.prPathFormatosOrigen.ToString() + FORMAT_TRAMITE;

            if (File.Exists(_plantilla_bancos))
            {
                //Archivo Excel del cual crear la copia:
                FileInfo templateFile = new FileInfo(_plantilla_bancos);
                string file_name = "Tramites_Consolidados.xlsx";

                using (ExcelPackage pck = new ExcelPackage(templateFile, true))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet ws = pck.Workbook.Worksheets["Tramite"];
                    LoadReportUcActivityProyectList(ws);
                    pck.Workbook.Calculate();
                    oUtil.fExcelSave(pck, file_name, false);
                }
            }
        }
        private void LoadReportUcActivityProyectList(ExcelWorksheet ws)
        {
            int currentRow = 3;
            DataSet ds = oActividades.sp_s_bancoactividad_reporte();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ws.Cells[currentRow.ToString() + ":" + currentRow.ToString()].Copy(ws.Cells[(currentRow + 1).ToString() + ":" + (currentRow + 1).ToString()]);
                    ws.InsertRow(currentRow + 1, 1);
                    int column = 0;
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["profesional_sgs"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["proyecto"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["radicado_sdht"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["fecha"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["entidad"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["area_dependencia"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["nombre_tramite"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["radicado"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["fecha_radicado"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["contacto"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["datos_contacto"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["solicitud"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["problematica"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["impacto"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["relacionamiento"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["gestion_sgs"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["gestion_adelantar"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["fecha_actividad"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["estado"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["desarrollo"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["compromiso"].ToString());
                    oUtil.fExcelWrite(ws, oUtil.getLetter(++column) + currentRow.ToString(), dr["fecha_cierre"].ToString(), isDate: true);

                    currentRow++;
                }
            }
        }
        private void LoadValidators(bool _enabled)
        {
            switch (Section)
            {
                case "Create":
                    rfv_estado_actividad.Enabled = _enabled;
                    rfv_actividad.Enabled = _enabled;
                    rev_fec_inicio.Enabled = _enabled;
                    rv_fec_inicio.Enabled = _enabled;
                    rfv_fec_inicio.Enabled = _enabled;
                    rev_fec_culminacion.Enabled = _enabled;
                    rv_fec_culminacion.Enabled = _enabled;
                    rfv_fec_culminacion.Enabled = _enabled;
                    rev_fec_finalizacion.Enabled = _enabled;
                    rv_fec_finalizacion.Enabled = _enabled;
                    rfv_porc_completado.Enabled = _enabled;
                    break;
            }
        }
        private string Save_Actividad()
        {
            string strResult;
            hdd_idbanco_actividad.Value = hdd_idbanco_actividad.Value == "" ? "0" : hdd_idbanco_actividad.Value;

            if (hdd_idbanco_actividad.Value == "0")
            {
                strResult = oActividades.sp_i_bancoactividades(IdBanco, oBasic.fInt(ddl_idestado_actividad), txt_actividad.Text.Trim(), oBasic.fInt(ddl_idEntidad),
                                            txt_dependencia.Text.Trim(), txt_radicado.Text.Trim(), oBasic.fDateTime(txt_fecha_radicado), txt_encargado.Text.Trim(),
                                            txt_datos_contacto.Text.Trim(), txt_solicitud.Text.Trim(), txt_problematica.Text.Trim(),
                                            txt_impacto.Text.Trim(), txt_gestion_sgs.Text.Trim(),txt_gestion_adelantar.Text.Trim(),
                                            oBasic.fDateTime(txt_fec_inicio), oBasic.fDateTime(txt_fec_culminacion), oBasic.fDateTime(txt_fec_finalizacion),
                                            chk_Clave.Checked, chk_activo.Checked, null, null, oBasic.fDec(txt_porccompletado));

                if (oBasic.AlertUserControl(msgActivity, msgActivityMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i"))
                {
                    hdd_idbanco_actividad.Value = hdd_idbanco_actividad.Value == "0" ? (strResult.Split(':'))[1] : hdd_idbanco_actividad.Value;
                }
                Save_ActividadEntidades(oBasic.fInt(hdd_idbanco_actividad), "i");
            }
            else
            {
                string fechainicio = hdd_fec_inicio.Value.Length > 0 ? hdd_fec_inicio.Value : oBasic.fDateTime(txt_fec_inicio);
                string fechaculminacion = hdd_fec_culminacion.Value.Length > 0 ? hdd_fec_culminacion.Value : oBasic.fDateTime(txt_fec_culminacion);
                string fechafinalizacion = hdd_fec_finalizacion.Value.Length > 0 ? hdd_fec_finalizacion.Value : oBasic.fDateTime(txt_fec_finalizacion);
                strResult = oActividades.sp_u_bancoactividades(Convert.ToInt32(hdd_idbanco_actividad.Value), oBasic.fInt(ddl_idestado_actividad), txt_actividad.Text.Trim(), 
                                            oBasic.fInt(ddl_idEntidad), txt_dependencia.Text.Trim(), txt_radicado.Text.Trim(), oBasic.fDateTime(txt_fecha_radicado), 
                                            txt_encargado.Text.Trim(), txt_datos_contacto.Text.Trim(), txt_solicitud.Text.Trim(), txt_problematica.Text.Trim(),
                                            txt_impacto.Text.Trim(), txt_gestion_sgs.Text.Trim(), txt_gestion_adelantar.Text.Trim(),
                                            fechainicio, fechaculminacion, fechafinalizacion, 
                                            chk_Clave.Checked, chk_activo.Checked, null, null, oBasic.fDec(txt_porccompletado));
                Save_ActividadEntidades(oBasic.fInt(hdd_idbanco_actividad), "u");

                oBasic.AlertUserControl(msgActivity, msgActivityMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u");
            }

            Enabled = false;
            ViewState["ST" + ControlID] = "Grid";
            oBasic.EnableControls(pnlDetail, false, true);
            ddl_Entidad.Enabled = Enabled;
            LoadControl();
            ViewControls();
            upActivityFoot.Update();
            upDetail.Update();

            return strResult;
        }
        private void Save_ActividadEntidades(string idActividad, string type)
        {
            string idEntidades = ddl_Entidad.GetSelectedValues();
            string strResult = oActividadEntidades.sp_d_bancoactividad_entidades(idActividad);

            oBasic.AlertUserControl(msgActivity, msgActivityMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".delete", "d");

            if (idEntidades.Split(',').Length > 0)
            {
                foreach (string idEntidad in idEntidades.Split(','))
                {
                    strResult = oActividadEntidades.sp_iu_bancoactividad_entidades(idActividad, idEntidad);
                    oBasic.AlertUserControl(msgActivity, msgActivityMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".insert", type);
                }
            }
        }
        private bool ValidateAccess(string section, string action, bool validateResponsible = false, bool requeridedResponsible = true)
        {
            string message = oPermisos.ValidateAccess(section, action, CodUsuario, validateResponsible, requeridedResponsible);

            if (message == "")
                return true;

            MessageInfo.ShowMessage(message);
            return false;
        }
        private void ViewAdd()
        {
            if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.INSERTAR, true)) return;

            ViewState["ST" + ControlID] = "Detail";
            hdd_idbanco_actividad.Value = "0";
            Enabled = true;
            LoadControl();
            LoadValidators(true);

            oBasic.FixPanel(divData, "Activity", 2);
            oBasic.EnableControls(pnlDetail, true, true);

            oBasic.AlertSection(msgActivity, clConstantes.MSG_I, "info");
        }
        private void ViewControls()
        {
            ViewState["ST" + ControlID] = ViewState["ST" + ControlID] ?? "Grid";

            pnlGrid.Visible = false;
            pnlDetail.Visible = false;

            switch (ViewState["ST" + ControlID].ToString())
            {
                case "Grid":
                    ViewState["IndexActivity"] = ViewState["IndexActivity"] ?? "0";
                    ViewState["SortExpActivity"] = ViewState["SortExpActivity"] ?? "fec_seguimiento";
                    ViewState["SortDirActivity"] = ViewState["SortDirActivity"] ?? "ASC";
                    pnlGrid.Visible = true;
                    oBasic.FixPanel(divData, "Activity", 0, pList: true, pEdit: false, pDelete: false);
                    upActivity.Update();
                    break;

                case "Detail":
                    pnlDetail.Visible = true;

                    oBasic.EnableControls(pnlDetail, Enabled, true);
                    ddl_Entidad.Enabled = Enabled;
                    txt_fec_inicio.Enabled = Enabled && hdd_fec_inicio.Value.Length == 0;
                    txt_fec_culminacion.Enabled = Enabled && hdd_fec_culminacion.Value.Length == 0;
                    txt_fec_finalizacion.Enabled = Enabled && hdd_fec_finalizacion.Value.Length == 0;
                    oBasic.FixPanel(divData, "Activity", Enabled ? 2 : 0, pList: true);
                    upDetail.Update();
                    break;
            }
        }
        #endregion
    }
}