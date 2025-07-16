using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class PLANESPMANZANAS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PLANESPMANZANAS_DAL";
		private const string TABLA_PLANESPMANZANAS = "PlanesPManzanas";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public PLANESPMANZANAS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_planesp_manzanas_cod_planp(string p_cod_planp)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_planp", p_cod_planp);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PLANESPMANZANAS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_i_planp_manzana(
			string p_cod_planp,
			string p_unidad_gestion,
			string p_manzana,
			string p_area_manzana,
			string p_id_uso_manzana,
			string p_porc_ejecutado,
			string p_fecha_fin,
			string p_UP_VIP,
			string p_UP_VIS,
			string p_UP_no_VIS,
			string p_UE_VIP,
			string p_UE_VIS,
			string p_UE_no_VIS,
			string p_area_multiple_VIP,
			string p_area_multiple_VIS,
			string p_area_multiple_no_VIS,
			string p_area_multiple_afectas,
			string p_area_multiple_comercio,
			string p_area_multiple_comercio_y_servicios,
			string p_area_multiple_dotacional,
			string p_area_multiple_industria,
			string p_area_multiple_industria_y_servicios,
			string p_area_multiple_servicios,
			bool p_es_obligacion_VIS,
			string p_porc_area_obligacion_VIS,
			bool p_es_obligacion_VIP,
			string p_porc_area_obligacion_VIP,
			bool p_es_obligacion_primera_etapa,
			bool p_es_declarado,
			string p_cod_planp_licencia,
			string p_observacion
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_planp", p_cod_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidad_gestion", p_unidad_gestion);
				oDB.MySQLAddParameter(MySqlCmd, "p_manzana", p_manzana);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_manzana", p_area_manzana);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_uso_manzana", p_id_uso_manzana);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_ejecutado", p_porc_ejecutado);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_fin", p_fecha_fin);
				oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIP", p_UP_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIS", p_UP_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_UP_no_VIS", p_UP_no_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_UE_VIP", p_UE_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_UE_VIS", p_UE_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_UE_no_VIS", p_UE_no_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_VIP", p_area_multiple_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_VIS", p_area_multiple_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_no_VIS", p_area_multiple_no_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_afectas", p_area_multiple_afectas);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_comercio", p_area_multiple_comercio);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_comercio_y_servicios", p_area_multiple_comercio_y_servicios);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_dotacional", p_area_multiple_dotacional);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_industria", p_area_multiple_industria);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_industria_y_servicios", p_area_multiple_industria_y_servicios);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_servicios", p_area_multiple_servicios);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_obligacion_VIS", p_es_obligacion_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_area_obligacion_VIS", p_porc_area_obligacion_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_obligacion_VIP", p_es_obligacion_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_area_obligacion_VIP", p_porc_area_obligacion_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_obligacion_primera_etapa", p_es_obligacion_primera_etapa);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_declarado", p_es_declarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_planp_licencia", p_cod_planp_licencia);
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
		public string sp_u_planp_manzana(
			string p_au_planp_manzana,
			string p_unidad_gestion,
			string p_manzana,
			string p_area_manzana,
			string p_id_uso_manzana,
			string p_porc_ejecutado,
			string p_fecha_fin,
			string p_UP_VIP,
			string p_UP_VIS,
			string p_UP_no_VIS,
			string p_UE_VIP,
			string p_UE_VIS,
			string p_UE_no_VIS,
			string p_area_multiple_VIP,
			string p_area_multiple_VIS,
			string p_area_multiple_no_VIS,
			string p_area_multiple_afectas,
			string p_area_multiple_comercio,
			string p_area_multiple_comercio_y_servicios,
			string p_area_multiple_dotacional,
			string p_area_multiple_industria,
			string p_area_multiple_industria_y_servicios,
			string p_area_multiple_servicios,
			bool p_es_obligacion_VIS,
			string p_porc_area_obligacion_VIS,
			bool p_es_obligacion_VIP,
			string p_porc_area_obligacion_VIP,
			bool p_es_obligacion_primera_etapa,
			bool p_es_declarado,
			string p_cod_planp_licencia,
			string p_observacion
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_planp_manzana", p_au_planp_manzana);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidad_gestion", p_unidad_gestion);
				oDB.MySQLAddParameter(MySqlCmd, "p_manzana", p_manzana);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_manzana", p_area_manzana);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_uso_manzana", p_id_uso_manzana);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_ejecutado", p_porc_ejecutado);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_fin", p_fecha_fin);
				oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIP", p_UP_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_UP_VIS", p_UP_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_UP_no_VIS", p_UP_no_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_UE_VIP", p_UE_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_UE_VIS", p_UE_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_UE_no_VIS", p_UE_no_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_VIP", p_area_multiple_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_VIS", p_area_multiple_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_no_VIS", p_area_multiple_no_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_afectas", p_area_multiple_afectas);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_comercio", p_area_multiple_comercio);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_comercio_y_servicios", p_area_multiple_comercio_y_servicios);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_dotacional", p_area_multiple_dotacional);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_industria", p_area_multiple_industria);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_industria_y_servicios", p_area_multiple_industria_y_servicios);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_multiple_servicios", p_area_multiple_servicios);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_obligacion_VIS", p_es_obligacion_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_area_obligacion_VIS", p_porc_area_obligacion_VIS);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_obligacion_VIP", p_es_obligacion_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_area_obligacion_VIP", p_porc_area_obligacion_VIP);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_obligacion_primera_etapa", p_es_obligacion_primera_etapa);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_declarado", p_es_declarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_planp_licencia", p_cod_planp_licencia);
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
		public string sp_d_planp_manzana(string p_au_planp_manzana)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_planp_manzana", p_au_planp_manzana);
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

		~PLANESPMANZANAS_DAL()
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

				oDataTable.Dispose();
				oDataTable = null;
			}
		}
		#endregion
	}
}