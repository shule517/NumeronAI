using NumeronAI.AI;
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
			int max = 0;
			for (int i = 0; i < 10; i++)
			{
				INumeronAI ai = new SorenariKazukiKun();
				List<int> number = ai.GetNumber();
				Console.WriteLine(string.Format("想像した値：{0}", ConverterNumber(number)));
				int answerCount = 0;
				while (true)
				{
					answerCount++;
					List<int> answer = ai.Answer();
					JudgeResult result = master.Judge(number, answer);

					ai.SetResult(answer, result);
					Console.WriteLine(string.Format("AIちゃんの答え({0}回目)：{1} => {2}EAT {3}BITE", answerCount, ConverterNumber(answer), result.Eat, result.Bite));

					if (result.Eat == GameMaster.NumeronDigit)
					{
						if (max < answerCount)
						{
							max = answerCount;
						}
						Console.WriteLine(string.Format("正解！ {0}回目で正解", answerCount));
						sum += answerCount;
						break;
					}
				}
			}

			Console.WriteLine(string.Format("平均値：{0:F4}回目で正解", (float)(sum / 10000.0)));
			Console.WriteLine(string.Format("最大値：{0}回目で正解", max));

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
