using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickYieldIcon : MonoBehaviour
{
    //Yield GameObjects
    public GameObject[] yieldObjects;

	public Button button;

	void Start()
	{
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		foreach(GameObject yield in yieldObjects)
        {
			yield.SetActive(false);
		}
	}

	void TaskOnClick()
	{
		foreach (GameObject yield in yieldObjects)
		{
			yield.SetActive(false);
		}
		if (gameObject.name == "Gold")
		{
			yieldObjects[0].SetActive(true);
		}
		if (gameObject.name == "Magicka")
		{
			yieldObjects[1].SetActive(true);
		}
		if (gameObject.name == "Production")
		{
			yieldObjects[2].SetActive(true);
		}
		if (gameObject.name == "Science")
		{
			yieldObjects[3].SetActive(true);
		}
		if (gameObject.name == "Influence")
		{
			yieldObjects[4].SetActive(true);
		}
		if (gameObject.name == "Festivity")
		{
			yieldObjects[5].SetActive(true);
		}
		if (gameObject.name == "Growth")
		{
			yieldObjects[6].SetActive(true);
		}
	}
}
