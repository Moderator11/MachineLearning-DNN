using LearningModel;
using MachineLearning;
using Matrices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleratorPower = 100f;
    public float steeringPower = 20f;
    Rigidbody rb;

    public float travelDistance = 0;
    private Vector3 lastPoint;
    public float renewDistance = 0.1f;

    public bool startTest = false;
    public bool wallHit = false;

    public bool testDone = false;
    public float score = 0;

    public float acc, str;

    public LearningModel<float> agent;

    public float limitTimer = 15f;
    public float currentTImer = 0f;
    public float velThreshold = 1f;
    public void ControlCar(float acc, float str)
    {
        this.acc = acc;
        this.str = str;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //ControlCar(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        if (rb.velocity.magnitude < velThreshold)
        {
            if (currentTImer < limitTimer)
            {
                currentTImer += Time.deltaTime;
            }
            else
            {
                wallHit = true;
                EndTest();
            }
        }
        if (startTest == true)
        {
            if (wallHit == false)
            {
                Vector3[] directions = new Vector3[8];
                RaycastHit[] hits = new RaycastHit[8];
                for (int i = 0; i < directions.Length; i++)
                {
                    directions[i] = Quaternion.AngleAxis(45 * i, Vector3.up) * transform.forward;
                    if (Physics.Raycast(transform.position, directions[i], out hits[i]))
                    {
                        Debug.DrawRay(transform.position, directions[i] * hits[i].distance, Color.red);
                    }
                }

                float[,] o = agent.GetPredict(GetInputData(hits)).GetMatrix();
                acc = o[0, 0];
                str = o[0, 1];

                rb.AddForce(transform.forward * acc * acceleratorPower * Time.deltaTime, ForceMode.Acceleration);
                rb.AddTorque(transform.up * str * steeringPower * Time.deltaTime, ForceMode.Acceleration);
                if (Vector3.Distance(transform.position, lastPoint) >= renewDistance)
                {
                    travelDistance += renewDistance;
                    lastPoint = transform.position;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Floor")
        {
            wallHit = true;
            EndTest();
        }
    }

    void EndTest()
    {
        score = travelDistance;
        testDone = true;
    }

    Matrix<float> GetInputData(RaycastHit[] datas)
    {
        float[,] data = new float[1, datas.Length + 4];
        for (int i = 0; i < datas.Length; i++)
        {
            data[0, i] = datas[i].distance;
        }
        data[0, datas.Length] = Mathf.Sin(transform.eulerAngles.y / (MathF.PI * 2));
        data[0, datas.Length + 1] = Mathf.Cos(transform.eulerAngles.y / (MathF.PI * 2));
        data[0, datas.Length + 2] = rb.velocity.x;
        data[0, datas.Length + 3] = rb.velocity.z;
        Matrix<float> result = new Matrix<float>(data);
        return result;
    }
}
