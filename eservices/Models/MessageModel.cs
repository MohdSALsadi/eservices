namespace eservices.Models
{
    public class MessageModel
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public dynamic Data { get; set; }
    }
}
