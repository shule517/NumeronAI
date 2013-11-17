
using System;
using System.Collections.Generic;
namespace NumeronAI.AI
{
	class MinmindahaKun : INumeronAI
	{
		/// <summary>
		/// ランダム生成クラス
		/// </summary>
		static Random random = new Random(Environment.TickCount);

		/// <summary>
		/// ゲームマスター
		/// </summary>
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
		/// 評価
		/// </summary>
		private List<int> point = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

		/// <summary>
		/// 桁ごとの評価
		/// </summary>
		private List<int> pointDigit1 = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		private List<int> pointDigit2 = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		private List<int> pointDigit3 = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

		/// <summary>
		/// 判定結果履歴
		/// </summary>
		private List<ResultHistory> resultList = new List<ResultHistory>();

		/// <summary>
		/// 1から順番に数えてく
		/// </summary>
		List<int> INumeronAI.Answer()
		{
			List<int> result = new List<int>();

			List<PointNumber> pointNumber = new List<PointNumber>();

			for (int i = 0; i < 1000; i++)
			{
				PointNumber p = new PointNumber();
				string strNum = String.Format("{0:D3}", i);
				p.Number[0] = int.Parse(strNum[0].ToString());
				p.Number[1] = int.Parse(strNum[1].ToString());
				p.Number[2] = int.Parse(strNum[2].ToString());

				// ぬめろん番号かチェック
				if (!master.CheckNumber(p.Number))
				{
					continue;
				}

				// NG番号があったらやり直し
				if (IsNgNumber(p.Number))
				{
					continue;
				}

				// 指定桁数にNG番号があったらやり直し
				if (IsNgDigit(p.Number))
				{
					continue;
				}

				// NGぬめろん番号があったらやり直し
				if (IsNgNumeronNumber(p.Number))
				{
					continue;
				}

				int pointSum = 0;
				foreach (int data in point)
				{
					pointSum += data;
				}

				// １回でもEAT/BITEが出ている場合は、その数字を使うこと
				/*
				if (pointSum != 0)
				{
					if (
						!(
							(point[p.Number[0]] != 0) ||
							(point[p.Number[1]] != 0) ||
							(point[p.Number[2]] != 0)
						)
					)
					{
						continue;
					}
				}
				 */

				foreach (ResultHistory data in resultList)
				{
					JudgeResult judgeResult = master.Judge(data.Number, p.Number);

					if (judgeResult.Eat != data.Result.Eat)
					{
						continue;
					}

					if (judgeResult.Bite != data.Result.Bite)
					{
						continue;
					}
				}

				//p.Point += point[p.Number[0]];
				//p.Point += point[p.Number[1]];
				//p.Point += point[p.Number[2]];

				// 7.53
				float pointWeight = 10.0f;
				float digitWeight = 0.7f;
				p.Point += (point[p.Number[0]] * pointWeight) + (pointDigit1[p.Number[0]] * digitWeight);
				p.Point += (point[p.Number[1]] * pointWeight) + (pointDigit2[p.Number[1]] * digitWeight);
				p.Point += (point[p.Number[2]] * pointWeight) + (pointDigit3[p.Number[2]] * digitWeight);

				//p.Point += pointDigit1[p.Number[0]];
				//p.Point += pointDigit2[p.Number[1]];
				//p.Point += pointDigit3[p.Number[2]];

				pointNumber.Add(p);
			}

			pointNumber.Sort(delegate(PointNumber p1, PointNumber p2) { return p1.Point.CompareTo(p2.Point) * -1; });

			Console.WriteLine("残り数：" + pointNumber.Count);
			int index = 0;
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

				/*
				answer = GetNumber();
				 */

				answer = pointNumber[index].Number;
				index++;

				 //answer = pointNumber[(random.Next() % 1000)].Number;

				// NG番号があったらやり直し
				if (IsNgNumber(answer))
				{
					continue;
				}

				// 指定桁数にNG番号があったらやり直し
				if (IsNgDigit(answer))
				{
					continue;
				}

				// NGぬめろん番号があったらやり直し
				if (IsNgNumeronNumber(answer))
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
		private bool IsNgNumeronNumber(List<int> numeronNumber)
		{
			foreach (List<int> number in ngNumeronNumber)
			{
				if ((numeronNumber[0] == number[0]) &&
					(numeronNumber[1] == number[1]) &&
					(numeronNumber[2] == number[2]))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// 指定桁のNG番号がいないかチェック
		/// </summary>
		private bool IsNgDigit(List<int> numeronNumber)
		{
			foreach (int ng in ngDigit1)
			{
				if (numeronNumber[0] == ng)
				{
					return true;
				}
			}
			foreach (int ng in ngDigit2)
			{
				if (numeronNumber[1] == ng)
				{
					return true;
				}
			}
			foreach (int ng in ngDigit3)
			{
				if (numeronNumber[2] == ng)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// NG番号が含まれているか
		/// </summary>
		private bool IsNgNumber(List<int> numeronNumber)
		{
			foreach (int number in numeronNumber)
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
			// 結果履歴を更新
			ResultHistory resultHistory = new ResultHistory();
			resultHistory.Number = number;
			resultHistory.Result = result;
			resultList.Add(resultHistory);

			// 点数評価
			foreach (int num in number)
			{
				point[num] += result.Eat + result.Bite;
			}

			// 桁毎の評価
			pointDigit1[number[0]] += result.Eat + result.Bite;
			pointDigit2[number[1]] += result.Eat + result.Bite;
			pointDigit3[number[2]] += result.Eat + result.Bite;

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

	class PointNumber
	{
		public float Point = 0;
		public List<int> Number = new List<int> { 0, 0, 0 };
	}

	public class ResultHistory
	{
		public List<int> Number;
		public JudgeResult Result;
	}
}
