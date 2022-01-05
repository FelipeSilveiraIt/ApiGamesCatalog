using ApiGamesCatalog.InputModel;
using ApiGamesCatalog.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGamesCatalog.Services
{
    public interface IGameService : IDisposable
    {
        Task<List<GameViewModel>> Get(int page, int amount);
        Task<GameViewModel> Get(Guid id);
        Task<GameViewModel> Post(GameInputModel game);
        Task Put(Guid id, GameInputModel game);
        Task Put(Guid id, double price);
        Task Delete(Guid id);



    }
}
