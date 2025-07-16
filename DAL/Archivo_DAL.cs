using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{

    public class ARCHIVO_DAL : IDisposable
    {
        private const string _SOURCEPAGE = "ARCHIVO_DAL";
        private const string TABLA_ARCHIVO = "Archivo";

        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clDB oDB = new clDB();

        private MySqlConnection MySqlConn;
        private MySqlDataAdapter MySqlDA;
        private DataTable oDataTable;
        private DataSet oDataSet;

        public ARCHIVO_DAL()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
            MySqlConn = new MySqlConnection(strConnString);
            MySqlDA = new MySqlDataAdapter();
            oDataTable = new DataTable();
            oDataSet = new DataSet();
        }

        public DataSet sp_s_archivo_listar(string p_filtro, string p_usuario)
        {
            string sp = "sp_s_archivo_listar";
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_filtro", p_filtro);
                oDB.MySQLAddParameter(MySqlCmd, "p_usuario", p_usuario);

                return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_ARCHIVO);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
                return null;
            }
        }

        public DataSet sp_s_archivos_listar_hijos(string p_idarchivo, string p_idtipo_archivo)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_idarchivo", p_idarchivo);
                oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_archivo", p_idtipo_archivo);

                return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_ARCHIVO);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
                return null;
            }
        }

        public DataSet sp_s_archivo_complemento_consultar(string p_id)
        {
            string sp = "sp_s_archivo_complemento_consultar";
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_id", p_id);

                return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_ARCHIVO);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
                return null;
            }
        }

        #region sp_i_archivo
        public string sp_i_archivo(
            int? p_idarchivo_padre,
            int p_idtipo_archivo,
            string p_codigo,
            string p_nombre,
            string p_ruta,
            string p_descripcion = null
        )
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_idarchivo_padre", p_idarchivo_padre.Value == 0 ? null : p_idarchivo_padre);
                oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_archivo", p_idtipo_archivo);
                oDB.MySQLAddParameter(MySqlCmd, "p_codigo", p_codigo);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);
                oDB.MySQLAddParameter(MySqlCmd, "p_ruta", p_ruta);
                oDB.MySQLAddParameter(MySqlCmd, "p_descripcion", p_descripcion);
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
        #endregion

        public string sp_u_archivo_referencia(string p_idarchivo, string p_idreferencia, int p_tipo_referencia)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_idarchivo", p_idarchivo);
                oDB.MySQLAddParameter(MySqlCmd, "p_idreferencia", p_idreferencia);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_referencia", p_tipo_referencia);
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

        #region sp_i_archivo_complemento
        public string sp_i_archivo_complemento(
            int p_idarchivo,
            string p_radicado,
            string p_fecha_radicado,
            string p_fecha_recepcion,
            string p_expediente,
            string p_curaduria,
            string p_fecha_ejecutoria,
            string p_chip,
            string p_mes_reporte,
            string p_tramite,
            string p_areavis,
            string p_numviviendadvis,
            string p_areavip,
            string p_numviviendadvip
        )
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_idarchivo", p_idarchivo);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado", p_radicado);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado", p_fecha_radicado);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_recepcion", p_fecha_recepcion);
                oDB.MySQLAddParameter(MySqlCmd, "p_expediente", p_expediente);
                oDB.MySQLAddParameter(MySqlCmd, "p_curaduria", p_curaduria);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_ejecutoria", p_fecha_ejecutoria);
                oDB.MySQLAddParameter(MySqlCmd, "p_chip", p_chip);
                oDB.MySQLAddParameter(MySqlCmd, "p_mes_reporte", p_mes_reporte);
                oDB.MySQLAddParameter(MySqlCmd, "p_tramite", p_tramite);
                oDB.MySQLAddParameter(MySqlCmd, "p_areavis", p_areavis);
                oDB.MySQLAddParameter(MySqlCmd, "p_numviviendadvis", p_numviviendadvis);
                oDB.MySQLAddParameter(MySqlCmd, "p_areavip", p_areavip);
                oDB.MySQLAddParameter(MySqlCmd, "p_numviviendadvip", p_numviviendadvip);
                oDB.MySQLAddParameter(MySqlCmd, "p_usuario", oVar.prUser.ToString());

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
        #endregion


        #region sp_u_archivo_complemento
        public string sp_u_archivo_complemento(
            int p_idarchivo,
            string p_radicado,
            string p_fecha_radicado,
            string p_fecha_recepcion,
            string p_expediente,
            string p_curaduria,
            string p_fecha_ejecutoria,
            string p_chip,
            string p_mes_reporte,
            string p_tramite,
            string p_areavis,
            string p_numviviendadvis,
            string p_areavip,
            string p_numviviendadvip
        )
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_idarchivo", p_idarchivo);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado", p_radicado);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado", p_fecha_radicado);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_recepcion", p_fecha_recepcion);
                oDB.MySQLAddParameter(MySqlCmd, "p_expediente", p_expediente);
                oDB.MySQLAddParameter(MySqlCmd, "p_curaduria", p_curaduria);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_ejecutoria", p_fecha_ejecutoria);
                oDB.MySQLAddParameter(MySqlCmd, "p_chip", p_chip);
                oDB.MySQLAddParameter(MySqlCmd, "p_mes_reporte", p_mes_reporte);
                oDB.MySQLAddParameter(MySqlCmd, "p_tramite", p_tramite);
                oDB.MySQLAddParameter(MySqlCmd, "p_areavis", p_areavis);
                oDB.MySQLAddParameter(MySqlCmd, "p_numviviendadvis", p_numviviendadvis);
                oDB.MySQLAddParameter(MySqlCmd, "p_areavip", p_areavip);
                oDB.MySQLAddParameter(MySqlCmd, "p_numviviendadvip", p_numviviendadvip);
                oDB.MySQLAddParameter(MySqlCmd, "p_usuario", oVar.prUser.ToString());

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
        #endregion

        #region sp_u_archivo
        public string sp_u_archivo(
            int p_idarchivo,
            int? p_idarchivo_padre,
            int p_idtipo_archivo,
            string p_codigo,
            string p_nombre,
            string p_ruta,
            string p_descripcion = null
        )
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_idarchivo", p_idarchivo);
                oDB.MySQLAddParameter(MySqlCmd, "p_idarchivo_padre", p_idarchivo_padre.Value == 0 ? null : p_idarchivo_padre);
                oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_archivo", p_idtipo_archivo);
                oDB.MySQLAddParameter(MySqlCmd, "p_codigo", p_codigo);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);
                oDB.MySQLAddParameter(MySqlCmd, "p_ruta", p_ruta);
                oDB.MySQLAddParameter(MySqlCmd, "p_descripcion", p_descripcion);
                oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());
                oDB.MySQLAddParameter(MySqlCmd, "p_estado", true);

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
        #endregion

        public string sp_d_archivo(string pId)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "pId", pId);
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

        ~ARCHIVO_DAL()
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