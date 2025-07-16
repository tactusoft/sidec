using GLOBAL.LOG;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class IDENTIDADESCATEGORIAS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "IDENTIDADESCATEGORIAS_DAL";

		private readonly clLog oLog = new clLog();

		#region PARAMETROS
		private const string TABLA_IDENTIDADESCATEGORIAS = "identidades_categorias";
		private const string PARAM_IDCATEGORIAIDENTIDAD = "p_id_categoria_identidad";
		private const string PARAM_CATEGORIAIDENTIDAD = "p_categoria_identidad";
		private const string PARAM_DESCRIPCION = "p_descripcion_categoria_identidad";
		#endregion

		#region OBJETOS GLOBALES
		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;

		private DataTable oDataTable;
		private DataSet oDataSet;
		#endregion

		#region CADENAS DE CONSULTA
		private string spSelectIdentidadesCategorias = "sp_s_identidades_categorias";
		private string spInsertIdentidadCategorias = "sp_i_identidad_categoria";
		private string spUpdateIdentidadCategorias = "sp_u_identidad_categoria";
		#endregion

		public IDENTIDADESCATEGORIAS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;

			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();

			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_IdentidadesCategorias()
		{
			oDataSet.Clear();
			oDataSet.Reset();
			try
			{
				MySqlConn.Open();

				MySqlCommand MySqlCmd = new MySqlCommand(spSelectIdentidadesCategorias, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlDA.SelectCommand = MySqlCmd;
				MySqlDA.Fill(oDataSet, TABLA_IDENTIDADESCATEGORIAS);

				MySqlConn.Close();
				return oDataSet;
			}
			catch (Exception Error)
			{
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_s_IdentidadesCategorias");
				MySqlConn.Close();
				return null;
			}
		}
		public string sp_i_identidad_categoria(string CategoriaIdentidad, string DescripcionCategoriaIdentidad)
		{
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(spInsertIdentidadCategorias, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_CATEGORIAIDENTIDAD, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_CATEGORIAIDENTIDAD].Value = CategoriaIdentidad;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_DESCRIPCION, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_DESCRIPCION].Value = DescripcionCategoriaIdentidad;

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
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_i_identidad_categoria");
				return "ERR12";
			}
		}
		public string sp_u_identidad_categoria(string IdCategoriaIdentidad, string CategoriaIdentidad, string DescripcionCategoriaIdentidad)
		{
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(spUpdateIdentidadCategorias, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_IDCATEGORIAIDENTIDAD, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_IDCATEGORIAIDENTIDAD].Value = IdCategoriaIdentidad;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_CATEGORIAIDENTIDAD, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_CATEGORIAIDENTIDAD].Value = CategoriaIdentidad;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_DESCRIPCION, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_DESCRIPCION].Value = DescripcionCategoriaIdentidad; ;

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
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_u_identidad_categoria");
				return "ERR12";
			}
		}

		#region-----DISPOSE
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~IDENTIDADESCATEGORIAS_DAL()
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