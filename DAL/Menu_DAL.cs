using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class MENUGLOBAL_DAL : IDisposable
	{
		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;

		private readonly string spSelectMenu = "sp_s_menu";
		
		public MENUGLOBAL_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
		}

		public DataTable sp_s_menu()
		{
			MySqlDA = new MySqlDataAdapter(spSelectMenu, MySqlConn);
			MySqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
			MySqlDA.Fill(oDataTable);
			return oDataTable;
		}

		#region-----DISPOSE
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~MENUGLOBAL_DAL()
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

				oDataTable.Dispose();
				oDataTable = null;
			}
		}
		#endregion

	}
}