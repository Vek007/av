//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AV
{
    using System;
    using System.Collections.Generic;
    
    public partial class al
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public al()
        {
            this.phs = new HashSet<ph>();
        }
    
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Nullable<int> r { get; set; }
        public Nullable<int> g { get; set; }
        public Nullable<int> b { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ph> phs { get; set; }
    }
}