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
		public void AddInFront()
		{
			Game game = new Game();

			// Add to an empty space.
			List<int> justFive = new List<int>();
			justFive.Add(5);
			game.AddInFront(justFive, 7);

			for (int i = 0; i < game.Spaces.Length; i++)
			{
				if (i == 7)
				{
					Assert.AreEqual(1, game.Spaces[i].Count);
					Assert.AreEqual(5, game.Spaces[i][0]);
				}
				else
				{
					Assert.AreEqual(0, game.Spaces[i].Count);
				}
			}

			// Add to a non-empty space.
			List<int> justOne = new List<int>();
			justOne.Add(1);
			game.AddInFront(justOne, 7);

			for (int i = 0; i < game.Spaces.Length; i++)
			{
				if (i == 7)
				{
					Assert.AreEqual(2, game.Spaces[i].Count);
					Assert.AreEqual(1, game.Spaces[i][0]);
					Assert.AreEqual(5, game.Spaces[i][1]);
				}
				else
				{
					Assert.AreEqual(0, game.Spaces[i].Count);
				}
			}

			// Add multiple to a non-empty space.
			List<int> multiNums = new List<int>();
			multiNums.Add(2);
			multiNums.Add(3);
			game.AddInFront(multiNums, 7);

			for (int i = 0; i < game.Spaces.Length; i++)
			{
				if (i == 7)
				{
					Assert.AreEqual(4, game.Spaces[i].Count);
					Assert.AreEqual(2, game.Spaces[i][0]);
					Assert.AreEqual(3, game.Spaces[i][1]);
					Assert.AreEqual(1, game.Spaces[i][2]);
					Assert.AreEqual(5, game.Spaces[i][3]);
				}
				else
				{
					Assert.AreEqual(0, game.Spaces[i].Count);
				}
			}
		}

		[TestMethod]
		public void AddInBack()
		{
			Game game = new Game();

			// Add to an empty space.
			List<int> justFive = new List<int>();
			justFive.Add(5);
			game.AddInBack(justFive, 7);

			for (int i = 0; i < game.Spaces.Length; i++)
			{
				if (i == 7)
				{
					Assert.AreEqual(1, game.Spaces[i].Count);
					Assert.AreEqual(5, game.Spaces[i][0]);
				}
				else
				{
					Assert.AreEqual(0, game.Spaces[i].Count);
				}
			}

			// Add to a non-empty space.
			List<int> justOne = new List<int>();
			justOne.Add(1);
			game.AddInBack(justOne, 7);

			for (int i = 0; i < game.Spaces.Length; i++)
			{
				if (i == 7)
				{
					Assert.AreEqual(2, game.Spaces[i].Count);
					Assert.AreEqual(5, game.Spaces[i][0]);
					Assert.AreEqual(1, game.Spaces[i][1]);
				}
				else
				{
					Assert.AreEqual(0, game.Spaces[i].Count);
				}
			}

			// Add multiple to a non-empty space.
			List<int> multiNums = new List<int>();
			multiNums.Add(2);
			multiNums.Add(3);
			game.AddInBack(multiNums, 7);

			for (int i = 0; i < game.Spaces.Length; i++)
			{
				if (i == 7)
				{
					Assert.AreEqual(4, game.Spaces[i].Count);
					Assert.AreEqual(5, game.Spaces[i][0]);
					Assert.AreEqual(1, game.Spaces[i][1]);
					Assert.AreEqual(2, game.Spaces[i][2]);
					Assert.AreEqual(3, game.Spaces[i][3]);
				}
				else
				{
					Assert.AreEqual(0, game.Spaces[i].Count);
				}
			}
		}

		[TestMethod]
		public void Extract()
		{
			Game game = new Game();

			// Create a space with multiple items.
			List<int> bigList = new List<int>();
			bigList.Add(1);
			bigList.Add(6);
			bigList.Add(2);
			bigList.Add(9);
			game.AddInFront(bigList, 4);

			// Extract the first two items.
			List<int> subList = game.Extract(2, 4);

			Assert.AreEqual(2, subList.Count);
			Assert.AreEqual(1, subList[0]);
			Assert.AreEqual(6, subList[1]);

			Assert.AreEqual(2, game.Spaces[4].Count);
			Assert.AreEqual(2, game.Spaces[4][0]);
			Assert.AreEqual(9, game.Spaces[4][1]);
		}
	}
}
