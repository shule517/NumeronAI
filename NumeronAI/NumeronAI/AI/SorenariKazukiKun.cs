using System;
using System.Collections.Generic;

namespace NumeronAI.AI
{
	/// <summary>
	/// それなりかずき君
	/// ランダム値を使って回答する
	/// ３文字確定していない場合はその答えを入れ替えても無駄
	/// 平均値：10回
	/// </summary>
	public class SorenariKazukiKun : INumeronAI
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

		/// <summary>
		/// 対象外番号
		/// </summary>
		private List<int> ngNumber = new List<int>();

		/// <summary>
		/// 対象外番号
		/// </summary>
		private List<List<int>> ngNumeronNumber = new List<List<int>>();

		/// <summary>
		/// ○桁目の対象外番号
		/// </summary>
		private List<int> ngDigit1 = new List<int>();
		private List<int> ngDigit2 = new List<int>();
		private List<int> ngDigit3 = new List<int>();

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
				/*
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
				 */

				answer = GetNumber();

				// NG番号があったらやり直し
				if (IsNgNumber())
				{
					continue;
				}

				// 指定桁数にNG番号があったらやり直し
				if (IsNgDigit())
				{
					continue;
				}

				// NGぬめろん番号があったらやり直し
				if (IsNgNumeronNumber())
				{
					continue;
				}

				// 正常な値かチェック
				if (master.CheckNumber(answer))
				{
					return answer;
				}
			}
		}

		/// <summary>
		/// 対象外ぬめろん番号がないかチェック
		/// </summary>
		private bool IsNgNumeronNumber()
		{
			foreach (List<int> numeronNumber in ngNumeronNumber)
			{
				if ((answer[0] == numeronNumber[0]) &&
					(answer[1] == numeronNumber[1]) &&
					(answer[2] == numeronNumber[2]))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// 指定桁のNG番号がいないかチェック
		/// </summary>
		private bool IsNgDigit()
		{
			foreach (int ng in ngDigit1)
			{
				if (answer[0] == ng)
				{
					return true;
				}
			}
			foreach (int ng in ngDigit2)
			{
				if (answer[1] == ng)
				{
					return true;
				}
			}
			foreach (int ng in ngDigit3)
			{
				if (answer[2] == ng)
				{
					return true;
				}
			}

			return false;
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
			// 回答した番号をNGに登録
			if (result.Eat != 3)
			{
				List<int> ng = new List<int>(number);
				ngNumeronNumber.Add(ng);
			}

			// 全ての数字は間違い(EAT + BITE = 0) -> NG番号を登録
			if (((result.Eat + result.Bite) == 0))
			{
				foreach (int num in number)
				{
					ngNumber.Add(num);
				}
			}

			// 3個確定(EAT + BITE = 3)
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
			// 3個確定してないため、並び変えてもだめ(EAT + BITE != 3)
			else
			{
				// NG
				List<int> ng = null;
				ng = new List<int>() { number[0], number[1], number[2] };
				ngNumeronNumber.Add(ng);
				ng = new List<int>() { number[0], number[2], number[1] };
				ngNumeronNumber.Add(ng);
				ng = new List<int>() { number[1], number[0], number[2] };
				ngNumeronNumber.Add(ng);
				ng = new List<int>() { number[1], number[2], number[0] };
				ngNumeronNumber.Add(ng);
				ng = new List<int>() { number[2], number[0], number[1] };
				ngNumeronNumber.Add(ng);
				ng = new List<int>() { number[2], number[1], number[0] };
				ngNumeronNumber.Add(ng);
			}

			// 0EATの場合
			// 各桁ごとのNG番号登録
			if (result.Eat == 0)
			{
				ngDigit1.Add(number[0]);
				ngDigit2.Add(number[1]);
				ngDigit3.Add(number[2]);
			}
		}
	}
}
