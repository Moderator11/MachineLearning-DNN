using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LearningModel;
using Matrices;

namespace MachineLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
        }

        static void Test()
        {
            LearningModelTester tester = new LearningModelTester();
            tester.ConsoleInterface();
        }
    }
}