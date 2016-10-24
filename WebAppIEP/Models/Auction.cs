namespace xx0000xWebAppIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class Auction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Auction()
        {
            this.Offers = new HashSet<Offer>();
        }

        public long Id { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Auction Duration")]
        public int Duration { get; set; }

        [Display(Name = "Starting Price")]
        public int StartingPrice { get; set; }

        [Display(Name = "Creation Time")]
        public System.DateTime CreationDT { get; set; }

        [Display(Name = "Opening Time")]
        public Nullable<System.DateTime> OpeningDT { get; set; }

        [Display(Name = "Closing Time")]
        public Nullable<System.DateTime> ClosingDT { get; set; }

        public int Status { get; set; }
      
        public byte[] Image { get; set; } 

        [Display(Name = "Price Increment")]
        public int PriceInc { get; set; }
        public bool Active { get; set; }


        //
        [StringLength(128)]
        [Display(Name = "Last Bidder")]
        public string AspNetUserId { get; set; }
        public virtual ApplicationUser AspNetUser { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }

        //

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Offer> Offers { get; set; }
    }
}