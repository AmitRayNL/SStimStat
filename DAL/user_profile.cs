//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SStimStat.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class user_profile
    {
        public string id { get; set; }
        public string email { get; set; }
        public string sign_in_domain { get; set; }
        public sbyte user_status { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public Nullable<System.DateTime> status_change_on { get; set; }
    }
}
