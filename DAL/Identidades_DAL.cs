using GLOBAL.LOG;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{

    [Serializable()]

    public class IDENTIDADES_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "IDENTIDADES_DAL";

		private readonly clLog oLog = new clLog();

		#region PARAMETROS
		private const string TABLA_IDENTIDADES = "Identidades";
		private const string PARAM_IDIDENTIDAD = "p_id_identidad";
		private const string PARAM_IDCATEGORIAIDENTIDAD = "p_id_categoria_identidad";
		private const string PARAM_NOMBREIDENTIDAD = "p_nombre_identidad";
		private const string PARAM_DESCRIPCIONIDENTIDAD = "p_descripcion_identidad";
		private const string PARAM_ORDENIDENTIDAD = "p_orden_identidad";
		private const string PARAM_NOMBREIDENTIDAD2 = "p_nombre_identidad2";
		private const string PARAM_HABILITADO = "p_habilitado";

		#endregion

		#region OBJETOS GLOBALES
		[NonSerialized]
		private MySqlConnection MySqlConn;
		[NonSerialized]
		private MySqlDataAdapter MySqlDA;

		private DataTable oDataTable;
		private DataSet oDataSet;

		#endregion

		#region CADENAS DE CONSULTA
		private string spSelectIdentidades = "sp_s_identidades";
		private string spInsertIdentidad = "sp_i_identidad";
		private string spUpdateIdentidad = "sp_u_identidad";
		private string spDeleteIdentidad = "sp_d_identidad";
		#endregion

		public IDENTIDADES_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;

			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();

			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_identidades()
		{
			oDataSet.Clear();
			oDataSet.Reset();

			try
			{
				MySqlConn.Open();

				MySqlCommand MySqlCmd = new MySqlCommand(spSelectIdentidades, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlDA.SelectCommand = MySqlCmd;
				MySqlDA.Fill(oDataSet, TABLA_IDENTIDADES);

				MySqlConn.Close();
				return oDataSet;
			}
			catch (Exception Error)
			{
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_s_identidades");
				MySqlConn.Close();
				return null;
			}
		}
		public DataSet sp_s_identidad_id_categoria(string IdCategoriaIdentidad)
		{
			string sp = "sp_s_identidad_id_categoria";
			oDataSet.Clear();
			oDataSet.Reset();

			try
			{
				MySqlConn.Open();

				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_IDCATEGORIAIDENTIDAD, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_IDCATEGORIAIDENTIDAD].Value = IdCategoriaIdentidad;

				MySqlDA.SelectCommand = MySqlCmd;
				MySqlDA.Fill(oDataSet, TABLA_IDENTIDADES);

				MySqlConn.Close();
				return oDataSet;
			}
			catch (Exception Error)
			{
				oLog.RegistrarLogError(Error, _SOURCEPAGE, sp);
				MySqlConn.Close();
				return null;
			}
		}
		public DataSet sp_s_identidad_id_categoria_op(string p_id_categoria_identidad, string p_opcion_identidad)
		{
			string sp = "sp_s_identidad_id_categoria_op";
			oDataSet.Clear();
			oDataSet.Reset();

			try
			{
				MySqlConn.Open();

				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_categoria_identidad", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_categoria_identidad"].Value = p_id_categoria_identidad;
				MySqlCmd.Parameters.Add(new MySqlParameter("p_opcion_identidad", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_opcion_identidad"].Value = p_opcion_identidad;

				MySqlDA.SelectCommand = MySqlCmd;
				MySqlDA.Fill(oDataSet, TABLA_IDENTIDADES);

				MySqlConn.Close();
				return oDataSet;
			}
			catch (Exception Error)
			{
				oLog.RegistrarLogError(Error, _SOURCEPAGE, sp);
				MySqlConn.Close();
				return null;
			}
		}
		public string sp_i_identidad(string IdCategoriaIdentidad, string NombreIdentidad, string DescripcionIdentidad, string OrdenIdentidad, string NombreIdentidad2, bool Habilitado)
		{
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(spInsertIdentidad, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_IDCATEGORIAIDENTIDAD, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_IDCATEGORIAIDENTIDAD].Value = IdCategoriaIdentidad;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_NOMBREIDENTIDAD, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_NOMBREIDENTIDAD].Value = NombreIdentidad;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_DESCRIPCIONIDENTIDAD, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_DESCRIPCIONIDENTIDAD].Value = DescripcionIdentidad;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ORDENIDENTIDAD, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_ORDENIDENTIDAD].Value = OrdenIdentidad; //Como se agrega este valor

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_NOMBREIDENTIDAD2, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_NOMBREIDENTIDAD2].Value = NombreIdentidad2;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_HABILITADO, MySqlDbType.Bit));
				MySqlCmd.Parameters[PARAM_HABILITADO].Value = Habilitado;

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
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_i_identidad");
				//oVar.prStatusDBAction = -1;
				return "ERR12";
			}

		}
		public string sp_u_identidad(string IdIdentidad, string IdCategoriaIdentidad, string NombreIdentidad, string DescripcionIdentidad, string OrdenIdentidad, string NombreIdentidad2, bool Habilitado)
		{
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(spUpdateIdentidad, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_IDIDENTIDAD, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_IDIDENTIDAD].Value = IdIdentidad;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_IDCATEGORIAIDENTIDAD, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_IDCATEGORIAIDENTIDAD].Value = IdCategoriaIdentidad;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_NOMBREIDENTIDAD, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_NOMBREIDENTIDAD].Value = NombreIdentidad;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ORDENIDENTIDAD, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_ORDENIDENTIDAD].Value = OrdenIdentidad;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_DESCRIPCIONIDENTIDAD, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_DESCRIPCIONIDENTIDAD].Value = DescripcionIdentidad; ;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_NOMBREIDENTIDAD2, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_NOMBREIDENTIDAD2].Value = NombreIdentidad2;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_HABILITADO, MySqlDbType.Bit));
				MySqlCmd.Parameters[PARAM_HABILITADO].Value = Habilitado;

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
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_u_identidad");
				return "ERR12";
			}
		}
		public string sp_d_identidad(string IdIdentidad)
		{
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(spDeleteIdentidad, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_IDIDENTIDAD, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_IDIDENTIDAD].Value = IdIdentidad;

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
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_d_identidad ");
				return "ERR12";
			}
		}

		#region-----DISPOSE
		// Metodo para el manejo del GC
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~IDENTIDADES_DAL()
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