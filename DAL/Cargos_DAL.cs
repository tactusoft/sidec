using GLOBAL.LOG;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class CARGOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "CARGOS_DAL";

		private readonly clLog oLog = new clLog();

		#region PARAMETROS
		private const string TABLA_CARGOS = "cargos";

		private const string PARAM_AUCARGO = "p_au_cargo";
		private const string PARAM_NOMBRECARGO = "p_nombre_cargo";
		#endregion

		#region OBJETOS GLOBALES

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;

		private DataTable oDataTable;
		private DataSet oDataSet;

		#endregion

		#region CADENAS DE CONSULTA
		private string spSelectCargos = "sp_s_cargos";
		private string spInsertCargo = "sp_i_cargo";
		private string spDeleteCargo = "sp_d_cargo";
		private string spUpdateCargo = "sp_u_cargo";

		#endregion

		public CARGOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;

			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();

			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_cargos()
		{
			oDataSet.Clear();
			oDataSet.Reset();

			try
			{
				MySqlConn.Open();

				MySqlCommand MySqlCmd = new MySqlCommand(spSelectCargos, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlDA.SelectCommand = MySqlCmd;
				MySqlDA.Fill(oDataSet, TABLA_CARGOS);

				MySqlConn.Close();
				return oDataSet;
			}
			catch (Exception Error)
			{
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_s_cargos");
				MySqlConn.Close();
				return null;
			}
		}
		public string sp_i_cargo(string NombreCargo)
		{
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(spInsertCargo, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_NOMBRECARGO, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_NOMBRECARGO].Value = NombreCargo;

				MySqlParameter MySqlParam = MySqlCmd.Parameters.Add(new MySqlParameter("p_Result", MySqlDbType.VarChar));
				MySqlParam.Direction = ParameterDirection.Output;

				MySqlDA.InsertCommand = MySqlCmd;
				MySqlDA.InsertCommand.Connection.Open();
				MySqlDA.InsertCommand.ExecuteNonQuery();
				MySqlDA.InsertCommand.Connection.Close();

				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				MySqlDA.InsertCommand.Connection.Close();
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_i_cargo");
				return "ERR12";
			}
		}
		public string sp_u_cargo(string AuCargo, string NombreCargo)
		{
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(spUpdateCargo, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_AUCARGO, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_AUCARGO].Value = AuCargo;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_NOMBRECARGO, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_NOMBRECARGO].Value = NombreCargo;

				MySqlParameter MySqlParam = MySqlCmd.Parameters.Add(new MySqlParameter("p_Result", MySqlDbType.VarChar));
				MySqlParam.Direction = ParameterDirection.Output;

				MySqlDA.UpdateCommand = MySqlCmd;
				MySqlDA.UpdateCommand.Connection.Open();
				MySqlDA.UpdateCommand.ExecuteNonQuery();
				MySqlDA.UpdateCommand.Connection.Close();

				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				MySqlDA.UpdateCommand.Connection.Close();
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_u_cargo");
				return "ERR12";
			}
		}
		public string sp_d_cargo(string AuCargo)
		{
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(spDeleteCargo, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_AUCARGO, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_AUCARGO].Value = AuCargo;

				MySqlParameter MySqlParam = MySqlCmd.Parameters.Add(new MySqlParameter("p_Result", MySqlDbType.VarChar));
				MySqlParam.Direction = ParameterDirection.Output;

				MySqlDA.UpdateCommand = MySqlCmd;
				MySqlDA.UpdateCommand.Connection.Open();
				MySqlDA.UpdateCommand.ExecuteNonQuery();
				MySqlDA.UpdateCommand.Connection.Close();

				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				MySqlDA.UpdateCommand.Connection.Close();
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_d_cargo");
				return "ERR12";
			}
		}


		#region-----DISPOSE
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~CARGOS_DAL()
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