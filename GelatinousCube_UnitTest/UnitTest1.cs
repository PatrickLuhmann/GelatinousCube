using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GelatinousCube_Library;

namespace GelatinousCube_UnitTest
{
	[TestClass]
	public class UnitTest_Library
	{
		[TestMethod]
		public void DefaultConstructor()
		{
			Game game = new Game();

			Assert.AreEqual(10, game.NumSpaces);
			Assert.AreEqual(10, game.Spaces.Length);
			foreach (var list in game.Spaces)
			{
				Assert.AreEqual(0, list.Count);
			}
		}

		[TestMethod]
		public void Constructor_SmallestGame()
		{
			// Three spaces is what is possible with the default dice for
			// starting positions. Two pieces are needed for an actual race.
			Game game = new Game(3, 2);

			Assert.AreEqual(3, game.NumSpaces);
			Assert.AreEqual(3, game.Spaces.Length);
			foreach (var list in game.Spaces)
			{
				Assert.AreEqual(0, list.Count);
			}

			// TODO: Something for results? Or should I not worry about validating implementation details?
		}

		[TestMethod]
		[ExpectedException(typeof(System.ArgumentException))]
		public void PlacePiece_Duplicate()
		{
			Game game = new Game(11, 3);
			game.PlacePiece(0, 1);
			game.PlacePiece(1, 2);
			game.PlacePiece(2, 3);

			// Duplicate piece ID.
			game.PlacePiece(0, 5);
		}

		[TestMethod]
		[ExpectedException(typeof(System.ArgumentException))]
		public void PlacePiece_BadSpaceTooHigh()
		{
			Game game = new Game(11, 3);

			// Space too high.
			game.PlacePiece(0, 12);
		}

		[TestMethod]
		[ExpectedException(typeof(System.ArgumentException))]
		public void PlacePiece_BadSpaceZero()
		{
			Game game = new Game(11, 3);

			// Space #0 is not valid. From the user's point of view, the first space
			// is #1.
			game.PlacePiece(0, 0);
		}

		[TestMethod]
		[ExpectedException(typeof(System.ArgumentException))]
		public void PlacePiece_BadSpaceNegative()
		{
			Game game = new Game(11, 3);

			// Negative numbers are not valid.
			game.PlacePiece(0, -1);
		}

		[TestMethod]
		public void PlacePiece_AllGood()
		{
			Game game = new Game(16, 5);

			game.PlacePiece(0, 1);
			game.PlacePiece(1, 1);
			game.PlacePiece(2, 2);
			game.PlacePiece(3, 2);
			game.PlacePiece(4, 3);

			Assert.AreEqual(0, game.Pieces[0].Id);
			Assert.AreEqual(1, game.Pieces[1].Id);
			Assert.AreEqual(2, game.Pieces[2].Id);
			Assert.AreEqual(3, game.Pieces[3].Id);
			Assert.AreEqual(4, game.Pieces[4].Id);
			Assert.AreEqual(1, game.Pieces[0].StartingSpace);
			Assert.AreEqual(1, game.Pieces[1].StartingSpace);
			Assert.AreEqual(2, game.Pieces[2].StartingSpace);
			Assert.AreEqual(2, game.Pieces[3].StartingSpace);
			Assert.AreEqual(3, game.Pieces[4].StartingSpace);
		}
	}
}
