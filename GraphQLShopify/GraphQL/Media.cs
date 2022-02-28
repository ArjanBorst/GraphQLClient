using System.IO;


namespace GraphQL.Res
{
    public class Media
    {
        public string AbsoluteUri { get; set; }
        public string BaseUrl { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string Alt { get; set; }
        public Media(string baseUrl, string location)
        {
            BaseUrl = baseUrl;
            Location = location;
        }
        private Media()
        {
        }
        public byte[] ReadAllBytes()
        {
            return File.ReadAllBytes(Location + Name);
        }
        public string GetFileName()
        {
            return Path.GetFileName(Location + Name);
        }

       
        public string URL()
        {
            return BaseUrl + Name;
        }
        public long Size()
        {
            string fileName = Location + Name;
            FileInfo fi = new FileInfo(fileName);
            return fi.Length;
        }
        public string MimeType()
        {
            if (Name.ToLower().EndsWith(".mp4"))
            {
                return "video/mp4";
            }
            return "";
        }
        public string Type()
        {
            if (Name.ToLower().EndsWith(".mp4"))
            {
                return "VIDEO";
            }
            return "";
        }
    }
}
