using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp
{
    public class AuthenticationService : IAuthenticationServices
    {
        public Task<bool> AuthenticateMobile(string mobile)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyOTP(string code)
        {
            throw new NotImplementedException();
        }
    }
}
