using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumeronAI
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			GameMaster master = new GameMaster();

			int sum = 0;
			for (int i = 0; i < 10000; i++)
			{
				INumeronAI ai = new ShikkariKun();
				List<int> number = ai.GetNumber();
				int answerCount = 0;
				while (true)
				{
					answerCount++;
					List<int> answer = ai.Answer();
					JudgeResult result = master.Judge(number, answer);
					//Console.WriteLine(string.Format("AIちゃんの答え({0}回目)：{1} => {2}EAT {3}BITE", answerCount, ConverterNumber(answer), result.Eat, result.Bite));

					if (result.Eat == GameMaster.NumeronDigit)
					{
						Console.WriteLine(string.Format("正解！ {0}回目で正解", answerCount));
						sum += answerCount;
						break;
					}
				}
			}

			Console.WriteLine(string.Format("平均値：{0}回目で正解", sum / 10000));

			/*
			// AIは重複なしの3桁の数を思い浮かべる事ができる
			for (int i = 0; i < 100; i++)
			{
				Console.WriteLine(string.Format("AIちゃんが考えた数値：{0}", ConverterNumber(ai.GetNumber())));
			}
			 */
		}

		static string ConverterNumber(List<int> number)
		{
			string result = "";

			foreach (int num in number)
			{
				result += num;
			}

			return result;
		}
	}
}
