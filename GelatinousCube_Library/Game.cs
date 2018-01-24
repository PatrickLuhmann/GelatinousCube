using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelatinousCube_Library
{
    public class Game
    {
		List<Piece> gamePieces; // The pieces, in order of placement.
		List<Piece>[] gameBoard; // An array where each element is a list of integers.
		int numSpaces;
		// TODO: Can't I remove this and just use the Count of gamePieces?
		int numPieces;
		GameResults[] results;
		Random rng;

		public Game(int spaces, int pieces)
		{
			numSpaces = spaces;
			gameBoard = new List<Piece>[numSpaces];
			for (int i = 0; i < numSpaces; i++)
			{
				gameBoard[i] = new List<Piece>();
			}

			numPieces = pieces;
			// Allocate the list now, but the Piece objects when they are placed.
			gamePieces = new List<Piece>(numPieces);

			results = new GameResults[numPieces];
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = new GameResults();
			}

			// TODO: Do something more intelligent with the seed.
			// TODO: Look into abstracting out the RNG so that it can be mocked.
			rng = new Random();
		}

		public Game() : this(10, 5) { }

		/// <summary>
		/// Put the given piece on the given space.
		/// </summary>
		/// <param name="id">The ID of the piece to place.</param>
		/// <param name="space">The space on which to place the piece. The first space is 1, not 0.</param>
		public void PlacePiece(int id, int space)
		{
			// Check to see if the space is valid.
			if (space > numSpaces || space < 1)
				throw new ArgumentException("That space is not valid.");

			// Check to see if this piece has already been placed.
			bool found = false;
			found = gamePieces.Exists(p => p.Id == id);
			if (found)
				throw new ArgumentException("That piece ID has already been placed.");

			// Create a new piece and add it to the end. This provides an implicit
			// order to the pieces; the first in the list is placed first, and thus
			// will be under any subsequent pieces placed in the same space.
			gamePieces.Add(new Piece(id, space));
		}

		public GameResults[] SimulateOneTurn()
		{
			const int numReps = 20000;

			for (int i = 0; i < numReps; i++)
			{
				// Clear the board.
				foreach (var s in gameBoard)
				{
					s.Clear();
				}

				// Place the pieces.
				foreach (var p in gamePieces)
				{
					// Space is 1-based, array is 0-based.
					gameBoard[p.StartingSpace - 1].Add(p);
				}

				// Run the simulation.
				ExecuteTurn();
			}

			// Calculate the results.
			for (int idx = 0; idx < results.Length; idx++)
			{
				results[idx].Id = gamePieces[idx].Id;
				results[idx].FirstPlace = (decimal)gamePieces[idx].FirstPlaceCount / numReps;
				results[idx].SecondPlace = (decimal)gamePieces[idx].SecondPlaceCount / numReps;
			}

			return results;
		}

		public GameResults[] ExecuteTurn()
		{
			int[] order = DetermineTurnOrder();

			for (int i = 0; i < numPieces; i++)
			{
				bool endOfGame = MovePiece(order[i]);
				if (endOfGame)
					break;
			}

			// Process results
			bool foundFirst = false;
			bool foundSecond = false;
			// Don't need to check array element 0 (space #1) because
			// it is not possible for a piece to end up there (they have
			// to start on space #1 or beyond, and they will move forward
			// at least one space).
			// TODO: This assumption is not true if oasis tokens are in play,
			// as there could be a "-1" in space #2 that a piece in space #1
			// might land on.
			for (int i = numSpaces - 1; i > 0; i--)
			{
				// Skip empty spaces
				if (gameBoard[i].Count == 0)
					continue;

				if (!foundFirst)
				{
					// Which piece is on top in this space?
					Piece piece = gameBoard[i][gameBoard[i].Count - 1];
					// Increment the first place counter.
					piece.FirstPlaceCount++;
					// Remember that we have found first place.
					foundFirst = true;

					// Is there another piece in this space?
					if (gameBoard[i].Count > 1)
					{
						// Find the corresponding Piece object.
						piece = gameBoard[i][gameBoard[i].Count - 2];
						// Increment the second place counter.
						piece.SecondPlaceCount++;
						// Remember that we have found second place.
						foundSecond = true;

						// We are done so no need to continue.
						break;
					}
					// No more pieces on this space, so go to the next one.
					continue;
				}

				if (!foundSecond)
				{
					// Which piece is on top in this space?
					var piece = gameBoard[i][gameBoard[i].Count - 1];
					// Increment the second place counter.
					piece.SecondPlaceCount++;
					// Remember that we have found second place.
					foundSecond = true;

					// We are done so no need to continue.
					break;
				}
			}

			return results;
		}

		public bool MovePiece(int id)
		{
			Piece movingPiece = gamePieces.Find(p => p.Id == id);

			// Roll the die for the piece.
			// TODO: Might add die characteristics
			int roll = rng.Next(1, 4);
//			Console.WriteLine("Piece {0} rolls a {1}.", id, roll);

			// Find the piece on the board.
			for (int idx = movingPiece.StartingSpace - 1; idx < numSpaces; idx++)
			{
				if (gameBoard[idx].Exists(p => p.Id == id))
				{
					// Extract the piece and all on top of it.
					int pos = gameBoard[idx].IndexOf(movingPiece);
					int count = gameBoard[idx].Count - pos;
					List<Piece> subList = gameBoard[idx].GetRange(pos, count);
					gameBoard[idx].RemoveRange(pos, count);

					// Move the pieces to their new space.
					gameBoard[idx + roll].AddRange(subList);
					// TODO: If Piece is refactored to track its current space,
					// update it here.
					// TODO: For a slight optimization, StartingSpace for each
					// piece could be updated. This would help any carried piece
					// that has not itself moved yet.

//					Console.WriteLine("  {0} pieces were moved from space {1} to space {2}.", count, idx + 1, idx + roll + 1);
					break;
				}
			}

			// TODO: Check for end of game condition.
			return false;
		}

		private int[] DetermineTurnOrder()
		{
			int[] turnOrder = new int[numPieces];
			for (int i = 0; i < numPieces; i++)
				turnOrder[i] = gamePieces[i].Id;

			for (int i = 0; i < numPieces - 1; i++)
			{
				int pos = rng.Next(i, numPieces);
				if (pos != i)
				{
					int tmp = turnOrder[i];
					turnOrder[i] = turnOrder[pos];
					turnOrder[pos] = tmp;
				}
			}

			return turnOrder;
		}

		// Public properties
		public int NumSpaces { get { return numSpaces; }  }
		public List<Piece>[] Spaces { get { return gameBoard; } }
		public List<Piece> Pieces {  get { return gamePieces; } }
		// TODO: Should there be a property for the results? Or should that just be returned by the corresponding methods?
    }

	public class Piece
	{
		public int Id;
		public int StartingSpace; // 1-based space where the piece starts.
		public int PlacementOrder; // Do I need this? Or will order be implicit in the array/list?
		public int FirstPlaceCount;
		public int SecondPlaceCount;

		public Piece(int id, int start)
		{
			Id = id;
			StartingSpace = start;
			FirstPlaceCount = SecondPlaceCount = 0;
		}
	}

	public class GameResults
	{
		public int Id;
		public int Space;
		public decimal FirstPlace;
		public decimal SecondPlace;

		public GameResults()
		{
			Id = -1;
			Space = 0; // start off the board by default.
			FirstPlace = -1.0M;
			SecondPlace = -1.0M;
		}
	}
}
