using DocumentFormat.OpenXml.Packaging;
using GLOBAL.VAR;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;


namespace GLOBAL.CARTATERMINOSTEMPLATE
{
    public class clCartaTerminosTemplate : IDisposable
	{
		public Exception fCrearTemplate(string pathDocument, DataSet dsSource, int indexTable, int indexRow)
		{
			return fCrearPlantillaCartaTerminos(pathDocument, dsSource, indexTable, indexRow);
		}
		private static Exception fCrearPlantillaCartaTerminos(string pathDocument, DataSet dsSource, int indexTable, int indexRow)
		{
			clGlobalVar oVar = new clGlobalVar();
			try
			{
				using (WordprocessingDocument doc = WordprocessingDocument.Open(pathDocument, true))
				{
					string docText = null;
					Regex regexText;
					using (StreamReader sr = new StreamReader(doc.MainDocumentPart.GetStream()))
					{
						docText = sr.ReadToEnd();
					}
					regexText = new Regex("tmp_nombre_propietario_tmp");
					docText = regexText.Replace(docText, dsSource.Tables[indexTable].Rows[indexRow]["nombre_propietario"].ToString());
					regexText = new Regex("tmp_tipo_documento_tmp");
					docText = regexText.Replace(docText, dsSource.Tables[indexTable].Rows[indexRow]["tipo_documento"].ToString());
					regexText = new Regex("tmp_numero_documento_tmp");
					docText = regexText.Replace(docText, dsSource.Tables[indexTable].Rows[indexRow]["num_doc_propietario"].ToString());
					regexText = new Regex("tmp_direccion_propietario_tmp");
					docText = regexText.Replace(docText, dsSource.Tables[indexTable].Rows[indexRow]["direccion_propietario"].ToString());

					string tel = dsSource.Tables[indexTable].Rows[indexRow]["telefono_propietario"].ToString();
					regexText = new Regex("tmp_telefono_propietario_tmp");
					if (tel.Length > 0)
						docText = regexText.Replace(docText, "Teléfono: " + dsSource.Tables[indexTable].Rows[indexRow]["telefono_propietario"].ToString());
					else
						docText = regexText.Replace(docText, "");

					regexText = new Regex("tmp_chip_tmp");
					docText = regexText.Replace(docText, dsSource.Tables[indexTable].Rows[indexRow]["chip"].ToString());
					regexText = new Regex("tmp_direccion_tmp");
					docText = regexText.Replace(docText, dsSource.Tables[indexTable].Rows[indexRow]["direccion"].ToString());
					regexText = new Regex("tmp_matricula_tmp");
					docText = regexText.Replace(docText, dsSource.Tables[indexTable].Rows[indexRow]["matricula"].ToString());
					regexText = new Regex("tmp_resolucion_declaratoria_tmp");
					docText = regexText.Replace(docText, dsSource.Tables[indexTable].Rows[indexRow]["resolucion_declaratoria"].ToString());
					regexText = new Regex("tmp_desc_declaratoria_tmp");
					docText = regexText.Replace(docText, dsSource.Tables[indexTable].Rows[indexRow]["desc_declaratoria"].ToString());
					regexText = new Regex("tmp_tipo_declaratoria_tmp");
					docText = regexText.Replace(docText, dsSource.Tables[indexTable].Rows[indexRow]["tipo_declaratoria"].ToString());

					regexText = new Regex("tmp_nombre_usuario_tmp");
					docText = regexText.Replace(docText, oVar.prUserName.ToString());
					regexText = new Regex("tmp_cargo_tmp");
					docText = regexText.Replace(docText, oVar.prUserCargo.ToString());

					using (StreamWriter sw = new StreamWriter(doc.MainDocumentPart.GetStream(FileMode.Create)))
					{
						sw.Write(docText);
					}
				}
				return ReplaceBookmarks(pathDocument);
			}
			catch (Exception MyError)
			{
				return MyError;
			}
		}
		private static Exception ReplaceBookmarks(string wordDocTemplatePath)
		{
			WordprocessingDocument doc = WordprocessingDocument.Open(wordDocTemplatePath, true);
			try
			{
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
