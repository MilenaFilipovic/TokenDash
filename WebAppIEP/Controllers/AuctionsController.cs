using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using xx0000xWebAppIEP.Models;
using PagedList;
using log4net;

namespace xx0000xWebAppIEP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuctionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // [AllowAnonymous] //na home pageu ce biti prikaz za ne adminske korisnike i neautorizovane korisnike uopsteno
        // GET: Auctions
        public ActionResult Index(string searchString, string currentSearch, string searchType, string currentSearchType, string statusList, string currentStatus, int? minPrice, int? currentMin, int? maxPrice, int? currentMax, int? page)
        {
            logger.Error("In Auction Index");

            List<Auction> allAuctions = db.Auctions.Include(auction => auction.AspNetUser).ToList();
            List<Auction> auctions = allAuctions;            

            if (String.IsNullOrEmpty(searchString))
            {
                searchString = currentSearch;
            }
            else
            {
                page = 1;
            }


            if (String.IsNullOrEmpty(searchType))
            {
                searchType = currentSearchType;
            }
            


            if (String.IsNullOrEmpty(statusList))
            {
                statusList = currentStatus;
            }
            else
            {
                page = 1;
            }

            if (minPrice == null)
            {
                minPrice = currentMin;
            }
            else
            {
                page = 1;
            }

            if (maxPrice == null)
            {
                maxPrice = currentMax;
            }
            else
            {
                page = 1;
            }


            if (!String.IsNullOrEmpty(searchString)) {

                string[] strings = searchString.Split(' ');

                if (String.IsNullOrEmpty(searchType)) searchType = "ANY";

                if (searchType.Equals("ANY"))
                {
                    auctions = new List<Auction>();
                    foreach (string str in strings)
                    {
                        auctions = auctions.Union(allAuctions.Where(auction => auction.ProductName.Contains(str))).ToList();
                    }

                }
                else
                {
                    foreach (string str in strings)
                    {
                        auctions = auctions.Where(auction => auction.ProductName.Contains(str)).ToList();
                    }
                }
            }

            if (!String.IsNullOrEmpty(statusList)) {

                switch (statusList) {
                    case "READY":
                        auctions = auctions.Where(auction => auction.Status==1).ToList();
                        break;
                    case "OPEN":
                        auctions = auctions.Where(auction => auction.Status == 2).ToList();
                        break;
                    case "SOLD":
                        auctions = auctions.Where(auction => auction.Status == 3).ToList();
                        break;
                    case "EXPIRED":
                        auctions = auctions.Where(auction => auction.Status == 4).ToList();
                        break; 
                        /*                  
                    case "DRAFT":
                        auctions = auctions.Where(auction => auction.Status == -1).ToList();
                        break;
                        */

                }
            }

            if (minPrice != null)
            {
                auctions = auctions.Where(auction => (auction.StartingPrice + auction.PriceInc) >= minPrice).ToList();
            }
            if (maxPrice != null && maxPrice >= minPrice)
            {
                auctions = auctions.Where(auction => (auction.StartingPrice + auction.PriceInc) <= maxPrice).ToList();
            }
            auctions = auctions.Where(auction => auction.Active == true).ToList();
            auctions = auctions.OrderBy(auction => auction.Status).ThenByDescending(auction => auction.OpeningDT).ToList();
//////////////////////
            ViewBag.CurrentName = searchString;
            ViewBag.CurrentStatus = statusList;
            ViewBag.CurrentMin = minPrice;
            ViewBag.CurrentMax = maxPrice;
            ViewBag.CurrentSearchType = searchType;
            //////////////////////
            //return View(auctions);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(auctions.ToPagedList(pageNumber, pageSize));
        }

        [AllowAnonymous]
        // GET: Auctions/Details/5
        public ActionResult Details(long? id)
        {
            logger.Error("In Auction Details");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }


        // POST: Auctions/Open/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Open(long? id)
        {
            logger.Error("Openin auction");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }

            if (auction.Status != 1)
            {
               return new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

            //
            DateTime now = DateTime.Now.ToUniversalTime();
            auction.Status = 2;            
            auction.OpeningDT = now;
            auction.ClosingDT = now.AddSeconds(auction.Duration);
            //
            db.Entry(auction).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        // GET: Auctions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductName,Duration,StartingPrice,ImageToUpload")] Auction auction)
        {


            if (ModelState.IsValid)
            {

                //dodato
                // Convert HttpPostedFileBase to byte array.
                if (auction.ImageToUpload != null)
                {
                    logger.Error("Creating Auction");

                    auction.Image = new byte[auction.ImageToUpload.ContentLength];
                    auction.ImageToUpload.InputStream.Read(auction.Image, 0, auction.Image.Length);
                    auction.CreationDT = DateTime.Now.ToUniversalTime();
                    auction.PriceInc = 0;
                    auction.Status = 1;
                    auction.Active = true;
                    db.Auctions.Add(auction);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    //dodato

                }//neko ovo za sad bude provera za ne unosenje slike 
                
            }

            return View(auction);
        }

        // GET: Auctions/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }

            if (auction.Status == 1 )
            {
                return View(auction);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // POST: Auctions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductName,Duration,StartingPrice, CreationDT, OpeningDT, ClosingDT,Status,ImageToUpload, PriceInc,Image,Active, RowVersion")] Auction auction)
        {
            if (ModelState.IsValid)
            {
                if (auction.Status == 1) {
                    logger.Error("Editing Auction");
                    //dodato
                    // Convert HttpPostedFileBase to byte array.
                    if (auction.ImageToUpload != null)
                    {
                        auction.Image = new byte[auction.ImageToUpload.ContentLength];
                        auction.ImageToUpload.InputStream.Read(auction.Image, 0, auction.Image.Length);
                    }
                    //dodato

                    db.Entry(auction).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                
            }
            return View(auction);
        }

        // GET: Auctions/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            if (auction.Status == 1)
            {
                return View(auction);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // POST: Auctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            
            Auction auction = db.Auctions.Find(id);
            if (auction.Status == 1)
            {
                logger.Error("Deleting Auction");
                auction.Status = -1;
                auction.Active = false;
                db.Entry(auction).State = EntityState.Modified;
                //db.Auctions.Remove(auction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                logger.Error("Tried to delete that is not in ready state");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
