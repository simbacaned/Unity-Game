using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasVars : MonoBehaviour
{
    GameObject currentMenu;
    public Stack<GameObject> previousMenus;

    public void Start()
    {
        previousMenus = new Stack<GameObject>();
    }
    public void setPrevActive()
    {
        previousMenus.Peek().SetActive(true);
    }
    public void setPrevInActive()
    {
        previousMenus.Peek().SetActive(false);
    }
    public void setPrevMenu(GameObject prevMenu)
    {
        previousMenus.Push(prevMenu);
    }
    public void dequeuePrevMenu()
    {
        previousMenus.Pop();
    }
    public GameObject getPrevMenu()
    {
        return previousMenus.Peek();
    }

    public void setCurActive()
    {
        currentMenu.SetActive(true);
    }
    public void setCurInActive()
    {
        currentMenu.SetActive(false);
    }
    public void setCurMenu(GameObject CurMenu)
    {
        currentMenu = CurMenu;
    }

    public GameObject getCurMenu()
    {
        return currentMenu;
    }

    public void Update()
    {
        //Debug.Log(previousMenus.Count);
    }
}
