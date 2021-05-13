using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingSiteNet.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MatchupClosingTime { get; set; }
        public string Name { get; set; }
        public bool TieAllowed { get; set; }
        public int PointsForPerfect { get; set; }
        public int PointForDifference { get; set; }
        public int PointsForWinnerOnly { get; set; }

        public Country Country { get; set; }
    }
}
