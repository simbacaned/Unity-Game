using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class CurrentMap : MonoBehaviour
{
    public List<Sprite> myMapSprites;
    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().sprite = myMapSprites[PlayerPrefs.GetInt("mapSize")];
    }
}
