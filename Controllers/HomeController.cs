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

        public ActionResult List()
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

        public ActionResult Index(int? id)
        {
            var tournament = db.Tournaments.SingleOrDefault(x => x.IsActive);
            if (tournament != null)
            {
                id = tournament.Id;
            }
            if (id == null)
            {
                return RedirectToAction("List", "Home");
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

            var isInTournament = db.PlayerTournaments.Any(x => x.TournamentId == id.Value && x.ApsnetUserId == userGuid);
            if (!isInTournament)
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
            
            foreach (var matchup in result.Tournament.Matchups)
            {
                if (matchup.GameTime > DateTime.UtcNow.AddHours(3).AddMinutes(result.Tournament.MatchupClosingTime))
                {
                    matchup.CanVote = true;
                }
            }
            var allCountries = db.Countries.ToList();
            var cc = allCountries.Last();
            var players = db.PlayerTournaments.Where(x => x.TournamentId == id).ToList().OrderByDescending(x => x.ApsnetUserId == userGuid);
            result.PlayerPredictions = new List<PlayerPredictions>();
            foreach (var player in players)
            {
                var playerPrediction = new PlayerPredictions();
                playerPrediction.Name = db.Users.Find(player.ApsnetUserId.ToString()).UserName;
                var predictions = db.Predictions.Include(t => t.Matchup).Include(t => t.Matchup.Country).Where(x => x.AspNetUserId == player.ApsnetUserId).OrderBy(x => x.Matchup.GameTime).ToList();
                playerPrediction.Predictions = predictions;
                playerPrediction.Total = predictions.Sum(x=>x.PointsEarned ?? 0);
                foreach (var prediction in predictions)
                {
                    if (prediction.Matchup.GameTime > DateTime.UtcNow.AddHours(3).AddMinutes(result.Tournament.MatchupClosingTime) && prediction.AspNetUserId != userGuid)
                    {
                        prediction.EnemyTeamScoreText = GetHiddenScoreText(prediction.EnemyTeamScore);
                        prediction.HomeTeamScoreText = GetHiddenScoreText(prediction.HomeTeamScore);
                    }
                    else
                    {
                        prediction.EnemyTeamScoreText = prediction.EnemyTeamScore?.ToString() ?? "-";
                        prediction.HomeTeamScoreText = prediction.HomeTeamScore?.ToString() ?? "-";
                    }
                }

                result.PlayerPredictions.Add(playerPrediction);
            }
            result.PlayerPredictions = result.PlayerPredictions.OrderByDescending(x => x.Total).ToList();
            return View(result);
        }

        private static string GetHiddenScoreText(int? score)
        {
            if (score.HasValue)
            {
                return "?";
            }
            else
            {
                return "-";
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePrediction([Bind(Include = "MatchupId,HomeTeamScore,EnemyTeamScore")] Prediction prediction)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var userGuid = new Guid(userId);

                var existingPrediction = db.Predictions.Where(x => x.AspNetUserId == userGuid && x.MatchupId == prediction.MatchupId).FirstOrDefault();
                if (existingPrediction == null)
                {
                    existingPrediction = new Prediction()
                    {
                        MatchupId = prediction.MatchupId,
                        AspNetUserId = userGuid,
                    };
                    existingPrediction.HomeTeamScore = prediction.HomeTeamScore;
                    existingPrediction.EnemyTeamScore = prediction.EnemyTeamScore;
                    db.Predictions.Add(existingPrediction);
                }
                existingPrediction.HomeTeamScore = prediction.HomeTeamScore;
                existingPrediction.EnemyTeamScore = prediction.EnemyTeamScore;

                db.SaveChanges();
            }
            return new EmptyResult();
        }
    }
}