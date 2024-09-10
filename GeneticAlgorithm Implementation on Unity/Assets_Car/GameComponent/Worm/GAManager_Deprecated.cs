using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LearningModel;
using LearningModel.Base;
using Matrices;
using Unity.VisualScripting;

public class GAManager_Deprecated : MonoBehaviour
{
    public GameObject creaturePrefab;
    public GameObject creature;
    private Transform head, body, tail;

    public Transform target;
    public float successMargin = 3f;

    public float limitTimer = 30f;
    public float currentTImer = 0f;

    const int poolSize = 20;

    public int poolIndex = 0;
    LearningModel<float>[] pool = new LearningModel<float>[poolSize];
    LearningModel<float> lm = null;
    public float[] score = new float[poolSize];

    public int successCountThreshold = 5;
    public int successCount = 0;

    public int Generation = 0;

    void Start()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = new LearningModel<float>(new int[] { 8, 10, 8, 10, 2 }, new string[] { "relu", "relu", "relu", "relu", "relu" }, "MSE");
            pool[i].Initialize_Silent();
        }
        PoolNext();
    }

    void Update()
    {
        if (currentTImer < limitTimer)
        {
            currentTImer += Time.deltaTime;
        }
        else
        {
            Failed();
        }

        Matrix<float> movement = lm.GetPredict(GetInputData());
        MoveCreature(movement.GetMatrix()[0, 0], movement.GetMatrix()[0, 1]);
        //MoveCreature(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        if (Vector3.Distance(GetCreaturePosition(), target.position) < successMargin)
        {
            if (successCount >= successCountThreshold)
            {
                Success();
            }
            else
            {
                successCount += 1;
                RefreshTarget();
            }
        }
        if (GetCreaturePosition().y < -5f)
        {
            Failed();
        }
    }

    void PoolNext()
    {
        if (poolIndex < pool.Length)
        {
            Debug.Log(string.Format("Testing {0} from pool", poolIndex));
            lm = pool[poolIndex];
            RefreshTarget();
            if (creature != null)
            {
                currentTImer = 0;
                Destroy(creature);
            }
            SpawnCreature();
            poolIndex += 1;
        }
        else
        {
            poolIndex = 0;
            Debug.Log("Generation Test Done");
            GenEval();
        }
    }

    void GenEval()
    {
        System.Array.Sort(score, pool);
        System.Array.Reverse(score);
        System.Array.Reverse(pool);
        for (int i = pool.Length / 2; i < pool.Length; i++)
        {
            int mother = Random.Range(0, pool.Length / 2 - 1);
            int father = Random.Range(0, pool.Length / 2 - 1);
            for (int k = 0; k < pool[i].weights.Length; k++)
            {
                pool[i].weights[k] = (k % 2 == 0) ? pool[mother].weights[k] : pool[father].weights[k];
            }
            for (int k = 0; k < pool[i].biases.Length; k++)
            {
                pool[i].biases[k] = (k % 2 == 0) ? pool[mother].biases[k] : pool[father].biases[k];
            }
        }
        Generation += 1;
    }

    Matrix<float> GetInputData()
    {
        float[,] data = new float[1, 8];
        data[0, 0] = (target.position - GetCreaturePosition()).x;
        data[0, 1] = (target.position - GetCreaturePosition()).z;
        data[0, 2] = Mathf.Sin(GetCreatureRotation().x);
        data[0, 3] = Mathf.Cos(GetCreatureRotation().x);
        data[0, 4] = Mathf.Sin(GetCreatureRotation().y);
        data[0, 5] = Mathf.Cos(GetCreatureRotation().y);
        data[0, 6] = Mathf.Sin(GetCreatureRotation().z);
        data[0, 7] = Mathf.Cos(GetCreatureRotation().z);
        Matrix<float> result = new Matrix<float>(data);
        return result;
    }

    void SpawnCreature()
    {
        creature = Instantiate(creaturePrefab);
        head = creature.transform.GetChild(0);
        body = creature.transform.GetChild(1);
        tail = creature.transform.GetChild(2);
    }

    Vector3 GetCreaturePosition()
    {
        return ((head.position + body.position + tail.position) / 3);
    }

    Vector3 GetCreatureRotation()
    {
        return (head.eulerAngles + body.eulerAngles + tail.eulerAngles) / 3;
    }    

    void MoveCreature(float hi, float ti)
    {
        creature.gameObject.GetComponent<CreatureController>().MoveHead(hi);
        creature.gameObject.GetComponent<CreatureController>().MoveTail(ti);
    }

    void RefreshTarget()
    {
        target.position = new Vector3(Random.Range(-15f, 15f), 0, Random.Range(-15f, 15f));
    }

    void Success()
    {
        Debug.Log("GA Success");
    }

    void Failed()
    {
        if (poolIndex != 0)
        {
            score[poolIndex - 1]
        = -(Vector3.Distance(target.position, GetCreaturePosition()) / 22f)
        + successCount
        - 1 * (GetCreaturePosition().y < 0 ? 1 : 0);
            successCount = 0;
        }
        PoolNext();
    }
}
