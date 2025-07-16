using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
   public class DOCUMENTOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "DOCUMENTOS_DAL";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private const string TABLA_DOCUMENTOS = "documentos";

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public DOCUMENTOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_documentos_cod_predio(string p_cod_predio_declarado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_declarado", p_cod_predio_declarado);
				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_DOCUMENTOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_i_documento(string p_cod_predio_declarado, string p_tipo_documento, string p_radicado_documento, string p_fecha_radicado_documento,
								string p_folio_inicial_documento, string p_folios_documento, string p_obs_documento, string p_cod_usu_archiva, bool p_is_file)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_declarado", p_cod_predio_declarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_tipo_documento", p_tipo_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_radicado_documento", p_radicado_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_documento", p_fecha_radicado_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_folio_inicial_documento", p_folio_inicial_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_folios_documento", p_folios_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_obs_documento", p_obs_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_archiva", p_cod_usu_archiva);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());
				oDB.MySQLAddParameter(MySqlCmd, "p_is_file", p_is_file);

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}
		public string sp_u_documento(string p_au_documento, string p_numero_carpeta, string p_tipo_documento, string p_radicado_documento,
							string p_fecha_radicado_documento, string p_folio_inicial_documento, string p_folios_documento, string p_obs_documento,
							string p_cod_usu_archiva, bool p_is_file)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_documento", p_au_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_numero_carpeta", p_numero_carpeta);
				oDB.MySQLAddParameter(MySqlCmd, "p_tipo_documento", p_tipo_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_radicado_documento", p_radicado_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_documento", p_fecha_radicado_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_folio_inicial_documento", p_folio_inicial_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_folios_documento", p_folios_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_obs_documento", p_obs_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_archiva", p_cod_usu_archiva);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());
				oDB.MySQLAddParameter(MySqlCmd, "p_is_file", p_is_file);

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}
		public string sp_d_documento(string p_au_documento)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_documento", p_au_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}
		public string sp_u_documentos_reorder(string p_opcion, string p_au_documento, string p_folio_inicial_nuevo)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_opcion", p_opcion);
				oDB.MySQLAddParameter(MySqlCmd, "p_au_documento", p_au_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_folio_inicial_nuevo", p_folio_inicial_nuevo);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_move", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}
		public string sp_v_documento(string p_cod_predio_declarado, string p_au_documento, string p_folio_inicial_documento, string p_folios_documento)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_declarado", p_cod_predio_declarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_au_documento", p_au_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_folio_inicial_documento", p_folio_inicial_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_folios_documento", p_folios_documento);

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}
		public DataSet sp_rpt_gestion_documentos(string p_fecha_ini, string p_fecha_fin, string p_cod_usu)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_ini", p_fecha_ini);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_fin", p_fecha_fin);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", p_cod_usu);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_DOCUMENTOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		
		#region-----DISPOSE
		// Metodo para el manejo del GC
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~DOCUMENTOS_DAL()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

		// Free the instance variables of this object.
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				MySqlConn.Dispose();
				MySqlConn = null;

				MySqlDA.Dispose();
				MySqlDA = null;

				oDataSet.Dispose();
				oDataSet = null;

				oDataTable.Dispose();
				oDataTable = null;
			}
		}
		#endregion
	}
}