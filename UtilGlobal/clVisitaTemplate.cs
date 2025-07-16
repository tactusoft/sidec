using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GLOBAL.UTIL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace GLOBAL.VISITATEMPLATE
{
    public class clVisitaTemplate : IDisposable
	{
		public Exception fCrearTemplate(string pathDocument, DataSet dsSource)
		{
			return fCrearPlantilla(pathDocument, dsSource);
		}
		private static Exception fCrearPlantilla(string pathDocument, DataSet dsSource)
		{
			try
			{
				// Open a WordprocessingDocument for editing using the filepath.
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
						regexText = new Regex("tmp_tipo_dec");
						headerText = regexText.Replace(headerText, dsSource.Tables[0].Rows[0]["tipo_declaratoria"].ToString());
						using (StreamWriter sw = new StreamWriter(header.GetStream(FileMode.Create)))
						{
							sw.Write(headerText);
						}
						regexText = new Regex("tmp_resolucion");
						headerText = regexText.Replace(headerText, dsSource.Tables[0].Rows[0]["resolucion"].ToString());
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
				int TotalFotos = -1;

				clFile oFile = new clFile();
				var infoFoto = new List<clInfoFoto>();
				infoFoto = oFile.fGetFilesProp();

				if (infoFoto != null)
					TotalFotos = infoFoto.Count;

				// Read all bookmarks from the word doc
				foreach (BookmarkStart bookmarkStart in doc.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
				{
					switch (bookmarkStart.Name.ToString())
					{
						case "tmp_chip":
							text = new Text(dsSource.Tables[0].Rows[0]["chip"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_cod_lote":
							text = new Text(dsSource.Tables[0].Rows[0]["cod_lote"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_cedula_catastral":
							text = new Text(dsSource.Tables[0].Rows[0]["cedula_catastral"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_matricula":
							text = new Text(dsSource.Tables[0].Rows[0]["matricula"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_fecha_visita":
							text = new Text(Convert.ToDateTime(dsSource.Tables[0].Rows[0]["fecha_visita"].ToString()).ToString("dd/MM/yyyy"));
							text = new Text(Convert.ToDateTime(dsSource.Tables[0].Rows[0]["fecha_visita"].ToString()).ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-ES")));
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_direccion":
							text = new Text(dsSource.Tables[0].Rows[0]["direccion"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_nombre_UPZ":
							text = new Text(dsSource.Tables[0].Rows[0]["nombre_UPZ"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_nombre_localidad":
							text = new Text(dsSource.Tables[0].Rows[0]["nombre_localidad"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmp_mapa":
							string s = dsSource.Tables[0].Rows[0]["chip"].ToString();
							s = "F:/SIGES_Planos/" + s + ".jpg";
							if (System.IO.File.Exists(s))
							{
								InsertMapIntoBookmark(doc, bookmarkStart, s);
								bookmarkStart.Remove();
							}
							break;

						case "tmp_lic_construccion":
							text = new Text(dsSource.Tables[0].Rows[0]["lic_construccion"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_lic_urbanismo":
							text = new Text(dsSource.Tables[0].Rows[0]["lic_urbanismo"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_lic_sin_valla":
							text = new Text(dsSource.Tables[0].Rows[0]["lic_sin_valla"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_nombre_urbanizacion_lote":
							text = new Text(dsSource.Tables[0].Rows[0]["nombre_urbanizacion_lote"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmp_id_ocupacion_visita1":
							text = new Text(dsSource.Tables[0].Rows[0]["id_ocupacion_visita1"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_id_ocupacion_visita2":
							text = new Text(dsSource.Tables[0].Rows[0]["id_ocupacion_visita2"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_id_ocupacion_visita3":
							text = new Text(dsSource.Tables[0].Rows[0]["id_ocupacion_visita3"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_id_ocupacion_visita4":
							text = new Text(dsSource.Tables[0].Rows[0]["id_ocupacion_visita4"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmp_act_viv_unifamiliar":
							text = new Text(dsSource.Tables[0].Rows[0]["act_viv_unifamiliar"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_act_viv_multifamiliar":
							text = new Text(dsSource.Tables[0].Rows[0]["act_viv_multifamiliar"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_act_parqueadero":
							text = new Text(dsSource.Tables[0].Rows[0]["act_parqueadero"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_act_comercio":
							text = new Text(dsSource.Tables[0].Rows[0]["act_comercio"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_act_otro":
							text = new Text(dsSource.Tables[0].Rows[0]["act_otro"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmp_ent_viv_unifamiliar":
							text = new Text(dsSource.Tables[0].Rows[0]["ent_viv_unifamiliar"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_ent_viv_multifamiliar":
							text = new Text(dsSource.Tables[0].Rows[0]["ent_viv_multifamiliar"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_ent_comercio":
							text = new Text(dsSource.Tables[0].Rows[0]["ent_comercio"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_ent_industria":
							text = new Text(dsSource.Tables[0].Rows[0]["ent_industria"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_ent_otro":
							text = new Text(dsSource.Tables[0].Rows[0]["ent_otro"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmp_acc_via_vehicular":
							text = new Text(dsSource.Tables[0].Rows[0]["acc_via_vehicular"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_acc_via_peatonal":
							text = new Text(dsSource.Tables[0].Rows[0]["acc_via_peatonal"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_acc_ninguna":
							text = new Text(dsSource.Tables[0].Rows[0]["acc_ninguna"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmp_id_pendiente_lote1":
							text = new Text(dsSource.Tables[0].Rows[0]["id_pendiente_lote1"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_id_pendiente_lote2":
							text = new Text(dsSource.Tables[0].Rows[0]["id_pendiente_lote2"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_id_pendiente_lote3":
							text = new Text(dsSource.Tables[0].Rows[0]["id_pendiente_lote3"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_id_pendiente_lote4":
							text = new Text(dsSource.Tables[0].Rows[0]["id_pendiente_lote4"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmp_id_pendiente_ladera1":
							text = new Text(dsSource.Tables[0].Rows[0]["id_pendiente_ladera1"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_id_pendiente_ladera2":
							text = new Text(dsSource.Tables[0].Rows[0]["id_pendiente_ladera2"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_id_pendiente_ladera3":
							text = new Text(dsSource.Tables[0].Rows[0]["id_pendiente_ladera3"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_id_pendiente_ladera4":
							text = new Text(dsSource.Tables[0].Rows[0]["id_pendiente_ladera4"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmp_cob_vivienda":
							text = new Text(dsSource.Tables[0].Rows[0]["cob_vivienda"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_cob_pastos":
							text = new Text(dsSource.Tables[0].Rows[0]["cob_pastos"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_cob_rastrojo":
							text = new Text(dsSource.Tables[0].Rows[0]["cob_rastrojo"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_cob_bosque":
							text = new Text(dsSource.Tables[0].Rows[0]["cob_bosque"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_cob_sin_cobertura":
							text = new Text(dsSource.Tables[0].Rows[0]["cob_sin_cobertura"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_cob_otro":
							text = new Text(dsSource.Tables[0].Rows[0]["cob_otro"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmp_inest_fisuras":
							text = new Text(dsSource.Tables[0].Rows[0]["inest_fisuras"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_inest_fracturas":
							text = new Text(dsSource.Tables[0].Rows[0]["inest_fracturas"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_inest_escarpe":
							text = new Text(dsSource.Tables[0].Rows[0]["inest_escarpe"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_inest_corona":
							text = new Text(dsSource.Tables[0].Rows[0]["inest_corona"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_inest_depositos":
							text = new Text(dsSource.Tables[0].Rows[0]["inest_depositos"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_inest_ninguna":
							text = new Text(dsSource.Tables[0].Rows[0]["inest_ninguna"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_inest_otro":
							text = new Text(dsSource.Tables[0].Rows[0]["inest_otro"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmp_agua_interna":
							text = new Text(dsSource.Tables[0].Rows[0]["agua_interna"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_agua_limitrofe":
							text = new Text(dsSource.Tables[0].Rows[0]["agua_limitrofe"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_agua_amortiguacion":
							text = new Text(dsSource.Tables[0].Rows[0]["agua_amortiguacion"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_agua_obras":
							text = new Text(dsSource.Tables[0].Rows[0]["agua_obras"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_agua_otro":
							text = new Text(dsSource.Tables[0].Rows[0]["agua_otro"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmp_geom_depositos":
							text = new Text(dsSource.Tables[0].Rows[0]["geom_depositos"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_geom_llenos":
							text = new Text(dsSource.Tables[0].Rows[0]["geom_llenos"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_geom_escarpes":
							text = new Text(dsSource.Tables[0].Rows[0]["geom_escarpes"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_geom_llanuras":
							text = new Text(dsSource.Tables[0].Rows[0]["geom_llanuras"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_geom_otro":
							text = new Text(dsSource.Tables[0].Rows[0]["geom_otro"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						case "tmp_obs_visita":
							text = new Text(dsSource.Tables[0].Rows[0]["obs_visita"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_nombre_completo":
							text = new Text(dsSource.Tables[0].Rows[0]["nombre_completo"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmp_matricula_usuario":
							text = new Text(dsSource.Tables[0].Rows[0]["matricula_usuario"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;

						//**************************************************************************************************************************
						//fotos           
						case "tmp_img_1_t":
							if (fEsIndiceValido(0, TotalFotos))
							{
								text = new Text(infoFoto[0].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmp_img_1":
							if (fEsIndiceValido(0, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[0].path.ToString());
								bookmarkStart.Remove();
							}
							break;
						case "tmp_img_1_d":
							if (fEsIndiceValido(0, TotalFotos))
							{
								text = new Text(infoFoto[0].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;

						case "tmp_img_2_t":
							if (fEsIndiceValido(1, TotalFotos))
							{
								text = new Text(infoFoto[1].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmp_img_2":
							if (fEsIndiceValido(1, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[1].path.ToString());
								bookmarkStart.Remove();
							}
							break;
						case "tmp_img_2_d":
							if (fEsIndiceValido(1, TotalFotos))
							{
								text = new Text(infoFoto[1].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;

						case "tmp_img_3_t":
							if (fEsIndiceValido(2, TotalFotos))
							{
								text = new Text(infoFoto[2].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmp_img_3":
							if (fEsIndiceValido(2, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[2].path.ToString());
								bookmarkStart.Remove();
							}
							break;

						case "tmp_img_3_d":
							if (fEsIndiceValido(2, TotalFotos))
							{
								text = new Text(infoFoto[2].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;

						case "tmp_img_4_t":
							if (fEsIndiceValido(3, TotalFotos))
							{
								text = new Text(infoFoto[3].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmp_img_4":
							if (fEsIndiceValido(3, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[3].path.ToString());
								bookmarkStart.Remove();
							}
							break;
						case "tmp_img_4_d":
							if (fEsIndiceValido(3, TotalFotos))
							{
								text = new Text(infoFoto[3].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;

						case "tmp_img_5_t":
							if (fEsIndiceValido(4, TotalFotos))
							{
								text = new Text(infoFoto[4].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;

						case "tmp_img_5":
							if (fEsIndiceValido(4, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[4].path.ToString());
								bookmarkStart.Remove();
							}
							break;

						case "tmp_img_5_d":
							if (fEsIndiceValido(4, TotalFotos))
							{
								text = new Text(infoFoto[4].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;

						case "tmp_img_6_t":
							if (fEsIndiceValido(5, TotalFotos))
							{
								text = new Text(infoFoto[5].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;

						case "tmp_img_6":
							if (fEsIndiceValido(5, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[5].path.ToString());
								bookmarkStart.Remove();
							}
							break;

						case "tmp_img_6_d":
							if (fEsIndiceValido(5, TotalFotos))
							{
								text = new Text(infoFoto[5].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
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
		private static bool fEsIndiceValido(int indexBookmarkFoto, int iTotalFotos)
		{
			return iTotalFotos > indexBookmarkFoto;
		}
		private static Exception InsertImageIntoBookmark(WordprocessingDocument doc, BookmarkStart bookmarkStart, string imageFilename)
		{
			// Remove anything present inside the bookmark
			OpenXmlElement elem = bookmarkStart.NextSibling();
			while (elem != null && !(elem is BookmarkEnd))
			{
				OpenXmlElement nextElem = elem.NextSibling();
				elem.Remove();
				elem = nextElem;
			}
			// Create an imagepart
			var imagePart = AddImagePart(doc.MainDocumentPart, imageFilename);
			System.Drawing.Image img = System.Drawing.Image.FromFile(imageFilename);
			// insert the image part after the bookmark start
			AddImageToBody(doc.MainDocumentPart.GetIdOfPart(imagePart), bookmarkStart, img.Width, img.Height);
			return null;
		}
		private static Exception InsertMapIntoBookmark(WordprocessingDocument doc, BookmarkStart bookmarkStart, string imageFilename)
		{
			// Remove anything present inside the bookmark
			OpenXmlElement elem = bookmarkStart.NextSibling();
			while (elem != null && !(elem is BookmarkEnd))
			{
				OpenXmlElement nextElem = elem.NextSibling();
				elem.Remove();
				elem = nextElem;
			}
			// Create an imagepart
			var imagePart = AddImagePart(doc.MainDocumentPart, imageFilename);
			// insert the image part after the bookmark start
			AddMapToBody(doc.MainDocumentPart.GetIdOfPart(imagePart), bookmarkStart);
			return null;
		}
		private static void AddImageToBody(string relationshipId, BookmarkStart bookmarkStart, int originalWidth, int originalHeight)
		{
			long maxWidth = 2500000L;
			long maxHeight = 2002000L;
			// To preserve the aspect ratio
			float ratioX = (float)maxWidth / (float)originalWidth;
			float ratioY = (float)maxHeight / (float)originalHeight;
			float ratioxy = Math.Min(ratioX, ratioY);

			// New width and height based on aspect ratio
			int newWidth = (int)(originalWidth * ratioxy);
			int newHeight = (int)(originalHeight * ratioxy);

			//***********************************************************************************************
			//************************************************************************************************
			// Define the reference of the image.
			var element =
			new Drawing(
			new DW.Inline(
			new DW.Extent()
			{
				Cx = newWidth,
				Cy = newHeight
			},
			new DW.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
			new DW.DocProperties() { Id = (UInt32Value)1U, Name = "Picture 1" },
			new DW.NonVisualGraphicFrameDrawingProperties(new A.GraphicFrameLocks() { NoChangeAspect = true }),
			new A.Graphic(
			new A.GraphicData(
			new PIC.Picture(
			new PIC.NonVisualPictureProperties(
			new PIC.NonVisualDrawingProperties()
			{
				Id = (UInt32Value)0U,
				Name = "New Bitmap Image.jpg"
			},
			new PIC.NonVisualPictureDrawingProperties()),
			new PIC.BlipFill(
			new A.Blip(
			new A.BlipExtensionList(
			new A.BlipExtension()
			{
				Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}"
			}))
			{
				Embed
			=
			relationshipId,
				CompressionState
			=
			A
			.BlipCompressionValues
			.Print
			},
			new A.Stretch(new A.FillRectangle())),
			new PIC.ShapeProperties(
			new A.Transform2D(new A.Offset() { X = 0L, Y = 0L }, new A.Extents() { Cx = newWidth, Cy = newHeight }),

			new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle })))
			{
				Uri =
			"http://schemas.openxmlformats.org/drawingml/2006/picture"
			}))
			{
				DistanceFromTop = (UInt32Value)0U,
				DistanceFromBottom = (UInt32Value)0U,
				DistanceFromLeft = (UInt32Value)0U,
				DistanceFromRight = (UInt32Value)0U,
				EditId = "50D07946"
			});
			// add the image element to body, the element should be in a Run.
			bookmarkStart.Parent.InsertAfter<Run>(new Run(element), bookmarkStart);
		}
		private static void AddMapToBody(string relationshipId, BookmarkStart bookmarkStart)
		{
			// Define the reference of the image.
			var element =
			new Drawing(
			new DW.Inline(
			new DW.Extent()
			{
				Cx = 6000000L,
				Cy = 3200000L
			},
			new DW.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
			new DW.DocProperties() { Id = (UInt32Value)1U, Name = "Picture 1" },
			new DW.NonVisualGraphicFrameDrawingProperties(new A.GraphicFrameLocks() { NoChangeAspect = true }),
			new A.Graphic(
			new A.GraphicData(
			new PIC.Picture(
			new PIC.NonVisualPictureProperties(
			new PIC.NonVisualDrawingProperties()
			{
				Id = (UInt32Value)0U,
				Name = "New Bitmap Image.jpg"
			},
			new PIC.NonVisualPictureDrawingProperties()),
			new PIC.BlipFill(
			new A.Blip(
			new A.BlipExtensionList(
			new A.BlipExtension()
			{
				Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}"
			}))
			{
				Embed
			=
			relationshipId,
				CompressionState
			=
			A
			.BlipCompressionValues
			.Print
			},
			new A.Stretch(new A.FillRectangle())),
			new PIC.ShapeProperties(
			new A.Transform2D(new A.Offset() { X = 0L, Y = 0L }, new A.Extents() { Cx = 6000000L, Cy = 3200000L }),
			new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle })))
			{
				Uri =
			"http://schemas.openxmlformats.org/drawingml/2006/picture"
			}))
			{
				DistanceFromTop = (UInt32Value)0U,
				DistanceFromBottom = (UInt32Value)0U,
				DistanceFromLeft = (UInt32Value)0U,
				DistanceFromRight = (UInt32Value)0U,
				EditId = "50D07946"
			});
			// add the image element to body, the element should be in a Run.
			bookmarkStart.Parent.InsertAfter<Run>(new Run(element), bookmarkStart);
		}
		public static ImagePart AddImagePart(MainDocumentPart mainPart, string imageFileName)
		{
			ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
			using (FileStream stream = new FileStream(imageFileName, FileMode.Open))
			{
				imagePart.FeedData(stream);
			}
			return imagePart;
		}

		#region-----DISPOSE
		public void Dispose()
		{
			GC.SuppressFinalize(true);
		}
		#endregion
	}
}
