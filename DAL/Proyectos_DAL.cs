using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class PROYECTOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PROYECTOS_DAL";
		private const string TABLA_PROYECTOS = "PlanesP";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public PROYECTOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}

		public DataSet sp_s_proyectos_nombre(string p_nombre_proyecto, string p_tipo_proyecto, string p_usuario)
		{
			string sp = "sp_s_proyectos_nombre";
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_proyecto", p_nombre_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_tipo_proyecto", p_tipo_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_usuario", p_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PROYECTOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		#region sp_i_proyecto
		public string sp_i_proyecto( string p_nombre_proyecto, string p_cod_planp, string p_chip, string p_direccion_proyecto, string p_cod_localidad,
			string p_idupz, string p_id_origen_proyecto, string p_id_clasificacion_suelo, string p_id_destino_catastral, string p_id_tratamiento_urbanistico,
			string p_id_instrumento_gestion, string p_id_instrumento_desarrollo, string p_porc_SE_total, string p_id_estado_proyecto, string p_id_resultado_proyecto,
			string p_areas_zonas, string p_area_bruta, string p_area_neta_urbanizable, string p_area_util, string p_UP_VIP, string p_UP_VIS, string p_UP_no_VIS,
			string p_UE_VIP, string p_UE_VIS, string p_UE_no_VIS, string p_empleos, string p_inversion, string p_fecha_inicio_ventas, string p_fecha_inicio_obras,
			string p_observacion, string p_cod_usu_responsable)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_proyecto", p_nombre_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_planp", p_cod_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_chip", p_chip);
				oDB.MySQLAddParameter(MySqlCmd, "p_direccion_proyecto", p_direccion_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_localidad", p_cod_localidad); 

				oDB.MySQLAddParameter(MySqlCmd, "p_idupz", p_idupz);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_origen_proyecto", p_id_origen_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_clasificacion_suelo", p_id_clasificacion_suelo);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_destino_catastral", p_id_destino_catastral);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tratamiento_urbanistico", p_id_tratamiento_urbanistico);
				
				oDB.MySQLAddParameter(MySqlCmd, "p_id_instrumento_gestion", p_id_instrumento_gestion);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_instrumento_desarrollo", p_id_instrumento_desarrollo);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_SE_total", p_porc_SE_total);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_estado_proyecto", p_id_estado_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_resultado_proyecto", p_id_resultado_proyecto);
				
				oDB.MySQLAddParameter(MySqlCmd, "p_areas_zonas", p_areas_zonas);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_bruta", p_area_bruta);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_neta_urbanizable", p_area_neta_urbanizable);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_util", p_area_util);
				oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIP", p_UP_VIP);
				
				oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIS", p_UP_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_UP_no_VIS", p_UP_no_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_UE_VIP", p_UE_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_UE_VIS", p_UE_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_UE_no_VIS", p_UE_no_VIS);
				
				oDB.MySQLAddParameter(MySqlCmd, "p_empleos", p_empleos);
				oDB.MySQLAddParameter(MySqlCmd, "p_inversion", p_inversion);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_inicio_ventas", p_fecha_inicio_ventas);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_inicio_obras", p_fecha_inicio_obras);
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
		#region sp_u_proyecto
		public string sp_u_proyecto(string p_au_proyecto, string p_nombre_proyecto, string p_cod_planp, string p_chip, string p_direccion_proyecto, 
			string p_cod_localidad, string p_idupz, string p_id_origen_proyecto, string p_id_clasificacion_suelo, string p_id_destino_catastral, 
			string p_id_tratamiento_urbanistico, string p_id_instrumento_gestion, string p_id_instrumento_desarrollo, string p_porc_SE_total, 
			string p_id_estado_proyecto, string p_id_resultado_proyecto, string p_areas_zonas, string p_area_bruta, string p_area_neta_urbanizable, 
			string p_area_util, string p_UP_VIP, string p_UP_VIS, string p_UP_no_VIS, string p_UE_VIP, string p_UE_VIS, string p_UE_no_VIS, string p_empleos, 
			string p_inversion, string p_fecha_inicio_ventas, string p_fecha_inicio_obras, string p_observacion, string p_cod_usu_responsable, string p_ruta_archivo,
			string p_id_como_cumple, string p_fecha_cumple, string p_area_cumple)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_proyecto", p_au_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_proyecto", p_nombre_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_planp", p_cod_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_chip", p_chip);
				oDB.MySQLAddParameter(MySqlCmd, "p_direccion_proyecto", p_direccion_proyecto);

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_localidad", p_cod_localidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_idupz", p_idupz);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_origen_proyecto", p_id_origen_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_clasificacion_suelo", p_id_clasificacion_suelo);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_destino_catastral", p_id_destino_catastral);
				
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tratamiento_urbanistico", p_id_tratamiento_urbanistico);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_instrumento_gestion", p_id_instrumento_gestion);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_instrumento_desarrollo", p_id_instrumento_desarrollo);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_SE_total", p_porc_SE_total);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_estado_proyecto", p_id_estado_proyecto);
				
				oDB.MySQLAddParameter(MySqlCmd, "p_id_resultado_proyecto", p_id_resultado_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_areas_zonas", p_areas_zonas);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_bruta", p_area_bruta);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_neta_urbanizable", p_area_neta_urbanizable);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_util", p_area_util);
				
				oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIP", p_UP_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIS", p_UP_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_UP_no_VIS", p_UP_no_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_UE_VIP", p_UE_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_UE_VIS", p_UE_VIS);
				
				oDB.MySQLAddParameter(MySqlCmd, "p_UE_no_VIS", p_UE_no_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_empleos", p_empleos);
				oDB.MySQLAddParameter(MySqlCmd, "p_inversion", p_inversion);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_inicio_ventas", p_fecha_inicio_ventas);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_inicio_obras", p_fecha_inicio_obras);
				
				oDB.MySQLAddParameter(MySqlCmd, "p_observacion", p_observacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_responsable", p_cod_usu_responsable);
				oDB.MySQLAddParameter(MySqlCmd, "p_ruta_archivo", p_ruta_archivo);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_como_cumple", p_id_como_cumple);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_cumple",p_fecha_cumple);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_cumple", p_area_cumple);
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
		public string sp_d_proyecto(string p_au_proyecto)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_proyecto", p_au_proyecto);
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

		public string sp_v_proyecto_validar(string p_au_proyecto)
        {
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_proyecto", p_au_proyecto);

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
		// Metodo para el manejo del GC
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~PROYECTOS_DAL()
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