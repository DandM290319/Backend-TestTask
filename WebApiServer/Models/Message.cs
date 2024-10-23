using System.ComponentModel.DataAnnotations;

namespace WebApiServer.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int Number { get; set; }

        [StringLength(128, ErrorMessage = "Message length has to be less then 129 symbols")]
        public string Content {  get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}