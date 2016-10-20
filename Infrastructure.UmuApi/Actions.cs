using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Infrastructure.UmuApi
{
    public class Actions
    {
        public RootObject GetPersonFromUmuApi()
        {
            var url = "https://websrv01.testad.umu.se/Catalogue/3.0/api/org/e841c73b5524c220b45690b8e1d841c87c454436/person";

            RootObject result = new RootObject();

            try
            {
                var length = 1266276;
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
                myReq.Method = "GET";
                myReq.Credentials = new NetworkCredential("sape0014", "Pwd2Chang3");
                HttpWebResponse myResponse = (HttpWebResponse)myReq.GetResponse();
                
                if (myResponse.StatusCode == HttpStatusCode.OK)
                {
                    
                    
                    if (myResponse.ContentLength != length)
                    { 
                    Stream rebut = myResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(rebut, Encoding.UTF8); // Pipes the stream to a higher level stream reader with the required encoding format. 
                    string rawData = readStream.ReadToEnd();


                    result = JsonConvert.DeserializeObject<RootObject>(rawData);


                    myResponse.Close();
                    readStream.Close();

                    }
                }
                
            }
            catch (WebException ex)
            {
                // same as normal response, get error response
                var errorResponse = (HttpWebResponse)ex.Response;
                string errorResponseJson;
                var statusCode = errorResponse.StatusCode;
                var errorIdFromHeader = errorResponse.GetResponseHeader("Error-Id");
                using (var responseStream = new StreamReader(errorResponse.GetResponseStream()))
                {
                    errorResponseJson = responseStream.ReadToEnd();
                }
            }

            return result;
        }
    }
}
