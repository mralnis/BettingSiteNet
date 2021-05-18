using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BettingSiteNet.Models
{
    public class Matchup
    {
        public int Id { get; set; }
        public int CountryId { get; set; }

        [DataType(DataType.DateTime)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString ="{0:dd.MM.yyyy HH:mm}")]
        public DateTime? GameTime { get; set; }
        public int TournamentId { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? EnemyTeamScore { get; set; }
        public Country Country { get; set; }
        public Tournament Tournament { get; set; }

        [NotMapped]
        public bool CanVote { get; set; }
    }
}
