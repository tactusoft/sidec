namespace GLOBAL.DAL
{
    using GLOBAL.DB;
    using GLOBAL.VAR;
    using MySql.Data.MySqlClient;
    using System;
    using System.Configuration;
    using System.Data;

    public class ActividadAcciones_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "ActividadAcciones_DAL";
		private const string TABLA_ACTIVIDAD_ACCIONES = "ActividadAcciones";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;

		public ActividadAcciones_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
		}

		public DataSet sp_s_actividadacciones_listar(string p_idactividad)
        {
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

				oDB.MySQLAddParameter(MySqlCmd, "p_idactividad", p_idactividad);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_ACTIVIDAD_ACCIONES);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public string sp_i_actividadacciones(string p_idactividad, string p_acciones)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idactividad", p_idactividad);
				oDB.MySQLAddParameter(MySqlCmd, "p_acciones", p_acciones);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public string sp_u_actividadacciones(string p_idaccion, string p_acciones)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};

				oDB.MySQLAddParameter(MySqlCmd, "p_idaccion", p_idaccion);
				oDB.MySQLAddParameter(MySqlCmd, "p_acciones", p_acciones);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

        public string sp_d_actividadacciones(string p_idaccion)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idaccion", p_idaccion);
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

        #region-----DISPOSE
        // Metodo para el manejo del GC
        public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~ActividadAcciones_DAL()
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
				MySqlConn = null;
			}
		}
		#endregion

	}
}