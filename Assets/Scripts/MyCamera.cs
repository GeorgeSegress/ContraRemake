using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public Transform myTarget;
    public float camSpeed = 0.02f;
    public float maxX;
    private float myY;
    private float myZ;

    private void Start()
    {
        myY = transform.position.y;
        myZ = transform.position.z;
    }

    private void Update()
    {
        if (myTarget != null)
        {
            float x = Mathf.Lerp(transform.position.x, myTarget.position.x, camSpeed);
            if (x > maxX) x = maxX;
            transform.position = new Vector3(x, myY, myZ);
        }
    }
}
