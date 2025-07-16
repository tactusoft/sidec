using GLOBAL.DB;
using GLOBAL.LOG;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{

    public class PLANESPLICENCIAS_DAL : IDisposable
    {
        private const string _SOURCEPAGE = "PLANESPLICENCIAS_DAL";
        private const string TABLA_PLANESPLICENCIAS = "PlanesPLicencias";
        
        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clDB oDB = new clDB();

        private MySqlConnection MySqlConn;
        private MySqlDataAdapter MySqlDA;
        private DataTable oDataTable;
        private DataSet oDataSet;

        public PLANESPLICENCIAS_DAL()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
            MySqlConn = new MySqlConnection(strConnString);
            MySqlDA = new MySqlDataAdapter();
            oDataTable = new DataTable();
            oDataSet = new DataSet();
        }

        public DataSet sp_s_planesp_licencias_cod_planp(string p_cod_planp)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_cod_planp", p_cod_planp);

                return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PLANESPLICENCIAS);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
                return null;
            }
        }

        public string sp_i_planp_licencia(
            string p_cod_planp,
            string p_id_fuente_informacion,
            string p_id_tipo_licencia,
            string p_numero_licencia,
            string p_curador,
            string p_fecha_ejecutoria,
            string p_termino_vigencia_meses,
            string p_plano_urbanistico_aprobado,
            string p_nombre_proyecto,
            string p_area_bruta,
            string p_area_neta,
            string p_area_util,
            string p_area_cesion_zonas_verdes,
            string p_area_cesion_vias,
            string p_area_cesion_eq_comunal,
            string p_porc_ejecucion_urbanismo,
            string p_id_obligacion_VIS,
            string p_id_obligacion_VIP,
            string p_area_terreno_VIS,
            string p_area_terreno_no_VIS,
            string p_area_terreno_VIP,
            string p_area_construida_VIS,
            string p_area_construida_no_VIS,
            string p_area_construida_VIP,
            string p_porc_obligacion_VIS,
            string p_porc_obligacion_VIP,
            string p_unidades_vivienda_VIS,
            string p_unidades_vivienda_no_VIS,
            string p_unidades_vivienda_VIP,
            string p_area_comercio,
            string p_area_oficina,
            string p_area_institucional,
            string p_area_industria,
            string p_area_lote,
            string p_area_sotano,
            string p_area_semisotano,
            string p_area_primer_piso,
            string p_area_pisos_restantes,
            string p_area_construida_total,
            string p_area_libre_primer_piso,
            string p_porc_ejecucion_construccion,
            bool p_cumple_area_obligacion,
            bool p_cumple_porc_area_util,
            string p_observacion
        )
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_cod_planp", p_cod_planp);
                oDB.MySQLAddParameter(MySqlCmd, "p_id_fuente_informacion", p_id_fuente_informacion);
                oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_licencia", p_id_tipo_licencia);
                oDB.MySQLAddParameter(MySqlCmd, "p_numero_licencia", p_numero_licencia);
                oDB.MySQLAddParameter(MySqlCmd, "p_curador", p_curador);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_ejecutoria", p_fecha_ejecutoria);
                oDB.MySQLAddParameter(MySqlCmd, "p_termino_vigencia_meses", p_termino_vigencia_meses);
                oDB.MySQLAddParameter(MySqlCmd, "p_plano_urbanistico_aprobado", p_plano_urbanistico_aprobado);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre_proyecto", p_nombre_proyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_bruta", p_area_bruta);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_neta", p_area_neta);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_util", p_area_util);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_cesion_zonas_verdes", p_area_cesion_zonas_verdes);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_cesion_vias", p_area_cesion_vias);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_cesion_eq_comunal", p_area_cesion_eq_comunal);
                oDB.MySQLAddParameter(MySqlCmd, "p_porc_ejecucion_urbanismo", p_porc_ejecucion_urbanismo);
                oDB.MySQLAddParameter(MySqlCmd, "p_id_obligacion_VIS", p_id_obligacion_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_id_obligacion_VIP", p_id_obligacion_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_terreno_VIS", p_area_terreno_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_terreno_no_VIS", p_area_terreno_no_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_terreno_VIP", p_area_terreno_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_construida_VIS", p_area_construida_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_construida_no_VIS", p_area_construida_no_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_construida_VIP", p_area_construida_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_porc_obligacion_VIS", p_porc_obligacion_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_porc_obligacion_VIP", p_porc_obligacion_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_unidades_vivienda_VIS", p_unidades_vivienda_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_unidades_vivienda_no_VIS", p_unidades_vivienda_no_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_unidades_vivienda_VIP", p_unidades_vivienda_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_comercio", p_area_comercio);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_oficina", p_area_oficina);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_institucional", p_area_institucional);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_industria", p_area_industria);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_lote", p_area_lote);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_sotano", p_area_sotano);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_semisotano", p_area_semisotano);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_primer_piso", p_area_primer_piso);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_pisos_restantes", p_area_pisos_restantes);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_construida_total", p_area_construida_total);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_libre_primer_piso", p_area_libre_primer_piso);
                oDB.MySQLAddParameter(MySqlCmd, "p_porc_ejecucion_construccion", p_porc_ejecucion_construccion);
                oDB.MySQLAddParameter(MySqlCmd, "p_cumple_area_obligacion", p_cumple_area_obligacion);
                oDB.MySQLAddParameter(MySqlCmd, "p_cumple_porc_area_util", p_cumple_porc_area_util);
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

        public string sp_u_planp_licencia(
            string p_au_planp_licencia,
            string p_id_fuente_informacion,
            string p_id_tipo_licencia,
            string p_numero_licencia,
            string p_curador,
            string p_fecha_ejecutoria,
            string p_termino_vigencia_meses,
            string p_plano_urbanistico_aprobado,
            string p_nombre_proyecto,
            string p_area_bruta,
            string p_area_neta,
            string p_area_util,
            string p_area_cesion_zonas_verdes,
            string p_area_cesion_vias,
            string p_area_cesion_eq_comunal,
            string p_porc_ejecucion_urbanismo,
            string p_id_obligacion_VIS,
            string p_id_obligacion_VIP,
            string p_area_terreno_VIS,
            string p_area_terreno_no_VIS,
            string p_area_terreno_VIP,
            string p_area_construida_VIS,
            string p_area_construida_no_VIS,
            string p_area_construida_VIP,
            string p_porc_obligacion_VIS,
            string p_porc_obligacion_VIP,
            string p_unidades_vivienda_VIS,
            string p_unidades_vivienda_no_VIS,
            string p_unidades_vivienda_VIP,
            string p_area_comercio,
            string p_area_oficina,
            string p_area_institucional,
            string p_area_industria,
            string p_area_lote,
            string p_area_sotano,
            string p_area_semisotano,
            string p_area_primer_piso,
            string p_area_pisos_restantes,
            string p_area_construida_total,
            string p_area_libre_primer_piso,
            string p_porc_ejecucion_construccion,
            bool p_cumple_area_obligacion,
            bool p_cumple_porc_area_util,
            string p_observacion,
            bool p_is_file
        )
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_au_planp_licencia", p_au_planp_licencia);
                oDB.MySQLAddParameter(MySqlCmd, "p_id_fuente_informacion", p_id_fuente_informacion);
                oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_licencia", p_id_tipo_licencia);
                oDB.MySQLAddParameter(MySqlCmd, "p_numero_licencia", p_numero_licencia);
                oDB.MySQLAddParameter(MySqlCmd, "p_curador", p_curador);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_ejecutoria", p_fecha_ejecutoria);
                oDB.MySQLAddParameter(MySqlCmd, "p_termino_vigencia_meses", p_termino_vigencia_meses);
                oDB.MySQLAddParameter(MySqlCmd, "p_plano_urbanistico_aprobado", p_plano_urbanistico_aprobado);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre_proyecto", p_nombre_proyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_bruta", p_area_bruta);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_neta", p_area_neta);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_util", p_area_util);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_cesion_zonas_verdes", p_area_cesion_zonas_verdes);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_cesion_vias", p_area_cesion_vias);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_cesion_eq_comunal", p_area_cesion_eq_comunal);
                oDB.MySQLAddParameter(MySqlCmd, "p_porc_ejecucion_urbanismo", p_porc_ejecucion_urbanismo);
                oDB.MySQLAddParameter(MySqlCmd, "p_id_obligacion_VIS", p_id_obligacion_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_id_obligacion_VIP", p_id_obligacion_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_terreno_VIS", p_area_terreno_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_terreno_no_VIS", p_area_terreno_no_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_terreno_VIP", p_area_terreno_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_construida_VIS", p_area_construida_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_construida_no_VIS", p_area_construida_no_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_construida_VIP", p_area_construida_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_porc_obligacion_VIS", p_porc_obligacion_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_porc_obligacion_VIP", p_porc_obligacion_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_unidades_vivienda_VIS", p_unidades_vivienda_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_unidades_vivienda_no_VIS", p_unidades_vivienda_no_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_unidades_vivienda_VIP", p_unidades_vivienda_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_comercio", p_area_comercio);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_oficina", p_area_oficina);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_institucional", p_area_institucional);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_industria", p_area_industria);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_lote", p_area_lote);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_sotano", p_area_sotano);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_semisotano", p_area_semisotano);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_primer_piso", p_area_primer_piso);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_pisos_restantes", p_area_pisos_restantes);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_construida_total", p_area_construida_total);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_libre_primer_piso", p_area_libre_primer_piso);
                oDB.MySQLAddParameter(MySqlCmd, "p_porc_ejecucion_construccion", p_porc_ejecucion_construccion);
                oDB.MySQLAddParameter(MySqlCmd, "p_cumple_area_obligacion", p_cumple_area_obligacion);
                oDB.MySQLAddParameter(MySqlCmd, "p_cumple_porc_area_util", p_cumple_porc_area_util);
                oDB.MySQLAddParameter(MySqlCmd, "p_observacion", p_observacion);
                oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());
                oDB.MySQLAddParameter(MySqlCmd, "p_is_file", p_is_file);

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

        public string sp_d_planp_licencia(string p_au_planp_licencia)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_au_planp_licencia", p_au_planp_licencia);
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

        ~PLANESPLICENCIAS_DAL()
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