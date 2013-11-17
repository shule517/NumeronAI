using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeronAI.AI
{
	class Input : INumeronAI
	{
		GameMaster master = new GameMaster();

		public List<int> GetNumber()
		{
			while (true)
			{
				Console.Write("敵AIの思い浮かんだ数字を入力してください：");
				string str = Console.ReadLine();

				if (str.Length != 3)
				{
					Console.WriteLine(string.Format("入力に間違いがあります({0})：", str));
					continue;
				}

				List<int> numeron = new List<int>();

				int digit1 = int.Parse(str[0].ToString());
				int digit2 = int.Parse(str[1].ToString());
				int digit3 = int.Parse(str[2].ToString());

				numeron.Add(digit1);
				numeron.Add(digit2);
				numeron.Add(digit3);

				if (!master.CheckNumber(numeron))
				{
					Console.WriteLine(string.Format("入力に間違いがあります({0})：", str));
					continue;
				}
				return numeron;
			}
		}

		public List<int> Answer()
		{
			while (true)
			{
				Console.Write("敵AIの回答を入力してください：");
				string str = Console.ReadLine();

				if (str.Length != 3)
				{
					Console.WriteLine(string.Format("入力に間違いがあります({0})：", str));
					continue;
				}

				List<int> numeron = new List<int>();

				int digit1 = int.Parse(str[0].ToString());
				int digit2 = int.Parse(str[1].ToString());
				int digit3 = int.Parse(str[2].ToString());

				numeron.Add(digit1);
				numeron.Add(digit2);
				numeron.Add(digit3);

				if (!master.CheckNumber(numeron))
				{
					Console.WriteLine(string.Format("入力に間違いがあります({0})：", str));
					continue;
				}
				return numeron;
			}
		}

		public void SetResult(List<int> answer, JudgeResult result)
		{
		}

		public override string ToString()
		{
			return "敵AI";
		}
	}
}
