﻿using NUnit.Framework;
using System;
using NSubstitute;
using System.Linq;
using System.Collections.Generic;

namespace Morabaraba.Test
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void CheckEmptyBoard()
        {
            IBoard board = new Board();
            char[,] gameBoard = board.getBoard();
            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 3; k++) if (gameBoard[i, k] == ' ') count++;
            }

            Assert.That(count == 24);
        }

        static object[] possibleMills =
        {
            new object[] { new string[] {"A1","A4","A7"}, true },
            new object[] { new string[] {"A1","B2","C3"}, true },
            new object[] { new string[] {"A1","D1","G1"}, true },
            new object[] { new string[] {"A4","B4","C4"}, true },
            new object[] { new string[] {"A7","B6","C5"}, true },
            new object[] { new string[] {"A7","D7","G7"}, true },
            new object[] { new string[] {"B2","B4","B6"}, true },
            new object[] { new string[] {"B2","D2","F2"}, true },
            new object[] { new string[] {"B6","D6","F6"}, true },
            new object[] { new string[] {"C3","C4","C5"}, true },
            new object[] { new string[] {"C3","D3","E3"}, true },
            new object[] { new string[] {"D1","D2","D3"}, true },
            new object[] { new string[] {"D5","D6","D7"}, true },
            new object[] { new string[] {"E3","E4","E5"}, true },
            new object[] { new string[] {"E4","F4","G4"}, true },
            new object[] { new string[] {"E5","F6","G7"}, true },
            new object[] { new string[] {"F2","F4","F6"}, true },
            new object[] { new string[] {"G1","G4","G7"}, true },
        };

        [Test]
        [TestCaseSource(nameof(possibleMills))]

        public void CheckMill_Is_Formed(string[] c, bool expected)
        {

            IPlayer player1 = new Player("Black");
            IReferee referee = new Referee();

            player1.addPlayedPositions(c[0]);
            player1.addPlayedPositions(c[1]);
            player1.addPlayedPositions(c[2]);

            bool result = referee.isMill(player1);
            Assert.AreEqual(expected, result);
        }

        static object[] possibleMills2 =
        {
            new object[] { new string[] {"A1","A4","A7","B2","B4"},false, true },
            new object[] { new string[] {"A1","B2","C3", "G1", "G7"},false, true },
            new object[] { new string[] {"A1","D1","G1", "B2", "B4"},false, true },
            new object[] { new string[] {"A4","B4","C4", "A1","A7"},false, true },
            new object[] { new string[] {"A7","B6","C5", "A1","A4"},false, true },
            new object[] { new string[] {"A7","D7","G7", "A1","A4"},false, true },
            new object[] { new string[] {"B2","B4","B6", "G7", "G1"},false, true },
            new object[] { new string[] {"B2","D2","F2", "A1", "A4"},false, true },
            new object[] { new string[] {"B6","D6","F6", "F2", "A1"},false, true },
            new object[] { new string[] {"C3","C4","C5", "A1","A4"},false, true },
            new object[] { new string[] {"C3","D3","E3", "B2", "B4"},false, true },
            new object[] { new string[] {"D1","D2","D3", "F2", "F6"},false, true },
            new object[] { new string[] {"D5","D6","D7", "B2", "B4"},false, true },
            new object[] { new string[] {"E3","E4","E5", "B2", "A4"},false, true },
            new object[] { new string[] {"E4","F4","G4", "D1", "G7"},false, true },
            new object[] { new string[] {"E5","F6","G7", "F2","D2"},false, true },
            new object[] { new string[] {"F2","F4","F6", "G1", "G7"},false, true },
            new object[] { new string[] {"G1","G4","G7", "A1", "A4"},false, true },
        };

        [Test]
        [TestCaseSource(nameof(possibleMills2))]
        public void Shooting_is_only_possible_on_the_turn_that_a_mill_is_Completed(string[] c, bool expected1, bool expected2)
        {
            IPlayer player1 = new Player("Black");
            IPlayer player2 = new Player("White");
            IReferee referee = new Referee();
            IBoard board = new Board();
            Game g = new Game(player1, player2, board, referee);

            player1.makePlacement(c[0],board);
            player1.makePlacement(c[1], board);
            player1.makePlacement(c[2], board);

            player2.makePlacement(c[3], board);
            player2.makePlacement(c[4], board);
            
            bool mill = referee.isMill(player1);
            g.eliminate(player2,board, c[3]);

            bool result = player2.playerOwnPosition(c[3]);
            Assert.AreEqual(expected1,result);

            mill = referee.isMill(player1);
            g.eliminate(player2, null, c[4]);

            result = player2.playerOwnPosition(c[4]);
            Assert.AreEqual(expected2, result);

        }

        static object[] no_Mills =
        {
            new object[] { new string[] { "A1"},new string[] {"B2","A7"}, false},
            new object[] { new string[] { "A1" },new string[] {"A4","C3"}, false },
            new object[] { new string[] {"A1","D1"}, new string[] {"G1" }, false },
            new object[] { new string[] { "A4", "B4" },new string[]{"C4"}, false },
            new object[] { new string[] { "A7" }, new string[]{"B6","C5"}, false },
            new object[] { new string[] { "A7" },new string[]{"D7","G7"}, false },
            new object[] { new string[] { "B2", "B4" },new string[]{"B6"}, false },
            new object[] { new string[] { "B2", "D2" },new string[]{"F2"}, false },
            new object[] { new string[] { "B6" },new string[]{"D6","F6"}, false },
            new object[] { new string[] { "C3" },new string[]{"C4","C5"}, false },
            new object[] { new string[] { "C3", "D3" },new string[]{"E3"}, false },
            new object[] { new string[] { "D1", "D2" },new string[]{"D3"}, false },
            new object[] { new string[] { "D5" },new string[]{"D6","D7"}, false },
            new object[] { new string[] { "E3" },new string[]{"E4","E5"}, false },
            new object[] { new string[] { "E4", "F4" },new string[]{"G4"}, false },
            new object[] { new string[] { "E5" },new string[]{"F6","G7"}, false },
            new object[] { new string[] { "F2", "F4" },new string[]{"F6"}, false },
            new object[] { new string[] { "A1", "A4" },new string[]{"A7"}, false },
            new object[] { new string[] { "G1" },new string[]{"G4","G7"}, false },
        };

        [Test]
        [TestCaseSource(nameof(no_Mills))]
        public void CheckMill_Is_Not_Formed_On_Different_Cows_On_SameLine(string[] w, string[] b, bool expected)
        {
            IPlayer player1 = new Player("Black");
            IPlayer player2 = new Player("White");
            IReferee referee = new Referee();
            
            for(int i=0;i<b.Length;i++) player1.addPlayedPositions(b[i]);
            for (int i = 0; i < w.Length; i++) player2.addPlayedPositions(w[i]);

            bool result = referee.isMill(player1);
            Assert.AreEqual(expected, result);

            bool result1 = referee.isMill(player2);
            Assert.AreEqual(expected, result1);
            //Assert.That(referee.isMill(player2) == false);

        }

        static object[] noMills2 =
       {
            new object[] { new string[] {"A1","A4","D1"}, false },
            new object[] { new string[] {"A1","B2","B4"}, false },
            new object[] { new string[] {"A1","D1","D2"}, false },
            new object[] { new string[] {"A4","B4","B6"}, false },
            new object[] { new string[] {"A7","B6","D6"}, false },
            new object[] { new string[] {"A7","D7","D6"}, false },
            new object[] { new string[] {"B2","B4","C4"}, false },
            new object[] { new string[] {"B2","D2","D3"}, false },
            new object[] { new string[] {"B6","D6","D5"}, false },
            new object[] { new string[] {"C3","C4","B4"}, false },
            new object[] { new string[] {"C3","D3","D2"}, false },
            new object[] { new string[] {"D1","D2","F2"}, false },
            new object[] { new string[] {"D5","D6","F6"}, false },
            new object[] { new string[] {"E3","E4","F4"}, false },
            new object[] { new string[] {"E4","F4","F2"}, false },
            new object[] { new string[] {"E5","F6","D6"}, false },
            new object[] { new string[] {"F2","F4","G1"}, false },
            new object[] { new string[] {"G1","G4","F4"}, false },
        };

        [Test]
        [TestCaseSource(nameof(noMills2))]
        public void CheckMill_ConnectedSpaces_ContainingCows_Do_Notform_ALine(string[] c, bool expected)
        {
            Player player1 = new Player("Black");
            IReferee referee = new Referee();

            player1.addPlayedPositions(c[0]);
            player1.addPlayedPositions(c[1]);
            player1.addPlayedPositions(c[2]);

            bool result = referee.isMill(player1);
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void BlackCowsGivenFirstChance()
        {
            IReferee referee = new Referee();
            string currentPlayer = referee.getcurrentPlayer();
            Assert.AreEqual("Black", currentPlayer);
        }
        
        [Test]
        public void CanOnlyMoveToConnectedSpace()
        {
            IPlayer player = new Player("Black");
            IBoard board = new Board();

            player.makeMove("A1", "A4",board);
            player.makeMove("F6", "F4", board);
            player.makeMove("D1", "F4", board);
            Assert.AreEqual(true, board.checkNeighbours("A1").Contains("A4"));
            Assert.AreEqual(true, board.checkNeighbours("F6").Contains("F4"));
            Assert.AreEqual(false, board.checkNeighbours("D1").Contains("F4"));
        }
        
        [Test]

        public void A_maximum_of_12_placements_per_player_are_allowed()
        {
            //Sakhele
            Board board = new Board();
            Player player = new Player("Black");
            

            player.makePlacement("A1" , board);
            player.makePlacement("A4", board);
            player.makePlacement("A7", board);
            player.makePlacement("B2", board);

            player.makePlacement("B4", board);
            player.makePlacement("B6", board);
            player.makePlacement("C3", board);
            player.makePlacement("C4", board);

            player.makePlacement("C5", board);
            player.makePlacement("D1", board);
            player.makePlacement("D2", board);
            player.makePlacement("D3", board);

            player.makePlacement("D5", board);
            player.makePlacement("D6", board);

            Assert.That(player.getNUmOfPlacedCows() == 12);
            
        }

        [Test]
        public void Cows_cannot_be_moved_during_placement()
        {
            //Sakhele

            Board board = new Board();
            Player player = new Player("Black");

            player.makePlacement("A1", board);
            player.makePlacement("A4", board);
            player.makePlacement("A7", board);
            player.makePlacement("B2" ,board);

            player.makeMove("A1", "D1", board);

            Assert.That(board.getPieceAtPos("D1") == ' ');
           
        }

        [Test]
        public void Cows_can_only_be_placed_on_empty_spaces()
        {
            //Sakhele
            Board board = new Board();
            Player player1 = new Player("Black");
            Player player2 = new Player("White");

            player1.makePlacement("A1", board);
            player2.makePlacement("A1", board);

            Assert.That(board.getPieceAtPos("A1") == 'b');
        }


        static object[] noKill =
     {
            new object[] { new string[] {"A1","A4","A7","B2"}, true },
            new object[] { new string[] {"A1","B2","C3", "A4"}, true },
            new object[] { new string[] {"A1","D1","G1", "A4"}, true },
            new object[] { new string[] {"A4","B4","C4","A1"}, true },
            new object[] { new string[] {"A7","B6","C5", "A1"}, true },
            new object[] { new string[] {"A7","D7","G7", "A1"}, true },
            new object[] { new string[] {"B2","B4","B6", "A1"}, true },
            new object[] { new string[] {"B2","D2","F2", "A1"}, true },
            new object[] { new string[] {"B6","D6","F6", "A1"}, true },
            new object[] { new string[] {"C3","C4","C5", "A1"}, true },
            new object[] { new string[] {"C3","D3","E3", "A1"}, true },
            new object[] { new string[] {"D1","D2","D3", "A1"}, true },
            new object[] { new string[] {"D5","D6","D7", "A1"}, true },
            new object[] { new string[] {"E3","E4","E5", "A1"}, true },
            new object[] { new string[] {"E4","F4","G4", "A1"}, true },
            new object[] { new string[] {"E5","F6","G7", "A1"}, true },
            new object[] { new string[] {"F2","F4","F6", "A1"}, true },
            new object[] { new string[] {"G1","G4","G7", "A1"}, true },
        };

        [Test]
        [TestCaseSource(nameof(noKill))]
        public void Cow_in_a_mill_when_nonMill_cows_exist_cannot_be_shot(string[] c, bool expected)
        {
            IPlayer player1 = new Player("Black");
            IPlayer player2 = new Player("White");
            IBoard board = new Board();
            IReferee referee = new Referee();
            Game game = new Game(player1,player2,board,referee);
            
            player1.makePlacement(c[0],board);
            player1.makePlacement(c[1],board);
            player1.makePlacement(c[2],board);
            player1.makePlacement(c[3],board);
            referee.isMill(player1);

            game.eliminate(player1,board, c[0]);
            
            bool result = player1.getPlayedPos().Contains(c[0]);

            Assert.AreEqual(expected, result);
            
        }

        static object[] aKill =
        {
            new object[] { new string[] {"A1","A4","A7"}, false },
            new object[] { new string[] {"A1","B2","C3"}, false },
            new object[] { new string[] {"A1","D1","G1"}, false },
            new object[] { new string[] {"A4","B4","C4"}, false },
            new object[] { new string[] {"A7","B6","C5"}, false },
            new object[] { new string[] {"A7","D7","G7"}, false },
            new object[] { new string[] {"B2","B4","B6"}, false },
            new object[] { new string[] {"B2","D2","F2"}, false },
            new object[] { new string[] {"B6","D6","F6"}, false },
            new object[] { new string[] {"C3","C4","C5"}, false },
            new object[] { new string[] {"C3","D3","E3"}, false },
            new object[] { new string[] {"D1","D2","D3"}, false },
            new object[] { new string[] {"D5","D6","D7"}, false },
            new object[] { new string[] {"E3","E4","E5"}, false },
            new object[] { new string[] {"E4","F4","G4"}, false },
            new object[] { new string[] {"E5","F6","G7"}, false },
            new object[] { new string[] {"F2","F4","F6"}, false },
            new object[] { new string[] {"G1","G4","G7"}, false },
        };
        
        [Test]
        [TestCaseSource(nameof(aKill))]
        public void A_cow_in_a_mill_when_all_cows_are_in_mills_can_be_shot(string[] c, bool expected)
        {
            IPlayer player1 = new Player("Black");
            IPlayer player2 = new Player("White");
            IBoard board = new Board();
            IReferee referee = new Referee();
            Game game = new Game(player1,player2,board,referee);

            player1.makePlacement(c[0],board);
            player1.makePlacement(c[1],board);
            player1.makePlacement(c[2],board);
            referee.isMill(player1);

            referee.swapcurrentPlayer();
            game.eliminate(player1, board, c[0]);

            bool result = player1.getPlayedPos().Contains(c[0]);
            Assert.AreEqual(expected, result);
        }

        static object[] noKill2 =
        {
            new object[] { new string[] {"G1","A4","A1"}, true },
            new object[] { new string[] {"G1","B2","C3"}, true },
            new object[] { new string[] {"A1","D1","G1"}, true },
            new object[] { new string[] {"A4","B4","C4"}, true },
            new object[] { new string[] {"A7","B6","C5"}, true },
            new object[] { new string[] {"A7","D7","G7"}, true },
            new object[] { new string[] {"B2","B4","B6"}, true },
            new object[] { new string[] {"B2","D2","F2"}, true },
            new object[] { new string[] {"B6","D6","F6"}, true },
            new object[] { new string[] {"C3","C4","C5"}, true },
            new object[] { new string[] {"C3","D3","E3"}, true },
            new object[] { new string[] {"D1","D2","D3"}, true },
            new object[] { new string[] {"D5","D6","D7"}, true },
            new object[] { new string[] {"E3","E4","E5"}, true },
            new object[] { new string[] {"E4","F4","G4"}, true },
            new object[] { new string[] {"E5","F6","G7"}, true },
            new object[] { new string[] {"F2","F4","F6"}, true },
            new object[] { new string[] {"G1","G4","G7"}, true },
        };


        [Test]
        [TestCaseSource(nameof(noKill2))]
        public void A_player_cannot_shoot_their_own_cows(string[] c, bool expected)
        {
            IPlayer player1 = new Player("Black");
            IPlayer player2 = new Player("White");
            IBoard board = new Board();
            IReferee referee = new Referee();
            Game game = new Game(player1, player2, board, referee);

            player1.makePlacement(c[0], board);
            player1.makePlacement(c[1], board);
            player1.makePlacement(c[2], board);
           
            game.eliminate(player1, board, c[0]);

            bool result = player1.getPlayedPos().Contains(c[0]);
            Assert.AreEqual(expected, result);
        }

        static object[] aKill3 =
        {
            new object[] { new string[] {"A1","A4","A7"}, true },
            new object[] { new string[] {"A1","B2","C3"}, true },
            new object[] { new string[] {"A1","D1","G1"}, true },
            new object[] { new string[] {"A4","B4","C4"}, true },
            new object[] { new string[] {"A7","B6","C5"}, true },
            new object[] { new string[] {"A7","D7","G7"}, true },
            new object[] { new string[] {"B2","B4","B6"}, true },
            new object[] { new string[] {"B2","D2","F2"}, true },
            new object[] { new string[] {"B6","D6","F6"}, true },
            new object[] { new string[] {"C3","C4","C5"}, true },
            new object[] { new string[] {"C3","D3","E3"}, true },
            new object[] { new string[] {"D1","D2","D3"}, true },
            new object[] { new string[] {"D5","D6","D7"}, true },
            new object[] { new string[] {"E3","E4","E5"}, true },
            new object[] { new string[] {"E4","F4","G4"}, true },
            new object[] { new string[] {"E5","F6","G7"}, true },
            new object[] { new string[] {"F2","F4","F6"}, true },
            new object[] { new string[] {"G1","G4","G7"}, true },
        };

        [Test]
        [TestCaseSource(nameof(aKill3))]
        public void Shot_cows_are_removed_from_the_board(string[] c, bool expected)
        {

            IPlayer player1 = new Player("Black");
            IPlayer player2 = new Player("White");
            IBoard board = new Board();
            IReferee referee = new Referee();
            Game game = new Game(player1, player2, board, referee);

            player1.makePlacement(c[0], board);
            player1.makePlacement(c[1], board);
            player1.makePlacement(c[2], board);
            referee.isMill(player1);

            referee.swapcurrentPlayer();
            game.eliminate(player1, board, c[0]);

            bool result = board.getPieceAtPos(c[0])==' ';
            Assert.AreEqual(expected, result);
        }
        
        
        [Test]
        public void Can_Only_Move_To_An_EmptySpace()
        {
            //Sakhele

            Board board = new Board();
            Player player = new Player("Black");

            player.makePlacement("A1", board);
            player.makePlacement("A4", board);
            player.makePlacement("A7", board);
            player.makePlacement("B2", board);

            player.makePlacement("B4", board);
            player.makePlacement("B6", board);
            player.makePlacement("C3", board);
            player.makePlacement("C4", board);
       
            player.makePlacement("C5", board);
            player.makePlacement("D1", board);
            player.makePlacement("D2", board);
            player.makePlacement("D3", board);

            board.updateBoard("E5", 'w');

            player.makeMove("D3", "E5" , board);

            Assert.That(board.getPieceAtPos("D3") == 'b');
            Assert.That(board.getPieceAtPos("E5") == 'w');

        }

        [Test]

        public void Moving_does_not_increase_or_decrease_the_number_of_cows_on_the_board()
        {
            //Sakhele

            Board board = new Board();
            Player player = new Player("Black");

            player.makePlacement("A1", board);
            player.makePlacement("A4", board);
            player.makePlacement("A7", board);
            player.makePlacement("B2", board);

            player.makePlacement("B4", board);
            player.makePlacement("B6", board);
            player.makePlacement("C3", board);
            player.makePlacement("C4", board);

            player.makePlacement("C5", board);
            player.makePlacement("D1", board);
            player.makePlacement("D2", board);
            player.makePlacement("D3", board);

            player.makeMove("C5", "D5", board);
            player.makeMove("D3", "E5", board);

            Assert.That(board.getNumberOfcows() == 12);
            Assert.That(board.getPieceAtPos("D5") == 'b');
        }
        static object[] aKill4 =
        {
            new object[] { "A1", true },
            new object[] { "A4", true },
            new object[] { "A7", true },
            new object[] { "B2", true },
            new object[] { "B4", true },
            new object[] { "B6", true },
            new object[] { "C3", true },
            new object[] { "C4", true },
            new object[] { "C5", true },
            new object[] { "D1", true },
            new object[] { "D2", true },
            new object[] { "D3", true },
            new object[] { "D5", true },
            new object[] { "D6", true },
            new object[] { "D7", true },
            new object[] { "F2", true },
            new object[] { "G7", true },
            new object[] { "G1", true },
        };
        

        [Test]
        [TestCaseSource(nameof(aKill4))]
        public void CantShotAnEmptySpace(string a, bool expected)
        {
            IReferee referee = new Referee();
            IPlayer playerBlack = new Player("Black");
            IPlayer playerWhite = new Player("White");
            IBoard brd = new Board();
            Game g = new Game(playerBlack, playerWhite, brd, referee);

            referee.updateGameStat("Mill");
            g.eliminate(playerBlack, brd, a);

            bool check = referee.get_GameState() == "Mill";
            Assert.AreEqual(expected, check);
        }

        [Test]

        public void Cows_can_move_to_any_empty_space_if_only_three_cows_of_that_colorremain()
        {
            //Sakhele

            Board board = new Board();
            IPlayer player1 = new Player("Black");
            IPlayer player2 = new Player("White");
            IReferee referee = new Referee();

            Game game = new Game(player1, player2, board, referee);

            player1.makePlacement("A1", board);
            player1.makePlacement("G4", board);
            player1.makePlacement("A7", board);
            player1.makePlacement("G7", board);

            player1.makePlacement("B4", board);
            player1.makePlacement("C3", board);
            player1.makePlacement("C5", board);
            player1.makePlacement("E3", board);

            player1.makePlacement("E5", board);
            player1.makePlacement("D1", board);
            player1.makePlacement("D6", board);
            player1.makePlacement("D2", board);
            referee.swapcurrentPlayer();

            referee.updateGameStat("Mill"); ;
            game.eliminate(player1, board, "A1");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "G4");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "A7");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "G7");

            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "B4");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "C3");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "C5");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "E3");

            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "E5");
            player1.updateState();
            player1.flyCow("D1", "G7", board);
            player1.flyCow("D2", "F2", board);


            Assert.That(board.getPieceAtPos("D1") == ' ');
            Assert.That(board.getPieceAtPos("G7") == 'b');

            Assert.That(board.getPieceAtPos("D3") == ' ');
            Assert.That(board.getPieceAtPos("F2") == 'b');
            Assert.That(board.getPieceAtPos("D1") == ' ');
        }

        [Test]

        public void A_win_occurs_if_an_opponent_has_two_or_fewer_cows_and_placement_isfinished()
        {
            //Sakhele

            Board board = new Board();
            IPlayer player1 = new Player("Black");
            IPlayer player2 = new Player("White");
            IReferee referee = new Referee();

            Game game = new Game(player1, player2, board, referee);

            player1.makePlacement("A1", board);
            player1.makePlacement("G4", board);
            player1.makePlacement("A7", board);
            player1.makePlacement("G7", board);

            player1.makePlacement("B4", board);
            player1.makePlacement("C3", board);
            player1.makePlacement("C5", board);
            player1.makePlacement("E3", board);

            player1.makePlacement("E5", board);
            player1.makePlacement("D1", board);
            player1.makePlacement("D6", board);
            player1.makePlacement("D2", board);
            referee.swapcurrentPlayer();

            referee.updateGameStat("Mill"); ;
            game.eliminate(player1, board, "A1");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "G4");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "A7");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "G7");

            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "B4");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "C3");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "C5");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "E3");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "E5");
            referee.updateGameStat("Mill");
            game.eliminate(player1, board, "D1");

            referee.updateGameStat(player1, new Player("White"));
            bool result = referee.get_GameState() == "Game Over!";

            Assert.AreEqual(true, result);
        }

    }

}

