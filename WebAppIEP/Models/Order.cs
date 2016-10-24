namespace xx0000xWebAppIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Order
    {
        public long Id { get; set; }
        // public string UserId { get; set; }
        [Display(Name = "Order Time")]
        public DateTime OrderTime { get; set; }

        [Display(Name = "Number of Tokens")]
        public int NoTokens { get; set; }

        public int Price { get; set; }

        public Nullable<bool> Status { get; set; }

        public String transactionID { get; set; }

        [Required]
        [StringLength(128)]
        public string AspNetUserId { get; set; }
        public virtual ApplicationUser AspNetUser { get; set; }
    }
}