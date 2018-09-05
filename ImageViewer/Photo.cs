using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaiwaProjects
{
    public class Photo
    {
        public Photo(string id, string name, string desc, string path, string infoTags)
        {
            this.id = id;
            this.name = name;
            this.description = desc;
            this.path = path;
            this.infoTags = infoTags;
        }

        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public byte[] photo { get; set; }
        public string path { get; set; }
        public Nullable<System.DateTimeOffset> time_stamp { get; set; }
        public string infoTags { get; set; }
    }
}
