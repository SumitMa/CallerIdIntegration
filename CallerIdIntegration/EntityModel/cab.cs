//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CallerIdIntegration.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class cab
    {
        public int id { get; set; }
        public Nullable<int> dispacher_id { get; set; }
        public string cab_name { get; set; }
        public Nullable<decimal> price_per_unit { get; set; }
        public Nullable<decimal> original_price { get; set; }
        public Nullable<int> available_seats { get; set; }
        public Nullable<byte> status { get; set; }
    }
}
