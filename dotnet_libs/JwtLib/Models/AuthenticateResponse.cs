using JwtLib.Entities;

namespace JwtLib.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Username { get; set; }
        public string TokenType { get; set; }
        public string Token { get; set; }
        public string IssuedDate { get; set; }
        public string ExpireDate { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Company = user.Company;
            Department = user.Department;
            Username = user.Username;
            TokenType = user.TokenType;
            Token = token;
            IssuedDate = user.IssuedDate;
            ExpireDate = user.ExpireDate;
        }
    }
}
