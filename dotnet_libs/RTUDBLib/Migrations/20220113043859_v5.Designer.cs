﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;
using RTUDBLib.DBContext;

namespace RTUDBLib.Migrations
{
    [DbContext(typeof(OracleContext))]
    [Migration("20220113043859_v5")]
    partial class v5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("WMSDB")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoAccount", b =>
                {
                    b.Property<decimal>("UniqueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("UNIQUE_ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("FailedCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasDefaultValue(0m)
                        .HasColumnName("FAILED_COUNT");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("ID");

                    b.Property<decimal>("IsTempPw")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasDefaultValue(0m)
                        .HasColumnName("IS_TEMP_PW");

                    b.Property<string>("PermissionList")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("PERMISSION_LIST");

                    b.Property<string>("Pw")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(100)")
                        .HasColumnName("PW");

                    b.Property<string>("PwSalt")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(100)")
                        .HasColumnName("PW_SALT");

                    b.Property<string>("RegDate")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("REG_DATE");

                    b.HasKey("UniqueId");

                    b.ToTable("SEO_ACCOUNT");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoArea", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("NAME");

                    b.Property<string>("Pos1")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("POS1");

                    b.Property<string>("Pos2")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("POS2");

                    b.HasKey("Id");

                    b.ToTable("SEO_AREA");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoCctv", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConString")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(256)")
                        .HasColumnName("CON_STRING");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("NAME");

                    b.Property<string>("Site")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("SITE");

                    b.Property<string>("Sn")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("SN");

                    b.HasKey("Id");

                    b.ToTable("SEO_CCTV");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoCommand", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Context")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(256)")
                        .HasColumnName("CONTEXT");

                    b.Property<string>("Rsp")
                        .HasColumnType("CLOB")
                        .HasColumnName("RSP");

                    b.Property<string>("State")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("STATE")
                        .HasDefaultValueSql("('WAIT'\n) ");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("TYPE");

                    b.HasKey("Id");

                    b.ToTable("SEO_COMMAND");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoDiscoveredRtu", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("IP");

                    b.Property<decimal>("Port")
                        .HasColumnType("NUMBER")
                        .HasColumnName("PORT");

