using GLOBAL.LOG;
using GLOBAL.UTIL;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Globalization;

namespace GLOBAL.DB
{
    public class clDB : IDisposable
	{
		private const string _SOURCEPAGE = "clDB";
		private MySqlDataAdapter MySqlDA;
		private DataSet oDataSet;

		private readonly clUtil oUtil = new clUtil();
		private readonly clLog oLog = new clLog();

		public void MySQLAddParameterString(MySqlCommand MySqlCmd, string p_nombre, string p_valor, string tipo)
		{
			MySqlDbType MySQLType = MySqlDbType.Int16;
			string valor = "";
			bool valor_bit = false;
			byte[] valor_byte = null;

			if (tipo == "texto" || tipo == "t")
			{
				MySQLType = MySqlDbType.VarChar;
				valor = oUtil.VerificarNull(p_valor);
			}
			else if (tipo == "fecha" || tipo == "f")
			{
				MySQLType = MySqlDbType.VarChar;
				valor = oUtil.ConvertToFechaDB(p_valor);
			}
			else if (tipo == "entero" || tipo == "e")
			{
				MySQLType = MySqlDbType.Int16;
				valor = oUtil.VerificarNull(p_valor);
			}
			else if (tipo == "decimal" || tipo == "d")
			{
				MySQLType = MySqlDbType.Decimal;
				valor = oUtil.VerificarDec(oUtil.VerificarNull(p_valor));
			}
			else if (tipo == "bit" || tipo == "b")
			{
				MySQLType = MySqlDbType.Bit;
				if (p_valor == "True")
					valor_bit = true;
			}
			else if (tipo == "pass")
			{
				MySQLType = MySqlDbType.Binary;
				valor_byte = oUtil.HashPW(p_valor);
			}

			MySqlCmd.Parameters.Add(new MySqlParameter(p_nombre, MySQLType));
			if (tipo == "bit" || tipo == "b")
				MySqlCmd.Parameters[p_nombre].Value = valor_bit;
			else if (tipo == "pass")
				MySqlCmd.Parameters[p_nombre].Value = valor_byte;
			else
				MySqlCmd.Parameters[p_nombre].Value = valor;
		}
		public void MySQLAddParameter(MySqlCommand MySqlCmd, string p_nombre, object p_valor, string tipo = "0")
		{
			MySqlDbType MySQLType = MySqlDbType.Int16;
			string valor = "";
			bool valor_bit = false;

			if (p_valor == null)
			{
				MySqlCmd.Parameters.Add(new MySqlParameter(p_nombre, MySqlDbType.String));
				MySqlCmd.Parameters[p_nombre].Value = DBNull.Value;
				return;
			}

			if (tipo == "pass")
			{
				MySqlCmd.Parameters.Add(new MySqlParameter(p_nombre, MySqlDbType.Binary));
				MySqlCmd.Parameters[p_nombre].Value = oUtil.HashPW(p_valor.ToString());
				return;
			}

			if (p_valor.GetType() == typeof(DateTime))
			{
				MySQLType = MySqlDbType.VarChar;
				valor = oUtil.ConvertToFechaDB(p_valor.ToString());
			}
			else if (p_valor.GetType() == typeof(Int16) || p_valor.GetType() == typeof(Int32) || p_valor.GetType() == typeof(Int64))
			{
				MySQLType = MySqlDbType.Int16;
				valor = oUtil.VerificarNull(p_valor.ToString());
			}
			else if (p_valor.ToString().StartsWith("(int)"))
			{
				MySQLType = MySqlDbType.Int16;
				valor = oUtil.VerificarNull(p_valor.ToString().Substring(5));
			}
			else if (p_valor.GetType() == typeof(Decimal))
			{
				MySQLType = MySqlDbType.Decimal;
				valor = oUtil.VerificarDec(oUtil.VerificarNull(p_valor.ToString()));
			}
			else if (p_valor.ToString().StartsWith("(dec)") || p_valor.ToString().StartsWith("(per)"))
			{
				MySQLType = MySqlDbType.Decimal;
				valor = oUtil.VerificarDec(oUtil.VerificarNull(p_valor.ToString().Substring(5)));
			}
			else if (p_valor.GetType() == typeof(Boolean))
			{
				MySQLType = MySqlDbType.Bit;
				if (p_valor.ToString() == "True")
					valor_bit = true;
			}
			else if (p_valor.GetType() == typeof(string))
			{
				MySQLType = MySqlDbType.VarChar;
				valor = oUtil.VerificarNull(p_valor.ToString());
			}

			MySqlCmd.Parameters.Add(new MySqlParameter(p_nombre, MySQLType));
			if (p_valor.GetType() == typeof(Boolean))
				MySqlCmd.Parameters[p_nombre].Value = valor_bit;
			else
				MySqlCmd.Parameters[p_nombre].Value = valor;
		}
		public void MySQLAddParameterReturn(MySqlCommand MySqlCmd)
		{
			MySqlParameter MySqlParam = MySqlCmd.Parameters.Add(new MySqlParameter("p_Result", MySqlDbType.VarChar));
			MySqlParam.Direction = ParameterDirection.Output;
		}
		public DataSet MySQLExecuteSPSelect(MySqlCommand MySqlCmd, string tabla)
		{
			try
			{
				MySqlDA = new MySqlDataAdapter();
				oDataSet = new DataSet();
				oDataSet.Clear();
				oDataSet.Reset();
				MySqlCmd.Connection.Open();
				MySqlDA.SelectCommand = MySqlCmd;
				MySqlDA.Fill(oDataSet, tabla);
				MySqlCmd.Connection.Close();
				return oDataSet;
			}
			catch (Exception Error)
			{
				MySqlCmd.Connection.Close();
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "MySQLExecuteSPSelect: " + MySqlCmd.CommandText);
				return null;
			}
		}
		public void MySQLExecuteSP(MySqlCommand MySqlCmd)
		{
			try
			{
				MySqlCmd.Connection.Open();
				MySqlCmd.ExecuteNonQuery();
				MySqlCmd.Connection.Close();
			}
			catch (Exception Error)
			{
				MySqlCmd.Connection.Close();
				oLog.RegistrarLogError(Error, _SOURCEPAGE, "MySQLExecuteSP: " + MySqlCmd.CommandText);
			}
		}
		public void MySQLSPError(Exception Error, string source, string sp)
		{
			oLog.RegistrarLogError(Error, source, sp);
		}
		public void Dispose()
		{
			MySqlDA.Dispose();
			MySqlDA = null;

			oDataSet.Dispose();
			oDataSet = null;
		}
	}
}
