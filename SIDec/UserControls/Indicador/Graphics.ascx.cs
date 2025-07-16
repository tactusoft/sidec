using GLOBAL.DAL;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using SigesTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace SIDec.UserControls.Indicador
{
    public partial class Graphics : System.Web.UI.UserControl
    {
        private const string _SOURCEPAGE = "UserControlGraphics";
        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clUtil oUtil = new clUtil();
        static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        #region Propiedades
        public string Code
        {
            get { return (hddId.Value); }
            set { hddId.Value = value; }
        }
        public DateTime DateFilter
        {
            get {
                return (DateTime)(Session[_SOURCEPAGE + ".DateFilter_" + Code] ?? DateTime.MinValue.ToShortDateString());
            }
            set { Session[_SOURCEPAGE + ".DateFilter_" + Code] = value; }
        }
        public string Detail
        {
            get { return (hddDetail.Value); }
            set
            {
                hddDetail.Value = value;
            }
        }
        public string FontSize
        {
            get { return (hddFont.Value); }
            set { hddFont.Value = value; }
        }
        public string Height
        {
            get{ return cnv_ind.Attributes["height"]??"300"; }
            set
            {
                cnv_ind.Attributes.Remove("height");
                cnv_ind.Attributes.Add("height", value);

                btnMaximize.Visible = (value == "300");
                btnMinimize.Visible = (value != "300");
            }
        }
        public char IndexAxis
        {
            get { return (char)(Session[_SOURCEPAGE + ".indexAxis_" + Code] ?? 'x'); }
            set { Session[_SOURCEPAGE + ".indexAxis_" + Code] = value; }
        }
        public string Type
        {
            get
            {
                return (Session[_SOURCEPAGE + ".Type_" + Code] ?? "bar").ToString();
            }
            set { Session[_SOURCEPAGE + ".Type_" + Code] = value; }
        }
        #endregion


        #region Eventos
        protected void Page_Load(object sender, EventArgs e){
        }
        protected void btnDetail_Click(object sender, EventArgs e)
        {
            SetSessions();
            Session["Indicador.Detail"] = Convert.ToInt32(Detail) + 1;
            Response.Redirect("dashboard");
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            LoadExcel();
        }
        protected void btnMinimize_Click(object sender, EventArgs e)
        {
            Session["Indicador.mes_visualización"] = DateFilter.ToString("yyyy-MM");
            Session["Indicador.Detail"] = 0;
            Session["Indicador.Explore"] = "0";
            Response.Redirect("dashboard");
        }
        protected void btnMaximize_Click(object sender, EventArgs e)
        {
            SetSessions();
            Response.Redirect("dashboard");
        }
        #endregion


        #region métodos
        public static Color ConvertRgbaToColor(string rgba)
        {
            try
            {
                string[] parts = rgba.Split(',');

                // Verifica que tenemos 4 partes (r, g, b)
                if (parts.Length >= 3)
                {
                    // Convierte las partes en enteros
                    int r = int.Parse(parts[0].Trim());
                    int g = int.Parse(parts[1].Trim());
                    int b = int.Parse(parts[2].Trim());

                    // Retorna el color convertido
                    return System.Drawing.Color.FromArgb(r, g, b);
                }
            }
            catch (Exception) { }
            return System.Drawing.Color.FromArgb(255, 255, 255);
        }

        private void ExportRepotLevel_01(ExcelWorksheet ws, string detail)
        {
            int currentRow = 4;

            if (Code == "0") return;

            string p_anio = DateFilter.Year.ToString();
            string p_mes = DateFilter.Month.ToString();
            string[] levels;

            switch (Code)
            {
                case "1":
                    levels = new string[] { "alto", "medio", "bajo" };
                    break;
                case "2":
                    levels = new string[] { "Disponible", "VIP", "VIS", "No VIS" };
                    break;
                default:
                    levels = new string[] { "" };
                    break;
            }

            using (Indicador_DAL dal = new Indicador_DAL())
            {
                List<IndicadorTO> indicadores = dal.GetIndicadores(Code, p_anio, p_mes, detail);
                if (indicadores != null && indicadores.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("nivel");
                    foreach(string level in levels)
                    {
                        dt.Columns.Add(level);
                    }
                    // Agrupar por 'Codigo'
                    var groupedResult = indicadores.GroupBy(x => x.Codigo);

                    foreach (var group in groupedResult)
                    {
                        // Crear una nueva fila
                        DataRow row = dt.NewRow();
                        row["nivel"] = group.Key;
                        bool flag = false;

                        // Asignar valores a 'alto', 'medio' y 'bajo'
                        foreach (var indicador in group)
                        {
                            foreach (string level in levels)
                            {
                                if (indicador.Descripcion1 == level)
                                {
                                    row[level] = indicador.Valor1;
                                    if (indicador.Valor1 != 0) flag = true;
                                }
                            }
                        }

                        if (Code == "2")
                            row["Disponible"] = Convert.ToDecimal("0"+(row["Disponible"]??0)) - Convert.ToDecimal("0" + (row["VIP"]??0))- Convert.ToDecimal("0" + (row["VIS"]??0))- Convert.ToDecimal("0" + (row["No VIS"]??0));
                        // Agregar la fila al DataTable
                        if (flag)
                            dt.Rows.Add(row);
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        ws.Cells[currentRow.ToString() + ":" + currentRow.ToString()].Copy(ws.Cells[(currentRow + 1).ToString() + ":" + (currentRow + 1).ToString()]);
                        ws.InsertRow(currentRow + 1, 1);
                        int column = 0;
                        oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["nivel"].ToString());
                        foreach (string level in levels)
                        {
                            oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr[level].ToString());
                        }

                        currentRow++;
                    }
                    switch (Code)
                    {
                        case "1":
                            ExportChartsLevel01(ws, currentRow);
                            break;
                        case "2":
                            ExportChartsLevel02(ws, currentRow);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ExportChartsLevel01(ExcelWorksheet ws, int maxRow)
        {
            var chart = ws.Drawings.AddBarChart("Grafica1", eBarChartType.BarStacked);
            var serie1 = chart.Series.Add(ws.Cells[4, 2, maxRow, 2], ws.Cells[4, 1, maxRow, 1]);
            var serie2 = chart.Series.Add(ws.Cells[4, 3, maxRow, 3], ws.Cells[4, 1, maxRow, 1]);
            var serie3 = chart.Series.Add(ws.Cells[4, 4, maxRow, 4], ws.Cells[4, 1, maxRow, 1]);
            serie1.Header = "Alto";
            serie2.Header = "Medio";
            serie3.Header = "Bajo";
            serie1.Fill.Color = Color.Red;
            serie2.Fill.Color = Color.Yellow;
            serie3.Fill.Color = Color.LightGreen;
            chart.VaryColors = false;
            chart.Axis[0].Orientation = eAxisOrientation.MaxMin;

            chart.SetPosition(0, 0, 0, 0);
            chart.SetSize(1200, 480);
            chart.Title.Text = "Ruta Crítica 2022-2026";
        }

        private void ExportChartsLevel02(ExcelWorksheet ws, int maxRow)
        {
            // Crear el gráfico de columnas apiladas
            var chart = ws.Drawings.AddBarChart("Grafica1", eBarChartType.ColumnStacked);

            // Primera columna (una sola serie "Disponible")
            var serie1 = chart.Series.Add(ws.Cells[4, 2, maxRow, 2], ws.Cells[4, 1, maxRow, 1]);
            serie1.Header = "Disponible";
            serie1.Fill.Color = Color.Blue;

            // Segunda columna (apiladas: "VIP", "VIS", "No VIS")
            var serie2 = chart.Series.Add(ws.Cells[4, 3, maxRow, 3], ws.Cells[4, 1, maxRow, 1]);
            var serie3 = chart.Series.Add(ws.Cells[4, 4, maxRow, 4], ws.Cells[4, 1, maxRow, 1]);
            var serie4 = chart.Series.Add(ws.Cells[4, 5, maxRow, 5], ws.Cells[4, 1, maxRow, 1]);

            serie2.Header = "VIP";
            serie3.Header = "VIS";
            serie4.Header = "No VIS";

            serie2.Fill.Color = Color.Red;
            serie3.Fill.Color = Color.Yellow;
            serie4.Fill.Color = Color.Green;

            // Opciones adicionales
            chart.VaryColors = false;
            chart.Axis[0].Orientation = eAxisOrientation.MinMax;

            // Posicionar y dimensionar el gráfico
            chart.SetPosition(0, 0, 0, 0);
            chart.SetSize(1200, 480);
            chart.Title.Text = "Distribución de Viviendas";
        }




        private void Initialize()
        {
            List<IndicadorReferenciaTO> references = (List<IndicadorReferenciaTO>)Session["Indicador.Referencias"];
            if (references != null)
            {
                var reference = references.Where(rg => rg.IdReferencia.ToString() == Code).FirstOrDefault();
                if (reference != null)
                {
                    lbl_ind.Text = reference.Nombre;
                    lbl_ind.ToolTip = reference.Nombre;
                    btnDetail.Visible = reference.NivelDetalle > (Convert.ToInt32(Detail) + (reference.ConDetalle ? 0 : 1));
                    hhdNext.Value = (reference.NivelDetalle == (Convert.ToInt32(Detail) + 1) && reference.ConDetalle ? "2" : "1");
                }
            }
        }
        public string GetChartDataReport()
        {
            if (Code == "0") return "";

            Initialize();
            string p_anio = DateFilter.Year.ToString();
            string p_mes = DateFilter.Month.ToString();

            using (Indicador_DAL dal = new Indicador_DAL())
            {
                List<IndicadorTO> indicadores = dal.GetIndicadores(Code, p_anio, p_mes, Detail);

                var labels = indicadores.Where(rg => rg.Valor1 > 0 || rg.Valor2 > 0 || rg.Valor3 > 0 || rg.Valor4 > 0).Select(rg => rg.Codigo).Distinct().ToArray();

                ChartDataset[] datasets = GetChartDatasets(indicadores);

                var y = new ChartAxes { beginAtZero = true, stacked = true, ticks = new ChartTicks { font = new ChartFont { size = FontSize } } };
                var x = new ChartAxes { beginAtZero = true, stacked = true, ticks = new ChartTicks { font = new ChartFont { size = FontSize } } };
                var scales = new ChartScales { y = y, x = x };

                var data = new ChartData { labels = labels, datasets = datasets };
                var options = new ChartOptions { indexAxis = IndexAxis, scales = scales };
                if (Type == "doughnut")
                    options = new ChartOptions();
                var chart = new Chart { type = Type, data = data, options = options };

                string section = "" +
                "   <script>" +
                "       Sys.Application.add_load(function() {" +
                "           const ctx = document.getElementById('" + cnv_ind.ClientID + "')" + (IndexAxis == 'y'? ".getContext('2d');" : ";") +
                "           new Chart(ctx, " +
                    JsonConvert.SerializeObject(chart, jsonSettings) +
                "               );" +
                "       });" +
                "   </script>";

                return section;
            }
        }
        protected ChartDataset[] GetChartDatasets(List<IndicadorTO> indicadores)
        {
            ChartDataset[] datasets;
            switch (Code)
            {
                case "1":
                    var labels1 = indicadores.Where(rg => rg.Valor1 > 0).Select(rg => rg.Codigo).ToList().Distinct().ToArray();
                    var labelds1 = indicadores.Where(rg => rg.Valor1 > 0).OrderBy(rg => rg.Orden2).Select(rg => rg.Descripcion1).Distinct().ToList();

                    datasets = new ChartDataset[labelds1.Count];
                    int i1 = 0;

                    foreach (string label in labelds1)
                    {
                        var dsdata = indicadores.Where(xx => xx.Descripcion1 == label && labels1.Contains(xx.Codigo)).Select(rg => rg.Valor1).ToArray<decimal>();
                        var colors1 = indicadores.Where(xx => xx.Descripcion1 == label && labels1.Contains(xx.Codigo)).Select(rg => rg.Color).ToArray();

                        var dataset = new ChartDataset
                        {
                            label = label,
                            data = dsdata,
                            backgroundColor = colors1
                        };
                        datasets[i1] = dataset;
                        i1++;
                    }
                    break;
                case "2":
                    var labels2 = indicadores.Where(rg => rg.Valor1 > 0).Select(rg => rg.Codigo).ToList().Distinct().ToArray();
                    var labelds2 = indicadores.Where(rg => rg.Valor1 > 0).OrderBy(rg => rg.Orden2).Select(rg => rg.Descripcion1).Distinct().ToList();

                    datasets = new ChartDataset[labelds2.Count];
                    int i2 = 0;

                    foreach (string label in labelds2)
                    {
                        var dsdata = indicadores.Where(xx => xx.Descripcion1 == label && labels2.Contains(xx.Codigo)).Select(rg => rg.Valor1).ToArray<decimal>();
                        var colors1 = indicadores.Where(xx => xx.Descripcion1 == label && labels2.Contains(xx.Codigo)).Select(rg => rg.Color).ToArray();

                        var dataset = new ChartDataset
                        {
                            label = label,
                            data = dsdata,
                            backgroundColor = colors1,
                            stack = label == "Disponible" ? null : "stackedGroup"
                        };
                        datasets[i2] = dataset;
                        i2++;
                    }
                    break;
                case "3":
                    var labels3 = indicadores.Where(rg => rg.Valor1 > 0).Select(rg => rg.Codigo).ToList().Distinct().ToArray();
                    var labelds3 = indicadores.Where(rg => rg.Valor1 > 0).OrderBy(rg => rg.Orden % 100).Select(rg => rg.Descripcion1).Distinct().ToList();

                    datasets = new[] {
                        new ChartDataset{
                                data = indicadores.Select(rg => rg.Valor1).ToArray<decimal>() ,
                                backgroundColor = indicadores.Select(rg => rg.Color).ToArray()
                        } };
                    break;
                default:
                    datasets = null;
                    break;
            }
            return datasets;
        }
        public void LoadExcel()
        {
            string codeFile = "00" + Code;
            codeFile = codeFile.Substring(codeFile.Length - 2, 2);
            string FORMAT_TRAMITE = "Inidcador" + codeFile + ".xlsx";
            string _plantilla_bancos = oVar.prPathFormatosOrigen.ToString() + "Indicadores\\" + FORMAT_TRAMITE;

            if (File.Exists(_plantilla_bancos))
            {
                //Archivo Excel del cual crear la copia:
                FileInfo templateFile = new FileInfo(_plantilla_bancos);
                string file_name = "", Level_1 = "", Level_2 = "", Level_3 = "", Level_r = "";

                using (ExcelPackage pck = new ExcelPackage(templateFile, true))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    switch (Code)
                    {
                        case "1":
                            file_name = "Ruta_Critica_" + DateFilter.ToString("yyyyMM") + ".xlsx";
                            Level_1 = "Estados";
                            Level_2 = "Proyectos";
                            Level_3 = "Entidades";
                            Level_r = "Resumen";
                            break;
                        case "2":
                            file_name = "Tipos_Vivienda_" + DateFilter.ToString("yyyyMM") + ".xlsx";
                            Level_1 = "Consolidado";
                            Level_2 = "Proyectos";
                            Level_r = "Resumen";
                            break;
                    }
                    ExcelWorksheet wss = pck.Workbook.Worksheets[Level_1];
                    ExportRepotLevel_01(wss, "0");
                    if (Level_2 != "")
                    {
                        ExcelWorksheet wsp = pck.Workbook.Worksheets[Level_2];
                        ExportRepotLevel_01(wsp, "1");
                    }
                    if (Level_3 != "")
                    {
                        ExcelWorksheet wse = pck.Workbook.Worksheets[Level_3];
                        ExportRepotLevel_01(wse, "2");
                    }
                    if (Level_r != "")
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets[Level_r];
                        LoadRepotDetail01(ws);
                    }
                    pck.Workbook.Calculate();
                    oUtil.fExcelSave(pck, file_name, false);
                }
            }
        }
        private void LoadRepotDetail01(ExcelWorksheet ws)
        {
            int currentRow = 2;
            string p_anio = DateFilter.Year.ToString();
            string p_mes = DateFilter.Month.ToString();

            using (Indicador_DAL dal = new Indicador_DAL())
            {
                DataSet ds = dal.GetIndicadorDetalle(Code, p_anio, p_mes);

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ws.Cells[currentRow.ToString() + ":" + currentRow.ToString()].Copy(ws.Cells[(currentRow + 1).ToString() + ":" + (currentRow + 1).ToString()]);
                        ws.InsertRow(currentRow + 1, 1);
                        int column = 0;
                        bool isDate = false;
                        int cols = dr.Table.Columns.Count - (Code == "1" ? 4 : 2); 
                        for (int i = 0; i < cols; i++)
                        {
                            string cellValue = dr[i].ToString();
                            isDate = (i >= 6 && i <= 8 && Code == "1"); 
                            if (i >= 6 && i <= 8 && Code == "2") cellValue = cellValue.Replace(",", Environment.NewLine).Replace(" (", "m² (").Replace(")", " Uni)"); // Reemplazar comas por saltos de línea
                            oUtil.fExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), cellValue, isDate);
                        }

                        if (Code == "1")
                        {
                            column = 5;
                            var color = Graphics.ConvertRgbaToColor(dr["Color"].ToString());
                            ws.Cells[ExcelAddress.GetAddressCol(column) + currentRow.ToString()].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[ExcelAddress.GetAddressCol(column) + currentRow.ToString()].Style.Fill.BackgroundColor.SetColor(color);
                        }

                        currentRow++;
                    }
                }
            }
        }
        public void Reload()
        {
            try {
                (new Indicador_DAL()).sp_di_indicadores();
            } catch (Exception) { }
        }
        private void SetSessions()
        {
            Session["Indicador.mes_visualización"] = DateFilter.ToString("yyyy-MM");
            Session["Indicador.Code"] = Code;
            Session["Indicador.Detail"] = Detail;
            Session["Indicador.IndexAxis"] = IndexAxis;
            Session["Indicador.Type"] = Type;
            Session["Indicador.Explore"] = hhdNext.Value;
        }
        #endregion
    }
}