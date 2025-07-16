using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GLOBAL.UTIL;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace GLOBAL.CONCEPTOTEMPLATE
{
	public class clConceptoTemplate : IDisposable
	{
		public Exception fCrearTemplate(string pathDocument, DataSet dsSource)
		{
			return fCrearPlantilla(pathDocument, dsSource);
		}
		private static Exception fCrearPlantilla(string pathDocument, DataSet dsSource)
		{
			try
			{
				using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(pathDocument, true))
				{
					string docText = null;
					Regex regexText;
					using (StreamReader sr = new StreamReader(wordprocessingDocument.MainDocumentPart.GetStream()))
					{
						docText = sr.ReadToEnd();
					}
					foreach (HeaderPart header in wordprocessingDocument.MainDocumentPart.HeaderParts)
					{
						string headerText = null;
						using (StreamReader sr = new StreamReader(header.GetStream()))
						{
							headerText = sr.ReadToEnd();
						}
						regexText = new Regex("tmpl_chip_encabezado");
						headerText = regexText.Replace(headerText, dsSource.Tables[0].Rows[0]["chip"].ToString());
						using (StreamWriter sw = new StreamWriter(header.GetStream(FileMode.Create)))
						{
							sw.Write(headerText);
						}
						header.Header.Save();
					}
					using (StreamWriter sw = new StreamWriter(wordprocessingDocument.MainDocumentPart.GetStream(FileMode.Create)))
					{
						sw.Write(docText);
					}
				}
				return ReplaceBookmarks(pathDocument, dsSource);
			}
			catch (Exception MyError)
			{
				return MyError;
			}
		}
		private static Exception ReplaceBookmarks(string wordDocTemplatePath, DataSet dsSource)
		{
			WordprocessingDocument doc = WordprocessingDocument.Open(wordDocTemplatePath, true);
			try
			{
				Text text = new Text();
				Run run = new Run();
				RunProperties runProperties = new RunProperties();
				// Read all bookmarks from the word doc
				foreach (BookmarkStart bookmarkStart in doc.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
				{
					switch (bookmarkStart.Name.ToString())
					{
						case "tmpl_tipo_declaratoria":
							text = new Text(dsSource.Tables[0].Rows[0]["tipo_declaratoria"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new Bold()));
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_resolucion_declaratoria":
							text = new Text(dsSource.Tables[0].Rows[0]["resolucion_declaratoria"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new Bold()));
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_resolucion_declaratoria_ano":
							text = new Text(dsSource.Tables[0].Rows[0]["resolucion_declaratoria_ano"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new Bold()));
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertBeforeSelf(run);
							break;
						case "tmpl_tipo_concepto":
							text = new Text(dsSource.Tables[0].Rows[0]["tipo_concepto"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new Bold()));
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertBeforeSelf(run);
							break;
						case "tmpl_chip":
							text = new Text(dsSource.Tables[0].Rows[0]["chip"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new Bold()));
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_fecha_concepto":
							text = new Text(Convert.ToDateTime(dsSource.Tables[0].Rows[0]["fecha_concepto"].ToString()).ToString("dd/MMMM/yyyy", CultureInfo.CreateSpecificCulture("es-ES")).ToUpper());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new Bold()));
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmpl_cod_lote":
							text = new Text(dsSource.Tables[0].Rows[0]["cod_lote"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_matricula":
							text = new Text(dsSource.Tables[0].Rows[0]["matricula"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_direccion":
							text = new Text(dsSource.Tables[0].Rows[0]["direccion"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_chip2":
							text = new Text(dsSource.Tables[0].Rows[0]["chip"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_barrio":
							text = new Text(dsSource.Tables[0].Rows[0]["barrio"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_upz":
							text = new Text(dsSource.Tables[0].Rows[0]["UPZ"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_localidad":
							text = new Text(dsSource.Tables[0].Rows[0]["localidad"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_cedula_catastral":
							text = new Text(dsSource.Tables[0].Rows[0]["cedula_catastral"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmpl_antecedentes":
							text = new Text(dsSource.Tables[0].Rows[0]["antecedentes"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_area_terreno_UAECD":
							text = new Text(dsSource.Tables[0].Rows[0]["area_terreno_UAECD"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_area_construccion":
							text = new Text(dsSource.Tables[0].Rows[0]["area_construccion"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_argumentos":
							text = new Text(dsSource.Tables[0].Rows[0]["argumentos"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_conclusiones":
							text = new Text(dsSource.Tables[0].Rows[0]["conclusiones"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_consideraciones":
							text = new Text(dsSource.Tables[0].Rows[0]["consideraciones"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_objeto":
							text = new Text(dsSource.Tables[0].Rows[0]["objeto"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_soportes":
							text = new Text(dsSource.Tables[0].Rows[0]["sd_otros"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.AppendChild(text);
							bookmarkStart.InsertAfterSelf(run);

							run.AppendChild(new Break());
							run.AppendChild(new Break());
							for (int i = 5; i >= 1; i--)
							{
								text = new Text(dsSource.Tables[0].Rows[0]["sd_" + i + "_t"].ToString());
								text.Text = string.Concat(text.Text, new Break());
								run = new Run();
								runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
								run.AppendChild(text);
								bookmarkStart.InsertAfterSelf(run);

								run.AppendChild(new Break());
								run.AppendChild(new Break());
							}
							break;
						case "tmpl_usu_concepto":
							text = new Text(dsSource.Tables[0].Rows[0]["usu_concepto"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_usu_concepto_matricula":
							text = new Text(dsSource.Tables[0].Rows[0]["usu_concepto_matricula"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_usu_concepto_revisa":
							text = new Text(dsSource.Tables[0].Rows[0]["usu_concepto_revisa"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_usu_concepto_revisa_matricula":
							text = new Text(dsSource.Tables[0].Rows[0]["usu_concepto_revisa_matricula"].ToString());
							run = new Run();
							runProperties = run.AppendChild(new RunProperties(new FontSize() { Val = "24" }));
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
					}
				}
				var body = doc.MainDocumentPart.Document.Body;
				foreach (var para in body.Elements<Paragraph>())
				{
					foreach (var run1 in para.Elements<Run>())
					{
						foreach (var text1 in run1.Elements<Text>())
						{
							if (text1.Text.Contains("DocumentFormat.OpenXml.Wordprocessing.Break"))
							{
								text1.Text = text1.Text.Replace("DocumentFormat.OpenXml.Wordprocessing.Break", "");
							}
						}
					}
				}
				doc.Close();
				return null;
			}
			catch (Exception MyErr)
			{
				doc.Close();
				return MyErr;
			}
		}

		#region-----DISPOSE
		// Metodo para el manejo del GC
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly")]
		public void Dispose()
		{
			GC.SuppressFinalize(true);
		}
		#endregion
	}
}
