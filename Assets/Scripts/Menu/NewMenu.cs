using UnityEngine;
using UnityEngine.EventSystems;

public class NewMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject targetMenu;
    public GameObject curMenu;
    public GameObject canvas;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + " Game Object Click in Progress");
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        switchToPrevMenu();
        Debug.Log(name + " No longer being clicked");
    }
    void switchToPrevMenu()
    {
        canvas.GetComponent<CanvasVars>().setCurMenu(targetMenu);
        canvas.GetComponent<CanvasVars>().setPrevMenu(curMenu);
        targetMenu.SetActive(true);
        curMenu.SetActive(false);
    }
}
