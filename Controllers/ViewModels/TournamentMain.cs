using BettingSiteNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BettingSiteNet.Controllers.ViewModels
{
    public class TournamentMain
    {
        public Tournament Tournament { get; set; }
        public List<PlayerPredictions> PlayerPredictions { get; set; }
    }
}