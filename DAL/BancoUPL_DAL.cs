
using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
namespace GLOBAL.DAL
{

    public class BancoUPL_DAL : IDisposable
    {
        private const string _SOURCEPAGE = "BancoUPL_DAL";
        private const string TABLA_BancoUPL = "banco_upl";

        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clDB oDB = new clDB();

        private MySqlConnection MySqlConn;
        private MySqlDataAdapter MySqlDA;
        private DataSet oDataSet;

        public BancoUPL_DAL()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
            MySqlConn = new MySqlConnection(strConnString);
            MySqlDA = new MySqlDataAdapter();
            oDataSet = new DataSet();
        }

        public DataSet sp_s_banco_upl(string p_idbanco)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);

                return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_BancoUPL);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
                return null;
            }
        }

        public string sp_iu_banco_upl(int p_idbanco, string p_idupl)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);
                oDB.MySQLAddParameter(MySqlCmd, "p_idupl", p_idupl);
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

        public string sp_d_banco_upl(int p_idbanco)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);
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

        ~BancoUPL_DAL()
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