using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


using Matrices;

namespace TwoZeroFourEightGame
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Initialize
            //TwoZeroFourEight game = new TwoZeroFourEight();
            //game.StartConsoleInterfacedGame();

            //FindTheTreasure.FindTheTreasure game = new FindTheTreasure.FindTheTreasure();
            //game.StartConsoleInterfacedGame();
            LearningModel.LearningModel_QLearning dqn;

            if (File.Exists(Environment.CurrentDirectory + @"\DDQN.dat"))
            {
                dqn = Load();
                Console.WriteLine("Pre-Existing data has been loaded.");
            }
            else
            {
                dqn = new LearningModel.LearningModel_QLearning(new int[] { 6, 32, 1 }, new string[] { "relu", "leakyrelu", "leakyrelu" }, "MSE");
                Console.WriteLine("Cannot find pre-existing data, creating new one.");
            }

            FindTheTreasure.FindTheTreasure game = new FindTheTreasure.FindTheTreasure(4, 4);
            #endregion
            #region Training
            Console.WriteLine("test game before training? (y/n)");
            if (Console.ReadLine() == "y") { TestGame(dqn, game); }
            
            int episods = 100;
            for (int i = 0; i < episods; i++)
            {
                Console.WriteLine("Episode {0}", i);
                RunGame(dqn, game);
            }
            
            /*Parallel.For(0, episods, (i) =>
            {
                Console.WriteLine("Episode {0}", i);
                lock (dqn)
                {
                    RunGame(dqn, game);
                }
            });*/

            Console.WriteLine("save trained data? (y/n)");
            if (Console.ReadLine() == "y") { Save(dqn); }
            TestGame(dqn, game);
            #endregion
        }

        static void RunGame(LearningModel.LearningModel_QLearning dqn, FindTheTreasure.FindTheTreasure game)
        {
            //dqn.ResetReplayBuffer();
            //dqn.ResetStepCount();
            game.Initialize();
            double totalR = 0;
            do
            {
                double score = game.getStates()[2];
                Matrix<double> s = new Matrix<double>(new double[,] { { game.getStates()[0], game.getStates()[1] } });
                int a = dqn.GetAction(s);
                game.Move(a);
                Matrix<double> ns = new Matrix<double>(new double[,] { { game.getStates()[0], game.getStates()[1] } });
                double r = (game.getStates()[2] == 10) ? 1 : -1;
                totalR += r;
                LearningModel.LearningModel_QLearning.replayData rd = new LearningModel.LearningModel_QLearning.replayData(s, ActionToMatrix(a, 4), r, ns);
                dqn.SetMemory(rd);
                dqn.Update();
                //game.Visualize();
                //Console.WriteLine("memory : {0}", dqn.GetReplayBufferCount());
            } while (game.CheckGameValid() == true);
            Console.WriteLine("total Reward : {0}", totalR);
            Console.WriteLine("epsilon : {0}", dqn.GetDecayedEpsilon());
        }

        static void TestGame(LearningModel.LearningModel_QLearning dqn, FindTheTreasure.FindTheTreasure game, int loopLimit = 100)
        {
            game.Initialize();
            do
            {
                loopLimit--;
                Matrix<double> s = new Matrix<double>(new double[,] { { game.getStates()[0], game.getStates()[1] } });
                game.Move(dqn.GetGreedyAction(s));
                game.Visualize();
                Console.ReadKey();
                Console.Clear();
            } while (game.CheckGameValid() == true && loopLimit > 0);
        }

        static void InspectQ(LearningModel.LearningModel_QLearning dqn, int x, int y)
        {
            Matrix<double> s = new Matrix<double>(new double[,] { { x, y } });
            double[] qs = new double[4];
            qs[0] = dqn.GetQ(dqn.predictNetwork, s, new Matrix<double>(new double[,] { { 1, 0, 0, 0 } })).GetMatrix()[0, 0];
            qs[1] = dqn.GetQ(dqn.predictNetwork, s, new Matrix<double>(new double[,] { { 0, 1, 0, 0 } })).GetMatrix()[0, 0];
            qs[2] = dqn.GetQ(dqn.predictNetwork, s, new Matrix<double>(new double[,] { { 0, 0, 1, 0 } })).GetMatrix()[0, 0];
            qs[3] = dqn.GetQ(dqn.predictNetwork, s, new Matrix<double>(new double[,] { { 0, 0, 0, 1 } })).GetMatrix()[0, 0];

            Console.WriteLine("UP = {0}", qs[0]);
            Console.WriteLine("DO = {0}", qs[1]);
            Console.WriteLine("RI = {0}", qs[2]);
            Console.WriteLine("LE = {0}", qs[3]);
        }

        static Matrix<double> ActionToMatrix(int action, int totalActionCount)
        {
            double[,] a = new double[1, totalActionCount];
            a[0, action] = 1;//is other indexes are set to 0 automatically?
            return new Matrix<double>(a);
        }

        static void Save(LearningModel.LearningModel_QLearning model)
        {
            Stream ws = new FileStream(Environment.CurrentDirectory + @"\DDQN.dat", FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(ws, model); // 직렬화
            ws.Close();
        }

        static LearningModel.LearningModel_QLearning Load()
        {
            Stream rs = new FileStream(Environment.CurrentDirectory + @"\DDQN.dat", FileMode.Open);
            BinaryFormatter deserializer = new BinaryFormatter();
            LearningModel.LearningModel_QLearning model = (LearningModel.LearningModel_QLearning)deserializer.Deserialize(rs); // 역 직렬화
            rs.Close();
            return model;
        }
    }
}