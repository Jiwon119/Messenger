using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace PublicDBLib
{
    public class MariaDbContextFactory : IDesignTimeDbContextFactory<MariaContext>
    {
        public MariaContext CreateDbContext(string[] args)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<MariaContext>();

            MySqlConnectionStringBuilder connBuilder = new MySqlConnectionStringBuilder();

            // 170번 컴퓨터
            //connBuilder.Server = "192.168.0.170";
            //connBuilder.Port = 9907;
            //connBuilder.UserID = "root";
            //connBuilder.Password = "qrqvud3";
            //connBuilder.Database = "PublicData";
            //connBuilder.CharacterSet = "utf8";

            // local 컴퓨터
            connBuilder.Server = "192.168.0.31";
            connBuilder.Port = 9907;
            connBuilder.UserID = "root";
            connBuilder.Password = "qrqvud3";
            connBuilder.Database = "PublicData";
            connBuilder.CharacterSet = "utf8";

            string connStr = connBuilder.ConnectionString;

            //MariaDbServerVersion
            ServerVersion version = ServerVersion.AutoDetect(connStr);

            dbContextBuilder.UseMySql(connStr, version, x => x.UseNetTopologySuite());
            dbContextBuilder.EnableSensitiveDataLogging();
            dbContextBuilder.EnableDetailedErrors();

            return new MariaContext(dbContextBuilder.Options);
        }
    }
}
