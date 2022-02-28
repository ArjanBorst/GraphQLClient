using System;
using System.IO;
using System.Net;


namespace GraphQL
{   
    public class GraphQLClient : IDisposable
    {
        public string ErrorMessage { get; set; }
        public string Response { get; set; }
        public string Path { get; set; }
        public string ApiKey { get; set; }
        public string Password { get; set; }    
        public GraphQLClient(string path, string apiKey, string password)
        {
            Path = path;
        }
        private GraphQLClient()
        {
        }
        public bool Execute(string query)
        {            
            var auth = System.Text.Encoding.UTF8.GetBytes(ApiKey  + ":" + Password);
            string auth64 = Convert.ToBase64String(auth);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Path);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic " + auth64);

            if (query != null)
            {
                if (!String.IsNullOrEmpty(query))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var writer = new StreamWriter(request.GetRequestStream()))
                        {
                            writer.Write(query);
                            writer.Close();
                        }
                    }
                }
            }
            
            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    Response = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (WebException ex)
            {
                // Any non 200 status code server errors
                using (var stream = ex.Response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        ErrorMessage = reader.ReadToEnd();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Some general error like server was never reached
                ErrorMessage = ex.Message;
                return false;
            }

            return true;
        }

        public void Dispose()
        {
           // Dispose(true);
        }
    }
}