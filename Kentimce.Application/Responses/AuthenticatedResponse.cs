using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.Responses
{
    public class AuthenticatedResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
