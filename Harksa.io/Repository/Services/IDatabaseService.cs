using System.Collections.Generic;
using System.Threading.Tasks;
using Repository.Models;
using Repository.Models.Helpers;

namespace Repository.Services
{
    public interface IDatabaseService {

        int? GetAccountIdFromIdentityId(string id);

        Task RegisterAccount(Account account);
        AccountViewModel GetAccountInformations(int accountId);

        IEnumerable<ShortGameModel> GetGamesCards(int accountId = -1);
        Task<int> RegisterGame(Game game);
        Task<FullGameModel> GetGame(int id);
        ShortGamesWithCategoryModel GetSameCategoryGameCards(string category);

        Task<IEnumerable<int>> GetCorrespondingCategoriesId(List<string> categories);
        Task AddGameCategories(int gameId, IEnumerable<int> categoriesList);
    }
}
