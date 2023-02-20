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
    Vector2Int posRounded;
    // Start is called before the first frame update
    void Awake()
    {
        // Get height and width of game map
        height = createHex.GetComponent<CreateHex>().height;
        width = createHex.GetComponent<CreateHex>().width;
        // Create texture with width and height
        texture = new Texture2D(width, height);
        // Make sure texture is pixilated, as we want each tile to be represented
        texture.filterMode = FilterMode.Point;
        texture.anisoLevel = 0;
        // Set texture of map game object to newly created texture
        GetComponent<Image>().material.mainTexture = texture;
    }

    /// <summary>
    /// * Reveals the entire map
    /// </summary>
    /// <param name="colours">
    /// Passed in from CreateHex script, contains an array with all the tiles represented by an appropriate colour
    /// </param>
    public void RevealMap(Color[] colours)
    {
        //Debug.Log(width + ", " + height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                texture.SetPixel(-i, j, colours[height * i + j]);   
            }
        }
        texture.Apply();
    }

    public void setPixel(Vector2Int position)
    {
        // Round position down to integers
        posRounded = new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
        if (posRounded.x < 0)
        {
            posRounded.x = 0;
        }
        if (posRounded.x > height)
        {
            posRounded.x = height;
        }
        if (posRounded.y < 0)
        {
            posRounded.y = 0;
        }
        if (posRounded.y > width)
        {
            posRounded.y = width;
        }
        //texture.SetPixel(-posRounded.y,posRounded.x, Color.white);
        texture.Apply();
    }
}
