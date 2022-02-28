using GraphQL.Res;
using System;
using System.Linq;

namespace GraphQL
{
    public class CreateProductMedia
    {
        public string Response { get; set; }
        public string ErrorMessage { get; set; }
        public bool Execute(StagedUploadsRes aws, Media[] resources, long productId)
        {
            if (aws.data.stagedUploadsCreate.stagedTargets.Count != resources.Length)
            {
                return false;
            }
            Media last = resources.Last();

            string query = "{" +
                "\"query\":\" mutation createProductMedia($id: ID! $media:[CreateMediaInput!]!) {" +
                "   productCreateMedia(productId: $id, media: $media) { " +
                "       media { " +
                "           mediaErrors { code details message } " +
                "       } " +
                "       product { id } " +
                "       mediaUserErrors { code field message } " +
                "   }" +
                "}\",\"variables\":{ " +
                "  \"id\": \"gid://shopify/Product/" + productId + "\"," +
                "  \"media\": [";

            for (int i = 0; i < resources.Length; i++)
            {
                string resourceURL = aws.data.stagedUploadsCreate.stagedTargets[i].resourceUrl;
                query += "   {" +
                        "      \"originalSource\": \"" + resourceURL + "\"," +
                        "      \"alt\": \"" + resources[i].Alt + "\", " +
                        "      \"mediaContentType\": \"" + resources[i].Type() + "\" " +
                        "   } ";

                if (!resources[i].Equals(last))
                    query += "   , ";
            }

            query += "   ]" +
                "  }" +
                "}";

            return Execute(query);
        }
        private bool Execute(string query)
        {
            Response ="";
            ErrorMessage = "";

            using (var client = new GraphQLClient("https://www-succubus-com.myshopify.com/admin/api/graphql.json", "", ""))
            {
                if (client.Execute(query))
                {                    
                    Response= client.Response;
                    return true;
                }
                else
                {
                    ErrorMessage = client.ErrorMessage;
                    Console.WriteLine(client.ErrorMessage);
                    return false;
                }
            }
        }            
    }
}
