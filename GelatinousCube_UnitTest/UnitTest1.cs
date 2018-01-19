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

			Assert.AreEqual(1, game.Spaces[0][0]);
			Assert.AreEqual(0, game.Spaces[0][1]);
			Assert.AreEqual(3, game.Spaces[1][0]);
			Assert.AreEqual(2, game.Spaces[1][1]);
			Assert.AreEqual(4, game.Spaces[2][0]);
		}

		[TestMethod]
		public void PopPieces_OnePiece()
		{
			Game game = new Game(10, 4);
			PrivateObject pvt = new PrivateObject(game);

			game.PlacePiece(100, 6);
			game.PlacePiece(1, 5);
			game.PlacePiece(2, 7);
			game.PlacePiece(3, 1);

			List<int> pieces = (List<int>)pvt.Invoke("PopPieces", 6, 100);

			Assert.AreEqual(1, pieces.Count);
			Assert.AreEqual(100, pieces[0]);
		}

		[TestMethod]
		public void PopPieces_TwoOfTwo()
		{
			Game game = new Game(10, 4);
			PrivateObject pvt = new PrivateObject(game);

			game.PlacePiece(100, 7);
			game.PlacePiece(101, 7);
			game.PlacePiece(2, 6);
			game.PlacePiece(3, 8);

			List<int> pieces = (List<int>)pvt.Invoke("PopPieces", 7, 100);

			Assert.AreEqual(2, pieces.Count);
			Assert.AreEqual(101, pieces[0]);
			Assert.AreEqual(100, pieces[1]);
		}

		[TestMethod]
		public void PopPieces_TwoOfThree()
		{
			Game game = new Game(10, 4);
			PrivateObject pvt = new PrivateObject(game);

			game.PlacePiece(100, 10);
			game.PlacePiece(101, 10);
			game.PlacePiece(99, 10);
			game.PlacePiece(3, 8);

			List<int> pieces = (List<int>)pvt.Invoke("PopPieces", 10, 101);

			Assert.AreEqual(2, pieces.Count);
			Assert.AreEqual(99, pieces[0]);
			Assert.AreEqual(101, pieces[1]);
		}

		[TestMethod]
		public void AddPiecesToFront_TwoToEmptySpace()
		{
			Game game = new Game(10, 4);
			PrivateObject pvt = new PrivateObject(game);

			// We need to use the API so that the results records will
			// be updated correctly.
			game.PlacePiece(100, 2);
			game.PlacePiece(200, 2);
			game.PlacePiece(300, 2);
			game.PlacePiece(400, 2);
			List<int> pieces = (List<int>)pvt.Invoke("PopPieces", 2, 300);

			pvt.Invoke("AddPiecesToFront", 1, pieces);

			// Check the pieces on the target space.
			// For each piece, check for the correct ID as
			// well as the correct space in the GameResults object.
			Assert.AreEqual(2, game.Spaces[0].Count);

			Assert.AreEqual(300, game.Spaces[0][1]);
			var res300 = Array.Find((GameResults[])pvt.GetField("results"), r => r.Id == 300);
			Assert.AreEqual(1, res300.Space);

			Assert.AreEqual(400, game.Spaces[0][0]);
			var res400 = Array.Find((GameResults[])pvt.GetField("results"), r => r.Id == 400);
			Assert.AreEqual(1, res400.Space);
		}

		[TestMethod]
		public void AddPiecesToFront_OneOnThree()
		{
			Game game = new Game(10, 4);
			PrivateObject pvt = new PrivateObject(game);

			// We need to use the API so that the results records will
			// be updated correctly.
			game.PlacePiece(100, 10);
			game.PlacePiece(200, 10);
			game.PlacePiece(300, 1);
			game.PlacePiece(400, 10);
			List<int> pieces = (List<int>)pvt.Invoke("PopPieces", 1, 300);

			pvt.Invoke("AddPiecesToFront", 10, pieces);

			// Check the pieces on the target space.
			// For each piece, check for the correct ID as
			// well as the correct space in the GameResults object.
			Assert.AreEqual(4, game.Spaces[10 - 1].Count);

			Assert.AreEqual(300, game.Spaces[10 - 1][0]);
			var res300 = Array.Find((GameResults[])pvt.GetField("results"), r => r.Id == 300);
			Assert.AreEqual(10, res300.Space);

			Assert.AreEqual(400, game.Spaces[10 - 1][1]);
			var res400 = Array.Find((GameResults[])pvt.GetField("results"), r => r.Id == 400);
			Assert.AreEqual(10, res400.Space);

			Assert.AreEqual(200, game.Spaces[10 - 1][2]);
			var res200 = Array.Find((GameResults[])pvt.GetField("results"), r => r.Id == 200);
			Assert.AreEqual(10, res200.Space);

			Assert.AreEqual(100, game.Spaces[10 - 1][3]);
			var res100 = Array.Find((GameResults[])pvt.GetField("results"), r => r.Id == 100);
			Assert.AreEqual(10, res100.Space);
		}
	}
}
