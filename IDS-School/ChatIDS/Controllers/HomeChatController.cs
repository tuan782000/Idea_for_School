using IDS_School.Data;
using IDS_School.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS_School.ChatIDS.Controllers
{
    [Authorize]
    public class HomeChatController : Controller
    {

        private ApplicationDbContext _context;

        public HomeChatController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //List<Chat> getRoom = null;
            var getRoom = _context.Chats.ToList();
            //ViewData["getRoom"] = getRoom;
            return View(getRoom);

        }
        [HttpPost, ActionName("CreateRoom")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoom(string name)
        {
            var createNew = new Chat()           
            {
                Name = name,
                Type = ChatType.Room

            };
            _context.Add(createNew);
            await _context.SaveChangesAsync();

            var roomId = _context.Chats.Where(i => i.Id == createNew.Id).Select(i => i.Id).FirstOrDefault();
            return RedirectToAction("Chat", new { id = roomId });
        }


        //[HttpGet("{id}")]
        public IActionResult Chat(int id)
        {
            List<Chat> getRoom = null;
            getRoom = _context.Chats.ToList();

            var chat = _context.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);

            ViewData["getRoom"] = getRoom;
            return View(chat);
        }

        [HttpPost, ActionName("CreateMessage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMessage(int chatId, string message)
        {
            var Message = new Message
            {
                ChatId = chatId,
                Text = message,
                Name = User.Identity.Name,
                Timestamp = DateTime.Now
            };
            _context.Messages.Add(Message);
            await _context.SaveChangesAsync();
            return RedirectToAction("Chat", new { id = chatId });
        }

        



    }
}