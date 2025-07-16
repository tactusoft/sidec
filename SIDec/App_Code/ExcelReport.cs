using GLOBAL.DAL;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Filter;
using System;
using System.Data;
using System.IO;
using System.Web;

using tipoArchivo = GLOBAL.CONST.clConstantes.TipoArchivo;
namespace SIDec
{
    public class ExcelReport
    {
        private const int ROW_HEADER = 3;
        private const int ROW_TRACING = 30;
        private const int ROW_ACTIVITY = 24;
        private const int ROW_MANAGEMENT = 17;

        #region Public Methods


        public static void LoadReportSheet(ExcelPackage pck, ExcelWorksheet wsBase, int IdBanco)
        {
            int rowLastTracing;
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("COPIA", wsBase);

            DateTime maxDate = new DateTime(1, 1, 1);
            int rowFirstTracing = ROW_TRACING;

            LoadReportHeader(IdBanco, ws);
            rowLastTracing = LoadReportTracing(IdBanco, ws, ref maxDate);
            rowFirstTracing += LoadReportActivity(IdBanco, ws);
            rowFirstTracing += LoadReportManagement(IdBanco, ws);

            ExcelRangeBase range = ws.Cells["A" + rowFirstTracing.ToString() + ":A" + (rowFirstTracing + rowLastTracing).ToString()];
            range.AutoFilter = true;
            var colFilter = ws.AutoFilter.Columns.AddValueFilterColumn(0);
            colFilter.Filters.Add(new ExcelFilterDateGroupItem(maxDate.Year, maxDate.Month));
            ws.Select("A1");
            ws.AutoFilter.ApplyFilter();
        }


