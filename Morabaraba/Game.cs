﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Morabaraba
{
    public class Game
    {
        private List<string> possibleMoves;
        private string currentPlayer;

        private IPlayer player1;
        private IPlayer player2;
        private IBoard Board;

        public Game()
        {
            currentPlayer = "Black";
            player1 = new Player("Black");
            player2 = new Player("White");
            Board = new Board();
            possibleMoves = generatePossibleMoves();

            Board = new Board();
            player1 = new Player("Black");

        }
        public string getCurrentPlayer()
        {
            return currentPlayer;
        }
        public List<string> checkNeighbours(string position)
        {
            switch (position)
            {
                case "A1": return new List<string> { "D1", "B2", "A4" };
                case "A4": return new List<string> { "A1", "A7", "B4" };
                case "A7": return new List<string> { "A4", "B6", "D7" };

                case "B2": return new List<string> { "A1", "C3", "B4", "D2" };
                case "B4": return new List<string> { "B2", "A4", "D6", "C4"};
                case "B6": return new List<string> { "A7", "B4", "D7", "C5" };

                case "C3": return new List<string> { "B2", "C4", "D3" };
                case "C4": return new List<string> { "C3", "B4", "C5" };
                case "C5": return new List<string> { "C4", "B6", "D5" };

                case "D1": return new List<string> { "A1", "D2", "G1" };
                case "D2": return new List<string> { "D1", "B2", "D3", "F2" };
                case "D3": return new List<string> { "C3","D2","E3" };

                case "D5": return new List<string> { "C5", "D6", "E5"};
                case "D6": return new List<string> { "D5", "B6", "D7", "F6" };
                case "D7": return new List<string> { "A7","D6","G7" };

                case "E3": return new List<string> { "D3", "E4", "F2" };
                case "E4": return new List<string> { "F4", "E3", "E5" };
                case "E5": return new List<string> { "D5","E4","F6" };

                case "F2": return new List<string> { "G1", "E3", "F4", "D2" };
                case "F4": return new List<string> { "E4", "F6", "G4", "F2" };
                case "F6": return new List<string> { "G7","D6","F4","E5" };

                case "G1": return new List<string> { "G1", "E3", "F4", "D2" };
                case "G4": return new List<string> { "E4", "F6", "G4", "F2" };
                case "G7": return new List<string> { "G7", "D6", "F4", "E5" };
            }
           
            return null;
        }
        public void makeMove(Board gameBoard,IPlayer player, string fromPos, string toPos) 
        {
            if (player.getState() == "Placing")
            {
                gameBoard.updateMoveToBoard(currentPlayer, toPos);
            }

            if(player.getState() == "Moving" || checkNeighbours(fromPos).Contains(toPos) )
            {
                gameBoard.updateMoveFromBoard(fromPos);
                gameBoard.updateMoveToBoard(currentPlayer, toPos);
            }
        }
 

        List<string> generatePossibleMoves()
        {
            return new List<string> { "A1","A4","A7",
                                      "B2","B4","B6",
                                      "C3","C4","C5",
                                      "D1","D2","D3","D5","D6","D7",
                                      "E3","E4","E5",
                                      "F2","F4","F6",
                                      "G1","G4","G7"};
        }

        void swapCurrentPlayer(Player tempPlayer)
        {
            switch (tempPlayer.currentplayer())
            {
                case "Black": currentPlayer = "White";
                    return;
                case "White": currentPlayer = "Black";
                    return;
            }
        }
        
        private bool cowIn_MillPos(List<List<string>> mill_List, string pos)
        {
            for (int i = 0; i < mill_List.Count; i++)
            {
                if (mill_List[i].Contains(pos)) return true;
            }
            return false;
        }


        private int numberOf_Cow_NotInMill(IPlayer player)
        {
            int count = 0;
            List<string> playedPos = player.getPlayedPos();
            List<List<string>> mill_list = player.getMills();
            bool cowINmill = false;
            for (int i=0;i<playedPos.Count;i++)
            {
               
                foreach(List<string> mill in mill_list)
                {
                    if(mill.Contains(playedPos[i]))
                    {
                        cowINmill = true;
                        break;
                    }
                }
                if (!cowINmill) count++;
                cowINmill = false;
            }
            return count;
        }

        private char getPieceAtPos(string position)
        {
            char[,] gameBoard = Board.getBoard();
            switch(position)
            {
                case "A1":return gameBoard[0, 0];
                case "A4": return gameBoard[0, 1];
                case "A7": return gameBoard[0, 2];
                case "B2": return gameBoard[1, 0];
                case "B4": return gameBoard[1, 1];
                case "B6": return gameBoard[1, 2];
                case "C3": return gameBoard[2, 0];
                case "C4": return gameBoard[2, 1];
                case "C5": return gameBoard[2, 2];
                case "D1": return gameBoard[3, 0];
                case "D2": return gameBoard[3, 1];
                case "D3": return gameBoard[3, 2];
                case "D5": return gameBoard[4, 0];
                case "D6": return gameBoard[4, 1];
                case "D7": return gameBoard[4, 2];
                case "E3": return gameBoard[5, 0];
                case "E4": return gameBoard[5, 1];
                case "E5": return gameBoard[5, 2];
                case "F2": return gameBoard[6, 0];
                case "F4": return gameBoard[6, 1];
                case "F6": return gameBoard[6, 2];
                case "G1": return gameBoard[7, 0];
                case "G4": return gameBoard[7, 1];
                case "G7": return gameBoard[7, 2];
            }
            return ' ';
        }

        private bool invalidKill(string position)
        {
            char piece = getPieceAtPos(position);
            if (piece == ' ')
            {
                // CLI -> print appropriate message
                return true;
            }
            else
            {
                if (currentPlayer == "Black")
                {
                    if (cowIn_MillPos(player2.getMills(), position) && numberOf_Cow_NotInMill(player2) == 0)
                    {
                        //messageDisplay("Cannot kill a cow already\n in a mill, try\n another cow");
                        return true;
                    }
                    return false;

                }
                else if (currentPlayer == "White")
                {
                    if (cowIn_MillPos(player1.getMills(), position) && numberOf_Cow_NotInMill(player1) == 0)
                    {
                        //messageDisplay("Cannot kill a cow already\n in a mill, try\n another cow");
                        return true;
                    }
                    return false;
                }
                else
                {
                    //messageDisplay("Don't kill yourself, Try again");
                    return true;
                }
            }
        }
        private void eliminate(string Position)
        {

        }

        public void runGame()
        {
            
        }
    }
   
}
