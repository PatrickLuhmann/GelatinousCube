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

			// Create a new game.
			Game MyGame = new Game();
			List<int> camels = new List<int>();

			// Place camel #0 in space #1 (the second space).
			camels.Add(0);
			MyGame.AddInFront(camels, 1);

			// Place camel #1 in space #0 (the first space).
			camels[0] = 1;
			MyGame.AddInFront(camels, 0);

			DisplayGameBoard(MyGame);
		}

		static void DisplayGameBoard(Game game)
		{
			for (int i = 0; i < game.NumSpaces; i++)
			{
				Console.Write("{0}: ", i);
				foreach (int camel in game.Spaces[i])
				{
					Console.Write("{0}, ", camel);
				}
				Console.WriteLine();
			}
		}
	}
}
