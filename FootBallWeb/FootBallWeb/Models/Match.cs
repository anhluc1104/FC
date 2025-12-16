using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootBallWeb.Models
{
    public class Match : BaseModel
    {
        [Key]
        public int MatchId { get; set; }

        public DateTime MatchDate { get; set; }

        // FK - Home team
        public int HomeTeamId { get; set; }
        [ForeignKey("HomeTeamId")]
        public Team HomeTeam { get; set; }

        // FK - Away team
        public int AwayTeamId { get; set; }
        [ForeignKey("AwayTeamId")]
        public Team AwayTeam { get; set; }

        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }

        public string Status { get; set; } // e.g., "Scheduled", "In Progress", "Completed"
        public string Venue { get; set; } // e.g., "Stadium Name, City"
        public string Referee { get; set; } // e.g., "Referee Name"
        public string Competition { get; set; } // e.g., "Premier League", "Champions League"
        public string MatchReport { get; set; } // Summary of the match, can be null if not available

    }
}
