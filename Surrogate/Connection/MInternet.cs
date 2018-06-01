// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using Newtonsoft.Json.Linq;
using Surrogate.Implementations;
using Surrogate.Model;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Roboter.MInternet
{
    /// <summary>
    /// Class provides different functions to connect to, send and receice data from the internet
    /// Following: https://stackoverflow.com/questions/27108264/c-sharp-how-to-properly-make-a-http-web-get-request
    /// </summary>
    public class Internet : AbstractConnection
    {
        public override string Name => FrameworkConstants.InternetName;

        public Internet()
        {

        }

        public static string GetString(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static JObject GetJSON(string uri)
        {
            string html = GetString(uri);
            return JObject.Parse(html);
        }

        public static async Task<string> GetHttpStringAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static async Task<JObject> GetHttpJSONAsync(string uri)
        {
            string html = await GetHttpStringAsync(uri);
            return JObject.Parse(html);
        }

        public string Post(string uri, string data, string contentType, string method = "POST")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = method;

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static async Task<string> PostAsync(string uri, string data, string contentType, string method = "POST")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = method;

            using (Stream requestBody = request.GetRequestStream())
            {
                await requestBody.WriteAsync(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public override bool Connect()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    Status = ConnectionStatus.Ready;
                    return true;
                }
            }
            catch
            {
                Status = ConnectionStatus.Disconnected;
                return false;
            }
        }
    }
}
