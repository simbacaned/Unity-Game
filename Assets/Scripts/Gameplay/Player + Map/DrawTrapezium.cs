using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawTrapezium : MonoBehaviour
{
    Vector2Int prevPosition;
    int prevSize;
    public Texture2D texture;
    int height;
    int width;
    // Start is called before the first frame update
    void Start()
    {
        // Get height and width of game map
        height = GameObject.Find("CreateHex").GetComponent<CreateHex>().height * 2;
        width = GameObject.Find("CreateHex").GetComponent<CreateHex>().width * 2;
        prevPosition = new Vector2Int();
        prevSize = 0;
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

    public void Draw(Vector2Int currentPosition, int size)
    {
        currentPosition.x += 0;
        for (int i = 0; i <= (3 * prevSize / 2) + prevSize; i++)
        {
            for (int j = 0; j < prevSize; j++)
            {
                int goForX = -prevPosition.y - j + i - (3 * prevSize) / 2;
                int goForY = 10 + prevPosition.x + j / 2 - prevSize / 2;
                if (-goForX > 0 && -goForX < width && goForY > 0 && goForY < height)
                {
                    texture.SetPixel(goForX, goForY, Color.clear);
                }
            }
        }
        for (int i = (3 * prevSize / 2) - prevSize; i < 3 * prevSize; i++)
        {
            for (int j = 0; j < prevSize; j++)
            {
                int goForX = -prevPosition.y + j + i - (3 * prevSize) / 2;
                int goForY = 10 + prevPosition.x + j / 2 - prevSize / 2;
                if (-goForX > 0 && -goForX < width && goForY > 0 && goForY < height)
                {
                    texture.SetPixel(goForX, goForY, Color.clear);
                }
            }
        }
        for (int i = 0; i <= (3 * size / 2) + size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i < 7 || i > (3 * size) - 8 || j < 3 || j > size - 4)
                {
                    int goForX = -currentPosition.y - j + i - (3 * size) / 2;
                    int goForY = 10 + currentPosition.x + j / 2 - size / 2;
                    if (-goForX > 0 && -goForX < width && goForY > 0 && goForY < height)
                    {
                        texture.SetPixel(goForX, goForY, Color.white);
                    }
                }
            }
        }
        for (int i = (3 * size / 2) - size; i < 3 * size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i < 7 || i > (3 * size) - 8 || j < 3 || j > size - 4)
                {
                    int goForX = -currentPosition.y + j + i - (3 * size) / 2;
                    int goForY = 10 + currentPosition.x + j / 2 - size / 2;
                    if (-goForX > 0 && -goForX < width && goForY > 0 && goForY < height)
                    {
                        texture.SetPixel(goForX, goForY, Color.white);
                    }
                }
            }
        }
        prevPosition = currentPosition;
        texture.Apply();
        prevPosition = currentPosition;
        prevSize = size;
    }
}
