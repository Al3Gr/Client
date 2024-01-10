using Client.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class PhotoInfoModel : INotifyPropertyChanged
    {
        public ObjectID _id { get; set; }
        public string username { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public List<string> tags { get; set; }
        public List<string> likes { get; set; }

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

        public bool HasMyLike
        {
            get
            {
                string username = UserService.Instance.Username;
                if (username == null)
                    return false;
                return likes.Contains(username);
            }
        }

        public ImageSource LikeColor
        {
            get => HasMyLike ? ImageSource.FromFile("mipiace.png") : ImageSource.FromFile("nomipiace.png");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyHasMyLikeChanged() => NotifyPropertyChanged(nameof(LikeColor));

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

    public class ObjectID
    {
        [JsonProperty("$oid")]
        public string Id { get; set; }
    }
}
