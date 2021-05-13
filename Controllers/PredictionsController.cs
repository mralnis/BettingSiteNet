using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BettingSiteNet.Models;

namespace BettingSiteNet.Controllers
{
    public class PredictionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Predictions
        public ActionResult Index()
        {
            var predictions = db.Predictions.Include(p => p.Matchup);
            return View(predictions.ToList());
        }

        // GET: Predictions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prediction prediction = db.Predictions.Find(id);
            if (prediction == null)
            {
                return HttpNotFound();
            }
            return View(prediction);
        }

        // GET: Predictions/Create
        public ActionResult Create()
        {
            ViewBag.MatchupId = new SelectList(db.Matchups, "Id", "Id");
            return View();
        }

        // POST: Predictions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MatchupId,AspNetUserId,HomeTeamScore,EnemyTeamScore")] Prediction prediction)
        {
            if (ModelState.IsValid)
            {
                db.Predictions.Add(prediction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MatchupId = new SelectList(db.Matchups, "Id", "Id", prediction.MatchupId);
            return View(prediction);
        }

        // GET: Predictions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prediction prediction = db.Predictions.Find(id);
            if (prediction == null)
            {
                return HttpNotFound();
            }
            ViewBag.MatchupId = new SelectList(db.Matchups, "Id", "Id", prediction.MatchupId);
            return View(prediction);
        }

        // POST: Predictions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MatchupId,AspNetUserId,HomeTeamScore,EnemyTeamScore")] Prediction prediction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prediction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MatchupId = new SelectList(db.Matchups, "Id", "Id", prediction.MatchupId);
            return View(prediction);
        }

        // GET: Predictions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prediction prediction = db.Predictions.Find(id);
            if (prediction == null)
            {
                return HttpNotFound();
            }
            return View(prediction);
        }

        // POST: Predictions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prediction prediction = db.Predictions.Find(id);
            db.Predictions.Remove(prediction);
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
