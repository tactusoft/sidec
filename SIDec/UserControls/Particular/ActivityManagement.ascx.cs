using GLOBAL.CONST;
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

namespace SIDec.UserControls.Particular
{
    public partial class ActivityManagement : UserControl
    {
        private readonly BANCOACTIVIDADES_DAL oActividades = new BANCOACTIVIDADES_DAL();
        private readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
        private readonly ActividadAcciones_DAL oActividadAcciones = new ActividadAcciones_DAL();

        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();
        private readonly clFile oFile = new clFile();
        private readonly clLog oLog = new clLog();
        private readonly clUtil oUtil = new clUtil();
        private readonly clGlobalVar oVar = new clGlobalVar();

        private const string _SOURCEPAGE = "ActivityManagement";
        public delegate void OnListEventHandler(object sender);
        public event OnListEventHandler ToList;

        public delegate void OnViewDocEventHandler(object sender);
        public event OnViewDocEventHandler ViewDoc;



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
        #endregion



        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterScript();

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
                    MessageBox1.ShowConfirmation("Detail", "¿Está seguro de actualizar la información?", type: "warning");
                    break;
            }
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
                    LoadGridAcciones();
                    break;
                case "Agregar":
                    oBasic.AlertMain(msgActivityMain, "", "0");
                    ViewAdd();
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
        protected void gvAcciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvAcciones.Rows.Count)
                    rowIndex = 0;
                hdd_idactividad_accion.Value = rowIndex >= 0 ? gvAcciones.DataKeys[rowIndex]["idactividad_accion"].ToString() : "0";
                txt_acciones.Text = "";

                switch (e.CommandName)
                {
                    case "Select":
                        ViewState["ST" + ControlID] = "Grid";
                        break;
                    case "_Delete":
                        if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.ELIMINAR, false, false)) return;
                        txt_acciones.Text = gvAcciones.DataKeys[rowIndex]["acciones"].ToString();
                        MessageBox1.ShowConfirmation("DeleteAccion", "¿Está seguro de eliminar este registro?", type: "danger");
                        break;
                    case "_Edit":
                        if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.EDITAR, true)) return;
                        txt_acciones.Text = gvAcciones.DataKeys[rowIndex]["acciones"].ToString();
                        break;
                    default:
                        return;
                }
            }
            ViewControls();
        }
        protected void gvAcciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (DateTime.TryParse(gvAcciones.DataKeys[e.Row.DataItemIndex]["fecha"].ToString(), out DateTime fecha))
                {
                    LinkButton btnEdit = e.Row.Cells[1].FindControl("btnEdit") as LinkButton;
                    LinkButton btnDelete = e.Row.Cells[1].FindControl("btnDelete") as LinkButton;

                    btnEdit.Visible = btnDelete.Visible = fecha.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd");
                    if(fecha.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd"))
                        gvAcciones.Columns[1].Visible = Enabled;
                }
            }
        }
        protected void gvActivity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvActivity.Rows.Count)
                    rowIndex = 0;
                hdd_idbanco_actividad.Value = rowIndex >= 0 ? gvActivity.DataKeys[rowIndex]["idbanco_actividad"].ToString() : "0";
                txt_acciones.Text = "";
                hdd_idactividad_accion.Value = "0";

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
                    case "_Edit":
                        if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.EDITAR, true)) return;

                        ViewState["ST" + ControlID] = "Detail";
                        oBasic.AlertMain(msgActivityMain, "", "0");
                        Enabled = true;
                        LoadDetail();
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

                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvActivity, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActivity.PageIndex = e.NewPageIndex;
            LoadGrid();
            ViewState["IndexActivity"] = ((gvActivity.PageSize * gvActivity.PageIndex) + gvActivity.PageIndex - 1).ToString();
        }
        protected void lblPdfTramite_Click(object sender, EventArgs e)
        {
            string ruta = hdd_ruta_tramite.Value;
            oFile.GetPath(ruta);

            ViewDoc?.Invoke(this);
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
                        break; 
                    case "DeleteAccion":
                        DeleteAccion();
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
            upActivityFoot.Update();
        }
        #endregion



        #region Métodos privados
        private void DeleteAccion()
        {
            if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.ELIMINAR, false, false)) return;

            hdd_idactividad_accion.Value = hdd_idactividad_accion.Value == "" ? "0" : hdd_idactividad_accion.Value;

            string strResult = oActividadAcciones.sp_d_actividadacciones(hdd_idactividad_accion.Value);
            if (oBasic.AlertUserControl(msgActivity, msgActivityMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d"))
            {
                hdd_idactividad_accion.Value = "0";
                gvAcciones.Columns[1].Visible = false;
                LoadDetail();
                ViewControls();
                upActivityFoot.Update();
            }
        }
        private void EnableControls(bool enabled)
        {
            oBasic.EnableControls(pnlDetail, enabled, true);
            txt_estado_actividad.Enabled = false;
            txt_actividad.Enabled = false;
            txt_entidad.Enabled = false;
            txt_fec_culminacion.Enabled = false;
        }
        private void Initialize()
        {
            ViewState["IndexActivity"] = "0";
            ViewState["SortExpActivity"] = "fec_inicio";
            ViewState["SortDirActivity"] = "ASC";
            ViewState["ST" + ControlID] = "Grid";
        }
        private void LoadDetail()
        {
            if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.CONSULTAR, false, false)) return;

            oBasic.fClearControls(pnlDetail);
            lblPdfTramite.Visible = false;

            hdd_idbanco_actividad.Value = hdd_idbanco_actividad.Value.Trim() == "" ? "0" : hdd_idbanco_actividad.Value;
            if (hdd_idbanco_actividad.Value != "0")
            {
                DataSet dSet = oActividades.sp_s_bancoactividad_consultar(hdd_idbanco_actividad.Value);
                if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = dSet.Tables[0].Rows[0];

                    LoadDropDown_Identidad(ddla_idtramite, "76", "in (" + dRow["identidad"].ToString() + ")");
                    oBasic.fValueControls(pnlDetail, dRow);
                }
                LoadGridAcciones();
            }
        }
        private void LoadDropDowns()
        {
            LoadDropDown_Identidad(ddla_idtramite, "76", " = 0" + hdd_identidad.Value);
        }
        private void LoadDropDown_Identidad(DropDownList ddl, string id, string filter = "")
        {
            DataView dv = oIdentidades.sp_s_identidad_id_categoria(id).Tables[0].DefaultView;
            dv.RowFilter = "opcion_identidad is null or opcion_identidad " + filter;
            ddl.Items.Clear();
            ddl.DataSource = dv;
            ddl.DataTextField = "nombre_identidad";
            ddl.DataValueField = "id_identidad";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Seleccione", ""));
        }
        private void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.ACTIVITY, cnsAction.CONSULTAR, false, false)) return;

            ViewState["ST" + ControlID] = "Grid";
            bool? activos = null;
            if (chk_activas.Checked) activos = true;
            gvActivity.DataSource = oActividades.sp_s_bancoactividades_listar(IdBanco, p_activo: activos, p_clave: true);
            gvActivity.DataBind();
        }
        private void LoadGridAcciones()
        {
            DataSet dSetAcciones = oActividadAcciones.sp_s_actividadacciones_listar(oBasic.fInt(hdd_idbanco_actividad));
            gvAcciones.DataSource = dSetAcciones;
            gvAcciones.DataBind();
        }
        private void RegisterScript()
        {
            string key = "changeTramiteA";
            StringBuilder scriptSlider = new StringBuilder();
            scriptSlider.Append("<script type='text/javascript'> ");
            scriptSlider.Append("   function change_tramiteA() { ");
            scriptSlider.Append("       $('#divOtroTramiteA').hide(); ");
            scriptSlider.Append("       var selectedText = $('#" + ddla_idtramite.ClientID + "').find('option:selected').text(); ");
            scriptSlider.Append("       if (selectedText == 'Otro trámite') { $('#divOtroTramiteA').show(); } else {$('#" + txta_otrotramite.ClientID + "').val('');} ");
            scriptSlider.Append("  }    change_tramiteA();");
            scriptSlider.Append(" </script> ");

            if (!Page.ClientScript.IsStartupScriptRegistered(key))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
            }
            //--------------------------------------------------------------------------------------------------------------------

            lbLoadActivity.Attributes.Add("onclick", "$(\"input[ID*='" + fuLoadActivity.ClientID + "']\").click();return false;");

            key = "LoadFileActivity";
            scriptSlider = new StringBuilder();
            scriptSlider.Append("<script type='text/javascript'> ");
            scriptSlider.Append("   function infoFileActivity() {  ");
            scriptSlider.Append("       var name = $(\"input[ID*='" + fuLoadActivity.ClientID + "']\").val(); ");
            scriptSlider.Append("       $('#" + lblPdfTramite.ClientID + "').hide(); ");
            scriptSlider.Append("       var pos = name.lastIndexOf('\\\\'); ");
            scriptSlider.Append("       document.getElementById('" + lblInfoFileActivity.ClientID + "').innerHTML = name.substring(pos + 1); ");
            scriptSlider.Append("       name = name.substring(name.lastIndexOf('\\\\') + 1); ");
            scriptSlider.Append("       var ext = name.substring(name.lastIndexOf('.') + 1).toLowerCase(); ");

            scriptSlider.Append("       if(ext !== 'pdf'){");
            scriptSlider.Append("          $('#" + lblInfoFileActivity.ClientID + "').html(''); ");
            scriptSlider.Append("           $('#" + lblErrorFileActivity.ClientID + "').html('Extensión inválida, solo se permite «pdf»');} ");
            scriptSlider.Append("       else {   $('#" + lblErrorFileActivity.ClientID + "').html(''); ");
            scriptSlider.Append("          $('#" + lblInfoFileActivity.ClientID + "').html(name);}}");
            scriptSlider.Append(" </script> ");

            if (!Page.ClientScript.IsStartupScriptRegistered(key))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
            }
        }
        private string Save_Actividad()
        {
            var fileName = "";

            if (fuLoadActivity.HasFile)
            {
                string contentType = fuLoadActivity.ContentType;
                if (contentType == "application/pdf")
                {
                    fileName = "ACSG_" + hdd_idbanco_actividad.Value + "_" + Guid.NewGuid() + Path.GetExtension(fuLoadActivity.FileName);
                    fileName = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);
                    try
                    {
                        fuLoadActivity.SaveAs(fileName);
                    }
                    catch (Exception e)
                    {
                        oLog.RegistrarLogError("Error subiendo archivo " + e.Message + ":" + fuLoadActivity.FileName + ":::" + fuLoadActivity.PostedFile.ContentLength, _SOURCEPAGE, "Save_Radicados");
                    }
                }
            }

            hdd_idbanco_actividad.Value = hdd_idbanco_actividad.Value == "" ? "0" : hdd_idbanco_actividad.Value;

            string strResult = oActividades.sp_u_bancoactividades_gestion(Convert.ToInt32(hdd_idbanco_actividad.Value), oBasic.fInt(ddla_idtramite),
                    txta_otrotramite.Text, oBasic.fDateTime(txt_fec_culminacion), fileName); //oBasic.fDateTime(txt_fec_posible));

            if(oBasic.AlertUserControl(msgActivity, msgActivityMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u"))
            {
                if (txt_acciones.Text.Trim() != "")
                {
                    if (hdd_idactividad_accion.Value == "0")
                    {
                        strResult = oActividadAcciones.sp_i_actividadacciones(oBasic.fInt(hdd_idbanco_actividad), txt_acciones.Text);
                    }
                    else
                    {
                        strResult = oActividadAcciones.sp_u_actividadacciones(oBasic.fInt(hdd_idactividad_accion), txt_acciones.Text);
                        hdd_idactividad_accion.Value = "0";
                    }

                    if (!oBasic.AlertUserControl(msgActivity, msgActivityMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u"))
                    {
                        return strResult;
                    }
                }
            }
            else
            {
                return strResult;
            }

            Enabled = false;
            ViewState["ST" + ControlID] = "Grid";
            EnableControls(Enabled);
            LoadControl();
            ViewControls();
            upActivityFoot.Update();
            upDetail.Update();

            return strResult;
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

            oBasic.FixPanel(divData, "Activity", 2);
            EnableControls(Enabled);
            lblPdfTramite.Enabled = true;

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
                    oBasic.FixPanel(divData, "Activity", 0, pList: true, pEdit: false, pAdd: false, pDelete: false);
                    upActivity.Update();
                    break;

                case "Detail":
                    pnlDetail.Visible = true;

                    EnableControls(Enabled);
                    txt_acciones.Visible = Enabled;
                    oBasic.FixPanel(divData, "Activity", Enabled ? 2 : 0, pList: true, pAdd: false, pDelete: false);
                    lblPdfTramite.Visible = lblPdfTramite.Enabled = (hdd_ruta_tramite.Value != "");
                    lbLoadActivity.Visible = Enabled;

                    upDetail.Update();
                    break;
            }
        }
        #endregion
    }
}