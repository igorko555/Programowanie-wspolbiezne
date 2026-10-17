using System;
using System.Threading;
using System.Threading.Tasks;

namespace Snow
{
    public class StarParameter
    {
        public int Iterations { get; set; }
        public ConsoleColor Color { get; set; }
        public int InitialX { get; set; }
    }

    class Punkt7
    {
        static readonly object consoleLock = new object();

        static void Star(object o)
        {
            StarParameter p = (StarParameter)o;
            int x = p.InitialX;
            int y = 0;

            for (int i = 0; i < p.Iterations; i++)
            {
                int width;
                int height;

                lock (consoleLock)
                {
                    width = Console.WindowWidth;
                    height = Console.WindowHeight;

                    if (x >= width) x = width - 1;
                    if (y >= height) y = 0;

                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = p.Color;
                    Console.Write('*');
                }

                Thread.Sleep(Random.Shared.Next(40, 100));

                lock (consoleLock)
                {
                    if (x < Console.WindowWidth && y < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(' ');
                    }
                }

                y++;
                if (y >= height)
                {
                    y = 0;
                    x = Random.Shared.Next(0, width);
                }

                x += Random.Shared.Next(-1, 2);
                if (x < 0) x = 0;
                if (x >= width) x = width - 1;
            }
        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(64, 64);
            Console.Clear();
            Console.CursorVisible = false;

            int starCount = 30;
            Task[] tasks = new Task[starCount];
            ConsoleColor[] colors = { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Gray };

            for (int i = 0; i < starCount; i++)
            {
                StarParameter param = new StarParameter
                {
                    Iterations = 60,
                    Color = colors[i % colors.Length],
                    InitialX = Random.Shared.Next(0, Math.Max(20, Console.WindowWidth))
                };

                tasks[i] = Task.Factory.StartNew(Star, param);
                Thread.Sleep(Random.Shared.Next(10, 50));
            }

            Task.WaitAll(tasks);

            lock (consoleLock)
            {
                Console.ResetColor();
                Console.CursorVisible = true;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Animacja Snow zakonczona sukcesem.");
            }
        }
    }
}
