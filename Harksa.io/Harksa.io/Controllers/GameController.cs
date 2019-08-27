using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Harksa.io.Models;
using Repository.Models;
using Repository.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Harksa.io.Controllers
{
    [Authorize]
    public class GameController : Controller {

        private readonly IDatabaseService _databaseService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public GameController(IDatabaseService service, IHostingEnvironment hostingEnvironment) {
            _databaseService = service;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Route("Games/Create")]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [Route("Games/Create")]
        public async Task<IActionResult> Create(GameRegisteringModel model) {
            if (!ModelState.IsValid) return View(model);

            int? id =  _databaseService.GetAccountIdFromIdentityId(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (id == null) return View(model);

            string uniqueFileName = null;
            if (model.GamePicture != null) {
                string basePath = "images\\games\\" + id;
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, basePath);

                if (!Directory.Exists(uploadFolder)) {
                    Directory.CreateDirectory(uploadFolder);
                }

                uniqueFileName = Guid.NewGuid() + "_" + model.GamePicture.FileName;
                
                using (var fileStream = new FileStream(Path.Combine(uploadFolder, uniqueFileName), FileMode.Create)) {
                    await model.GamePicture.CopyToAsync(fileStream);
                }
            }

            var categories = model.Categories == null ? new List<string>() : model.Categories.Split(";").ToList();

            Game game = new Game {Title = model.Title, Description = model.Description, AccountId = (int) id, ShortDescription = model.ShortDescription, ImageFileName = uniqueFileName, PublicationDate = DateTime.Today};

            await _databaseService.AddGameCategories(await _databaseService.RegisterGame(game), await _databaseService.GetCorrespondingCategoriesId(categories));

            return RedirectToAction("Index");

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Games")]
        public IActionResult Index() {
            return View(_databaseService.GetGamesCards());
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Games/{id}")]
        public async Task<IActionResult> Details(int id) {
            return View(await _databaseService.GetGame(id));
        }
    }
}