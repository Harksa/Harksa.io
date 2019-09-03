using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Repository.Services;

namespace Harksa.io.Controllers
{
    public class CommentController : Controller {
        private readonly IDatabaseService _databaseService;

        public CommentController(IDatabaseService databaseService) {
            _databaseService = databaseService;
        }

        public async Task<IActionResult> Details(int gameId) {
            return PartialView("Comment/Details", await _databaseService.GetComments(gameId));
        }

        [HttpGet]
        public IActionResult Post(int gameId) {
            return PartialView("Comment/Post", new Comment{GameId = gameId});
        }

        [HttpPost]
        public async Task<IActionResult> Post(Comment comment) {
            int? id =  _databaseService.GetAccountIdFromIdentityId(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (id == null) return RedirectToAction("Details", "Game", new {id = comment.GameId});

            comment.AccountId = (int) id;

            await _databaseService.PostComment(comment);
            return RedirectToAction("Details", "Game", new {id = comment.GameId});
        }

    }
}