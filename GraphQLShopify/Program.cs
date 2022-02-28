using GraphQL.Res;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://json2csharp.com/
//https://shopify.dev/api/examples/product-media

namespace GraphQL
{
    class Program
    {
        public static Media[] CreateProductMedia()
        {
            Media[] resources = new Media[3];

            resources[0] = new Media(@"http://image.succubus.nl/server/", @"Z:\Image\server\");
            resources[0].Name = "killstar.mp4";
            resources[0].Alt = "Killstar";

            resources[1] = new Media(@"http://image.succubus.nl/server/", @"Z:\Image\server\");
            resources[1].Name = "irregular-choice1.mp4";
            resources[1].Alt = "Irregular Choice";

            resources[2] = new Media(@"http://image.succubus.nl/server/", @"Z:\Image\server\");
            resources[2].Name = "irregular-choice2.mp4";
            resources[2].Alt = "Irregular Choice 2";

            return resources;
        }

        public static StagedUploadsRes Upload(Media[] resources)
        {
            StagedUploads uploads = new StagedUploads();
            StagedUploadsRes res = uploads.Execute(resources);

            return res;
        }

        static void Main(string[] args)
        {
            Media[] media = CreateProductMedia();
            StagedUploadsRes awsBuckets = Upload(media);

            GenerateStagedUploads upload = new GenerateStagedUploads();
            upload.Execute(awsBuckets, media);


            CreateProductMedia create = new CreateProductMedia();
            create.Execute(awsBuckets, media, 4622762639435);            
        }

    }
}
