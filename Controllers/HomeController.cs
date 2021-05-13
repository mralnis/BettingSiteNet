using BettingSiteNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Net;
using BettingSiteNet.Controllers.ViewModels;

namespace BettingSiteNet.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userGuid = new Guid(userId);
            var tournaments = db.Tournaments.Include(t => t.Country).ToList();
            var playersTournaments = db.PlayerTournaments.Where(x => x.ApsnetUserId == userGuid).ToList();
            foreach (var tournament in tournaments)
            {
                tournament.HasUserJoinedTournament = playersTournaments.Any(x => x.TournamentId == tournament.Id);
            }

            return View(tournaments);
        }

        public ActionResult Tournament(int? id, bool? isInTournament)
        {
            if (id == null || isInTournament == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userGuid = new Guid(userId);

            var result = new TournamentMain();
            result.Tournament = db.Tournaments.Include(t => t.Country).Include(t => t.Matchups).Where(x => x.Id == id).First();
            result.Tournament.Matchups = result.Tournament.Matchups.OrderBy(x => x.GameTime).ToList();

            if (!isInTournament.Value)
            {
                var playerTournament = new PlayerTournament()
                {
                    TournamentId = id.Value,
                    ApsnetUserId = userGuid,
                };
                db.PlayerTournaments.Add(playerTournament);
                foreach (var matchup in result.Tournament.Matchups)
                {
                    var prediction = new Prediction()
                    {
                        AspNetUserId = userGuid,
                        MatchupId = matchup.Id,
                    };
                    db.Predictions.Add(prediction);
                }
                db.SaveChanges();
            }


            var players = db.PlayerTournaments.Where(x => x.TournamentId == id).ToList();
            result.PlayerPredictions = new List<PlayerPredictions>();
            foreach (var player in players)
            {
                var playerPrediction = new PlayerPredictions();
                playerPrediction.Name = db.Users.Find(player.ApsnetUserId.ToString()).UserName;
                var predictions = db.Predictions.Include(t=>t.Matchup).Include(t=>t.Matchup.Country).Where(x => x.AspNetUserId == player.ApsnetUserId).OrderBy(x=>x.Matchup.GameTime).ToList();
                playerPrediction.Predictions = predictions;
                result.PlayerPredictions.Add(playerPrediction);
            }

            return View(result);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}