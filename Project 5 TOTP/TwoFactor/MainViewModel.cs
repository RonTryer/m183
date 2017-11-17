using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TwoFactor
{
    public class MainViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// Generated OTP
        /// </summary>
        public string OTPassword { get; set; }

        /// <summary>
        /// Generated OTP
        /// </summary>
        public string EnteredTOTP { get; set; }

        internal void Login()
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

            if (tfa.ValidateTwoFactorPIN("SuperSecretKeyGoesHere", EnteredTOTP))
            {
                MessageBox.Show("Login sucessful");
            }
            else
            {
                MessageBox.Show("Login not successful");
            }
        }

        internal void SetupOTPQR()
        {
            if (Username == "Manuel" && Password == "Nicole")
            {
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                var setupInfo = tfa.GenerateSetupCode("MyApp", "user@example.com", "SuperSecretKeyGoesHere", 300, 300);

                // Generate setup QR code
                string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                string manualEntrySetupCode = setupInfo.ManualEntryKey;

                // Open setup QR code in default browser
                System.Diagnostics.Process.Start(qrCodeImageUrl);
            }
            else
            {
                MessageBox.Show("Wrong username or password");
            }
        }

        private async void SendHttpRequest(string oTPPassword)
        {
            var client = new HttpClient();

            // Create the HttpContent for the form to be posted.
            var requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("api_key", "4de9ebde"),
                new KeyValuePair<string, string>("api_secret", "b4a730dd9192692e"),
                new KeyValuePair<string, string>("to", "41765480052"),
                new KeyValuePair<string, string>("from", "NEXMO"),
                new KeyValuePair<string, string>("text", oTPPassword),
            });

            // Get the response.
            HttpResponseMessage response = await client.PostAsync(
                "https://rest.nexmo.com/sms/json",
                requestContent);

            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                string response2 = (await reader.ReadToEndAsync());
                // Write the output.
                //MessageBox.Show(response2);
            }
        }
    }
}
