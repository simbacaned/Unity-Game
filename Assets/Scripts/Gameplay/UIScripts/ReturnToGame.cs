using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReturnToGame : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject cameraObj;
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + "Game Object Click in Progress");
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        cameraObj.GetComponent<PlayerMovement>().flipMenu();
        Debug.Log(name + "No longer being clicked");
    }
}
