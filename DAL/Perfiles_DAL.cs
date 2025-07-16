using GLOBAL.DB;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    [Serializable()]
	public class PERFILES_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PERFILES_DAL";
		private const string TABLA_PERFILES = "Perfiles";

		[NonSerialized]
		private readonly clDB oDB = new clDB();

		[NonSerialized]
		private MySqlConnection MySqlConn;
		[NonSerialized]
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public PERFILES_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;

			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_perfiles(string p_nombre = "")
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};

				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PERFILES);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_perfil(string p_cod_perfil)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_perfil", p_cod_perfil);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PERFILES);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_i_perfil(string p_nombre_perfil, string p_desc_perfil)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_perfil", p_nombre_perfil);
				oDB.MySQLAddParameter(MySqlCmd, "p_desc_perfil", p_desc_perfil);

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}
		public string sp_u_perfil(string p_cod_perfil, string p_nombre_perfil, string p_desc_perfil)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_perfil", p_cod_perfil);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_perfil", p_nombre_perfil);
				oDB.MySQLAddParameter(MySqlCmd, "p_desc_perfil", p_desc_perfil);

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}
		public string sp_d_perfil(string p_cod_perfil)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_perfil", p_cod_perfil);

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_result"].Value.ToString();
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

		~PERFILES_DAL()
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

				oDataTable.Dispose();
				oDataTable = null;
			}
		}
		#endregion
	}
}