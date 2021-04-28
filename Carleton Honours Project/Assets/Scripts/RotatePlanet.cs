using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    public float rotationSpeed = 5;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0.0f, rotationSpeed * 0.001f, 0.0f);  //Rotate
    }
}

