
using System.Collections.Generic;
namespace NumeronAI
{
	public class GameMaster
	{
		/// <summary>
		/// 桁数
		/// </summary>
		private const int NumeronDigit = 3;

		/// <summary>
		/// 判定
		/// </summary>
		public JudgeResult Judge(List<int> number1, List<int> number2)
		{
			// 数値チェック
			if ((!CheckNumber(number1)) || (!CheckNumber(number2)))
			{
				return new JudgeResult { Eat = -1, Bite = -1 };
			}

			JudgeResult result = new JudgeResult { Eat = 0, Bite = 0 };

			// EATの判定
			for (int i = 0; i < number1.Count; i++)
			{
				// 桁と数一致
				if (number1[i] == number2[i])
				{
					result.Eat++;
				}
				else
				{
					// 数だけ一致
					if (number1[i] == number2[0])
					{
						result.Bite++;
					}
					else if (number1[i] == number2[1])
					{
						result.Bite++;
					}
					else if (number1[i] == number2[2])
					{
						result.Bite++;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// 数値チェック
		/// </summary>
		private bool CheckNumber(List<int> number)
		{
			// 桁数チェック
			if (!CheckDigit(number))
			{
				return false;
			}

			// 桁ごとに重複しないこと
			if ((number[0] == number[1]) ||
				(number[0] == number[2]) ||
				(number[1] == number[2]))
			{
				return false;
			}

			return true;
		}


		/// <summary>
		/// 桁数のチェック
		/// </summary>
		private static bool CheckDigit(List<int> number)
		{
			return (number.Count == NumeronDigit);
		}
	}

	/// <summary>
	/// 判定結果
	/// </summary>
	public class JudgeResult
	{
		/// <summary>
		/// 数字と桁が合っていた場合
		/// </summary>
		public int Eat = -1;

		/// <summary>
		/// 数字は合っているが桁は合っていない場合
		/// </summary>
		public int Bite = -1;
	}
}
