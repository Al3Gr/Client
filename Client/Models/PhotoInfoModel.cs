using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class PhotoInfoModel
    {
        public string username { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public List<string> tags { get; set; }

        public ImageSource Image
        {
            get
            {
                try
                {
                    return ImageSource.FromUri(new Uri(url));
                }
                catch (Exception)
                {
                    return ImageSource.FromFile("nofoto.png");
                }
            }
        }

        public string TagsString
        {
            get
            {
                string output = "";
                if (tags.Count == 0)
                    output = "Non ci sono tag!";

                foreach (string tag in tags)
                    output += tag + " - ";
                if (tags.Count > 0)
                    output = output.Substring(0, output.Length - 3);

                return output;
            }
        }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
