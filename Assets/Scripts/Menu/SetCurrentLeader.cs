using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetCurrentLeader : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject[] myLeaderButtons;
    private void Start()
    {
        if (name == "Befana" && PlayerPrefs.GetInt("leader") == 0)
        {
            myLeaderButtons[0].GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
        }
        if (name == "Circe" && PlayerPrefs.GetInt("leader") == 1)
        {
            myLeaderButtons[1].GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
        }
        if (name == "Dedi" && PlayerPrefs.GetInt("leader") == 2)
        {
            myLeaderButtons[2].GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
        }
        if (name == "Grimhildr" && PlayerPrefs.GetInt("leader") == 3)
        {
            myLeaderButtons[3].GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
        }
        if (name == "Gwydion" && PlayerPrefs.GetInt("leader") == 4)
        {
            myLeaderButtons[4].GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
        }
        if (name == "Merlin" && PlayerPrefs.GetInt("leader") == 5)
        {
            myLeaderButtons[5].GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
        }
        if (name == "Titania" && PlayerPrefs.GetInt("leader") == 6)
        {
            myLeaderButtons[6].GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
        }
        if (name == "Zarqa' Al-Yamama" && PlayerPrefs.GetInt("leader") == 7)
        {
            myLeaderButtons[7].GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
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
        foreach(GameObject leader in myLeaderButtons)
        {
            leader.GetComponent<Image>().color = new Color(0, 0, 0, 0.70f);
        }
        GetComponent<Image>().color = new Color(0.68f, 1.00f, 0.62f, 0.70f);
        if (name == "Befana")
        {
            PlayerPrefs.SetInt("leader", 0);
        }
        if (name == "Circe")
        {
            PlayerPrefs.SetInt("leader", 1);
        }
        if (name == "Dedi")
        {
            PlayerPrefs.SetInt("leader", 2);
        }
        if (name == "Grimhildr")
        {
            PlayerPrefs.SetInt("leader", 3);
        }
        if (name == "Gwydion")
        {
            PlayerPrefs.SetInt("leader", 4);
        }
        if (name == "Merlin")
        {
            PlayerPrefs.SetInt("leader", 5);
        }
        if (name == "Titania")
        {
            PlayerPrefs.SetInt("leader", 6);
        }
        if (name == "Zarqa' Al-Yamama")
        {
            PlayerPrefs.SetInt("leader", 7);
        }
    }
}
