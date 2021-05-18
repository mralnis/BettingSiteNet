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
    public class MatchupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Matchups
        public ActionResult Index()
        {
            var matchups = db.Matchups.Include(m => m.Country).Include(m => m.Tournament);
            return View(matchups.ToList());
        }

        // GET: Matchups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matchup matchup = db.Matchups.Find(id);
            if (matchup == null)
            {
                return HttpNotFound();
            }
            return View(matchup);
        }

        // GET: Matchups/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name");
            ViewBag.TournamentId = new SelectList(db.Tournaments, "Id", "Name");
            return View();
        }

        // POST: Matchups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CountryId,GameTime,TournamentId,HomeTeamScore,EnemyTeamScore")] Matchup matchup)
        {
            if (ModelState.IsValid)
            {
                db.Matchups.Add(matchup);
                var players = db.PlayerTournaments.Where(x => x.TournamentId == matchup.TournamentId).ToList();
                foreach (var player in players)
                {
                    var prediction = new Prediction()
                    {
                        AspNetUserId = player.ApsnetUserId,
                        MatchupId = matchup.Id,
                    };
                    db.Predictions.Add(prediction);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", matchup.CountryId);
            ViewBag.TournamentId = new SelectList(db.Tournaments, "Id", "Name", matchup.TournamentId);
            return View(matchup);
        }

        // GET: Matchups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matchup matchup = db.Matchups.Find(id);
            if (matchup == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", matchup.CountryId);
            ViewBag.TournamentId = new SelectList(db.Tournaments, "Id", "Name", matchup.TournamentId);
            return View(matchup);
        }

        // POST: Matchups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CountryId,GameTime,TournamentId,HomeTeamScore,EnemyTeamScore")] Matchup matchup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matchup).State = EntityState.Modified;
                if (matchup.HomeTeamScore != null)
                {
                    SetScore(matchup);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", matchup.CountryId);
            ViewBag.TournamentId = new SelectList(db.Tournaments, "Id", "Name", matchup.TournamentId);
            return View(matchup);
        }

        private void SetScore(Matchup matchup)
        {
            var predictions = db.Predictions.Where(x => x.MatchupId == matchup.Id).ToList();
            var tournament = db.Tournaments.Find(matchup.TournamentId);
            foreach (var prediction in predictions)
            {
                if (prediction.HomeTeamScore == null)
                {
                    continue;
                }
                if (prediction.HomeTeamScore == matchup.HomeTeamScore && prediction.EnemyTeamScore == matchup.EnemyTeamScore)
                {
                    prediction.PointsEarned = tournament.PointsForPerfect;
                }
                else
                {
                    if (prediction.HomeTeamScore - prediction.EnemyTeamScore == matchup.HomeTeamScore - matchup.EnemyTeamScore)
                    {
                        prediction.PointsEarned = tournament.PointForDifference;
                    }
                    else
                    {
                        if (prediction.HomeTeamScore > prediction.EnemyTeamScore == matchup.HomeTeamScore > matchup.EnemyTeamScore)
                        {
                            prediction.PointsEarned = tournament.PointsForWinnerOnly;
                        }
                        else
                        {
                            prediction.PointsEarned = 0;
                        }
                    }
                }
            }
        }

        // GET: Matchups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matchup matchup = db.Matchups.Find(id);
            if (matchup == null)
            {
                return HttpNotFound();
            }
            return View(matchup);
        }

        // POST: Matchups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Matchup matchup = db.Matchups.Find(id);
            db.Matchups.Remove(matchup);
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
