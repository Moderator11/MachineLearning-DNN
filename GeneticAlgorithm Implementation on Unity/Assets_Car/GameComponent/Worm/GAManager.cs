using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MachineLearning;
using LearningModel;
using Matrices;
using System.Linq;

public class GAManager : MonoBehaviour
{
    public int poolSize = 20;
    GeneticAlgorithm<float> ga;
    LearningModel<float> dummy;

    public GameObject testInstance;

    public TestInstance[] tests;

    public int generation = 0;


    void Start()
    {
        dummy = new LearningModel<float>(new int[] { 8, 10, 10, 10, 2 }, new string[] { "leakyrelu", "leakyrelu", "leakyrelu", "leakyrelu", "leakyrelu" }, "MSE");
        dummy.Initialize_Silent();
        ga = new GeneticAlgorithm<float>(poolSize, dummy, 1, -1, 0.9f, 0.25f);
        tests = new TestInstance[poolSize];
        NewGeneration();
    }

    void Update()
    {
        bool allTestDone = false;
        for (int i = 0; i < poolSize; i++)
        {
            if (tests[i].testDone == false) { break; }
            if (i == poolSize - 1) { allTestDone = true; }
        }
        if (allTestDone == true)
        {
            EndGeneration();
        }
    }

    void NewGeneration()
    {
        generation++;
        for (int i = 0; i < poolSize; i++)
        {
            tests[i] = Instantiate(testInstance).GetComponent<TestInstance>();
            tests[i].agent = new LearningModel<float>(new int[] { 8, 10, 10, 10, 2 }, new string[] { "leakyrelu", "leakyrelu", "leakyrelu", "leakyrelu", "leakyrelu" }, "MSE");
            tests[i].agent.Initialize_Silent();
            ga.SetupMachine(tests[i].agent, i);
        }
        for (int i = 0; i < poolSize; i++)
        {
            tests[i].startTest = true;
        }
        Debug.Log(string.Format("Generation {0} started.", generation));
    }

    void EndGeneration()
    {
        for (int i = 0; i < poolSize; i++)
        {
            ga.SetScore(i, tests[i].score);
            Destroy(tests[i].gameObject);
        }
        ga.EvaluateGeneration();
        ga.CrossOver_HillSlice();
        Debug.Log(string.Format("Bestie score : {0}\nGeneration {1} Ended.", tests[poolSize - 1].score, generation)); 
        NewGeneration();
    }
}
