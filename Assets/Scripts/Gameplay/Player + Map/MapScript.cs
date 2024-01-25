using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapScript : MonoBehaviour
{
    public Texture2D texture;
    public GameObject createHex;
    int height;
    int width;
    // Start is called before the first frame update
    void Awake()
    {
        // Get height and width of game map
        height = createHex.GetComponent<CreateHex>().height;
        width = createHex.GetComponent<CreateHex>().width;
        // Create texture with width and height
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

    public void RevealMap(Color[] colours)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                texture.SetPixel(-i, j, colours[(height) * i + j]);
            }
        }
        texture.Apply();
    }


}
