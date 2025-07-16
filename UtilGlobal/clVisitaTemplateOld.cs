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
 	public class clVisitaTemplateOld : IDisposable
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
						regexText = new Regex("tmpl_resolucion");
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
				string strChb = "X";
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
						case "tmpl_cod_lote":
							text = new Text(dsSource.Tables[0].Rows[0]["cod_lote"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_matricula":
							text = new Text(dsSource.Tables[0].Rows[0]["matricula"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_direccion":
							text = new Text(dsSource.Tables[0].Rows[0]["direccion"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_CHIP":
							text = new Text(dsSource.Tables[0].Rows[0]["CHIP"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_nombre_localidad":
							text = new Text(dsSource.Tables[0].Rows[0]["nombre_localidad"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_cedula_catastral":
							text = new Text(dsSource.Tables[0].Rows[0]["cedula_catastral"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_nombre_UPZ":
							text = new Text(dsSource.Tables[0].Rows[0]["nombre_UPZ"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_fecha_visita":
							text = new Text(Convert.ToDateTime(dsSource.Tables[0].Rows[0]["fecha_visita"].ToString()).ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-ES")));
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_uso_lote":
							text = new Text(dsSource.Tables[0].Rows[0]["uso_lote"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_uso_entorno":
							text = new Text(dsSource.Tables[0].Rows[0]["uso_entorno"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_estado_vias_internas":
							text = new Text(dsSource.Tables[0].Rows[0]["estado_vias_internas"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_estado_vias_perimetrales":
							text = new Text(dsSource.Tables[0].Rows[0]["estado_vias_perimetrales"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_tiene_servidumbres":
							text = new Text(dsSource.Tables[0].Rows[0]["tiene_servidumbres"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_construccion_existente_lote":
							text = new Text(dsSource.Tables[0].Rows[0]["construccion_existente_lote"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_estado_construccion_existente_lote":
							text = new Text(dsSource.Tables[0].Rows[0]["estado_construccion_existente_lote"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_uso_construccion_lote":
							text = new Text(dsSource.Tables[0].Rows[0]["uso_construccion_lote"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_construccion_existente_entorno":
							text = new Text(dsSource.Tables[0].Rows[0]["construccion_existente_entorno"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_estado_consolidacion_entorno":
							text = new Text(dsSource.Tables[0].Rows[0]["estado_consolidacion_entorno"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_obs_vivienda_lote":
							text = new Text(dsSource.Tables[0].Rows[0]["obs_vivienda_lote"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_obs_vivienda_entorno":
							text = new Text(dsSource.Tables[0].Rows[0]["obs_vivienda_entorno"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_nombre_urbanizacion_lote":
							text = new Text(dsSource.Tables[0].Rows[0]["nombre_urbanizacion_lote"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_obs_visita":
							text = new Text(dsSource.Tables[0].Rows[0]["obs_visita"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl_nombre_completo":
							text = new Text(dsSource.Tables[0].Rows[0]["nombre_completo"].ToString());
							run = new Run();
							run.Append(text);
							bookmarkStart.InsertAfterSelf(run);
							break;
						case "tmpl1":
							run = new Run(new RunProperties(new Bold()));
							if (dsSource.Tables[0].Rows[0]["existe_vivienda_lote"].ToString() == "Si")
							{
								text = new Text(strChb);
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmpl2":
							run = new Run(new RunProperties(new Bold()));
							if (dsSource.Tables[0].Rows[0]["existe_vivienda_lote"].ToString() == "No")
							{
								text = new Text(strChb);
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmpl3":
							run = new Run(new RunProperties(new Bold()));
							if (dsSource.Tables[0].Rows[0]["existe_vivienda_entorno"].ToString() == "Si")
							{
								text = new Text(strChb);
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmpl4":
							run = new Run(new RunProperties(new Bold()));
							if (dsSource.Tables[0].Rows[0]["existe_vivienda_entorno"].ToString() == "No")
							{
								text = new Text(strChb);
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmplA":
							run = new Run(new RunProperties(new Bold()));
							if (dsSource.Tables[0].Rows[0]["id_pendiente_topografia"].ToString() == "193")
							{
								text = new Text(strChb);
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmplB":
							run = new Run(new RunProperties(new Bold()));
							if (dsSource.Tables[0].Rows[0]["id_pendiente_topografia"].ToString() == "195")
							{
								text = new Text(strChb);
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmplM":
							run = new Run(new RunProperties(new Bold()));
							if (dsSource.Tables[0].Rows[0]["id_pendiente_topografia"].ToString() == "194")
							{
								text = new Text(strChb);
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;


						case "tmpl_mapa":
							string s = dsSource.Tables[0].Rows[0]["chip"].ToString();
							s = "F:/SIGES_Planos/" + s + ".jpg";
							if (System.IO.File.Exists(s))
							{
								InsertMapIntoBookmark(doc, bookmarkStart, s);
								bookmarkStart.Remove();
							}
							break;

						//**************************************************************************************************************************
						//fotos           
						case "tmpl_img_1_t":
							if (fEsIndiceValido(0, TotalFotos))
							{
								text = new Text(infoFoto[0].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmpl_img_1":
							if (fEsIndiceValido(0, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[0].path.ToString());
								bookmarkStart.Remove();
							}
							break;
						case "tmpl_img_1_d":
							if (fEsIndiceValido(0, TotalFotos))
							{
								text = new Text(infoFoto[0].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;


						case "tmpl_img_2_t":
							if (fEsIndiceValido(1, TotalFotos))
							{
								text = new Text(infoFoto[1].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmpl_img_2":
							if (fEsIndiceValido(1, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[1].path.ToString());
								bookmarkStart.Remove();
							}
							break;
						case "tmpl_img_2_d":
							if (fEsIndiceValido(1, TotalFotos))
							{
								text = new Text(infoFoto[1].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;


						case "tmpl_img_3_t":
							if (fEsIndiceValido(2, TotalFotos))
							{
								text = new Text(infoFoto[2].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmpl_img_3":
							if (fEsIndiceValido(2, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[2].path.ToString());
								bookmarkStart.Remove();
							}
							break;
						case "tmpl_img_3_d":
							if (fEsIndiceValido(2, TotalFotos))
							{
								text = new Text(infoFoto[2].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;


						case "tmpl_img_4_t":
							if (fEsIndiceValido(3, TotalFotos))
							{
								text = new Text(infoFoto[3].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmpl_img_4":
							if (fEsIndiceValido(3, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[3].path.ToString());
								bookmarkStart.Remove();
							}
							break;
						case "tmpl_img_4_d":
							if (fEsIndiceValido(3, TotalFotos))
							{
								text = new Text(infoFoto[3].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;


						case "tmpl_img_5_t":
							if (fEsIndiceValido(4, TotalFotos))
							{
								text = new Text(infoFoto[4].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmpl_img_5":
							if (fEsIndiceValido(4, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[4].path.ToString());
								bookmarkStart.Remove();
							}
							break;
						case "tmpl_img_5_d":
							if (fEsIndiceValido(4, TotalFotos))
							{
								text = new Text(infoFoto[4].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;


						case "tmpl_img_6_t":
							if (fEsIndiceValido(5, TotalFotos))
							{
								text = new Text(infoFoto[5].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmpl_img_6":
							if (fEsIndiceValido(5, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[5].path.ToString());
								bookmarkStart.Remove();
							}
							break;
						case "tmpl_img_6_d":
							if (fEsIndiceValido(5, TotalFotos))
							{
								text = new Text(infoFoto[5].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;


						case "tmpl_img_7_t":
							if (fEsIndiceValido(6, TotalFotos))
							{
								text = new Text(infoFoto[6].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmpl_img_7":
							if (fEsIndiceValido(6, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[6].path.ToString());
								bookmarkStart.Remove();
							}
							break;
						case "tmpl_img_7_d":
							if (fEsIndiceValido(6, TotalFotos))
							{
								text = new Text(infoFoto[6].observacion.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;

						case "tmpl_img_8_t":
							if (fEsIndiceValido(7, TotalFotos))
							{
								text = new Text(infoFoto[7].tipo.ToString());
								run = new Run();
								run.Append(text);
								bookmarkStart.InsertAfterSelf(run);
							}
							break;
						case "tmpl_img_8":
							if (fEsIndiceValido(7, TotalFotos))
							{
								InsertImageIntoBookmark(doc, bookmarkStart, infoFoto[7].path.ToString());
								bookmarkStart.Remove();
							}
							break;
						case "tmpl_img_8_d":
							if (fEsIndiceValido(7, TotalFotos))
							{
								text = new Text(infoFoto[7].observacion.ToString());
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
