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
    public class VISITAS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "VISITAS_DAL";

		private readonly clLog oLog = new clLog();
		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clUtil oUtil = new clUtil();
		private readonly clDB oDB = new clDB();

		private const string TABLA_VISITAS = "visitas";

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;
		
		public VISITAS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_visitas_cod_predio(string p_cod_predio_declarado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_declarado", p_cod_predio_declarado);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_VISITAS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_visita_plantilla(string p_au_visita)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_visita", p_au_visita);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_VISITAS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_i_visita(
			string p_cod_predio_declarado,
			string p_id_tipo_visita,
			string p_cod_usu_visita,
			string p_fecha_visita,
			string p_id_estado_visita,
			string p_lic_construccion,
			string p_lic_urbanismo,
			string p_lic_sin_valla,
			string p_id_ocupacion_visita,
			string p_act_viv_unifamiliar,
			string p_act_viv_multifamiliar,
			string p_act_parqueadero,
			string p_act_comercio,
			string p_act_otro,
			string p_ent_viv_unifamiliar,
			string p_ent_viv_multifamiliar,
			string p_ent_comercio,
			string p_ent_industria,
			string p_ent_otro,
			string p_acc_via_vehicular,
			string p_acc_via_peatonal,
			string p_acc_ninguna,
			string p_id_pendiente_lote,
			string p_id_pendiente_ladera,
			string p_cob_vivienda,
			string p_cob_pastos,
			string p_cob_rastrojo,
			string p_cob_bosque,
			string p_cob_sin_cobertura,
			string p_cob_otro,
			string p_inest_fisuras,
			string p_inest_fracturas,
			string p_inest_escarpe,
			string p_inest_corona,
			string p_inest_depositos,
			string p_inest_ninguna,
			string p_inest_otro,
			string p_agua_interna,
			string p_agua_limitrofe,
			string p_agua_amortiguacion,
			string p_agua_obras,
			string p_agua_otro,
			string p_geom_depositos,
			string p_geom_llenos,
			string p_geom_escarpes,
			string p_geom_llanuras,
			string p_geom_otro,
			string p_id_uso_lote,
			string p_uso_entorno,
			string p_id_estado_vias_internas,
			string p_id_estado_vias_perimetrales,
			string p_tiene_servidumbres,
			string p_construccion_existente_lote,
			string p_id_estado_construccion_existente_lote,
			string p_uso_construccion_lote,
			string p_construccion_existente_entorno,
			string p_estado_consolidacion_entorno,
			string p_existe_vivienda_lote,
			string p_obs_vivienda_lote,
			string p_existe_vivienda_entorno,
			string p_obs_vivienda_entorno,
			string p_nombre_urbanizacion_lote,
			string p_id_pendiente_topografia,
			string p_obs_visita
		)
		{
			oDataSet.Clear();
			oDataSet.Reset();
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cod_predio_declarado", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_cod_predio_declarado"].Value = p_cod_predio_declarado;

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_tipo_visita", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_tipo_visita"].Value = p_id_tipo_visita;

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cod_usu_visita", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_cod_usu_visita"].Value = p_cod_usu_visita;

				MySqlCmd.Parameters.Add(new MySqlParameter("p_fecha_visita", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_fecha_visita"].Value = oUtil.ConvertToFechaDB(p_fecha_visita);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_estado_visita", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_estado_visita"].Value = p_id_estado_visita;

				MySqlCmd.Parameters.Add(new MySqlParameter("p_lic_construccion", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_lic_construccion"].Value = Convert.ToBoolean(p_lic_construccion.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_lic_urbanismo", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_lic_urbanismo"].Value = Convert.ToBoolean(p_lic_urbanismo.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_lic_sin_valla", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_lic_sin_valla"].Value = Convert.ToBoolean(p_lic_sin_valla.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_ocupacion_visita", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_ocupacion_visita"].Value = oUtil.VerificarNull(p_id_ocupacion_visita);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_act_viv_unifamiliar", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_act_viv_unifamiliar"].Value = Convert.ToBoolean(p_act_viv_unifamiliar.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_act_viv_multifamiliar", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_act_viv_multifamiliar"].Value = Convert.ToBoolean(p_act_viv_multifamiliar.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_act_parqueadero", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_act_parqueadero"].Value = Convert.ToBoolean(p_act_parqueadero.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_act_comercio", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_act_comercio"].Value = Convert.ToBoolean(p_act_comercio.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_act_otro", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_act_otro"].Value = oUtil.VerificarNull(p_act_otro);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_ent_viv_unifamiliar", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_ent_viv_unifamiliar"].Value = Convert.ToBoolean(p_ent_viv_unifamiliar.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_ent_viv_multifamiliar", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_ent_viv_multifamiliar"].Value = Convert.ToBoolean(p_ent_viv_multifamiliar.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_ent_comercio", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_ent_comercio"].Value = Convert.ToBoolean(p_ent_comercio.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_ent_industria", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_ent_industria"].Value = Convert.ToBoolean(p_ent_industria.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_ent_otro", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_ent_otro"].Value = oUtil.VerificarNull(p_ent_otro);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_acc_via_vehicular", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_acc_via_vehicular"].Value = Convert.ToBoolean(p_acc_via_vehicular.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_acc_via_peatonal", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_acc_via_peatonal"].Value = Convert.ToBoolean(p_acc_via_peatonal.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_acc_ninguna", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_acc_ninguna"].Value = Convert.ToBoolean(p_acc_ninguna.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_pendiente_lote", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_pendiente_lote"].Value = oUtil.VerificarNull(p_id_pendiente_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_pendiente_ladera", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_pendiente_ladera"].Value = oUtil.VerificarNull(p_id_pendiente_ladera);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cob_vivienda", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_cob_vivienda"].Value = Convert.ToBoolean(p_cob_vivienda.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cob_pastos", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_cob_pastos"].Value = Convert.ToBoolean(p_cob_pastos.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cob_rastrojo", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_cob_rastrojo"].Value = Convert.ToBoolean(p_cob_rastrojo.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cob_bosque", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_cob_bosque"].Value = Convert.ToBoolean(p_cob_bosque.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cob_sin_cobertura", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_cob_sin_cobertura"].Value = Convert.ToBoolean(p_cob_sin_cobertura.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cob_otro", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_cob_otro"].Value = oUtil.VerificarNull(p_cob_otro);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_fisuras", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_inest_fisuras"].Value = Convert.ToBoolean(p_inest_fisuras.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_fracturas", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_inest_fracturas"].Value = Convert.ToBoolean(p_inest_fracturas.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_escarpe", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_inest_escarpe"].Value = Convert.ToBoolean(p_inest_escarpe.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_corona", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_inest_corona"].Value = Convert.ToBoolean(p_inest_corona.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_depositos", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_inest_depositos"].Value = Convert.ToBoolean(p_inest_depositos.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_ninguna", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_inest_ninguna"].Value = Convert.ToBoolean(p_inest_ninguna.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_otro", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_inest_otro"].Value = oUtil.VerificarNull(p_inest_otro);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_agua_interna", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_agua_interna"].Value = Convert.ToBoolean(p_agua_interna.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_agua_limitrofe", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_agua_limitrofe"].Value = Convert.ToBoolean(p_agua_limitrofe.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_agua_amortiguacion", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_agua_amortiguacion"].Value = Convert.ToBoolean(p_agua_amortiguacion.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_agua_obras", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_agua_obras"].Value = Convert.ToBoolean(p_agua_obras.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_agua_otro", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_agua_otro"].Value = oUtil.VerificarNull(p_agua_otro);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_geom_depositos", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_geom_depositos"].Value = Convert.ToBoolean(p_geom_depositos.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_geom_llenos", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_geom_llenos"].Value = Convert.ToBoolean(p_geom_llenos.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_geom_escarpes", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_geom_escarpes"].Value = Convert.ToBoolean(p_geom_escarpes.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_geom_llanuras", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_geom_llanuras"].Value = Convert.ToBoolean(p_geom_llanuras.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_geom_otro", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_geom_otro"].Value = oUtil.VerificarNull(p_geom_otro);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_uso_lote", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_uso_lote"].Value = oUtil.VerificarNull(p_id_uso_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_uso_entorno", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_uso_entorno"].Value = oUtil.VerificarNull(p_uso_entorno);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_estado_vias_internas", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_estado_vias_internas"].Value = oUtil.VerificarNull(p_id_estado_vias_internas);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_estado_vias_perimetrales", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_estado_vias_perimetrales"].Value = oUtil.VerificarNull(p_id_estado_vias_perimetrales);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_tiene_servidumbres", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_tiene_servidumbres"].Value = Convert.ToBoolean(p_tiene_servidumbres.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_construccion_existente_lote", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_construccion_existente_lote"].Value = oUtil.VerificarNull(p_construccion_existente_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_estado_construccion_existente_lote", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_estado_construccion_existente_lote"].Value = oUtil.VerificarNull(p_id_estado_construccion_existente_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_uso_construccion_lote", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_uso_construccion_lote"].Value = oUtil.VerificarNull(p_uso_construccion_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_construccion_existente_entorno", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_construccion_existente_entorno"].Value = oUtil.VerificarNull(p_construccion_existente_entorno);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_estado_consolidacion_entorno", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_estado_consolidacion_entorno"].Value = oUtil.VerificarNull(p_estado_consolidacion_entorno);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_existe_vivienda_lote", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_existe_vivienda_lote"].Value = Convert.ToBoolean(p_existe_vivienda_lote.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_obs_vivienda_lote", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_obs_vivienda_lote"].Value = oUtil.VerificarNull(p_obs_vivienda_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_existe_vivienda_entorno", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_existe_vivienda_entorno"].Value = Convert.ToBoolean(p_existe_vivienda_entorno.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_obs_vivienda_entorno", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_obs_vivienda_entorno"].Value = oUtil.VerificarNull(p_obs_vivienda_entorno);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_nombre_urbanizacion_lote", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_nombre_urbanizacion_lote"].Value = oUtil.VerificarNull(p_nombre_urbanizacion_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_pendiente_topografia", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_pendiente_topografia"].Value = oUtil.VerificarNull(p_id_pendiente_topografia);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_obs_visita", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_obs_visita"].Value = oUtil.VerificarNull(p_obs_visita);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cod_usu", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_cod_usu"].Value = oVar.prUserCod.ToString();

				MySqlParameter MySqlParam = MySqlCmd.Parameters.Add(new MySqlParameter("p_Result", MySqlDbType.VarChar));
				MySqlParam.Direction = ParameterDirection.Output;

				MySqlDA.InsertCommand = MySqlCmd;
				MySqlDA.InsertCommand.Connection.Open();
				MySqlDA.InsertCommand.ExecuteNonQuery();
				MySqlDA.InsertCommand.Connection.Close();

				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				MySqlDA.InsertCommand.Connection.Close();
				oLog.RegistrarLogError(Error, _SOURCEPAGE, sp);
				return "ERR12";
			}
		}
		public string sp_u_visita(
			string p_au_visita,
			string p_id_tipo_visita,
			string p_cod_usu_visita,
			string p_fecha_visita,
			string p_id_estado_visita,
			string p_lic_construccion,
			string p_lic_urbanismo,
			string p_lic_sin_valla,
			string p_id_ocupacion_visita,
			string p_act_viv_unifamiliar,
			string p_act_viv_multifamiliar,
			string p_act_parqueadero,
			string p_act_comercio,
			string p_act_otro,
			string p_ent_viv_unifamiliar,
			string p_ent_viv_multifamiliar,
			string p_ent_comercio,
			string p_ent_industria,
			string p_ent_otro,
			string p_acc_via_vehicular,
			string p_acc_via_peatonal,
			string p_acc_ninguna,
			string p_id_pendiente_lote,
			string p_id_pendiente_ladera,
			string p_cob_vivienda,
			string p_cob_pastos,
			string p_cob_rastrojo,
			string p_cob_bosque,
			string p_cob_sin_cobertura,
			string p_cob_otro,
			string p_inest_fisuras,
			string p_inest_fracturas,
			string p_inest_escarpe,
			string p_inest_corona,
			string p_inest_depositos,
			string p_inest_ninguna,
			string p_inest_otro,
			string p_agua_interna,
			string p_agua_limitrofe,
			string p_agua_amortiguacion,
			string p_agua_obras,
			string p_agua_otro,
			string p_geom_depositos,
			string p_geom_llenos,
			string p_geom_escarpes,
			string p_geom_llanuras,
			string p_geom_otro,
			string p_id_uso_lote,
			string p_uso_entorno,
			string p_id_estado_vias_internas,
			string p_id_estado_vias_perimetrales,
			string p_tiene_servidumbres,
			string p_construccion_existente_lote,
			string p_id_estado_construccion_existente_lote,
			string p_uso_construccion_lote,
			string p_construccion_existente_entorno,
			string p_estado_consolidacion_entorno,
			string p_existe_vivienda_lote,
			string p_obs_vivienda_lote,
			string p_existe_vivienda_entorno,
			string p_obs_vivienda_entorno,
			string p_nombre_urbanizacion_lote,
			string p_id_pendiente_topografia,
			string p_obs_visita
		)
		{
			oDataSet.Clear();
			oDataSet.Reset();
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter("p_au_visita", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_au_visita"].Value = p_au_visita;

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_tipo_visita", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_tipo_visita"].Value = p_id_tipo_visita;

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cod_usu_visita", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_cod_usu_visita"].Value = p_cod_usu_visita;

				MySqlCmd.Parameters.Add(new MySqlParameter("p_fecha_visita", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_fecha_visita"].Value = oUtil.ConvertToFechaDB(p_fecha_visita);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_estado_visita", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_estado_visita"].Value = p_id_estado_visita;

				MySqlCmd.Parameters.Add(new MySqlParameter("p_lic_construccion", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_lic_construccion"].Value = Convert.ToBoolean(p_lic_construccion.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_lic_urbanismo", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_lic_urbanismo"].Value = Convert.ToBoolean(p_lic_urbanismo.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_lic_sin_valla", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_lic_sin_valla"].Value = Convert.ToBoolean(p_lic_sin_valla.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_ocupacion_visita", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_ocupacion_visita"].Value = oUtil.VerificarNull(p_id_ocupacion_visita);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_act_viv_unifamiliar", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_act_viv_unifamiliar"].Value = Convert.ToBoolean(p_act_viv_unifamiliar.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_act_viv_multifamiliar", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_act_viv_multifamiliar"].Value = Convert.ToBoolean(p_act_viv_multifamiliar.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_act_parqueadero", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_act_parqueadero"].Value = Convert.ToBoolean(p_act_parqueadero.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_act_comercio", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_act_comercio"].Value = Convert.ToBoolean(p_act_comercio.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_act_otro", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_act_otro"].Value = oUtil.VerificarNull(p_act_otro);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_ent_viv_unifamiliar", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_ent_viv_unifamiliar"].Value = Convert.ToBoolean(p_ent_viv_unifamiliar.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_ent_viv_multifamiliar", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_ent_viv_multifamiliar"].Value = Convert.ToBoolean(p_ent_viv_multifamiliar.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_ent_comercio", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_ent_comercio"].Value = Convert.ToBoolean(p_ent_comercio.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_ent_industria", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_ent_industria"].Value = Convert.ToBoolean(p_ent_industria.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_ent_otro", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_ent_otro"].Value = oUtil.VerificarNull(p_ent_otro);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_acc_via_vehicular", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_acc_via_vehicular"].Value = Convert.ToBoolean(p_acc_via_vehicular.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_acc_via_peatonal", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_acc_via_peatonal"].Value = Convert.ToBoolean(p_acc_via_peatonal.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_acc_ninguna", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_acc_ninguna"].Value = Convert.ToBoolean(p_acc_ninguna.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_pendiente_lote", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_pendiente_lote"].Value = oUtil.VerificarNull(p_id_pendiente_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_pendiente_ladera", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_pendiente_ladera"].Value = oUtil.VerificarNull(p_id_pendiente_ladera);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cob_vivienda", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_cob_vivienda"].Value = Convert.ToBoolean(p_cob_vivienda.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cob_pastos", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_cob_pastos"].Value = Convert.ToBoolean(p_cob_pastos.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cob_rastrojo", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_cob_rastrojo"].Value = Convert.ToBoolean(p_cob_rastrojo.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cob_bosque", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_cob_bosque"].Value = Convert.ToBoolean(p_cob_bosque.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cob_sin_cobertura", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_cob_sin_cobertura"].Value = Convert.ToBoolean(p_cob_sin_cobertura.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cob_otro", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_cob_otro"].Value = oUtil.VerificarNull(p_cob_otro);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_fisuras", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_inest_fisuras"].Value = Convert.ToBoolean(p_inest_fisuras.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_fracturas", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_inest_fracturas"].Value = Convert.ToBoolean(p_inest_fracturas.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_escarpe", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_inest_escarpe"].Value = Convert.ToBoolean(p_inest_escarpe.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_corona", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_inest_corona"].Value = Convert.ToBoolean(p_inest_corona.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_depositos", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_inest_depositos"].Value = Convert.ToBoolean(p_inest_depositos.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_ninguna", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_inest_ninguna"].Value = Convert.ToBoolean(p_inest_ninguna.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_inest_otro", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_inest_otro"].Value = oUtil.VerificarNull(p_inest_otro);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_agua_interna", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_agua_interna"].Value = Convert.ToBoolean(p_agua_interna.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_agua_limitrofe", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_agua_limitrofe"].Value = Convert.ToBoolean(p_agua_limitrofe.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_agua_amortiguacion", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_agua_amortiguacion"].Value = Convert.ToBoolean(p_agua_amortiguacion.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_agua_obras", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_agua_obras"].Value = Convert.ToBoolean(p_agua_obras.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_agua_otro", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_agua_otro"].Value = oUtil.VerificarNull(p_agua_otro);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_geom_depositos", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_geom_depositos"].Value = Convert.ToBoolean(p_geom_depositos.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_geom_llenos", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_geom_llenos"].Value = Convert.ToBoolean(p_geom_llenos.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_geom_escarpes", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_geom_escarpes"].Value = Convert.ToBoolean(p_geom_escarpes.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_geom_llanuras", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_geom_llanuras"].Value = Convert.ToBoolean(p_geom_llanuras.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_geom_otro", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_geom_otro"].Value = oUtil.VerificarNull(p_geom_otro);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_uso_lote", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_uso_lote"].Value = oUtil.VerificarNull(p_id_uso_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_uso_entorno", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_uso_entorno"].Value = oUtil.VerificarNull(p_uso_entorno);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_estado_vias_internas", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_estado_vias_internas"].Value = oUtil.VerificarNull(p_id_estado_vias_internas);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_estado_vias_perimetrales", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_estado_vias_perimetrales"].Value = oUtil.VerificarNull(p_id_estado_vias_perimetrales);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_tiene_servidumbres", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_tiene_servidumbres"].Value = Convert.ToBoolean(p_tiene_servidumbres.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_construccion_existente_lote", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_construccion_existente_lote"].Value = oUtil.VerificarNull(p_construccion_existente_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_estado_construccion_existente_lote", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_estado_construccion_existente_lote"].Value = oUtil.VerificarNull(p_id_estado_construccion_existente_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_uso_construccion_lote", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_uso_construccion_lote"].Value = oUtil.VerificarNull(p_uso_construccion_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_construccion_existente_entorno", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_construccion_existente_entorno"].Value = oUtil.VerificarNull(p_construccion_existente_entorno);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_estado_consolidacion_entorno", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_estado_consolidacion_entorno"].Value = oUtil.VerificarNull(p_estado_consolidacion_entorno);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_existe_vivienda_lote", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_existe_vivienda_lote"].Value = Convert.ToBoolean(p_existe_vivienda_lote.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_obs_vivienda_lote", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_obs_vivienda_lote"].Value = oUtil.VerificarNull(p_obs_vivienda_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_existe_vivienda_entorno", MySqlDbType.Bit));
				MySqlCmd.Parameters["p_existe_vivienda_entorno"].Value = Convert.ToBoolean(p_existe_vivienda_entorno.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter("p_obs_vivienda_entorno", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_obs_vivienda_entorno"].Value = oUtil.VerificarNull(p_obs_vivienda_entorno);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_nombre_urbanizacion_lote", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_nombre_urbanizacion_lote"].Value = oUtil.VerificarNull(p_nombre_urbanizacion_lote);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_pendiente_topografia", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_pendiente_topografia"].Value = oUtil.VerificarNull(p_id_pendiente_topografia);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_obs_visita", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_obs_visita"].Value = oUtil.VerificarNull(p_obs_visita);

				MySqlCmd.Parameters.Add(new MySqlParameter("p_cod_usu", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_cod_usu"].Value = oVar.prUserCod.ToString();

				MySqlParameter MySqlParam = MySqlCmd.Parameters.Add(new MySqlParameter("p_Result", MySqlDbType.VarChar));
				MySqlParam.Direction = ParameterDirection.Output;

				MySqlDA.UpdateCommand = MySqlCmd;
				MySqlDA.UpdateCommand.Connection.Open();
				MySqlDA.UpdateCommand.ExecuteNonQuery();
				MySqlDA.UpdateCommand.Connection.Close();

				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				MySqlDA.UpdateCommand.Connection.Close();
				oLog.RegistrarLogError(Error, _SOURCEPAGE, sp);
				return "ERR12";
			}
		}
		public string sp_d_visita(string p_au_visita)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_visita", p_au_visita);
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

		~VISITAS_DAL()
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

				oDataTable.Dispose();
				oDataTable = null;
			}
		}
		#endregion
	}
}