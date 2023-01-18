using System;
using CommonLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Oracle.ManagedDataAccess.Client;
using RTUDBLib.DBSchema;

#nullable disable

namespace RTUDBLib.DBContext
{
    public partial class OracleContext : DbContext
    {
        public OracleContext()
        {
        }

        public OracleContext(DbContextOptions<OracleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SeoAccount> SeoAccounts { get; set; }
        public virtual DbSet<SeoArea> SeoAreas { get; set; }
        public virtual DbSet<SeoCctv> SeoCctvs { get; set; }
        public virtual DbSet<SeoCommand> SeoCommands { get; set; }
        public virtual DbSet<SeoDiscoveredRtu> SeoDiscoveredRtus { get; set; }
        public virtual DbSet<SeoFacList> SeoFacLists { get; set; }
        public virtual DbSet<SeoFacility> SeoFacilities { get; set; }
        public virtual DbSet<SeoFacility> SeoFacilityEventLogs { get; set; }
        public virtual DbSet<SeoFacilitySceneInfo> SeoFacilitySceneInfos { get; set; }
        public virtual DbSet<SeoMapArea> SeoMapAreas { get; set; }
        public virtual DbSet<SeoRtu> SeoRtus { get; set; }
        public virtual DbSet<SeoRtuControl> SeoRtuControls { get; set; }
        public virtual DbSet<SeoSensorDatum> SeoSensorData { get; set; }
        public virtual DbSet<SeoUnit> SeoUnits { get; set; }
        public virtual DbSet<SeoUnitDatum> SeoUnitData { get; set; }
        public virtual DbSet<SeoUnitStatus> SeoUnitStatuses { get; set; }
        public virtual DbSet<SeoUsageLog> SeoUsageLogs { get; set; }
        public virtual DbSet<SeoWaterLane> SeoWaterLanes { get; set; }

        public virtual DbSet<SeoSysInfo> SeoSysInfos { get; set; }
        public virtual DbSet<SeoTTL> SeoTTLs { get; set; }
        public virtual DbSet<SeoSysLog> SeoLogs { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string usr = EnvConfig.GetOrDefaultEnvValue("ORACLE_DB_USER", "WMSDB");
                string pw = EnvConfig.GetOrDefaultEnvValue("ORACLE_DB_PW", "oracle");
                //string ip = EnvConfig.GetOrDefaultEnvValue("ORACLE_DB_IP", "192.168.0.150");
                string ip = EnvConfig.GetOrDefaultEnvValue("ORACLE_DB_IP", "127.0.0.1");
                string port = EnvConfig.GetOrDefaultEnvValue("ORACLE_DB_PORT", "49154");

                var connBuilder = new OracleConnectionStringBuilder();
                connBuilder.UserID = usr;
                connBuilder.Password = pw;
                connBuilder.DataSource = $"{ip}:{port}";
                connBuilder.PersistSecurityInfo = true;

                string connStr = connBuilder.ConnectionString;
                optionsBuilder.UseOracle(connStr, opt => opt.UseOracleSQLCompatibility("11"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("WMSDB");

            modelBuilder.Entity<SeoFacilitySceneInfo>(entity =>
            {
                entity.ToTable("SEO_FACILITY_SCENE_INFO");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.FacilityId)
                    .IsRequired()
                    .HasColumnType("NUMBER")
                    .HasComment("시설 아이디")
                    .HasColumnName("FACILITY_ID");

                entity.Property(e => e.RtuId)
                    .IsRequired()
                    .HasColumnType("NUMBER")
                    .HasComment("연결된 RTU 아이디")
                    .HasColumnName("RTU_ID");

                entity.Property(e => e.UnitId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("유닛 아이디")
                    .HasColumnName("UNIT_ID");

                entity.Property(e => e.SceneNum)
                    .IsRequired()
                    .HasColumnType("NUMBER")
                    .HasComment("뷰 번호")
                    .HasColumnName("SCENE_NUM");

                entity.Property(e => e.RenderOrder)
                    .IsRequired()
                    .HasColumnType("NUMBER")
                    .HasComment("렌더링 순서")
                    .HasColumnName("RENDER_ORDER");
            });

            modelBuilder.Entity<SeoSysLog>(entity =>
            {
                entity.ToTable("SEO_SYSLOG");

                entity.HasKey(e => e.ID);
                entity.HasIndex(e => new { e.Logger, e.LogDateTime, e.LogLevel }, "SEO_SYSLOG_INDEX");

                entity.Property(e => e.ID)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LogLevel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LOGLEVEL");

                entity.Property(e => e.LogDateTime)
                    .HasPrecision(6)
                    .IsRequired()
                    .HasColumnName("LOGDATETIME");

                entity.Property(e => e.Logger)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LOGGER");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnType("CLOB")
                    .HasColumnName("MESSAGE");
            });

            modelBuilder.Entity<SeoTTL>(entity =>
            {
                entity.ToTable("SEO_TTL");

                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.TypeId }, "SEO_TTL_INDEX");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.TypeId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TYPEID");

                entity.Property(e => e.LastCheckTime)
                    .HasPrecision(6)
                    .IsRequired()
                    .HasColumnName("LASTCHECKTIME");

                entity.Property(e => e.LiveLimitSecond)
                    .IsRequired()
                    .HasColumnType("NUMBER")
                    .HasColumnName("LIVELIMITSECOND");
            });

            modelBuilder.Entity<SeoSysInfo>(entity =>
            {
                entity.ToTable("SEO_SYSINFO");

                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.RtuId, e.Checkdatetime }, "SEO_SYSINFO_INDEX");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.RtuId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("RTU_ID");

                entity.Property(e => e.Checkdatetime)
                    .HasPrecision(6)
                    .HasColumnName("CHECKDATETIME");

                entity.Property(e => e.CpuCount)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CPU_COUNT");

                entity.Property(e => e.CpuPersent)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CPU_PERSENT");

                entity.Property(e => e.MemPersent)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MEM_PERSENT");

                entity.Property(e => e.HDDPersent)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HDD_PERSENT");

                entity.Property(e => e.SysData)
                    .HasColumnType("CLOB")
                    .HasColumnName("SYS_DATA");
            });


