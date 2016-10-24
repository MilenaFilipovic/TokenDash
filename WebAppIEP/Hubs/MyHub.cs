using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using xx0000xWebAppIEP.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.ModelBinding;

namespace xx0000xWebAppIEP.Hubs
{
    public class MyHub : Hub
    {

        private ApplicationDbContext db = new ApplicationDbContext();


        public void BidPlaced(String buttonId , byte[] rowVersion)
        {

            Processing(buttonId, rowVersion);
           
        }


        private void Processing(String aucId, byte[] rowVersion)
        {
            
            String username = HttpContext.Current.User.Identity.Name.ToString();         
            String bidderId = HttpContext.Current.User.Identity.GetUserId();

            int auctionID = Convert.ToInt32(aucId);
            int priceInc = 0;
            String aucExtended = "false";


            Auction biddedAuc = db.Auctions.Find(auctionID);
            if (biddedAuc != null)
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser user = UserManager.FindById(bidderId);

                DateTime currentTime = DateTime.Now.ToUniversalTime();

                if (biddedAuc.ClosingDT < currentTime) {
                    Clients.Caller.auctionOver();
                    return;
                    //izlazak ako je istekla aukcija
                }


                if (user.NoTokens < (biddedAuc.StartingPrice + biddedAuc.PriceInc)) {
                    Clients.Caller.insufFunds();
                    return;
                    //izlazak ako nema dovoljno tokena
                }

                //ima dovoljno tokena

               

                try
                {  
                    biddedAuc.AspNetUserId = bidderId;

                    biddedAuc.PriceInc++;
                    priceInc = biddedAuc.PriceInc-1;

                    DateTime closing = (DateTime)biddedAuc.ClosingDT;

                    if ((closing.Subtract(currentTime).TotalSeconds >= 0) && (closing.Subtract(currentTime).TotalSeconds < 10))
                    {
                        DateTime opening = (DateTime)biddedAuc.OpeningDT;
                        int dif = (int)closing.Subtract(currentTime).TotalSeconds;
                        biddedAuc.Duration += (10-dif);
                        biddedAuc.ClosingDT = opening.AddSeconds(biddedAuc.Duration);
                        //produzena aukcija

                        aucExtended = "true";
                    }

                    db.Entry(biddedAuc).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();

                    Auction newAuction = db.Auctions.Find(auctionID);
                    rowVersion = newAuction.RowVersion;

                    Offer offer = new Offer
                    {
                        AuctionId = auctionID,
                        AspNetUserId = bidderId,
                        OfferTime = currentTime,
                        Quantity = biddedAuc.StartingPrice + priceInc,
                        Outcome = false
                    };

                    db.Offers.Add(offer);
                    db.SaveChanges();
                   
                    
                    user.NoTokens = user.NoTokens - offer.Quantity;
                    UserManager.Update(user);

                    priceInc++;
                    Clients.All.bidOutcome(username, auctionID, priceInc, rowVersion, aucExtended);
                    return;


                }
                catch (Exception ex)
                {
                    Clients.Caller.bidFailed();
                    return;
                    //izlazak ako je bio problem sa row version
                }



            }
            else
            {
                
                Clients.Caller.bidFailed();
                return;
            }

        }



    }
}