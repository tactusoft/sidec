using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class DISTRIBUCION_VIVIENDA_DAL : IDisposable
    {
        private const string _SOURCEPAGE = "DISTRIBUCION_VIVIENDA_DAL";
        private const string TABLA_DISTRIBUCION_VIVIENDA = "distribucion_vivienda";

        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clDB oDB = new clDB();

        private MySqlConnection MySqlConn;
        private MySqlDataAdapter MySqlDA;
        private DataSet oDataSet;

        public DISTRIBUCION_VIVIENDA_DAL()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
            MySqlConn = new MySqlConnection(strConnString);
            MySqlDA = new MySqlDataAdapter();
            oDataSet = new DataSet();
        }

        // Método para listar distribuciones
        public DataSet sp_s_distribucion_vivienda(string p_idproyecto)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_idproyecto", p_idproyecto);

                return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_DISTRIBUCION_VIVIENDA);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
                return null;
            }
        }

        // Método para insertar una nueva distribución
        public void sp_i_distribucion_vivienda(string p_idproyecto, string delimitedData)
        {
            string sp = "sp_i_distribucion_vivienda";
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_idproyecto", p_idproyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_data", delimitedData);
                oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

                oDB.MySQLExecuteSP(MySqlCmd);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
            }
        }


        // Método para actualizar una distribución existente
        public string sp_u_distribucion_vivienda(string p_iddistribucion, string p_idproyecto, int p_tipo_vivienda, decimal p_area, int p_cantidad, bool p_estado)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_iddistribucion", p_iddistribucion);
                oDB.MySQLAddParameter(MySqlCmd, "p_idproyecto", p_idproyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_vivienda", p_tipo_vivienda);
                oDB.MySQLAddParameter(MySqlCmd, "p_area", p_area);
                oDB.MySQLAddParameter(MySqlCmd, "p_cantidad", p_cantidad);
                oDB.MySQLAddParameter(MySqlCmd, "p_estado", p_estado);
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

        // Método para eliminar una distribución (marcar como inactiva)
        public string sp_d_distribucion_vivienda(string p_iddistribucion)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_iddistribucion", p_iddistribucion);
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        ~DISTRIBUCION_VIVIENDA_DAL()
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
