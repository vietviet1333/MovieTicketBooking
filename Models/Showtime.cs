//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovieTicketBooking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Showtime
    {
        public int showtime_id { get; set; }
        public int movie_id { get; set; }
        public int theater_id { get; set; }
        public int room_id { get; set; }
        public DateTime show_date { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public int status { get; set; }
    
        public virtual Movie Movie { get; set; }
        public virtual Room Room { get; set; }
        public virtual Theater Theater { get; set; }
    }
}
