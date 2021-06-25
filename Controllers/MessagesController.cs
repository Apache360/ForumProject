using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models;

namespace ForumProject.Controllers
{
    public class MessagesController : Controller
    {
        private Entities db = new Entities();

        // GET: Messages
        public ActionResult Index()
        {
            var messages = db.Messages.Include(m => m.Topics).Include(m => m.Users);
            return View(messages.ToList());
        }

        // GET: Messages/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = db.Messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        // GET: Messages/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.IdTopic = new SelectList(db.Topics, "Id", "TopicName");
            ViewBag.IdUser = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        // POST: Messages/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,IdUser,IdTopic,Text,DataTime")] Messages messages, int? id)
        {
            if (ModelState.IsValid)
            {
                messages.IdUser = Int32.Parse(User.Identity.Name);
                messages.Datetime = DateTime.Now;
                messages.Id = GenerateUniqueId();
                messages.IdTopic = (int)id;
                db.Messages.Add(messages);
                db.SaveChanges();
                return RedirectToAction($"View/{messages.IdTopic}", "Topics");
            }

            ViewBag.IdTopic = new SelectList(db.Topics, "Id", "TopicName", messages.IdTopic);
            ViewBag.IdUser = new SelectList(db.Users, "Id", "UserName", messages.IdUser);
            return View("Index", "Home");
        }

        int GenerateUniqueId()
        {
            var Id = new Random().Next(0, Int32.MaxValue);
            var tempMessage = db.Messages.Find(Id);
            while (db.Messages.Contains(tempMessage))
            {
                Id = new Random().Next(0, Int32.MaxValue);
                tempMessage = db.Messages.Find(Id);
            }
            return Id;
        }

        // GET: Messages/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = db.Messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTopic = new SelectList(db.Topics, "Id", "TopicName", messages.IdTopic);
            ViewBag.IdUser = new SelectList(db.Users, "Id", "UserName", messages.IdUser);
            return View(messages);
        }

        // POST: Messages/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,IdUser,IdTopic,Text,Datetime")] Messages messages)
        {
            if (ModelState.IsValid)
            {
                //messages.Datetime = DateTime.Now;
                db.Entry(messages).State = EntityState.Modified;
                System.Diagnostics.Debug.WriteLine(messages.Datetime);
                //System.Diagnostics.Debug.WriteLine(db.Entry(messages).Entity.Datetime.ToString());          

                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.IdTopic = new SelectList(db.Topics, "Id", "TopicName", messages.IdTopic);
            ViewBag.IdUser = new SelectList(db.Users, "Id", "UserName", messages.IdUser);
            return View(messages);
        }

        // GET: Messages/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = db.Messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Messages messages = db.Messages.Find(id);
            db.Messages.Remove(messages);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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
