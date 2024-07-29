using System.ComponentModel.DataAnnotations;

namespace MyAPINetCore6.Data
{
    public class Notification
    {
        [Key]

        public int Id { get; set; }
        public string? UserSendId { get; set; }
        public string? UserSendName { get; set; }

        public string? UserReceiveId { get; set; }

        public string? Description { get; set; }
        public DateTime? NotificationTime { get; set; }
        public int? NotificationType { get; set; }  // 1-All 2-Personal
        public bool? IsRead { get; set; }
        public string? Link { get; set; }

    }
}
