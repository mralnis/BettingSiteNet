using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingSiteNet.Models
{
    public class Prediction
    {
        public int Id { get; set; }
        public int MatchupId { get; set; }
        public Guid AspNetUserId { get; set; }
        public int HomeTeamScore { get; set; }
        public int EnemyTeamScore { get; set; }
        public Matchup Matchup { get; set; }

    }
}
