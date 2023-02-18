using IDS_School.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS_School.ViewComponents
{
    public class RoomViewComponent: ViewComponent
    {
        private ApplicationDbContext _context;

        public RoomViewComponent (ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var chats = _context.Chats.ToList();
            return View(chats);
        }
    }
}
