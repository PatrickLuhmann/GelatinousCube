using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelatinousCube_Library
{
    public class Game
    {
		List<int>[] spaces; // An array where each element is a list of integers.
		int numSpaces;

		public Game()
		{
			numSpaces = 10;
			spaces = new List<int>[numSpaces];
			for (int i = 0; i < numSpaces; i++)
			{
				spaces[i] = new List<int>();
			}
		}

		public void AddInFront(List<int> ids, int space)
		{
			spaces[space].InsertRange(0, ids);
		}

		public void AddInBack(List<int> ids, int space)
		{
			spaces[space].AddRange(ids);
		}

		public List<int> Extract(int num, int space)
		{
			List<int> subList = spaces[space].GetRange(0, num);
			spaces[space].RemoveRange(0, num);
			return subList;
		}

		public int NumSpaces { get { return numSpaces; }  }

		public List<int>[] Spaces { get { return spaces; } }
    }
}
