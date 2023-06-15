using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp
{
    public interface IAuthenticationServices
    {
        Task<bool> AuthenticateMobile(string mobile);
        Task<bool> VerifyOTP(string code);
    }
}
