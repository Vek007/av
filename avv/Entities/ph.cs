using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AV
{
    public struct Ph
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreationDate { get; set; }
        public string FilePath { get; set; }
    }
}
