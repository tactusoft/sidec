using GLOBAL.LOG;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class BARRIOS_DAL : IDisposable
	{

		private const string _SOURCEPAGE = "BARRIOS_DAL";

		private readonly clLog oLog = new clLog();

		#region PARAMETROS
		private const string TABLA_BARRIOS = "barrios";

		#endregion

		#region OBJETOS GLOBALES

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;

		private DataTable oDataTable;
		private DataSet oDataSet;

		#endregion

		#region CADENAS DE CONSULTA
		//Cadenas de Consulta: Procedimientos almacenados
		private string spSelectBarriosInfo = "sp_s_barrios_info";
		#endregion

		public BARRIOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;

			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();

			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_barrios_info()
		{
			oDataSet.Clear();
			oDataSet.Reset();

			try
			{
				MySqlConn.Open();

				MySqlCommand MySqlCmd = new MySqlCommand(spSelectBarriosInfo, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlDA.SelectCommand = MySqlCmd;
				MySqlDA.Fill(oDataSet, TABLA_BARRIOS);

				MySqlConn.Close();
				return oDataSet;
			}
			catch (Exception Error)
			{
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_s_barrios_info");
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

		~BARRIOS_DAL()
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
