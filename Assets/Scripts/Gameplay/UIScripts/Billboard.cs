using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform myCamera;
    Vector3 originalSize;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = GameObject.Find("Main Camera").transform;
        originalSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = transform.position - new Vector3(myCamera.position.x, myCamera.position.y, myCamera.position.z + 1);
        // Rotate the sprite to face the player

        transform.rotation = Quaternion.LookRotation(direction);

    }
}
