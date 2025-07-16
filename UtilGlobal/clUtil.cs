using GLOBAL.LOG;
using GLOBAL.VAR;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GLOBAL.UTIL
{
    public class clUtil : IDisposable
	{
		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clLog oLog = new clLog();

		private const string _SOURCEPAGE = "clUtil";
		public string fFormatText(string str, int option)
		{
			byte[] tempBytes;
			tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(str);
			str = System.Text.Encoding.UTF8.GetString(tempBytes);
			if (option == 1)
			{
				str = str.Replace(" ", "_");
			}
			return str;
		}

		#region-------------------------------------------------------------------- strings
		public bool ValidateText(string input, int minLength = 0, int type = 1)
		{
			bool validated = false;
			int validCases = 0;

			switch (type)
			{
				case 1:  //ALFANUMÉRICO: contener al menos [minLength] caracteres entre letras y números
					for (int i = 0; i < input.Length; i++)
						validCases += char.IsLetterOrDigit(input, i) ? 1 : 0;
					return minLength <= validCases;
				default: return validated;
			}
		}
		#endregion



		#region-------------------------------------------------------------------- excel
		public void fExcelExportDS(string title, DataSet ds, List<string> nombre_hoja, List<GridViewRow> gvr)
		{
			string sTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
			string file_name = fFormatText(title, 1) + "_" + sTimeStamp + ".xlsx"; //********* parametro *********//

			//Crear la Hoja y asignarle un nombre
			using (ExcelPackage pck = new ExcelPackage())
			{
				for (int index_table = 0; index_table < ds.Tables.Count; index_table++)
				{
					List<int> columns_int = new List<int>();
					List<int> columns_dec = new List<int>();
					List<int> columns_date = new List<int>();

					for (int index_column = 0; index_column < ds.Tables[index_table].Columns.Count; index_column++)
					{
						Type type_column = ds.Tables[index_table].Columns[index_column].DataType;
						if (type_column == typeof(Int16) || type_column == typeof(Int32) || type_column == typeof(Int64))
							columns_int.Add(index_column + 1);
						else if (type_column == typeof(Decimal) || type_column == typeof(Double))
							//columns_dec.Add(index_column + 1);
							columns_int.Add(index_column + 1);
						else if (type_column == typeof(DateTime))
							columns_date.Add(index_column + 1);
					}

					ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
					ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nombre_hoja[index_table]);

					int nRows = ds.Tables[index_table].Rows.Count;
					int nCols = ds.Tables[index_table].Columns.Count;

					//FORMATO DE LA HOJA
					ws.Cells.Style.Font.Size = 10;
					ws.DefaultRowHeight = 12;
					ws.View.FreezePanes(2, 1);
					ws.Row(1).Height = 35;

					//Cargar Datatable en la celda indicada        
					ws.Cells["A1"].LoadFromDataTable(ds.Tables[index_table], true);
					fExcelTitleHeader(ws, gvr[index_table]);
					fExcelFormatCell(ws, nRows, nCols);
					fExcelFormatData(ws, columns_int, columns_dec, columns_date);
					fExcelHeaderFooter(ws);
				}
				fExcelSave(pck, file_name);
			}
		}
		public void fExcelExportDSTemplate(string title, DataSet ds, List<string> sheetName, List<string> sheetOrigin, string template, bool autofit = true)
		{
			FileInfo templateFile = new FileInfo(template);
			string file_name = fFormatText(title, 1) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";

			using (ExcelPackage pck = new ExcelPackage(templateFile, true))
			{
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				//for (int index_table = 0; index_table < ds.Tables.Count; index_table++)
				for (int index_table = 0; index_table < sheetName.Count; index_table++)
				{
					ExcelWorksheet ws = pck.Workbook.Worksheets[sheetName[index_table]];
					ws.Cells[sheetOrigin[index_table]].LoadFromDataTable(ds.Tables[index_table], false);
					int nRows = ds.Tables[index_table].Rows.Count;
					int nCols = ds.Tables[index_table].Columns.Count;
					int x = 2;
					try
					{
						x = Convert.ToInt32(sheetOrigin[index_table].Substring(1));
					}
					catch { }
					fExcelFormatCellItems(ws, nRows, nCols, x, autofit);
					fExcelHeaderFooter(ws, "", 1, 2);
				}
				fExcelSave(pck, file_name);
			}
		}
		public void fExcelTitleHeader(ExcelWorksheet ws, GridViewRow gvr)
		{
			if (gvr != null)
			{
				for (int i = 1; i > 0; i++)
				{
					try
					{
						ExcelRange rng = ws.Cells[1, i, 1, i];
						rng.Value = HttpContext.Current.Server.HtmlDecode(gvr.Cells[i - 1].Text); //GridViewRow
																																											//rng.Value = Server.HtmlDecode(gv[index_table].HeaderRow.Cells[i - 1].Text); //GridView
					}
					catch
					{
						i = -1;
					};
				}
			}

		}
		public void fExcelFormatData(ExcelWorksheet ws, List<int> columns_int, List<int> columns_dec, List<int> columns_date)
		{
			//////   Formato Fecha 
			if (columns_date != null)
			{
				foreach (int i in columns_date)
				{
					ws.Column(i).Style.Numberformat.Format = "dd-mm-yyyy";
					ws.Column(i).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
				}
			}

			//////   Formato Numero 
			if (columns_int != null)
			{
				foreach (int i in columns_int)
				{
					ws.Column(i).Style.Numberformat.Format = "#,##0";
				}
			}

			////   Formato Decimal 
			///
			if (columns_dec != null)
			{
				foreach (int i in columns_dec)
				{
					ws.Column(i).Style.Numberformat.Format = "#,##0.00";
				}
			}
		}
		public void fExcelFormatCell(ExcelWorksheet ws, int nRows, int nCols, bool autofit = true)
		{
			//   Titulos de campos
			string sColorBorder = "#999";
			using (ExcelRange rng = ws.Cells[1, 1, 1, nCols])
			{
				rng.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
				rng.Style.Font.Bold = true;
				rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
				rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
				rng.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
				rng.Style.Font.Color.SetColor(Color.White);
				rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rng.Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(sColorBorder));
				rng.Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(sColorBorder));
				rng.Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(sColorBorder));
				rng.Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(sColorBorder));
			}
			//   Items
			if (nRows == int.MaxValue)
				throw new ArgumentOutOfRangeException("nRows", "valor inválido");
			using (ExcelRange rng = ws.Cells[2, 1, nRows + 1, nCols])
			{
				rng.Style.Font.Bold = false;
				rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rng.Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(sColorBorder));
				rng.Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(sColorBorder));
				rng.Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(sColorBorder));
				rng.Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(sColorBorder));
			}
			if (autofit)
				ws.Cells[1, 1, nRows, nCols].AutoFitColumns();
		}
		public void fExcelFormatCellItems(ExcelWorksheet ws, int nRows, int nCols, int iniRow = 2, bool autofit = false)
		{
			string sColorBorder = "#999";
			//   Items
			if (nRows == int.MaxValue)
				throw new ArgumentOutOfRangeException("nRows", "valor inválido");
			using (ExcelRange rng = ws.Cells[iniRow, 1, nRows + 2, nCols])
			{
				rng.Style.Font.Bold = false;
				rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rng.Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(sColorBorder));
				rng.Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(sColorBorder));
				rng.Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(sColorBorder));
				rng.Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(sColorBorder));
			}
			if (autofit)
			{
				if (nRows == int.MaxValue)
					throw new ArgumentOutOfRangeException("nRows", "valor inválido");
				ws.Cells[iniRow, 1, nRows + 1, nCols].AutoFitColumns();
			}
		}
		public void fExcelHeaderFooter(ExcelWorksheet ws, string title = "", int row_i = 1, int row_f = 1, bool change_foot = true)
		{
			try
			{
				if (row_i != 0)
					ws.PrinterSettings.RepeatRows = new ExcelAddress("$" + row_i + ":$" + row_f);
			}
			catch { }
			ws.HeaderFooter.OddHeader.CenteredText = "&24&\"Bold\" " + title;
			if (change_foot)
			{
				ws.HeaderFooter.OddFooter.RightAlignedText = string.Format("Página {0} de {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
				ws.HeaderFooter.OddFooter.LeftAlignedText = "SIGES - Fecha información: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
			}
		}

		public string getLetter(int number, bool nivel = true)
		{
			if (nivel) number--;
			string[] letter = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
			if (number < 0) return "";
			return (number >= 26 ? getLetter((number / 26) - 1, false) : "") + letter[number % 26];
		}
		public void fExcelWrite(ExcelWorksheet ws, string address, string valor, bool isDate = false, bool isFormula = false)
		{
			if (!string.IsNullOrEmpty(valor.Trim()))
			{
				try
				{
					if (isFormula) {
						ws.Cells[address].Formula = valor;// HttpUtility.HtmlDecode(valor);
						return;
					}
				} catch { }

				try
				{
					if (isDate) 
						if( DateTime.TryParse(HttpUtility.HtmlDecode(valor), out DateTime _date))
					{
						ws.Cells[address].Value = _date;
						return;
					}
				} catch { }

				try
				{
					if (decimal.TryParse(HttpUtility.HtmlDecode(valor), out decimal _decimal))
					{
						ws.Cells[address].Value = _decimal;
						return;
					}
				} catch { }

				try
				{
					if (int.TryParse(HttpUtility.HtmlDecode(valor), out int _int))
					{
						ws.Cells[address].Value = _int;
						return;
					}
				} catch { }
			}
			try
			{
				ws.Cells[address].Value = HttpUtility.HtmlDecode(valor);
			} catch { };
		}

		public void fExcelSave(ExcelPackage pck, string file_name, bool withProperties = true)
		{
			try
			{
				//Asignar Información del archivo creado
				if (withProperties)
				{
					pck.Workbook.Properties.Company = "Secretaría Distrital del Hábitat";
					pck.Workbook.Properties.Author = "Subdirección de Gestión del Suelo";
					pck.Workbook.Properties.Application = "SIGES";
					pck.Workbook.Properties.Comments = file_name;
				}

				//Permitir la opcion de Guardar en el cliente el ExcelPackage creado
				HttpContext.Current.Response.Clear();
				HttpContext.Current.Response.ClearHeaders();
				HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=\"" + file_name + "\"");
				HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				HttpContext.Current.Response.BinaryWrite(pck.GetAsByteArray());
				HttpContext.Current.Response.Flush();
				HttpContext.Current.Response.End();
				//HttpContext.Current.ApplicationInstance.CompleteRequest();
			}
			catch (Exception Error)
			{
				if(!Error.Message.Contains("Subproceso anulado") && !Error.Message.Contains("Thread was being aborted"))
					oLog.RegistrarLogError(Error, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + Error.Message);
			}
		}
		public void fExcelInsertImage(ExcelWorksheet ws, int row, int col, string file_path, decimal width_col, decimal height_col)
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
		#endregion



		#region-------------------------------------------------------------------- old
		public void ClearSession()
		{
			System.Web.HttpContext.Current.Session.Clear();
			System.Web.HttpContext.Current.Session.Abandon();
			//oVar.prSubModuloActivo = 0;
			oVar.prUser = "";
			oVar.prIP = "";
		}
		public void setIPSesion()
		{
			string IPSesion = "ø";
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					IPSesion = ip.ToString();
				}
			}
			oVar.prIP = IPSesion;
		}
		public string CreaPW()
		{
			string Var1 = DateTime.Now.DayOfWeek.ToString();
			string Var2 = DateTime.Now.Add(System.TimeSpan.FromDays(-1)).DayOfWeek.ToString();
			string Var3 = DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString();

			int iMax1 = Var1.Length - 1;
			int iMax2 = Var2.Length - 1;

			Random oNum = new Random();
			return Var1.Substring(oNum.Next(0, iMax1), 1).ToUpper() + Var1.Substring(oNum.Next(0, iMax1 - 1), 1).ToUpper() + Var2.Substring(oNum.Next(0, iMax2), 1).ToLower() + Var2.Substring(oNum.Next(0, iMax2 - 1), 1).ToLower() + DateTime.Today.Day.ToString().PadLeft(2, '0') + Var3.Substring(0, 2) + oNum.Next(0, 99).ToString().PadLeft(2, '0');
		}
		public string CrearUniqueName()
		{
			return DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
		}
		public bool DBOperationResult(string Resultado)
		{
			if (Resultado.Substring(0, 5) == "00000" && Convert.ToInt16(Resultado.Substring(6)) > 0)
				return true;
			else
				return false;
		}
		public byte[] HashPW(string csena)
		{
			HashAlgorithm haPW = HashAlgorithm.Create("MD5");
			byte[] bPW = System.Text.Encoding.Unicode.GetBytes(csena);
			byte[] bPWHash = haPW.ComputeHash(bPW);
			return bPWHash;
		}
		/// Convertir un Dataview en un DataSet
		public DataSet ConvertToDataSet(DataView dtView)
		{
			DataSet dsSorted = new DataSet();
			dsSorted.Tables.Add(dtView.ToTable());
			return dsSorted;
		}
		public string ConvertToFechaDB(string fecha)
		{
			if (!string.IsNullOrEmpty(fecha))
				return Convert.ToDateTime(fecha).ToString("yyyyMMdd");
			else
				return null;
		}
		public string ConvertToFechaDetalle(string fecha)
		{
			if (!string.IsNullOrEmpty(fecha))
				return Convert.ToDateTime(fecha).ToString("yyyy-MM-dd");
			else
				return "";
		}
		public int ObtenerNumeroDiaFecha(DateTime Fecha)
		{
			if (Fecha.DayOfWeek == 0)
				return 7;
			else
				return (int)Fecha.DayOfWeek;
		}
		public string VerificarNull(string valor)
		{
			if (string.IsNullOrEmpty(valor))
				valor = null;
			else if (valor == "&nbsp;")
				valor = null;
			return valor;
		}
		public string VerificarDec(string numero)
		{
			if (!string.IsNullOrEmpty(numero))
				return numero.Replace(".", ",");
			else
				return null;
		}
		public string OpDirection(string sortDirection)
		{
			if (sortDirection == "ASC")
				sortDirection = "DESC";
			else
				sortDirection = "ASC";
			return sortDirection;
		}
		public void fEstiloEstadoControl(View viewControl)
		{
            string ctrlEstilo;
            Label lblCtrl;
			foreach (Control ctrlSource in viewControl.Controls)
			{
				//Console.Write(ctrlSource.ID);
				if ((ctrlSource is TextBox || ctrlSource is DropDownList) && ctrlSource.Visible)
				{
                    bool Habilitado = ((WebControl)ctrlSource).Enabled;
                    string ctrlCssClass = ((WebControl)ctrlSource).CssClass;
                    bool isLbl = false;
					try
					{
						lblCtrl = (Label)viewControl.FindControl(((System.Web.UI.WebControls.WebControl)ctrlSource).ID.Contains("txt") ? ((System.Web.UI.WebControls.WebControl)ctrlSource).ID.Replace("txt", "lbl") : ((System.Web.UI.WebControls.WebControl)ctrlSource).ID.Replace("ddlb", "lbl"));
						if (lblCtrl != null)
							isLbl = true;
					}
					catch
					{
						isLbl = false;
					}
					if (isLbl)
					{
						lblCtrl = (Label)viewControl.FindControl(((WebControl)ctrlSource).ID.Contains("txt") ? ((WebControl)ctrlSource).ID.Replace("txt", "lbl") : ((WebControl)ctrlSource).ID.Replace("ddlb", "lbl"));
                        string lblCssClass = lblCtrl.CssClass;
                        ctrlEstilo = ctrlCssClass;
                        string lblEstilo = lblCssClass;
                        if (Habilitado)
						{
							if (ctrlEstilo.Contains(" txtDis"))
								ctrlEstilo = ctrlEstilo.Replace(" txtDis", "");
							if (lblEstilo.Contains(" lblDis"))
								lblEstilo = lblEstilo.Replace(" lblDis", "");
						}
						else
						{
							if (!ctrlEstilo.Contains(" txtDis"))
								ctrlEstilo = ctrlEstilo.Insert(ctrlEstilo.Length, " txtDis");
							if (!lblEstilo.Contains(" lblDis"))
								lblEstilo = lblEstilo.Insert(lblEstilo.Length, " lblDis");
						}
							((System.Web.UI.WebControls.WebControl)ctrlSource).CssClass = ctrlEstilo;
						lblCtrl.CssClass = lblEstilo;
					}
				}
			}
		}
		public Control FindControlRecursive(Control root, string id)
		{
			if (root.ID == id)
			{
				return root;
			}
			foreach (Control c in root.Controls)
			{
				Control t = FindControlRecursive(c, id);
				if (t != null)
				{
					return t;
				}
			}
			return null;
		}
		public void fSetIndex(GridView gv)
		{
			if (gv.Rows.Count >= Convert.ToInt16(oVar.prIndexValue))
				gv.SelectedIndex = Convert.ToInt16(oVar.prIndexValue);
			else
				gv.SelectedIndex = 0;
		}
		public void fSetIndex2(GridView gv, int index)
		{
			try
			{
				if (gv.Rows.Count >= index)
					gv.SelectedIndex = index;
				else
					gv.SelectedIndex = 0;
			}
			catch { }
		}
		#endregion

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// dispose managed resources
				oVar.Dispose();
			}
			// free native resources
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}