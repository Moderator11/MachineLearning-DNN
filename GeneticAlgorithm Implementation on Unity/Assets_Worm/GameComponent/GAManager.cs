using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MachineLearning;
using LearningModel;
using Matrices;

public class GAManager : MonoBehaviour
{
    public int poolSize = 20;
    GeneticAlgorithm<float> ga;
    LearningModel<float> agent;

    public GameObject creaturePrefab;
    public GameObject targetPrefab;

    public int creatureID;
    public GameObject creature;
    public GameObject target;

    public int generation = 0;

    public float limitTimer = 15f;
    public float currentTImer = 0f;
    public float successMargin = 3f;
    public int successCountThreshold = 5;
    public int successCount = 0;

    void Start()
    {
        agent = new LearningModel<float>(new int[] { 8, 6, 4, 2 }, new string[] { "leakyrelu", "leakyrelu", "leakyrelu", "leakyrelu" }, "MSE");
        agent.Initialize_Silent();
        ga = new GeneticAlgorithm<float>(poolSize, agent, 1, -1, 0.9f, 0.25f);
        Start_Generation();
    }

    void Update()
    {
        if (creature != null)
        {
            MoveCreature();
            CheckStatus();
        }
    }

    void MoveCreature()
    {
        float[,] o = agent.GetPredict(GetInputData(creature, target)).GetMatrix();
        creature.GetComponent<CreatureController>().MoveHead(o[0, 0]);
        creature.GetComponent<CreatureController>().MoveTail(o[0, 1]);
    }

    void CheckStatus()
    {
        if (Vector3.Distance(creature.GetComponent<CreatureController>().GetPosition(), target.transform.position) < successMargin)
        {
            if (successCount >= successCountThreshold)
            {
                Success();
            }
            else
            {
                successCount++;
                RefreshTarget();
            }
        }
        else if (creature.GetComponent<CreatureController>().GetPosition().y < -5f)
        {
            Failed(true);
        }
        else if (currentTImer < limitTimer)
        {
            currentTImer += Time.deltaTime;
        }
        else
        {
            Failed();
        }
    }

    void Success()
    {
        Debug.Log("GA Success");
    }

    void Failed(bool fall = false)
    {
        float score
            = (50 - Vector3.Distance(creature.GetComponent<CreatureController>().GetPosition(), target.transform.position)) / 50
            + ((limitTimer - currentTImer) / limitTimer)
            + creature.GetComponent<CreatureController>().travelDistance / 50
            + (fall ? 0 : 1);
        if (score < 0) { score = 0; }
        ga.SetScore(creatureID, score);
        if (creatureID == poolSize - 1)
        {
            End_Generation();
        }
        else
        {
            Next_Generation();
        }
    }

    void Start_Generation()
    {
        generation++;
        creatureID = 0;
        target = Instantiate(targetPrefab);
        Debug.Log(string.Format("Generation {0} started.", generation));
        SpawnCreature();
    }

    void Next_Generation()
    {
        creatureID++;
        Destroy(creature);
        SpawnCreature();
    }

    void End_Generation()
    {
        ga.EvaluateGeneration();
        ga.CrossOver_HillSlice();
        Debug.Log(string.Format("Generation {0} ended.", generation));
        Debug.Log(string.Format("Bestie score : {0}", ga.score[poolSize - 1]));
        Destroy(creature);
        Destroy(target);
        Start_Generation();
    }

    void SpawnCreature()
    {
        successCount = 0;
        currentTImer = 0;
        RefreshTarget();
        ga.SetupMachine(agent, creatureID);
        creature = Instantiate(creaturePrefab);
    }

    void RefreshTarget()
    {
        target.transform.position = new Vector3(Random.Range(-15f, 15f), 0, Random.Range(-15f, 15f));
    }

    Matrix<float> GetInputData(GameObject creature, GameObject target)
    {
        CreatureController cc = creature.GetComponent<CreatureController>();
        Vector3 targetDirection = target.transform.position - cc.GetPosition();
        Vector3 rot = cc.GetRotation();
        float[,] data = new float[1, 8];
        data[0, 0] = targetDirection.x;
        data[0, 1] = targetDirection.z;
        data[0, 2] = Mathf.Sin(rot.x);
        data[0, 3] = Mathf.Cos(rot.x);
        data[0, 4] = Mathf.Sin(rot.y);
        data[0, 5] = Mathf.Cos(rot.y);
        data[0, 6] = Mathf.Sin(rot.z);
        data[0, 7] = Mathf.Cos(rot.z);
        Matrix<float> result = new Matrix<float>(data);
        return result;
    }
}
