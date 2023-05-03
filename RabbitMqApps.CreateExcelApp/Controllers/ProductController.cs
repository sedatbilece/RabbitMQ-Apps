using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMqApps.CreateExcelApp.Models;
using RabbitMqApps.CreateExcelApp.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMqApps.CreateExcelApp.Controllers
{


    [Authorize]
    public class ProductController : Controller
    {


        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RabbitMQPublisher _rabbitMQPublisher;

        public ProductController(UserManager<IdentityUser> userManager,
            AppDbContext context, RabbitMQPublisher rabbitMQPublisher)
        {
            _userManager = userManager;
            _context = context;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

       



        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateProductExcel()
        {
            var user =await  _userManager.FindByNameAsync(User.Identity.Name);

            var fileName = $"product-excel-{Guid.NewGuid().ToString().Substring(1, 10)}";

            UserFile file = new UserFile
            {
                UserId = user.Id,
                FileName = fileName,
                FileStatus = FileStatus.Creatting
            };

              await _context.UserFiles.AddAsync(file);
              await _context.SaveChangesAsync();


            // rabbitMQ'ya mesaj gönderilecek
            _rabbitMQPublisher.Publish(new Shared.CreateExcelMessage()
            {
                FileId = file.Id,
                UserId = user.Id
            });

            TempData["StartCreatingExcel"] = true;

            return RedirectToAction("Files");
        }


        public async Task<IActionResult> Files()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);


            return View(await _context.UserFiles.Where(x => x.UserId == user.Id).ToListAsync());
        }



    }
}
