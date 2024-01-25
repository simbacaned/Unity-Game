using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetCurrentMap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject[] mySizeButtons;
    private void Start()
    {
        if (name == "Small" && PlayerPrefs.GetInt("mapSize") == 0)
        {
            mySizeButtons[0].GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
        }
        if (name == "Medium" && PlayerPrefs.GetInt("mapSize") == 1)
        {
            mySizeButtons[1].GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
        }
        if (name == "Large" && PlayerPrefs.GetInt("mapSize") == 2)
        {
            mySizeButtons[2].GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
        }
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + " Game Object Click in Progress");
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log(name + " No longer being clicked");
        foreach(GameObject size in mySizeButtons)
        {
            size.GetComponent<Image>().color = new Color(0, 0, 0, 0.70f);
        }
        GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
        if (name == "Small")
        {
            PlayerPrefs.SetInt("mapSize", 0);
        }
        if (name == "Medium")
        {
            PlayerPrefs.SetInt("mapSize", 1);
        }
        if (name == "Large")
        {
            PlayerPrefs.SetInt("mapSize", 2);
        }
    }
}