            modelBuilder.Entity<SeoAccount>(entity =>
            {
                entity.HasKey(e => e.UniqueId);
                entity.ToTable("SEO_ACCOUNT");

                entity.Property(e => e.UniqueId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("UNIQUE_ID");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.PermissionList)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PERMISSION_LIST");

                entity.Property(e => e.Pw)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PW");

                entity.Property(e => e.PwSalt)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PW_SALT");

                entity.Property(e => e.RegDate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REG_DATE");

                entity.Property(e => e.IsTempPw)
                    .HasColumnType("NUMBER")
                    .HasColumnName("IS_TEMP_PW")
                    .HasDefaultValue(0);

                entity.Property(e => e.FailedCount)
                    .HasColumnType("NUMBER")
                    .HasColumnName("FAILED_COUNT")
                    .HasDefaultValue(0);
            });

            modelBuilder.Entity<SeoArea>(entity =>
            {
                entity.ToTable("SEO_AREA");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Pos1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POS1");

                entity.Property(e => e.Pos2)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POS2");
            });

            modelBuilder.Entity<SeoCctv>(entity =>
            {
                entity.ToTable("SEO_CCTV");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ConString)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CON_STRING");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SITE");

                entity.Property(e => e.Sn)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SN");
            });

            modelBuilder.Entity<SeoCommand>(entity =>
            {
                entity.ToTable("SEO_COMMAND");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Context)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CONTEXT");

                entity.Property(e => e.Rsp)
                    .HasColumnType("CLOB")
                    .HasColumnName("RSP");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("STATE")
                    .HasDefaultValueSql("('WAIT'\n) ");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<SeoDiscoveredRtu>(entity =>
            {
                entity.ToTable("SEO_DISCOVERED_RTU");

                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.State, "SEO_DISCOVERED_RTU_INDEX");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IP");

                entity.Property(e => e.Port)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PORT");

                entity.Property(e => e.State)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATE");
            });

