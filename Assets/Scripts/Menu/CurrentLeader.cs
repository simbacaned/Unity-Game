using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLeader : MonoBehaviour
{
    public List<Sprite> myLeaderSprites;
    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().sprite = myLeaderSprites[PlayerPrefs.GetInt("leader")];
    }
}
