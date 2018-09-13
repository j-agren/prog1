using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Snake
{
    class Program
    {
        static int direction = 0;

        static void ReadKeys()
        {
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Spacebar)
                {
                    direction--;
                    if (direction < 0)
                        direction = 3;
                }
                if (key.Key == ConsoleKey.RightArrow)
                {
                    direction++;
                    if (direction > 3)
                        direction = 0;
                }
                Thread.Sleep(0);
            }
        }

        static void GenerateCandy(int[,] map)
        {
            Random rnd = new Random();
            int x = rnd.Next(0, 20);
            int y = rnd.Next(0, 20);

            while(map[x, y] == 1)
            {
                x = rnd.Next(0, 20);
                y = rnd.Next(0, 20);
            }
            map[x, y] = 2;
        }

        static void Main(string[] args)
        {
            int[,] map = new int[20, 20];
            int time = 1000;
            List<SnakePosition> positions = new List<SnakePosition>();
            positions.Add(new SnakePosition() { X = 10, Y = 10 });
            positions.Add(new SnakePosition() { X = 10, Y = 10 });
            positions.Add(new SnakePosition() { X = 10, Y = 10 });

            map[positions[0].X, positions[0].Y] = 1;
            GenerateCandy(map);

            Thread keyThread = new Thread(ReadKeys);
            keyThread.Start();

            while (true)
            {
                Console.SetCursorPosition(0, 0);

                for (int y = 0; y < 20; y++)
                {
                    for (int x = 0; x < 20; x++)
                    {
                        if (map[x, y] == 0)
                            Console.Write(". ");
                        else if (map[x, y] == 1)
                            Console.Write("# ");
                        else if (map[x, y] == 2)
                            Console.Write("¤ ");
                    }
                    Console.WriteLine();
                }

                Thread.Sleep(time);

                var tail = positions[positions.Count - 1];
                var head = positions[0];

                if(positions.Count(p => p.X == tail.X && p.Y == tail.Y) <= 1)
                    map[tail.X, tail.Y] = 0;

                switch (direction)
                {
                    case 0: tail.X = head.X + 1; tail.Y = head.Y; break;
                    case 1: tail.Y = head.Y + 1; tail.X = head.X; break;
                    case 2: tail.X = head.X - 1; tail.Y = head.Y; break;
                    case 3: tail.Y = head.Y - 1; tail.X = head.X; break;
                }

                if (map[tail.X, tail.Y] != 2)
                    positions.RemoveAt(positions.Count - 1);
                else
                {
                    GenerateCandy(map);
                    time = time / 2;
                }
                
                positions.Insert(0, tail);
                map[tail.X, tail.Y] = 1;
            }
        }
    }
}
