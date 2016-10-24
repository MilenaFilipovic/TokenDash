using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace xx0000xWebAppIEP.Models
{
    
    [MetadataType(typeof(AuctionMetaData))]
    public partial class Auction
    {
        [NotMapped]
        public HttpPostedFileBase ImageToUpload { get; set; }
    }

    public class AuctionMetaData
    {
        

        // [MaxLength(100, ErrorMessage="Image must contain less than 100 bytes.")]
        public byte[] Image { get; set; }
    }
    
}