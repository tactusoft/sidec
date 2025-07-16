using GLOBAL.CONST;
using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;
using tipoArchivo = GLOBAL.CONST.clConstantes.TipoArchivo;

namespace SIDec.UserControls.ComIntersectorial
{
    public partial class Folios : UserControl
    {
        private readonly ARCHIVO_DAL oArchivo = new ARCHIVO_DAL();
        private readonly FOLIOS_DAL oFolios = new FOLIOS_DAL();

        private readonly clBasic oBasic = new clBasic();
        private readonly clFile oFile = new clFile();
        private readonly clLog oLog = new clLog();
        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clPermisos oPermisos = new clPermisos();

        private const string _SOURCEPAGE = "Foslios";
        private const string EXCEPTION_EXTENSION = "Extensión inválida, solo se permite «pdf»";

        public delegate void OnViewDocEventHandler(object sender);
        public event OnViewDocEventHandler ViewDoc;

        public delegate void OnUserControlExceptionEventHandler(object sender, Exception ex);
        public event OnUserControlExceptionEventHandler UserControlException;



        #region Propiedades
        public string CodUsuario
        {
            get
            {
                return (Session[ControlID + ".Folio.CodUsuario"] ?? "").ToString();
            }
            set
            {
                Session[ControlID + ".Folio.CodUsuario"] = value;
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
                return (Session[ControlID + ".Folio.Enabled"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".Folio.Enabled"] = value ? "1" : "0";
            }
        }
        public string Filter
        {
            get
            {
                return (Session[ControlID + ".Folio.Filter"] ?? "").ToString();
            }
            set
            {
                Session[ControlID + ".Folio.Filter"] = value;
            }
        }



        private DataSet FoliosLimits
        {
            get
            {
                return (DataSet)(Session[ControlID + ".Folio.Limits"]);
            }
            set
            {
                Session[ControlID + ".Folio.Limits"] = value;
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
            }

            ViewControls();
            ValidPdfProject();
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            switch (ViewState["ST" + ControlID])
            {
                case "Detail":
                    if (hdd_idfolio.Value != "0")
                    {
                        MessageBox1.ShowConfirmation("Detail", "¿Está seguro de actualizar la información?", type: "warning");
                    }
                    else
                        MessageBox1.ShowConfirmation("Detail", "¿Está seguro de continuar con la acción solicitada?");
                    break;
            }
        }
        protected void btnFichaExcel_Click(object sender, EventArgs e)
        {
            //oBasic.AlertMain(msgFolioMain, "", "0");
            //if (ddlAnio.SelectedValue == "")
            //{
            //    MessageBox1.ShowMessage("Seleccione el año a generar");
            //    return;
            //}

            if (gvFolio.Rows.Count > 0)
            {
                string FORMAT_PS03_F0379 = "PS03-FO379_CI.xlsx";
                string _plantilla_f039 = oVar.prPathFormatosOrigen.ToString() + FORMAT_PS03_F0379;

                if (File.Exists(_plantilla_f039))
                {
                    string NewName = FORMAT_PS03_F0379.Replace(".xlsx", "");

                    DataTable dt = oFolios.sp_s_folios(ddlAnio.SelectedValue, "").Tables[0];

                    //Archivo Excel del cual crear la copia:
                    FileInfo templateFile = new FileInfo(_plantilla_f039);
                    string year = ddlAnio.SelectedValue == "" ? "TOTAL" : ddlAnio.SelectedValue;
                    string fileName = NewName + "_" + year + ".xlsx";

                    using (ExcelPackage pck = new ExcelPackage(templateFile, true))
                    {
                        int TotalRegistros = dt.Rows.Count;
                        double LimiteImprimir = 0;
                        double iConsecutivo = 0;

                        //Filas que no se imprimen
                        int FilasFijas = 10;
                        int FilaImprimir = FilasFijas;
                        //Abrir primera Hoja del Archivo Excel que se crea'
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        ExcelWorksheet ws = pck.Workbook.Worksheets[0];

                        ws.Cells["C8"].Value = year;

                        foreach (DataRow row in dt.Rows)
                        {
                            string yearreg = ddlAnio.SelectedValue == "" ? row["anio"].ToString() + " - " : "";
                            ws.Cells["A" + FilaImprimir.ToString()].Value = Server.HtmlDecode(yearreg + row["orden"].ToString());
                            ws.Cells["B" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row["nombre"].ToString());
                            ws.Cells["C" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row["radicado"].ToString().Replace("&nbsp;", "N/A"));
                            ws.Cells["D" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row["fecha_radicado"].ToString().Replace("&nbsp;", "N/A"));
                            ws.Cells["E" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row["folios"].ToString());
                            ws.Cells["F" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row["folio_inicial"].ToString());
                            ws.Cells["G" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row["folio_final"].ToString());
                            ws.Cells["H" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row["usuario"].ToString());
                            ws.Cells["I" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row["observaciones"].ToString());

                            FilaImprimir++;
                            iConsecutivo = Convert.ToInt16(row["orden"].ToString().Replace("&nbsp;", ""));
                        }

                        //Buscar la centena mas cercana
                        LimiteImprimir = Math.Ceiling(iConsecutivo / 100) * 100;
                        iConsecutivo++;
                        for (int inc = FilaImprimir; iConsecutivo <= LimiteImprimir; inc++)
                        {
                            ws.Cells["A" + inc.ToString()].Value = iConsecutivo.ToString();
                            iConsecutivo++;
                        }

                        var Rango = ws.Cells["A10:I" + (LimiteImprimir + FilasFijas - 1).ToString()];

                        Rango.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        Rango.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        Rango.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        Rango.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        Response.Clear();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.BinaryWrite(pck.GetAsByteArray());
                        Response.End();

                    }
                    MessageInfo.ShowMessage(clConstantes.MSG_OK_FILE_CREADO);
                }
                else
                {
                    MessageInfo.ShowMessage("La plantilla PS03-F0379 no se encuentra. Comuníquese con el administrador del sistema");
                    oLog.RegistrarLogError("La plantilla PS03-F0379 no se encuentra :" + _plantilla_f039, _SOURCEPAGE, "btnFichaExcel_Click");
                    return;
                }
            }
        }
        protected void btnFolioAdd_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgFolioMain, "", "0");
            ViewAdd();
        }
        protected void btnFolioAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgFolioMain, "", "0");

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    LoadGrid();
                    break;
                case "Editar":
                    if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.EDITAR)) return;

                    Enabled = true;
                    break;
                case "Agregar":
                    ViewAdd();
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.ELIMINAR)) return;

                    MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar este registro?", type: "danger");
                    return;
            }
            ViewControls();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            oBasic.AlertMain(msgFolioMain, "", "0");
            ViewState["ST" + ControlID] = "Grid";

            Enabled = false;
            ViewControls();
        }
        protected void ddlAnio_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
        protected void gvFolio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                oBasic.AlertMain(msgFolioMain, "", "0");
                if (rowIndex >= gvFolio.Rows.Count)
                    rowIndex = 0;
                hdd_idfolio.Value = rowIndex >= 0 ? gvFolio.DataKeys[rowIndex]["idfolio"].ToString() : "0";

                switch (e.CommandName)
                {
                    case "Select":
                        ViewState["ST" + ControlID] = "Grid";
                        break;
                    case "_OpenFile":
                        string ruta = gvFolio.DataKeys[rowIndex]["ruta_archivo"].ToString();
                        oFile.GetPath(ruta);

                        ViewDoc?.Invoke(this);
                        break;
                    case "_Detail":
                        if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.CONSULTAR)) return;

                        ViewState["ST" + ControlID] = "Detail";
                        Enabled = false;
                        LoadDetail();
                        break;
                    case "_Delete":
                        if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.ELIMINAR)) return;

                        ViewState["ST" + ControlID] = "Detail";
                        Enabled = false;
                        LoadDetail();
                        MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar este folio?", type: "danger");
                        break;
                    case "_Edit":
                        if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.EDITAR)) return;

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
        protected void gvFolio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvFolio, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvFolio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFolio.PageIndex = e.NewPageIndex;
            LoadGrid();
            ViewState["IndexFolio"] = ((gvFolio.PageSize * gvFolio.PageIndex) + gvFolio.PageIndex - 1).ToString();
        }
        protected void lblPdfProject_Click(object sender, EventArgs e)
        {
            string pathFile = hdd_ruta_archivo.Value;
            oFile.GetPath(pathFile);

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
                        strResult = Save();
                        break;
                    case "Delete":
                        strResult = DeleteFolio();
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
        protected void txt_fecha_evento_TextChanged(object sender, EventArgs e)
        {
            LoadLimits();
        }
        protected void ucAnexos_UserControlException(object sender, Exception ex)
        {
            UserControlException?.Invoke(sender, ex);
        }
        protected void ucAnexos_ViewDoc(object sender)
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
        private void Clear()
        {
            oBasic.fClearControls(pnlFolio);
            lblInfoFileProject.Text = "";
            lblErrorFileProject.Text = "";
        }
        private string DeleteFolio()
        {
            hdd_idfolio.Value = hdd_idfolio.Value == "" ? "0" : hdd_idfolio.Value;

            string strResult = oFolios.sp_d_folio(hdd_idfolio.Value);
            if (oBasic.AlertUserControl(msgFolio, msgFolioMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d"))
            {
                ViewState["ST" + ControlID] = "Grid";
                LoadControl();
                ViewControls();
                upFolioFoot.Update();
            }

            return strResult;
        }
        private void EvaluateFolios()
        {
            if (!Int32.TryParse(txt_folio_inicial.Text, out int folioInicial))
            {
                folioInicial = 0;
            }
            if (!Int32.TryParse(txt_folios.Text, out int numeroFolios))
            {
                numeroFolios = 0;
            }

            oBasic.fValueTextBox(txt_folio_final, "int", (numeroFolios == 0 ? "0" : (folioInicial + numeroFolios - 1).ToString()));
            upDetail.Update();
        }
        private void Initialize()
        {
            ViewState["IndexFolio"] = "0";
            ViewState["SortExpFolio"] = "folio_inicial";
            ViewState["SortDirFolio"] = "DESC";
            ViewState["ST" + ControlID] = "Grid";

            cefecha_evento.StartDate = cefecha_radicado.StartDate = new DateTime(2000, 1, 1);
            rvfecha_evento.MinimumValue = rvfecha_radicado.MinimumValue = (new DateTime(2000, 1, 1)).ToString("yyyy-MM-dd");
            cefecha_evento.EndDate = cefecha_radicado.EndDate = DateTime.Today;
            rvfecha_evento.MaximumValue = rvfecha_radicado.MaximumValue = (DateTime.Today).ToString("yyyy-MM-dd");

            FoliosLimits = oFolios.sp_s_foliolimites();
            foreach (DataRow dr in FoliosLimits.Tables[0].Rows)
            {
                ddlAnio.Items.Insert(0, new ListItem(dr["anio"].ToString(), dr["anio"].ToString()));
            }
            ddlAnio.Items.Insert(0, new ListItem("Seleccione", ""));
        }
        private void LoadDetail()
        {
            if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.CONSULTAR)) return;

            FoliosLimits = oFolios.sp_s_foliolimites();

            Clear();
            DataSet dSet = oFolios.sp_s_folio(hdd_idfolio.Value);
            DataRow dRow = dSet.Tables[0].Rows.Count > 0 ? dSet.Tables[0].Rows[0] : dSet.Tables[0].NewRow();

            oBasic.fValueControls(pnlFolio, dRow);
            hdd_idfolio.Value = hdd_idfolio.Value.Trim() == "" ? "0" : hdd_idfolio.Value;

            ucAnexos.SectionPermission = cnsSection.PROY_ASOC_VISITAS;
            ucAnexos.Prefix = tipoArchivo.ANCI;
            ucAnexos.FilePath = oVar.prPathComisionIntersectorial.ToString();
            ucAnexos.ReferenceID = Convert.ToInt32(hdd_idfolio.Value);
            ucAnexos.ArchivoID = Convert.ToInt32(("0" + (dRow["idarchivo"] ?? "-1")).ToString());
            ucAnexos.Enabled = true;
            ucAnexos.LoadGrid();

            upAnexos.Update();

        }
        private void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.CONSULTAR)) return;

            ViewState["ST" + ControlID] = "Grid";
            gvFolio.DataSource = oFolios.sp_s_folios(ddlAnio.SelectedValue, Filter);
            gvFolio.DataBind();

            oBasic.FixPanel(divData, "Folios", 0, pAdd: true, pEdit: false, pDelete: false);
        }
        private void LoadLimits()
        {
            DataView dv = ((DataSet)FoliosLimits).Tables[0].DefaultView;
            if (DateTime.TryParse(txt_fecha_evento.Text, out DateTime fe))
                dv.RowFilter = "anio = " + fe.Year.ToString() + "";
            else if (ddlAnio.SelectedValue != "")
            {
                dv.RowFilter = "anio = " + ddlAnio.SelectedValue + "";
            }
            dv.Sort = "reg desc, anio desc";
            if (dv.Count > 0)
            {
                DataRow dr = dv[0].Row;
                txt_fecha_evento.Text = txt_fecha_evento.Text.Trim() != "" ? txt_fecha_evento.Text :
                                               (dr["reg"].ToString() != "" ? Convert.ToDateTime(dr["reg"].ToString()).ToString("yyyy-MM-dd") :
                                                    (ddlAnio.SelectedValue != "" ? (new DateTime(Convert.ToInt32(ddlAnio.SelectedValue), 1, 1)).ToString("yyyy-MM-dd") : ""));

                oBasic.fValueTextBox(txt_carpeta, "int", dr["carpeta"].ToString());
                oBasic.fValueTextBox(txt_folio_inicial, "int", dr["folio_final"].ToString());
            }
            EvaluateFolios();
        }
        private void RegisterScript()
        {
            Page.Form.Enctype = "multipart/form-data";
            lbLoadProject.Attributes.Add("onclick", "$(\"input[ID*='" + fuLoadProject.ClientID + "']\").click();return false;");
        }
        private string Save()
        {
            hdd_idfolio.Value = hdd_idfolio.Value == "" ? "0" : hdd_idfolio.Value;

            string strResult = SaveFolioFile();
            if (strResult.Substring(0, 5) != clConstantes.DB_ACTION_OK)
                return strResult;

            strResult = SaveFolio();
            if (strResult.Substring(0, 5) != clConstantes.DB_ACTION_OK)
                return strResult;

            Enabled = false;
            oBasic.EnableControls(pnlFolio, false, true);
            LoadControl();
            ViewControls();
            upFolioFoot.Update();
            upDetail.Update();

            return clConstantes.DB_ACTION_OK;
        }
        private string SaveFolio()
        {
            int folioFinal = Convert.ToInt32(txt_folio_inicial.Text) + Convert.ToInt32(txt_folios.Text) - 1;

            if (hdd_idfolio.Value == "0")
            {
                string strResult = oFolios.sp_i_folio(oBasic.fDateTime(txt_fecha_evento), oBasic.fInt(txt_carpeta), txt_nombre.Text.Trim(), txt_radicado.Text.Trim(), oBasic.fDateTime(txt_fecha_radicado),
                oBasic.fInt(txt_folio_inicial), folioFinal, txt_observaciones.Text.Trim(), hdd_idarchivo.Value);
                if (strResult.Substring(0, 5) == clConstantes.DB_ACTION_ERR_DATOS)
                {
                    MessageInfo.ShowMessage(strResult.Substring(6));
                    return clConstantes.DB_ACTION_ERR_DATOS;
                }
                else if (oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i"))
                {
                    hdd_idfolio.Value = hdd_idfolio.Value == "0" ? (strResult.Split(':'))[1] : hdd_idfolio.Value;
                }
                else
                {
                    MessageInfo.ShowMessage("Error creando el folio");
                    //oLog.RegistrarLogError("Error creando folio. " + strResult.Substring(6), _SOURCEPAGE, "ProyectosEdit");
                    return strResult.Substring(6);
                }
            }
            else
            {
                string strResult = oFolios.sp_u_folio(oBasic.fInt(hdd_idfolio), oBasic.fDateTime(txt_fecha_evento), oBasic.fInt(txt_carpeta), txt_nombre.Text.Trim(), txt_radicado.Text.Trim(), oBasic.fDateTime(txt_fecha_radicado),
                    oBasic.fInt(txt_folio_inicial), folioFinal, txt_observaciones.Text.Trim(), hdd_idarchivo.Value);
                if (strResult.Substring(0, 5) == clConstantes.DB_ACTION_ERR_DATOS)
                {
                    MessageInfo.ShowMessage(strResult.Substring(6));
                    return clConstantes.DB_ACTION_ERR_DATOS;
                }
                if (!oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u"))
                {
                    MessageInfo.ShowMessage("Error creando el folio");
                    //oLog.RegistrarLogError("Error creando folio. " + strResult.Substring(6), _SOURCEPAGE, "ProyectosEdit");
                    return strResult.Substring(6);
                }
                ViewState["ST" + ControlID] = "Grid";
            }
            return clConstantes.DB_ACTION_OK;
        }
        private string SaveFolioFile()
        {
            hdd_idarchivo.Value = hdd_idarchivo.Value == "" ? "0" : hdd_idarchivo.Value;
            if (fuLoadProject.HasFile)
            {
                string contentType = fuLoadProject.ContentType;
                string strResult, fileName, pathFile;

                if (contentType == "application/pdf")
                {
                    string anio = txt_fecha_evento.Text.Split('-')[0];
                    fileName = tipoArchivo.FLCI.ToString() + "_" + anio + "_" + txt_folio_inicial.Text + "_" + txt_folio_final.Text + "_" + Guid.NewGuid() + Path.GetExtension(fuLoadProject.FileName);
                    pathFile = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);
                    try
                    {
                        fuLoadProject.SaveAs(pathFile);
                    }
                    catch (Exception e)
                    {
                        MessageInfo.ShowMessage("Error cargando el archivo");
                        oLog.RegistrarLogError("Error cargando archivo " + e.Message + ":" + fuLoadProject.FileName + ":::" + fuLoadProject.PostedFile.ContentLength, _SOURCEPAGE, "SaveFolioFile");
                        return e.Message;
                    }

                    try
                    {
                        if (hdd_idarchivo.Value == "0")
                            strResult = oArchivo.sp_i_archivo(0, Convert.ToInt32(tipoArchivo.FLCI), tipoArchivo.FLCI.ToString(), fuLoadProject.FileName, pathFile, "");
                        else
                            strResult = oArchivo.sp_u_archivo(Convert.ToInt32(hdd_idarchivo.Value), 0, Convert.ToInt32(tipoArchivo.FLCI), tipoArchivo.FLCI.ToString(), fuLoadProject.FileName, pathFile, "");

                        if (strResult.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                            hdd_idarchivo.Value = hdd_idarchivo.Value == "0" ? hdd_idarchivo.Value = strResult.Split(':')[1] : hdd_idarchivo.Value;
                        else
                        {
                            MessageInfo.ShowMessage("Error cargando el archivo");
                            oLog.RegistrarLogError("Error subiendo archivo " + strResult.Substring(6) + ":" + fuLoadProject.FileName + ":::" + fuLoadProject.PostedFile.ContentLength, _SOURCEPAGE, "SaveFolio");
                            return strResult.Substring(6);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageInfo.ShowMessage("Error cargando el archivo");
                        oLog.RegistrarLogError("Error subiendo archivo " + e.Message + ":" + fuLoadProject.FileName + ":::" + fuLoadProject.PostedFile.ContentLength, _SOURCEPAGE, "SaveFolio");
                        return e.Message;
                    }

                }
            }
            return clConstantes.DB_ACTION_OK;
        }
        private bool ValidateAccess(string section, string action, bool validateResponsible = false, bool requeridedResponsible = false)
        {
            string message = oPermisos.ValidateAccess(section, action, CodUsuario, validateResponsible, requeridedResponsible);

            if (message == "")
                return true;

            MessageInfo.ShowMessage(message);
            return false;
        }
        protected void ValidPdfProject()
        {
            if (fuLoadProject.HasFile)
            {
                string extension = Path.GetExtension(fuLoadProject.FileName).Substring(1);
                bool isValidExtension = extension.ToLower() == "pdf";

                if (!isValidExtension)
                {
                    lblErrorFileProject.Attributes.Add("title", EXCEPTION_EXTENSION);
                    lblErrorFileProject.Text = EXCEPTION_EXTENSION;
                    lblInfoFileProject.Text = "";
                    fuLoadProject.ClearAllFilesFromPersistedStore();
                }
                else
                {
                    lblErrorFileProject.Attributes.Remove("title");
                    lblErrorFileProject.Text = "";
                    lblInfoFileProject.Text = fuLoadProject.FileName;
                }
            }
            else
            {
                lblErrorFileProject.Attributes.Remove("title");
                lblErrorFileProject.Text = "";
            }
            RegisterScript();
        }
        private void ViewAdd()
        {
            if (!ValidateAccess(cnsSection.COM_INTERSEC_FOLIOS, cnsAction.INSERTAR)) return;

            Clear();
            ViewState["ST" + ControlID] = "Detail";
            hdd_idfolio.Value = "0";
            Enabled = true;
            LoadControl();
            LoadLimits();

            oBasic.FixPanel(divData, "Folio", 2);
            oBasic.EnableControls(pnlFolio, true, true);

            pnlAnexos.Visible = false;
            upAnexos.Update();
        }
        private void ViewControls()
        {
            ViewState["ST" + ControlID] = ViewState["ST" + ControlID] ?? "Grid";

            pnlGrid.Visible = false;
            pnlDetail.Visible = false;
            pnlAnexos.Visible = false;

            switch (ViewState["ST" + ControlID].ToString())
            {
                case "Grid":
                    ViewState["IndexFolio"] = ViewState["IndexFolio"] ?? "0";
                    ViewState["SortExpFolio"] = ViewState["SortExpFolio"] ?? "folio_inicial";
                    ViewState["SortDirFolio"] = ViewState["SortDirFolio"] ?? "DESC";
                    pnlGrid.Visible = true;
                    oBasic.FixPanel(divData, "Folio", 0, pList: false, pAdd: true, pEdit: false, pDelete: false);
                    btnFichaExcel.Visible = true;
                    upFolio.Update();
                    break;

                case "Detail":
                    pnlDetail.Visible = true;
                    pnlAnexos.Visible = true && hdd_idfolio.Value != "0";

                    oBasic.EnableControls(pnlFolio, Enabled, true);
                    oBasic.FixPanel(divData, "Folio", Enabled ? 2 : 0, pList: true);
                    btnFichaExcel.Visible = false;

                    lblPdfProject.Visible = hdd_ruta_archivo.Value != "";
                    lblPdfProject.Enabled = true;

                    lbLoadProject.Visible = Enabled;
                    upDetail.Update();
                    break;
            }
        }
        #endregion




    }
}