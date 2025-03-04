using System;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace SqlAlias
{
    public static class Aliases
    {
        public static string Map(string connectionString)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);

                var key = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "x86"
                    ?  @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\MSSQLServer\Client\ConnectTo"
                    :  @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSSQLServer\Client\ConnectTo";

                var newSource = (string)Microsoft.Win32.Registry.GetValue(key, builder.DataSource, null);
                if (newSource != null)
                    builder.DataSource = newSource.Substring(newSource.IndexOf(',') + 1);

                return builder.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to map the SQL Server alias", ex);
            }
        }
    }
}
