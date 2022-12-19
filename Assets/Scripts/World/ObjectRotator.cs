using UnityEngine;
using System.Collections;

public class ObjectRotator : MonoBehaviour
{
    public Transform rotateAroundTransform;
    public float rotateAroundSpeed = 0.1f;
    public float rotateAxisSpeed = 3f;

    void Update()
    {
        transform.Rotate(0, rotateAxisSpeed * Time.deltaTime, 0, Space.Self);
        if (rotateAroundTransform)
            transform.RotateAround(rotateAroundTransform.position, Vector3.forward, rotateAroundSpeed * Time.deltaTime);
    }
}

           
	