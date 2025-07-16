using GLOBAL.LOG;
using GLOBAL.VAR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace GLOBAL.UTIL
{
    public class clFile :IDisposable
	{
		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clLog oLog = new clLog();

		string _PATHDOC = "{0}//{1}";     // Ubicación / CodPredioDeclarado
		string _PATHDOCFULL = "{0}//{1}.pdf";      // _PATHDOC / FolioInicial
		string _PATHDECLARATORIA = "{0}//{1}_{2}";     // Ubicación / Resolución _ AñoResolucion(YY)
		string _PATHRESOLUCION = "{0}//{1}_{2}";     // Ubicación / Año(YYYY) _ Numero
		string _PATHDECLARATORIAFULL = "{0}.pdf";      // _PATHDOC 
		string _PATHPDF = "{0}.pdf";      // _PATHDOC 

		public bool fBorrarFile(string pathFile)
		{
			if (File.Exists(pathFile))
				try
				{
					File.Delete(pathFile);
					return true;
				}
				catch (Exception MyErr)
				{
					oLog.RegistrarLogError(MyErr, "clFile", "fBorrarFile");
				}
			return false;
		}
		public bool fCopiarFile(string pathFileOrigen, string pathFileDestino)
		{
			try
			{
				if (File.Exists(pathFileDestino))
				{
					File.Delete(pathFileDestino);
					File.Copy(pathFileOrigen, pathFileDestino);
				}
				else
					File.Copy(pathFileOrigen, pathFileDestino);

				return true;
			}
			catch (Exception MyErr)
			{
				oLog.RegistrarLogError(MyErr, "clFile", "fCopiarFile");
				return false;
			}
		}
		public string[] fGetArchivoFotos(string AuVisita)
		{
			string pathFotos = oVar.prPathPrediosVisitas.ToString() + "//" + AuVisita;
			if (Directory.Exists(pathFotos))
				return Directory.GetFiles(pathFotos, "*.jpg");
			else
				return null;
		}
		public string[] fGetArchivoPrediosVisitasFotos(string PrediosVisitasAu)
		{
			string path = oVar.prPathPrediosVisitas.ToString() + PrediosVisitasAu;
			if (Directory.Exists(path))
			{
				var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).Where(s => (s.ToLower().Contains("\\" + PrediosVisitasAu + "_")) && (s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg") || s.ToLower().EndsWith(".png")));
				return files.ToArray();
			}
			else
				return null;
		}
		public string[] fGetArchivoPlanesPVisitasFotos(string PlanesPVisitasAu)
		{
			string path = oVar.prPathPlanesPVisitas.ToString() + PlanesPVisitasAu;
			if (Directory.Exists(path))
			{
				var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).Where(s => (s.ToLower().Contains("\\" + PlanesPVisitasAu + "_")) && (s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg") || s.ToLower().EndsWith(".png")));
				return files.ToArray();
			}
			else
				return null;
		}
		public string[] fGetArchivoPlanesPFotos(string PlanesPAu)
		{
			string path = oVar.prPathPlanesP.ToString() + PlanesPAu;
			if (Directory.Exists(path))
			{
				var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).Where(s => (s.ToLower().Contains(PlanesPAu + "\\e") || s.ToLower().Contains(PlanesPAu + "\\u")) && (s.ToLower().EndsWith("s") || s.ToLower().EndsWith(".jpeg") || s.ToLower().EndsWith(".png")));
				return files.ToArray();
			}
			else
				return null;
		}
		public string[] fGetFileProp(string pathFuente)
		{
			System.Drawing.Imaging.PropertyItem propItem;

			string[] infoProp = new string[2];
			infoProp[0] = "";
			infoProp[1] = "";
			try
			{
				System.Drawing.Image imageSource = System.Drawing.Image.FromFile(pathFuente);

				if (imageSource.PropertyIdList.Contains(0x9286))  // PropertyTagExifUserComment
				{
					propItem = imageSource.GetPropertyItem(0x9286);
					infoProp[0] = System.Text.Encoding.Default.GetString(propItem.Value);

					if (infoProp[0] != null && infoProp[0].Replace("\0", "").Length < 50)
						infoProp[0] = System.Text.Encoding.Default.GetString(propItem.Value);
					else
						infoProp[0] = "";
				}


				if (imageSource.PropertyIdList.Contains(0x927C))  // PropertyTagExifMakerNote
				{
					propItem = imageSource.GetPropertyItem(0x927C);
					infoProp[1] = System.Text.Encoding.Default.GetString(propItem.Value);
					if (infoProp[1] != null && infoProp[1].Replace("\0", "").Length < 50)
						infoProp[1] = System.Text.Encoding.Default.GetString(propItem.Value);
					else
						infoProp[1] = "";
				}

				imageSource.Dispose();
				return infoProp;
			}
			catch (Exception MyErr)
			{
				oLog.RegistrarLogError(MyErr, "clFile", "fGetFileProp");
				return null;
			}
		}
		public List<clInfoFoto> fGetFilesProp()
		{
			string[] fotos = fGetArchivoFotos(oVar.prVisitaAu.ToString());

			if (fotos != null)
			{
				string[] infoProp = new string[2];
				var infoFoto = new List<clInfoFoto>();

				foreach (string foto in fotos)
				{
					infoProp = fGetFileProp(foto);
					infoFoto.Add(new clInfoFoto
					{
						path = foto,
						tipo = infoProp[0].ToString().Replace("\0", ""),
						observacion = infoProp[1].ToString().Replace("\0", ""),
					});
				}
				return infoFoto;
			}

			return null;
		}
		public string GetPath(string path_file)
		{
			//return path_file;
			oVar.prFullPathDoc = string.Format(path_file);
			return oVar.prFullPathDoc.ToString();
		}
		public string GetFullPathDoc(string cod_predio_declarado, string Archivo)
		{
			string PathDoc = string.Format(_PATHDOC, oVar.prPathDocumentos.ToString(), cod_predio_declarado);
			oVar.prFullPathDoc = string.Format(_PATHDOCFULL, PathDoc, Archivo);
			return oVar.prFullPathDoc.ToString();
		}
		public string GetFullPathDeclaratoria(string Resolucion, string FechaResolucion)
		{
			string _Resolucion = Resolucion.Replace(" ", string.Empty);
			string _AñoResolucion = Convert.ToDateTime(FechaResolucion).Year.ToString();

			string PathDoc = string.Format(_PATHDECLARATORIA, oVar.prPathDeclaratorias.ToString(), _Resolucion, _AñoResolucion);
			oVar.prFullPathDoc = string.Format(_PATHDECLARATORIAFULL, PathDoc);
			return oVar.prFullPathDoc.ToString();
		}
		public string GetPathResoluciones(string numero_acto, string fecha)
		{
			string _NumeroActo = "";
			string _Ano = "";
			try
			{
				_NumeroActo = Convert.ToInt32(numero_acto).ToString();
				_Ano = Convert.ToDateTime(fecha).Year.ToString();
			}
			catch
			{
				_NumeroActo = numero_acto.Substring(0, numero_acto.IndexOf("/"));
				_Ano = numero_acto.Substring(numero_acto.IndexOf("/") + 1, 4);
			}


			string PathDoc = string.Format(_PATHRESOLUCION, oVar.prPathResoluciones.ToString(), _Ano, _NumeroActo);
			oVar.prFullPathDoc = string.Format(_PATHPDF, PathDoc);
			return oVar.prFullPathDoc.ToString();
		}
		private void SaveResizeImage(Bitmap image, int maxWidth, int maxHeight, int quality, string filePath)
		{
			int originalWidth = image.Width;
			int originalHeight = image.Height;

			// To preserve the aspect ratio
			float ratioX = (float)maxWidth / (float)originalWidth;
			float ratioY = (float)maxHeight / (float)originalHeight;
			float ratio = Math.Min(ratioX, ratioY);

			// New width and height based on aspect ratio
			int newWidth = (int)(originalWidth * ratio);
			int newHeight = (int)(originalHeight * ratio);

			// Convert other formats (including CMYK) to RGB.
			Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

			// Draws the image in the specified size with quality mode set to HighQuality
			using (Graphics graphics = Graphics.FromImage(newImage))
			{
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.DrawImage(image, 0, 0, newWidth, newHeight);
			}

			// Get an ImageCodecInfo object that represents the JPEG codec.
			ImageCodecInfo imageCodecInfo = this.GetEncoderInfo(ImageFormat.Jpeg);

			// Create an Encoder object for the Quality parameter.
			System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;

			// Create an EncoderParameters object. 
			EncoderParameters encoderParameters = new EncoderParameters(1);

			// Save the image as a JPEG file with quality level.
			EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
			encoderParameters.Param[0] = encoderParameter;

			newImage.Save(filePath, imageCodecInfo, encoderParameters);
		}
		private ImageCodecInfo GetEncoderInfo(ImageFormat format)
		{
			return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
		}
		public bool fSetFileProp1(string pathFile, string pathFileTmp, string propTipoFoto, string propDescripcion)
		{
			try
			{
				var propItem = (System.Drawing.Imaging.PropertyItem)FormatterServices.GetUninitializedObject(typeof(System.Drawing.Imaging.PropertyItem));
				using (var file = System.Drawing.Image.FromFile(pathFileTmp))
				{
					propItem.Id = 0x9286; // PropertyTagExifUserComment
					propItem.Type = 2;
					propItem.Value = System.Text.Encoding.Default.GetBytes(propTipoFoto + "\0");
					propItem.Len = propItem.Value.Length;
					file.SetPropertyItem(propItem);

					propItem.Id = 0x927C; // PropertyTagExifMakerNote
					propItem.Type = 2;
					propItem.Value = System.Text.Encoding.Default.GetBytes(propDescripcion + "\0");
					propItem.Len = propItem.Value.Length;
					file.SetPropertyItem(propItem);
					file.Save(pathFile);
				}
				return true;
			}
			catch (Exception MyErr)
			{
				oLog.RegistrarLogError(MyErr, "clFile", "fSetFileProp1");
				return false;
			}
		}
		public bool fSetFileProp(Bitmap fotoSeleccionada, string pathFileOrigen, string pathFileDestino, string propTipoFoto, string propDescripcion)
		{
			try
			{
				SaveResizeImage(fotoSeleccionada, 1080, 1080, 70, pathFileOrigen);

				var propItem = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(System.Drawing.Imaging.PropertyItem));

				using (var file = Image.FromFile(pathFileOrigen))
				{
					propItem.Id = 0x9286; // PropertyTagExifUserComment
					propItem.Type = 2;
					propItem.Value = System.Text.Encoding.Default.GetBytes(propTipoFoto + "\0");
					propItem.Len = propItem.Value.Length;
					file.SetPropertyItem(propItem);

					propItem.Id = 0x927C; // PropertyTagExifMakerNote
					propItem.Type = 2;
					propItem.Value = System.Text.Encoding.Default.GetBytes(propDescripcion + "\0");
					propItem.Len = propItem.Value.Length;
					file.SetPropertyItem(propItem);

					file.Save(pathFileDestino);
				}
				fBorrarFile(pathFileOrigen);
				return true;
			}
			catch (Exception MyErr)
			{
				fBorrarFile(pathFileOrigen);
				oLog.RegistrarLogError(MyErr, "clFile", "fSetFileProp");
				return false;
			}
		}
		public bool fVerificarPath(string path)
		{
			try
			{
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
				return true;
			}
			catch (Exception MyError)
			{
				oLog.RegistrarLogError(MyError, "GLOBAL.UTIL", "VerificarPath: " + path);
				return false;
			}
		}
		public bool fVerificarFile(string filepath)
		{
			if (File.Exists(filepath))
				return true;
			else
				return false;
		}
		public bool fVerificarFileSinExtension(string path, string fileName)
		{
			if (fVerificarPath(path))
			{
				var files = Directory.GetFiles(path, fileName + ".*");
				if (files.Length > 0)
				{
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}

        public void Dispose()
        {
			GC.SuppressFinalize(this);
		}
    }

	public class clInfoFoto
	{
		public string path { get; set; }
		public string tipo { get; set; }
		public string observacion { get; set; }
	}
}