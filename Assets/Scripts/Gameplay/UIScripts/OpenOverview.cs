using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenOverview : MonoBehaviour
{
	//Overview GameObjects
	public GameObject[] overviews;

	public Sprite[] mySprites;

	public Button button;

	void Start()
	{
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{

		foreach (GameObject overview in overviews)
		{
			overview.SetActive(false);
		}
		if (gameObject.name == "Gold")
		{
			ChangeUI(0);
		}
		if (gameObject.name == "Magicka")
		{
			ChangeUI(1);
		}
		if (gameObject.name == "Production")
		{
			ChangeUI(2);
		}
		if (gameObject.name == "Science")
		{
			ChangeUI(3);
		}
		if (gameObject.name == "Influence")
		{
			ChangeUI(4);
		}
		if (gameObject.name == "Festivity")
		{
			ChangeUI(5);
		}
		if (gameObject.name == "Growth")
		{
			ChangeUI(6);
		}
	}

	void ChangeUI(int i)
    {
		overviews[i].SetActive(true);
		GameObject.Find("CreateHex").GetComponent<CreateHex>().infoUIBox = overviews[i];
		Transform myOverview = overviews[i].transform.Find("ScrollArea").transform.Find("Cities").transform;

		for (int j = 0; j < myOverview.childCount; j++)
        {
			//GameObject Go = overviews[i].transform.Find("ScrollArea").transform.Find("Cities").transform.GetChild(j).gameObject as GameObject;
			myOverview.GetChild(j).transform.GetChild(1).GetComponent<Image>().sprite = mySprites[i];
			TextMeshProUGUI yieldText = myOverview.GetChild(j).transform.GetChild(3).GetComponent<TextMeshProUGUI>();
			Classes.City myCity = GameObject.Find("CreateHex").GetComponent<CreateHex>().myPlayer.GetCity(j);
			yieldText.text = myCity.GetYield(i);
		}
	}
}
