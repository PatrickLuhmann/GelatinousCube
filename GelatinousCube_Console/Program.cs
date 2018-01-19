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
		}

		static void DisplayGameBoard(Game game)
		{
			for (int i = 0; i < game.NumSpaces; i++)
			{
				Console.Write("{0}: ", i+1);
				foreach (int camel in game.Spaces[i])
				{
					Console.Write("{0}, ", camel);
				}
				Console.WriteLine();
			}
		}

		static void DisplayResults(GameResults[] results)
		{
			foreach (var res in results)
			{
				Console.WriteLine("Piece {0} at {3}: {1} in first place, {2} in second place.",
					res.Id,
					res.FirstPlace,
					res.SecondPlace,
					res.Space);
			}
		}
	}
}
