using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    public Rigidbody head, body, tail;
    public float power = 1000000f;
    public float travelDistance = 0;
    private Vector3 lastPoint;
    public float renewDistance = 0.1f;

    private void Start()
    {
        travelDistance = 0;
        lastPoint = GetPosition();
    }

    void Update()
    {
        if (Vector3.Distance(GetPosition(), lastPoint) >= renewDistance)
        {
            travelDistance += renewDistance;
            lastPoint = GetPosition();
        }
    }

    public void MoveHead(float input)
    {
        head.AddTorque(input * head.transform.right * Time.deltaTime * power, ForceMode.Acceleration);
    }

    public void MoveTail(float input)
    {
        tail.AddTorque(input * tail.transform.forward * Time.deltaTime * power, ForceMode.Acceleration);
    }

    public Vector3 GetPosition()
    {
        return ((head.position + body.position + tail.position) / 3);

    }

    public Vector3 GetRotation()
    {
        return (head.transform.eulerAngles + body.transform.eulerAngles + tail.transform.eulerAngles) / 3;
    }
}
