using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using xx0000xWebAppIEP.Models;
using log4net;

namespace xx0000xWebAppIEP.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index(string searchString, string currentSearch, string searchType,string currentSearchType, string statusList, string currentStatus, int? minPrice, int? currentMin, int? maxPrice, int? currentMax, int? page)
        {
            logger.Error("In Home Index");

            bool showTopFive = true;

            List<Auction> allAuctions = db.Auctions.Include(auction=>auction.AspNetUser).ToList();
           // allAuctions = allAuctions.Where(auction => auction.Status >= 2).ToList();
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



            if (!String.IsNullOrEmpty(searchString))
            {
                showTopFive = false;

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

            if (!String.IsNullOrEmpty(statusList))
            {
                showTopFive = false;

                switch (statusList)
                {
                    case "OPEN":
                        auctions = auctions.Where(auction => auction.Status == 2).ToList();
                        break;
                    case "SOLD":
                        auctions = auctions.Where(auction => auction.Status == 3).ToList();
                        break;
                    case "EXPIRED":
                        auctions = auctions.Where(auction => auction.Status == 4).ToList();
                        break;

                }
            }
            else {
                auctions = auctions.Where(auction => auction.Status >= 2).ToList();
            }
            

            if (minPrice != null)
            {
                showTopFive = false;
                auctions = auctions.Where(auction => (auction.StartingPrice + auction.PriceInc) >= minPrice).ToList();
            }
            if (maxPrice != null && maxPrice >= minPrice)
            {
                showTopFive = false;
                auctions = auctions.Where(auction => (auction.StartingPrice + auction.PriceInc) <= maxPrice).ToList();
            }

            auctions = auctions.Where(auction => auction.Active == true).ToList();

            if (showTopFive)
            {
                auctions = allAuctions.Where(auction => auction.Active == true).Where(auction => auction.Status == 2).OrderByDescending(auction => auction.OpeningDT).ToList();
                int numberA = auctions.Count();
                if (numberA < 5)
                {
                    auctions = auctions.Take(numberA).ToList();
                }
                else
                {
                    auctions = auctions.Take(5).ToList();
                }
            }
            

            

            auctions = auctions.OrderByDescending(auction => auction.Status).ThenByDescending(auction => auction.OpeningDT).ToList();
            //////////////////////
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentStatus = statusList;
            ViewBag.CurrentMin = minPrice;
            ViewBag.CurrentMax = maxPrice;
            ViewBag.CurrentSearchType = searchType;
            //////////////////////
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(auctions.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}