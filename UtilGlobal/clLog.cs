using GLOBAL.UTIL.DAL;
using GLOBAL.VAR;
using System;
using System.IO;

namespace GLOBAL.LOG
{
    [Serializable()]
	public class clLog : IDisposable
	{
		#region VARIALES Y OBJETOS

		private readonly LOG_DAL oLog = new LOG_DAL();
		private readonly clGlobalVar oVar = new clGlobalVar();

		private string _MSGLOGERROR = "{1}\t{2}\t{3}\t{4}{0}{5}\t{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}";
		private string _MSGLOGINFO = "{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}{0}";

		private const string _SEP = "===================================================================================================";

		#endregion

		/// <summary>
		/// Metodo que registra los errores detectados en la ejecución de la aplicación.
		/// </summary>
		/// <param name="strException">Excepcion detectada en la aplicación</param> 
		/// <param name="SourcePage">Pagina desde donde se llama el metodo</param>
		/// <param name="Seccion">Seccion del codigo donde se llama el metodo</param>    
		public void RegistrarLogError(Exception strException, string SourcePage, string Seccion)
		{
			string strMessageLog = string.Format(_MSGLOGERROR, System.Environment.NewLine, System.DateTime.Now.ToString(), oVar.prIP.ToString(), oVar.prPCInfo.ToString(),
			oVar.prUser.ToString(), SourcePage, Seccion, strException.GetBaseException().Source.ToString(), strException.GetType().Name.ToString(),
			strException.GetBaseException().Message.ToString(), strException.GetBaseException().StackTrace.ToString(), _SEP);
			switch (Convert.ToInt16(oVar.prLogErrorDestino.ToString()))
			{
				case 1: //Registrar en Archivo
					fRegistrarFile(strMessageLog, true);
					break;
				case 2: //Registrar en Base de Datos
					fRegistrarErrorBD(strException, SourcePage, Seccion);
					break;
				case 3: //Registrar en Archivo y en Base de Datos
					fRegistrarFile(strMessageLog, true);
					fRegistrarErrorBD(strException, SourcePage, Seccion);
					break;
			}
		}

		public void RegistrarLogError(string CustomError, string SourcePage, string Seccion)
		{
			string strMessageLog = string.Format(
				_MSGLOGERROR,
				System.Environment.NewLine,
				System.DateTime.Now.ToString(),
				oVar.prIP.ToString(),
				oVar.prPCInfo.ToString(),
				oVar.prUser.ToString(),
				SourcePage,
				Seccion,
				CustomError,
				"",
				"",
				"",
				_SEP
			);
			switch (Convert.ToInt16(oVar.prLogErrorDestino.ToString()))
			{
				case 1: //Registrar en Archivo
					fRegistrarFile(strMessageLog, true);
					break;
				case 2: //Registrar en Base de Datos
					fRegistrarErrorBD(CustomError, SourcePage, Seccion);
					break;
				case 3: //Registrar en Archivo y en Base de Datos
					fRegistrarFile(strMessageLog, true);
					fRegistrarErrorBD(CustomError, SourcePage, Seccion);
					break;
			}
		}

		/// <summary>
		/// Metodo para registrar Log de Eventos 
		/// </summary>
		/// <param name="strsource">Pagina donde se genera el Evento.</param>
		/// <param name="strMensaje">Evento que se quiere registrar.</param>
		public void RegistrarLogInfo(string strsource, string strSeccion, string strDescripcion)
		{
			string Matricula = oVar.prUser.ToString();
			if (string.IsNullOrEmpty(Matricula))
				Matricula = "ø";
			string strMessageLog = string.Format(
				_MSGLOGINFO,
				Environment.NewLine,
				DateTime.Now.ToString(),
				oVar.prIP.ToString(),
				oVar.prPCInfo.ToString(),
				Matricula,
				strsource,
				strSeccion,
				strDescripcion
			);
			switch (Convert.ToInt16(oVar.prLogInfoDestino.ToString()))
			{
				case 1: //Registrar en Archivo
					fRegistrarFile(strMessageLog, false);
					break;
				case 2: //Registrar en Base de Datos
					fRegistrarLogInfoBD(strsource, strSeccion, strDescripcion);
					break;
				case 3: //Registrar en Archivo y en Base de Datos
					fRegistrarFile(strMessageLog, false);
					fRegistrarLogInfoBD(strsource, strSeccion, strDescripcion);
					break;
			}
		}

		/// <summary>
		/// Escribir en archivo de Log.
		/// </summary>
		/// <param name="strMessageLog">Mensaje a escribir</param>
		/// <param name="bError">True:Mensaje a Log de Errores. False: Mensaje a LogInfo.</param>
		private void fRegistrarFile(string strMessageLog, bool bError)
		{
			string strLogDestino = oVar.prLogError.ToString();
			if (!bError)
				strLogDestino = oVar.prLogInfo.ToString();
			try
			{
				File.AppendAllText(strLogDestino, strMessageLog);
			}
			catch (Exception MyError)
			{
				MyError.ToString();
			}
		}

		private void fRegistrarErrorBD(Exception strException, string SourcePage, string Seccion)
		{
			int iResult = oLog.InsertLogError(SourcePage, Seccion, strException.GetBaseException().Source.ToString(), strException.GetType().Name.ToString(),
			strException.GetBaseException().Message.ToString(), strException.GetBaseException().StackTrace.ToString());
			if (iResult < 0)
			{
				string strMessageLog = string.Format(_MSGLOGERROR, System.Environment.NewLine, System.DateTime.Now.ToString(), oVar.prIP.ToString(), oVar.prPCInfo.ToString(),
				oVar.prUser.ToString(), SourcePage, Seccion, strException.GetBaseException().Source.ToString(), strException.GetType().Name.ToString(),
				strException.GetBaseException().Message.ToString(), strException.GetBaseException().StackTrace.ToString(), _SEP);
				fRegistrarFile("**Registro de Errores en BD presenta inconvenientes." + System.Environment.NewLine + strMessageLog, true);
			}
		}
		private void fRegistrarErrorBD(string CustomError, string SourcePage, string Seccion)
		{
			int iResult = oLog.InsertLogError(SourcePage, Seccion, CustomError, "", "", "");
			if (iResult < 0)
			{
				string strMessageLog = string.Format(_MSGLOGERROR, System.Environment.NewLine, System.DateTime.Now.ToString(), oVar.prIP.ToString(), oVar.prPCInfo.ToString(),
				oVar.prUser.ToString(), SourcePage, Seccion, CustomError, "", "", "", _SEP);
				fRegistrarFile("**Registro de Errores en BD presenta inconvenientes." + System.Environment.NewLine + strMessageLog, true);
			}
		}
		private void fRegistrarLogInfoBD(string SourcePage, string Seccion, string Descripcion)
		{
			oLog.InsertLogInfo(SourcePage, Seccion, Descripcion);
		}

		#region-----DISPOSE
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~clLog()
		{
			Dispose(false);
		}

		// Free the instance variables of this object.
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				oLog.Dispose();
				oVar.Dispose();
			}
		}
		#endregion
	}
}