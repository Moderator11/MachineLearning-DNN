using LearningModel;
using MachineLearning;
using Matrices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TestInstance : MonoBehaviour
{
    public GameObject creature;
    public GameObject target;
    public LearningModel<float> agent;
    
    public float limitTimer = 15f;
    public float currentTImer = 0f;
    public float successMargin = 3f;
    public int successCountThreshold = 5;
    public int successCount = 0;

    public bool startTest = false;
    public bool testDone = false;
    public float score = 0;

    void Start()
    {
        RefreshTarget();
        //SetupAgent();
    }

    void Update()
    {
        if (startTest == true)
        {
            MoveCreature();
            CheckStatus();
        }
    }

    public void SetupAgent()
    {
        agent = new LearningModel<float>(new int[] { 8, 10, 10, 2 }, new string[] { "leakyrelu", "leakyrelu", "leakyrelu", "leakyrelu" }, "MSE");
        agent.Initialize_Silent();
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
            EndTest();
        }
        else if (creature.GetComponent<CreatureController>().GetPosition().y < -5f)
        {
            EndTest(true);
        }
        else if (currentTImer < limitTimer)
        {
            currentTImer += Time.deltaTime;
        }
        else
        {
            EndTest();
        }
    }

    void RefreshTarget()
    {
        target.transform.position = new Vector3(Random.Range(-15f, 15f), 0, Random.Range(-15f, 15f));
    }

    void EndTest(bool fall = false)
    {
        score
            = (50 - Vector3.Distance(creature.GetComponent<CreatureController>().GetPosition(), target.transform.position)) / 10;
            //+ ((limitTimer - currentTImer) / limitTimer)
            //+ creature.GetComponent<CreatureController>().travelDistance / 50
            //+ (fall ? 0 : 1);
        if (score < 0) { score = 0; }
        testDone = true;
        startTest = false;
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
