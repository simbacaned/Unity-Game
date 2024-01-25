using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineScript : MonoBehaviour
{
    // List of colours          ||Magenta||          ||Cyan||          ||Yellow||            ||Red||           ||Green||             ||Blue||             ||Orange||             ||Yellow||              ||Purple||
    Vector3[] colours = { new Vector3(1, 0, 1), new Vector3(0, 1, 1), new Vector3(1, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 0.5f, 0), new Vector3(0.5f, 0.5f, 0), new Vector3(0.5f, 0, 0.5f) };
    /// <summary>
    /// Draws an outline around the game object's children using the specified color.
    /// </summary>
    /// <param name="leaderNo">The index of the color to use for the outline.</param>
    public void drawOutline(int leaderNo)
    { 
        for (int i = 0; i < transform.childCount; i++) // Loop through all of the children of the game object
        {
            transform.GetChild(i).GetComponent<Outline>().OutlineColor = new Color(colours[leaderNo].x, colours[leaderNo].y, colours[leaderNo].z); // Set the outline color of the current child to the specified color
            transform.GetChild(i).GetComponent<Outline>().OutlineWidth = 7; // Set the outline width of the current child to 7
        }
    }

    /// <summary>
    /// Weakens the outline around the game object's children by decreasing its width and keeping the last outline color.
    /// </summary>
    /// <param name="lastLeaderNo">The index of the last color used for the outline.</param>
    public void weakenOutline(int lastLeaderNo)
    {
        for (int i = 0; i < transform.childCount; i++) // Loop through all of the children of the game object
        {
            transform.GetChild(i).GetComponent<Outline>().OutlineColor = new Color(colours[lastLeaderNo].x, colours[lastLeaderNo].y, colours[lastLeaderNo].z); // Set the outline color of the current child to the last used color
            transform.GetChild(i).GetComponent<Outline>().OutlineWidth = 3.5f; // Decrease the outline width of the current child to 3.5f
        }
    }

    // <summary>
    /// Returns a color vector based on the given index.
    /// </summary>
    /// <param name="colourNo">The index of the color to retrieve.</param>
    /// <returns>The color vector at the given index.</returns>
    public Vector3 GetColour(int colourNo)
    {
        return colours[colourNo];
    }
}
