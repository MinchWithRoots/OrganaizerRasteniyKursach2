//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1.AppData
{
    using System;
    using System.Collections.Generic;
    
    public partial class Payments
    {
        public int payment_id { get; set; }
        public int order_id { get; set; }
        public System.DateTime payment_date { get; set; }
        public decimal amount { get; set; }
        public string payment_method { get; set; }
        public string status { get; set; }
    
        public virtual Orders Orders { get; set; }
    }
}
