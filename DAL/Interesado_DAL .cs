using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using SigesTO;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class INTERESADO_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "INTERESADO_DAL";
		private const string TABLA_INTERESADO = "interesado";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataSet oDataSet;

		public INTERESADO_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataSet = new DataSet();
		}

		public DataSet sp_s_interesado(string p_idinteresado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn) { CommandType = CommandType.StoredProcedure };

				oDB.MySQLAddParameter(MySqlCmd, "p_idinteresado", p_idinteresado);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_INTERESADO);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public DataSet sp_s_interesados(string p_idpredio_declarado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn) { CommandType = CommandType.StoredProcedure };

				oDB.MySQLAddParameter(MySqlCmd, "p_idpredio_declarado", p_idpredio_declarado);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_INTERESADO);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public string sp_i_interesado(InteresadoTO pInteresado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn){ CommandType = CommandType.StoredProcedure };

				oDB.MySQLAddParameter(MySqlCmd, "p_idpredio_declarado", pInteresado.IdPredioDeclarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_interesado", pInteresado.IdTipoInteresado);
				oDB.MySQLAddParameter(MySqlCmd, "p_documento", pInteresado.Documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", pInteresado.Nombre);
				oDB.MySQLAddParameter(MySqlCmd, "p_telefono", pInteresado.Telefono);
				oDB.MySQLAddParameter(MySqlCmd, "p_direccion", pInteresado.Direccion);
				oDB.MySQLAddParameter(MySqlCmd, "p_correo", pInteresado.Correo);
				oDB.MySQLAddParameter(MySqlCmd, "p_otro", pInteresado.Otro);
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

		public string sp_u_interesado(InteresadoTO pInteresado )
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn) { CommandType = CommandType.StoredProcedure };

				oDB.MySQLAddParameter(MySqlCmd, "p_idinteresado", pInteresado.IdInteresado);
				oDB.MySQLAddParameter(MySqlCmd, "p_idpredio_declarado", pInteresado.IdPredioDeclarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_interesado", pInteresado.IdTipoInteresado);
				oDB.MySQLAddParameter(MySqlCmd, "p_documento", pInteresado.Documento);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", pInteresado.Nombre);
				oDB.MySQLAddParameter(MySqlCmd, "p_telefono", pInteresado.Telefono);
				oDB.MySQLAddParameter(MySqlCmd, "p_direccion", pInteresado.Direccion);
				oDB.MySQLAddParameter(MySqlCmd, "p_correo", pInteresado.Correo);
				oDB.MySQLAddParameter(MySqlCmd, "p_otro", pInteresado.Otro);
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

		public string sp_d_interesado(string pidInteresado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn) { CommandType = CommandType.StoredProcedure};

				oDB.MySQLAddParameter(MySqlCmd, "p_idinteresado", pidInteresado);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

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
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~INTERESADO_DAL()
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
			}
		}
		#endregion

	}
}