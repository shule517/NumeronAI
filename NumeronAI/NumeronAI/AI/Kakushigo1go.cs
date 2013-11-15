using System;
using System.Collections.Generic;

namespace NumeronAI.AI
{
	/// <summary>
	/// シュールの隠し子1号AI
	/// 3個の数字が確定したら、それ以外の数字使いません
	/// 平均：19回
	/// </summary>
	class Kakushigo1go : INumeronAI
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
			// NG数字登録
			if ((result.Eat == 0) && (result.Bite == 0))
			{
				foreach (int num in number)
				{
					ngNumber.Add(num);
				}
			}

			// 3個確定
			if ((result.Eat + result.Bite) == GameMaster.NumeronDigit)
			{
				for (int i = 0; i < 10; i++)
				{
					if ((number[0] != i) && (number[1] != i) && (number[2] != i))
					{
						ngNumber.Add(i);
					}
				}
			}
		}
	}
}
