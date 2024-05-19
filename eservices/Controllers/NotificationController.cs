using Microsoft.AspNetCore.Mvc;
using Pattern_of_life.Data;
using Pattern_of_life.Services;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Pattern_of_life.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;


        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }
        [CustomAuthorize("Admin")]
        public JsonResult MarkRead(int notificationId)
        {
            try
            {
                var notification = _context.CoreNotification.FirstOrDefault(e => e.Id == notificationId);
                notification.IsRead = true;
                return Json(new MessageModel() { IsSuccess = _context.SaveChanges() > 0 });
            }
            catch (Exception ex)
            {
                return Json(new MessageModel() { IsSuccess = false, Message = ex.Message });
            }

        }
        [CustomAuthorize("Admin")]
        public JsonResult MarkAllRead()
        {
            try
            {
                var notificationsToUpdate = _context.CoreNotification.Where(e => !e.IsRead).ToList();

                notificationsToUpdate.ForEach(n => n.IsRead = true);

                _context.SaveChanges();

                return Json(new MessageModel() { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Json(new MessageModel() { IsSuccess = false, Message = ex.Message });
            }
        }


    }
}

