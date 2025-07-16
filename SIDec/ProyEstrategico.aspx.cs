using GLOBAL.CONST;
using GLOBAL.DAL;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Filter;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;
using tipoArchivo = GLOBAL.CONST.clConstantes.TipoArchivo;

namespace SIDec
{
    public partial class ProyEstrategico : Page
    {
        #region Propiedades
        private readonly BANCOPROYECTOS_DAL oProyEstrategico = new BANCOPROYECTOS_DAL();

        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clUtil oUtil = new clUtil();
        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();

        private const string _SOURCEPAGE = "ProyEstrategico Proyectos";
        private const int ROW_HEADER = 3;
        private const int ROW_TRACING = 30;
        private const int ROW_ACTIVITY = 24;
        private const int ROW_MANAGEMENT = 17;
        #endregion



        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            btnConfirmarProyEstrategico.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarProyEstrategico.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarProyEstrategico, "Click") + "; return false;");

            if (!IsPostBack)
            {
                Page.Form.Enctype = "multipart/form-data";
                ViewState["CriterioBuscar"] = "";
                ViewState["AccionFinal"] = "";

                ViewState["IndexProyEstrategico"] = "0";

                ViewState["SortExpProyEstrategico"] = "codigo";
                ViewState["SortDirProyEstrategico"] = "ASC";

                txtBuscar.Focus();
                Load_ProyEstrategico(ViewState["CriterioBuscar"].ToString());
            }
            VerifyImageFiles();
            btnImprimir.Visible = (oVar.prUserCod.ToString() == "101");            
        }
        protected void btnAddBank_Click(object sender, EventArgs e)
        {
            ViewAdd();
        }
        protected void btnProyEstrategicoNavegacion_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexProyEstrategico"]) - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexProyEstrategico"]) + 1;
                    break;
                case "First":
                    index = 0;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSBanco).Tables[0].Rows.Count - 1;
                    break;
            }
            gvProyEstrategico.PageIndex = (index - (index % gvProyEstrategico.PageSize)) / gvProyEstrategico.PageSize;
            gvProyEstrategico.DataSource = ((DataSet)oVar.prDSBanco).Tables[0];
            gvProyEstrategico.DataBind();
            gvProyEstrategico.SelectedIndex = index % gvProyEstrategico.PageSize;

            if (gvProyEstrategico.SelectedDataKey != null)
            {
                oVar.prBancoAu = gvProyEstrategico.SelectedDataKey["idbanco"].ToString();
                Session["ProyEstrategico.cod_usu_responsable"] = gvProyEstrategico.DataKeys[index % gvProyEstrategico.PageSize]["cod_usu_responsable"].ToString();
                Session["ProyEstrategico.idproyecto"] = gvProyEstrategico.DataKeys[index % gvProyEstrategico.PageSize]["idproyecto"].ToString();
                ViewState["IndexProyEstrategico"] = index.ToString();
                LoadDetail();
                oBasic.FixPanel(divData, "ProyEstrategico", 1);
                PB_Header.Enabled = false;
                PB_Header.LoadDetail();
                oBasic.FixPanel(divData, "FichaProyecto", 0, pList: (hdd_Proyecto_ProyEstrategico_Id.Value == ""), pAdd: (hdd_Proyecto_ProyEstrategico_Id.Value == ""));
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            mvProyEstrategico.ActiveViewIndex = 0;
            ViewState["CriterioBuscar"] = txtBuscar.Text.Trim();
            Load_ProyEstrategico(ViewState["CriterioBuscar"].ToString());
            gvProyEstrategico_SelectedIndexChanged(null, null);
        }
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            bool permiso = false;
            switch (ViewState["AccionFinal"].ToString())
            {
                case "_Edit":
                    if ((Session["ProyEstrategico.idproyecto"] ?? 0).ToString() == "0" || (Session["ProyEstrategico.idproyecto"] ?? 0).ToString() == "")
                    {
                        if (!ValidateAccess(cnsSection.FICHA_BANCO, cnsAction.EDITAR, true, false)) return;

                        Save_ProyEstrategico("u");
                    }
                    ViewDetail();
                    hddsectionview.Value = "collapse1";
                    Session["ReloadXFU"] = "1";
                    Load_ProyEstrategico(ViewState["CriterioBuscar"].ToString());
                    (Master as AuthenticNew).fReload();
                    return;
                case "_Add":
                    if (!ValidateAccess(cnsSection.FICHA_BANCO, cnsAction.INSERTAR, true, false)) return;
                    Save_ProyEstrategico("i");

                    ViewDetail();

                    Session["ReloadXFU"] = "1";
                    Load_ProyEstrategico(ViewState["CriterioBuscar"].ToString());
                    (Master as AuthenticNew).fReload();
                    return;
                case "_Delete":
                    if (!ValidateAccess(cnsSection.FICHA_BANCO, cnsAction.ELIMINAR, true)) return;
                    Delete_ProyEstrategico();
                    permiso = true;
                    break;
            }
            if (!permiso)
            {
                oBasic.AlertMain(msgMain, clConstantes.MSG_ERR_PERMISO, "danger");
                oBasic.AlertSection(msgProyEstrategico, "", "0");
            }
            Load_ProyEstrategico(ViewState["CriterioBuscar"].ToString());
            oUtil.fSetIndex(gvProyEstrategico);
            gv_SelectedIndexChanged(gvProyEstrategico);
            PB_Header.Enabled = false;
            oBasic.FixPanel(divData, "ProyEstrategico", 0);

            Session["ReloadXFU"] = "1";
            (Master as AuthenticNew).fReload();
        }
        protected void btnFichaProyectoAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = btnAccionSource.CommandName;

            switch (btnAccionSource.CommandName)
            {
                case "_List":
                    ViewList();
                    break;
                case "_Edit":
                    ViewEdit();
                    break;
                case "_Add":
                    ViewAdd();
                    break;
                case "_Delete":
                    ViewDelete();
                    return;
                case "_Excel":
                    LoadReport("Detail");
                    return;
            }
        }
        protected void btnFichaProyectoCancelar_Click(object sender, EventArgs e)
        {
            PB_Header.Enabled = false;
            PB_Header.LoadDetail();

            hddsectionview.Value = "collapse1";
            hddsectionview.Value = "collapse1";
            if (ViewState["AccionFinal"].ToString() == "_Add")
            {
                oBasic.FixPanel(divData, "ProyEstrategico", 0);
                oBasic.FixPanel(divData, "FichaProyecto", 2);
            }
            else
            {
                oBasic.FixPanel(divData, "FichaProyecto", 0, pList: (hdd_Proyecto_ProyEstrategico_Id.Value == ""), pAdd: (hdd_Proyecto_ProyEstrategico_Id.Value == ""));
                oBasic.FixPanel(divData, "ProyEstrategico", 2);
            }
            oBasic.AlertSection(msgFichaProyecto, "", "0");
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            if (gvProyEstrategico.Rows.Count > 0)
            {
                LoadReport("Grid");
            }
            else
                MessageInfo.ShowMessage("Seleccione al menos un proyecto para generar la ficha");
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Session["Retorno.Proyecto.Page"] = "PB";
            Response.Redirect("Proyectos");
        }
        #region Grillas
        protected void gvProyEstrategico_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProyEstrategico.PageIndex = e.NewPageIndex;
            Load_ProyEstrategico(ViewState["CriterioBuscar"].ToString());
            ViewState["IndexProyEstrategico"] = ((gvProyEstrategico.PageSize * gvProyEstrategico.PageIndex) + gvProyEstrategico.PageIndex - 1).ToString();
        }
        protected void gvProyEstrategico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            oBasic.AlertMain(msgMain, "", "0");
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvProyEstrategico.Rows.Count)
                    rowIndex = 0;
                if (rowIndex < 0) return;

                ViewState["IndexProyEstrategico"] = rowIndex.ToString();
                gvProyEstrategico.SelectedIndex = rowIndex;
                gv_SelectedIndexChanged(gvProyEstrategico);
                oVar.prBancoAu = gvProyEstrategico.DataKeys[rowIndex]["idbanco"].ToString();
                DataSet dsColaborators = oProyEstrategico.sp_s_banco_colaboradores(oVar.prBancoAu.ToString());

                string strQuery = string.Format("cod_usuario = '{0}'", oVar.prUserCod.ToString());
                DataRow[] oDr = ((DataSet)dsColaborators).Tables[0].Select(strQuery);

                Session["ProyEstrategico.cod_usu_responsable"] = (oDr.Length > 0 ? oVar.prUserCod.ToString() : gvProyEstrategico.DataKeys[rowIndex]["cod_usu_responsable"].ToString());
                Session["ProyEstrategico.idproyecto"] = gvProyEstrategico.DataKeys[rowIndex]["idproyecto"].ToString();
            }

            switch (e.CommandName)
            {
                case "_Detail":
                    ViewDetail();
                    hddsectionview.Value = "collapse1";
                    break;
                case "_Edit":
                    ViewEdit();
                    break;
                case "_Delete":
                    ViewDelete();
                    return;
                default:
                    return;
            }
            oBasic.FixPanel(divData, "ProyEstrategico", 1);
        }
        protected void gvProyEstrategico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvProyEstrategico, "Select$" + e.Row.RowIndex.ToString()));
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.FindControl("btnAdd").Visible = (hdd_Proyecto_ProyEstrategico_Id.Value == "");
            }
        }
        protected void gvProyEstrategico_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["IndexProyEstrategico"] = ((gvProyEstrategico.PageIndex * gvProyEstrategico.PageSize) + gvProyEstrategico.SelectedIndex).ToString();
            if (gvProyEstrategico.Rows.Count > 0)
            {
                gv_SelectedIndexChanged(gvProyEstrategico);
                oVar.prBancoAu = gvProyEstrategico.SelectedDataKey["idbanco"].ToString();
            }
            oBasic.FixPanel(divData, "ProyEstrategico", 0);
        }
        protected void gvProyEstrategico_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvProyEstrategico, e.SortExpression.ToString(), ViewState["SortDirProyEstrategico"].ToString(), oVar.prDSBanco);
            oVar.prDSBanco = oVar.prDataSet;
        }
        protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header)
                return;

            GridView gv = sender as GridView;
            string modulo = gv.ID.Substring(2);
            string sortExpression = ViewState["SortExp" + modulo].ToString();
            string sortDirection = ViewState["SortDir" + modulo].ToString();
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
        protected void MessageBox_Accept(string key)
        {
            try
            {
                switch (key)
                {
                    case "Delete":
                        Delete_ProyEstrategico();
                        oBasic.AlertMain(msgMain, clConstantes.MSG_ERR_PERMISO, "danger");
                        oBasic.AlertSection(msgProyEstrategico, "", "0");
                        Load_ProyEstrategico(ViewState["CriterioBuscar"].ToString());
                        oUtil.fSetIndex(gvProyEstrategico);
                        gv_SelectedIndexChanged(gvProyEstrategico);
                        PB_Header.Enabled = false;
                        oBasic.FixPanel(divData, "ProyEstrategico", 0);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }
        protected void ucActivity_OnSave(object sender)
        {
            PB_Header.LoadChart();
            ucActivityManagement.LoadControl();
            ucTracing.IdBanco = Int32.Parse(oVar.prBancoAu.ToString());
        }
        protected void ucActivity_ToList(object sender)
        {
            ViewList();
        }
        protected void ucTracing_SaveActivity(object sender)
        {
            ucActivity.IdBanco = Int32.Parse(oVar.prBancoAu.ToString());
        }
        protected void ucTracing_ToList(object sender)
        {
            ViewList();
        }
        protected void ucTracing_UserControlException(object sender, Exception ex)
        {
            MessageInfo.ShowMessage(ex.Message);
        }
        protected void uc_ViewDoc(object sender)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
        }
        #endregion



        #region Métodos Privados
        private void ColumnsMergeLeft(ExcelWorksheet ws, string Cells)
        {
            if (!ws.Cells[Cells].Merge)
                ws.Cells[Cells].Merge = true;
            ws.Cells[Cells].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        }
        private void Delete_ProyEstrategico()
        {
            string strResult = PB_Header.Delete();
            oBasic.AlertUserControl(msgFichaProyecto, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d");
        }
        private void gv_SelectedIndexChanged(GridView gv)
        {
            string modulo = gv.ID.Substring(2);
            ViewState["Index" + modulo] = ((gv.PageIndex * gv.PageSize) + gv.SelectedIndex).ToString();
            try
            {
                UpdatePanel up = (UpdatePanel)divData.FindControl("up" + modulo + "Foot");
                oBasic.LblRegistros(up, ((DataSet)oVar.prDSBanco).Tables[0].Rows.Count, Convert.ToInt32(ViewState["Index" + modulo]));
            }
            catch { }
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
        private void Load_ProyEstrategico(string Parametro)
        {
            if (string.IsNullOrEmpty(Parametro))
                Parametro = "%";
            if (!ValidateAccess(cnsSection.FICHA_BANCO, cnsAction.CONSULTAR, false, false)) return;

            string usuario = (string)((oPermisos.TienePermisosAccion(cnsSection.FICHA_BANCO, cnsAction.CONSULTAR, "", oVar.prUserCod.ToString())) ? "" : oVar.prUserCod.ToString());

            string isPA = "PE";

            oVar.prDSBanco = oProyEstrategico.sp_s_banco_listar(Parametro, usuario, isPA);
            gvProyEstrategico.DataSource = ((DataSet)(oVar.prDSBanco));
            gvProyEstrategico.DataBind();

            if (gvProyEstrategico.Rows.Count > 0)
            {
                gvProyEstrategico.Visible = true;
                gvProyEstrategico.SelectedIndex = Convert.ToInt16(ViewState["IndexProyEstrategico"].ToString());
                if (gvProyEstrategico.Rows.Count <= gvProyEstrategico.SelectedIndex)
                    gvProyEstrategico.SelectedIndex = 0;
                oBasic.FixPanel(divData, "ProyEstrategico", 0);

                gv_Sorting(gvProyEstrategico, ViewState["SortExpProyEstrategico"].ToString(), oUtil.OpDirection(ViewState["SortDirProyEstrategico"].ToString()), oVar.prDSBanco);
                oVar.prBancoAu = gvProyEstrategico.SelectedDataKey.Value.ToString();
            }
            else
            {
                gvProyEstrategico.Visible = false;
                gv_SelectedIndexChanged(gvProyEstrategico);
                oBasic.FixPanel(divData, "ProyEstrategico", 3);
            }
        }
        private void LoadDetail()
        {
            int Indice = Convert.ToInt16(ViewState["IndexProyEstrategico"]);
            PB_Header.IdBanco = Int32.Parse(oVar.prBancoAu.ToString());
            PB_Header.CodUsuResponsable = Convert.ToInt32((Session["ProyEstrategico.cod_usu_responsable"] ?? oVar.prUserCod.ToString()).ToString());
            PB_Header.LoadDetail();
            lblNombreProyecto_Sec2.Text = lblNombreProyecto_Sec3.Text = lblNombreProyecto_Sec4.Text = PB_Header.NombreProyecto;
            ucActivity.IdBanco = Int32.Parse(oVar.prBancoAu.ToString());
            ucActivity.CodUsuario = (Session["ProyEstrategico.cod_usu_responsable"] ?? oVar.prUserCod.ToString()).ToString();
            ucActivity.LoadControl();
            ucActivity.TipoProyecto = "PE";
            ucActivityManagement.IdBanco = Int32.Parse(oVar.prBancoAu.ToString());
            ucActivityManagement.CodUsuario = (Session["ProyEstrategico.cod_usu_responsable"] ?? oVar.prUserCod.ToString()).ToString();
            ucActivityManagement.LoadControl();
            ucTracing.IdBanco = Int32.Parse(oVar.prBancoAu.ToString());
            ucTracing.CodUsuario = (Session["ProyEstrategico.cod_usu_responsable"] ?? oVar.prUserCod.ToString()).ToString();
            ucTracing.LoadControl();
            oBasic.LblRegistros(upProyEstrategicoFoot, ((DataSet)oVar.prDSBanco).Tables[0].Rows.Count, Indice);
            oBasic.FixPanel(divData, "FichaProyecto", 0, pList: (hdd_Proyecto_ProyEstrategico_Id.Value == ""), pAdd: (hdd_Proyecto_ProyEstrategico_Id.Value == ""));

        }
        private void LoadReport(string origen)
        {
            string FORMAT_PM02_F0667 = "PM02-FO667.xlsx";
            string _plantilla_bancos = oVar.prPathFormatosOrigen.ToString() + FORMAT_PM02_F0667;

            if (File.Exists(_plantilla_bancos))
            {
                //Archivo Excel del cual crear la copia:
                FileInfo templateFile = new FileInfo(_plantilla_bancos);
                string file_name = "Ficha_Seguimiento.xlsx";

                using (ExcelPackage pck = new ExcelPackage(templateFile, true))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet wsBase = pck.Workbook.Worksheets["Ficha"];
                    if (origen == "Grid")
                    {
                        foreach (GridViewRow proyecto in gvProyEstrategico.Rows)
                        {
                            if (Int32.TryParse(gvProyEstrategico.DataKeys[proyecto.RowIndex]["idbanco"].ToString(), out int IdBanco))
                            {
                                ExcelReport.LoadReportSheet(pck, wsBase, IdBanco);
                            }
                        }
                    }
                    else if (origen == "Detail")
                    {
                        if (PB_Header.IdBanco != 0)
                        {
                            ExcelReport.LoadReportSheet(pck, wsBase, PB_Header.IdBanco);
                        }
                    }
                    wsBase.Hidden = eWorkSheetHidden.VeryHidden;
                    oUtil.fExcelSave(pck, file_name, false);
                }
            }
        }
        private void Save_ProyEstrategico(string accion)
        {
            string strResult = PB_Header.Save();

            if (oBasic.AlertUserControl(msgFichaProyecto, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, accion))
            {
                oVar.prBancoAu = PB_Header.IdBanco == 0 ?  strResult.Substring(6): PB_Header.IdBanco.ToString();
                PB_Header.IdBanco = Int32.Parse(PB_Header.IdBanco == 0 ? strResult.Substring(6) : PB_Header.IdBanco.ToString());
            }
        }
        private bool ValidateAccess(string section, string action, bool validateResponsible = false, bool requeridedResponsible = true)
        {
            string cod_usu_responsable = (Session["ProyEstrategico.cod_usu_responsable"] ?? 0).ToString();
            string message = oPermisos.ValidateAccess(section, action, cod_usu_responsable, validateResponsible, requeridedResponsible);

            if (message == "")
                return true;

            MessageInfo.ShowMessage(message);
            return false;
        }
        private void VerifyImageFiles()
        {
            for (int i = 1; i <= 13; i++)
            {
                string funombre = "fu_imagen" + (i < 10 ? "0" : "") + i.ToString();
                FileUpload file = (FileUpload)divProyEstrategico.FindControl(funombre);
                if (file != null && file.HasFile)
                {
                    string nombreArchivo = file.FileName;
                    string extension = nombreArchivo.Substring(nombreArchivo.LastIndexOf("."));
                    string nombreImagen = funombre + Guid.NewGuid().ToString() + extension;

                    Session["ProyEstrategico." + funombre] = nombreImagen;
                }
            }
        }
        private void ViewAdd()
        {
            if (!ValidateAccess(cnsSection.FICHA_BANCO, cnsAction.INSERTAR, true, false)) return;

            oVar.prBancoAu = 0;
            ViewState["AccionFinal"] = "_Add";
            Session["ProyEstrategico.cod_usu_responsable"] = oVar.prUserCod.ToString();
            LoadDetail();

            oBasic.FixPanel(divData, "ProyEstrategico", 1);

            PB_Header.IdBanco = 0;
            PB_Header.CodUsuResponsable = Convert.ToInt32(oVar.prUserCod.ToString());
            PB_Header.Enabled = true;
            PB_Header.LoadDetail();
            hddsectionview.Value = "collapse1";
            oBasic.AlertSection(msgFichaProyecto, clConstantes.MSG_I, "info");
            oBasic.FixPanel(divData, "FichaProyecto", 2);
        }
        private void ViewDetail()
        {
            if (!ValidateAccess(cnsSection.FICHA_BANCO, cnsAction.CONSULTAR, false, false)) return;

            LoadDetail();
            oBasic.FixPanel(divData, "ProyEstrategico", 1);
            PB_Header.Enabled = false;
            PB_Header.LoadDetail();

            oBasic.FixPanel(divData, "FichaProyecto", 0, pList: (hdd_Proyecto_ProyEstrategico_Id.Value == ""), pAdd: (hdd_Proyecto_ProyEstrategico_Id.Value == ""));
        }
        private void ViewDelete()
        {
            if (!ValidateAccess(cnsSection.FICHA_BANCO, cnsAction.ELIMINAR, true)) return;

            ViewDetail();
            MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar este proyecto?", type: "danger");
            oBasic.AlertSection(msgFichaProyecto, clConstantes.MSG_D, "danger");
            oBasic.FixPanel(divData, "FichaProyecto", 0, pList: (hdd_Proyecto_ProyEstrategico_Id.Value == ""), pAdd: (hdd_Proyecto_ProyEstrategico_Id.Value == ""));
        }
        private void ViewEdit()
        {
            if (!ValidateAccess(cnsSection.FICHA_BANCO, cnsAction.EDITAR, true, false)) return;

            LoadDetail();

            oBasic.FixPanel(divData, "ProyEstrategico", 2);
            PB_Header.Enabled = true;
            PB_Header.LoadDetail();
            oBasic.AlertSection(msgFichaProyecto, clConstantes.MSG_U, "warning");
            oBasic.FixPanel(divData, "FichaProyecto", 2);
        }
        private void ViewList()
        {
            int index;
            int iPagina = Convert.ToInt16(ViewState["IndexProyEstrategico"]) / gvProyEstrategico.PageSize;
            if (iPagina > 0)
                index = Convert.ToInt16(ViewState["IndexProyEstrategico"]) % gvProyEstrategico.PageSize;
            else
                index = Convert.ToInt16(ViewState["IndexProyEstrategico"]);
            gvProyEstrategico.PageIndex = iPagina;
            gvProyEstrategico.SelectedIndex = index;
            oBasic.FixPanel(divData, "ProyEstrategico", 0);
        }
        #endregion
    }
}