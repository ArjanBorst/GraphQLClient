using GraphQL.Res;
using System.IO;
using System.Net.Http;

namespace GraphQL
{
    public class GenerateStagedUploads 
    {       

        public string Execute(StagedUploadsRes uploads, Media[] media)
        {
            if (uploads.data.stagedUploadsCreate.stagedTargets.Count != media.Length)
            {
                return "";
            }

            //Used tool to create code below: https://curl.olsh.me/
            using (var httpClient = new HttpClient())
            {
                for (int i = 0; i < uploads.data.stagedUploadsCreate.stagedTargets.Count; i++)
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://shopify-video-production-core-originals.s3.amazonaws.com/"))
                    {                    
                        var multipartContent = new MultipartFormDataContent();
                        multipartContent.Add(new StringContent(uploads.data.stagedUploadsCreate.stagedTargets[i].parameters[0].value), "bucket");
                        multipartContent.Add(new StringContent(uploads.data.stagedUploadsCreate.stagedTargets[i].parameters[1].value), "key");
                        multipartContent.Add(new StringContent(uploads.data.stagedUploadsCreate.stagedTargets[i].parameters[2].value), "policy");
                        multipartContent.Add(new StringContent(uploads.data.stagedUploadsCreate.stagedTargets[i].parameters[3].value), "cache-control");
                        multipartContent.Add(new StringContent(uploads.data.stagedUploadsCreate.stagedTargets[i].parameters[4].value), "x-amz-signature");
                        multipartContent.Add(new StringContent(uploads.data.stagedUploadsCreate.stagedTargets[i].parameters[5].value), "x-amz-credential");
                        multipartContent.Add(new StringContent(uploads.data.stagedUploadsCreate.stagedTargets[i].parameters[6].value), "x-amz-algorithm");
                        multipartContent.Add(new StringContent(uploads.data.stagedUploadsCreate.stagedTargets[i].parameters[7].value), "x-amz-date");                        
                        multipartContent.Add(new ByteArrayContent(media[i].ReadAllBytes()), "file", media[i].GetFileName());
                        request.Content = multipartContent;

                        var response = httpClient.SendAsync(request);
                        response.Wait();
                        string uri = response.Result.Headers.Location.AbsoluteUri;
                        media[i].AbsoluteUri = response.Result.Headers.Location.AbsoluteUri;
                    }                    
                }

                return "";
            }            
        }
    }
}
