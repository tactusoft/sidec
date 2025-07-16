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
    public partial class Tracing : UserControl
    {

        private readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
        private readonly BANCOACTIVIDADES_DAL oActividades = new BANCOACTIVIDADES_DAL();
        private readonly Seguimientos_DAL oSeguimiento = new Seguimientos_DAL();
        private readonly SeguimientoParticipantes_DAL oSeguimientoParticipante = new SeguimientoParticipantes_DAL();
        private readonly SeguimientoRadicados_DAL oSeguimientoRadicado = new SeguimientoRadicados_DAL();

        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clBasic oBasic = new clBasic();
        private readonly clFile oFile = new clFile();
        private readonly clLog oLog = new clLog();
        private readonly clUtil oUtil = new clUtil();

        private const string _SOURCEPAGE = "Tracing";
        private const string TIPO_ACTIVIDAD__ID_NO_REUNION = "648";

        public delegate void OnListEventHandler(object sender);
        public event OnListEventHandler ToList;

        public delegate void OnViewDocEventHandler(object sender);
        public event OnViewDocEventHandler ViewDoc;

        public delegate void OnSaveActivityEventHandler(object sender);
        public event OnSaveActivityEventHandler SaveActivity;




        #region Propiedades
        public string CodUsuario
        {
            get
            {
                return (Session[ControlID + ".Tracing.CodUsuario"] ?? "").ToString();
            }
            set
            {
                Session[ControlID + ".Tracing.CodUsuario"] = value;
            }
        }
        public string ControlID { 
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
                return (Session[ControlID + ".Tracing.Enabled"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".Tracing.Enabled"] = value ? "1" : "0";
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
                Load_Actividades();
                Initialize();
            }
        }
        public int IdProyecto
        {
            get
            {
                return (Int32.TryParse(hddIdProyecto.Value, out int id) ? id : 0);
            }
            set
            {
                hddIdProyecto.Value = value.ToString();
            }
        }
        public bool ReturnToList
        {
            get
            {
                return (Session[ControlID + ".Tracing.ReturnToList"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".Tracing.ReturnToList"] = value ? "1" : "0";
            }
        }
        #endregion



        #region "Eventos"
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
            switch(ViewState["ST" + ControlID])
            {
                case "Detail":
                    if (hdd_idseguimiento.Value != "0")
                    {
                        MessageBox1.ShowConfirmation("Detail", "¿Está seguro de actualizar la información?", type: "warning");
                    }
                    else
                        MessageBox1.ShowConfirmation("Detail", "¿Está seguro de continuar con la acción solicitada?");
                    break;
                case "Participante":
                    if (hdd_idseguimiento.Value == "0")
                    {
                        MessageInfo.ShowMessage("No se puede realizar la operación. Guarde primero la información del seguimiento.");
                    }
                    else if (ddl_identidad.SelectedValue == "" && !oUtil.ValidateText(btnParticipantes.Text, 3))
                    {
                        MessageInfo.ShowMessage("La información ingresada no es suficiente para ser almacenada.");
                    }
                    else if (hdd_idseguimiento_participante.Value != "0")
                    {
                        MessageBox1.ShowConfirmation("Participante", "¿Está seguro de actualizar la información?", type: "warning");
                    }
                    else
                        MessageBox1.ShowConfirmation("Participante", "¿Está seguro de continuar con la acción solicitada?");
                    break;
                case "Radicado":
                    if (hdd_idseguimiento.Value == "0")
                    {
                        MessageInfo.ShowMessage("No se puede realizar la operación. Guarde primero la información del seguimiento.");
                    }

                    if (hdd_idseguimiento_radicado.Value != "0")
                        MessageBox1.ShowConfirmation("Radicado", "¿Está seguro de actualizar la información?", type: "warning");
                    else
                        MessageBox1.ShowConfirmation("Radicado", "¿Está seguro de continuar con la acción solicitada?");
                    break;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgMain, "", "0");
            if (ViewState["ST" + ControlID].ToString() == "Participante" || ViewState["ST" + ControlID].ToString() == "Radicado")
                ViewState["ST" + ControlID] = "Detail";
            else
                ViewState["ST" + ControlID] = "Grid";

            Enabled = false;
            ViewControls();
        }
        protected void btnParticipantes_Click(object sender, EventArgs e)
        {
            uctbgParticipantes.ShowModal("Visualizador de Participantes", "Participante", btnParticipantes.Text);
            upRadicado.Update();
        }
        protected void btnParticipanteAdd_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgMain, "", "0");
            ViewAddParticipante();
        }
        protected void btnRadicadoAdd_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgMain, "", "0");
            oBasic.fClearControls(pnlTramite);
            ViewAddRadicado();
        }
        protected void btnTracingAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgMain, "", "0");

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    if (ViewState["ST" + ControlID].ToString() == "Grid")
                        ToList?.Invoke(this);
                    else if (ViewState["ST" + ControlID].ToString() == "Participante")
                        ViewDetail();
                    else if (ViewState["ST" + ControlID].ToString() == "Radicado")
                        ViewDetail();
                    else
                        ViewList();
                    break;
                case "Editar":
                    if (ViewState["ST" + ControlID].ToString() == "Participante")
                        ViewEditParticipante();
                    else if (ViewState["ST" + ControlID].ToString() == "Radicado")
                    {
                        oBasic.fClearControls(pnlTramite);
                        ViewEditRadicado();
                    }
                    else
                        ViewEdit();
                    break;
                case "Agregar":
                    if (ViewState["ST" + ControlID].ToString() == "Participante")
                        ViewAddParticipante();
                    else if (ViewState["ST" + ControlID].ToString() == "Radicado")
                    {
                        oBasic.fClearControls(pnlTramite);
                        ViewAddRadicado();
                    }
                    else
                        ViewAdd();
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.TRACING, cnsAction.ELIMINAR, true, false)) return;

                    if (ViewState["ST" + ControlID].ToString() == "Participante")
                        MessageBox1.ShowConfirmation("DeleteParticipante", "¿Está seguro de eliminar este participante?", type: "danger");
                    else if (ViewState["ST" + ControlID].ToString() == "Radicado")
                        MessageBox1.ShowConfirmation("DeleteRadicado", "¿Está seguro de eliminar este radicado?", type: "danger");
                    else
                        MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar este seguimiento?", type: "danger");
                    return;
            }
            ViewControls();
        }
        protected void btnTracingAdd_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgMain, "", "0");
            ViewAdd();
        }
        protected void btnTracingSection_Click(object sender, EventArgs e)
        {
            int oldIndex = mvTracingSection.ActiveViewIndex;
            int newIndex = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            string oldID = "lbTracingSection_" + oldIndex.ToString();
            string newID = "lbTracingSection_" + newIndex.ToString();
            LinkButton lbOld = (LinkButton)ulTracingSection.FindControl(oldID);
            LinkButton lbNew = (LinkButton)ulTracingSection.FindControl(newID);
            oBasic.ActiveNav(mvTracingSection, lbOld, lbNew, newIndex);
            ViewControls();
        }
        protected void ddl_idbanco_actividad_SelectedIndexChanged(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgMain, "", "0");
            txt_estado_actividad.Text = "";
            LoadSelected_Estado(ddl_idbanco_actividad.SelectedValue);
        }
        protected void ddl_idtipo_actividad_SelectedIndexChanged(object sender, EventArgs e)
        {
            rfv_idbanco_actividad.Enabled = rfv_id_banco_actividad_na.Enabled = !(ddl_idtipo_actividad.SelectedValue == TIPO_ACTIVIDAD__ID_NO_REUNION);
        }
        #region--------------------------------------------------------------------GRIDVIEW
        protected void gvParticipante_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvParticipantes.Rows.Count)
                    rowIndex = 0;
                hdd_idseguimiento_participante.Value = rowIndex >= 0 ? gvParticipantes.DataKeys[rowIndex]["idseguimiento_participante"].ToString() : "0";

                switch (e.CommandName)
                {
                    case "Select":
                        break;
                    case "_Detail":
                        oBasic.AlertMain(msgMain, "", "0");
                        ViewParticipante();
                        break;
                    case "_Delete":
                        if (!ValidateAccess(cnsSection.TRACING, cnsAction.ELIMINAR, true, false)) return;

                        oBasic.AlertMain(msgMain, "", "0");
                        ViewParticipante();
                        MessageBox1.ShowConfirmation("DeleteParticipante", "¿Está seguro de eliminar este participante?", type: "danger");
                        break;
                    case "_Edit":
                        oBasic.AlertMain(msgMain, "", "0");
                        ViewEditParticipante();
                        break;
                    default:
                        return;
                }
                ViewControls();
            }
        }
        protected void gvParticipante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvParticipantes, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvRadicado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvRadicado.Rows.Count)
                    rowIndex = 0;
                hdd_idseguimiento_radicado.Value = rowIndex >= 0 ? gvRadicado.DataKeys[rowIndex]["idseguimiento_radicado"].ToString() : "0";

                switch (e.CommandName)
                {
                    case "Select":
                        //ViewState["ST" + ControlID] = "Grid";
                        break;
                    case "_OpenFile":
                        string ruta = gvRadicado.DataKeys[rowIndex]["ruta_tramite"].ToString();
                        oFile.GetPath(ruta);

                        ViewDoc?.Invoke(this);
                        break;
                    case "_Detail":
                        oBasic.AlertMain(msgMain, "", "0");
                        ViewRadicado();
                        break;
                    case "_Delete":
                        if (!ValidateAccess(cnsSection.TRACING, cnsAction.ELIMINAR, true, false)) return ;

                        oBasic.AlertMain(msgMain, "", "0");
                        ViewRadicado();
                        MessageBox1.ShowConfirmation("DeleteRadicado", "¿Está seguro de eliminar este radicado?", type: "danger");
                        break;
                    case "_Edit":
                        oBasic.AlertMain(msgMain, "", "0");
                        ViewEditRadicado();
                        break;
                    default:
                        return;
                }
                ViewControls();
            }
        }
        protected void gvRadicado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvRadicado, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvTracing_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTracing.PageIndex = e.NewPageIndex;
            LoadControl();
            ViewState["IndexTracing"] = ((gvRadicado.PageSize * gvRadicado.PageIndex) + gvRadicado.PageIndex - 1).ToString();
        }
        protected void gvTracing_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvTracing, "Select$" + e.Row.RowIndex.ToString()));

                if (gvTracing.DataKeys[e.Row.DataItemIndex % gvTracing.PageSize]["seg_historico"].ToString() == "1")
                {
                    e.Row.ForeColor = System.Drawing.Color.FromArgb(55, 55, 55);
                    e.Row.BackColor = System.Drawing.Color.FromArgb(241, 242, 243);
                    e.Row.Font.Italic = true;
                }
            }
        }
        protected void gvTracing_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["IndexTracing"] = ((gvTracing.PageIndex * gvTracing.PageSize) + gvTracing.SelectedIndex).ToString();
            if (gvTracing.Rows.Count > 0)
            {
                hdd_idseguimiento.Value = gvTracing.SelectedDataKey["idseguimiento"].ToString();
            }
        }
        protected void gvTracing_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvTracing, e.SortExpression.ToString(), ViewState["SortDirTracing"].ToString(), oVar.prDSTracing);
            oVar.prDSTracing = oVar.prDataSet;
        }
        protected void gvTracing_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvTracing.Rows.Count)
                    rowIndex = 0;
                hdd_idseguimiento.Value = rowIndex >= 0 ? gvTracing.DataKeys[rowIndex]["idseguimiento"].ToString() : "0";

                switch (e.CommandName)
                {
                    case "Select":
                        //ViewState["ST" + ControlID] = "Grid";
                        break;
                    case "_Detail":
                        oBasic.AlertMain(msgMain, "", "0");
                        ViewDetail();
                        break;
                    case "_Delete":
                        if (!ValidateAccess(cnsSection.TRACING, cnsAction.ELIMINAR, true, false)) return;

                        oBasic.AlertMain(msgMain, "", "0");
                        ViewDetail();
                        MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar este seguimiento?", type: "danger");
                        break;
                    case "_Edit":
                        oBasic.AlertMain(msgMain, "", "0");
                        ViewEdit();
                        break;
                    default:
                        return;
                }
            }
        }
        protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header)
                return;

            string sortExpression = (ViewState["SortExpTracing"] ?? "fec_seguimiento").ToString();
            string sortDirection = (ViewState["SortDirTracing"]?? "ASC").ToString();
            foreach (TableCell tableCell in e.Row.Cells)
            {
                if (!tableCell.HasControls())
                    continue;
                if (!(tableCell.Controls[0] is LinkButton lbSort))
                    continue;
                if (lbSort.CommandArgument == sortExpression)
                {
                    Image imageSort = new Image
                    {
                        ImageAlign = ImageAlign.AbsMiddle,
                        Width = 12
                    };
                    imageSort.Style.Add("margin-left", "3px");
                    if (sortDirection == "ASC")
                        imageSort.ImageUrl = "~/Images/icon/up.png";
                    else
                        imageSort.ImageUrl = "~/Images/icon/down.png";
                    tableCell.Controls.Add(imageSort);
                }
            }
        }
        #endregion
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
                        Save_Seguimientos();
                        break;
                    case "Participante":
                        strResult = Save_Participante();
                        break;
                    case "Radicado":
                        strResult = Save_Radicados();
                        break;
                    case "Delete":
                        strResult = Delete_Seguimientos();
                        break;
                    case "DeleteParticipante":
                        strResult = Delete_Participante();
                        break;
                    case "DeleteRadicado":
                        strResult = Delete_Radicados();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }
        protected void txt_mes_visualizacion_TextChanged(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgMain, "", "0");
            LoadGrid();
            ViewControls();
        }
        protected void uctbgParticipantes_Accept(object sender)
        {
            btnParticipantes.Text = uctbgParticipantes.ToString();
        }
        protected void uctbgParticipantes_Remove(object sender)
        {
            btnParticipantes.Text = uctbgParticipantes.ToString();
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
                case "Participante":
                    LoadParticipante();
                    break;
                case "Radicado":
                    LoadRadicado();
                    break;
                default: break;
            }

            ViewControls();
        }
        #endregion



        #region "Métodos privados"
        private string Delete_Seguimientos()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.ELIMINAR, true, false)) return "";

            hdd_idseguimiento.Value = hdd_idseguimiento.Value == "" ? "0" : hdd_idseguimiento.Value;

            string strResult = oSeguimiento.sp_d_seguimiento(hdd_idseguimiento.Value);

            if (oBasic.AlertUserControl(msgTracing, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d"))
            {
                ViewList();
                ViewControls();
                upTracingFoot.Update();
            }

            return strResult;
        }
        private string Delete_Participante()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.ELIMINAR, true, false)) return "";

            hdd_idseguimiento_participante.Value = hdd_idseguimiento_participante.Value == "" ? "0" : hdd_idseguimiento_participante.Value;

            string strResult = oSeguimientoParticipante.sp_d_seguimientoparticipantes(hdd_idseguimiento_participante.Value);
            if (oBasic.AlertUserControl(msgTracing, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d"))
            {
                ViewDetail();
                ViewControls();
                upTracingFoot.Update();
            }

            return strResult;
        }
        private string Delete_Radicados()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.ELIMINAR, true, false)) return "";

            hdd_idseguimiento_radicado.Value = hdd_idseguimiento_radicado.Value == "" ? "0" : hdd_idseguimiento_radicado.Value;

            string strResult = oSeguimientoRadicado.sp_d_seguimientoradicados(hdd_idseguimiento_radicado.Value);
            if (oBasic.AlertUserControl(msgTracing, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d"))
            {
                ViewDetail();
                ViewControls();
                upTracingFoot.Update();
            }
            return strResult;
        }
        private void gv_Sorting(GridView gv, string sortExpression, string sortDirection, object ds)
        {
            string modulo = gv.ID.Substring(2);
            if (ViewState["SortExp" + modulo].ToString() != sortExpression)
                sortDirection = "ASC";
            else
            {
                if (sortDirection == "ASC")
                    sortDirection = "DESC";
                else
                    sortDirection = "ASC";
            }
            ViewState["SortExp" + modulo] = sortExpression;
            ViewState["SortDir" + modulo] = sortDirection;
            gv.SelectedIndex = 0;

            DataView dataView = new DataView(((DataSet)(ds)).Tables[0])
            {
                Sort = sortExpression + " " + sortDirection
            };
            gv.DataSource = dataView;
            gv.DataBind();
            oVar.prDataSet = oUtil.ConvertToDataSet(dataView);
            gv_SelectedIndexChanged(gv);
        }
        private void gv_SelectedIndexChanged(GridView gv)
        {
            string modulo = gv.ID.Substring(2);
            ViewState["Index" + modulo] = ((gv.PageIndex * gv.PageSize) + gv.SelectedIndex).ToString();
        }
        private void Initialize()
        {
            ViewState["IndexTracing"] = "0";
            ViewState["SortExpTracing"] = "fec_seguimiento";
            ViewState["SortDirTracing"] = "ASC";
            ViewState["ST" + ControlID] = "Grid";

            txt_mes_visualizacion.Text = DateTime.Now.ToString("yyyy-MM");
            ce_fec_seguimiento.StartDate = ce_fecha.StartDate = new DateTime(2000, 1, 1);
            rv_fec_seguimiento.MinimumValue = rv_fecha.MinimumValue = (new DateTime(2000, 1, 1)).ToString("yyyy-MM-dd");
            ce_fec_seguimiento.EndDate = ce_fecha.EndDate = DateTime.Today.AddYears(50);
            rv_fec_seguimiento.MaximumValue = rv_fecha.MaximumValue = (DateTime.Today.AddYears(50)).ToString("yyyy-MM-dd");
        }
        private void Load_Actividades(string idActividad = null)
        {
            ddl_idbanco_actividad.ClearSelection();
            if (IdBanco > 0)
            {
                ddl_idbanco_actividad.DataSource = oActividades.sp_s_bancoactividades_listar(IdBanco, p_activo: true, p_vigente: true, p_idActividad: idActividad);
                ddl_idbanco_actividad.DataTextField = "nombre";
                ddl_idbanco_actividad.DataValueField = "idbanco_actividad";
                ddl_idbanco_actividad.DataBind();

                ddl_idbanco_actividad.Items.Insert(0, new ListItem("-- Seleccione opción", ""));
                ddl_idbanco_actividad.SelectedValue = idActividad ?? "";
            }
        }
        private void LoadDetail()
        {
            oBasic.fClearControls(pnlDetail);
            txt_fec_seguimiento.Text = DateTime.Now.ToString("yyyy-MM-dd");
            hdd_idseguimiento.Value = hdd_idseguimiento.Value.Trim() == "" ? "0" : hdd_idseguimiento.Value;
            pnlGridRadicado.Visible = false;
            if (hdd_idseguimiento.Value != "0")
            {
                DataSet dSet = oSeguimiento.sp_s_seguimiento_consultar(hdd_idseguimiento.Value);
                if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = dSet.Tables[0].Rows[0];

                    Load_Actividades(dRow["idbanco_actividad"].ToString());
                    oBasic.fValueControls(pnlDetail, dRow);
                    pnlGridRadicado.Visible = true;

                    DataSet dSetPart = oSeguimientoParticipante.sp_s_seguimientoparticipantes(hdd_idseguimiento.Value);
                    gvParticipantes.DataSource = dSetPart;
                    gvParticipantes.DataBind();

                    DataSet dSetRad = oSeguimientoRadicado.sp_s_seguimientoradicados(hdd_idseguimiento.Value);
                    gvRadicado.DataSource = dSetRad;
                    gvRadicado.DataBind();
                }
            }
        }
        private void LoadDropDowns()
        {
            LoadDropDown_Identidad(ddl_identidad, "75");
            LoadDropDown_Identidad(ddl_identidad_radicado, "75");
            LoadDropDown_Identidad(ddlt_idtramite, "76", " = 0" + ddl_identidad_radicado.SelectedValue);
            LoadDropDown_Identidad(ddl_idasunto, "77");
            LoadDropDown_Identidad(ddl_idtipo_actividad, "78");
        }
        private void LoadDropDown_Identidad(DropDownList ddl, string id, string filter = "")
        {
            DataView dv = oIdentidades.sp_s_identidad_id_categoria(id).Tables[0].DefaultView;
            if (filter != "")
            {
                dv.RowFilter = "opcion_identidad is null or opcion_identidad " + filter;
                ddl.Items.Clear();
            }
            ddl.DataSource = dv;
            ddl.DataTextField = "nombre_identidad";
            ddl.DataValueField = "id_identidad";
            ddl.DataBind();
            if (!ddl.AppendDataBoundItems)
                ddl.Items.Insert(0, new ListItem("Seleccione", ""));
        }
        private void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.CONSULTAR, false, false)) {
                IdBanco = IdProyecto = -100;
                return;
            }

            string fechaMes = txt_mes_visualizacion.Text.Length > 0 ? txt_mes_visualizacion.Text : null;
            oVar.prDSTracing = oSeguimiento.sp_s_seguimientos(IdBanco.ToString(), IdProyecto.ToString(), fechaMes);
            gvTracing.DataSource = ((DataSet)(oVar.prDSTracing));
            gvTracing.DataBind();
        }
        private void LoadParticipante()
        {
            oBasic.fClearControls(pnlParticipante);
            btnParticipantes.Text = "";
            hdd_idseguimiento_participante.Value = hdd_idseguimiento_participante.Value.Trim() == "" ? "0" : hdd_idseguimiento_participante.Value;
            pnlParticipante.Visible = false;
            pnlTramite.Visible = false;
            if (hdd_idseguimiento_participante.Value != "0")
            {
                DataSet dSet = oSeguimientoParticipante.sp_s_seguimientoparticipantes_consultar(hdd_idseguimiento_participante.Value);
                if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = dSet.Tables[0].Rows[0];
                    oBasic.fValueControls(pnlParticipante, dRow);
                    btnParticipantes.Text = dRow["participantes"].ToString();
                }
            }
        }
        private void LoadRadicado()
        {
            oBasic.fClearControls(pnlTramite);
            lblPdfTramite.Visible = false;
            hdd_idseguimiento_radicado.Value = hdd_idseguimiento_radicado.Value.Trim() == "" ? "0" : hdd_idseguimiento_radicado.Value;
            pnlParticipante.Visible = false;
            pnlTramite.Visible = false;
            if (hdd_idseguimiento_radicado.Value != "0")
            {
                DataSet dSet = oSeguimientoRadicado.sp_s_seguimientoradicados_consultar(hdd_idseguimiento_radicado.Value);
                if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = dSet.Tables[0].Rows[0];

                    LoadDropDown_Identidad(ddlt_idtramite, "76", " = 0" + dRow["identidad_radicado"].ToString());
                    oBasic.fValueControls(pnlTramite, dRow);
                    pnlTramite.Visible = true;
                    lblPdfTramite.Visible = lblPdfTramite.Enabled = ((dRow["ruta_tramite"] ?? "").ToString() != "");
                }
            }
            lbLoadTracing.Visible = Enabled;
        }
        private void LoadSelected_Estado(string id)
        {
            DataSet actividad = oActividades.sp_s_bancoactividades_listar(-1, p_idActividad: id);
            if (actividad.Tables.Count > 0 && actividad.Tables[0].Rows.Count > 0)
                txt_estado_actividad.Text = actividad.Tables[0].Rows[0]["estado_actividad"].ToString();
            upDetail.Update();
        }
        private void LoadValidators(string section, bool _enabled)
        {
            rev_fec_seguimiento.Enabled = section == "Detail" && _enabled;
            rv_fec_seguimiento.Enabled = section == "Detail" && _enabled;
            rfv_fec_seguimiento.Enabled = section == "Detail" && _enabled;
            rfv_asunto.Enabled = section == "Detail" && _enabled;
            rfv_idbanco_actividad.Enabled = section == "Detail" && _enabled && !(ddl_idtipo_actividad.SelectedValue == TIPO_ACTIVIDAD__ID_NO_REUNION);
            rfv_id_banco_actividad_na.Enabled = section == "Detail" && _enabled && !(ddl_idtipo_actividad.SelectedValue == TIPO_ACTIVIDAD__ID_NO_REUNION);
            rfv_gestion.Enabled = section == "Detail" && _enabled;

            rev_fecha.Enabled = section == "Radicado" && _enabled;
            rv_fecha.Enabled = section == "Radicado" && _enabled;
            rfv_fecha.Enabled = section == "Radicado" && _enabled;
            rfv_idtipo_radicado.Enabled = section == "Radicado" && _enabled;
            rfv_identidad_radicado.Enabled = section == "Radicado" && _enabled;
        }
        private void RegisterScript()
        {
            if (Enabled)
            {
                string key = "changeTramiteT";
                StringBuilder scriptSlider = new StringBuilder();
                scriptSlider.Append("<script type='text/javascript'> ");
                scriptSlider.Append("   function change_tramiteT() { ");
                scriptSlider.Append("       $('#" + txtt_otrotramite.ClientID + "').prop('disabled', true); ");
                scriptSlider.Append("       var selectedText = $('#" + ddlt_idtramite.ClientID + "').find('option:selected').text(); ");
                scriptSlider.Append("       if (selectedText == 'Otro trámite') { $('#" + txtt_otrotramite.ClientID + "').prop('disabled', false); } else {$('#" + txtt_otrotramite.ClientID + "').val('');} ");
                scriptSlider.Append("  }    change_tramiteT();");
                scriptSlider.Append(" </script> ");

                if (!Page.ClientScript.IsStartupScriptRegistered(key))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
                }

                //--------------------------------------------------------------------------------------------------------------------

                lbLoadTracing.Attributes.Add("onclick", "$(\"input[ID*='" + fuLoadTracing.ClientID + "']\").click();return false;");

                key = "LoadFileTracing";
                scriptSlider = new StringBuilder();
                scriptSlider.Append("<script type='text/javascript'> ");
                scriptSlider.Append("   function infoFileTracing() {  ");
                scriptSlider.Append("       var name = $(\"input[ID*='" + fuLoadTracing.ClientID + "']\").val(); ");
                scriptSlider.Append("       $('#" + lblPdfTramite.ClientID + "').hide(); ");
                scriptSlider.Append("       var pos = name.lastIndexOf('\\\\'); ");
                scriptSlider.Append("       document.getElementById('" + lblInfoFileTracing.ClientID + "').innerHTML = name.substring(pos + 1); ");
                scriptSlider.Append("       name = name.substring(name.lastIndexOf('\\\\') + 1); ");
                scriptSlider.Append("       var ext = name.substring(name.lastIndexOf('.') + 1).toLowerCase(); ");

                scriptSlider.Append("       if(ext !== 'pdf'){");
                scriptSlider.Append("          $('#" + lblInfoFileTracing.ClientID + "').html(''); ");
                scriptSlider.Append("           $('#" + lblErrorFileTracing.ClientID + "').html('Extensión inválida, solo se permite «pdf»');} ");
                scriptSlider.Append("       else {   $('#" + lblErrorFileTracing.ClientID + "').html(''); ");
                scriptSlider.Append("          $('#" + lblInfoFileTracing.ClientID + "').html(name); }}");
                scriptSlider.Append(" </script> ");

                if (!Page.ClientScript.IsStartupScriptRegistered(key))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
                }
            }
        }
        private string Save_Participante()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.INSERTAR, true)) return "";

            string strResult;
            hdd_idseguimiento_participante.Value = hdd_idseguimiento_participante.Value == "" ? "0" : hdd_idseguimiento_participante.Value;

            if (hdd_idseguimiento_participante.Value == "0")
            {
                strResult = oSeguimientoParticipante.sp_i_seguimientoparticipantes(
                    oBasic.fInt(hdd_idseguimiento),
                    oBasic.fInt(ddl_identidad),
                    btnParticipantes.Text.Trim());

                if (oBasic.AlertUserControl(msgTracing, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i"))
                {
                    hdd_idseguimiento_participante.Value = hdd_idseguimiento_participante.Value == "0" ? (strResult.Split(':'))[1] : hdd_idseguimiento_participante.Value;
                }
                else
                {
                    return strResult;
                }
            }
            else
            {
                strResult = oSeguimientoParticipante.sp_u_seguimientoparticipantes(
                    oBasic.fInt(hdd_idseguimiento_participante),
                    oBasic.fInt(ddl_identidad),
                    btnParticipantes.Text.Trim());

                if (!oBasic.AlertUserControl(msgTracing, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u"))
                {
                    oBasic.SPError(msgMain, msgTracing, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResult);
                    return strResult;
                }
            }

            ViewDetail();

            return strResult;
        }
        private string Save_Radicados()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.INSERTAR, true)) return "";

            var fileName = "";

            if (fuLoadTracing.HasFile)
            {
                string contentType = fuLoadTracing.ContentType;
                if (contentType == "application/pdf")
                {
                    fileName = "TCPY_" + hdd_idseguimiento.Value + "_" + Guid.NewGuid() + Path.GetExtension(fuLoadTracing.FileName);
                    fileName = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);
                    try
                    {
                        fuLoadTracing.SaveAs(fileName);
                    }
                    catch (Exception e)
                    {
                        oLog.RegistrarLogError("Error subiendo archivo " + e.Message + ":" + fuLoadTracing.FileName + ":::" + fuLoadTracing.PostedFile.ContentLength, _SOURCEPAGE, "Save_Radicados");
                    }
                }
            }

            string strResult;
            hdd_idseguimiento_radicado.Value = hdd_idseguimiento_radicado.Value == "" ? "0" : hdd_idseguimiento_radicado.Value;

            if (hdd_idseguimiento_radicado.Value == "0")
            {
                strResult = oSeguimientoRadicado.sp_i_seguimientoradicados(
                    oBasic.fInt(hdd_idseguimiento),
                    oBasic.fInt(ddl_identidad_radicado),
                    oBasic.fInt(ddl_idtipo_radicado),
                    txt_radicado.Text.Trim(),
                    oBasic.fDateTime(txt_fecha),
                    oBasic.fInt(ddlt_idtramite),
                    txtt_otrotramite.Text,
                    oBasic.fInt(ddl_idasunto),
                    txt_observaciones_radicado.Text.Trim(),
                    fileName);

                if (oBasic.AlertUserControl(msgTracing, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i"))
                    hdd_idseguimiento_radicado.Value = hdd_idseguimiento_radicado.Value == "0" ? (strResult.Split(':'))[1] : hdd_idseguimiento_radicado.Value;
                else
                    return strResult;
            }
            else
            {
                strResult = oSeguimientoRadicado.sp_u_seguimientoradicados(
                    oBasic.fInt(hdd_idseguimiento_radicado),
                    oBasic.fInt(ddl_identidad_radicado),
                    oBasic.fInt(ddl_idtipo_radicado),
                    txt_radicado.Text.Trim(),
                    oBasic.fDateTime(txt_fecha),
                    oBasic.fInt(ddlt_idtramite), 
                    txtt_otrotramite.Text,
                    oBasic.fInt(ddl_idasunto),
                    txt_observaciones_radicado.Text.Trim(),
                    fileName);

                if (!oBasic.AlertUserControl(msgTracing, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u"))
                    return strResult;
            }

            ViewDetail();

            return strResult;
        }
        private void Save_Seguimientos()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.INSERTAR, true)) return;

            hdd_idseguimiento.Value = hdd_idseguimiento.Value == "" ? "0" : hdd_idseguimiento.Value;

            if (hdd_idseguimiento.Value == "0")
            {
                string strResult = oSeguimiento.sp_i_seguimiento(IdBanco.ToString(), 
                    oBasic.fInt(ddl_idbanco_actividad),
                    txt_fec_seguimiento.Text.Trim(), 
                    txt_asunto.Text.Trim(),
                    ddl_idtipo_actividad.Text.Trim(),
                    txt_gestion.Text.Trim(), 
                    txt_compromisos.Text.Trim());

                if (oBasic.AlertUserControl(msgTracing, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i"))
                {
                    hdd_idseguimiento.Value = hdd_idseguimiento.Value == "0" ? (strResult.Split(':'))[1] : hdd_idseguimiento.Value;
                }
                else
                {
                    return;
                }
            }
            else
            {
                string strResult = oSeguimiento.sp_u_seguimiento(
                    oBasic.fInt(hdd_idseguimiento),
                    oBasic.fInt(ddl_idbanco_actividad),
                    txt_fec_seguimiento.Text.Trim(), 
                    txt_asunto.Text.Trim(),
                    ddl_idtipo_actividad.Text.Trim(),
                    txt_gestion.Text.Trim(), 
                    txt_compromisos.Text.Trim());

                if (!oBasic.AlertUserControl(msgTracing, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i")) 
                {
                    oBasic.SPError(msgMain, msgTracing, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResult);
                    return;
                }
            }

            ViewDetail();
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
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.INSERTAR, true)) return;

            ViewState["ST" + ControlID] = "Detail";
            hdd_idseguimiento.Value = "0";
            Enabled = true;
            LoadControl();
            LoadValidators("Detail", true);

            oBasic.FixPanel(divData, "Tracing", 2);
            oBasic.EnableControls(pnlDetail, true, true);
            txt_estado_actividad.Enabled = false;
            txt_fec_seguimiento.Text = DateTime.Now.ToString("yyyy-MM-dd");
            oBasic.AlertSection(msgTracing, clConstantes.MSG_I, "info");
        }
        private void ViewAddParticipante()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.INSERTAR, true)) return;

            ViewState["ST" + ControlID] = "Participante";
            hdd_idseguimiento_participante.Value = "0";
            Enabled = true;
            LoadControl();
            LoadValidators("Participante", true);

            oBasic.FixPanel(divData, "Tracing", 2);
            oBasic.EnableControls(pnlParticipante, true, true);
            pnlParticipante.Visible = true;
            pnlTramite.Visible = false;
            uctbgParticipantes.Enabled = true;
            oBasic.AlertSection(msgTracing, clConstantes.MSG_I, "info");
        }
        private void ViewAddRadicado()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.INSERTAR, true)) return;

            ViewState["ST" + ControlID] = "Radicado";
            hdd_idseguimiento_radicado.Value = "0";
            Enabled = true;
            LoadControl();
            LoadValidators("Radicado", true);

            oBasic.FixPanel(divData, "Tracing", 2);
            oBasic.EnableControls(pnlTramite, true, true);
            pnlParticipante.Visible = false;
            pnlTramite.Visible = true;
            oBasic.AlertSection(msgTracing, clConstantes.MSG_I, "info");
        }
        private void ViewControls()
        {
            ControlID = ControlID == string.Empty ? Guid.NewGuid().ToString() : ControlID;
            ViewState["ST" + ControlID] = ViewState["ST" + ControlID] ?? "Grid";

            pnlGrid.Visible = false;
            pnlDetail.Visible = false;
            pnlParticipante.Visible = false;
            pnlTramite.Visible = false;

            switch (ViewState["ST" + ControlID].ToString())
            {
                case "Grid":
                    ViewState["IndexTracing"] = ViewState["IndexTracing"] ?? "0";
                    ViewState["SortExpTracing"] = ViewState["SortExpTracing"] ?? "fec_seguimiento";
                    ViewState["SortDirTracing"] = ViewState["SortDirTracing"] ?? "ASC";
                    pnlGrid.Visible = true;
                    oBasic.FixPanel(divData, "Tracing", 0, pList: ReturnToList, pEdit: false, pDelete: false);
                    upTracing.Update();
                    break;

                case "Detail":
                    pnlDetail.Visible = true;
                    oBasic.FixPanel(divData, "Tracing", Enabled ? 2 : 0, pList: true);
                    txt_estado_actividad.Enabled = false;
                    upDetail.Update();
                    break;

                case "Participante":
                    pnlParticipante.Visible = true;
                    btnParticipantes.CssClass = btnParticipantes.CssClass.Replace("disabled", "") + (Enabled ? "" : " disabled");
                    oBasic.FixPanel(divData, "Tracing", Enabled ? 2 : 0, pList: true);
                    upRadicado.Update();
                    break;

                case "Radicado":
                    pnlTramite.Visible = true;
                    oBasic.FixPanel(divData, "Tracing", Enabled ? 2 : 0, pList: true);
                    upRadicado.Update();
                    break;

            }

        }
        private void ViewDetail()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.CONSULTAR, false, false)) return;

            Enabled = false;
            ViewState["ST" + ControlID] = "Detail";
            oBasic.EnableControls(pnlDetail, false, true);
            oBasic.EnableControls(pnlGridRadicado, true, true);
            pnlGridRadicado.Enabled = true;
            LoadControl();
            oBasic.FixPanel(divData, "Tracing", 0, pList: true);
            upTracingFoot.Update();
            upDetail.Update();
            upRadicado.Update();
        }
        private void ViewEdit()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.EDITAR, true)) return;

            Enabled = true;
            ViewState["ST" + ControlID] = "Detail";
            LoadControl();
            LoadValidators("Detail", true);
            oBasic.FixPanel(divData, "Tracing", 2);
            oBasic.EnableControls(pnlDetail, true, true);
            txt_estado_actividad.Enabled = false;
            oBasic.AlertSection(msgTracing, clConstantes.MSG_U, "warning");
        }
        private void ViewEditParticipante()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.EDITAR, true)) return;

            ViewState["ST" + ControlID] = "Participante";
            Enabled = true;
            LoadControl();
            LoadValidators("Participante", true);

            oBasic.FixPanel(divData, "Tracing", 2);
            oBasic.EnableControls(pnlParticipante, true, true);
            uctbgParticipantes.Enabled = true;
            oBasic.AlertSection(msgTracing, clConstantes.MSG_U, "info");
        }
        private void ViewEditRadicado()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.EDITAR, true)) return;

            ViewState["ST" + ControlID] = "Radicado";
            Enabled = true;
            LoadControl();
            LoadValidators("Radicado", true);

            oBasic.FixPanel(divData, "Tracing", 2);
            oBasic.EnableControls(pnlTramite, true, true);
            oBasic.AlertSection(msgTracing, clConstantes.MSG_U, "info");
        }
        private void ViewList()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.CONSULTAR, false, false)) return;

            ViewState["ST" + ControlID] = "Grid";
            Enabled = false;
            LoadControl();
        }
        private void ViewParticipante()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.CONSULTAR, false, false)) return;

            Enabled = false;
            ViewState["ST" + ControlID] = "Participante";
            LoadControl();
            oBasic.EnableControls(pnlParticipante, false, true);
            btnParticipantes.Enabled = true;
            uctbgParticipantes.Enabled = false;
            oBasic.FixPanel(divData, "Tracing", 0, pList: true);
        }
        private void ViewRadicado()
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.CONSULTAR, false, false)) return;

            Enabled = false;
            ViewState["ST" + ControlID] = "Radicado";
            LoadControl();
            oBasic.EnableControls(pnlTramite, false, true);
            lblPdfTramite.Enabled = true;
            oBasic.FixPanel(divData, "Tracing", 0, pList: true);
        }

        #endregion

        protected void ddl_identidad_radicado_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDropDown_Identidad(ddlt_idtramite, "76", " = 0" + ddl_identidad_radicado.SelectedValue);
        }
    }
}