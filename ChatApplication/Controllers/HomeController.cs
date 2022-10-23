using ChatApplication.Models;
using ChatApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ChatApplication.Controllers
{
    [Route("{controller}/{action}")]
    public class HomeController : Controller
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<User> userManager;
        private readonly IWebHostEnvironment environment;

        public HomeController(AppDbContext appDbContext , UserManager<User> userManager , IWebHostEnvironment environment)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
            this.environment = environment;
        }

       
        [HttpGet]
        [Authorize]
        
        public IActionResult index()
        {
            string sender = HttpContext.User.Identity.Name;
            var users = (this.appDbContext.Users.Where(e => e.UserName != HttpContext.User.Identity.Name)).ToList();
            List<Messages> Messages = new List<Messages>();
            foreach (var user in users)
            {

                var MyRecentRecord = this.appDbContext.Messages.Include(e => e.UserSender).Include(e => e.Reciever).Where(e=>(e.UserSender.UserName == user.UserName && e.Reciever.UserName == sender) || (e.UserSender.UserName == sender && e.Reciever.UserName == user.UserName)).OrderByDescending(b=>b.Id).FirstOrDefault();
                if(MyRecentRecord != null)
                {
                    if(MyRecentRecord.message.Length > 40)
                    {
                        MyRecentRecord.message = MyRecentRecord.message.Substring(0, 40);
                    }
                    
                    
                    Messages.Add(MyRecentRecord);

                }
                else
                {
                    Messages.Add(new Messages()
                    {
                        message = ""
                    });
                }
            }
            IndexModel indexModel = new IndexModel
            {
                users = users,
                messages = Messages
            };

            User user1 = this.appDbContext.Users.FirstOrDefault(e => e.UserName == sender);

            ViewBag.Sender = sender;
            ViewBag.Photo = user1.UserPhoto;
            
            return View(indexModel);
        }
        [HttpPost]
        [Authorize]
        public IActionResult GetMessages(string receiver, string sender)
        {

            var messages = this.appDbContext.Messages.Include(e=>e.UserSender).Include(e=>e.Reciever).Where<Messages>(e =>(e.UserSender.UserName == receiver && e.Reciever.UserName == sender)|| (e.UserSender.UserName == sender && e.Reciever.UserName == receiver));
            return Json(messages);
        }
        [HttpPost]
        [Authorize]
        public IActionResult UpdateUser(UpdateUser updateUser)
        {
            string UniqueFileName = "h";
            string UploadsFolder = Path.Combine(environment.WebRootPath, "img");
            UniqueFileName = Guid.NewGuid().ToString() + "_" + updateUser.UserPhoto.FileName;
            string filepath = Path.Combine(UploadsFolder, UniqueFileName);
            updateUser.UserPhoto.CopyTo(new FileStream(filepath, FileMode.Create));

            User user = this.appDbContext.Users.FirstOrDefault(e => e.UserName == HttpContext.User.Identity.Name);
            user.UserPhoto = UniqueFileName;
            this.appDbContext.Update(user);
            this.appDbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
