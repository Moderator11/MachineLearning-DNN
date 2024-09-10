using System;
using System.Collections.Generic;
using System.Text;

namespace FindTheTreasure
{
    class FindTheTreasure
    {
        int plateWidth, plateHeight;
        int[,] gamePlate;
        int score = 0;
        int playerX, playerY;
        int prev_pX, prev_pY;

        public FindTheTreasure(int _plateWidth = 4, int _plateHeight = 4)
        {
            plateWidth = _plateWidth;
            plateHeight = _plateHeight;
            gamePlate = new int[plateHeight, plateWidth];
        }

        public void StartConsoleInterfacedGame()
        {
            FindTheTreasure game = new FindTheTreasure(10, 10);
            game.Initialize();
            game.Visualize();
            do
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        game.Move(0);
                        break;
                    case ConsoleKey.DownArrow:
                        game.Move(1);
                        break;
                    case ConsoleKey.RightArrow:
                        game.Move(2);
                        break;
                    case ConsoleKey.LeftArrow:
                        game.Move(3);
                        break;
                }
                Console.Clear();
                game.Visualize();
                game.Print(string.Format("score = {0}", game.score));
            } while (game.CheckGameValid() == true);
            Console.WriteLine("Game End");
            Console.ReadKey();
        }

        public void Initialize()
        {
            score = 0;
            Map1();

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

            void Map1()
            {
                ResetPlate();
                playerY = 0;
                playerX = 0;
                gamePlate[playerY, playerX] = 1;
                gamePlate[2, 2] = 2;
            }//4x4

            void Map2()
            {
                ResetPlate();
                playerY = 0;
                playerX = 0;
                gamePlate[playerY, playerX] = 1;
                gamePlate[3, 3] = 2;


                gamePlate[0, 2] = 3;
                gamePlate[1, 2] = 3;
                gamePlate[2, 2] = 3;
                gamePlate[2, 1] = 3;
            }//10x10

            void Map3()
            {
                ResetPlate();
                playerY = 0;
                playerX = 0;
                gamePlate[playerY, playerX] = 1;
                gamePlate[9, 9] = 2;

                gamePlate[0, 2] = 3;
                gamePlate[1, 2] = 3;
                gamePlate[2, 2] = 3;
                gamePlate[2, 1] = 3;

                gamePlate[5, 9] = 3;
                gamePlate[5, 8] = 3;
                gamePlate[5, 7] = 3;
                gamePlate[5, 4] = 3;
                gamePlate[5, 3] = 3;
                gamePlate[5, 2] = 3;
                gamePlate[5, 1] = 3;

                gamePlate[7, 7] = 3;
                gamePlate[8, 7] = 3;
                gamePlate[9, 7] = 3;
            }
        }

        public void Visualize()
        {
            int tileWidth = 2;
            StringBuilder output = new StringBuilder();
            for (int y = 0; y < plateHeight; y++)
            {
                for (int x = 0; x < plateWidth; x++)
                {
                    int numberSize = gamePlate[y, x].ToString().Length;
                    switch (gamePlate[y, x])
                    {
                        case 0: // Air
                            output.Append("0");
                            break;
                        case 1: // Player
                            output.Append("T");
                            break;
                        case 2: // Treasure
                            output.Append("$");
                            break;
                        case 3: // Wall
                            output.Append("X");
                            break;
                    }
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

        public void Move(int action)
        {
            prev_pX = playerX;
            prev_pY = playerY;
            switch(action)
            {
                case 0:
                    playerY--;
                    break;
                case 1:
                    playerY++;
                    break;
                case 2:
                    playerX++;
                    break;
                case 3:
                    playerX--;
                    break;
                default:
                    throw new Exception("Wrong action had been performed.");
            }
            CheckMovable();

            void CheckMovable()
            {
                if (playerX >= 0 && playerY >= 0 && playerX < plateWidth && playerY < plateHeight)
                {
                    switch (gamePlate[playerY, playerX])
                    {
                        case 0:
                            gamePlate[playerY, playerX] = 1;
                            gamePlate[prev_pY, prev_pX] = 0;
                            break;
                        case 2:
                            gamePlate[playerY, playerX] = 1;
                            gamePlate[prev_pY, prev_pX] = 0;
                            score += 10;
                            break;
                        default:
                            playerX = prev_pX;
                            playerY = prev_pY;
                            break;
                    }
                }
                else
                {
                    playerX = prev_pX;
                    playerY = prev_pY;
                }
            }
        }

        public bool CheckGameValid()
        {
            return (score < 10) ? true : false;
        }

        public int[] getStates()
        {
            return new int[] { playerX, playerY, score };
        }

        private void Print(string str)
        {
            Console.WriteLine(str);
        }
    }
}