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
    public int scalar;
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
        currentPosition = new Vector2Int(currentPosition.x + size + 10, -currentPosition.y - 5);
        size *= 3;
        // Clear trapezoid
        for (int i = 0; i < prevSize; i++)
        {
            for (int j = 0; j < prevSize; j++)
            {
                if (
                  j > i / 3 &&
                  j < prevSize - i / 3 &&
                  i > prevSize / 2 &&
                  i < prevSize
                )
                {
                    texture.SetPixel(j + prevPosition.y - prevSize / 2, -i + prevPosition.x + prevSize / 2, Color.clear);
                }
            }
        }

        // Draw trapezoid
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (
                  j > i / 3 &&
                  j < size - i / 3 &&
                  i > size / 2 &&
                  i < size
                )
                {
                    texture.SetPixel(j + currentPosition.y - size / 2, -i + currentPosition.x + size / 2, Color.white);
                }
            }
        }
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (
                  j - 3 > i / 3 &&
                  j + 3 < size - i / 3 &&
                  i - 3 > size / 2 &&
                  i + 3 < size
                      )
                {
                    texture.SetPixel(j + currentPosition.y - size / 2, -i + currentPosition.x + size / 2, Color.clear);
                }
            }
        }


        //while (currentPosition.x < 0)
        //{
        //    currentPosition.x += height;
        //}
        //while (currentPosition.y < 0)
        //{
        //    currentPosition.y += width;
        //}
        //while (currentPosition.x >= height)
        //{
        //    currentPosition.x -= height;
        //}
        //while (currentPosition.y >= width)
        //{
        //    currentPosition.y -= width;
        //}
        //for (int i = 0; i <= (3 * prevSize / 2) + prevSize; i++)
        //{
        //    for (int j = 0; j < prevSize; j++)
        //    {
        //        int goForX = -prevPosition.y - j + i - (3 * prevSize) / 2;
        //        int goForY = (prevSize / 2) - 6 + (prevPosition.x + j / 2 - prevSize / 2);
        //        if (-goForX > 0 && -goForX < width && goForY >= 0 && goForY < height)
        //        {
        //            texture.SetPixel(goForX, goForY, Color.clear);
        //        }
        //    }
        //}
        //for (int i = (3 * prevSize / 2) - prevSize; i < 3 * prevSize; i++)
        //{
        //    for (int j = 0; j < prevSize; j++)
        //    {
        //        int goForX = -prevPosition.y + j + i - (3 * prevSize) / 2;
        //        int goForY = (prevSize / 2) - 6 + (prevPosition.x + j / 2 - prevSize / 2);
        //        if (-goForX > 0 && -goForX < width && goForY >= 0 && goForY < height)
        //        {
        //            texture.SetPixel(goForX, goForY, Color.clear);
        //        }
        //    }
        //}
        //for (int i = 0; i <= (3 * size / 2) + size; i++)
        //{
        //    for (int j = 0; j < size; j++)
        //    {
        //        if (i < 7 || i > (3 * size) - 8 || j < 3 || j > size - 4)
        //        {
        //            int goForX = -currentPosition.y - j + i - (3 * size) / 2;
        //            int goForY = (size / 2) - 6 + (currentPosition.x + j / 2 - size / 2);
        //            if (-goForX > 0 && -goForX < width && goForY >= 0 && goForY < height)
        //            {
        //                texture.SetPixel(goForX, goForY, Color.white);
        //            }
        //        }
        //    }
        //}
        //for (int i = (3 * size / 2) - size; i < 3 * size; i++)
        //{
        //    for (int j = 0; j < size; j++)
        //    {
        //        if (i < 7 || i > (3 * size) - 8 || j < 3 || j > size - 4)
        //        {
        //            int goForX = -currentPosition.y + j + i - (3 * size) / 2;
        //            int goForY = (size / 2) - 6 + (currentPosition.x + j / 2 - size / 2);
        //            if (-goForX > 0 && -goForX < width && goForY >= 0 && goForY < height)
        //            {
        //                texture.SetPixel(goForX, goForY, Color.white);
        //            }
        //        }
        //    }
        //}

        prevPosition = currentPosition;
        prevSize = size;
        texture.Apply();
    }
}
