using GLOBAL.LOG;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class CONCEPTOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "CONCEPTOS_DAL";

		private readonly clLog oLog = new clLog();
		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clUtil oUtil = new clUtil();

		#region PARAMETROS
		private const string TABLA_CONCEPTOS = "conceptos";

		private const string PARAM_AU_CONCEPTO = "p_au_concepto";
		private const string PARAM_COD_PREDIO_DECLARADO = "p_cod_predio_declarado";
		private const string PARAM_ID_TIPO_CONCEPTO = "p_id_tipo_concepto";
		private const string PARAM_COD_USU_CONCEPTO = "p_cod_usu_concepto";
		private const string PARAM_FECHA_CONCEPTO = "p_fecha_concepto";
		private const string PARAM_ID_ESTADO_CONCEPTO = "p_id_estado_concepto";
		private const string PARAM_OBJETO = "p_objeto";
		private const string PARAM_ANTECEDENTES = "p_antecedentes";
		private const string PARAM_ARGUMENTOS = "p_argumentos";
		private const string PARAM_SD_1 = "p_sd_1";
		private const string PARAM_SD_1_FECHA = "p_sd_1_fecha";
		private const string PARAM_SD_1_T = "p_sd_1_t";
		private const string PARAM_SD_2 = "p_sd_2";
		private const string PARAM_SD_2_FECHA = "p_sd_2_fecha";
		private const string PARAM_ID_ORIGEN_CERTIFICADO_CATASTRAL = "p_id_origen_certificado_catastral";
		private const string PARAM_SD_2_T = "p_sd_2_t";
		private const string PARAM_SD_3 = "p_sd_3";
		private const string PARAM_SD_3_FECHA = "p_sd_3_fecha";
		private const string PARAM_ID_ORIGEN_MATRICULA = "p_id_origen_matricula";
		private const string PARAM_SD_3_T = "p_sd_3_t";
		private const string PARAM_SD_4 = "p_sd_4";
		private const string PARAM_SD_4_FECHA = "p_sd_4_fecha";
		private const string PARAM_SD_4_T = "p_sd_4_t";
		private const string PARAM_SD_5 = "p_sd_5";
		private const string PARAM_SD_5_T = "p_sd_5_t";
		private const string PARAM_SD_OTROS = "p_sd_otros";
		private const string PARAM_SOPORTES = "p_soportes";
		private const string PARAM_CONSIDERACIONES = "p_consideraciones";
		private const string PARAM_CONCLUSIONES = "p_conclusiones";
		private const string PARAM_OBS_CONCEPTO = "p_obs_concepto";
		private const string PARAM_COD_USU_CONCEPTO_REVISA = "p_cod_usu_concepto_revisa";
		private const string PARAM_COD_USU_CONCEPTO_APRUEBA = "p_cod_usu_concepto_aprueba";
		private const string PARAM_COD_USU = "p_cod_usu";

		#endregion

		#region OBJETOS GLOBALES

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;

		private DataTable oDataTable;
		private DataSet oDataSet;

		#endregion

		#region CADENAS DE CONSULTA
		//private string spSelectConceptos = "sp_s_conceptos";
		private string spSelectConceptosCodPredio = "sp_s_conceptos_cod_predio";
		private string spSelectConceptoPlantilla = "sp_s_concepto_plantilla";
		private string spDeleteConcepto = "sp_d_concepto";
		private string spInsertConcepto = "sp_i_concepto";
		private string spUpdateConcepto = "sp_u_concepto";
		#endregion

		public CONCEPTOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;

			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();

			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_conceptos_cod_predio(string CodPredio)
		{
			oDataSet.Clear();
			oDataSet.Reset();

			try
			{
				MySqlConn.Open();

				MySqlCommand MySqlCmd = new MySqlCommand(spSelectConceptosCodPredio, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_COD_PREDIO_DECLARADO, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_COD_PREDIO_DECLARADO].Value = CodPredio;

				MySqlDA.SelectCommand = MySqlCmd;
				MySqlDA.Fill(oDataSet, TABLA_CONCEPTOS);

				MySqlConn.Close();
				return oDataSet;
			}
			catch (Exception Error)
			{
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_s_conceptos_cod_predio");
				MySqlConn.Close();
				return null;
			}
		}
		public DataSet sp_s_concepto_plantilla(string au_concepto)
		{
			oDataSet.Clear();
			oDataSet.Reset();
			try
			{
				MySqlConn.Open();

				MySqlCommand MySqlCmd = new MySqlCommand(spSelectConceptoPlantilla, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_AU_CONCEPTO, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_AU_CONCEPTO].Value = au_concepto;

				MySqlDA.SelectCommand = MySqlCmd;
				MySqlDA.Fill(oDataSet, TABLA_CONCEPTOS);

				MySqlConn.Close();
				return oDataSet;
			}
			catch (Exception Error)
			{
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_s_concepto_plantilla");
				MySqlConn.Close();
				return null;
			}
		}
		public string sp_i_concepto(
			string cod_predio_declarado,
			string id_tipo_concepto,
			string cod_usu_concepto,
			string fecha_concepto,
			string id_estado_concepto,
			string objeto,
			string antecedentes,
			string argumentos,
			string sd_1,
			string sd_1_fecha,
			string sd_1_t,
			string sd_2,
			string sd_2_fecha,
			string id_origen_certificado_catastral,
			string sd_2_t,
			string sd_3,
			string sd_3_fecha,
			string id_origen_matricula,
			string sd_3_t,
			string sd_4,
			string sd_4_fecha,
			string sd_4_t,
			string sd_5,
			string sd_5_t,
			string sd_otros,
			string soportes,
			string consideraciones,
			string conclusiones,
			string obs_concepto,
			string cod_usu_concepto_revisa,
			string cod_usu_concepto_aprueba
			)
		{
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(spInsertConcepto, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_COD_PREDIO_DECLARADO, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_COD_PREDIO_DECLARADO].Value = cod_predio_declarado;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ID_TIPO_CONCEPTO, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_ID_TIPO_CONCEPTO].Value = id_tipo_concepto;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_COD_USU_CONCEPTO, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_COD_USU_CONCEPTO].Value = cod_usu_concepto;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_FECHA_CONCEPTO, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_FECHA_CONCEPTO].Value = oUtil.ConvertToFechaDB(fecha_concepto);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ID_ESTADO_CONCEPTO, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_ID_ESTADO_CONCEPTO].Value = id_estado_concepto;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_OBJETO, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_OBJETO].Value = oUtil.VerificarNull(objeto);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ANTECEDENTES, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_ANTECEDENTES].Value = oUtil.VerificarNull(antecedentes);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ARGUMENTOS, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_ARGUMENTOS].Value = oUtil.VerificarNull(argumentos);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_1, MySqlDbType.Bit));
				MySqlCmd.Parameters[PARAM_SD_1].Value = Convert.ToBoolean(sd_1.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_1_FECHA, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_1_FECHA].Value = oUtil.ConvertToFechaDB(sd_1_fecha);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_1_T, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_1_T].Value = oUtil.VerificarNull(sd_1_t);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_2, MySqlDbType.Bit));
				MySqlCmd.Parameters[PARAM_SD_2].Value = Convert.ToBoolean(sd_2.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_2_FECHA, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_2_FECHA].Value = oUtil.ConvertToFechaDB(sd_2_fecha);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ID_ORIGEN_CERTIFICADO_CATASTRAL, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_ID_ORIGEN_CERTIFICADO_CATASTRAL].Value = oUtil.VerificarNull(id_origen_certificado_catastral);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_2_T, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_2_T].Value = oUtil.VerificarNull(sd_2_t);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_3, MySqlDbType.Bit));
				MySqlCmd.Parameters[PARAM_SD_3].Value = Convert.ToBoolean(sd_3.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_3_FECHA, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_3_FECHA].Value = oUtil.ConvertToFechaDB(sd_3_fecha);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ID_ORIGEN_MATRICULA, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_ID_ORIGEN_MATRICULA].Value = oUtil.VerificarNull(id_origen_matricula);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_3_T, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_3_T].Value = oUtil.VerificarNull(sd_3_t);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_4, MySqlDbType.Bit));
				MySqlCmd.Parameters[PARAM_SD_4].Value = Convert.ToBoolean(sd_4.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_4_FECHA, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_4_FECHA].Value = oUtil.ConvertToFechaDB(sd_4_fecha);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_4_T, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_4_T].Value = oUtil.VerificarNull(sd_4_t);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_5, MySqlDbType.Bit));
				MySqlCmd.Parameters[PARAM_SD_5].Value = Convert.ToBoolean(sd_5.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_5_T, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_5_T].Value = oUtil.VerificarNull(sd_5_t);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_OTROS, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_OTROS].Value = oUtil.VerificarNull(sd_otros);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SOPORTES, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SOPORTES].Value = oUtil.VerificarNull(soportes);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_CONSIDERACIONES, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_CONSIDERACIONES].Value = oUtil.VerificarNull(consideraciones);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_CONCLUSIONES, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_CONCLUSIONES].Value = oUtil.VerificarNull(conclusiones);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_OBS_CONCEPTO, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_OBS_CONCEPTO].Value = oUtil.VerificarNull(obs_concepto);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_COD_USU_CONCEPTO_REVISA, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_COD_USU_CONCEPTO_REVISA].Value = oUtil.VerificarNull(cod_usu_concepto_revisa);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_COD_USU_CONCEPTO_APRUEBA, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_COD_USU_CONCEPTO_APRUEBA].Value = oUtil.VerificarNull(cod_usu_concepto_aprueba);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_COD_USU, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_COD_USU].Value = oVar.prUserCod.ToString();

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
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_i_concepto");
				return "ERR12";
			}
		}
		public string sp_u_concepto(
			string au_concepto,
			string id_tipo_concepto,
			string cod_usu_concepto,
			string fecha_concepto,
			string id_estado_concepto,
			string objeto,
			string antecedentes,
			string argumentos,
			string sd_1,
			string sd_1_fecha,
			string sd_1_t,
			string sd_2,
			string sd_2_fecha,
			string id_origen_certificado_catastral,
			string sd_2_t,
			string sd_3,
			string sd_3_fecha,
			string id_origen_matricula,
			string sd_3_t,
			string sd_4,
			string sd_4_fecha,
			string sd_4_t,
			string sd_5,
			string sd_5_t,
			string sd_otros,
			string soportes,
			string consideraciones,
			string conclusiones,
			string obs_concepto,
			string cod_usu_concepto_revisa,
			string cod_usu_concepto_aprueba
			)
		{
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(spUpdateConcepto, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_AU_CONCEPTO, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_AU_CONCEPTO].Value = au_concepto;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ID_TIPO_CONCEPTO, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_ID_TIPO_CONCEPTO].Value = id_tipo_concepto;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_COD_USU_CONCEPTO, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_COD_USU_CONCEPTO].Value = cod_usu_concepto;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_FECHA_CONCEPTO, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_FECHA_CONCEPTO].Value = oUtil.ConvertToFechaDB(fecha_concepto);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ID_ESTADO_CONCEPTO, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_ID_ESTADO_CONCEPTO].Value = id_estado_concepto;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_OBJETO, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_OBJETO].Value = oUtil.VerificarNull(objeto);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ANTECEDENTES, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_ANTECEDENTES].Value = oUtil.VerificarNull(antecedentes);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ARGUMENTOS, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_ARGUMENTOS].Value = oUtil.VerificarNull(argumentos);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_1, MySqlDbType.Bit));
				MySqlCmd.Parameters[PARAM_SD_1].Value = Convert.ToBoolean(sd_1.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_1_FECHA, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_1_FECHA].Value = oUtil.ConvertToFechaDB(sd_1_fecha);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_1_T, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_1_T].Value = oUtil.VerificarNull(sd_1_t);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_2, MySqlDbType.Bit));
				MySqlCmd.Parameters[PARAM_SD_2].Value = Convert.ToBoolean(sd_2.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_2_FECHA, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_2_FECHA].Value = oUtil.ConvertToFechaDB(sd_2_fecha);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ID_ORIGEN_CERTIFICADO_CATASTRAL, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_ID_ORIGEN_CERTIFICADO_CATASTRAL].Value = oUtil.VerificarNull(id_origen_certificado_catastral);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_2_T, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_2_T].Value = oUtil.VerificarNull(sd_2_t);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_3, MySqlDbType.Bit));
				MySqlCmd.Parameters[PARAM_SD_3].Value = Convert.ToBoolean(sd_3.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_3_FECHA, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_3_FECHA].Value = oUtil.ConvertToFechaDB(sd_3_fecha);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_ID_ORIGEN_MATRICULA, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_ID_ORIGEN_MATRICULA].Value = oUtil.VerificarNull(id_origen_matricula);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_3_T, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_3_T].Value = oUtil.VerificarNull(sd_3_t);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_4, MySqlDbType.Bit));
				MySqlCmd.Parameters[PARAM_SD_4].Value = Convert.ToBoolean(sd_4.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_4_FECHA, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_4_FECHA].Value = oUtil.ConvertToFechaDB(sd_4_fecha);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_4_T, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_4_T].Value = oUtil.VerificarNull(sd_4_t);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_5, MySqlDbType.Bit));
				MySqlCmd.Parameters[PARAM_SD_5].Value = Convert.ToBoolean(sd_5.ToString());

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_5_T, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_5_T].Value = oUtil.VerificarNull(sd_5_t);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SD_OTROS, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SD_OTROS].Value = oUtil.VerificarNull(sd_otros);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_SOPORTES, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_SOPORTES].Value = oUtil.VerificarNull(soportes);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_CONSIDERACIONES, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_CONSIDERACIONES].Value = oUtil.VerificarNull(consideraciones);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_CONCLUSIONES, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_CONCLUSIONES].Value = oUtil.VerificarNull(conclusiones);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_OBS_CONCEPTO, MySqlDbType.VarChar));
				MySqlCmd.Parameters[PARAM_OBS_CONCEPTO].Value = oUtil.VerificarNull(obs_concepto);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_COD_USU_CONCEPTO_REVISA, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_COD_USU_CONCEPTO_REVISA].Value = oUtil.VerificarNull(cod_usu_concepto_revisa);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_COD_USU_CONCEPTO_APRUEBA, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_COD_USU_CONCEPTO_APRUEBA].Value = oUtil.VerificarNull(cod_usu_concepto_aprueba);

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_COD_USU, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_COD_USU].Value = oVar.prUserCod.ToString();

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
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_u_concepto");
				return "ERR12";
			}
		}
		public string sp_d_concepto(string au_concepto)
		{
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(spDeleteConcepto, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter(PARAM_AU_CONCEPTO, MySqlDbType.Int16));
				MySqlCmd.Parameters[PARAM_AU_CONCEPTO].Value = au_concepto;

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
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "sp_d_concepto");
				return "ERR12";
			}
		}

		#region-----DISPOSE
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~CONCEPTOS_DAL()
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