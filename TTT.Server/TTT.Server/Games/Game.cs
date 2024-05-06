using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Network_Shared.Models;
using TTT.Server.Utilities;

namespace TTT.Server.Games
{
    public class Game {
        private const int GRID_SIZE = 3;
        public Game(string xUser,string oUser) {
            Id = Guid.NewGuid();
            startTime = DateTime.UtcNow;
            CurrentRoundStartTime = startTime;


            this.xUser = xUser;
            this.oUser = oUser;
            round = 1;
            
            grid = new MarkType[GRID_SIZE,GRID_SIZE];
            currentUser = this.xUser;
        }
        public Guid Id { get; set; }
        public ushort round {  get; set; }
        public string xUser { get; set; }
        public ushort xWins {  get; set; }
        public string oUser {  get; set; }
        public ushort oWins { get; set; }
        public DateTime startTime { get; set; }
        public DateTime CurrentRoundStartTime { get; set; }


        public bool oWantsRematch {  get; set; }
        public bool xWantsRematch { get; set; }
        public string currentUser {  get; set; }


        public MarkType[,] grid { get; }

        public MarkedResult MarkeCell(byte index) {
            var (row, col) = BasicsExtensions.GetRowCol(index);
            grid[row, col] = GetPlayerType(currentUser);


            var (isWin,lineType) = CheckWin(row, col);
            var draw = CheckDraw();
            var resutl = new MarkedResult();
            if (isWin)
            {
                resutl.MarkedOutCome = MarkedOutCome.Win;
                resutl.LineType = lineType;
            }
            else if(draw)
            {
                resutl.MarkedOutCome = MarkedOutCome.Draw;
                resutl.LineType = lineType;
            }
            return resutl;
        }

        private bool CheckDraw() {
            for (int row = 0; row < GRID_SIZE; row++) {
                for (int col = 0; col < GRID_SIZE; col++) {
                    if (grid[row,col] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private (bool isWin, WinLineType lineType) CheckWin(byte row, byte col) {
            var type = grid[row, col];
            // Check col.
            for (int i = 0; i < GRID_SIZE; i++) {
                if (grid[row,i] != type)
                {
                    break;
                }
                if(i == GRID_SIZE - 1) return (true, ResolveRowWinLineType(row));
            }

            // Check Row
            for (int i = 0; i < GRID_SIZE; i++) {
                if (grid[i,col] != type)
                {
                    break;
                }
                if (i == GRID_SIZE - 1) return (true, ResolveColWinLineType(col));
            }

            // check Diagonal

            if(row == col)
            {
                for(int i = 0; i <GRID_SIZE; i++)
                {
                    if (grid[i, i] != type) break;
                    if (i == GRID_SIZE - 1) return (true, WinLineType.Diagnoal);
                }
            }
            // check anti-diagonal
            if (row + col == GRID_SIZE - 1)
            {
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    if (grid[i, (GRID_SIZE - 1) - i] != type){
                        break;
                    }
                    if (i == GRID_SIZE - 1) return (true, WinLineType.AntiDiaognal);
                }
            }
            return (false, WinLineType.None);
        }

        private WinLineType ResolveColWinLineType(byte col)
        {
            return (WinLineType)(col + 3);
        }

        private WinLineType ResolveRowWinLineType(byte row)
        {
            return (WinLineType)(row + 6);
        }

        private MarkType GetPlayerType(string currentUser)
        {
            if(currentUser == xUser)
            {
                return MarkType.X;
            }
            else
            {
                return MarkType.O;
            }
        }

        public string GetOppoenent(string otherId)
        {
            if(otherId == xUser)
            {
                return oUser;
            }
            else
            {
                return xUser;
            }
        }

        public void SwitchCurrentPlayer() {
            currentUser = GetOppoenent(currentUser);
        }

        public void AddWin(string winnerId)
        {
            var winnerType = GetPlayerType(winnerId);
            if(winnerType == MarkType.X)
            {
                xWins++;
            }
            else
            {
                oWins++;
            }
        }

        public void SetRematchReadiness(string userId) {
            var playerType = GetPlayerType(userId);
            if(playerType == MarkType.X)
            {
                xWantsRematch = true;
            }
            else
            {
                oWantsRematch = true;
            }
        }

        public bool BothPlayerReady()
        {
            return xWantsRematch && oWantsRematch;
        }

        public void NewRound()
        {
            CurrentRoundStartTime = DateTime.UtcNow;
            ResetGrid();
            currentUser = xUser;
        }

        private void ResetGrid()
        {
            for (int row = 0; row < GRID_SIZE; row++) {
                for(int col = 0; col < GRID_SIZE; col++)
                {
                    grid[row, col] = 0;
                }
            }
        }
    }
    public struct MarkedResult
    {
        public MarkedOutCome MarkedOutCome { get; set;}
        public WinLineType LineType { get; set;}
    }
}
