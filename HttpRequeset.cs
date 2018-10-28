using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lib {
    public class HttpRequest {
        static private int TIMEOUT = 5000;
        static public string makeHttpRequestPost(string url) {
            // Create a request using a URL that can receive a post. 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4";
            // Set the Method property of the request to POST.
            request.Timeout = TIMEOUT;
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        static public string makeHttpRequestGet(string url) {
            // Get the stream associated with the response.
            Stream receiveStream = makeHttpRequestGetStream(url);

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);


            string reet = readStream.ReadToEnd();

            receiveStream.Close();
            readStream.Close();

            return reet;
        }
        static public Stream makeHttpRequestGetStream(string url) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4";
            request.Timeout = TIMEOUT;
            // Set some reasonable limits on resources used by this request
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            // Set credentials to use for this request.
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();


            // Get the stream associated with the response.
            return response.GetResponseStream();
        }
        public static string HttpPost(string URI, string Parameters) {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we're sending. 
            //Post'ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr =
                  new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public static JObject makeRequest(string url, List<HttpProp> data, bool post = true) {
            string rurl = url;

            if (data != null) {
                for (int i = 0; i < data.Count; i++) {
                    if (i == 0) rurl += "?";
                    rurl += data[i].Name + "=" + HttpUtility.UrlEncode(data[i].Value) + "&";
                }
            }
            if (post) {
                return JObject.Parse(HttpRequest.makeHttpRequestPost(rurl));
            } else {
                return JObject.Parse(HttpRequest.makeHttpRequestGet(rurl));
            }
            
        }

        public static string urlEncoding(string par) {
            return HttpUtility.UrlEncode(par);
        }
        public static string urlDecoding(string par) {
            return HttpUtility.UrlDecode(par);
        }
    }
    public class HttpProp {
        public string Name { get; set; }
        public string Value { get; set; }

        public HttpProp(string name, string value) {
            this.Name = name;
            this.Value = value;
        }
        public HttpProp(string name, object value) {
            this.Name = name;
            this.Value = Lib.Converter.toString(value);
        }
    }
}
