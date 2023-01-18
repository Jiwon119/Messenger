using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PublicDBLib.MariaDB.Meta
{
    [Table("Meta_CheckPoint")]
    [Index(nameof(TargetCode), IsUnique = true)]
    public partial class Meta_CheckPoint
    {
        public long Id { get; set; }

        public string Category { get; set; }
        public string TargetCode { get; set; }

        //중간에 끊길 경우를 대비하여 어디까지 작업했는지 체크
        public DateTime DateCheckPoint { get; set; }

        // 데이터 컬랙팅이 끝났는지 체크
        public ushort IsComplete { get; set; }

        //데이터 시작일
        public DateTime DataBeginTime { get; set; }

        // 현재까지 DB에 수집된 가장 최신 일자
        public DateTime DataLastTime { get; set; }

        public short LastPage { get; set; }
        public short RetryCount { get; set; }
    }
}
