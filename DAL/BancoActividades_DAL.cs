using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class BANCOACTIVIDADES_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "BANCOACTIVIDADES_DAL";
		private const string TABLA_BANCO_ACTIVIDADES = "BancoActividades";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;

		public BANCOACTIVIDADES_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
		}

		public DataSet sp_s_bancoactividades_listar(int p_idbanco, bool? p_clave = null, bool? p_activo = null, bool? p_vigente = null, string p_idActividad = null)
        {
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);
				oDB.MySQLAddParameter(MySqlCmd, "p_clave", p_clave);
				oDB.MySQLAddParameter(MySqlCmd, "p_activo", p_activo);
				oDB.MySQLAddParameter(MySqlCmd, "p_vigente", p_vigente);
				oDB.MySQLAddParameter(MySqlCmd, "p_idactividad", p_idActividad);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_BANCO_ACTIVIDADES);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public DataSet sp_s_bancoactividad_consultar(string p_idactividad)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idactividad", p_idactividad);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_BANCO_ACTIVIDADES);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public string sp_i_bancoactividades(int p_idbanco, string p_idestado_actividad, string p_nombre, string p_identidad, string p_dependencia, 
											string p_radicado, string p_fecha_radicado, string p_encargado, string p_datos_contacto, string p_solicitud, 
											string p_problematica, string p_impacto, string p_gestion_sgs, string p_gestion_adelantar,
											string p_fec_inicio, string p_fec_culminacion, string p_fec_finalizacion, bool p_clave, 
											bool p_activo, string p_idtramite, string p_fec_posible, string p_porc_completado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);
				oDB.MySQLAddParameter(MySqlCmd, "p_idestado_actividad", p_idestado_actividad);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);
				oDB.MySQLAddParameter(MySqlCmd, "p_identidad", p_identidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_dependencia", p_dependencia);
				
				oDB.MySQLAddParameter(MySqlCmd, "p_radicado", p_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado", p_fecha_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_encargado", p_encargado);
				oDB.MySQLAddParameter(MySqlCmd, "p_datos_contacto", p_datos_contacto);
				oDB.MySQLAddParameter(MySqlCmd, "p_solicitud", p_solicitud);
				
				oDB.MySQLAddParameter(MySqlCmd, "p_problematica", p_problematica);
				oDB.MySQLAddParameter(MySqlCmd, "p_impacto", p_impacto);
				oDB.MySQLAddParameter(MySqlCmd, "p_gestion_sgs", p_gestion_sgs);
				oDB.MySQLAddParameter(MySqlCmd, "p_gestion_adelantar", p_gestion_adelantar);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_inicio", p_fec_inicio);

				oDB.MySQLAddParameter(MySqlCmd, "p_fec_culminacion", p_fec_culminacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_finalizacion", p_fec_finalizacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_clave", p_clave);				
				oDB.MySQLAddParameter(MySqlCmd, "p_idtramite", p_idtramite);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_posible", p_fec_posible);

				oDB.MySQLAddParameter(MySqlCmd, "p_activo", p_activo);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_completado", p_porc_completado);
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

		public string sp_u_bancoactividades(int p_idactividad, string p_idestado_actividad, string p_nombre, string p_identidad, string p_dependencia,
											string p_radicado, string p_fecha_radicado, string p_encargado, string p_datos_contacto, string p_solicitud,
											string p_problematica, string p_impacto, string p_gestion_sgs, string p_gestion_adelantar, 
											string p_fec_inicio, string p_fec_culminacion, string p_fec_finalizacion, bool p_clave, 
											bool p_activo, string p_idtramite, string p_fec_posible, string p_porc_completado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};

				oDB.MySQLAddParameter(MySqlCmd, "p_idactividad", p_idactividad);
				oDB.MySQLAddParameter(MySqlCmd, "p_idestado_actividad", p_idestado_actividad);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);
				oDB.MySQLAddParameter(MySqlCmd, "p_identidad", p_identidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_dependencia", p_dependencia);

				oDB.MySQLAddParameter(MySqlCmd, "p_radicado", p_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado", p_fecha_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_encargado", p_encargado);
				oDB.MySQLAddParameter(MySqlCmd, "p_datos_contacto", p_datos_contacto);
				oDB.MySQLAddParameter(MySqlCmd, "p_solicitud", p_solicitud);

				oDB.MySQLAddParameter(MySqlCmd, "p_problematica", p_problematica);
				oDB.MySQLAddParameter(MySqlCmd, "p_impacto", p_impacto);
				oDB.MySQLAddParameter(MySqlCmd, "p_gestion_sgs", p_gestion_sgs);
				oDB.MySQLAddParameter(MySqlCmd, "p_gestion_adelantar", p_gestion_adelantar);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_inicio", p_fec_inicio);

				oDB.MySQLAddParameter(MySqlCmd, "p_fec_culminacion", p_fec_culminacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_finalizacion", p_fec_finalizacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_clave", p_clave);
				oDB.MySQLAddParameter(MySqlCmd, "p_idtramite", p_idtramite);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_posible", p_fec_posible);

				oDB.MySQLAddParameter(MySqlCmd, "p_activo", p_activo);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_completado", p_porc_completado);
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

		public string sp_u_bancoactividades_gestion(int p_idactividad, string p_idtramite, string p_otrotramite, string p_fec_posible, string p_ruta_archivo)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};

				oDB.MySQLAddParameter(MySqlCmd, "p_idactividad", p_idactividad);
				oDB.MySQLAddParameter(MySqlCmd, "p_idtramite", p_idtramite);
				oDB.MySQLAddParameter(MySqlCmd, "p_otrotramite", p_otrotramite);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_posible", p_fec_posible);
				oDB.MySQLAddParameter(MySqlCmd, "p_ruta_archivo", p_ruta_archivo);
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

		public string sp_d_bancoactividades(string p_idbanco_actividad)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};

				oDB.MySQLAddParameter(MySqlCmd, "p_idbanco_actividad", p_idbanco_actividad);
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


		public DataSet sp_s_bancoactividad_reporte()
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_BANCO_ACTIVIDADES);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		#region-----DISPOSE
		// Metodo para el manejo del GC
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~BANCOACTIVIDADES_DAL()
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