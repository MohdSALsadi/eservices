using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pattern_of_life.Data;
using Pattern_of_life.Models.Entity;
using Pattern_of_life.Services;
using Pattern_of_life;
using Pattern_of_life.Authentication;

namespace Pattern_of_life.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        //[CustomAuthorize("Admin")]
        public IActionResult Index()
        {
            var Locations = _context.Location.ToList();
            ViewBag.Locations = Locations;

            return View();
        }
        public IActionResult Exit()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CheckEmailExist(string email)
        {
            bool emailExists = _context.Users.Any(e => e.Email.ToLower() == email.ToLower());
            return Json(emailExists);
        }

        [HttpPost]
        public JsonResult CheckUsernameExist(string username)
        {
            bool usernameExists = _context.Users.Any(e => e.UserName.ToLower() == username.ToLower());
            return Json(usernameExists);
        }


        [HttpPost]
        //[CustomAuthorize("Admin")]
        public ActionResult GetUsers(DataTablesParameters parameters)
        {


            var data = from user in _context.Users
                       where (parameters.DepartmentId == null || user.LocationId == parameters.DepartmentId)
                             && (parameters.Status == null || (user.IsActive == (parameters.Status == "Active") ? true : false))
                             && (parameters.Email == null || user.Email == parameters.Email)
                             && (parameters.MobileNumber == null || user.PhoneNumber == parameters.MobileNumber)
                       select user;

            var count = data.Count();

            var pagedData = data
                .Skip(parameters.Start).Take(parameters.Length).ToList()
                  .Select(e => new
                  {
                      e.Id,
                      e.UserName,
                      e.Email,
                      e.IsActive,
                      e.PhoneNumber,
                      Department = e.LocationId ?? 0,
                      e.NormalizedUserName,
                      e.CreatedOn
                  }).OrderByDescending(e => e.CreatedOn).ToList();
            var response = new
            {
                draw = parameters.Draw,
                recordsTotal = count,
                recordsFiltered = count,
                data = pagedData
            };
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return Json(response);
        }
       // [CustomAuthorize("Admin")]
        public JsonResult DeleteUser(string userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(e => e.Id == userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    return Json(new MessageModel() { IsSuccess = _context.SaveChanges() > 0 });

                }
                else
                    return Json(new MessageModel() { IsSuccess = false, Message = "No Data Found" });
            }
            catch (Exception ex)
            {
                return Json(new MessageModel() { IsSuccess = false, Message = ExceptionHandler.GetErrorMessage(ex) });
            }

        }
       // [CustomAuthorize("Admin")]
        public JsonResult GetUserData(string userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(e => e.Id == userId);

                if (user != null)
                {
                    return Json(new MessageModel()
                    {
                        IsSuccess = true,
                        Data = new
                        {
                            Username = user.UserName,
                            Email = user.Email,
                            MobileNumber = user.PhoneNumber,
                            FullName = user.UserName,
                            DepartmentId = user.LocationId,
                        }
                    });
                }
                else
                    return Json(new MessageModel() { IsSuccess = false, Message = "No Data Found" });
            }
            catch (Exception ex)
            {

                return Json(new MessageModel() { IsSuccess = false, Message = ExceptionHandler.GetErrorMessage(ex) });
            }
        }
        //[CustomAuthorize("Admin")]
        public JsonResult ChangePassword(string userId, string password)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(e => e.Id == userId);
                if (user != null)
                {
                    user.PasswordHash = EncryptionHelper.Encrypt(password);
                    return Json(new MessageModel() { IsSuccess = _context.SaveChanges() > 0 });
                }
                else
                    return Json(new MessageModel() { IsSuccess = false, Message = "No Data Found" });
            }
            catch (Exception ex)
            {
                return Json(new MessageModel() { IsSuccess = false, Message = ExceptionHandler.GetErrorMessage(ex) });
            }

        }

        // [CustomAuthorize("Admin")]
        public JsonResult Activate(string userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(e => e.Id == userId);
                if (user != null)
                {
                    user.IsActive = !user.IsActive;
                    return Json(new MessageModel() { IsSuccess = _context.SaveChanges() > 0 });
                }
                else
                    return Json(new MessageModel() { IsSuccess = false, Message = "No Data Found" });
            }
            catch (Exception ex)
            {
                return Json(new MessageModel() { IsSuccess = false, Message = ExceptionHandler.GetErrorMessage(ex) });
            }

        }
       // [CustomAuthorize("Admin")]
        public JsonResult GetUserRoles(string userId)
        {
            var query = (from role in _context.Roles
                         join userRole in _context.UserRoles
                         on role.Id equals userRole.RoleId into userRolesGroup
                         from userRole in userRolesGroup.Where(ur => ur.UserId == userId).DefaultIfEmpty()
                         select new
                         {
                             RoleName = role.Name,
                             RoleId = role.Id,
                             HasRole = userRole != null
                         }).ToList();
            return Json(query);

        }
        [HttpPost]
      //  [CustomAuthorize("Admin")]
        public JsonResult AssignRoles([FromBody] AssignRolesModel model)
        {
            try
            {
                var userRoles = _context.UserRoles.Where(e => e.UserId == model.UserId).ToList();
                _context.UserRoles.RemoveRange(userRoles);
                model.RoleIds?.ForEach(role =>
                 _context.UserRoles.Add(new UserRoles() { UserId = model.UserId, RoleId = role })

                );
                return Json(new MessageModel() { IsSuccess = _context.SaveChanges() > 0 });
            }
            catch (Exception ex)
            {
                return Json(new MessageModel() { IsSuccess = false, Message = ExceptionHandler.GetErrorMessage(ex) });
            }

        }
        [HttpPost]
      //  [CustomAuthorize("Admin")]
        public JsonResult UpdateUserData([FromBody] UpdateUserDataViewModel updateUserDataViewModel)
        {
            try
            {
                var user = _context.Users.Find(updateUserDataViewModel.UserId);
                if (user != null)
                {
                    user.Email = updateUserDataViewModel.Email;
                    user.UserName = updateUserDataViewModel.UserName;
                    user.PhoneNumber = updateUserDataViewModel.MobileNumber;
                    user.UserName = updateUserDataViewModel.FUllName;
                    return Json(new MessageModel() { IsSuccess = _context.SaveChanges() > 0 });
                }
                else
                    return Json(new MessageModel() { IsSuccess = false, Message = "No Data Found" });
            }
            catch (Exception ex)
            {

                return Json(new MessageModel() { IsSuccess = false, Message = ExceptionHandler.GetErrorMessage(ex) });
            }

        }


        public JsonResult GetUserDepartments(string userId)
        {
            var departmentId = _context.Users.FirstOrDefault(e => e.Id == userId)?.LocationId;
            return Json(departmentId);
        }

        public JsonResult SetUserDepartment(string userId, int departmentId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(e => e.Id == userId);
                user!.LocationId = departmentId == -1 ? null : departmentId;
                return Json(new MessageModel() { IsSuccess = _context.SaveChanges() > 0 });
            }
            catch (Exception ex)
            {

                return Json(new MessageModel() { IsSuccess = false, Message = ExceptionHandler.GetErrorMessage(ex) });
            }

        }

        public ActionResult Roles()
        {
            return View();
        }

        [HttpPost]
       // [CustomAuthorize("Admin")]
        public ActionResult GetRoles(DataTablesParameters parameters)
        {
            var count = _context.Roles.Count();


            var pagedData = _context.Roles.Skip(parameters.Start).Take(parameters.Length).ToList()
                .Select(e => new
                {
                    e.Id,
                    e.Name

                }).ToList();
            var response = new
            {
                draw = parameters.Draw,
                recordsTotal = count,
                recordsFiltered = count,
                data = pagedData
            };
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return Json(response);
        }


    }

}
