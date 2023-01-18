using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Athenticate
{
    public interface IToken
    {
        string url { get; }
        string Type { get; }
        int ExpiresDate { get; }

        object Request { get; set; }

        Task SaveToken(string token);
        Task<string> RetrivedToken(string company, string department, string tokenType, string userName);
        string GetRecentToken();
    }

    public class JwtRequest
    {
        public string OTP { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string TokenType { get; set; }
        public string UserName { get; set; }
    }

}
