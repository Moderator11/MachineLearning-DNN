using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrices;
using RandomSeedGenerator;

namespace LearningModel
{
    [Serializable]
    class LearningModel_QLearning//https://ai.stackexchange.com/questions/10719/why-does-deep-q-network-outputs-multiple-q-values/10720#10720
    {
        int replayBufferSize = 1000;
        int batchSize = 64;
        double learningRate = 0.01;
        double discountFactor = 0.9;
        double epsilonStart = 1.0;
        public double epsilonEnd = 0.1;
        double epsilonDecay = 100000;
        int stepDone = 0;
        Random actRandomness = new Random(RSG.CreateSeedFromNewGuid());//Near True RNG needed
        Queue<replayData> replayBuffer;

        int targetNetworkUpdateCycle = 100;
        int currentCycle = 0;
        public LearningModel<double> predictNetwork;
        public LearningModel<double> targetNetwork;
        int[] layerModel;

        int trainIteration = 1;

        [Serializable]
        public class replayData
        {
            public Matrix<double> state, action, nextstate;
            public double reward;

            public replayData(Matrix<double> _state, Matrix<double> _action, double _reward, Matrix<double> _nextstate)
            {
                state = _state.DeepCopy();
                action = _action.DeepCopy();
                reward = _reward;
                nextstate = _nextstate.DeepCopy();
            }
        }

        public LearningModel_QLearning(int[] _layerModel, string[] _activationFunctions, string _lossFunction)
        {
            Initialize();

            void Initialize()
            {
                stepDone = 0;
                replayBuffer = new Queue<replayData>();
                predictNetwork = new LearningModel<double>(_layerModel, _activationFunctions, _lossFunction);
                targetNetwork = new LearningModel<double>(_layerModel, _activationFunctions, _lossFunction);
                predictNetwork.Initialize_Silent();
                targetNetwork.Initialize_Silent();
                targetNetwork.biases = predictNetwork.biases;
                targetNetwork.weights = predictNetwork.weights;
                predictNetwork.learningRate = learningRate;
                targetNetwork.learningRate = learningRate;
                layerModel = _layerModel;
            }
        }

        public void SetMemory(replayData _memData)
        {
            if (replayBuffer.Count < replayBufferSize)
            {
                replayBuffer.Enqueue(_memData);
            }
            else
            {
                replayBuffer.Dequeue();
                replayBuffer.Enqueue(_memData);
            }
        }

        public int GetAction(Matrix<double> state)
        {
            int actionCount = layerModel[0] - state.GetMatrix().GetLength(1);
            double epsilonThreshold = GetDecayedEpsilon();
            stepDone++;
            if (actRandomness.NextDouble() > epsilonThreshold)
            {
                int a; double q;
                GetMaxQ(predictNetwork, state, out q, out a);
                return a;
            }
            else
                return actRandomness.Next(0, actionCount);
        }

        public double GetDecayedEpsilon()
        {
            return epsilonEnd + (epsilonStart - epsilonEnd) * Math.Exp(-1 * stepDone / epsilonDecay);
        }

        public void ResetStepCount()
        {
            stepDone = 0;
        }

        public int GetGreedyAction(Matrix<double> state)
        {
            int a; double q;
            GetMaxQ(predictNetwork, state, out q, out a);
            return a;
        }

        public int GetDumbAction(Matrix<double> state)
        {
            int actionCount = layerModel[0] - state.GetMatrix().GetLength(1);
            return actRandomness.Next(0, actionCount);
        }//state no needed

        public void GetMaxQ(LearningModel<double> model, Matrix<double> state, out double maxQ, out int maxQIndex)
        {
            int actionCount = layerModel[0] - state.GetMatrix().GetLength(1);
            double[,] availableAct = new double[actionCount, actionCount];
            for (int y = 0; y < actionCount; y++)
            {
                for (int x = 0; x < actionCount; x++)
                {
                    availableAct[y, x] = (((actionCount * y + x) % (actionCount + 1)) == 0) ? 1 : 0;
                }
            }

            double[,] qVals = GetQ(model, state, new Matrix<double>(availableAct)).GetMatrix();
            maxQ = qVals[0, 0];
            maxQIndex = 0;
            for (int i = 1; i < qVals.GetLength(0); i++)
            {
                if (maxQ < qVals[i, 0])
                {
                    maxQ = qVals[i, 0];
                    maxQIndex = i;
                }
            }
        }

        public Matrix<double> GetQ(LearningModel<double> model, Matrix<double> state, Matrix<double> action)
        {
            return model.GetBatchPredict(GetPair(state, action));
        }

