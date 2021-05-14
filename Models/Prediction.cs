using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BettingSiteNet.Models
{
    public class Prediction
    {
        public int Id { get; set; }
        public int MatchupId { get; set; }
        public Guid AspNetUserId { get; set; }

        [DisplayFormat(NullDisplayText = "_")]
        public int? HomeTeamScore { get; set; }

        [DisplayFormat(NullDisplayText = "_")]
        public int? EnemyTeamScore { get; set; }

        public int? PointsEarned { get; set; }
        public Matchup Matchup { get; set; }

    }
}
