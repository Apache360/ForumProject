using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ForumProject.Models;

namespace ForumProject.Controllers
{
    public class UsersController : Controller
    {
        private Entities db = new Entities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Email,Password,Role")] Users user)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(x => x.UserName == user.UserName))
                {
                    ViewBag.DublicateMessage = "Username already exists.";
                    return View(user);
                }
                user.Id = GenerateUniqueId();
                db.Users.Add(user);
                db.SaveChanges();
                ViewBag.SuccessfulMessage = "Successful registration.";
                FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                return View(user);
            }

            return View(user);
        }

        int GenerateUniqueId()
        {
            var Id = new Random().Next(0, Int32.MaxValue);
            var tempUser = db.Users.Find(Id);
            while (db.Users.Contains(tempUser))
            {
                Id = new Random().Next(0, Int32.MaxValue);
                tempUser = db.Users.Find(Id);
            }
            return Id;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users user)
        {
            //if (ModelState.IsValid)
            {
                Users tempUser = db.Users.FirstOrDefault(u => u.UserName == user.UserName
                    && u.Password == user.Password);
                if (tempUser != null)
                {
                    FormsAuthentication.SetAuthCookie(tempUser.Id.ToString(), false);                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.LoginErrorMessage = "There is no such user.";
                }
            }
            return View(user);
        }

        [Authorize]
        public ActionResult SignOut(Users user)
        {
            Users tempUser = db.Users.FirstOrDefault(u => u.UserName == user.UserName
                    && u.Password == user.Password);
            System.Diagnostics.Debug.WriteLine(FormsAuthentication.Timeout);
            FormsAuthentication.SignOut(); 
            if (tempUser != null)
            {
                System.Diagnostics.Debug.WriteLine(FormsAuthentication.Timeout);                
                FormsAuthentication.SignOut();
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,UserName,Email,Password,Role")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(users);
        }

        // GET: Users/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
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
