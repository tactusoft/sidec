using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class PLANESP_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PLANESP_DAL";
		private const string TABLA_PLANESP = "PlanesP";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public PLANESP_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_planesp(int p_au_planp)
		{
			string sp = "sp_s_planesp";
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_planp", p_au_planp);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PLANESP);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_planesp_nombre(string p_nombre_planp)
		{
			string sp = "sp_s_planesp_nombre";
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_planp", p_nombre_planp);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PLANESP);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_proyecto_x_planesp(
			int p_au_planp, string p_chip)
		{
			string sp = "sp_s_proyecto_x_planesp";
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_planp", p_au_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_chip", p_chip);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PLANESP);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_planesp_lista(bool? p_es_proyecto_asociativo = null)
		{
			string sp = "sp_s_planesp_lista";
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_es_proyecto_asociativo", p_es_proyecto_asociativo);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PLANESP);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_rpt_planesp_general(
			string p_id_tipo_tratamiento,
			string p_rango_1,
			string p_rango_2,
			string p_ano_1,
			string p_ano_2)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_tratamiento", p_id_tipo_tratamiento);
				oDB.MySQLAddParameter(MySqlCmd, "p_rango_1", p_rango_1);
				oDB.MySQLAddParameter(MySqlCmd, "p_rango_2", p_rango_2);
				oDB.MySQLAddParameter(MySqlCmd, "p_ano_1", p_ano_1);
				oDB.MySQLAddParameter(MySqlCmd, "p_ano_2", p_ano_2);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PLANESP);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_rpt_planesp_cesiones(
			string p_id_tipo_tratamiento,
			string p_rango_1,
			string p_rango_2,
			string p_ano_1,
			string p_ano_2)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_tratamiento", p_id_tipo_tratamiento);
				oDB.MySQLAddParameter(MySqlCmd, "p_rango_1", p_rango_1);
				oDB.MySQLAddParameter(MySqlCmd, "p_rango_2", p_rango_2);
				oDB.MySQLAddParameter(MySqlCmd, "p_ano_1", p_ano_1);
				oDB.MySQLAddParameter(MySqlCmd, "p_ano_2", p_ano_2);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PLANESP);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public DataSet sp_rpt_planesp_indicadores(
			string p_id_tipo_tratamiento,
			string p_ano_1,
			string p_ano_2)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_tratamiento", p_id_tipo_tratamiento);
				oDB.MySQLAddParameter(MySqlCmd, "p_ano_1", p_ano_1);
				oDB.MySQLAddParameter(MySqlCmd, "p_ano_2", p_ano_2);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PLANESP);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		#region sp_i_planp
		public string sp_i_planp(
			string p_cod_sdp,
			string p_nombre_planp,
			string p_direccion_planp,
			string p_cod_localidad,
			string p_idupz,
			string p_id_categoria_planp,
			string p_id_estado_planp,
			string p_id_tipo_tratamiento,
			string p_id_clasificacion_suelo,
			bool p_es_proyecto_asociativo,
			bool p_tiene_carta_intencion,
			string p_fecha_firma_carta_intencion,
			string p_fecha_iniciacion_viviendas,

			string p_rango_edificabilidad,
			string p_areas_zonas,

			string p_area_bruta,
			string p_area_neta_urbanizable,
			string p_area_base_calculo_cesiones,
			string p_area_util,
			string p_habitantes_vivienda,

			string p_area_af_malla_vial_arterial,
			string p_area_af_rondas,
			string p_area_af_zmpa,
			string p_area_af_espacio_publico,
			string p_area_af_servicios_publicos,
			string p_area_af_manejo_dif,
			string p_af_otro,
			string p_area_af_otro,

			string p_unidades_potencial_VIP,
			string p_unidades_potencial_VIS,
			string p_unidades_potencial_no_VIS,
			string p_unidades_ejecutadas_VIP,
			string p_unidades_ejecutadas_VIS,
			string p_unidades_ejecutadas_no_VIS,

			bool p_obligacion_suelo_VIP_en_sitio,
			bool p_obligacion_suelo_VIP_compensacion,
			bool p_obligacion_suelo_VIP_traslado,

			bool p_obligacion_suelo_VIS_en_sitio,
			bool p_obligacion_suelo_VIS_compensacion,
			bool p_obligacion_suelo_VIS_traslado,

			bool p_es_obligacion_construccion_VIP,
			bool p_obligacion_construccion_VIP_en_sitio,
			bool p_obligacion_construccion_VIP_compensacion,
			bool p_obligacion_construccion_VIP_traslado,
			string p_obligacion_construccion_VIP_area,
			string p_obligacion_construccion_VIP_area_ejecutada,
			string p_porc_obligacion_construccion_VIP,

			bool p_es_obligacion_construccion_VIS,
			bool p_obligacion_construccion_VIS_en_sitio,
			bool p_obligacion_construccion_VIS_compensacion,
			bool p_obligacion_construccion_VIS_traslado,
			string p_obligacion_construccion_VIS_area,
			string p_obligacion_construccion_VIS_area_ejecutada,
			string p_porc_obligacion_construccion_VIS,

			string p_decreto_obligacion,
			string p_articulo_obligacion,

			string p_traslado_acto_proyecto_generador,
			string p_traslado_area,
			string p_traslado_acto_proyecto_receptor,
			string p_traslado_localizacion_receptor,
			bool p_traslado_cumple_area_receptor,
			bool p_traslado_cumple_porc_receptor,
			bool p_traslado_es_primera_etapa_receptor,

			string p_compensacion_licencia,
			bool p_compensacion_tiene_certificado_pago,
			bool p_compensacion_cumple_area,
			string p_obs_modalidad_cumplimiento,

			bool p_es_suelo_desarrollo_prioritario,
			string p_decreto_declaratoria,
			string p_articulo_declaratoria,
			string p_fecha_inicio_declaratoria,
			string p_fecha_fin_declaratoria,
			string p_id_estado_declaratoria,
			string p_observacion_declaratoria,

			string p_observacion,
			string p_cod_usu_responsable
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_sdp", p_cod_sdp);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_planp", p_nombre_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_direccion_planp", p_direccion_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_localidad", p_cod_localidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_idupz", p_idupz);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_categoria_planp", p_id_categoria_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_estado_planp", p_id_estado_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_tratamiento", p_id_tipo_tratamiento);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_clasificacion_suelo", p_id_clasificacion_suelo);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_proyecto_asociativo", p_es_proyecto_asociativo);
				oDB.MySQLAddParameter(MySqlCmd, "p_tiene_carta_intencion", p_tiene_carta_intencion);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_firma_carta_intencion", p_fecha_firma_carta_intencion);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_iniciacion_viviendas", p_fecha_iniciacion_viviendas);

				oDB.MySQLAddParameter(MySqlCmd, "p_rango_edificabilidad", p_rango_edificabilidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_areas_zonas", p_areas_zonas);

				oDB.MySQLAddParameter(MySqlCmd, "p_area_bruta", p_area_bruta);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_neta_urbanizable", p_area_neta_urbanizable);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_base_calculo_cesiones", p_area_base_calculo_cesiones);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_util", p_area_util);
				oDB.MySQLAddParameter(MySqlCmd, "p_habitantes_vivienda", p_habitantes_vivienda);

				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_malla_vial_arterial", p_area_af_malla_vial_arterial);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_rondas", p_area_af_rondas);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_zmpa", p_area_af_zmpa);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_espacio_publico", p_area_af_espacio_publico);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_servicios_publicos", p_area_af_servicios_publicos);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_manejo_dif", p_area_af_manejo_dif);
				oDB.MySQLAddParameter(MySqlCmd, "p_af_otro", p_af_otro);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_otro", p_area_af_otro);

				oDB.MySQLAddParameter(MySqlCmd, "p_unidades_potencial_VIP", p_unidades_potencial_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidades_potencial_VIS", p_unidades_potencial_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidades_potencial_no_VIS", p_unidades_potencial_no_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidades_ejecutadas_VIP", p_unidades_ejecutadas_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidades_ejecutadas_VIS", p_unidades_ejecutadas_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidades_ejecutadas_no_VIS", p_unidades_ejecutadas_no_VIS);

				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_suelo_VIP_en_sitio", p_obligacion_suelo_VIP_en_sitio);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_suelo_VIP_compensacion", p_obligacion_suelo_VIP_compensacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_suelo_VIP_traslado", p_obligacion_suelo_VIP_traslado);

				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_suelo_VIS_en_sitio", p_obligacion_suelo_VIS_en_sitio);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_suelo_VIS_compensacion", p_obligacion_suelo_VIS_compensacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_suelo_VIS_traslado", p_obligacion_suelo_VIS_traslado);

				oDB.MySQLAddParameter(MySqlCmd, "p_es_obligacion_construccion_VIP", p_es_obligacion_construccion_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIP_en_sitio", p_obligacion_construccion_VIP_en_sitio);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIP_compensacion", p_obligacion_construccion_VIP_compensacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIP_traslado", p_obligacion_construccion_VIP_traslado);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIP_area", p_obligacion_construccion_VIP_area);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIP_area_ejecutada", p_obligacion_construccion_VIP_area_ejecutada);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_obligacion_construccion_VIP", p_porc_obligacion_construccion_VIP);

				oDB.MySQLAddParameter(MySqlCmd, "p_es_obligacion_construccion_VIS", p_es_obligacion_construccion_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIS_en_sitio", p_obligacion_construccion_VIS_en_sitio);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIS_compensacion", p_obligacion_construccion_VIS_compensacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIS_traslado", p_obligacion_construccion_VIS_traslado);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIS_area", p_obligacion_construccion_VIS_area);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIS_area_ejecutada", p_obligacion_construccion_VIS_area_ejecutada);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_obligacion_construccion_VIS", p_porc_obligacion_construccion_VIS);

				oDB.MySQLAddParameter(MySqlCmd, "p_decreto_obligacion", p_decreto_obligacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_articulo_obligacion", p_articulo_obligacion);

				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_acto_proyecto_generador", p_traslado_acto_proyecto_generador);
				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_area", p_traslado_area);
				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_acto_proyecto_receptor", p_traslado_acto_proyecto_receptor);
				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_localizacion_receptor", p_traslado_localizacion_receptor);
				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_cumple_area_receptor", p_traslado_cumple_area_receptor);
				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_cumple_porc_receptor", p_traslado_cumple_porc_receptor);
				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_es_primera_etapa_receptor", p_traslado_es_primera_etapa_receptor);

				oDB.MySQLAddParameter(MySqlCmd, "p_compensacion_licencia", p_compensacion_licencia);
				oDB.MySQLAddParameter(MySqlCmd, "p_compensacion_tiene_certificado_pago", p_compensacion_tiene_certificado_pago);
				oDB.MySQLAddParameter(MySqlCmd, "p_compensacion_cumple_area", p_compensacion_cumple_area);
				oDB.MySQLAddParameter(MySqlCmd, "p_obs_modalidad_cumplimiento", p_obs_modalidad_cumplimiento);

				oDB.MySQLAddParameter(MySqlCmd, "p_es_suelo_desarrollo_prioritario", p_es_suelo_desarrollo_prioritario);
				oDB.MySQLAddParameter(MySqlCmd, "p_decreto_declaratoria", p_decreto_declaratoria);
				oDB.MySQLAddParameter(MySqlCmd, "p_articulo_declaratoria", p_articulo_declaratoria);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_inicio_declaratoria", p_fecha_inicio_declaratoria);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_fin_declaratoria", p_fecha_fin_declaratoria);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_estado_declaratoria", p_id_estado_declaratoria);
				oDB.MySQLAddParameter(MySqlCmd, "p_observacion_declaratoria", p_observacion_declaratoria);

				oDB.MySQLAddParameter(MySqlCmd, "p_observacion", p_observacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_responsable", p_cod_usu_responsable);
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

		#region sp_u_planp
		public string sp_u_planp(
			string p_au_planp,
			string p_cod_sdp,
			string p_nombre_planp,
			string p_direccion_planp,
			string p_cod_localidad,
			string p_idupz,
			string p_id_categoria_planp,
			string p_id_estado_planp,
			string p_id_tipo_tratamiento,
			string p_id_clasificacion_suelo,
			bool p_es_proyecto_asociativo,
			bool p_tiene_carta_intencion,
			string p_fecha_firma_carta_intencion,
			string p_fecha_iniciacion_viviendas,

			string p_rango_edificabilidad,
			string p_areas_zonas,

			string p_area_bruta,
			string p_area_neta_urbanizable,
			string p_area_base_calculo_cesiones,
			string p_area_util,
			string p_habitantes_vivienda,

			string p_area_af_malla_vial_arterial,
			string p_area_af_rondas,
			string p_area_af_zmpa,
			string p_area_af_espacio_publico,
			string p_area_af_servicios_publicos,
			string p_area_af_manejo_dif,
			string p_af_otro,
			string p_area_af_otro,

			string p_unidades_potencial_VIP,
			string p_unidades_potencial_VIS,
			string p_unidades_potencial_no_VIS,
			string p_unidades_ejecutadas_VIP,
			string p_unidades_ejecutadas_VIS,
			string p_unidades_ejecutadas_no_VIS,

			bool p_obligacion_suelo_VIP_en_sitio,
			bool p_obligacion_suelo_VIP_compensacion,
			bool p_obligacion_suelo_VIP_traslado,

			bool p_obligacion_suelo_VIS_en_sitio,
			bool p_obligacion_suelo_VIS_compensacion,
			bool p_obligacion_suelo_VIS_traslado,

			bool p_es_obligacion_construccion_VIP,
			bool p_obligacion_construccion_VIP_en_sitio,
			bool p_obligacion_construccion_VIP_compensacion,
			bool p_obligacion_construccion_VIP_traslado,
			string p_obligacion_construccion_VIP_area,
			string p_obligacion_construccion_VIP_area_ejecutada,
			string p_porc_obligacion_construccion_VIP,

			bool p_es_obligacion_construccion_VIS,
			bool p_obligacion_construccion_VIS_en_sitio,
			bool p_obligacion_construccion_VIS_compensacion,
			bool p_obligacion_construccion_VIS_traslado,
			string p_obligacion_construccion_VIS_area,
			string p_obligacion_construccion_VIS_area_ejecutada,
			string p_porc_obligacion_construccion_VIS,

			string p_decreto_obligacion,
			string p_articulo_obligacion,

			string p_traslado_acto_proyecto_generador,
			string p_traslado_area,
			string p_traslado_acto_proyecto_receptor,
			string p_traslado_localizacion_receptor,
			bool p_traslado_cumple_area_receptor,
			bool p_traslado_cumple_porc_receptor,
			bool p_traslado_es_primera_etapa_receptor,

			string p_compensacion_licencia,
			bool p_compensacion_tiene_certificado_pago,
			bool p_compensacion_cumple_area,
			string p_obs_modalidad_cumplimiento,

			bool p_es_suelo_desarrollo_prioritario,
			string p_decreto_declaratoria,
			string p_articulo_declaratoria,
			string p_fecha_inicio_declaratoria,
			string p_fecha_fin_declaratoria,
			string p_id_estado_declaratoria,
			string p_observacion_declaratoria,

			string p_observacion,
			string p_cod_usu_responsable
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_planp", p_au_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_sdp", p_cod_sdp);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_planp", p_nombre_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_direccion_planp", p_direccion_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_localidad", p_cod_localidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_idupz", p_idupz);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_categoria_planp", p_id_categoria_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_estado_planp", p_id_estado_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_tratamiento", p_id_tipo_tratamiento);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_clasificacion_suelo", p_id_clasificacion_suelo);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_proyecto_asociativo", p_es_proyecto_asociativo);
				oDB.MySQLAddParameter(MySqlCmd, "p_tiene_carta_intencion", p_tiene_carta_intencion);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_firma_carta_intencion", p_fecha_firma_carta_intencion);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_iniciacion_viviendas", p_fecha_iniciacion_viviendas);

				oDB.MySQLAddParameter(MySqlCmd, "p_rango_edificabilidad", p_rango_edificabilidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_areas_zonas", p_areas_zonas);

				oDB.MySQLAddParameter(MySqlCmd, "p_area_bruta", p_area_bruta);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_neta_urbanizable", p_area_neta_urbanizable);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_base_calculo_cesiones", p_area_base_calculo_cesiones);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_util", p_area_util);
				oDB.MySQLAddParameter(MySqlCmd, "p_habitantes_vivienda", p_habitantes_vivienda);

				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_malla_vial_arterial", p_area_af_malla_vial_arterial);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_rondas", p_area_af_rondas);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_zmpa", p_area_af_zmpa);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_espacio_publico", p_area_af_espacio_publico);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_servicios_publicos", p_area_af_servicios_publicos);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_manejo_dif", p_area_af_manejo_dif);
				oDB.MySQLAddParameter(MySqlCmd, "p_af_otro", p_af_otro);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_af_otro", p_area_af_otro);

				oDB.MySQLAddParameter(MySqlCmd, "p_unidades_potencial_VIP", p_unidades_potencial_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidades_potencial_VIS", p_unidades_potencial_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidades_potencial_no_VIS", p_unidades_potencial_no_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidades_ejecutadas_VIP", p_unidades_ejecutadas_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidades_ejecutadas_VIS", p_unidades_ejecutadas_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidades_ejecutadas_no_VIS", p_unidades_ejecutadas_no_VIS);

				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_suelo_VIP_en_sitio", p_obligacion_suelo_VIP_en_sitio);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_suelo_VIP_compensacion", p_obligacion_suelo_VIP_compensacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_suelo_VIP_traslado", p_obligacion_suelo_VIP_traslado);

				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_suelo_VIS_en_sitio", p_obligacion_suelo_VIS_en_sitio);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_suelo_VIS_compensacion", p_obligacion_suelo_VIS_compensacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_suelo_VIS_traslado", p_obligacion_suelo_VIS_traslado);

				oDB.MySQLAddParameter(MySqlCmd, "p_es_obligacion_construccion_VIP", p_es_obligacion_construccion_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIP_en_sitio", p_obligacion_construccion_VIP_en_sitio);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIP_compensacion", p_obligacion_construccion_VIP_compensacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIP_traslado", p_obligacion_construccion_VIP_traslado);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIP_area", p_obligacion_construccion_VIP_area);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIP_area_ejecutada", p_obligacion_construccion_VIP_area_ejecutada);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_obligacion_construccion_VIP", p_porc_obligacion_construccion_VIP);

				oDB.MySQLAddParameter(MySqlCmd, "p_es_obligacion_construccion_VIS", p_es_obligacion_construccion_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIS_en_sitio", p_obligacion_construccion_VIS_en_sitio);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIS_compensacion", p_obligacion_construccion_VIS_compensacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIS_traslado", p_obligacion_construccion_VIS_traslado);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIS_area", p_obligacion_construccion_VIS_area);
				oDB.MySQLAddParameter(MySqlCmd, "p_obligacion_construccion_VIS_area_ejecutada", p_obligacion_construccion_VIS_area_ejecutada);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_obligacion_construccion_VIS", p_porc_obligacion_construccion_VIS);

				oDB.MySQLAddParameter(MySqlCmd, "p_decreto_obligacion", p_decreto_obligacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_articulo_obligacion", p_articulo_obligacion);

				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_acto_proyecto_generador", p_traslado_acto_proyecto_generador);
				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_area", p_traslado_area);
				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_acto_proyecto_receptor", p_traslado_acto_proyecto_receptor);
				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_localizacion_receptor", p_traslado_localizacion_receptor);
				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_cumple_area_receptor", p_traslado_cumple_area_receptor);
				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_cumple_porc_receptor", p_traslado_cumple_porc_receptor);
				oDB.MySQLAddParameter(MySqlCmd, "p_traslado_es_primera_etapa_receptor", p_traslado_es_primera_etapa_receptor);

				oDB.MySQLAddParameter(MySqlCmd, "p_compensacion_licencia", p_compensacion_licencia);
				oDB.MySQLAddParameter(MySqlCmd, "p_compensacion_tiene_certificado_pago", p_compensacion_tiene_certificado_pago);
				oDB.MySQLAddParameter(MySqlCmd, "p_compensacion_cumple_area", p_compensacion_cumple_area);
				oDB.MySQLAddParameter(MySqlCmd, "p_obs_modalidad_cumplimiento", p_obs_modalidad_cumplimiento);

				oDB.MySQLAddParameter(MySqlCmd, "p_es_suelo_desarrollo_prioritario", p_es_suelo_desarrollo_prioritario);
				oDB.MySQLAddParameter(MySqlCmd, "p_decreto_declaratoria", p_decreto_declaratoria);
				oDB.MySQLAddParameter(MySqlCmd, "p_articulo_declaratoria", p_articulo_declaratoria);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_inicio_declaratoria", p_fecha_inicio_declaratoria);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_fin_declaratoria", p_fecha_fin_declaratoria);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_estado_declaratoria", p_id_estado_declaratoria);
				oDB.MySQLAddParameter(MySqlCmd, "p_observacion_declaratoria", p_observacion_declaratoria);

				oDB.MySQLAddParameter(MySqlCmd, "p_observacion", p_observacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_responsable", p_cod_usu_responsable);
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

		public string sp_d_planp(string p_au_planp)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_planp", p_au_planp);
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

		~PLANESP_DAL()
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