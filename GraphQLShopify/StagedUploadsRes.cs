using GraphQL.Res;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraphQL
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
  
    public class StagedUploads
    {
        private JsonSerializerSettings _Settings;
        public void InitJSON()
        {

            _Settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }
        public StagedUploadsRes Execute(Media[] resources)
        {
            Media last = resources.Last();

            string query = "{" +
                "   \"query\":\" mutation generateStagedUploads($input: [StagedUploadInput!]!) {" +
                "       stagedUploadsCreate(input: $input) {" +
                "           stagedTargets {" +
                "               url" +
                "               resourceUrl" +
                "               parameters { name value }" +
                "           } " +
                "       }" +
                "   }\",\"variables\": {" +
                "       \"input\":[";

            foreach (Media resource in resources) {
                query += "      {" +
                    "           \"filename\":\"" + resource.URL() + "\"," +
                    "           \"mimeType\":\"" + resource.MimeType() + "\"," +
                    "           \"resource\":\"" + resource.Type() + "\"," +
                    "           \"fileSize\":\"" + resource.Size() + "\"" +
                    "       }";
                if (!resource.Equals(last))
                    query += "   , ";
            }

            query += "       ]" +
                "   }," +
                "   \"operationName\":\"generateStagedUploads\"" +
                "}";

            using (GraphQLClient client = new GraphQLClient("https://www-succubus-com.myshopify.com/admin/api/graphql.json", "", ""))
            {
                if (client.Execute(query))
                {
                    StagedUploadsRes res = Newtonsoft.Json.JsonConvert.DeserializeObject<StagedUploadsRes>(client.Response, _Settings);
                    return res;
                }
                else
                {
                    Console.WriteLine(client.ErrorMessage);
                    return null;
                }                
            }
        }
    }
}