using System.Collections.Generic;
using System.Threading.Tasks;
using Repository.Models;
using Repository.Models.Helpers;

namespace Repository.Services
{
    public interface IDatabaseService {

        int? GetAccountIdFromIdentityId(string id);

        Task RegisterAccount(Account account);

        IEnumerable<ShortGameModel> GetGamesCards();
        Task<int> RegisterGame(Game game);
        Task<FullGameModel> GetGame(int id);

        Task<IEnumerable<int>> GetCorrespondingCategoriesId(List<string> categories);
        Task AddGameCategories(int gameId, IEnumerable<int> categoriesList);
    }
}
