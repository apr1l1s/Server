using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    public class ErrorHandler
    {
        public enum StatusType
        {
            ok = 200,
            accepted = 202,
            no_content = 204,
            bad_request = 400,
            unauthorized = 401,
            not_found = 404,
            internal_server_error = 500,
        };
    }
}
