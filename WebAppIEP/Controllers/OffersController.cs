using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using xx0000xWebAppIEP.Models;
using PagedList;
using log4net;


namespace xx0000xWebAppIEP.Controllers
{
    [Authorize]
    public class OffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Offers
        public ActionResult Index(int? page)
        {
            logger.Error("In Offers Index");
            //  List<Offer> offers = db.Offers.Include(offer=>offer.AspNetUser).Include(offer=>offer.Auction).ToList();
            //   offers = offers.Where(offer => offer.AspNetUser.Id.Equals( User.Identity.GetUserId())).ToList();
            //  offers = offers.Where(offer => offer.Outcome==true).ToList();
            String userID = User.Identity.GetUserId();
            List<Auction> wonAuc = db.Auctions.Include(won => won.AspNetUser).Where(won=>won.Status==3).Where(won => won.AspNetUser.Id.Equals(userID)).ToList();

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(wonAuc.ToPagedList(pageNumber, pageSize));
            
        }

        // GET: Offers/Details/5
        public ActionResult Details(long? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            /*          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            if (!offer.Outcome)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!offer.AspNetUser.Id.Equals(User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            long aucID = offer.AuctionId;
            return RedirectToAction("Details", "Auction", new { id = aucID });
            */
            /*
            return View(offer);
            */
        }

        // GET: Offers/Create
        public ActionResult Create()
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //ovoga nema
            // return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OfferTime,Outcome,Quantity")] Offer offer)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /*
            if (ModelState.IsValid)
            {
                db.Offers.Add(offer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(offer);
            */
        }

        // GET: Offers/Edit/5
        public ActionResult Edit(long? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /*
            //ovoga nema
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
            */
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OfferTime,Outcome,Quantity")] Offer offer)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /*
            if (ModelState.IsValid)
            {
                db.Entry(offer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(offer);
            */
        }

        // GET: Offers/Delete/5
        public ActionResult Delete(long? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /*
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
            */
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /*
            Offer offer = db.Offers.Find(id);
            db.Offers.Remove(offer);
            db.SaveChanges();
            return RedirectToAction("Index");
            */
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
