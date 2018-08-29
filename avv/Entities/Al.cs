using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace AV
{
    public struct Al
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ReadOnlyCollection<Ph> Phs { get; set; }
    }
}