                    b.Property<decimal>("State")
                        .HasColumnType("NUMBER")
                        .HasColumnName("STATE");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "State" }, "SEO_DISCOVERED_RTU_INDEX");

                    b.ToTable("SEO_DISCOVERED_RTU");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoFacList", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CamList")
                        .HasMaxLength(128)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(128)")
                        .HasColumnName("CAM_LIST");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(48)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(48)")
                        .HasColumnName("NAME");

                    b.HasKey("Id");

                    b.ToTable("SEO_FAC_LIST");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoFacility", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DoorState")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("DOOR_STATE");

                    b.Property<string>("FacType")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("FAC_TYPE");

                    b.Property<string>("InsideWaterlevel")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("INSIDE_WATERLEVEL");

                    b.Property<string>("Name")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("NAME");

                    b.Property<string>("OutsideWaterlevel")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("OUTSIDE_WATERLEVEL");

                    b.Property<string>("RtuList")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("RTU_LIST");

                    b.Property<decimal?>("Type")
                        .HasColumnType("NUMBER")
                        .HasColumnName("TYPE");

                    b.Property<string>("WaterCourse")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("WATER_COURSE");

                    b.HasKey("Id");

                    b.ToTable("SEO_FACILITY");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoFacilitySceneInfo", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("FacilityId")
                        .HasColumnType("NUMBER")
                        .HasColumnName("FACILITY_ID")
                        .HasComment("시설 아이디");

                    b.Property<decimal>("RenderOrder")
                        .HasColumnType("NUMBER")
                        .HasColumnName("RENDER_ORDER")
                        .HasComment("렌더링 순서");

                    b.Property<decimal>("RtuId")
                        .HasColumnType("NUMBER")
                        .HasColumnName("RTU_ID")
                        .HasComment("연결된 RTU 아이디");

                    b.Property<decimal>("SceneNum")
                        .HasColumnType("NUMBER")
                        .HasColumnName("SCENE_NUM")
                        .HasComment("뷰 번호");

                    b.Property<string>("UnitId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("UNIT_ID")
                        .HasComment("유닛 아이디");

                    b.HasKey("Id");

                    b.ToTable("SEO_FACILITY_SCENE_INFO");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoMapArea", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConnectState")
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(45)")
                        .HasColumnName("CONNECT_STATE");

                    b.Property<string>("IsWork")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("IS_WORK");

                    b.Property<string>("Name")
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(45)")
                        .HasColumnName("NAME");

                    b.Property<string>("Pos1")
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(45)")
                        .HasColumnName("POS1");

                    b.Property<string>("Pos2")
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(45)")
                        .HasColumnName("POS2");

                    b.Property<decimal>("SceneNum")
                        .HasColumnType("NUMBER")
                        .HasColumnName("SCENE_NUM");

                    b.Property<string>("Type")
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(45)")
                        .HasColumnName("TYPE");

                    b.Property<string>("WaterLevel")
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(45)")
                        .HasColumnName("WATER_LEVEL");

                    b.Property<string>("WaterLevelRate")
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(45)")
                        .HasColumnName("WATER_LEVEL_RATE");

                    b.Property<decimal>("facType")
                        .HasColumnType("NUMBER")
                        .HasColumnName("FAC_TYPE");

                    b.HasKey("Id");

                    b.ToTable("SEO_MAP_AREA");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoRtu", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Enable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ENABLE")
                        .HasDefaultValueSql("0.0 ");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("IP");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("NAME");

                    b.Property<decimal>("Port")
                        .HasColumnType("NUMBER")
                        .HasColumnName("PORT");

                    b.Property<string>("Site")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("SITE");

                    b.Property<string>("Sn")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("SN");

                    b.HasKey("Id");

                    b.ToTable("SEO_RTU");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoRtuControl", b =>
                {
                    b.Property<decimal>("RtuId")
                        .HasColumnType("NUMBER")
                        .HasColumnName("RTU_ID");

                    b.Property<DateTime>("Timestamp")
                        .HasPrecision(6)
                        .HasColumnType("TIMESTAMP(6)")
                        .HasColumnName("TIMESTAMP");

                    b.Property<decimal>("Value")
                        .HasColumnType("NUMBER")
                        .HasColumnName("VALUE");

                    b.HasKey("RtuId");

                    b.ToTable("SEO_RTU_CONTROL");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoSensorDatum", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Obsdatetime")
                        .HasPrecision(6)
                        .HasColumnType("TIMESTAMP(6)")
                        .HasColumnName("OBSDATETIME");

                    b.Property<decimal?>("ProcValue")
                        .HasColumnType("NUMBER")
                        .HasColumnName("PROCVALUE");

                    b.Property<decimal>("RtuId")
                        .HasColumnType("NUMBER")
                        .HasColumnName("RTU_ID");

                    b.Property<string>("SensorId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("SENSOR_ID");

                    b.Property<decimal>("Value")
                        .HasColumnType("NUMBER")
                        .HasColumnName("VALUE");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "RtuId", "SensorId", "Obsdatetime" }, "SEO_SENSOR_DATA_INDEX");

                    b.ToTable("SEO_SENSOR_DATA");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoSysInfo", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Checkdatetime")
                        .HasPrecision(6)
                        .HasColumnType("TIMESTAMP(6)")
                        .HasColumnName("CHECKDATETIME");

                    b.Property<decimal>("CpuCount")
                        .HasColumnType("NUMBER")
                        .HasColumnName("CPU_COUNT");

                    b.Property<decimal>("CpuPersent")
                        .HasColumnType("NUMBER")
                        .HasColumnName("CPU_PERSENT");

                    b.Property<decimal>("HDDPersent")
                        .HasColumnType("NUMBER")
                        .HasColumnName("HDD_PERSENT");

                    b.Property<decimal>("MemPersent")
                        .HasColumnType("NUMBER")
                        .HasColumnName("MEM_PERSENT");

                    b.Property<decimal>("RtuId")
                        .HasColumnType("NUMBER")
                        .HasColumnName("RTU_ID");

                    b.Property<string>("SysData")
                        .HasColumnType("CLOB")
                        .HasColumnName("SYS_DATA");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "RtuId", "Checkdatetime" }, "SEO_SYSINFO_INDEX");

                    b.ToTable("SEO_SYSINFO");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoSysLog", b =>
                {
                    b.Property<decimal>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LogDateTime")
                        .HasPrecision(6)
                        .HasColumnType("TIMESTAMP(6)")
                        .HasColumnName("LOGDATETIME");

                    b.Property<string>("LogLevel")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("LOGLEVEL");

                    b.Property<string>("Logger")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("LOGGER");

                    b.Property<string>("Message")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("CLOB")
                        .HasColumnName("MESSAGE");

                    b.HasKey("ID");

                    b.HasIndex(new[] { "Logger", "LogDateTime", "LogLevel" }, "SEO_SYSLOG_INDEX");

                    b.ToTable("SEO_SYSLOG");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoTTL", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LastCheckTime")
                        .HasPrecision(6)
                        .HasColumnType("TIMESTAMP(6)")
                        .HasColumnName("LASTCHECKTIME");

                    b.Property<decimal>("LiveLimitSecond")
                        .HasColumnType("NUMBER")
                        .HasColumnName("LIVELIMITSECOND");

                    b.Property<string>("TypeId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("TYPEID");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "TypeId" }, "SEO_TTL_INDEX");

                    b.ToTable("SEO_TTL");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoUnit", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AnnotationMax")
                        .HasColumnType("NUMBER")
                        .HasColumnName("ANNOTATION_MAX");

                    b.Property<decimal>("AnnotationMin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ANNOTATION_MIN")
                        .HasDefaultValueSql("0");

                    b.Property<string>("Context")
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(256)")
                        .HasColumnName("CONTEXT");

                    b.Property<decimal?>("GateOrder")
                        .HasColumnType("NUMBER")
                        .HasColumnName("GATE_ORDER");

                    b.Property<decimal>("IsNewUnit")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ISNEWUNIT")
                        .HasDefaultValueSql("0");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("NAME");

                    b.Property<decimal>("RtuId")
                        .HasColumnType("NUMBER")
                        .HasColumnName("RTU_ID");

                    b.Property<string>("StateType")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("STATE_TYPE");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("TYPE");

                    b.Property<string>("TypeCode")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("TYPE_CODE");

                    b.Property<decimal>("ValueMax")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("VALUE_MAX")
                        .HasDefaultValueSql("100\n   ");

                    b.Property<decimal>("ValueMin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("VALUE_MIN")
                        .HasDefaultValueSql("0");

                    b.Property<string>("ValueName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("VALUE_NAME");

                    b.Property<decimal?>("WaterLaneId")
                        .HasColumnType("NUMBER")
                        .HasColumnName("WATER_LANE_ID");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "RtuId" }, "SEO_UNIT_INDEX1");

                    b.ToTable("SEO_UNIT");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoUnitDatum", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("DATE")
                        .HasColumnName("LOG_DATE");

                    b.Property<decimal>("UnitId")
                        .HasColumnType("NUMBER")
                        .HasColumnName("UNIT_ID");

                    b.Property<decimal>("Value")
                        .HasColumnType("NUMBER")
                        .HasColumnName("VALUE");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UnitId", "LogDate" }, "SEO_UNIT_DATA_INDEX2");

                    b.ToTable("SEO_UNIT_DATA");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoUnitStatus", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(500)")
                        .HasColumnName("CONTENT");

                    b.Property<string>("Type")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(20)")
                        .HasColumnName("TYPE");

                    b.HasKey("Id");

                    b.ToTable("SEO_UNIT_STATUS");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoUsageLog", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("ActionCode")
                        .HasColumnType("NUMBER")
                        .HasColumnName("ACTION_CODE");

                    b.Property<string>("Actor")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("ACTOR");

                    b.Property<string>("Target")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("TARGET");

                    b.Property<DateTime>("Timestamp")
                        .HasPrecision(6)
                        .HasColumnType("TIMESTAMP(6)")
                        .HasColumnName("TIMESTAMP");

                    b.Property<decimal?>("Value")
                        .HasColumnType("NUMBER")
                        .HasColumnName("VALUE");

                    b.HasKey("Id");

                    b.ToTable("SEO_USAGE_LOG");
                });

            modelBuilder.Entity("RTUDBLib.DBSchema.SeoWaterLane", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasColumnName("ID")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FacList")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(100)")
                        .HasColumnName("FAC_LIST");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR2(50)")
                        .HasColumnName("NAME");

                    b.HasKey("Id");

                    b.ToTable("SEO_WATER_LANE");
                });
#pragma warning restore 612, 618
        }
    }
}
