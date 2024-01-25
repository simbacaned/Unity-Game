using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TileScript : MonoBehaviour
{
    Transform tileCap;
    public List<int> connections;
    public int index;
    public Sprite[] mySprites;
    private void Awake()
    {
        connections = new List<int>();
        tileCap = transform.GetChild(0);
        tileCap.gameObject.SetActive(false);
    }

    public void SetCap(bool boolean)
    {
        tileCap.gameObject.SetActive(boolean);
    }
    public void SetIndex(int _index)
    {
        index = _index;
    }
    public int GetIndex()
    {
        return index;
    }
    public List<int> GetConnections()
    {
        return connections;
    }
    public void SetConnection(int myIndex)
    {
        connections.Add(myIndex);
    }
    public void SetCapCol(int sprite)
    {
        tileCap.GetComponent<SpriteRenderer>().sprite = mySprites[sprite];
    }
    void OnMouseUp()
    {
        if (!GameObject.Find("Main Camera").GetComponent<PlayerMovement>().isDragging1)
        {
            GameObject.Find("CreateHex").GetComponent<CreateHex>().ClickOnTile(index);
        }
    }
}
