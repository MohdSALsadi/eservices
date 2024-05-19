using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Pattern_of_life
{
    public class EmailRequest
    {
        public string? Receiver { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
