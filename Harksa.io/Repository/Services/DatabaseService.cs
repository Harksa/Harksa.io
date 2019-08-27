using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Contexts;
using Repository.Models;
using Repository.Models.Helpers;

namespace Repository.Services
{
    public class DatabaseService : IDatabaseService
    {

        #region ACCOUNT

        public int? GetAccountIdFromIdentityId(string id) {
            DatabaseContext context = new DatabaseContext();
            return context.Accounts.FirstOrDefault(a => a.UserId == id)?.Id;
        }

        public async Task RegisterAccount(Account account) {
            DatabaseContext context = new DatabaseContext();
            await context.Accounts.AddAsync(account);
            await context.SaveChangesAsync();
        }

        #endregion

        public IEnumerable<ShortGameModel> GetGamesCards() {
            DatabaseContext context = new DatabaseContext();
            var games = context.Games.ToList();

            List<ShortGameModel> gameCards = new List<ShortGameModel>();

            foreach (var game in games) {
                ShortGameModel model = new ShortGameModel {
                    Id               = game.Id,
                    AccountId        = game.AccountId,
                    Title            = game.Title,
                    Categories       = GetCategoriesNameFromGameId(game.Id),
                    ShortDescription = game.ShortDescription,
                    ImageFileName    = game.ImageFileName
                };

                gameCards.Add(model);
            }

            return gameCards;
        }

        public async Task<int> RegisterGame(Game game) {
            DatabaseContext context = new DatabaseContext();
            var g = await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            return g.Entity.Id;
        }

        public async Task<FullGameModel> GetGame(int id) {
            DatabaseContext context = new DatabaseContext();
            Game game = await context.Games.FindAsync(id);

            FullGameModel fullGame = new FullGameModel {
                Id               = id,
                AccountId        = game.AccountId,
                Title            = game.Title,
                Categories       = GetCategoriesNameFromGameId(game.Id),
                ShortDescription = game.ShortDescription,
                ImageFileName    = game.ImageFileName,
                FullDescription  = game.Description,
                PublicationTime  = game.PublicationDate,
                Author           = GetAuthorName(game.Id)
            };

            return fullGame;
        }

        public string GetAuthorName(int gameId) {
            DatabaseContext context = new DatabaseContext();
            var result = (from g in context.Games
                          join a in context.Accounts on g.AccountId equals a.Id
                          join ua in context.ApplicationUsers on a.UserId equals ua.Id
                          where g.Id == gameId
                          select ua.UserName).Distinct().SingleOrDefault();

            return result;
        }

        public IEnumerable<string> GetCategoriesNameFromGameId(int id) {
            DatabaseContext context = new DatabaseContext();
            var result = from cg in context.GameCategories
                         join c in context.Categories on cg.CategoryId equals c.Id
                         where cg.GameId == id
                         select c.Name;

            return result.ToList();
        }

        public async Task<IEnumerable<int>> GetCorrespondingCategoriesId(List<string> categories) {
            DatabaseContext context = new DatabaseContext();
            List<int> ints = new List<int>();

            if (categories == null || categories.Count == 0) {
                var cat = context.Categories.FirstOrDefault(c => c.Name == "UNTAGGED");

                if (cat == null) {
                    var newcat = await context.Categories.AddAsync(new Category {Name = "UNTAGGED"});
                    await context.SaveChangesAsync();
                    ints.Add(newcat.Entity.Id);
                } else {
                    ints.Add(cat.Id);
                }

                return ints;
            }

            foreach (var category in categories) {
                if (String.IsNullOrEmpty(category)) continue;

                var toUpper = category.ToUpperInvariant();
                var cat = context.Categories.FirstOrDefault(c => c.Name == toUpper);

                if (cat == null) {
                    var newcat = await context.Categories.AddAsync(new Category {Name = toUpper});
                    await context.SaveChangesAsync();
                    ints.Add(newcat.Entity.Id);
                } else {
                    ints.Add(cat.Id);
                }
            }

            return ints;
        }

        public async Task AddGameCategories(int gameId, IEnumerable<int> categoriesList) {
            DatabaseContext context = new DatabaseContext();

            foreach (var i in categoriesList) {
                context.GameCategories.Add(new GameCategory {GameId = gameId, CategoryId = i});
                await context.SaveChangesAsync();
            }
        }

    }
}
