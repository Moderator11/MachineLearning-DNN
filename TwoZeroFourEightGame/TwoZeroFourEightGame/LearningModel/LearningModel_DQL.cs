using System;
using Matrices;

namespace LearningModel
{
    class LearningModel_DQL<T>
    {
        //Probably most important thing to check, when open this project, check this first. Im now in 2022-07-31 12:06pm, I gotta go kim hyeon seung kuk u hak won ssibal
        //su nung 109 il nam um... emi
        //https://ai.stackexchange.com/questions/20384/what-is-the-target-q-value-in-dqns



        //https://greentec.github.io/reinforcement-learning-second/
        //https://medium.com/intro-to-artificial-intelligence/deep-q-network-dqn-applying-neural-network-as-a-functional-approximation-in-q-learning-6ffe3b0a9062

        string path = Environment.CurrentDirectory + @"\Model-Data.dqldat";

        int[] layerModel;

        public Matrix<T>[] R;
        public Matrix<T>[] Q;

        public T mapMax = (T)Convert.ChangeType(1, typeof(T));
        public T mapMin = (T)Convert.ChangeType(-1, typeof(T));
        public T learningRate = (T)Convert.ChangeType(0.01, typeof(T));

        LearningModel<T> predNetwork;
        LearningModel<T> targNetwork;

        public void Initialize()
        {
            predNetwork = new LearningModel<T>(new int[] { 6, 4, 4, 4 }, new string[] { "relu", "leakyrelu", "leakyrelu", "leakyrelu" }, "MSE");
            targNetwork = new LearningModel<T>(new int[] { 2, 4, 4, 4 }, new string[] { "relu", "leakyrelu", "leakyrelu", "leakyrelu" }, "MSE");
        }

        public void Train()
        {

        }
    }
}