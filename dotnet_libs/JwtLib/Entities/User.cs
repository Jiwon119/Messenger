using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JwtLib.Entities
{
    public class User
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string TokenType { get; set; }
        public string Username { get; set; }
        public string IssuedDate { get; set; }
        public string ExpireDate { get; set; }

        [JsonIgnore]
        public string OTP { get; set; }
    }
}
