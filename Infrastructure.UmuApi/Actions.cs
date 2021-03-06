﻿using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Infrastructure.UmuApi
{
    public class Actions
    {
        public List<ItsPerson> GetPersonFromUmuApi()
        {
            var url = "https://websrv06741.testads.umu.se/Catalogue/5.0/api/org/e841c73b5524c220b45690b8e1d841c87c454436/person";

            var result = new RootObject();

            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
                myReq.Method = "GET";
                myReq.Credentials = new NetworkCredential("sa34e0014", "Pwj67hang3");
                HttpWebResponse myResponse = (HttpWebResponse)myReq.GetResponse();

                if (myResponse.StatusCode == HttpStatusCode.OK)
                {
                    Stream rebut = myResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(rebut, Encoding.UTF8); // Pipes the stream to a higher level stream reader with the required encoding format. 
                    string rawData = readStream.ReadToEnd();

                    result = JsonConvert.DeserializeObject<RootObject>(rawData);

                    myResponse.Close();
                    readStream.Close();
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

            return result.Persons;
        }
    }
}
