using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject canvas;
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + "Game Object Click in Progress");
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        // Set previous menu to active menu
        canvas.GetComponent<CanvasVars>().setPrevActive();
        // Set current menu to inactive
        canvas.GetComponent<CanvasVars>().setCurInActive();
        // Update canvas variables to have previous menu as current menu
        canvas.GetComponent<CanvasVars>().setCurMenu(canvas.GetComponent<CanvasVars>().getPrevMenu());
        // Remove previous menu from queue of previous menus (Because it is now the active menu)
        canvas.GetComponent<CanvasVars>().dequeuePrevMenu();
        Debug.Log(name + "No longer being clicked");
    }
}
