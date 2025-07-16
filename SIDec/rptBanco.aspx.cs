using GLOBAL.DAL;
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

using tipoArchivo = GLOBAL.CONST.clConstantes.TipoArchivo;

namespace SIDec
{
    public partial class rptBanco : Page
    {
        readonly clGlobalVar oVar = new clGlobalVar();
        readonly clUtil oUtil = new clUtil();
        readonly BANCOPROYECTOS_DAL oBanco = new BANCOPROYECTOS_DAL();

        private const int ROW_HEADER = 3;
        private const int ROW_TRACING = 30;
        private const int ROW_ACTIVITY = 24;
        private const int ROW_MANAGEMENT = 17;

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExportar);

            if (!IsPostBack)
            {
                LoadGrid();
            }
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        #endregion

        #region Métodos
        private void ColumnsMergeLeft(ExcelWorksheet ws, string Cells)
        {
            if (!ws.Cells[Cells].Merge)
                ws.Cells[Cells].Merge = true;
            ws.Cells[Cells].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        }
        private void LoadGrid()
        {
            DataSet ds = oBanco.sp_s_banco_reporte();
            Session["rptBanco.Data"] = ds;
            gvBanco.DataSource = ds;
            gvBanco.DataBind();
        }
        private void LoadReport()
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
                    pck.Workbook.CalcMode = ExcelCalcMode.Automatic;

                    ExcelWorksheet wsBase = pck.Workbook.Worksheets["Ficha"];
                    bool flag = false;

                    foreach (ListItem li in chk_sections.Items)
                    {
                        if (li.Selected == true)
                        {
                            flag = true;
                            if (li.Value == "1")
                            {
                                ExcelWorksheet ws = pck.Workbook.Worksheets["Proyectos Incorporados"];
                                ws.Hidden = eWorkSheetHidden.Visible;
                                DataSet dsList = (DataSet)Session["rptBanco.Data"];
                                ExcelReport.LoadReportProyectList(ws, dsList);
                            }

                            if (li.Value == "2")
                            {
                                foreach (GridViewRow proyecto in gvBanco.Rows)
                                {
                                    if (gvBanco.DataKeys[proyecto.RowIndex]["activo"].ToString() == "1")
                                    {
                                        if (Int32.TryParse(gvBanco.DataKeys[proyecto.RowIndex]["idbanco"].ToString(), out int IdBanco))
                                        {
                                            ExcelReport.LoadReportSheet(pck, wsBase, IdBanco);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!flag)
                    {
                        MessageInfo.ShowMessage("Seleccione al menos una opción: </br><b>Lista</b> para ver la pestaña de «PROYECTOS INCORPORADOS» ó </br><b>Ficha</b> para ver el seguimiento de cada proyecto");
                        Session["ReloadXFU"] = "1";
                        (this.Master as AuthenticNew).fReload();
                        return;
                    }
                    wsBase.Hidden = eWorkSheetHidden.VeryHidden;
                    pck.Workbook.Calculate();
                    oUtil.fExcelSave(pck, file_name, false); 
                    Session["ReloadXFU"] = "1";
                    (this.Master as AuthenticNew).fReload();

                }
            }
        }
        

        #endregion
    }
}