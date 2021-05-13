using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingSiteNet.Models
{
    public class PlayerTournament
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public Guid ApsnetUserId { get; set; }
        public Tournament Tournament { get;set;}
    }
}
