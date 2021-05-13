using BettingSiteNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BettingSiteNet.Controllers.ViewModels
{
    public class PlayerPredictions
    {
        public string Name { get; set; }
        public List<Prediction> Predictions { get; set; }
    }
}