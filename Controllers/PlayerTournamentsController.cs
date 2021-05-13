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
    public class PlayerTournamentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PlayerTournaments
        public ActionResult Index()
        {
            var playerTournaments = db.PlayerTournaments.Include(p => p.Tournament);
            return View(playerTournaments.ToList());
        }

        // GET: PlayerTournaments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerTournament playerTournament = db.PlayerTournaments.Find(id);
            if (playerTournament == null)
            {
                return HttpNotFound();
            }
            return View(playerTournament);
        }

        // GET: PlayerTournaments/Create
        public ActionResult Create()
        {
            ViewBag.TournamentId = new SelectList(db.Tournaments, "Id", "Name");
            return View();
        }

        // POST: PlayerTournaments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TournamentId,ApsnetUserId")] PlayerTournament playerTournament)
        {
            if (ModelState.IsValid)
            {
                db.PlayerTournaments.Add(playerTournament);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TournamentId = new SelectList(db.Tournaments, "Id", "Name", playerTournament.TournamentId);
            return View(playerTournament);
        }

        // GET: PlayerTournaments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerTournament playerTournament = db.PlayerTournaments.Find(id);
            if (playerTournament == null)
            {
                return HttpNotFound();
            }
            ViewBag.TournamentId = new SelectList(db.Tournaments, "Id", "Name", playerTournament.TournamentId);
            return View(playerTournament);
        }

        // POST: PlayerTournaments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TournamentId,ApsnetUserId")] PlayerTournament playerTournament)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playerTournament).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TournamentId = new SelectList(db.Tournaments, "Id", "Name", playerTournament.TournamentId);
            return View(playerTournament);
        }

        // GET: PlayerTournaments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerTournament playerTournament = db.PlayerTournaments.Find(id);
            if (playerTournament == null)
            {
                return HttpNotFound();
            }
            return View(playerTournament);
        }

        // POST: PlayerTournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlayerTournament playerTournament = db.PlayerTournaments.Find(id);
            db.PlayerTournaments.Remove(playerTournament);
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
