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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System.Net.Mail;
using log4net;

namespace xx0000xWebAppIEP.Controllers
{

    [Authorize]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //valjda
        private ApplicationUserManager _userManager;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        //valjda

        // GET: Orders
        [AllowAnonymous]
        public ActionResult Index(string clientid, string transactionid, string status, int? page)
        {
            if (String.IsNullOrEmpty(clientid) || String.IsNullOrEmpty(transactionid))
            {
                if (User.Identity.IsAuthenticated)
                {
                    List<Order> orders = db.Orders.Include(o=>o.AspNetUser).ToList();
                    orders = orders.Where(order => order.AspNetUser.Id.Equals(User.Identity.GetUserId())).ToList();

                    int pageSize = 10;
                    int pageNumber = (page ?? 1);

                    return View(orders.ToPagedList(pageNumber, pageSize));

                }
                else
                {
                    return RedirectToAction("Login", "AccountController", new { area = "" });
                }
            }else {

                logger.Error("Order from Centili");

                var user = UserManager.FindById(clientid);
                if (user != null)
                {



                    if (status.Equals("success"))
                    {

                        Order order = db.Orders.Where(o => o.transactionID.Equals(transactionid)).First();
                        if (order != null)
                        {
                            order.Status = true;


                            db.Entry(order).State = EntityState.Modified;
                            db.SaveChanges();
                            user.NoTokens += order.NoTokens;
                            UserManager.Update(user);


                            // sendMailT(user, newamount);


                        }

                    }
                    else if (status.Equals("failed") || status.Equals("canceled"))
                    {
                        int orderID = Convert.ToInt32(transactionid);
                        Order order = db.Orders.Where(o => o.transactionID == transactionid).First();
                        if (order != null)
                        {
                            order.Status = false;
                            db.Entry(order).State = EntityState.Modified;
                            db.SaveChanges();


                        }
                    }
                }


                List<Order> orders = db.Orders.Include(o => o.AspNetUser).ToList();
                    orders = orders.Where(o => o.AspNetUser.Id.Equals(User.Identity.GetUserId())).ToList();
                    //return View(orders);
                    int pageSize = 10;
                    int pageNumber = (page ?? 1);

                    return View(orders.ToPagedList(pageNumber, pageSize));
                }



        }

        static void sendMailT(ApplicationUser usr, int numT)
        {

            MailMessage mail = new MailMessage();



            SmtpClient SmtpServer = new SmtpClient();
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.EnableSsl = true;
            SmtpServer.Host = "smtp.gmail.com";
            SmtpServer.Port = 587;

            // setup Smtp authentication
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("yourEmail@email.com", "******************");
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = credentials;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;


            mail.From = new MailAddress("anotherEmail@email.com, "TokenDash");
            String adrTo = usr.Email;
            mail.To.Add(adrTo);
            mail.Subject = "You have successfully purchased Tokens!";
            mail.Body = "You have successfully purchased "+ numT +" Tokens!";



            SmtpServer.Send(mail);

    }


        [AllowAnonymous]
        public HttpStatusCodeResult PurchaseComplete(string status, string clientId, string transactionid, string enduserprice, int? amount) {

            logger.Error("PurchaseComplete");

            var user = UserManager.FindById(clientId);
            if (user != null && amount!=null)
            {

                logger.Error("Order sent to Centili");
                String userID = clientId;

                Order order = new Order
                {
                    transactionID = transactionid,
                    OrderTime = DateTime.Now.ToUniversalTime(),
                    NoTokens = 0,
                    Price = 0,
                    AspNetUserId = userID

                };

                int newamount;
                if (amount != null)
                {
                    newamount = (int)amount;
                }
                else
                {
                    newamount = 0;
                }

                order.NoTokens = newamount;

                switch (newamount)
                {
                    case 3: order.Price = 50; break;
                    case 5: order.Price = 75; break;
                    case 10: order.Price = 100; break;
                }

                if (status.Equals("failed") || status.Equals("canceled"))
                {

                    //failed trans
                    order.Status = false;
                    logger.Error("Order sent to Centili failed");
                    db.Orders.Add(order);
                    db.SaveChanges();
                    return new HttpStatusCodeResult(406);

                }
                else
                {
                    //pending trans
                    order.Status = null;
                    logger.Error("Order sent to Centili pending");
                    db.Orders.Add(order);
                    db.SaveChanges();
                    return new HttpStatusCodeResult(200);
                }



            }
            return new HttpStatusCodeResult(406);

        }

        // GET: Orders/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
           // return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderTime,NoTokens,Price,Status")] Order order)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /*
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
            */
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(long? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /*
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
            */
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderTime,NoTokens,Price,Status")] Order order)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /*
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
            */
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(long? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /*
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
            */
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /*
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
