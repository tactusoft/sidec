using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class PROYECTOSACTOR_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PROYECTOSACTOR_DAL";
		private const string TABLA_PROYECTOSACTOR = "proyectosactor";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataSet oDataSet;

		public PROYECTOSACTOR_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_proyectos_actores(string p_idproyecto)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idproyecto", p_idproyecto);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PROYECTOSACTOR);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_i_proyecto_actor(
			string p_idproyecto,
			string p_idtipo_actor,
			string p_idpersona,
			string p_idpersona_representante
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idproyecto", p_idproyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_actor", p_idtipo_actor);
				oDB.MySQLAddParameter(MySqlCmd, "p_idpersona", p_idpersona);
				oDB.MySQLAddParameter(MySqlCmd, "p_idpersona_representante", p_idpersona_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_usuario", oVar.prUserCod.ToString());

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
		public string sp_u_proyecto_actor(
			string p_idproyecto_actor,
			string p_idproyecto,
			string p_idtipo_actor,
			string p_idpersona,
			string p_idpersona_representante
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idproyecto_actor", p_idproyecto_actor);
				oDB.MySQLAddParameter(MySqlCmd, "p_idproyecto", p_idproyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_actor", p_idtipo_actor);
				oDB.MySQLAddParameter(MySqlCmd, "p_idpersona", p_idpersona);
				oDB.MySQLAddParameter(MySqlCmd, "p_idpersona_representante", p_idpersona_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_usuario", oVar.prUserCod.ToString());

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
		public string sp_d_proyecto_actor(string p_idproyecto_actor)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idproyecto_actor", p_idproyecto_actor);

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

		#region-----DISPOSE
		// Metodo para el manejo del GC
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~PROYECTOSACTOR_DAL()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

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
			}
		}
		#endregion

	}
}