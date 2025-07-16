namespace GLOBAL.DAL
{
    using GLOBAL.DB;
    using GLOBAL.VAR;
    using MySql.Data.MySqlClient;
    using System;
    using System.Configuration;
    using System.Data;

    public class PERSONA_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PERSONA_DAL";
		private const string TABLA_PERSONA = "persona";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataSet oDataSet;

		public PERSONA_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataSet = new DataSet();
		}

		public DataSet sp_s_persona(string p_idpersona)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idpersona", p_idpersona);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PERSONA);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public DataSet sp_s_persona_x_identificacion(
			string p_idtipo_documento,
			string p_documento)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_documento", p_idtipo_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_documento", p_documento);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PERSONA);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public string sp_i_persona(
			string p_idtipo_documento,
			string p_documento,
			string p_nombre,
			string p_direccion,
			string p_telefono,
			string p_correo
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_documento", p_idtipo_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_documento", p_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);
				oDB.MySQLAddParameter(MySqlCmd, "p_direccion", p_direccion);
				oDB.MySQLAddParameter(MySqlCmd, "p_telefono", p_telefono);
				oDB.MySQLAddParameter(MySqlCmd, "p_correo", p_correo);
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

		public string sp_u_persona(
			string p_idpersona,
			string p_idtipo_documento,
			string p_documento,
			string p_nombre,
			string p_direccion,
			string p_telefono,
			string p_correo
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idpersona", p_idpersona);
				oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_documento", p_idtipo_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_documento", p_documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);
				oDB.MySQLAddParameter(MySqlCmd, "p_direccion", p_direccion);
				oDB.MySQLAddParameter(MySqlCmd, "p_telefono", p_telefono);
				oDB.MySQLAddParameter(MySqlCmd, "p_correo", p_correo);
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

		public string sp_d_persona(string p_idpersona)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idpersona", p_idpersona);
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

		#region-----DISPOSE
		// Metodo para el manejo del GC
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~PERSONA_DAL()
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