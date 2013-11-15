
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

			// EATの判定


			// BITEの判定


			return new JudgeResult { Bite = 0, Eat = 0 };
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
