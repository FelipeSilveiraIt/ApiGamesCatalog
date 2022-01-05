using ApiGamesCatalog.Services;
using ApiGamesCatalog.Entities;
using ApiGamesCatalog.Controllers;
using ApiGamesCatalog.InputModel;
using ApiGamesCatalog.ViewModel;
using ApiGamesCatalog.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGamesCatalog.Exceptions;

namespace ApiGamesCatalog.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _iGameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _iGameRepository = gameRepository;
        }

        public async Task<List<GameViewModel>> Get(int page, int amount)
        {
            var games = await _iGameRepository.Get(page, amount);

            //for each game creates its viewmodel and adds to a list
            return games.Select(game => new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Publisher = game.Publisher,
                Price = game.Price
            })
                               .ToList();
        }

        public async Task<GameViewModel> Get(Guid id)
        {
            var game = await _iGameRepository.Get(id);

            if (game == null)
                return null;

            return new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Publisher = game.Publisher,
                Price = game.Price
            };
        }

        public async Task<GameViewModel> Post(GameInputModel game)
        {
            var entityGame = await _iGameRepository.Get(game.Name, game.Publisher);

            if (entityGame.Count > 0)
                throw new GameAlreadyExistsException();

            var gameInsert = new Game
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                Publisher = game.Publisher,
                Price = game.Price
            };

            await _iGameRepository.Post(gameInsert);

            return new GameViewModel
            {
                Id = gameInsert.Id,
                Name = game.Name,
                Publisher = game.Publisher,
                Price = game.Price
            };
        }

        public async Task Put(Guid id, GameInputModel game)
        {
            var entityGame = await _iGameRepository.Get(id);

            if (entityGame == null)
                throw new GameDoesNotExistException();

            entityGame.Name = game.Name;
            entityGame.Publisher = game.Publisher;
            entityGame.Price = game.Price;

            await _iGameRepository.Put(entityGame);
        }

        public async Task Put(Guid id, double Price)
        {
            var entityGame = await _iGameRepository.Get(id);

            if (entityGame == null)
                throw new GameDoesNotExistException();

            entityGame.Price = Price;

            await _iGameRepository.Put(entityGame);
        }

        public async Task Delete(Guid id)
        {
            var game = await _iGameRepository.Get(id);

            if (game == null)
                throw new GameDoesNotExistException();

            await _iGameRepository.Delete(id);
        }

        public void Dispose()
        {
            _iGameRepository?.Dispose();
        }
    }
}
