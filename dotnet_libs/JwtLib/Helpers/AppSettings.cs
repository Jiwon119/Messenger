using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtLib.Helpers
{
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    public class AppSettings
    {
        public string Secret { get; set; }
    }
}
