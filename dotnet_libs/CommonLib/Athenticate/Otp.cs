using OtpNet;
using System;
using System.Text;

namespace CommonLib.Athenticate
{
    public class OTP
    {
        private Totp otp_sha512, otp_sha256, otp_sha1;

        public OTP(string secretKey)
        {
            var key = Encoding.ASCII.GetBytes(secretKey);

            otp_sha512 = new Totp(key, mode: OtpHashMode.Sha512, totpSize: 6, step: 15);
            otp_sha256 = new Totp(key, mode: OtpHashMode.Sha256, totpSize: 6, step: 15);
            otp_sha1 = new Totp(key, mode: OtpHashMode.Sha1, totpSize: 6, step: 15);
        }

        public string GetOtp()
        {
            string str1 = otp_sha512.ComputeTotp(DateTime.UtcNow);
            string str2 = otp_sha256.ComputeTotp(DateTime.UtcNow);
            string str3 = otp_sha1.ComputeTotp(DateTime.UtcNow);

            return string.Join("-", str1, str2, str3);
        }
    }
}
