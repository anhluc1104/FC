namespace FootBallWeb.Models
{
    public class PlayerTeamHistory : BaseModel
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } // có thể null nếu đang thi đấu

        public string Position { get; set; } // nếu muốn lưu vị trí lúc ở đội đó
        public int? ShirtNumber { get; set; } // tùy chọn
        public int? Appearances { get; set; } // số lần ra sân, tùy chọn
        public int? Goals { get; set; } // số bàn thắng, tùy chọn
        public int? Assists { get; set; } // số kiến tạo, tùy chọn
    }
}
