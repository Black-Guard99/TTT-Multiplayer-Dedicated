using System;
using System.Collections.Generic;
using System.Linq;

namespace TTT.Server.Games
{
    public class GamesManager {
        private List<Game> gameList = new List<Game>();
        public GamesManager() {
            gameList = new List<Game>();
        }
        public Guid RegisterGame(string xUser, string oUser) {
            var newGame = new Game(xUser,oUser);
            gameList.Add(newGame);
            return newGame.Id;
        }
        public Game FindGame(string userName)
        {
            return gameList.FirstOrDefault(g => g.xUser == userName || g.oUser == userName);
        }
        public Game ClosedGame(string userName)
        {
            var game = FindGame(userName);
            gameList.Remove(game);
            return game;
        }
        public bool GameExist(string userName)
        {
            return gameList.Any(g => g.xUser == userName || g.oUser == userName);
        }
        public int GetGameCount()
        {
            return gameList.Count();
        }
    }
}
