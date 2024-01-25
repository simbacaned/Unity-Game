using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawCities : MonoBehaviour
{
    public Texture2D texture;
    int height;
    int width;
    // Start is called before the first frame update
    void Start()
    {
        // Get height and width of game map
        height = GameObject.Find("CreateHex").GetComponent<CreateHex>().height;
        width = GameObject.Find("CreateHex").GetComponent<CreateHex>().width;
        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        // Make sure texture is pixilated, as we want each tile to be represented
        texture.filterMode = FilterMode.Point;
        texture.anisoLevel = 0;
        GetComponent<Image>().material.mainTexture = texture;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                texture.SetPixel(-i, j, Color.clear);
            }
        }
        texture.Apply();
    }

    public void Draw(int hexID)
    {
        int i = 0;
        int storeHexID = hexID;
        while(storeHexID - (width / 2) > 0)
        {
            i += 1;
            storeHexID -= (width / 2);
        }
        texture.SetPixel((-storeHexID * 2) + width - 10, (i * 2) + 10, Color.black);

        texture.Apply();
    }
}
