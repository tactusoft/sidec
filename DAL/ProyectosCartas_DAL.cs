using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using SigesTO;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class PROYECTOSCARTAS_DAL : IDisposable
    {
        private const string _SOURCEPAGE = "PROYECTOSCARTAS_DAL";
        private const string TABLA_PROYECTOSCARTAS = "ProyectosCartas";

        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clDB oDB = new clDB();

        private MySqlConnection MySqlConn;
        private MySqlDataAdapter MySqlDA;
        private DataTable oDataTable;
        private DataSet oDataSet;

        public PROYECTOSCARTAS_DAL()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
            MySqlConn = new MySqlConnection(strConnString);
            MySqlDA = new MySqlDataAdapter();
            oDataTable = new DataTable();
            oDataSet = new DataSet();
        }


        public DataSet sp_s_proyecto_carta(string p_idproyecto_carta)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn) { CommandType = CommandType.StoredProcedure };

                oDB.MySQLAddParameter(MySqlCmd, "p_idproyecto_carta", p_idproyecto_carta);

                return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PROYECTOSCARTAS);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
                return null;
            }
        }
        public DataSet sp_s_proyectos_cartas_cod_proyecto(string p_cod_proyecto)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_cod_proyecto", p_cod_proyecto);

                return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PROYECTOSCARTAS);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
                return null;
            }
        }

        public string sp_i_proyecto_carta(string p_cod_proyecto, string p_radicado_manifestacion_interes, string p_fecha_radicado_manifestacion_interes,
                string p_radicado_carta_intencion, string p_fecha_radicado_carta_intencion, bool p_carta_intencion_firmada, string p_fecha_firma, 
                string p_id_documento_constitucion_proyecto, string p_radicado_otrosi, string p_fecha_radicado_otrosi, string p_meses_desarrollo,
                string p_unidad_gestion_aplica_proyecto, string p_etapa_aplica_proyecto, string p_area_util, string p_area_minima_vivienda, string p_localizacion_proyecto,
                string p_UP_VIP, string p_UP_VIS, string p_UP_E3, string p_UP_E4, string p_UP_E5, string p_UP_E6, string p_ruta_archivo, string p_observacion )
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_cod_proyecto", p_cod_proyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_manifestacion_interes", p_radicado_manifestacion_interes);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_manifestacion_interes", p_fecha_radicado_manifestacion_interes);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_carta_intencion", p_radicado_carta_intencion);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_carta_intencion", p_fecha_radicado_carta_intencion);
                oDB.MySQLAddParameter(MySqlCmd, "p_carta_intencion_firmada", p_carta_intencion_firmada);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_firma", p_fecha_firma);
                oDB.MySQLAddParameter(MySqlCmd, "p_id_documento_constitucion_proyecto", p_id_documento_constitucion_proyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_otrosi", p_radicado_otrosi);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_otrosi", p_fecha_radicado_otrosi);
                oDB.MySQLAddParameter(MySqlCmd, "p_meses_desarrollo", p_meses_desarrollo);
                oDB.MySQLAddParameter(MySqlCmd, "p_unidad_gestion_aplica_proyecto", p_unidad_gestion_aplica_proyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_etapa_aplica_proyecto", p_etapa_aplica_proyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_util", p_area_util);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_minima_vivienda", p_area_minima_vivienda);
                oDB.MySQLAddParameter(MySqlCmd, "p_localizacion_proyecto", p_localizacion_proyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIP", p_UP_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIS", p_UP_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E3", p_UP_E3);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E4", p_UP_E4);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E5", p_UP_E5);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E6", p_UP_E6);
                oDB.MySQLAddParameter(MySqlCmd, "p_ruta_archivo", p_ruta_archivo);
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

        public string sp_i_proyecto_carta(ProyectoCartaTO proyectocarta)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_cod_proyecto", proyectocarta.IdProyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_manifestacion_interes", proyectocarta.RadicadoManifestacionInteres);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_manifestacion_interes", proyectocarta.FechaRadicadoManifestacionInteres);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_carta_intencion", proyectocarta.RadicadoCartaIntencion);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_carta_intencion", proyectocarta.FechaRadicadoCartaIntencion);
                oDB.MySQLAddParameter(MySqlCmd, "p_carta_intencion_firmada", proyectocarta.CartaIntencionFirmada);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_firma", proyectocarta.FechaFirma);
                oDB.MySQLAddParameter(MySqlCmd, "p_id_documento_constitucion_proyecto", proyectocarta.IdDocumentoConstitucionProyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_otrosi", proyectocarta.RadicadoOtrosi);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_otrosi", proyectocarta.FechaRadicadoOtrosi);
                oDB.MySQLAddParameter(MySqlCmd, "p_meses_desarrollo", proyectocarta.MesesDesarrollo);
                oDB.MySQLAddParameter(MySqlCmd, "p_unidad_gestion_aplica_proyecto", proyectocarta.UnidadGestionAplicaProyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_etapa_aplica_proyecto", proyectocarta.EtapaAplicaProyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_util", proyectocarta.AreaUtil);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_minima_vivienda", proyectocarta.AreaMinimaVivienda);
                oDB.MySQLAddParameter(MySqlCmd, "p_localizacion_proyecto", proyectocarta.LocalizacionProyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIP", proyectocarta.UP_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIS", proyectocarta.UP_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E3", proyectocarta.UP_E3);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E4", proyectocarta.UP_E4);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E5", proyectocarta.UP_E5);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E6", proyectocarta.UP_E6);
                oDB.MySQLAddParameter(MySqlCmd, "p_ruta_archivo", proyectocarta.RutaArchivo);
                oDB.MySQLAddParameter(MySqlCmd, "p_observacion", proyectocarta.Observacion);
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

        public string sp_u_proyecto_carta(ProyectoCartaTO proyectocarta)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_au_proyecto_carta", proyectocarta.IdProyectoCarta);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_manifestacion_interes", proyectocarta.RadicadoManifestacionInteres);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_manifestacion_interes", proyectocarta.FechaRadicadoManifestacionInteres);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_carta_intencion", proyectocarta.RadicadoCartaIntencion);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_carta_intencion", proyectocarta.FechaRadicadoCartaIntencion);
                oDB.MySQLAddParameter(MySqlCmd, "p_carta_intencion_firmada", proyectocarta.CartaIntencionFirmada);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_firma", proyectocarta.FechaFirma);
                oDB.MySQLAddParameter(MySqlCmd, "p_id_documento_constitucion_proyecto", proyectocarta.IdDocumentoConstitucionProyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_otrosi", proyectocarta.RadicadoOtrosi);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_otrosi", proyectocarta.FechaRadicadoOtrosi);
                oDB.MySQLAddParameter(MySqlCmd, "p_meses_desarrollo", proyectocarta.MesesDesarrollo);
                oDB.MySQLAddParameter(MySqlCmd, "p_unidad_gestion_aplica_proyecto", proyectocarta.UnidadGestionAplicaProyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_etapa_aplica_proyecto", proyectocarta.EtapaAplicaProyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_util", proyectocarta.AreaUtil);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_minima_vivienda", proyectocarta.AreaMinimaVivienda);
                oDB.MySQLAddParameter(MySqlCmd, "p_localizacion_proyecto", proyectocarta.LocalizacionProyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIP", proyectocarta.UP_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIS", proyectocarta.UP_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E3", proyectocarta.UP_E3);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E4", proyectocarta.UP_E4);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E5", proyectocarta.UP_E5);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E6", proyectocarta.UP_E6);
                oDB.MySQLAddParameter(MySqlCmd, "p_ruta_archivo", proyectocarta.RutaArchivo);
                oDB.MySQLAddParameter(MySqlCmd, "p_observacion", proyectocarta.Observacion);
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

        public string sp_u_proyecto_carta( string p_au_proyecto_carta, string p_radicado_manifestacion_interes, string p_fecha_radicado_manifestacion_interes,
                string p_radicado_carta_intencion, string p_fecha_radicado_carta_intencion, bool p_carta_intencion_firmada, string p_fecha_firma, string p_id_documento_constitucion_proyecto,
                string p_radicado_otrosi, string p_fecha_radicado_otrosi, string p_meses_desarrollo, string p_unidad_gestion_aplica_proyecto, string p_etapa_aplica_proyecto,
                string p_area_util, string p_area_minima_vivienda, string p_localizacion_proyecto, string p_UP_VIP, string p_UP_VIS, string p_UP_E3, string p_UP_E4,
                string p_UP_E5, string p_UP_E6, string p_ruta_archivo, string p_observacion)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_au_proyecto_carta", p_au_proyecto_carta);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_manifestacion_interes", p_radicado_manifestacion_interes);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_manifestacion_interes", p_fecha_radicado_manifestacion_interes);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_carta_intencion", p_radicado_carta_intencion);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_carta_intencion", p_fecha_radicado_carta_intencion);
                oDB.MySQLAddParameter(MySqlCmd, "p_carta_intencion_firmada", p_carta_intencion_firmada);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_firma", p_fecha_firma);
                oDB.MySQLAddParameter(MySqlCmd, "p_id_documento_constitucion_proyecto", p_id_documento_constitucion_proyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_otrosi", p_radicado_otrosi);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_otrosi", p_fecha_radicado_otrosi);
                oDB.MySQLAddParameter(MySqlCmd, "p_meses_desarrollo", p_meses_desarrollo);
                oDB.MySQLAddParameter(MySqlCmd, "p_unidad_gestion_aplica_proyecto", p_unidad_gestion_aplica_proyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_etapa_aplica_proyecto", p_etapa_aplica_proyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_util", p_area_util);
                oDB.MySQLAddParameter(MySqlCmd, "p_area_minima_vivienda", p_area_minima_vivienda);
                oDB.MySQLAddParameter(MySqlCmd, "p_localizacion_proyecto", p_localizacion_proyecto);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIP", p_UP_VIP);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIS", p_UP_VIS);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E3", p_UP_E3);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E4", p_UP_E4);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E5", p_UP_E5);
                oDB.MySQLAddParameter(MySqlCmd, "p_UP_E6", p_UP_E6);
                oDB.MySQLAddParameter(MySqlCmd, "p_ruta_archivo", p_ruta_archivo);
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

        public string sp_d_proyecto_carta(string p_au_proyecto_carta)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
                MySqlCmd.CommandType = CommandType.StoredProcedure;

                oDB.MySQLAddParameter(MySqlCmd, "p_au_proyecto_carta", p_au_proyecto_carta);
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

        ~PROYECTOSCARTAS_DAL()
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