using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseResourceScript : MonoBehaviour
{
    GameObject myCamera;
    Vector3 cameraPosition;
    Vector3 myPosition;
    private void Start()
    {
        myCamera = GameObject.Find("Main Camera");
        myPosition = transform.position;
    }

}