            modelBuilder.Entity<SeoFacList>(entity =>
            {
                entity.ToTable("SEO_FAC_LIST");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CamList)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("CAM_LIST");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(48)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<SeoFacility>(entity =>
            {
                entity.ToTable("SEO_FACILITY");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.DoorState)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DOOR_STATE");

                entity.Property(e => e.FacType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FAC_TYPE");

                entity.Property(e => e.InsideWaterlevel)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("INSIDE_WATERLEVEL");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.OutsideWaterlevel)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OUTSIDE_WATERLEVEL");

                entity.Property(e => e.Type)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE");

                entity.Property(e => e.WaterCourse)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("WATER_COURSE");

                entity.Property(e => e.RtuList)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RTU_LIST");

                entity.Property(e => e.Pos1)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("POS1");

                entity.Property(e => e.Pos2)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("POS2");

                entity.Property(e => e.Address)
                    .HasColumnName("Address")
                    .IsUnicode(false)
                    .HasMaxLength(100);

                entity.Property(e => e.Cam_List)
                    .HasColumnName("Cam_List")
                    .IsUnicode(false)
                    .HasMaxLength(200);

            });

            modelBuilder.Entity<SeoFacilityEventLog>(entity =>
            {
                entity.ToTable("SEO_FACILITY_EVENT_LOG");

                entity.HasKey(e => e.ID);
                entity.HasIndex(e => new { e.FacilityId, e.Logger, e.LogDateTime, e.LogLevel }, "SEO_Facility_Event_INDEX");

                entity.Property(e => e.ID)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.FacilityId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("FACILITY_ID");

                entity.Property(e => e.LogLevel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LOGLEVEL");

                entity.Property(e => e.LogDateTime)
                    .HasPrecision(6)
                    .IsRequired()
                    .HasColumnName("LOGDATETIME");

                entity.Property(e => e.Logger)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LOGGER");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnType("CLOB")
                    .HasColumnName("MESSAGE");
            });

            modelBuilder.Entity<SeoMapArea>(entity =>
            {
                entity.ToTable("SEO_MAP_AREA");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ConnectState)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("CONNECT_STATE");

                entity.Property(e => e.IsWork)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IS_WORK");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Pos1)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("POS1");

                entity.Property(e => e.Pos2)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("POS2");

                entity.Property(e => e.Type)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");

                entity.Property(e => e.WaterLevel)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("WATER_LEVEL");

                entity.Property(e => e.WaterLevelRate)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("WATER_LEVEL_RATE");

                entity.Property(e => e.SceneNum)
                    .IsRequired()
                    .HasColumnType("NUMBER")
                    .HasColumnName("SCENE_NUM");

                entity.Property(e => e.facType)
                    .IsRequired()
                    .HasColumnType("NUMBER")
                    .HasColumnName("FAC_TYPE");
            });

            modelBuilder.Entity<SeoRtu>(entity =>
            {
                entity.ToTable("SEO_RTU");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Enable)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ENABLE")
                    .HasDefaultValueSql("0.0 ");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Port)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PORT");

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SITE");

