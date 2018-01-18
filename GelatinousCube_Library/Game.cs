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
				results[i].Id = -1;
				results[i].FirstPlace = -1.0M;
				results[i].SecondPlace = -1.0M;
			}
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
			empty.FirstPlace = 0;
			empty.SecondPlace = 0;

			// Put the piece on its spot. It will be at the front (i.e. on top).
			AddInFront(id, space);
		}

		public GameResults[] ExecuteTurn()
		{
			return results;
		}

		// TODO: This 'space' is 1-based. Make them all consistent.
		void AddInFront(int id, int space)
		{
			gameBoard[space - 1].Insert(0, id);
		}

		// TODO: This 'space' is 0-based. Make them all consistent.
		// TODO: Make these private or eliminate them. The user is not going to manipulate the Game state directly like this.
		public void AddInFront(List<int> ids, int space)
		{
			gameBoard[space].InsertRange(0, ids);
		}

		public void AddInBack(List<int> ids, int space)
		{
			gameBoard[space].AddRange(ids);
		}

		public List<int> Extract(int num, int space)
		{
			List<int> subList = gameBoard[space].GetRange(0, num);
			gameBoard[space].RemoveRange(0, num);
			return subList;
		}

		// Public properties
		public int NumSpaces { get { return numSpaces; }  }
		public List<int>[] Spaces { get { return gameBoard; } }
		// TODO: Should there be a property for the results? Or should that just be returned by the corresponding methods?
    }

	public class GameResults
	{
		public int Id;
		public decimal FirstPlace;
		public decimal SecondPlace;
	}
}
