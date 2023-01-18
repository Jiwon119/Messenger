using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JwtLib.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string OTP { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string TokenType { get; set; }

        [Required]
        public string UserName { get; set; }

        [JsonIgnore]
        public string IssuedDate { get; set; }
    }
}
