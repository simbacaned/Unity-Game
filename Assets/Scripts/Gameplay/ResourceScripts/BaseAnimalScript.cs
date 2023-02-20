using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimalScript : MonoBehaviour
{
    private void Start()
    {

    }
    private void Update()
    {
        // rotate animals clockwise 
        transform.Rotate(0, -0.025f * (Mathf.Cos(Time.deltaTime * 4)+1), 0);
    }
}
