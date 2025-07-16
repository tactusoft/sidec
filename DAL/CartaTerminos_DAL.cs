using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using SigesTO;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class CARTA_TERMINOS_DAL : IDisposable
    {
        private const string _SOURCEPAGE = "CARTA_TERMINOS_DAL";
        private const string TABLA_CARTA_TERMINOS = "carta_terminos";

        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clDB oDB = new clDB();

        private MySqlConnection MySqlConn;
        private MySqlDataAdapter MySqlDA;
        private DataSet oDataSet;

        public CARTA_TERMINOS_DAL()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
            MySqlConn = new MySqlConnection(strConnString);
            MySqlDA = new MySqlDataAdapter();
            oDataSet = new DataSet();
        }
        public DataSet sp_s_carta_terminos(string p_idcarta_terminos)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn) { CommandType = CommandType.StoredProcedure };

                oDB.MySQLAddParameter(MySqlCmd, "p_idcarta_terminos", p_idcarta_terminos);

                return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_CARTA_TERMINOS);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
                return null;
            }
        }
        public DataSet sp_s_cartas_terminos_reporte()
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn) { CommandType = CommandType.StoredProcedure };

                return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_CARTA_TERMINOS);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
                return null;
            }
        }

        public DataSet sp_s_cartas_terminos(string p_idpredio_declarado)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn) { CommandType = CommandType.StoredProcedure };

                oDB.MySQLAddParameter(MySqlCmd, "p_idpredio_declarado", p_idpredio_declarado);

                return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_CARTA_TERMINOS);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
                return null;
            }
        }
        public string sp_i_carta_terminos(CartaTerminosTO pCartaTerminos)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn) { CommandType = CommandType.StoredProcedure };

                oDB.MySQLAddParameter(MySqlCmd, "p_idprediodeclarado", pCartaTerminos.IdPredioDeclarado);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_busqueda_info", pCartaTerminos.ProfBusquedaInfo);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_elab_carta", pCartaTerminos.ProfElabCarta);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_revision", pCartaTerminos.ProfRevision);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_elab_carta_par", pCartaTerminos.ProfElabCartaPar);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_revision_par", pCartaTerminos.ProfRevisionPar);
                oDB.MySQLAddParameter(MySqlCmd, "p_envio_carta", pCartaTerminos.EnvioCarta);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_salida", pCartaTerminos.RadicadoSalida);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_salida", pCartaTerminos.FechaRadicadoSalida);
                oDB.MySQLAddParameter(MySqlCmd, "p_obs_rev_info", pCartaTerminos.ObsRevInfo);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre_receptor", pCartaTerminos.NombreReceptor);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_entrega", pCartaTerminos.FechaEntrega);
                oDB.MySQLAddParameter(MySqlCmd, "p_motivo_devolucion", pCartaTerminos.MotivoDevolucion);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_devolucion", pCartaTerminos.FechaDevolucion);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_entrada", pCartaTerminos.RadicadoEntrada);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_manifestacion", pCartaTerminos.FechaManifestacion);
                oDB.MySQLAddParameter(MySqlCmd, "p_resumen_manifestacion", pCartaTerminos.ResumenManifestacion);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_licencia", pCartaTerminos.TipoLicencia);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_licencia", pCartaTerminos.RadicadoLicencia);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_licencia", pCartaTerminos.FechaLicencia);
                oDB.MySQLAddParameter(MySqlCmd, "p_obs_adicionales", pCartaTerminos.ObsAdicionales);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_elab_respuesta", pCartaTerminos.ProfElabRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_rev_respuesta", pCartaTerminos.ProfRevRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_resumen_respuesta_sdht", pCartaTerminos.ResumenRespuestaSdht);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_respuesta_salida", pCartaTerminos.RadicadoRespuestaSalida);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_respuesta", pCartaTerminos.FechaRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre_receptor_respuesta", pCartaTerminos.NombreReceptorRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_entrega_respuesta", pCartaTerminos.FechaEntregaRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_motivo_devolucion_respuesta", pCartaTerminos.MotivoDevolucionRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_devolucion_respuesta", pCartaTerminos.FechaDevolucionRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_primera_notif", pCartaTerminos.FechaPrimeraNotif);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_notif_primera", pCartaTerminos.TipoNotifPrimera);
                oDB.MySQLAddParameter(MySqlCmd, "p_num_propietarios_notif1", pCartaTerminos.NumPropietariosNotif1);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre_prop_notif1", pCartaTerminos.NombrePropNotif1);
                oDB.MySQLAddParameter(MySqlCmd, "p_dir_corresp_fisica1", pCartaTerminos.DirCorrespFisica1);
                oDB.MySQLAddParameter(MySqlCmd, "p_dir_corresp_electronica1", pCartaTerminos.DirCorrespElectronica1);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_recurso1", pCartaTerminos.FechaRecurso1);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_recurso1", pCartaTerminos.RadicadoRecurso1);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_resol_recurso1", pCartaTerminos.FechaResolRecurso1);
                oDB.MySQLAddParameter(MySqlCmd, "p_num_acto_admin1", pCartaTerminos.NumActoAdmin1);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_notif_resol1", pCartaTerminos.FechaNotifResol1);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_notif_resol1", pCartaTerminos.TipoNotifResol1);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_segunda_notif", pCartaTerminos.FechaSegundaNotif);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_notif_segunda", pCartaTerminos.TipoNotifSegunda);
                oDB.MySQLAddParameter(MySqlCmd, "p_num_propietarios_notif2", pCartaTerminos.NumPropietariosNotif2);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre_prop_notif2", pCartaTerminos.NombrePropNotif2);
                oDB.MySQLAddParameter(MySqlCmd, "p_dir_corresp_fisica2", pCartaTerminos.DirCorrespFisica2);
                oDB.MySQLAddParameter(MySqlCmd, "p_dir_corresp_electronica2", pCartaTerminos.DirCorrespElectronica2);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_recurso2", pCartaTerminos.FechaRecurso2);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_recurso2", pCartaTerminos.RadicadoRecurso2);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_resol_recurso2", pCartaTerminos.FechaResolRecurso2);
                oDB.MySQLAddParameter(MySqlCmd, "p_num_acto_admin2", pCartaTerminos.NumActoAdmin2);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_notif_resol2", pCartaTerminos.FechaNotifResol2);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_notif_resol2", pCartaTerminos.TipoNotifResol2);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_resol_recurso3", pCartaTerminos.FechaResolRecurso3);
                oDB.MySQLAddParameter(MySqlCmd, "p_num_acto_admin3", pCartaTerminos.NumActoAdmin3);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_notif_resol3", pCartaTerminos.FechaNotifResol3);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_notif_resol3", pCartaTerminos.TipoNotifResol3);
                oDB.MySQLAddParameter(MySqlCmd, "p_num_propietarios_notif3", pCartaTerminos.NumPropietariosNotif3);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre_prop_notif3", pCartaTerminos.NombrePropNotif3);
                oDB.MySQLAddParameter(MySqlCmd, "p_dir_corresp_fisica3", pCartaTerminos.DirCorrespFisica3);
                oDB.MySQLAddParameter(MySqlCmd, "p_dir_corresp_electronica3", pCartaTerminos.DirCorrespElectronica3);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_firmeza_ejec1", pCartaTerminos.FechaFirmezaEjec1);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_expedicion_ejec1", pCartaTerminos.FechaExpedicionEjec1);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_firmeza_ejec2", pCartaTerminos.FechaFirmezaEjec2);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_expedicion_ejec2", pCartaTerminos.FechaExpedicionEjec2);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_firmeza_ejec3", pCartaTerminos.FechaFirmezaEjec3);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_expedicion_ejec3", pCartaTerminos.FechaExpedicionEjec3);

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
        public string sp_u_carta_terminos(CartaTerminosTO pCartaTerminos)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn) { CommandType = CommandType.StoredProcedure };

                oDB.MySQLAddParameter(MySqlCmd, "p_id_carta_terminos", pCartaTerminos.IdCartaTerminos);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_busqueda_info", pCartaTerminos.ProfBusquedaInfo);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_elab_carta", pCartaTerminos.ProfElabCarta);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_revision", pCartaTerminos.ProfRevision);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_elab_carta_par", pCartaTerminos.ProfElabCartaPar);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_revision_par", pCartaTerminos.ProfRevisionPar);
                oDB.MySQLAddParameter(MySqlCmd, "p_envio_carta", pCartaTerminos.EnvioCarta);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_salida", pCartaTerminos.RadicadoSalida);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado_salida", pCartaTerminos.FechaRadicadoSalida);
                oDB.MySQLAddParameter(MySqlCmd, "p_obs_rev_info", pCartaTerminos.ObsRevInfo);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre_receptor", pCartaTerminos.NombreReceptor);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_entrega", pCartaTerminos.FechaEntrega);
                oDB.MySQLAddParameter(MySqlCmd, "p_motivo_devolucion", pCartaTerminos.MotivoDevolucion);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_devolucion", pCartaTerminos.FechaDevolucion);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_entrada", pCartaTerminos.RadicadoEntrada);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_manifestacion", pCartaTerminos.FechaManifestacion);
                oDB.MySQLAddParameter(MySqlCmd, "p_resumen_manifestacion", pCartaTerminos.ResumenManifestacion);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_licencia", pCartaTerminos.TipoLicencia);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_licencia", pCartaTerminos.RadicadoLicencia);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_licencia", pCartaTerminos.FechaLicencia);
                oDB.MySQLAddParameter(MySqlCmd, "p_obs_adicionales", pCartaTerminos.ObsAdicionales);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_elab_respuesta", pCartaTerminos.ProfElabRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_prof_rev_respuesta", pCartaTerminos.ProfRevRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_resumen_respuesta_sdht", pCartaTerminos.ResumenRespuestaSdht);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_respuesta_salida", pCartaTerminos.RadicadoRespuestaSalida);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_respuesta", pCartaTerminos.FechaRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre_receptor_respuesta", pCartaTerminos.NombreReceptorRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_entrega_respuesta", pCartaTerminos.FechaEntregaRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_motivo_devolucion_respuesta", pCartaTerminos.MotivoDevolucionRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_devolucion_respuesta", pCartaTerminos.FechaDevolucionRespuesta);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_primera_notif", pCartaTerminos.FechaPrimeraNotif);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_notif_primera", pCartaTerminos.TipoNotifPrimera);
                oDB.MySQLAddParameter(MySqlCmd, "p_num_propietarios_notif1", pCartaTerminos.NumPropietariosNotif1);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre_prop_notif1", pCartaTerminos.NombrePropNotif1);
                oDB.MySQLAddParameter(MySqlCmd, "p_dir_corresp_fisica1", pCartaTerminos.DirCorrespFisica1);
                oDB.MySQLAddParameter(MySqlCmd, "p_dir_corresp_electronica1", pCartaTerminos.DirCorrespElectronica1);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_recurso1", pCartaTerminos.FechaRecurso1);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_recurso1", pCartaTerminos.RadicadoRecurso1);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_resol_recurso1", pCartaTerminos.FechaResolRecurso1);
                oDB.MySQLAddParameter(MySqlCmd, "p_num_acto_admin1", pCartaTerminos.NumActoAdmin1);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_notif_resol1", pCartaTerminos.FechaNotifResol1);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_notif_resol1", pCartaTerminos.TipoNotifResol1);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_segunda_notif", pCartaTerminos.FechaSegundaNotif);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_notif_segunda", pCartaTerminos.TipoNotifSegunda);
                oDB.MySQLAddParameter(MySqlCmd, "p_num_propietarios_notif2", pCartaTerminos.NumPropietariosNotif2);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre_prop_notif2", pCartaTerminos.NombrePropNotif2);
                oDB.MySQLAddParameter(MySqlCmd, "p_dir_corresp_fisica2", pCartaTerminos.DirCorrespFisica2);
                oDB.MySQLAddParameter(MySqlCmd, "p_dir_corresp_electronica2", pCartaTerminos.DirCorrespElectronica2);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_recurso2", pCartaTerminos.FechaRecurso2);
                oDB.MySQLAddParameter(MySqlCmd, "p_radicado_recurso2", pCartaTerminos.RadicadoRecurso2);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_resol_recurso2", pCartaTerminos.FechaResolRecurso2);
                oDB.MySQLAddParameter(MySqlCmd, "p_num_acto_admin2", pCartaTerminos.NumActoAdmin2);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_notif_resol2", pCartaTerminos.FechaNotifResol2);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_notif_resol2", pCartaTerminos.TipoNotifResol2);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_resol_recurso3", pCartaTerminos.FechaResolRecurso3);
                oDB.MySQLAddParameter(MySqlCmd, "p_num_acto_admin3", pCartaTerminos.NumActoAdmin3);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_notif_resol3", pCartaTerminos.FechaNotifResol3);
                oDB.MySQLAddParameter(MySqlCmd, "p_tipo_notif_resol3", pCartaTerminos.TipoNotifResol3);
                oDB.MySQLAddParameter(MySqlCmd, "p_num_propietarios_notif3", pCartaTerminos.NumPropietariosNotif3);
                oDB.MySQLAddParameter(MySqlCmd, "p_nombre_prop_notif3", pCartaTerminos.NombrePropNotif3);
                oDB.MySQLAddParameter(MySqlCmd, "p_dir_corresp_fisica3", pCartaTerminos.DirCorrespFisica3);
                oDB.MySQLAddParameter(MySqlCmd, "p_dir_corresp_electronica3", pCartaTerminos.DirCorrespElectronica3);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_firmeza_ejec1", pCartaTerminos.FechaFirmezaEjec1);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_expedicion_ejec1", pCartaTerminos.FechaExpedicionEjec1);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_firmeza_ejec2", pCartaTerminos.FechaFirmezaEjec2);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_expedicion_ejec2", pCartaTerminos.FechaExpedicionEjec2);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_firmeza_ejec3", pCartaTerminos.FechaFirmezaEjec3);
                oDB.MySQLAddParameter(MySqlCmd, "p_fecha_expedicion_ejec3", pCartaTerminos.FechaExpedicionEjec3);

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
        public string sp_d_carta_terminos(string p_idcarta_terminos)
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn) { CommandType = CommandType.StoredProcedure };

                oDB.MySQLAddParameter(MySqlCmd, "p_idcarta_terminos", p_idcarta_terminos);
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

        ~CARTA_TERMINOS_DAL()
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
