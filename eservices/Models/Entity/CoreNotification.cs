using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pattern_of_life.Models.Entity;
using Pattern_of_life.Service;
using Pattern_of_life.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pattern_of_life.Models.Entity
{
    public class CoreNotification : AuditableEntity
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
    }

}
//add notification
//_context.CoreNotification.Add(new CoreNotification()
//{
//    Message = $"New user registered with id:{user.Id}",
//    CreatedBy = user.Id,
//});