using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RTUDBLib.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "WMSDB");

            migrationBuilder.CreateTable(
                name: "SEO_ACCOUNT",
                schema: "WMSDB",
                columns: table => new
                {
                    UNIQUE_ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    PERMISSION_LIST = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    PW = table.Column<string>(type: "VARCHAR2(100)", unicode: false, maxLength: 100, nullable: false),
                    ID = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false),
                    REG_DATE = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: true),
                    PW_SALT = table.Column<string>(type: "VARCHAR2(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_ACCOUNT", x => x.UNIQUE_ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_AREA",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    POS1 = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false),
                    POS2 = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false),
                    NAME = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_AREA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_CCTV",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    SN = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    CON_STRING = table.Column<string>(type: "VARCHAR2(256)", unicode: false, maxLength: 256, nullable: false),
                    SITE = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_CCTV", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_COMMAND",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    TYPE = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    CONTEXT = table.Column<string>(type: "VARCHAR2(256)", unicode: false, maxLength: 256, nullable: false),
                    STATE = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false, defaultValueSql: "('WAIT'\n) "),
                    RSP = table.Column<string>(type: "CLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_COMMAND", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_DISCOVERED_RTU",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    STATE = table.Column<decimal>(type: "NUMBER", nullable: false),
                    IP = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    PORT = table.Column<decimal>(type: "NUMBER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_DISCOVERED_RTU", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_FAC_LIST",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "VARCHAR2(48)", unicode: false, maxLength: 48, nullable: false),
                    CAM_LIST = table.Column<string>(type: "VARCHAR2(128)", unicode: false, maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_FAC_LIST", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_FACILITY",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: true),
                    DOOR_STATE = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: true),
                    TYPE = table.Column<decimal>(type: "NUMBER", nullable: true),
                    INSIDE_WATERLEVEL = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: true),
                    OUTSIDE_WATERLEVEL = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: true),
                    WATER_COURSE = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: true),
                    FAC_TYPE = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_FACILITY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_MAP_AREA",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "VARCHAR2(45)", unicode: false, maxLength: 45, nullable: true),
                    POS1 = table.Column<string>(type: "VARCHAR2(45)", unicode: false, maxLength: 45, nullable: true),
                    POS2 = table.Column<string>(type: "VARCHAR2(45)", unicode: false, maxLength: 45, nullable: true),
                    TYPE = table.Column<string>(type: "VARCHAR2(45)", unicode: false, maxLength: 45, nullable: true),
                    WATER_LEVEL = table.Column<string>(type: "VARCHAR2(45)", unicode: false, maxLength: 45, nullable: true),
                    WATER_LEVEL_RATE = table.Column<string>(type: "VARCHAR2(45)", unicode: false, maxLength: 45, nullable: true),
                    CONNECT_STATE = table.Column<string>(type: "VARCHAR2(45)", unicode: false, maxLength: 45, nullable: true),
                    IS_WORK = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_MAP_AREA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_RTU",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    SN = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    IP = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    PORT = table.Column<decimal>(type: "NUMBER", nullable: false),
                    SITE = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    ENABLE = table.Column<decimal>(type: "NUMBER", nullable: false, defaultValueSql: "0.0 ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_RTU", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_RTU_CONTROL",
                schema: "WMSDB",
                columns: table => new
                {
                    RTU_ID = table.Column<decimal>(type: "NUMBER", nullable: false),
                    VALUE = table.Column<decimal>(type: "NUMBER", nullable: false),
                    TIMESTAMP = table.Column<DateTime>(type: "TIMESTAMP(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_RTU_CONTROL", x => x.RTU_ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_SENSOR_DATA",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    RTU_ID = table.Column<decimal>(type: "NUMBER", nullable: false),
                    SENSOR_ID = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    OBSDATETIME = table.Column<DateTime>(type: "TIMESTAMP(6)", precision: 6, nullable: false),
                    VALUE = table.Column<decimal>(type: "NUMBER", nullable: false),
                    PROCVALUE = table.Column<decimal>(type: "NUMBER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_SENSOR_DATA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_SYSINFO",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    RTU_ID = table.Column<decimal>(type: "NUMBER", nullable: false),
                    CHECKDATETIME = table.Column<DateTime>(type: "TIMESTAMP(6)", precision: 6, nullable: false),
                    CPU_COUNT = table.Column<decimal>(type: "NUMBER", nullable: false),
                    CPU_PERSENT = table.Column<decimal>(type: "NUMBER", nullable: false),
                    MEM_PERSENT = table.Column<decimal>(type: "NUMBER", nullable: false),
                    HDD_PERSENT = table.Column<decimal>(type: "NUMBER", nullable: false),
                    SYS_DATA = table.Column<string>(type: "CLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_SYSINFO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_SYSLOG",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    LOGLEVEL = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false),
                    LOGDATETIME = table.Column<DateTime>(type: "TIMESTAMP(6)", precision: 6, nullable: false),
                    LOGGER = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false),
                    MESSAGE = table.Column<string>(type: "CLOB", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_SYSLOG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_TTL",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    TYPEID = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false),
                    LASTCHECKTIME = table.Column<DateTime>(type: "TIMESTAMP(6)", precision: 6, nullable: false),
                    LIVELIMITSECOND = table.Column<decimal>(type: "NUMBER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_TTL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_UNIT",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    RTU_ID = table.Column<decimal>(type: "NUMBER", nullable: false),
                    TYPE = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    NAME = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    VALUE_NAME = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    ANNOTATION_MIN = table.Column<decimal>(type: "NUMBER", nullable: false, defaultValueSql: "0"),
                    ANNOTATION_MAX = table.Column<decimal>(type: "NUMBER", nullable: false),
                    CONTEXT = table.Column<string>(type: "VARCHAR2(256)", unicode: false, maxLength: 256, nullable: true),
                    VALUE_MIN = table.Column<decimal>(type: "NUMBER", nullable: false, defaultValueSql: "0"),
                    VALUE_MAX = table.Column<decimal>(type: "NUMBER", nullable: false, defaultValueSql: "100\n   "),
                    STATE_TYPE = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: true),
                    TYPE_CODE = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: true),
                    WATER_LANE_ID = table.Column<decimal>(type: "NUMBER", nullable: true),
                    GATE_ORDER = table.Column<decimal>(type: "NUMBER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_UNIT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_UNIT_DATA",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    UNIT_ID = table.Column<decimal>(type: "NUMBER", nullable: false),
                    LOG_DATE = table.Column<DateTime>(type: "DATE", nullable: false),
                    VALUE = table.Column<decimal>(type: "NUMBER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_UNIT_DATA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_UNIT_STATUS",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    CONTENT = table.Column<string>(type: "VARCHAR2(500)", unicode: false, maxLength: 500, nullable: true),
                    TYPE = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_UNIT_STATUS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_USAGE_LOG",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    ACTION_CODE = table.Column<decimal>(type: "NUMBER", nullable: false),
                    TIMESTAMP = table.Column<DateTime>(type: "TIMESTAMP(6)", precision: 6, nullable: false),
                    VALUE = table.Column<decimal>(type: "NUMBER", nullable: true),
                    TARGET = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: true),
                    ACTOR = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_USAGE_LOG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEO_WATER_LANE",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: true),
                    FAC_LIST = table.Column<string>(type: "VARCHAR2(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_WATER_LANE", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "SEO_DISCOVERED_RTU_INDEX",
                schema: "WMSDB",
                table: "SEO_DISCOVERED_RTU",
                column: "STATE");

            migrationBuilder.CreateIndex(
                name: "SEO_SENSOR_DATA_INDEX",
                schema: "WMSDB",
                table: "SEO_SENSOR_DATA",
                columns: new[] { "RTU_ID", "SENSOR_ID", "OBSDATETIME" });

            migrationBuilder.CreateIndex(
                name: "SEO_SYSINFO_INDEX",
                schema: "WMSDB",
                table: "SEO_SYSINFO",
                columns: new[] { "RTU_ID", "CHECKDATETIME" });

            migrationBuilder.CreateIndex(
                name: "SEO_SYSLOG_INDEX",
                schema: "WMSDB",
                table: "SEO_SYSLOG",
                columns: new[] { "LOGGER", "LOGDATETIME", "LOGLEVEL" });

            migrationBuilder.CreateIndex(
                name: "SEO_TTL_INDEX",
                schema: "WMSDB",
                table: "SEO_TTL",
                column: "TYPEID");

            migrationBuilder.CreateIndex(
                name: "SEO_UNIT_INDEX1",
                schema: "WMSDB",
                table: "SEO_UNIT",
                column: "RTU_ID");

            migrationBuilder.CreateIndex(
                name: "SEO_UNIT_DATA_INDEX2",
                schema: "WMSDB",
                table: "SEO_UNIT_DATA",
                columns: new[] { "UNIT_ID", "LOG_DATE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SEO_ACCOUNT",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_AREA",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_CCTV",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_COMMAND",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_DISCOVERED_RTU",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_FAC_LIST",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_FACILITY",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_MAP_AREA",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_RTU",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_RTU_CONTROL",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_SENSOR_DATA",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_SYSINFO",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_SYSLOG",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_TTL",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_UNIT",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_UNIT_DATA",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_UNIT_STATUS",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_USAGE_LOG",
                schema: "WMSDB");

            migrationBuilder.DropTable(
                name: "SEO_WATER_LANE",
                schema: "WMSDB");
        }
    }
}
