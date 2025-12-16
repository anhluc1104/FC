using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FootBallWeb.Models
{
    public class Player : BaseModel
    {
        [Key]
        public int PlayerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Position { get; set; }

        public int Age { get; set; }
        

        public string Nationality { get; set; }

        public string PhotoUrl { get; set; }

        // FK to Team
        [Required(ErrorMessage = "Bạn phải chọn đội bóng")]
        public int? TeamId { get; set; }  // CHÚ Ý: nullable để có thể kiểm soát khi chưa chọn
        [ValidateNever]
        public Team Team { get; set; }
        [ValidateNever]
        public ICollection<PlayerTeamHistory> PlayerTeamHistories { get; set; }
    }
}