                entity.Property(e => e.Sn)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SN");
            });

            modelBuilder.Entity<SeoRtuControl>(entity =>
            {
                entity.HasKey(e => e.RtuId);

                entity.ToTable("SEO_RTU_CONTROL");

                entity.Property(e => e.RtuId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("RTU_ID");

                entity.Property(e => e.Timestamp)
                    .HasPrecision(6)
                    .HasColumnName("TIMESTAMP");

                entity.Property(e => e.Value)
                    .HasColumnType("NUMBER")
                    .HasColumnName("VALUE");
            });

            modelBuilder.Entity<SeoSensorDatum>(entity =>
            {
                entity.ToTable("SEO_SENSOR_DATA");

                entity.HasKey(e => e.Id);

                entity.HasIndex(e => new { e.RtuId, e.SensorId, e.Obsdatetime }, "SEO_SENSOR_DATA_INDEX");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Obsdatetime)
                    .HasPrecision(6)
                    .HasColumnName("OBSDATETIME");

                entity.Property(e => e.RtuId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("RTU_ID");

                entity.Property(e => e.SensorId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SENSOR_ID");

                entity.Property(e => e.Value)
                    .HasColumnType("NUMBER")
                    .HasColumnName("VALUE");

                entity.Property(e => e.ProcValue)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PROCVALUE");
            });

            modelBuilder.Entity<SeoUnit>(entity =>
            {
                entity.ToTable("SEO_UNIT");

                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.RtuId, "SEO_UNIT_INDEX1");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.AnnotationMax)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ANNOTATION_MAX");

                entity.Property(e => e.AnnotationMin)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ANNOTATION_MIN")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Context)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CONTEXT");

                entity.Property(e => e.GateOrder)
                    .HasColumnType("NUMBER")
                    .HasColumnName("GATE_ORDER");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.RtuId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("RTU_ID");

                entity.Property(e => e.StateType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("STATE_TYPE");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");

                entity.Property(e => e.TypeCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_CODE");

                entity.Property(e => e.ValueMax)
                    .HasColumnType("NUMBER")
                    .HasColumnName("VALUE_MAX")
                    .HasDefaultValueSql("100\n   ");

                entity.Property(e => e.ValueMin)
                    .HasColumnType("NUMBER")
                    .HasColumnName("VALUE_MIN")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ValueName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("VALUE_NAME");

                entity.Property(e => e.WaterLaneId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("WATER_LANE_ID");

                entity.Property(e => e.IsNewUnit)
                    .IsRequired()
                    .HasColumnType("NUMBER")
                    .HasColumnName("ISNEWUNIT")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<SeoUnitDatum>(entity =>
            {
                entity.ToTable("SEO_UNIT_DATA");

                entity.HasKey(e => e.Id);

                entity.HasIndex(e => new { e.UnitId, e.LogDate }, "SEO_UNIT_DATA_INDEX2");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LogDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LOG_DATE");

                entity.Property(e => e.UnitId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("UNIT_ID");

                entity.Property(e => e.Value)
                    .HasColumnType("NUMBER")
                    .HasColumnName("VALUE");
            });

            modelBuilder.Entity<SeoUnitStatus>(entity =>
            {
                entity.ToTable("SEO_UNIT_STATUS");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Content)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("CONTENT");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<SeoUsageLog>(entity =>
            {
                entity.ToTable("SEO_USAGE_LOG");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ActionCode)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ACTION_CODE");

                entity.Property(e => e.Actor)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ACTOR");

                entity.Property(e => e.Target)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TARGET");

                entity.Property(e => e.Timestamp)
                    .HasPrecision(6)
                    .HasColumnName("TIMESTAMP");

                entity.Property(e => e.Value)
                    .HasColumnType("NUMBER")
                    .HasColumnName("VALUE");
            });

            modelBuilder.Entity<SeoWaterLane>(entity =>
            {
                entity.ToTable("SEO_WATER_LANE");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.FacList)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FAC_LIST");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<SeoWaterSystem>(entity =>
            {
                entity.ToTable("SEO_WATERSYSTEM");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.FacID)
                    .HasColumnType("NUMBER")
                    .HasColumnName("FACID");

                entity.Property(e => e.WaterLaneID)
                    .HasColumnType("NUMBER")
                    .HasColumnName("WATERLANEID");

                entity.Property(e => e.SensorID)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SENSORID");

                entity.Property(e => e.Order)
                     .HasColumnType("NUMBER")
                     .HasColumnName("ORDER");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
