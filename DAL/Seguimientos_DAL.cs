using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class Seguimientos_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "Seguimientos_DAL";
		private const string TABLA_SEGUIMIENTOS = "Seguimientos";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public Seguimientos_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}

		public DataSet sp_s_seguimientos(string p_idbanco, string p_idproyecto, string p_fecha)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);
				oDB.MySQLAddParameter(MySqlCmd, "p_idproyecto", p_idproyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha", p_fecha);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_SEGUIMIENTOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public DataSet sp_s_seguimiento_consultar(string p_idseguimiento)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idseguimiento", p_idseguimiento);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_SEGUIMIENTOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public string sp_i_seguimiento(string p_idbanco, string p_idbanco_actividad, string p_fec_seguimiento, string p_asunto, string p_idtipo_actividad, 
						string p_gestion, string p_compromisos)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);
				oDB.MySQLAddParameter(MySqlCmd, "p_idbanco_actividad", p_idbanco_actividad);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_seguimiento", p_fec_seguimiento);
				oDB.MySQLAddParameter(MySqlCmd, "p_asunto", p_asunto);
				oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_actividad", p_idtipo_actividad);

				oDB.MySQLAddParameter(MySqlCmd, "p_gestion", p_gestion);
				oDB.MySQLAddParameter(MySqlCmd, "p_compromisos", p_compromisos);
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

		public string sp_u_seguimiento(string p_idseguimiento, string p_idbanco_actividad, string p_fec_seguimiento, string p_asunto, string p_idtipo_actividad,
						string p_gestion, string p_compromisos)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idseguimiento", p_idseguimiento);
				oDB.MySQLAddParameter(MySqlCmd, "p_idbanco_actividad", p_idbanco_actividad);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_seguimiento", p_fec_seguimiento);
				oDB.MySQLAddParameter(MySqlCmd, "p_asunto", p_asunto);
				oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_actividad", p_idtipo_actividad);

				oDB.MySQLAddParameter(MySqlCmd, "p_gestion", p_gestion);
				oDB.MySQLAddParameter(MySqlCmd, "p_compromisos", p_compromisos);
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

		public string sp_d_seguimiento(string p_idseguimiento)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idseguimiento", p_idseguimiento);
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

		public DataSet sp_s_seguimiento_reporte(string p_idbanco)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_SEGUIMIENTOS);
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

		~Seguimientos_DAL()
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