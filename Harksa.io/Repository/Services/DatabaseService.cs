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
            using (DatabaseContext context = new DatabaseContext()) {
                return context.Accounts.FirstOrDefault(a => a.UserId == id)?.Id;
            }
        }

        public async Task RegisterAccount(Account account) {
            using (DatabaseContext context = new DatabaseContext()) {
                await context.Accounts.AddAsync(account);
                await context.SaveChangesAsync();
            }
        }

        public AccountViewModel GetAccountInformations(int accountId) {
            using (DatabaseContext context = new DatabaseContext()) {
                AccountViewModel account = new AccountViewModel 
                {
                    Id = accountId,
                    AccountName = GetAuthorNameFromAccountId(accountId),
                    GameCards = GetGamesCards(accountId)
                };

                return account;
            }
        }

        #endregion

        public IEnumerable<ShortGameModel> GetGamesCards(int accountId) {
            using (DatabaseContext context = new DatabaseContext()) {
                IEnumerable<Game> games;

                if (accountId == -1) {
                    games = context.Games;
                } else {
                    games = from a in context.Accounts
                            join g in context.Games on a.Id equals g.AccountId
                            where a.Id == accountId
                            select g;
                }

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
        }

        public async Task<int> RegisterGame(Game game) {
            using (DatabaseContext context = new DatabaseContext()) {
                var g = await context.Games.AddAsync(game);
                await context.SaveChangesAsync();

                return g.Entity.Id;
            }
        }

        public async Task<FullGameModel> GetGame(int id) {
            using (DatabaseContext context = new DatabaseContext()) {
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
                    Author           = GetAuthorNameFromGameId(game.Id)
                };

                return fullGame;
            }
        }

        public ShortGamesWithCategoryModel GetSameCategoryGameCards(string category) {
            using (DatabaseContext context = new DatabaseContext()) {

                var result = from g in context.Games
                             join gc in context.GameCategories on g.Id equals gc.GameId
                             join c in context.Categories on gc.CategoryId equals c.Id
                             where c.Name == category
                             select g;

                List<ShortGameModel> games = new List<ShortGameModel>();
                foreach (var game in result) {
                    ShortGameModel model = new ShortGameModel {
                        Id               = game.Id,
                        AccountId        = game.AccountId,
                        Title            = game.Title,
                        ImageFileName    = game.ImageFileName,
                        ShortDescription = game.ShortDescription,
                        Categories       = GetCategoriesNameFromGameId(game.Id)
                    };

                    games.Add(model);
                }
                
                return new ShortGamesWithCategoryModel {
                    Category = category,
                    ShortGameModels = games
                };
            }
        }

        public string GetAuthorNameFromAccountId(int accountId) {
            using (DatabaseContext context = new DatabaseContext()) {
                var result = (from a in context.Accounts
                              join ua in context.ApplicationUsers on a.UserId equals ua.Id
                              where a.Id == accountId
                              select ua.UserName).Distinct().SingleOrDefault();

                return result;
            }
        }

        public string GetAuthorNameFromGameId(int gameId) {
            using (DatabaseContext context = new DatabaseContext()) {
                var result = (from g in context.Games
                              join a in context.Accounts on g.AccountId equals a.Id
                              join ua in context.ApplicationUsers on a.UserId equals ua.Id
                              where g.Id == gameId
                              select ua.UserName).Distinct().SingleOrDefault();

                return result;
            }
        }

        public IEnumerable<string> GetCategoriesNameFromGameId(int id) {
            using (DatabaseContext context = new DatabaseContext()) {
                var result = from cg in context.GameCategories
                             join c in context.Categories on cg.CategoryId equals c.Id
                             where cg.GameId == id
                             select c.Name;

                return result.ToList();
            }
        }

        public async Task<IEnumerable<int>> GetCorrespondingCategoriesId(List<string> categories) {
            using (DatabaseContext context = new DatabaseContext()) {
                List<int> ints = new List<int>();

                if (categories == null || categories.Count == 0) {
                    var cat = context.Categories.FirstOrDefault(c => c.Name == "untagged");

                    if (cat == null) {
                        var newcat = await context.Categories.AddAsync(new Category {Name = "untagged"});
                        await context.SaveChangesAsync();
                        ints.Add(newcat.Entity.Id);
                    } else {
                        ints.Add(cat.Id);
                    }

                    return ints;
                }

                foreach (var category in categories) {
                    if (String.IsNullOrEmpty(category)) continue;

                    var finalCategoryName = category.Trim();
                    finalCategoryName = finalCategoryName.ToLowerInvariant();
                    finalCategoryName = finalCategoryName.Replace(" ", "-");

                    var cat = context.Categories.FirstOrDefault(c => c.Name == finalCategoryName);

                    if (cat == null) {
                        var newcat = await context.Categories.AddAsync(new Category {Name = finalCategoryName});
                        await context.SaveChangesAsync();
                        ints.Add(newcat.Entity.Id);
                    } else {
                        ints.Add(cat.Id);
                    }
                }

                return ints;
            }
        }

        public async Task AddGameCategories(int gameId, IEnumerable<int> categoriesList) {
            using (DatabaseContext context = new DatabaseContext()) {

                foreach (var i in categoriesList) {
                    context.GameCategories.Add(new GameCategory {GameId = gameId, CategoryId = i});
                    await context.SaveChangesAsync();
                }
            }
        }

    }
}
