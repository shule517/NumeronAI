
using System;
using System.Collections.Generic;
namespace NumeronAI.AI
{
	/// <summary>
	/// あほ君AI
	/// 0EAT 0BITE の数字は使いません！
	/// 成績：平均50回
	/// </summary>
	class AhoKun : INumeronAI
	{
		static Random random = new Random(Environment.TickCount);
		private static int seed = 0;

		GameMaster master = new GameMaster();

		/// <summary>
		/// ランダムの重複なしの3ケタを返す
		/// </summary>
		List<int> INumeronAI.GetNumber()
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

		/// <summary>
		/// 対象外番号
		/// </summary>
		private List<int> ngNumber = new List<int>();

		/// <summary>
		/// 答え
		/// </summary>
		private List<int> answer = new List<int> { 0, 0, 0 };

		/// <summary>
		/// 1から順番に数えてく
		/// </summary>
		List<int> INumeronAI.Answer()
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

				if (IsNgNumber())
				{
					continue;
				}

				if (master.CheckNumber(answer))
				{
					return answer;
				}
			}
		}

		/// <summary>
		/// NG番号が含まれているか
		/// </summary>
		private bool IsNgNumber()
		{
			foreach (int number in answer)
			{
				foreach (int ng in ngNumber)
				{
					if (number == ng)
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// 結果を聞く
		/// </summary>
		void INumeronAI.SetResult(List<int> number, JudgeResult result)
		{
			if ((result.Eat == 0) && (result.Bite == 0))
			{
				ngNumber.Add(number[0]);
				ngNumber.Add(number[1]);
				ngNumber.Add(number[2]);
			}
		}
	}
}
