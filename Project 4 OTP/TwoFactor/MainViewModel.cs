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
        public string EnteredOTP { get; set; }

        internal void Login()
        {
            if (OTPassword == EnteredOTP)
            {
                MessageBox.Show("Login sucessful");
            }
            else
            {
                MessageBox.Show("Login not successful");
            }
        }

        internal void SendOTPPassword()
        {
            if (Username == "Manuel" && Password == "Nicole")
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);

                OTPassword = finalString;
                SendHttpRequest(OTPassword);
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
