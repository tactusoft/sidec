using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

using OfficeOpenXml;
using System.IO;

using GLOBAL.DAL;
using GLOBAL.UTIL;

namespace SIDec
{
    public partial class rptActosAdmin : System.Web.UI.Page
    {
        IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
        ACTOSADMIN_DAL oActosAdmin = new ACTOSADMIN_DAL();

        clUtil oUtil = new clUtil();
        DataSet oDSReporte = new DataSet();

        #region---EVENTOS

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fLoadDropDowns();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            fLoadGVReporte();
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            oDSReporte = oActosAdmin.sp_rpt_actos_administrativos(txtFecIni.Text, txtFecFin.Text, ddlbTipoActo.SelectedValue, ddlbCausalActo.SelectedValue, txtPredioDec.Text);

            if (oDSReporte.Tables[0].Rows.Count > 0)
                fCrearExcel(oDSReporte.Tables[0]);
        }

        protected void gvReporte_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReporte.PageIndex = e.NewPageIndex;
            fLoadGVReporte();
        }

        #endregion

        #region---METODOS

        private void fCrearExcel(DataTable tbl)
        {
            string PredioDec = string.IsNullOrEmpty(txtPredioDec.Text) ? "Todos los Predios" : txtPredioDec.Text;
            string NumeroRangoRegistros = (oDSReporte.Tables[0].Rows.Count + 4).ToString(); //Sumar los 4 correspondientes a la informacion del Reporte

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Crear la Hoja y asignarle un nombre

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Reporte");

                //FORMATO DE LA HOJA
                //Fondo Blanco a toda la hoja = Sin bordes
                ws.Cells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);
                ws.Cells.Style.Font.Color.SetColor(Color.Black);
                //Color del t-b y Alto de filas
                ws.TabColor = Color.Blue;
                ws.DefaultRowHeight = 12;
                //ws.Row(1).Height = 30;
                //ws.Row(2).Height = 22;


                //Cargar Datatable en la celdsa indicada        
                ws.Cells["A4"].LoadFromDataTable(tbl, true);

                //Eliminar las columnas innecesarias de Dataset
                //ws.DeleteColumn(1);

                //Agregar Informacion del Reporte
                ws.Cells["A1"].Value = "Fechas de Declaratoria";
                ws.Cells["B1"].Value = "Tipo Acto";
                ws.Cells["C1"].Value = "Causal Acto";
                ws.Cells["D1"].Value = "Predio Declarado";
                ws.Cells["E1"].Value = "Total Registros";
                ws.Cells["A2"].Value = txtFecIni.Text + " - " + txtFecFin.Text;
                ws.Cells["B2"].Value = ddlbTipoActo.SelectedItem;
                ws.Cells["C2"].Value = ddlbCausalActo.SelectedItem;
                ws.Cells["D2"].Value = PredioDec;
                ws.Cells["E2"].Value = tbl.Rows.Count;



                //Formato a los Criterios del reporte
                using (ExcelRange rng = ws.Cells["A1:E2"])
                {
                    rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    rng.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    rng.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    rng.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    rng.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }
                using (ExcelRange rng = ws.Cells["A1:E1"])
                {
                    rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 204, 255));
                    rng.Style.Font.Color.SetColor(Color.Black);
                }
                //*****************************************************************************************************************************



                //Formato a los datos del reporte
                //   Titulos de campos
                using (ExcelRange rng = ws.Cells["A4:I4"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                    rng.Style.Font.Color.SetColor(Color.White);                 // Color de Texto

                    rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    rng.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    rng.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    rng.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    rng.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }
                //   Items
                using (ExcelRange rng = ws.Cells["A5:I" + NumeroRangoRegistros])
                {
                    rng.Style.Font.Bold = false;
                    rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                    rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    rng.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    rng.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    rng.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    rng.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }
                //*****************************************************************************************************************************



                //Formatear Celdas o Columnas de manera particular
                //   Asignar filtro a un campo
                ws.Cells["B4"].AutoFilter = true;

                //   Formato Fecha 
                using (ExcelRange col = ws.Cells["E5:E" + (NumeroRangoRegistros) + "," + "H5:H" + (NumeroRangoRegistros)])
                {
                    col.Style.Numberformat.Format = "dd-mm-yyyy";
                    col.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }
                //Anchos de columnas 
                ws.Column(1).Width = 22;
                ws.Column(2).Width = 24;
                ws.Column(3).Width = 26;
                ws.Column(4).Width = 21;
                ws.Column(5).Width = 16;
                ws.Column(6).Width = 16;
                ws.Column(7).Width = 16;
                ws.Column(8).Width = 16;
                ws.Column(9).Width = 35;
                //*****************************************************************************************************************************



                //Crear Header y Footer a la Hoja
                //headerRow.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.HeaderFooter.OddHeader.CenteredText = "&24&U&\"Arial,Regular Bold\" Reporte Actos Administrativos";
                ws.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName + ExcelHeaderFooter.NumberOfPages;
                ws.HeaderFooter.OddFooter.RightAlignedText = string.Format("Página {0} de {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                ws.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                //*****************************************************************************************************************************



                //Asignar Información del archivo creado
                pck.Workbook.Properties.Company = "Secretaría Distrital del Hábitat";
                pck.Workbook.Properties.Author = "Daniel Barrera";
                pck.Workbook.Properties.Comments = "Reporte Actos Administrativos";



                //Permitir la opcion de Guardar en el cliente el ExcelPackage creado
                Response.Clear();
                Response.ClearHeaders();
                Response.AddHeader("content-disposition", "attachment; filename=rptActosAdmin.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(pck.GetAsByteArray());

                Response.End();
            }
        }

        private void fLoadDropDowns()
        {
            ddlbCausalActo.DataSource = oIdentidades.sp_s_identidad_id_categoria("13");
            ddlbCausalActo.DataTextField = "nombre_identidad";
            ddlbCausalActo.DataValueField = "id_identidad";
            ddlbCausalActo.DataBind();

            ddlbTipoActo.DataSource = oIdentidades.sp_s_identidad_id_categoria("6");
            ddlbTipoActo.DataTextField = "nombre_identidad";
            ddlbTipoActo.DataValueField = "id_identidad";
            ddlbTipoActo.DataBind();
        }

        private void fLoadGVReporte()
        {
            oDSReporte = oActosAdmin.sp_rpt_actos_administrativos(txtFecIni.Text, txtFecFin.Text, ddlbTipoActo.SelectedValue, ddlbCausalActo.SelectedValue, txtPredioDec.Text);

            gvReporte.DataSource = oDSReporte;
            gvReporte.DataBind();

            total.Text = "Total Registros " + oDSReporte.Tables[0].Rows.Count.ToString();
        }

        #endregion
    }
}