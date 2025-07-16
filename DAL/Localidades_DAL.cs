using GLOBAL.LOG;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class LOCALIDADES_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "LOCALIDADES_DAL";

		private readonly clLog oLog = new clLog();

		#region PARAMETROS
		private const string TABLA_LOCALIDADES = "localidades";
		#endregion

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;
		
		private string spSelectLocalidades = "sp_s_localidades";
		
		public LOCALIDADES_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_localidades()
		{
			oDataSet.Clear();
			oDataSet.Reset();
			try
			{
				MySqlConn.Open();
				MySqlCommand MySqlCmd = new MySqlCommand(spSelectLocalidades, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;
				MySqlDA.SelectCommand = MySqlCmd;
				MySqlDA.Fill(oDataSet, TABLA_LOCALIDADES);
				MySqlConn.Close();
				return oDataSet;
			}
			catch (Exception Error)
			{
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_s_localidades");
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

		~LOCALIDADES_DAL()
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