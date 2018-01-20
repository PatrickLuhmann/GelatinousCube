using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelatinousCube_Library
{
    public class Game
    {
		List<int>[] gameBoard; // An array where each element is a list of integers.
		int numSpaces;
		int numPieces;
		GameResults[] results;
		Random rng;

		public Game(int spaces, int pieces)
		{
			numSpaces = spaces;
			gameBoard = new List<int>[numSpaces];
			for (int i = 0; i < numSpaces; i++)
			{
				gameBoard[i] = new List<int>();
			}

			numPieces = pieces;
			results = new GameResults[numPieces];
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = new GameResults();
			}

			// TODO: Do something more intelligent with the seed.
			// TODO: Look into abstracting out the RNG so that it can be mocked.
			rng = new Random(0);
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
			foreach (var res in results)
			{
				if (id == res.Id)
					found = true;
			}
			if (found)
				throw new ArgumentException("That piece ID has already been placed.");

			// Find the first empty place and use it for this piece.
			// By convention, a place is unused when both "place" results are -1.
			// Can't use id because this an opaque handle provided by the user; they might want
			// to use -1 or any other value for their own purposes.
			var empty = Array.Find(results, r => r.FirstPlace == -1.0M);
			empty.Id = id;
			empty.Space = space;
			empty.FirstPlace = 0;
			empty.SecondPlace = 0;

			// Put the piece on its spot. It will be at the front (i.e. on top).
			gameBoard[space - 1].Insert(0, id);
		}

		public GameResults[] SimulateOneTurn()
		{
			const int numTurns = 2;
			for (int i = 0; i < numTurns; i++)
			{
				ExecuteTurn();
			}

			foreach (var res in results)
			{
				res.FirstPlace = res.FirstPlace / (decimal)numTurns;
				res.SecondPlace = res.SecondPlace / (decimal)numTurns;
			}

			return results;
		}

		public GameResults[] ExecuteTurn()
		{
			DetermineTurnOrder();

			for (int i = 0; i < numPieces; i++)
			{
				// Grab the moving piece (and any pieces on top of it).
				List<int> movingPieces = PopPieces(results[i].Space, results[i].Id);

				int roll = rng.Next(1, 4);
				Console.WriteLine("Piece {0} rolls a {1}.", results[i].Id, roll);
				int newSpace = results[i].Space + roll;

				if (newSpace > numSpaces)
				{
					// TODO: Make sure to handle crossing the finish line.

					// Obvioiusly, can't put the pieces back on the board because
					// the new space doesn't exist.

					// Now that the game is over, do not process any more pieces.
					break;
				}

				
				// Move the pieces to their new space.
				AddPiecesToFront(newSpace, movingPieces);
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
			for (int i = (numSpaces - 1); i > 0; i--)
			{
				// Skip empty spaces
				if (gameBoard[i].Count == 0)
					continue;

				if (!foundFirst)
				{
					// Which piece is on top in this space?
					int id = gameBoard[i][0];
					// Find the entry in results that corresponds to this piece.
					var res = Array.Find(results, r => r.Id == id);
					// Increment the first place counter.
					res.FirstPlace++;
					// Remember that we have found first place.
					foundFirst = true;

					// Is there another piece in this space?
					if (gameBoard[i].Count > 1)
					{
						id = gameBoard[i][1];
						// Find the entry in results that corresponds to this piece.
						res = Array.Find(results, r => r.Id == id);
						// Increment the second place counter.
						res.SecondPlace++;
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
					int id = gameBoard[i][0];
					// Find the entry in results that corresponds to this piece.
					var res = Array.Find(results, r => r.Id == id);
					// Increment the second place counter.
					res.SecondPlace++;
					// Remember that we have found second place.
					foundSecond = true;

					// We are done so no need to continue.
					break;
				}
			}

			return results;
		}

		/// <summary>
		/// Remove from the given space the specified piece, along with all above it.
		/// </summary>
		/// <param name="space">The space containing the piece (1-based).</param>
		/// <param name="id">The ID of the piece to pop.</param>
		/// <returns></returns>
		private List<int> PopPieces(int space, int id)
		{
			// Find the target piece on the space.
			int pos = gameBoard[space - 1].IndexOf(id);
			// Get the sub-list from the first to the target.
			List<int> subList = gameBoard[space - 1].GetRange(0, pos + 1);
			// Now remove them from the space.
			gameBoard[space - 1].RemoveRange(0, pos + 1);

			return subList;
		}

		/// <summary>
		/// Add the given list of pieces to the given space.
		/// </summary>
		/// <param name="space">The target space (1-based).</param>
		/// <param name="pieces">The list of pieces.</param>
		private void AddPiecesToFront(int space, List<int> pieces)
		{
			// Insert the list of pieces into the target game board list.
			gameBoard[space - 1].InsertRange(0, pieces);

			// Update the location of the moved pieces.
			foreach(var p in pieces)
			{
				var res = Array.Find(results, r => r.Id == p);
				res.Space = space;
			}
		}

		private void DetermineTurnOrder()
		{
			for (int i = 0; i < numPieces - 1; i++)
			{
				int pos = rng.Next(i, numPieces);
				if (pos != i)
				{
					GameResults tmp = results[i];
					results[i] = results[pos];
					results[pos] = tmp;
				}
			}
		}

		// Public properties
		public int NumSpaces { get { return numSpaces; }  }
		public List<int>[] Spaces { get { return gameBoard; } }
		// TODO: Should there be a property for the results? Or should that just be returned by the corresponding methods?
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
