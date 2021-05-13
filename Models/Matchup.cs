using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingSiteNet.Models
{
    public class Matchup
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public DateTime? GameTime { get; set; }
        public int TournamentId { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? EnemyTeamScore { get; set; }
        public Country Country { get; set; }
        public Tournament Tournament { get; set; }
    }
}
