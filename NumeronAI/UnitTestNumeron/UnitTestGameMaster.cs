using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumeronAI;
using System.Collections.Generic;

namespace UnitTestNumeron
{
	[TestClass]
	public class UnitTestGameMaster
	{
		[TestMethod]
		public void TestJudge()
		{

			List<int> number = new List<int>() { 1, 2, 3 };

			// 例外
			test(new List<int>() { 0, 0, 0 }, new List<int>() { 1, 2, 3 }, new JudgeResult { Eat = -1, Bite = -1 });
			test(new List<int>() { 1, 2, 3 }, new List<int>() { 0, 0, 0 }, new JudgeResult { Eat = -1, Bite = -1 });

			// 正常
			test(new List<int>() { 1, 2, 3 }, new List<int>() { 1, 2, 3 }, new JudgeResult { Eat = 3, Bite = 0 });
			test(new List<int>() { 1, 2, 3 }, new List<int>() { 1, 2, 4 }, new JudgeResult { Eat = 2, Bite = 0 });
			test(new List<int>() { 1, 2, 3 }, new List<int>() { 1, 4, 5 }, new JudgeResult { Eat = 1, Bite = 0 });
			test(new List<int>() { 1, 2, 3 }, new List<int>() { 3, 1, 2 }, new JudgeResult { Eat = 0, Bite = 3 });
			test(new List<int>() { 1, 2, 3 }, new List<int>() { 2, 1, 4 }, new JudgeResult { Eat = 0, Bite = 2 });
			test(new List<int>() { 1, 2, 3 }, new List<int>() { 3, 4, 5 }, new JudgeResult { Eat = 0, Bite = 1 });
			test(new List<int>() { 1, 2, 3 }, new List<int>() { 4, 5, 6 }, new JudgeResult { Eat = 0, Bite = 0 });
			test(new List<int>() { 1, 2, 3 }, new List<int>() { 1, 3, 4 }, new JudgeResult { Eat = 1, Bite = 1 });
			test(new List<int>() { 1, 2, 3 }, new List<int>() { 1, 3, 2 }, new JudgeResult { Eat = 1, Bite = 2 });

			// EAT2はあり得ない -> つまりEAT3
		}

		private static void test(List<int> number1, List<int> number2, JudgeResult answer)
		{
			GameMaster master = new GameMaster();
			JudgeResult result = master.Judge(number1, number2);

			Assert.AreEqual(result.Eat, answer.Eat);
			Assert.AreEqual(result.Bite, answer.Bite);
		}
	}
}