        public static void LoadReportProyectList(ExcelWorksheet ws, DataSet dsList)
        {
            int currentRow = 2;

            if (dsList != null && dsList.Tables != null && dsList.Tables.Count > 0)
            {
                foreach (DataRow dr in dsList.Tables[0].Rows)
                {
                    ws.Cells[currentRow.ToString() + ":" + currentRow.ToString()].Copy(ws.Cells[(currentRow + 1).ToString() + ":" + (currentRow + 1).ToString()]);
                    ws.InsertRow(currentRow + 1, 1);
                    int column = 0;
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["ID"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["tipo_proyecto"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["nombre"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["estado_proyecto"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["descripcion"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fecha_primera_radicacion"].ToString(), true);
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["entidad_responsable"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["responsables"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["localidad"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["upz"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["tratamiento"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["instrumento"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["vinculacion"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["poblacion_beneficiaria"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), (Convert.ToDouble(dr["area_bruta"]) / 10000.0).ToString("N5"));
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), (Convert.ToDouble(dr["area_neta"]) / 10000.0).ToString("N5"));
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), (Convert.ToDouble(dr["suelo_util"]) / 10000.0).ToString("N5"));
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["viviendas_vip"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["viviendas_vis"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["viviendas_novip"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["totalviviendas"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["cesion_parque"].ToString());
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["cesion_equipamiento"].ToString());
                    column += 4;
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fec_inicio_ventas"].ToString(), true);
                    ExcelWrite(ws, ExcelAddress.GetAddressCol(++column) + currentRow.ToString(), dr["fec_inicio_construccion"].ToString(), true);

                    if (dr["activo"].ToString() == "0")
                    {
                        ws.Cells["A" + currentRow.ToString() + ":" + "A" + currentRow.ToString()].Style.Font.Italic = true;
                        ws.Cells["A" + currentRow.ToString() + ":" + "A" + currentRow.ToString()].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells["A" + currentRow.ToString() + ":" + "A" + currentRow.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 204, 204));
                    }

                    currentRow++;
                }
            }
        }
        #endregion



        #region Private Methods
        private static void ColumnsMergeLeft(ExcelWorksheet ws, string Cells)
        {
            if (!ws.Cells[Cells].Merge)
                ws.Cells[Cells].Merge = true;
            ws.Cells[Cells].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        }
        private static void ExcelInsertImage(ExcelWorksheet ws, int row, int col, string file_path, decimal width_col, decimal height_col)
        {
            if (Path.GetExtension(file_path) == "")
                file_path += ".jpg";
            if (!System.IO.File.Exists(file_path))
            {
                file_path = file_path.Replace(".jpg", ".png");
                if (!System.IO.File.Exists(file_path))
                    return;
            }

            var img = System.Drawing.Image.FromFile(file_path);

            decimal new_width = width_col;
            decimal new_height = height_col;
            decimal pic_width = Convert.ToDecimal(img.Width) / Convert.ToDecimal(img.HorizontalResolution) * 100;
            decimal pic_height = Convert.ToDecimal(img.Height) / Convert.ToDecimal(img.VerticalResolution) * 100;
            decimal rw = 1;
            decimal rh = 1;

            if (pic_width > pic_height)
                rw = pic_width / pic_height;
            else
                rh = pic_height / pic_width;

            if (rw > 1)
            {
                if (pic_width > width_col)
                    new_height = width_col / rw;
                if (pic_height > height_col)
                {
                    new_width = width_col / new_height * height_col;
                    new_height = height_col;
                }
            }
            else if (rh >= 1)
            {
                if (pic_height >= height_col)
                    new_width = height_col / rh;
                if (new_width >= width_col)
                {
                    new_height = height_col / new_width * width_col;
                    new_width = width_col;
                }
            }

            new_width = new_width * 98 / 100;
            new_height = new_height * 98 / 100;

            var pic = ws.Drawings.AddPicture(Guid.NewGuid().ToString(), file_path);
            pic.SetSize(Convert.ToInt32(new_width / pic_width * 100));
            int or = Convert.ToInt32((height_col - new_height) / 2);
            int oc = Convert.ToInt32((width_col - new_width) / 2);

            while (oc > ws.Column(col).Width * (1000 / 142.3))
            {
                oc -= Convert.ToInt32(ws.Column(col).Width * (1000 / 142.3));
                if (col == int.MaxValue)
                    throw new ArgumentOutOfRangeException("col", "valor inválido");
                col++;
            }

            //pic.SetPosition(row, or, col, oc);
            pic.SetPosition(row, or, col - 1, oc);
        }
        private static void ExcelWrite(ExcelWorksheet ws, string address, string valor, bool isDate = false, bool isFormula = false)
        {
            if (!string.IsNullOrEmpty(valor.Trim()))
            {
                try
                {
                    if (isFormula)
                    {
                        ws.Cells[address].Formula = valor;
                        return;
                    }
                }
                catch { }

                try
                {
                    if (isDate)
                        if (DateTime.TryParse(HttpUtility.HtmlDecode(valor), out DateTime _date))
                        {
                            ws.Cells[address].Value = _date;
                            return;
                        }
                }
                catch { }

                try
                {
                    if (decimal.TryParse(HttpUtility.HtmlDecode(valor), out decimal _decimal))
                    {
                        ws.Cells[address].Value = _decimal;
                        return;
                    }
                }
                catch { }

                try
                {
                    if (int.TryParse(HttpUtility.HtmlDecode(valor), out int _int))
                    {
                        ws.Cells[address].Value = _int;
                        return;
                    }
                }
                catch { }
            }
            try
            {
                ws.Cells[address].Value = HttpUtility.HtmlDecode(valor);
            }
            catch { };
        }


        private static int LoadReportActivity(int IdBanco, ExcelWorksheet ws)
        {
            BANCOACTIVIDADES_DAL oActividades = new BANCOACTIVIDADES_DAL();
            DataSet dsManagement = oActividades.sp_s_bancoactividades_listar(IdBanco, p_activo: true, p_clave: true);
            int currentRow = ROW_ACTIVITY;
            if (dsManagement != null && dsManagement.Tables != null && dsManagement.Tables.Count > 0)
            {
                int firstRow = currentRow;
                foreach (DataRow dr in dsManagement.Tables[0].Rows)
                {
                    ws.InsertRow(currentRow + 1, 1);
                    ws.Cells[currentRow.ToString() + ":" + currentRow.ToString()].Copy(ws.Cells[(currentRow + 1).ToString() + ":" + (currentRow + 1).ToString()]);
                    ColumnsMergeLeft(ws, "B" + currentRow.ToString() + ":D" + currentRow.ToString());
                    ColumnsMergeLeft(ws, "F" + currentRow.ToString() + ":H" + currentRow.ToString());
                    ColumnsMergeLeft(ws, "J" + currentRow.ToString() + ":L" + currentRow.ToString());

                    ExcelWrite(ws, "A" + currentRow.ToString(), dr["estado_actividad"].ToString());
                    ExcelWrite(ws, "B" + currentRow.ToString(), dr["nombre"].ToString());
                    ExcelWrite(ws, "E" + currentRow.ToString(), dr["relacionamiento"].ToString());
                    ExcelWrite(ws, "F" + currentRow.ToString(), dr["tramite"].ToString());
                    ExcelWrite(ws, "I" + currentRow.ToString(), dr["fec_posible"].ToString(), true);
                    ExcelWrite(ws, "J" + currentRow.ToString(), dr["acciones"].ToString());
                    currentRow++;
                }
                ws.DeleteRow(currentRow, 1);
                return currentRow - firstRow - 1;  //rows in section minus row deleted
            }
            return 0;
        }
        private static void LoadReportChartGantt(ExcelWorksheet ws, int rowFirst, int rowLast, DateTime minDate, DateTime maxDate)
        {
            DateTime dateOne = new DateTime(1900, 1, 1);

            //Add a bar chart
            var chart = (ExcelBarChart)ws.Drawings["bar3dChart"];
            var serie1 = chart.Series[0];
            serie1.XSeries = ws.Cells[rowFirst, 3, rowLast, 3].FullAddress;
            serie1.Series = ws.Cells[rowFirst, 7, rowLast, 7].FullAddress;
            serie1.Fill.Color = System.Drawing.Color.White;
            serie1.Fill.Transparancy = 100;

            var serie2 = chart.Series[1];
            serie2.XSeries = ws.Cells[rowFirst, 3, rowLast, 3].FullAddress;
            serie2.Series = ws.Cells[rowFirst, 13, rowLast, 13].FullAddress;

            var serie3 = chart.Series[2];
            serie3.XSeries = ws.Cells[rowFirst, 3, rowLast, 3].FullAddress;
            serie3.Series = ws.Cells[rowFirst, 14, rowLast, 14].FullAddress;

            chart.YAxis.MinValue = (minDate.Date - dateOne).TotalDays;
            chart.YAxis.MaxValue = (maxDate.Date - dateOne).TotalDays;

            double MajorUnit = Math.Round((chart.YAxis.MaxValue.Value - chart.YAxis.MinValue.Value) / 50.0);
            double MinorUnit = Math.Round(MajorUnit / 7.0);
            chart.YAxis.MajorUnit = MajorUnit < 7 ? 7 : MajorUnit;
            chart.YAxis.MinorUnit = MinorUnit < 1 ? 1 : MinorUnit;


            double width = 1200;
            double height = 70 + (rowLast - rowFirst) * 18;

            height = height < 160 ? 160 : height;

            chart.SetSize((int)width, (int)height);
            ws.Row(11).Height = height * 2 / 3 + 30;

        }
        private static void LoadReportHeader(int IdBanco, ExcelWorksheet ws)
        {
            BANCOPROYECTOS_DAL oBanco = new BANCOPROYECTOS_DAL();
            DataSet dsHeader = oBanco.sp_s_banco_consultar(IdBanco, 0);
            if (dsHeader != null && dsHeader.Tables != null && dsHeader.Tables.Count > 0)
            {
                DataRow dr = dsHeader.Tables[0].Rows[0];
                ws.Name = dr["nombre"].ToString().ToUpper().Replace(" ", "_");

                int firstRowHeader = ROW_HEADER;
                ExcelWrite(ws, "A" + firstRowHeader.ToString(), dr["codigo"].ToString());
                ExcelWrite(ws, "B" + firstRowHeader.ToString(), dr["nombre"].ToString());
                firstRowHeader += 2;    //5
                ExcelWrite(ws, "A" + firstRowHeader.ToString(), dr["descripcion"].ToString());
                firstRowHeader += 3;    //8
                ExcelWrite(ws, "A" + firstRowHeader.ToString(), dr["estado_proyecto"].ToString());
                ExcelWrite(ws, "C" + firstRowHeader.ToString(), dr["localidad"].ToString());
                ExcelWrite(ws, "E" + firstRowHeader.ToString(), dr["tratamiento"].ToString());
                ExcelWrite(ws, "G" + firstRowHeader.ToString(), dr["area_bruta"].ToString());
                ExcelWrite(ws, "H" + firstRowHeader.ToString(), dr["area_neta"].ToString());
                ExcelWrite(ws, "I" + firstRowHeader.ToString(), dr["area_suelo_util"].ToString());
                ExcelWrite(ws, "J" + firstRowHeader.ToString(), dr["cesion_parque"].ToString());
                ExcelWrite(ws, "K" + firstRowHeader.ToString(), dr["cesion_equipamiento"].ToString());
                firstRowHeader += 2;    //10
                ExcelWrite(ws, "A" + firstRowHeader.ToString(), dr["vinculacion"].ToString());
                ExcelWrite(ws, "D" + firstRowHeader.ToString(), dr["upz"].ToString());
                ExcelWrite(ws, "G" + firstRowHeader.ToString(), dr["viviendas_vip"].ToString());
                ExcelWrite(ws, "H" + firstRowHeader.ToString(), dr["viviendas_vis"].ToString());
                ExcelWrite(ws, "I" + firstRowHeader.ToString(), dr["viviendas_novip"].ToString());
                ExcelWrite(ws, "J" + firstRowHeader.ToString(), dr["totalviviendas"].ToString());
                ExcelWrite(ws, "K" + firstRowHeader.ToString(), dr["poblacion_beneficiaria"].ToString());

                LoadReportImages(ws, dr["idarchivo"].ToString());
            }
        }
        private static void LoadReportImages(ExcelWorksheet ws, string pidArchivo)
        {
            int MAX_RECURSION = 1;
            if (!int.TryParse(pidArchivo, out int idArchivo))
                return;
            ARCHIVO_DAL oArchivo = new ARCHIVO_DAL();
            int iRow = 2, iCol = 12;
            decimal img_w = 250, img_h = 210;

            DataSet ds = oArchivo.sp_s_archivos_listar_hijos(idArchivo.ToString(), ((int)tipoArchivo.IMG_BP).ToString());
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    if (MAX_RECURSION <= 0)
                        return;
                    string new_path = dr["ruta"].ToString();
                    ExcelInsertImage(ws, iRow, iCol, new_path, img_w, img_h);
                    iCol += 3;
                    MAX_RECURSION--;
                }
            }
        }
        private static int LoadReportManagement(int IdBanco, ExcelWorksheet ws)
        {
            BANCOACTIVIDADES_DAL oActividades = new BANCOACTIVIDADES_DAL();
            DataSet dsActivity = oActividades.sp_s_bancoactividades_listar(IdBanco, p_activo: true);

            int currentRow = ROW_MANAGEMENT;
            if (dsActivity != null && dsActivity.Tables != null && dsActivity.Tables.Count > 0)
            {
                DateTime minDateAct = DateTime.Now, maxDateAct = DateTime.Now;
                int firstRow = currentRow;
                foreach (DataRow dr in dsActivity.Tables[0].Rows)
                {
                    ColumnsMergeLeft(ws, "C" + currentRow.ToString() + ":" + "E" + currentRow.ToString());
                    ws.InsertRow(currentRow + 1, 1);
                    ws.Cells[currentRow.ToString() + ":" + currentRow.ToString()].Copy(ws.Cells[(currentRow + 1).ToString() + ":" + (currentRow + 1).ToString()]);
                    ExcelWrite(ws, "A" + currentRow.ToString(), dr["clave"].ToString() == "1" ? "X" : "");
                    ExcelWrite(ws, "B" + currentRow.ToString(), dr["estado_actividad"].ToString());
                    ExcelWrite(ws, "C" + currentRow.ToString(), dr["nombre"].ToString());
                    ExcelWrite(ws, "F" + currentRow.ToString(), dr["relacionamiento"].ToString());
                    ExcelWrite(ws, "G" + currentRow.ToString(), dr["fec_inicio"].ToString(), true);
                    ExcelWrite(ws, "H" + currentRow.ToString(), dr["fec_culminacion"].ToString(), true);
                    ExcelWrite(ws, "I" + currentRow.ToString(), dr["dias_en_tramite"].ToString());
                    ExcelWrite(ws, "J" + currentRow.ToString(), dr["fec_finalizacion"].ToString(), true);
                    ExcelWrite(ws, "L" + currentRow.ToString(), dr["porccompletado"].ToString());

                    DateTime.TryParse(dr["fec_culminacion"].ToString(), out DateTime fecClm);
                    int dias_disponibles = Convert.ToInt32(dr["dias_disponibles"].ToString());
                    double porcentajeesperado = Convert.ToDouble(dr["porcentajeesperado"].ToString());
                    int semaforo = Convert.ToInt32(dr["semaforo"].ToString());
                    string col = "K";


                    ws.Cells[col + currentRow.ToString() + ":" + col + currentRow.ToString()].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    if (Math.Abs(semaforo) == 2)
                        if (semaforo == 2)
                        {
                            ws.Cells[col + currentRow.ToString() + ":" + col + currentRow.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LimeGreen);
                            ExcelWrite(ws, col + currentRow.ToString(), "OK, Trámite en términos");
                        }
                        else
                        {
                            ws.Cells[col + currentRow.ToString() + ":" + col + currentRow.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DeepSkyBlue);
                            ExcelWrite(ws, col + currentRow.ToString(), "OK, Trámite fuera de términos");
                        }
                    else
                    {
                        if (semaforo == -1)
                            ws.Cells[col + currentRow.ToString() + ":" + col + currentRow.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                        else if (semaforo == 0)
                            ws.Cells[col + currentRow.ToString() + ":" + col + currentRow.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        else
                            ws.Cells[col + currentRow.ToString() + ":" + col + currentRow.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green);

                        ExcelWrite(ws, col + currentRow.ToString(), dias_disponibles.ToString("N0") + (dias_disponibles == 1 || dias_disponibles == -1 ? " día" : " días"));


                        if (DateTime.TryParse(dr["fec_inicio"].ToString(), out DateTime fecIni))
                        {
                            minDateAct = (fecIni < minDateAct ? fecIni : minDateAct).AddDays(-3);
                            maxDateAct = (fecClm > maxDateAct ? fecClm : maxDateAct).AddDays(3);
                        }
                    }

                    if (dr["clave"].ToString() == "1")
                    {
                        ws.Cells["A" + currentRow.ToString() + ":" + "A" + currentRow.ToString()].Style.Font.Bold = true;
                        ws.Cells["A" + currentRow.ToString() + ":" + "A" + currentRow.ToString()].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells["A" + currentRow.ToString() + ":" + "A" + currentRow.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
                    }
                    ws.Cells["A" + currentRow.ToString() + ":" + "L" + currentRow.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    currentRow++;
                }
                ws.DeleteRow(currentRow, 1);
                LoadReportChartGantt(ws, firstRow, currentRow - 1, minDateAct, maxDateAct);
                return currentRow - firstRow - 1;  //rows in section minus row deleted
            }
            return 0;
        }
        private static int LoadReportTracing(int IdBanco, ExcelWorksheet ws, ref DateTime maxDate)
        {
            Seguimientos_DAL oSeguimiento = new Seguimientos_DAL();
            DataSet dsTracing = oSeguimiento.sp_s_seguimiento_reporte(IdBanco.ToString());
            int currentRow = ROW_TRACING;
            if (dsTracing != null && dsTracing.Tables != null && dsTracing.Tables.Count > 0)
            {
                int firstRow = currentRow;
                currentRow++;
                foreach (DataRow dr in dsTracing.Tables[0].Rows)
                {
                    ws.InsertRow(currentRow + 1, 1);
                    ws.Cells[currentRow.ToString() + ":" + currentRow.ToString()].Copy(ws.Cells[(currentRow + 1).ToString() + ":" + (currentRow + 1).ToString()]);
                    ExcelWrite(ws, "A" + currentRow.ToString(), dr["fec_seguimiento"].ToString(), true);
                    ExcelWrite(ws, "B" + currentRow.ToString(), dr["asunto"].ToString());
                    ExcelWrite(ws, "C" + currentRow.ToString(), dr["estado_actividad"].ToString());
                    ExcelWrite(ws, "D" + currentRow.ToString(), dr["tipo_Actividad"].ToString());
                    ExcelWrite(ws, "E" + currentRow.ToString(), dr["actividad"].ToString());
                    ExcelWrite(ws, "F" + currentRow.ToString(), dr["gestion"].ToString());
                    ExcelWrite(ws, "G" + currentRow.ToString(), dr["entidad"].ToString());
                    ExcelWrite(ws, "H" + currentRow.ToString(), dr["participantes"].ToString());
                    ExcelWrite(ws, "I" + currentRow.ToString(), dr["compromisos"].ToString());
                    ExcelWrite(ws, "J" + currentRow.ToString(), dr["radicado"].ToString());
                    ExcelWrite(ws, "K" + currentRow.ToString(), dr["entidad_radicado"].ToString());
                    ExcelWrite(ws, "L" + currentRow.ToString(), dr["tramite"].ToString());
                    ExcelWrite(ws, "M" + currentRow.ToString(), dr["observaciones_radicado"].ToString());
                    if (dr["seg_historico"].ToString() == "1")
                    {
                        ws.Cells["A" + currentRow.ToString() + ":" + "N" + currentRow.ToString()].Style.Font.Italic = true;
                        ws.Cells["A" + currentRow.ToString() + ":" + "N" + currentRow.ToString()].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells["A" + currentRow.ToString() + ":" + "N" + currentRow.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(241, 242, 243));
                    }
                    DateTime.TryParse(dr["fec_seguimiento"].ToString(), out DateTime fec_seguimiento);
                    maxDate = maxDate < fec_seguimiento ? fec_seguimiento : maxDate;
                    currentRow++;
                }
                ws.DeleteRow(currentRow, 1);
                return currentRow - firstRow;
            }

            return 0;
        }
        #endregion
    }
}