        public Matrix<double> GetPair(Matrix<double> state, Matrix<double> action)
        {
            double[,] s = state.GetMatrix();
            double[,] a = action.GetMatrix();
            double[,] pair = new double[a.GetLength(0), s.GetLength(1) + a.GetLength(1)];

            for (int y = 0; y < a.GetLength(0); y++)
            {
                for (int x = 0; x < pair.GetLength(1); x++)
                {
                    if (x < s.GetLength(1))
                    {
                        pair[y, x] = s[0, x];
                    }
                    else
                    {
                        pair[y, x] = a[y, x - s.GetLength(1)];
                    }
                }
            }

            return new Matrix<double>(pair);
        }

        public void Update()
        {
            if (replayBuffer.Count < batchSize) { return; }
            replayData[] buffer = replayBuffer.ToArray();

            //OneByOne();
            AllAtOnce();

            void OneByOne()
            {
                for (int i = 0; i < batchSize; i++)
                {
                    LearnSingle(buffer[actRandomness.Next(0, buffer.Length)]);
                }

                void LearnSingle(replayData replayData)
                {
                    Matrix<double> a = replayData.action;
                    Matrix<double> s = replayData.state;
                    double r = replayData.reward;
                    Matrix<double> ns = replayData.nextstate;

                    int maxTargetQIndex;
                    double maxTargetQ;
                    GetMaxQ(targetNetwork, ns, out maxTargetQ, out maxTargetQIndex);
                    predictNetwork.Train_Silent(GetPair(s, a), new Matrix<double>(new double[,] { { r + discountFactor * maxTargetQ } }));

                    //TrainToUpdateQValue();
                    CopyToTargetNetwork();

                    //Deprecated
                    void TrainToUpdateQValue()
                    {
                        //double[,] predict = predictNetwork.GetPredict(s).GetMatrix();
                        double[,] target = targetNetwork.GetPredict(ns).GetMatrix();
                        double quality_MAX = target[0, 0];
                        int maxQIndex = 0;
                        for (int i = 1; i < layerModel[layerModel.Length - 1]; i++)
                        {
                            if (quality_MAX < target[0, i])
                            {
                                quality_MAX = target[0, i];
                                maxQIndex = i;
                            }
                        }
                        //predict[0, maxQIndex] = (1 - learningRate) * predict[0, maxQIndex] + learningRate * (r + discountFactor * target[0, maxQIndex]);
                        Console.WriteLine();
                        Matrix<double>.EnumerateMatrix(target);
                        target[0, maxQIndex] = (r + discountFactor * target[0, maxQIndex]);
                        Matrix<double>.EnumerateMatrix(target);
                        predictNetwork.Train_Silent(s, new Matrix<double>(target));
                    }

                    void CopyToTargetNetwork()
                    {
                        if (currentCycle >= targetNetworkUpdateCycle)
                        {
                            targetNetwork.biases = predictNetwork.biases;
                            targetNetwork.weights = predictNetwork.weights;
                            currentCycle = 0;
                        }
                        else
                        {
                            currentCycle++;
                        }
                    }
                }
            }

            void AllAtOnce()
            {
                int sSize = buffer[0].state.GetMatrix().GetLength(1);
                int aSize = buffer[0].action.GetMatrix().GetLength(1);
                double[,] pair = new double[batchSize, sSize + aSize];
                double[,] expectedQ = new double[batchSize, 1];

                for (int i = 0; i < batchSize; i++)
                {
                    replayData rd = buffer[actRandomness.Next(0, buffer.Length)];
                    for (int k = 0; k < pair.GetLength(1); k++)
                    {
                        if (k < sSize)
                        {
                            pair[i, k] = rd.state.GetMatrix()[0, k];
                        }
                        else
                        {
                            pair[i, k] = rd.action.GetMatrix()[0, k - sSize];
                        }
                        int maxQIndex;
                        double maxQ;
                        GetMaxQ(targetNetwork, rd.nextstate, out maxQ, out maxQIndex);
                        expectedQ[i, 0] = rd.reward + discountFactor * maxQ;
                    }
                }
                for (int i = 0; i < trainIteration; i++)
                {
                    predictNetwork.Train_BatchSilent(new Matrix<double>(pair), new Matrix<double>(expectedQ));
                }
            }
        }

        public int GetReplayBufferCount()
        {
            return replayBuffer.Count;
        }

        public void ResetReplayBuffer()
        {
            replayBuffer.Clear();
        }
    }
}