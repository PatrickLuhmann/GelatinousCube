using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GelatinousCube_Library;

namespace GelatinousCube_Console
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Welcome to the GelatinousCube console.");

#if false

			// Create a new game with 10 spaces and 2 pieces.
			Game MyGame = new Game(10, 2);

			// Place piece #0 in space #2 (the second space).
			MyGame.PlacePiece(0, 2);

			// Place piece #1 in space #1 (the first space).
			MyGame.PlacePiece(1, 1);

			DisplayGameBoard(MyGame);

			GameResults[] TurnResults = MyGame.ExecuteTurn();

			DisplayResults(TurnResults);

			DisplayGameBoard(MyGame);
#endif

			// Simulate a turn.
			Game game2 = new Game(16, 3);
			
			// Place the pieces such that they can't interact. This should
			// help me to judge the accuracy of the simulation.
			// In this case, P1 will always be in first place and P0 will
			// always be in second place.
			// TODO: It feels like I also want the distribution of pieces
			// at the end of the turn. However, I am not sure how to record
			// that data. It isn't just the ending space that is important,
			// it is the stacking within the space.
			game2.PlacePiece(0, 1);
			game2.PlacePiece(1, 1);
			game2.PlacePiece(2, 1);
			//DisplayGameBoard(game2);

			GameResults[] game2Results = game2.SimulateOneTurn();

			DisplayGameBoard(game2);
			DisplayResults(game2Results);
		}

		static void DisplayGameBoard(Game game)
		{
			for (int i = 0; i < game.NumSpaces; i++)
			{
				Console.Write("{0}: ", i+1);
				foreach (Piece piece in game.Spaces[i])
				{
					Console.Write("{0}, ", piece.Id);
				}
				Console.WriteLine();
			}
		}

		static void DisplayResults(GameResults[] results)
		{
			foreach (var res in results)
			{
				Console.WriteLine("Piece {0} at {3}: {1:N2} in first place, {2:N2} in second place.",
					res.Id,
					res.FirstPlace,
					res.SecondPlace,
					res.Space);
			}
		}
	}
}
