using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickYieldIcon : MonoBehaviour
{
    //Yield GameObjects
    public GameObject[] yieldObjects;

	public Button button;
	public int lastClickedButton;

	void Start()
	{
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		foreach(GameObject yield in yieldObjects)
        {
			yield.SetActive(false);
		}
	}

	public void TaskOnClick()
	{
		foreach (GameObject yield in yieldObjects)
		{
			yield.SetActive(false);
		}
		if (gameObject.name == "Gold")
		{
			yieldObjects[0].SetActive(true);
			GameObject.Find("CreateHex").GetComponent<CreateHex>().lastClickedButtonNo = 0;
		}
		if (gameObject.name == "Magicka")
		{
			yieldObjects[1].SetActive(true);
			GameObject.Find("CreateHex").GetComponent<CreateHex>().lastClickedButtonNo = 1;
		}
		if (gameObject.name == "Production")
		{
			yieldObjects[2].SetActive(true);
			GameObject.Find("CreateHex").GetComponent<CreateHex>().lastClickedButtonNo = 2;
		}
		if (gameObject.name == "Science")
		{
			yieldObjects[3].SetActive(true);
			GameObject.Find("CreateHex").GetComponent<CreateHex>().lastClickedButtonNo = 3;
		}
		if (gameObject.name == "Influence")
		{
			yieldObjects[4].SetActive(true);
			GameObject.Find("CreateHex").GetComponent<CreateHex>().lastClickedButtonNo = 4;
		}
		if (gameObject.name == "Festivity")
		{
			yieldObjects[5].SetActive(true);
			GameObject.Find("CreateHex").GetComponent<CreateHex>().lastClickedButtonNo = 5;
		}
		if (gameObject.name == "Growth")
		{
			yieldObjects[6].SetActive(true);
			GameObject.Find("CreateHex").GetComponent<CreateHex>().lastClickedButtonNo = 6;
		}
	}
}
