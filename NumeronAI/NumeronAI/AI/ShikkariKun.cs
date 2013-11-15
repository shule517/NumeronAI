
using System;
using System.Collections.Generic;
namespace NumeronAI
{
	/// <summary>
	/// しっかり君AI
	/// </summary>
	class ShikkariKun : INumeronAI
	{
		static Random random = new Random(Environment.TickCount);
		private static int seed = 0;

		GameMaster master = new GameMaster();

		/// <summary>
		/// ランダムの重複なしの3ケタを返す
		/// </summary>
		public List<int> GetNumber()
		{
			List<int> number = null;

			while (true)
			{
				number = new List<int>();

				for (int i = 0; i < GameMaster.NumeronDigit; i++)
				{
					number.Add(random.Next() % 10);
				}

				if (master.CheckNumber(number))
				{
					return number;
				}
			}
		}

		private List<int> answer = new List<int> { 0, 0, 0};

		/// <summary>
		/// 1から順番に数えてく
		/// </summary>
		public List<int> Answer()
		{
			List<int> result = new List<int>();

			while (true)
			{
				answer[2]++;

				if (answer[2] >= 10)
				{
					answer[1]++;
					answer[2] -= 10;
				}

				if (answer[1] >= 10)
				{
					answer[0]++;
					answer[1] -= 10;
				}

				if (master.CheckNumber(answer))
				{
					return answer;
				}
			}
		}
	}
}
