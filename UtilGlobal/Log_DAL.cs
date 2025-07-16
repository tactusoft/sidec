using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.UTIL.DAL
{

    [Serializable()]
	public class LOG_DAL : IDisposable
	{
		#region Parámetros

		private readonly clGlobalVar oVar = new clGlobalVar();

		[NonSerialized]
		private MySqlConnection MySqlConn;
		[NonSerialized]
		private MySqlDataAdapter MySqlDA;
		private DataSet oDataSet;

		#endregion

		public LOG_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataSet = new DataSet();
		}


		public int InsertLogError(string SourcePage, string Seccion, string BaseExceptionSource, string TypeName, string BaseExceptionMessage, string BaseExceptionStackTrace)
		{
			try
			{
				using (MySqlCommand MySqlCmd = new MySqlCommand("sp_i_logerror", MySqlConn))
				{
					MySqlCmd.CommandType = CommandType.StoredProcedure;

					MySqlCmd.Parameters.AddWithValue("p_IPOrigen", oVar.prIP.ToString());
					MySqlCmd.Parameters.AddWithValue("p_usuario", oVar.prUser.ToString());
					MySqlCmd.Parameters.AddWithValue("p_user_windows", oVar.prPCInfo.ToString());
					MySqlCmd.Parameters.AddWithValue("p_source_page", SourcePage);
					MySqlCmd.Parameters.AddWithValue("p_seccion", Seccion);

					MySqlCmd.Parameters.AddWithValue("p_BaseExceptionSource", BaseExceptionSource);
					MySqlCmd.Parameters.AddWithValue("p_TypeName", TypeName);
					MySqlCmd.Parameters.AddWithValue("p_BaseExceptionMessage", BaseExceptionMessage);
					MySqlCmd.Parameters.AddWithValue("p_BaseExceptionStackTrace", BaseExceptionStackTrace);

					if ((MySqlCmd.Connection == null) || (MySqlCmd.Connection.State != ConnectionState.Open))
						MySqlCmd.Connection.Open();
					MySqlCmd.ExecuteNonQuery();
					MySqlCmd.Connection.Close();
				};
				return 0;
			}
			catch (Exception )
			{
				return -1;
			}
		}

		public int InsertLogInfo(string SourcePage, string Seccion, string Descripcion)
		{
			try
			{
				using (MySqlCommand MySqlCmd = new MySqlCommand("sp_i_loginfo", MySqlConn))
				{
					MySqlCmd.CommandType = CommandType.StoredProcedure;
					MySqlCmd.Parameters.AddWithValue("p_IPOrigen", oVar.prIP.ToString());
					MySqlCmd.Parameters.AddWithValue("p_usuario", oVar.prUser.ToString());
					MySqlCmd.Parameters.AddWithValue("p_user_windows", oVar.prPCInfo.ToString());
					MySqlCmd.Parameters.AddWithValue("p_source_page", SourcePage);
					MySqlCmd.Parameters.AddWithValue("p_seccion", Seccion);

					MySqlCmd.Parameters.AddWithValue("p_descripcion", Descripcion);

					if ((MySqlCmd.Connection == null) || (MySqlCmd.Connection.State != ConnectionState.Open))
						MySqlCmd.Connection.Open();
					MySqlCmd.ExecuteNonQuery();
					MySqlCmd.Connection.Close();
				};
                
				return 0;
			}
			catch (Exception)
			{
				return -1;
            }
		}


		#region-----DISPOSE
		// Metodo para el manejo del GC
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~LOG_DAL()
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
				MySqlDA.Dispose();
				oDataSet.Dispose();

				MySqlConn = null;
				MySqlDA = null;
				oDataSet = null;
			}
		}

		#endregion


	}
}