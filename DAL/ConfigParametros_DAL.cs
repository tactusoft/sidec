using GLOBAL.LOG;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class CONFIGPARAMETROS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "CONFIGPARAMETROS_DAL";

		private readonly clLog oLog = new clLog();

		#region PARAMETROS
		private const string TABLA_CONFIGPARAMETROS = "config_parametros";
		#endregion

		#region OBJETOS GLOBALES
		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;
		#endregion

		public CONFIGPARAMETROS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;

			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();

			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_config_parametros()
		{
			try
			{
				MySqlConn.Open();
				MySqlCommand MySqlCmd = new MySqlCommand("sp_s_config_parametros", MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;
				MySqlDA.SelectCommand = MySqlCmd;
				MySqlDA.Fill(oDataSet, TABLA_CONFIGPARAMETROS);
				MySqlConn.Close();
				return oDataSet;
			}
			catch (Exception Error)
			{
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_s_config_parametros");
				MySqlConn.Close();
				return null;
			}
		}


		#region-----DISPOSE
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~CONFIGPARAMETROS_DAL()
		{
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