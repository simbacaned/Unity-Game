using UnityEngine;
using System.IO;

public class Properties
{
    public int width = 768;
    public int height = 108;
    public string[] lines;

    public Properties()
    {
        string filePath = "C:\\Users\\Joe\\Documents\\CityBuildergame\\CityBuilderGame\\Assets\\Scripts\\Gameplay\\cityNames.txt";
        lines = File.ReadAllLines(filePath);
    }

    public string[] GetNames()
    {
        return lines;
    }
}
