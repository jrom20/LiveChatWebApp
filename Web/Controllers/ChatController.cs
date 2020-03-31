using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;

namespace Web.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatViewModelService _chatViewModelService;

        public ChatController(IChatViewModelService chatViewModelService)
        {
            this._chatViewModelService = chatViewModelService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> RoomsAsync()
        {
            var rooms = await _chatViewModelService.GetChatsWithTotalPeopleAsync();
            return View(rooms);
        }

        [HttpGet("Chat/{chatId}")]
        public IActionResult Room(int chatId)
        {
            ViewBag.ChatId = chatId;
            return View();
        }
    }
}