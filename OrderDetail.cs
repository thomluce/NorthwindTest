//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NorthwindTest
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public int OrderDetailKey { get; set; }
        public int orderKey { get; set; }
        public int productKey { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<short> Quantity { get; set; }
        public Nullable<float> Discount { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
