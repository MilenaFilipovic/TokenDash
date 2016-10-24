namespace xx0000xWebAppIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Offer
    {
        public long Id { get; set; }

        //[Required]
        // [StringLength(128)]
        // public string UserId { get; set; }

        //  public long AuctionId { get; set; }


        [Display(Name = "Offer Time")]
        public DateTime OfferTime { get; set; }

        public bool Outcome { get; set; }

        [Display(Name = "Tokens Paid")]
        public int Quantity { get; set; }

        [Required]
        public long AuctionId { get; set; }
        public virtual Auction Auction { get; set; }

        [Required]
        [StringLength(128)]
        public string AspNetUserId { get; set; }
        public virtual ApplicationUser AspNetUser { get; set; }
    }
}