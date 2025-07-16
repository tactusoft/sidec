using GLOBAL.DAL;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using OfficeOpenXml;
using SigesTO;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec.UserControls.PrediosDeclarados
{
    public partial class Carta : UserControl
    {
        private readonly CARTA_TERMINOS_DAL oCarta = new CARTA_TERMINOS_DAL();
        private readonly USUARIOS_DAL oUsuarios = new USUARIOS_DAL();

        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();
        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clUtil oUtil = new clUtil();

        private const string _SOURCEPAGE = "Carta";

        #region Propiedades
        /// <summary>
        /// Identificador de un registro específico
        /// </summary>
        public int CartaID
        {
            get
            {
                return (int.TryParse(hddCartaPrimary.Value, out int id) ? id : 0);
            }
            set
            {
                hddCartaPrimary.Value = value.ToString();
                hdd_idcarta.Value = hddCartaPrimary.Value;
            }
        }
        /// <summary>
        /// Identificador del control en caso de requerirse varios en un mismo formulario
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
        /// <summary>
        /// Estado de edición en el control
        /// </summary>
        public bool Enabled
        {
            get
            {
                return (Session[ControlID + ".Carta.Enabled"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".Carta.Enabled"] = value ? "1" : "0";
            }
        }
        /// <summary>
        /// Identificador del padre de la informción que se maneja en el control
        /// </summary>
        public int ReferenceID
        {
            get
            {
                return (Int32.TryParse(hddReferenceID.Value, out int id) ? id : 0);
            }
            set
            {
                hddReferenceID.Value = value.ToString();
                oBasic.AlertMain(msgCartasMain, "", "0");
                ViewControls(false);
            }
        }
        /// <summary>
        /// Esta propiedad solo se debe asignar para asegurar que el contenedor de control requiera un usuario responsable
        /// </summary>
        public string ResponsibleUserCode
        {
            get
            {
                return (Session[ControlID + ".Carta.ResponsibleUserCode"] ?? "NoRequired").ToString();
            }
            set
            {
                Session[ControlID + ".Carta.ResponsibleUserCode"] = value;
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
                ViewControls(false);
            }
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            if (hdd_idcarta.Value != "0")
            {
                MessageBox1.ShowConfirmation("EDIT", "¿Está seguro de actualizar la información?", type: "warning");
            }
            else
                MessageBox1.ShowConfirmation("ADD", "¿Está seguro de continuar con la acción solicitada?");
        }
        protected void btnCartaAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgCartasMain, "", "0");

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    LoadGrid();
                    break;
                case "Editar":
                    if (!ValidateAccess(cnsSection.PRED_DECL_CARTA, cnsAction.EDITAR)) return;
                    ViewEdit();
                    break;
                case "Agregar":
                    if (!ValidateAccess(cnsSection.PRED_DECL_CARTA, cnsAction.INSERTAR)) return;
                    ViewAdd();
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.PRED_DECL_CARTA, cnsAction.ELIMINAR)) return;
                    ViewDelete();
                    return;
            }
        }
        protected void btnCartaAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateAccess(cnsSection.PRED_DECL_CARTA, cnsAction.INSERTAR)) return;
            ViewAdd();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hdd_idcarta.Value = hddCartaPrimary.Value;
            Enabled = false;
            if (hdd_idcarta.Value == "0")
                LoadGrilla();
            else
                LoadDetail();
        }
        protected void gvCartas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvCartas, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvCartas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            oBasic.AlertMain(msgCartasMain, "", "0");
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvCartas.Rows.Count)
                    rowIndex = 0;
                CartaID = rowIndex >= 0 ? Convert.ToInt32(gvCartas.DataKeys[rowIndex]["id_carta_terminos"].ToString()) : 0;

                switch (e.CommandName)
                {
                    case "_Detail":
                        ViewControls(true);
                        Enabled = false;
                        LoadDetail();
                        break;
                    case "_Delete":
                        if (!ValidateAccess(cnsSection.PRED_DECL_CARTA, cnsAction.ELIMINAR)) return;
                        ViewDelete();
                        break;
                    case "_Edit":
                        if (!ValidateAccess(cnsSection.PRED_DECL_CARTA, cnsAction.EDITAR)) return;
                        ViewEdit();
                        break;
                    default:
                        return;
                }
            }
        }
        protected void MessageBox_Accept(string key)
        {
            try
            {
                switch (key)
                {
                    case "ADD":
                        Save();
                        break;
                    case "EDIT":
                        Save();
                        break;
                    case "DELETE":
                        Delete();
                        break;
                    default:
                        break;
                }
                LoadGrid();
                ViewControls(false);
                gvCartas.HeaderRow.Focus();
            }
            catch (Exception e)
            {

            }
        }
        #endregion



        #region Métodos Públicos
        public void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.PRED_DECL_CARTA, cnsAction.CONSULTAR, false))
            {
                hdd_idcarta.Value = "-100";
                gvCartas.EmptyDataText = "No cuenta con permisos suficientes para realizar esta acción";

                LoadGrilla();
                return;
            }

            LoadGrilla();
            upCarta.Update();
        }
        public void LoadReport()
        {
            string LETTER_TEMPLATE = "Plantilla_Cartas_Terminos.xlsx";
            string _terms_letter_template = oVar.prPathFormatosOrigen.ToString() + LETTER_TEMPLATE;

            if (File.Exists(_terms_letter_template))
            {
                //Archivo Excel del cual crear la copia:
                FileInfo templateFile = new FileInfo(_terms_letter_template);
                string file_name = "RadicadosCartasSeguimiento_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";

                using (ExcelPackage pck = new ExcelPackage(templateFile, true))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet ws = pck.Workbook.Worksheets["DATOS NOTIFICACIÓN"];
                    LoadReportUcLettersList(ws);
                    pck.Workbook.Calculate();
                    oUtil.fExcelSave(pck, file_name, false);
                }
            }
        }
        #endregion



        #region Métodos Privados
        private void Delete()
        {
            if (!ValidateAccess(cnsSection.PRED_DECL_CARTA, cnsAction.ELIMINAR)) return;

            string strResult = oCarta.sp_d_carta_terminos(hdd_idcarta.Value);
            oBasic.AlertUserControl(msgCartas, msgCartasMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d");
            CartaID = 0;
        }
        private void Initialize()
        {
            cal_fecha_radicado_salida.StartDate = cal_fecha_entrega.StartDate = cal_fecha_devolucion.StartDate = cal_fecha_manifestacion.StartDate =
                cal_fecha_licencia.StartDate = cal_fecha_respuesta.StartDate = cal_fecha_entrega_respuesta.StartDate = cal_fecha_devolucion_respuesta.StartDate =
                cal_fecha_primera_notif.StartDate = cal_fecha_recurso1.StartDate = cal_fecha_resol_recurso1.StartDate = cal_fecha_notif_resol1.StartDate =
                cal_fecha_segunda_notif.StartDate = cal_fecha_recurso2.StartDate = cal_fecha_resol_recurso2.StartDate = cal_fecha_notif_resol2.StartDate =
                cal_fecha_resol_recurso3.StartDate = cal_fecha_notif_resol3.StartDate =
                cal_fecha_firmeza_ejec1.StartDate = cal_fecha_expedicion_ejec1.StartDate = cal_fecha_firmeza_ejec2.StartDate = cal_fecha_expedicion_ejec2.StartDate = cal_fecha_firmeza_ejec3.StartDate = cal_fecha_expedicion_ejec3.StartDate = new DateTime(2008, 1, 1);

            rv_fecha_radicado_salida.MinimumValue = rv_fecha_entrega.MinimumValue = rv_fecha_devolucion.MinimumValue = rv_fecha_manifestacion.MinimumValue =
                rv_fecha_licencia.MinimumValue = rv_fecha_respuesta.MinimumValue = rv_fecha_entrega_respuesta.MinimumValue = rv_fecha_devolucion_respuesta.MinimumValue =
                rv_fecha_primera_notif.MinimumValue = rv_fecha_recurso1.MinimumValue = rv_fecha_resol_recurso1.MinimumValue = rv_fecha_notif_resol1.MinimumValue =
                rv_fecha_segunda_notif.MinimumValue = rv_fecha_recurso2.MinimumValue = rv_fecha_resol_recurso2.MinimumValue = rv_fecha_notif_resol2.MinimumValue =
                rv_fecha_resol_recurso3.MinimumValue = rv_fecha_notif_resol3.MinimumValue =
                rv_fecha_firmeza_ejec1.MinimumValue = rv_fecha_expedicion_ejec1.MinimumValue = rv_fecha_firmeza_ejec2.MinimumValue = rv_fecha_expedicion_ejec2.MinimumValue = rv_fecha_firmeza_ejec3.MinimumValue = rv_fecha_expedicion_ejec3.MinimumValue = (new DateTime(2008, 1, 1)).ToString("yyyy-MM-dd");

            cal_fecha_radicado_salida.EndDate = cal_fecha_entrega.EndDate = cal_fecha_devolucion.EndDate = cal_fecha_manifestacion.EndDate =
                cal_fecha_licencia.EndDate = cal_fecha_respuesta.EndDate = cal_fecha_entrega_respuesta.EndDate = cal_fecha_devolucion_respuesta.EndDate = 
                cal_fecha_primera_notif.EndDate = cal_fecha_recurso1.EndDate = cal_fecha_resol_recurso1.EndDate = cal_fecha_notif_resol1.EndDate =
                cal_fecha_segunda_notif.EndDate = cal_fecha_recurso2.EndDate = cal_fecha_resol_recurso2.EndDate = cal_fecha_notif_resol2.EndDate =
                cal_fecha_resol_recurso3.EndDate = cal_fecha_notif_resol3.EndDate =
                cal_fecha_firmeza_ejec1.EndDate = cal_fecha_expedicion_ejec1.EndDate = cal_fecha_firmeza_ejec2.EndDate = cal_fecha_expedicion_ejec2.EndDate = cal_fecha_firmeza_ejec3.EndDate = cal_fecha_expedicion_ejec3.EndDate = DateTime.Today;

            rv_fecha_radicado_salida.MaximumValue = rv_fecha_entrega.MaximumValue = rv_fecha_devolucion.MaximumValue = rv_fecha_manifestacion.MaximumValue =
                rv_fecha_licencia.MaximumValue = rv_fecha_respuesta.MaximumValue = rv_fecha_entrega_respuesta.MaximumValue = rv_fecha_devolucion_respuesta.MaximumValue =
                rv_fecha_primera_notif.MaximumValue = rv_fecha_recurso1.MaximumValue = rv_fecha_resol_recurso1.MaximumValue = rv_fecha_notif_resol1.MaximumValue =
                rv_fecha_segunda_notif.MaximumValue = rv_fecha_recurso2.MaximumValue = rv_fecha_resol_recurso2.MaximumValue = rv_fecha_notif_resol2.MaximumValue =
                rv_fecha_resol_recurso3.MaximumValue = rv_fecha_notif_resol3.MaximumValue =
                rv_fecha_firmeza_ejec1.MaximumValue = rv_fecha_expedicion_ejec1.MaximumValue = rv_fecha_firmeza_ejec2.MaximumValue = rv_fecha_expedicion_ejec2.MaximumValue = rv_fecha_firmeza_ejec3.MaximumValue = rv_fecha_expedicion_ejec3.MaximumValue = (DateTime.Today).ToString("yyyy-MM-dd");
        }
        private void LoadDetail()
        {
            DataSet dSet = oCarta.sp_s_carta_terminos(hdd_idcarta.Value);
            if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
            {
                DataRow dRow = dSet.Tables[0].Rows[0];

                oBasic.fValueControls(pnlPBCartaActions, dRow);
            }
            oBasic.EnableControls(pnlPBCartaActions, Enabled, true);
            oBasic.FixPanel(divData, "Carta", Enabled ? 2 : 0, pList: true);
        }
        private void LoadDropDowns()
        {
            LoadDropDownList_usuarios(ddl_prof_busqueda_info);
            LoadDropDownList_usuarios(ddl_prof_elab_carta);
            LoadDropDownList_usuarios(ddl_prof_revision);
            LoadDropDownList_usuarios(ddl_prof_elab_carta_par);
            LoadDropDownList_usuarios(ddl_prof_revision_par);
            LoadDropDownList_usuarios(ddl_prof_elab_respuesta);
            LoadDropDownList_usuarios(ddl_prof_rev_respuesta);
        }
        private void LoadDropDownList_usuarios(DropDownList ddl_usuario)
        {
            ddl_usuario.Items.Clear();
            ddl_usuario.DataSource = oUsuarios.sp_s_usuarios_filtro(3, -1);
            ddl_usuario.DataTextField = "nombre_completo";
            ddl_usuario.DataValueField = "cod_usuario";
            ddl_usuario.DataBind();
            ddl_usuario.Items.Insert(0, new ListItem("--Seleccione opción", "-1"));
        }
        private void LoadGrilla()
        {
            gvCartas.DataSource = oCarta.sp_s_cartas_terminos(ReferenceID.ToString());
            gvCartas.DataBind();
            gvCartas.SelectedIndex = 0;
            ViewControls(false);

            oBasic.FixPanel(divData, "Carta", 0, pAdd: false, pEdit: false, pDelete: false);
        }
        private void LoadReportUcLettersList(ExcelWorksheet ws)
        {
            int currentRow = 5;
            DataSet ds = oCarta.sp_s_cartas_terminos_reporte();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ws.Cells[currentRow, 1, currentRow, 700].Copy(ws.Cells[currentRow + 1, 1, currentRow + 1, 700]);
                    ws.InsertRow(currentRow + 1, 1, currentRow);

                    int column = 0;

                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["profesional_busqueda_info"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["pospredio"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["profesional_elab_carta"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["profesional_revision"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["profesional_elab_carta_par"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["profesional_revision_par"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["envio_carta"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["chip"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["direccion"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["localidad"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["radicado_salida"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_radicado_salida"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["obs_rev_info"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["nombre_receptor"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_entrega"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["motivo_devolucion"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_devolucion"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["radicado_entrada"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_manifestacion"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["resumen_manifestacion"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["tipo_licencia"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["radicado_licencia"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_licencia"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["obs_adicionales"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["profesional_elab_respuesta"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["profesional_rev_respuesta"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["resumen_respuesta_sdht"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["radicado_respuesta_salida"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_respuesta"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["nombre_receptor_respuesta"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_entrega_respuesta"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["motivo_devolucion_respuesta"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_devolucion_respuesta"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_primera_notif"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["tipo_notif_primera"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["num_propietarios_notif1"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["nombre_prop_notif1"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["dir_corresp_fisica1"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["dir_corresp_electronica1"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_recurso1"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["radicado_recurso1"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_resol_recurso1"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["num_acto_admin1"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_notif_resol1"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["tipo_notif_resol1"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_segunda_notif"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["tipo_notif_segunda"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["num_propietarios_notif2"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["nombre_prop_notif2"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["dir_corresp_fisica2"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["dir_corresp_electronica2"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_recurso2"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["radicado_recurso2"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_resol_recurso2"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["num_acto_admin2"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_notif_resol2"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["tipo_notif_resol2"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_resol_recurso3"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["num_acto_admin3"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_notif_resol3"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["tipo_notif_resol3"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["num_propietarios_notif3"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["nombre_prop_notif3"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["dir_corresp_fisica3"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["dir_corresp_electronica3"].ToString());
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_firmeza_ejec1"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_expedicion_ejec1"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_firmeza_ejec2"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_expedicion_ejec2"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_firmeza_ejec3"].ToString(), isDate: true);
                    oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_expedicion_ejec3"].ToString(), isDate: true);

                    //int pos = Convert.ToInt32(dr["pospredio"]);
                    //ws.Cells["A" + currentRow.ToString()].Style.Font.Color.SetColor(pos % 2 == 1 ? System.Drawing.Color.FromArgb(241, 169, 131) : System.Drawing.Color.FromArgb(221, 235, 245));

                    //pos = Convert.ToInt32(dr["poscartapredio"]);
                    //ws.Cells["A" + currentRow.ToString()].Style.Font.Color.SetColor(pos > 1 ? System.Drawing.Color.White : System.Drawing.Color.Black);
                    //ws.Cells["B" + currentRow.ToString()].Style.Font.Color.SetColor(pos > 1 ? System.Drawing.Color.White : System.Drawing.Color.Black);

                    currentRow++;
                }
            }
        }
        private void RegisterScript()
        {
            string key = "disabledlink";
            StringBuilder scriptSlider = new StringBuilder();
            scriptSlider.Append("<script type='text/javascript'> ");
            scriptSlider.Append("   $(document).ready(function () {");
            scriptSlider.Append("   disabledLink('tab');  ");
            scriptSlider.Append("       select_tab('tab', 0); ");
            scriptSlider.Append("    });");
            scriptSlider.Append(" </script> ");

            if (!Page.ClientScript.IsStartupScriptRegistered(key))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
            }

        }
        private void Save()
        {
            hdd_idcarta.Value = hdd_idcarta.Value == "" ? "0" : hdd_idcarta.Value;
            Save_Carta();
        }
        private string Save_Carta()
        {
            CartaTerminosTO cartaTerminos = new CartaTerminosTO()
            {
                IdPredioDeclarado = ReferenceID,
                ProfBusquedaInfo = Convert.ToInt32(ddl_prof_busqueda_info.SelectedValue),
                ProfElabCarta = Convert.ToInt32(ddl_prof_elab_carta.SelectedValue),
                ProfRevision = Convert.ToInt32(ddl_prof_revision.SelectedValue),
                ProfElabCartaPar = Convert.ToInt32(ddl_prof_elab_carta_par.SelectedValue),
                ProfRevisionPar = Convert.ToInt32(ddl_prof_revision_par.SelectedValue),
                EnvioCarta = txt_envio_carta.Text,
                RadicadoSalida = txt_radicado_salida.Text,
                FechaRadicadoSalida = oBasic.fDateTime(txt_fecha_radicado_salida),
                ObsRevInfo = txt_obs_rev_info.Text,
                NombreReceptor = txt_nombre_receptor.Text,
                FechaEntrega = oBasic.fDateTime(txt_fecha_entrega),
                MotivoDevolucion = txt_motivo_devolucion.Text,
                FechaDevolucion = oBasic.fDateTime(txt_fecha_devolucion),
                RadicadoEntrada = txt_radicado_entrada.Text,
                FechaManifestacion = oBasic.fDateTime(txt_fecha_manifestacion),
                ResumenManifestacion = ddl_resumen_manifestacion.SelectedValue,
                TipoLicencia = txt_tipo_licencia.Text,
                RadicadoLicencia = txt_radicado_licencia.Text,
                FechaLicencia = oBasic.fDateTime(txt_fecha_licencia),
                ObsAdicionales = txt_obs_adicionales.Text,
                ProfElabRespuesta = Convert.ToInt32(ddl_prof_elab_respuesta.SelectedValue),
                ProfRevRespuesta = Convert.ToInt32(ddl_prof_rev_respuesta.SelectedValue),
                ResumenRespuestaSdht = txt_resumen_respuesta_sdht.Text,
                RadicadoRespuestaSalida = txt_radicado_respuesta_salida.Text,
                FechaRespuesta = oBasic.fDateTime(txt_fecha_respuesta),
                NombreReceptorRespuesta = txt_nombre_receptor_respuesta.Text,
                FechaEntregaRespuesta = oBasic.fDateTime(txt_fecha_entrega_respuesta),
                MotivoDevolucionRespuesta = txt_motivo_devolucion_respuesta.Text,
                FechaDevolucionRespuesta = oBasic.fDateTime(txt_fecha_devolucion_respuesta),
                FechaPrimeraNotif = oBasic.fDateTime(txt_fecha_primera_notif),
                TipoNotifPrimera = txt_tipo_notif_primera.Text,
                NumPropietariosNotif1 = txt_num_propietarios_notif1.Text,
                NombrePropNotif1 = txt_nombre_prop_notif1.Text,
                DirCorrespFisica1 = txt_dir_corresp_fisica1.Text,
                DirCorrespElectronica1 = txt_dir_corresp_electronica1.Text,
                FechaRecurso1 = oBasic.fDateTime(txt_fecha_recurso1),
                RadicadoRecurso1 = txt_radicado_recurso1.Text,
                FechaResolRecurso1 = oBasic.fDateTime(txt_fecha_resol_recurso1),
                NumActoAdmin1 = txt_num_acto_admin1.Text,
                FechaNotifResol1 = oBasic.fDateTime(txt_fecha_notif_resol1),
                TipoNotifResol1 = txt_tipo_notif_resol1.Text,
                FechaSegundaNotif = oBasic.fDateTime(txt_fecha_segunda_notif),
                TipoNotifSegunda = txt_tipo_notif_segunda.Text,
                NumPropietariosNotif2 = txt_num_propietarios_notif2.Text,
                NombrePropNotif2 = txt_nombre_prop_notif2.Text,
                DirCorrespFisica2 = txt_dir_corresp_fisica2.Text,
                DirCorrespElectronica2 = txt_dir_corresp_electronica2.Text,
                FechaRecurso2 = oBasic.fDateTime(txt_fecha_recurso2),
                RadicadoRecurso2 = txt_radicado_recurso2.Text,
                FechaResolRecurso2 = oBasic.fDateTime(txt_fecha_resol_recurso2),
                NumActoAdmin2 = txt_num_acto_admin2.Text,
                FechaNotifResol2 = oBasic.fDateTime(txt_fecha_notif_resol2),
                TipoNotifResol2 = txt_tipo_notif_resol2.Text,
                FechaResolRecurso3 = oBasic.fDateTime(txt_fecha_resol_recurso3),
                NumActoAdmin3 = txt_num_acto_admin3.Text,
                FechaNotifResol3 = oBasic.fDateTime(txt_fecha_notif_resol3),
                TipoNotifResol3 = txt_tipo_notif_resol3.Text,
                NumPropietariosNotif3 = txt_num_propietarios_notif3.Text,
                NombrePropNotif3 = txt_nombre_prop_notif3.Text,
                DirCorrespFisica3 = txt_dir_corresp_fisica3.Text,
                DirCorrespElectronica3 = txt_dir_corresp_electronica3.Text,
                FechaFirmezaEjec1 = oBasic.fDateTime(txt_fecha_firmeza_ejec1),
                FechaExpedicionEjec1 = oBasic.fDateTime(txt_fecha_expedicion_ejec1),
                FechaFirmezaEjec2 = oBasic.fDateTime(txt_fecha_firmeza_ejec2),
                FechaExpedicionEjec2 = oBasic.fDateTime(txt_fecha_expedicion_ejec2),
                FechaFirmezaEjec3 = oBasic.fDateTime(txt_fecha_firmeza_ejec3),
                FechaExpedicionEjec3 = oBasic.fDateTime(txt_fecha_expedicion_ejec3)
            };

            if (hdd_idcarta.Value == "0")
            {
                string strResult = oCarta.sp_i_carta_terminos(cartaTerminos);
                oBasic.AlertUserControl(msgCartas, msgCartasMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i");
                return strResult.Split(':')[1];
            }
            else
            {
                cartaTerminos.IdCartaTerminos = Convert.ToInt32(hdd_idcarta.Value);
                string strResult = oCarta.sp_u_carta_terminos(cartaTerminos);
                oBasic.AlertUserControl(msgCartas, msgCartasMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u");
                return hdd_idcarta.Value;
            }
        }

        private bool ValidateAccess(string section, string action, bool validresponsible = true)
        {
            if (ResponsibleUserCode == "0" && validresponsible)
            {
                MessageInfo.ShowMessage("Esta acción requiere que se asigne un responsable al predio");
                return false;
            }
            string cod_usu_responsable = (ResponsibleUserCode == "NoRequired" ? oVar.prUserCod : ResponsibleUserCode).ToString();
            if (!oPermisos.TienePermisosAccion(section, action))
            {
                MessageInfo.ShowMessage("No cuenta con permisos suficientes para realizar esta acción");
                return false;
            }
            else if (!oPermisos.TienePermisosAccion(section, action, cod_usu_responsable, oVar.prUserCod.ToString()))
            {
                if (cod_usu_responsable.Contains("-"))
                {
                    MessageInfo.ShowMessage("Este registro no es editable. Si requiere su habilitación, por favor comunicarse con el administrador");
                    return false;
                }
                MessageInfo.ShowMessage("Para realizar esta acción comuníquese con el usuario responsable del proyecto");
                return false;
            }
            return true;
        }
        private void ViewAdd()
        {
            hdd_idcarta.Value = "0";
            ViewControls(true);
            Enabled = true;
            oBasic.EnableControls(pnlPBCartaActions, Enabled, true);
            oBasic.FixPanel(divData, "Carta", 2);
        }
        private void ViewControls(bool _visible)
        {
            dvCartaActions.Visible = _visible;
            gvCartas.Visible = !_visible;
            oBasic.fClearControls(dvCartaActions);
        }
        private void ViewDelete()
        {
            ViewControls(true);
            Enabled = false;
            LoadDetail();
            MessageBox1.ShowConfirmation("DELETE", "¿Esta seguro que desea eliminar el registro?", type: "danger");
        }
        private void ViewEdit()
        {
            ViewControls(true);
            Enabled = true;
            LoadDetail();
            upCarta.Update();
        }

        #endregion


    }
}