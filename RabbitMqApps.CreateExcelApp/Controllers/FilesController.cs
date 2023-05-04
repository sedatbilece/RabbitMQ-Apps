using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMqApps.CreateExcelApp.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RabbitMqApps.CreateExcelApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        private readonly AppDbContext _context;

        public FilesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file ,int FileId )
        {

            if (file.Length<=0) return BadRequest();

            var userFile = await  _context.UserFiles.FirstOrDefaultAsync(f => f.Id == FileId);

            var filePath = userFile.FilePath + Path.GetExtension(file.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", filePath);

            using FileStream stream = new FileStream(path, FileMode.Create);

            await file.CopyToAsync(stream);

            userFile.CreatedDate = DateTime.Now;

            userFile.FilePath = filePath;

            userFile.FileStatus = FileStatus.Completed;

            await _context.SaveChangesAsync();

            //signalR notifications

            return Ok();

        }

        [HttpGet] 
        public String GetTest()
        {
            return "testurl";
        }
    }
}
