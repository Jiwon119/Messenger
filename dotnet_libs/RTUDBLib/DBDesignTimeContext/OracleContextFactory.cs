using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
//using PublicDBLib.MariaDB;
using Oracle.ManagedDataAccess.Client;
using RTUDBLib.DBContext;

namespace RTUDBLib
{
    public class OracleContextFactory : IDesignTimeDbContextFactory<OracleContext>
    {
        public OracleContext CreateDbContext(string[] args)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<OracleContext>();

            var connBuilder = new OracleConnectionStringBuilder();
            connBuilder.UserID = "WMSDB";
            connBuilder.Password = "oracle";
            connBuilder.DataSource = "192.168.0.31:49150";
            //connBuilder.DataSource = "192.168.0.170:49156";
            //connBuilder.DataSource = "192.168.0.170:49154";
            connBuilder.PersistSecurityInfo = true;

            string connStr = connBuilder.ConnectionString;

            dbContextBuilder.UseOracle(connStr, opt => opt.UseOracleSQLCompatibility("11"));
            return new OracleContext(dbContextBuilder.Options);
        }
    }

    /*
     string usr = EnvConfig.GetOrDefaultEnvValue("ORACLE_DB_USER", "WMSDB");
                    string pw = EnvConfig.GetOrDefaultEnvValue("ORACLE_DB_PW", "oracle");
                    string ip = EnvConfig.GetOrDefaultEnvValue("ORACLE_DB_IP", "192.168.0.150");
                    string port = EnvConfig.GetOrDefaultEnvValue("ORACLE_DB_PORT", "49154");

                    var connBuilder = new OracleConnectionStringBuilder();
                    connBuilder.UserID = usr;
                    connBuilder.Password = pw;
                    connBuilder.DataSource = $"{ip}:{port}";
                    connBuilder.PersistSecurityInfo = true;

                    string connStr = connBuilder.ConnectionString;
                    optionsBuilder.UseOracle(connStr, opt => opt.UseOracleSQLCompatibility("11"));
     */
}
