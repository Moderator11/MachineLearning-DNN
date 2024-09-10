using System;
using System.Collections.Generic;
using System.Text;

namespace TwoZeroFourEightGame
{
    class TwoZeroFourEight//    ;(x)   ;(o)
    {
        int plateWidth, plateHeight;
        int[,] gamePlate;
        int score = 0;
        Random rand = new Random(Guid.NewGuid().GetHashCode());
        double RandomFourChance = 0.1;

        public TwoZeroFourEight(int _plateWidth = 4, int _plateHeight = 4)
        {
            plateWidth = _plateWidth;
            plateHeight = _plateHeight;
            gamePlate = new int[plateHeight, plateWidth];
        }

        public void StartConsoleInterfacedGame()
        {
            TwoZeroFourEight game = new TwoZeroFourEight();
            game.Initialize();
            game.Visualize();
            do
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        game.Push(0);
                        break;
                    case ConsoleKey.DownArrow:
                        game.Push(1);
                        break;
                    case ConsoleKey.RightArrow:
                        game.Push(2);
                        break;
                    case ConsoleKey.LeftArrow:
                        game.Push(3);
                        break;
                }
                Console.Clear();
                game.Visualize();
                game.Print(string.Format("score = {0}", game.score));
            } while (game.CheckPlateValid() == true);
            Console.WriteLine("Game Over");
            Console.ReadKey();
        }

        private void Initialize()
        {
            score = 0;
            ResetPlate();
            SetRandomOnTile();
            SetRandomOnTile();

            void ResetPlate()
            {
                for (int y = 0; y < plateHeight; y++)
                {
                    for (int x = 0; x < plateWidth; x++)
                    {
                        gamePlate[y, x] = 0;
                    }
                }
            }
        }

        private void SetRandomOnTile()
        {
            List<int> emptyX = new List<int>();
            List<int> emptyY = new List<int>();
            GetEmptyTiles();
            SetTile();

            void GetEmptyTiles()
            {
                for (int y = 0; y < plateHeight; y++)
                {
                    for (int x = 0; x < plateWidth; x++)
                    {
                        if (gamePlate[y, x] == 0)
                        {
                            emptyX.Add(x);
                            emptyY.Add(y);
                        }
                    }
                }
            }

            void SetTile()
            {
                if (emptyX.Count > 0)
                {
                    double r1 = rand.NextDouble();
                    int target = rand.Next(0, emptyX.Count);
                    if (r1 <= RandomFourChance)
                    {
                        gamePlate[emptyY[target], emptyX[target]] = 4;
                    }
                    else
                    {
                        gamePlate[emptyY[target], emptyX[target]] = 2;
                    }
                }
                else
                {
                    //Game Over? -> No
                }
            }
        }

        public bool CheckPlateValid()
        {
            bool isThereEmptySpace = false;
            for (int y = 0; y < plateHeight; y++)
            {
                for (int x = 0; x < plateWidth; x++)
                {
                    if (gamePlate[y, x] == 0)
                    {
                        isThereEmptySpace = true;
                    }
                }
            }

            bool isThereMergable = false;
            for (int y = 0; y < plateHeight; y++)
            {
                for (int x = 0; x < plateWidth - 1; x++)
                {
                    if (gamePlate[y, x] == gamePlate[y, x + 1])
                    {
                        isThereMergable = true;
                    }
                }
            }
            for (int x = 0; x < plateWidth; x++)
            {
                for (int y = 0; y < plateHeight - 1; y++)
                {
                    if (gamePlate[y, x] == gamePlate[y + 1, x])
                    {
                        isThereMergable = true;
                    }
                }
            }
            return isThereEmptySpace || isThereMergable;
        }

        public void ResetGame()
        {
            Initialize();
        }

        public void Push(int direction)
        {
            switch (direction)
            {
                case 0:
                    Up();
                    break;
                case 1:
                    Down();
                    break;
                case 2:
                    Right();
                    break;
                case 3:
                    Left();
                    break;
            }

            SetRandomOnTile();

            void Up()
            {
                int[,] mergedCheck = new int[plateHeight, plateWidth];
                for (int y = 1; y < plateHeight; y++)
                {
                    for (int x = 0; x < plateWidth; x++)
                    {
                        if (gamePlate[y, x] != 0)
                        {
                            for (int w = y - 1; w >= 0; w--)
                            {
                                if (gamePlate[w, x] == 0)
                                {
                                    if (w == 0)
                                    {
                                        gamePlate[w, x] = gamePlate[y, x];
                                        gamePlate[y, x] = 0;
                                    }
                                }
                                else
                                {
                                    if (gamePlate[w, x] == gamePlate[y, x] && mergedCheck[w, x] == 0)
                                    {
                                        mergedCheck[w, x] = 1;
                                        gamePlate[w, x] += gamePlate[y, x];
                                        gamePlate[y, x] = 0;
                                        MergeHappend(gamePlate[w, x]);
                                        break;
                                    }
                                    else
                                    {
                                        gamePlate[w + 1, x] = gamePlate[y, x];
                                        if (w + 1 != y)
                                        {
                                            gamePlate[y, x] = 0;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            void Down()
            {
                int[,] mergedCheck = new int[plateHeight, plateWidth];
                for (int y = plateHeight - 2; y >= 0; y--)
                {
                    for (int x = 0; x < plateWidth; x++)
                    {
                        if (gamePlate[y, x] != 0)
                        {
                            for (int w = y + 1; w < plateHeight; w++)
                            {
                                if (gamePlate[w, x] == 0)
                                {
                                    if (w == plateHeight - 1)
                                    {
                                        gamePlate[w, x] = gamePlate[y, x];
                                        gamePlate[y, x] = 0;
                                    }
                                }
                                else
                                {
                                    if (gamePlate[w, x] == gamePlate[y, x] && mergedCheck[w, x] == 0)
                                    {
                                        mergedCheck[w, x] = 1;
                                        gamePlate[w, x] += gamePlate[y, x];
                                        gamePlate[y, x] = 0;
                                        MergeHappend(gamePlate[w, x]);
                                        break;
                                    }
                                    else
                                    {
                                        gamePlate[w - 1, x] = gamePlate[y, x];
                                        if (w - 1 != y)
                                        {
                                            gamePlate[y, x] = 0;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            void Right()
            {
                int[,] mergedCheck = new int[plateHeight, plateWidth];
                for (int y = 0; y < plateHeight; y++)
                {
                    for (int x = plateWidth - 2; x >= 0; x--)
                    {
                        if (gamePlate[y, x] != 0)
                        {
                            for (int w = x + 1; w < plateWidth; w++)
                            {
                                if (gamePlate[y, w] == 0)
                                {
                                    if (w == plateWidth - 1)
                                    {
                                        gamePlate[y, w] = gamePlate[y, x];
                                        gamePlate[y, x] = 0;
                                    }
                                }
                                else
                                {
                                    if (gamePlate[y, w] == gamePlate[y, x] && mergedCheck[y, w] == 0)
                                    {
                                        mergedCheck[y, w] = 1;
                                        gamePlate[y, w] += gamePlate[y, x];
                                        gamePlate[y, x] = 0;
                                        MergeHappend(gamePlate[y, w]);
                                        break;
                                    }
                                    else
                                    {
                                        gamePlate[y, w - 1] = gamePlate[y, x];
                                        if (w - 1 != x)
                                        {
                                            gamePlate[y, x] = 0;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            void Left()
            {
                int[,] mergedCheck = new int[plateHeight, plateWidth];
                for (int y = 0; y < plateHeight; y++)
                {
                    for (int x = 1; x < plateWidth; x++)
                    {
                        if (gamePlate[y, x] != 0)
                        {
                            for (int w = x - 1; w >= 0; w--)
                            {
                                if (gamePlate[y, w] == 0)
                                {
                                    if (w == 0)
                                    {
                                        gamePlate[y, w] = gamePlate[y, x];
                                        gamePlate[y, x] = 0;
                                    }
                                }
                                else
                                {
                                    if (gamePlate[y, w] == gamePlate[y, x] && mergedCheck[y, w] == 0)
                                    {
                                        mergedCheck[y, w] = 1;
                                        gamePlate[y, w] += gamePlate[y, x];
                                        gamePlate[y, x] = 0;
                                        MergeHappend(gamePlate[y, w]);
                                        break;
                                    }
                                    else
                                    {
                                        gamePlate[y, w + 1] = gamePlate[y, x];
                                        if (w + 1 != x)
                                        {
                                            gamePlate[y, x] = 0;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            void MergeHappend(int mergedNumber)
            {
                score += mergedNumber;
            }
        }

        public void Visualize()
        {
            int tileWidth = 5;
            StringBuilder output = new StringBuilder();
            for (int y = 0; y < plateHeight; y++)
            {
                for (int x = 0; x < plateWidth; x++)
                {
                    int numberSize = gamePlate[y, x].ToString().Length;
                    output.Append(gamePlate[y, x]);
                    for (int i = 0; i < tileWidth - numberSize; i++)
                    {
                        output.Append(" ");
                    }
                }
                output.Append(Environment.NewLine);
                output.Append(Environment.NewLine);
            }
            Print(output.ToString());
        }

        private void Print(string str)
        {
            Console.WriteLine(str);
        }


        // AI feature
        public double[,] GetStatus()
        {
            double[,] status = new double[1, plateHeight * plateWidth];
            for (int y = 0; y < plateHeight; y++)
            {
                for (int x = 0; x < plateWidth; x++)
                {
                    status[0, y * plateHeight + x] = gamePlate[y, x];
                }
            }
            return status;
        }

        public double GetScore()
        {
            return score;
        }
        //
    }
}