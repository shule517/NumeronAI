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
			INumeronAI ai1 = null;
			INumeronAI ai2 = null;

			Console.Write("先行後攻を入力してください：1.敵AI 2.ちゃっかりシュール君:");
			string senko = Console.ReadLine();

			if (senko == "1")
			{
				ai1 = new Input();
				ai2 = new GakkariShuleKun();
			}
			else
			{
				ai1 = new GakkariShuleKun();
				ai2 = new Input();
			}

			Console.WriteLine("");
			Console.WriteLine("以下の順番でゲームをはじめます。");
			Console.WriteLine("先行：{0}", ai1.ToString());
			Console.WriteLine("後攻：{0}", ai2.ToString());
			Console.WriteLine("");
			Console.WriteLine("========================================================================");
			Console.WriteLine("");

			List<int> ai1number = ai1.GetNumber();
			Console.WriteLine(ai1.ToString() + "が思い浮かべた数：" + ConverterNumber(ai1number));
			List<int> ai2number = ai2.GetNumber();
			Console.WriteLine(ai2.ToString() + "が思い浮かべた数：" + ConverterNumber(ai2number));
			Console.WriteLine("");

			Console.WriteLine("========================================================================");
			Console.WriteLine("Numer0n START");
			Console.WriteLine("========================================================================");

			int count = 0;
			while (true)
			{
				Console.WriteLine("");
				List<int> ai1Answer = ai1.Answer();
				JudgeResult result1 = master.Judge(ai2number, ai1Answer);
				ai1.SetResult(ai1Answer, result1);
				Console.WriteLine(@"{0}の回答：{1}
-> {2}EAT {3}BITE", ai1.ToString(), ConverterNumber(ai1Answer), result1.Eat, result1.Bite);
				Console.WriteLine("");

				if (result1.Eat == 3)
				{
					Console.WriteLine(@"{0}の勝ち！", ai1.ToString());
					Console.ReadLine();
					return;
				}

				List<int> ai2Answer = ai2.Answer();
				JudgeResult result2 = master.Judge(ai1number, ai2Answer);
				ai2.SetResult(ai2Answer, result2);
				Console.WriteLine(@"{0}の回答：{1}
-> {2}EAT {3}BITE", ai2.ToString(), ConverterNumber(ai2Answer), result2.Eat, result2.Bite);
				Console.WriteLine("");
	
				if (result2.Eat == 3)
				{
					Console.WriteLine(@"{0}の勝ち！", ai2.ToString());
					Console.ReadLine();
					return;
				}

				Console.WriteLine("========================================================================");
				count++;
			}
		}

		private static void test()
		{

			GameMaster master = new GameMaster();

			List<int> resultList = new List<int>();
			int testCount = 1000;
			for (int i = 0; i < testCount; i++)
			{
				INumeronAI ai = new GakkariShuleKun();
				List<int> number = ai.GetNumber();
				//Console.WriteLine(string.Format("想像した値：{0}", ConverterNumber(number)));
				int answerCount = 0;
				while (true)
				{
					answerCount++;
					List<int> answer = ai.Answer();
					JudgeResult result = master.Judge(number, answer);

					ai.SetResult(answer, result);
					//Console.WriteLine(string.Format("AIちゃんの答え({0}回目)：{1} => {2}EAT {3}BITE", answerCount, ConverterNumber(answer), result.Eat, result.Bite));

					if (result.Eat == GameMaster.NumeronDigit)
					{
						resultList.Add(answerCount);
						//Console.WriteLine(string.Format("正解！ {0}回目で正解", answerCount));
						break;
					}
				}
			}


			int max = 0;
			int min = 10;
			int sum = 0;
			List<int> bunpu = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };

			foreach (int result in resultList)
			{
				sum += result;
				if (max < result)
				{
					max = result;
				}
				if (min > result)
				{
					min = result;
				}

				bunpu[result]++;
			}

			float ave = (float)(sum / (float)testCount);
			Console.WriteLine(string.Format("累計回答数：{0}回目で正解", sum));
			Console.WriteLine(string.Format("平均値：{0:F4}回目で正解", ave));
			Console.WriteLine(string.Format("最小値：{0}回目で正解", min));
			Console.WriteLine(string.Format("最大値：{0}回目で正解", max));

			for (int i = 0; i < bunpu.Count; i++)
			{
				if (bunpu[i] != 0)
				{
					Console.WriteLine(string.Format("回答数{0}回：{1}回", i, bunpu[i]));
				}
			}

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
