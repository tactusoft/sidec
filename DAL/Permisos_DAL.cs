using GLOBAL.DB;
using GLOBAL.LOG;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    [Serializable()]
	public class PERMISOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PERMISOS_DAL";
		private const string TABLA_PERMISOS = "permisos";

		private readonly clLog oLog = new clLog();
		[NonSerialized]
		private readonly clDB oDB = new clDB();

		[NonSerialized]
		private MySqlConnection MySqlConn;
		[NonSerialized]
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public PERMISOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_permisos_listar(string p_cod_perfil)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_perfil", p_cod_perfil);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PERMISOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_permisos_cod_usuario(string p_cod_usu)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", p_cod_usu);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, _SOURCEPAGE);
			}
			catch (Exception Error)
			{
				oLog.RegistrarLogError(Error, _SOURCEPAGE, sp);
				MySqlConn.Close();
				return null;
			}
		}
		public string sp_iud_permiso(string p_cod_perfil, string p_tipo_permiso, string p_objeto_permiso, 
								string p_consultar, string p_insertar, string p_modificar, string p_eliminar)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_perfil", p_cod_perfil);
				oDB.MySQLAddParameter(MySqlCmd, "p_tipo_permiso", p_tipo_permiso);
				oDB.MySQLAddParameter(MySqlCmd, "p_objeto_permiso", p_objeto_permiso);
				oDB.MySQLAddParameter(MySqlCmd, "p_consultar", p_consultar);
				oDB.MySQLAddParameter(MySqlCmd, "p_insertar", p_insertar);
				oDB.MySQLAddParameter(MySqlCmd, "p_modificar", p_modificar);
				oDB.MySQLAddParameter(MySqlCmd, "p_eliminar", p_eliminar);

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

		~PERMISOS_DAL()
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