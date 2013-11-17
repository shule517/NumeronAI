using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeronAI.AI
{
	/// <summary>
	/// それなり君
	/// 0EATの時に、○桁目に△じゃないことを覚えてる
	/// 平均値：15回
	/// </summary>
	class SorenariKun : INumeronAI
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

				// 正常な値かチェック
				if (master.CheckNumber(answer))
				{
					return answer;
				}
			}
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

			// 0EATの場合
			if (result.Eat == 0)
			{
				ngDigit1.Add(number[0]);
				ngDigit2.Add(number[1]);
				ngDigit3.Add(number[2]);
			}
		}
	}
}
