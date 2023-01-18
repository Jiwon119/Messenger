using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NetServerDemo.DB;

namespace NetServerDemo
{
    internal class DBContext : DbContext
    {
        public DbSet<DB.User>? Users { get; set; }
        public DbSet<Room>? Rooms { get; set; }
        public DbSet<DB.Chat>? Chats { get; set; }
        public DbSet<User_Room>? User_Rooms { get; set; }
        public DbSet<User_Chat>? User_Chats { get; set; }

        public DbSet<User_Profile>? User_Profiles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            MySqlConnectionStringBuilder connBuilder = new MySqlConnectionStringBuilder();

            string ip = "127.0.0.1";
            string port = "3306";
            connBuilder.Server = ip;
            connBuilder.Port = uint.Parse(port);
            connBuilder.UserID = "root";
            connBuilder.Password = "1234";
            connBuilder.Database = "user_info";
            connBuilder.CharacterSet = "utf8";

            string connStr = connBuilder.ConnectionString;
            ServerVersion version = new MariaDbServerVersion(new Version(10, 6));

            options.UseMySql(connStr, version);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DB.User>(entity =>
            {
                entity.Property(e => e.UserID)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UserID");

                entity.Property(e => e.UserPW)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UserPW");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UserName");

                entity.Property(e => e.UserPhon)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UserPhon");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomID)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RoomID");

                entity.Property(e => e.RoomName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RoomName");

                entity.Property(e => e.CreateTime)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CreateTime");
            });

            modelBuilder.Entity<DB.Chat>(entity =>
            {
                entity.Property(e => e.chatID)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("chatID");

                entity.Property(e => e.RoomNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RoomNo");

                entity.Property(e => e.UserNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UserNo");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Content");

                entity.Property(e => e.CreateTime)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CreateTime");
            });

            modelBuilder.Entity<User_Room>(entity =>
            {
                entity.Property(e => e.UserID)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UserID");

                entity.Property(e => e.RoomID)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RoomID");

                entity.Property(e => e.RoomLeader)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("RoomLeader");

                entity.Property(e => e.CreateTime)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CreateTime");

                entity.HasKey(d => new { d.UserID, d.RoomID });

            });

            modelBuilder.Entity<User_Chat>(entity =>
            {
                entity.Property(e => e.UserID)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UserID");

                entity.Property(e => e.RoomID)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RoomID");

                entity.Property(e => e.ChatID)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ChatID");

                entity.Property(e => e.Check)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Check");

                entity.Property(e => e.CreateTime)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CreateTime");

                entity.HasKey(d => new { d.UserID, d.RoomID, d.ChatID });
            });

            modelBuilder.Entity<User_Profile>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UserID");

                entity.Property(e => e.Img)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Img");

                entity.HasKey(d => new { d.UserId});
            });

        }
    }
}
