using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FootBallWeb.Models
{
    public class Team : BaseModel
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string city { get; set; }
        public string stadium { get; set; }

        public string LogoUrl { get; set; }

        [ValidateNever]
        public ICollection<Player> Players { get; set; }
        [ValidateNever]
        public ICollection<PlayerTeamHistory> PlayerTeamHistories { get; set; }
        [ValidateNever]
        public ICollection<Match> HomeMatches { get; set; }
        [ValidateNever]
        public ICollection<Match> AwayMatches { get; set; }
    }
}
