using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class PROPIETARIOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PROPIETARIOS_DAL";
		private const string TABLA_PROPIETARIOS = "propietarios";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataSet oDataSet;

		public PROPIETARIOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataSet = new DataSet();
		}

		public DataSet sp_s_propietarios_cod_predio_propietario(string p_cod_predio_propietario)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_propietario", p_cod_predio_propietario);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PROPIETARIOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_i_propietario(
			string p_cod_predio_propietario,
			string p_nombre_propietario,
			string p_id_tipo_doc_propietario,
			string p_num_doc_propietario,
			string p_direccion_propietario,
			string p_telefono_propietario,
			string p_correo_propietario,
			string p_nombre_representante,
			string p_id_tipo_doc_representante,
			string p_num_doc_representante,
			string p_direccion_representante,
			string p_telefono_representante,
			string p_correo_representante,
			string p_observacion
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_propietario", p_cod_predio_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_propietario", p_nombre_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_doc_propietario", p_id_tipo_doc_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_num_doc_propietario", p_num_doc_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_direccion_propietario", p_direccion_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_telefono_propietario", p_telefono_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_correo_propietario", p_correo_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_representante", p_nombre_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_doc_representante", p_id_tipo_doc_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_num_doc_representante", p_num_doc_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_direccion_representante", p_direccion_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_telefono_representante", p_telefono_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_correo_representante", p_correo_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_observacion", p_observacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}
		public string sp_u_propietario(
			string p_au_propietario,
			string p_nombre_propietario,
			string p_id_tipo_doc_propietario,
			string p_num_doc_propietario,
			string p_direccion_propietario,
			string p_telefono_propietario,
			string p_correo_propietario,
			string p_nombre_representante,
			string p_id_tipo_doc_representante,
			string p_num_doc_representante,
			string p_direccion_representante,
			string p_telefono_representante,
			string p_correo_representante,
			string p_observacion
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_propietario", p_au_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_propietario", p_nombre_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_doc_propietario", p_id_tipo_doc_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_num_doc_propietario", p_num_doc_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_direccion_propietario", p_direccion_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_telefono_propietario", p_telefono_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_correo_propietario", p_correo_propietario);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_representante", p_nombre_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_doc_representante", p_id_tipo_doc_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_num_doc_representante", p_num_doc_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_direccion_representante", p_direccion_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_telefono_representante", p_telefono_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_correo_representante", p_correo_representante);
				oDB.MySQLAddParameter(MySqlCmd, "p_observacion", p_observacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}
		public string sp_d_propietario(string p_au_propietario)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_propietario", p_au_propietario);
	
				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}

		#region-----DISPOSE
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~PROPIETARIOS_DAL()
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
			}
		}
		#endregion

	}
}