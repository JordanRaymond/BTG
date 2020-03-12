using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public float angleToRatate = 90f;
    public float rotateSpeed = 20f;
    public Transform parentTransform;
    private bool isGoingRight = true;
    private float lastAngle = 0;

    private void Start()
    {
    }

    private void Update()
    {
        float currentAngle = Vector3.Angle(transform.forward, parentTransform.forward);

        float deltaAngle = currentAngle - lastAngle;
        float rotation = rotateSpeed * Time.deltaTime;

        if (currentAngle + deltaAngle > angleToRatate) isGoingRight = !isGoingRight;

        if (isGoingRight)
        {
            RotateRadar(rotation);
        }
        else
        {
            RotateRadar(rotation * -1);
        }

        lastAngle = currentAngle;
    }

    private void RotateRadar(float rotation)
    {
        transform.Rotate(transform.up * rotation);
    }
}


//     if (currentAngle != angleToRatateTo || (currentAngle - deltaAngle < angleToRatateTo && currentAngle + deltaAngle > angleToRatateTo))
//     {
//         transform.Rotate(transform.up * deltaAngle * rotateSign);
//     }
//     else
//     {
//         if (angleToRatateTo != 0)
//         {
//             angleToRatateTo = 0;
//             rotateSign *= -1;
//         }
//         else
//         {
//             angleToRatateTo = angleToRatate;
//         }
//     }