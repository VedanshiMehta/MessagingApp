using Firebase;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp
{
    public class AuthenticationService : PhoneAuthProvider.OnVerificationStateChangedCallbacks,IAuthenticationServices
    {
        private TaskCompletionSource<bool> _verificationCodeCompletionSource;
        private string _verificationId;
        public Task<bool> AuthenticateMobile(string mobile)
        {
            _verificationCodeCompletionSource = new TaskCompletionSource<bool>();
           var authOption = PhoneAuthOptions.NewBuilder()
                .SetPhoneNumber(mobile)
                .SetTimeout((Java.Lang.Long)60L,Java.Util.Concurrent.TimeUnit.Seconds)
                .SetActivity(Platform.CurrentActivity)
                .SetCallbacks(this).Build();
            PhoneAuthProvider.VerifyPhoneNumber(authOption);
            return _verificationCodeCompletionSource.Task;

        }
        public override void OnCodeSent(string verificationId, PhoneAuthProvider.ForceResendingToken p1)
        {
            base.OnCodeSent(verificationId, p1);
            _verificationCodeCompletionSource.SetResult(true);
            _verificationId = verificationId;
        }
        public override void OnVerificationCompleted(PhoneAuthCredential p0)
        {
            System.Diagnostics.Debug.WriteLine("Verification Completed");
        }

        public override void OnVerificationFailed(FirebaseException p0)
        {
           _verificationCodeCompletionSource.SetResult(false);  
        }

        public async Task<bool> VerifyOTP(string code)
        {
            bool returnValue = false;
            if(!string.IsNullOrEmpty(_verificationId))
            {
                var credential = PhoneAuthProvider.GetCredential(_verificationId, code);
                await FirebaseAuth.Instance.SignInWithCredentialAsync(credential).ContinueWith((authTask) =>
                {
                    if(authTask.IsFaulted || authTask.IsCanceled)
                    {
                        returnValue = false;
                        return;
                    }
                    returnValue = true;

                });
            }
            return returnValue;
        }
    }
}
