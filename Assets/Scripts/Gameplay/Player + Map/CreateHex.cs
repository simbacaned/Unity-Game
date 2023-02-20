using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum hexType { DeepWater, Water, Sand, GrassLand, Snow, Mountains };
public enum resourceType { None, Cow, Sheep, Horse, Iron, Coal }
public struct Hex
{
    public hexType hexTexture;
    public resourceType hexResource;
    public bool isForest;
    public GameObject hexObj;
    public GameObject resourceObject;
    public GameObject currentPassiveUnit;
    public GameObject currentMilitaryUnit;
    public GameObject currentTileBuilding;
    public GameObject forestTile;
    public Vector2 XY;
    public Vector3 pos;
};

public class CreateHex : MonoBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject[] myMountains;
    public GameObject[] mySnow;
    public GameObject[] mySand;
    public GameObject[] myGrassland;
    public GameObject[] myForest;
    public GameObject[] myWater;
    public GameObject[] myPassiveUnits;
    public GameObject[] myInfoUIBoxes;
    public GameObject[] myYieldDisplays;
    public GameObject infoUIBox;
    public GameObject myBaseImage;
    public GameObject mySheep;
    public GameObject myHorse;
    public GameObject myIron;
    public GameObject myCoal;

    // GameObject to store map resources (e.g. sheep, coal, iron ect...)
    public GameObject mapResources;
    public GameObject mapUnits;
    public GameObject mapShrubbery;

    public GameObject myCamera;

    // Floats for perlin noise
    int scale;
    float xCoord;
    float yCoord;
    float xFirstYPos;

    // Every other row of hexagons
    float xCoordPlusHalf;
    float yCoordPlusHalf;

    // Height and Width of map
    public int width;
    public int height;
    // Z component of map
    int mapHeight;

    // Map 
    public GameObject map;

    // 2d vector for map
    Vector2Int vector2i;

    // Quaternion to store the rotation of current hex game object
    public Quaternion rotation;
    public List<Hex> hexs;
    // Temp hex to store hex 
    Hex tempHex;

    // Colours to store for map
    public Color[] pixels;

    // Colour to draw map
    Color colour;

    // Chosen hex texture
    GameObject chosenHex;

    //Properties
    Properties myProperties;

    //Player
    public Classes.Player myPlayer;

    float unit = 1.0f / 1.2f;

    private void Awake()
    {
        myProperties = new Properties();
        width = myProperties.width;
        height = myProperties.height;
        myPlayer = new Classes.Player();
        myPlayer.setHuman();
        myPlayer.AddGold(100);
        settleCity("London");
        Classes.City newCity;
        settleCity("Paris");
        settleCity("Amsterdam");
        newCity = myPlayer.GetCity(2);
        newCity.AddGold(1);
        newCity.AddFestivity(3);
        newCity.AddMagicka(23);
        newCity.AddGrowth(2);
        settleCity("Berlin");
        settleCity("Washington");
        settleCity("Tunis");
        settleCity("Brasília");
        settleCity("Tokyo");
    }
    float lowestVal = 100;

    // Start is called before the first frame update
    void Start()
    {
        tempHex.hexResource = resourceType.None;
        mapHeight = 1;
        scale = 10;
        hexs = new List<Hex>();
        pixels = new Texture2D(width / 2, height * 2).GetPixels();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                rotation = Quaternion.Euler(0, 0, 0);
                xCoord = 2 * (float)i / width * scale;
                xCoordPlusHalf =  2 * (float)(0.5f + i) / width * scale;
                yCoordPlusHalf = (float)(0.5f + j) / height * 2 * scale;
                yCoord = (float)(j) / height * 2 * scale;

                //Creating Tiles
                CreateTiles(i, j);

                // Creating Resources
                CreateResources(i,j);

                if (tempHex.hexTexture == hexType.Sand)
                {
                    if(tempHex.pos.y < lowestVal)
                    {
                        lowestVal = tempHex.pos.y;
                    }
                }   
            }
        }
        Debug.Log("Lowest val" + lowestVal);
        map.GetComponent<MapScript>().RevealMap(pixels);
    }

    void CreateTiles(int i, int j)
    {
        if (i % 2 == 0)
        {
            xFirstYPos = Mathf.PerlinNoise(xCoord, yCoord);
        }
        else
        {
            xFirstYPos = Mathf.PerlinNoise(xCoordPlusHalf, yCoordPlusHalf);
        }
        // Mountains
        if (xFirstYPos > 0.9f * mapHeight)
        {
            // Rotate mountain
            rotation = Quaternion.Euler(0, 60 * (int)Random.Range(0f, 6f), 0);
            tempHex.hexTexture = hexType.Mountains;
            // Choose 1 of 5 mountain models randomly
            chosenHex = myMountains[Random.Range(0, 4)];
            colour.r = 0.5343f;
            colour.g = 0.7068f;
            colour.b = 0.4362f;
        }
        // Snow
        else if (xFirstYPos > 0.75f )
        {
            tempHex.hexTexture = hexType.Snow;
            chosenHex = mySnow[0];
            colour.r = 0.5108f;
            colour.g = 0.6872f;
            colour.b = 0.4205f;
        }
        // Grassland
        else if (xFirstYPos > 0.5f )
        {
            tempHex.hexTexture = hexType.GrassLand;
            chosenHex = myGrassland[0];
            colour.r = 0.4912f;
            colour.g = 0.6676f;
            colour.b = 0.3931f;
        }
        // Sand
        else if (xFirstYPos > 0.45f )
        {
            tempHex.hexTexture = hexType.Sand;
            chosenHex = mySand[0];
            colour.r = 0.4716f;
            colour.g = 0.6480f;
            colour.b = 0.3696f;
        }
        // Shallow water
        else if (xFirstYPos > 0.3f)
        {
            tempHex.hexTexture = hexType.Water;
            chosenHex = myWater[0];
            xFirstYPos = 1.5f * mapHeight;
            colour.r = 0.5315f;
            colour.g = 0.7082f;
            colour.b = 0.7435f;
        }
        // Deep water
        else
        {
            tempHex.hexTexture = hexType.DeepWater;
            chosenHex = myWater[1];
            xFirstYPos = 1.5f * mapHeight;
            colour.r = 0.4490f;
            colour.g = 0.5941f;
            colour.b = 0.6960f;
        }

        xFirstYPos *= 3.3333f * mapHeight;
        vector2i.Set(i, j);
        colour.a = 1;
        pixels[(height * i) + j] = colour;

        if (i % 2 == 0)
        {
            tempHex.pos = new Vector3(2 * j * Mathf.Sqrt(Mathf.Pow(2 * unit, 2) - Mathf.Pow(unit, 2)), xFirstYPos, i * unit);
        }
        else
        {
            tempHex.pos = new Vector3((1 + (2 * j)) * Mathf.Sqrt(Mathf.Pow(2 * unit, 2) - Mathf.Pow(unit, 2)), xFirstYPos, i * unit);
        }

        if (tempHex.hexTexture == hexType.DeepWater || tempHex.hexTexture == hexType.Water)
        {
            tempHex.pos = new Vector3(tempHex.pos.x, 1.15f, tempHex.pos.z);
        }
        if (tempHex.hexTexture == hexType.Mountains)
        {
            tempHex.pos = new Vector3(tempHex.pos.x, tempHex.pos.y + (chosenHex.GetComponent<BoxCollider>().size.y * 25), tempHex.pos.z);
        }

        tempHex.hexObj = Instantiate(chosenHex, tempHex.pos, rotation) as GameObject;

    }

    public bool CloseInfoMenu()
    {
        if (infoUIBox != null)
        {
            infoUIBox.SetActive(false);
            infoUIBox = null;
            return true;
        }
        return false;
    }

    void CreateResources(int i, int j)
    {
        if (Random.Range(0, 35) == 1)
        {
            int randomInt = Random.Range(0, 4);
            if (tempHex.hexTexture == hexType.GrassLand)
            {
                //Sheep
                if (randomInt == 1)
                {
                    //Debug.Log("CreatingSheep");
                    tempHex.hexResource = resourceType.Sheep;
                    tempHex.resourceObject = Instantiate(mySheep, tempHex.pos + new Vector3(0.0f, 0.0f, 0.0f), rotation) as GameObject;
                }
                //Horses
                else if (randomInt == 2)
                {
                    //Debug.Log("CreatingHorse");
                    tempHex.hexResource = resourceType.Horse;
                    tempHex.resourceObject = Instantiate(myHorse, tempHex.pos + new Vector3(0.0f, 0.0f, 0.0f), rotation) as GameObject;
                }
                //Iron
                else if (randomInt == 3)
                {
                    //Debug.Log("CreatingIron");
                    tempHex.hexResource = resourceType.Iron;
                    tempHex.resourceObject = Instantiate(myIron, tempHex.pos + new Vector3(0.0f, 0.5f, 0.0f), rotation) as GameObject;
                }
                //Coal
                else
                {
                    //Debug.Log("CreatingCoal");
                    tempHex.hexResource = resourceType.Coal;
                    tempHex.resourceObject = Instantiate(myCoal, tempHex.pos + new Vector3(0.0f, 0.5f, 0.0f), rotation) as GameObject;
                }
            }
            else if (tempHex.hexTexture == hexType.Sand)
            {
                //Iron
                if (randomInt == 1)
                {
                    //Debug.Log("CreatingIron");
                    tempHex.hexResource = resourceType.Iron;
                    tempHex.resourceObject = Instantiate(myIron, tempHex.pos + new Vector3(0.0f, 0.5f, 0.0f), rotation) as GameObject;
                }
                //Coal
                if (randomInt == 2)
                {
                    //Debug.Log("CreatingCoal");
                    tempHex.hexResource = resourceType.Coal;
                    tempHex.resourceObject = Instantiate(myCoal, tempHex.pos + new Vector3(0.0f, 0.5f, 0.0f), rotation) as GameObject;
                }
            }
            else if (tempHex.hexTexture == hexType.Snow)
            {
                //Iron
                if (randomInt == 1)
                {
                    //Debug.Log("CreatingIron");
                    tempHex.hexResource = resourceType.Iron;
                    tempHex.resourceObject = Instantiate(myIron, tempHex.pos + new Vector3(0.0f, 0.5f, 0.0f), rotation) as GameObject;
                }
                //Coal
                if (randomInt == 2)
                {
                    //Debug.Log("CreatingCoal");
                    tempHex.hexResource = resourceType.Coal;
                    tempHex.resourceObject = Instantiate(myCoal, tempHex.pos + new Vector3(0.0f, 0.5f, 0.0f), rotation) as GameObject;
                }
            }
        }
        else
        {
            tempHex.resourceObject = null;
        }
        if ((Mathf.PerlinNoise(xCoord * 3, yCoord * 3) <= 0.3f)
            && tempHex.hexTexture != hexType.DeepWater && tempHex.hexTexture != hexType.Water && tempHex.hexTexture != hexType.Sand && tempHex.hexTexture != hexType.Mountains)
        {
            Quaternion fRotation = Quaternion.Euler(0, 60 * (int)Random.Range(0f, 6f), 0);
            tempHex.isForest = true;
            if (tempHex.hexTexture == hexType.Snow)
            {
                tempHex.forestTile = Instantiate(myForest[1], tempHex.pos, fRotation) as GameObject;
            }
            else
            {
                tempHex.forestTile = Instantiate(myForest[0], tempHex.pos, fRotation) as GameObject;
            }
            tempHex.forestTile.transform.SetParent(mapShrubbery.transform);
            tempHex.forestTile.SetActive(false);
        }
        else
        {
            tempHex.isForest = false;
            tempHex.forestTile = null;
        }
        tempHex.currentMilitaryUnit = null;
        if (i == 180 && j == 53)
        {
            tempHex.currentPassiveUnit = Instantiate(myPassiveUnits[0], tempHex.pos, rotation) as GameObject;
            tempHex.currentPassiveUnit.transform.SetParent(mapUnits.transform);
            tempHex.currentPassiveUnit.SetActive(false);
        }
        else
        {
            tempHex.currentPassiveUnit = null;
        }
        tempHex.currentTileBuilding = null;
        hexs.Add(tempHex);
        tempHex.hexObj.SetActive(false);
        tempHex.hexObj.transform.SetParent(transform);
        if (tempHex.resourceObject != null)
        {
            tempHex.resourceObject.transform.SetParent(mapResources.transform);
            tempHex.resourceObject.SetActive(false);
        }
    }
    void settleCity(string name)
    {
        myPlayer.settleCity(name);
        foreach (GameObject cityBox in myInfoUIBoxes)
        {
            GameObject tempCity = Instantiate(myBaseImage, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
            SetParent(tempCity.transform, cityBox.transform);
            if (myPlayer.getNumberCities() > 3)
            {
                RectTransform cityBoxrt = cityBox.GetComponent(typeof(RectTransform)) as RectTransform;
                cityBoxrt.sizeDelta = new Vector2(670, myPlayer.getNumberCities() * 102);
                cityBoxrt.localPosition = new Vector3(0, -56 * myPlayer.getNumberCities(), 0);
            }
            tempCity.name = name;
            TextMeshProUGUI nameText = tempCity.transform.Find("CityName").GetComponent<TextMeshProUGUI>();
            nameText.text = name;
        }
    }

    void SetParent(Transform child, Transform parent)
    {
        child.parent = parent;
        child.localPosition = Vector3.zero;
        child.localRotation = Quaternion.identity;
        child.localScale = Vector3.one;
        parent.localPosition = Vector3.zero;
        parent.localRotation = Quaternion.identity;
        parent.localScale = Vector3.one;
    }

    public void MoveHex(int index)
    {
        hexs[index].hexObj.transform.position = new Vector3(hexs[index].pos.x, 20, hexs[index].pos.z);
    }

    public void CollectYields()
    {
        foreach (Classes.City c in myPlayer.GetCities())
        {
            myPlayer.AddGold(c.GetGold());
            myPlayer.AddMagicka(c.GetMagicka());
            myPlayer.AddScience(c.GetScience());
            myPlayer.AddInfluence(c.GetInfluence());
            myPlayer.AddFestivity(c.GetFestivity());
            myYieldDisplays[0].GetComponent<TextMeshProUGUI>().text = myPlayer.GetGold().ToString();
            myYieldDisplays[1].GetComponent<TextMeshProUGUI>().text = myPlayer.GetMagicka().ToString();
            myYieldDisplays[2].GetComponent<TextMeshProUGUI>().text = myPlayer.GetCity(0).GetProduction().ToString();
            myYieldDisplays[3].GetComponent<TextMeshProUGUI>().text = myPlayer.GetScience().ToString();
            myYieldDisplays[4].GetComponent<TextMeshProUGUI>().text = myPlayer.GetInfluence().ToString();
            myYieldDisplays[5].GetComponent<TextMeshProUGUI>().text = myPlayer.GetFestivity().ToString();
            myYieldDisplays[6].GetComponent<TextMeshProUGUI>().text = myPlayer.GetCity(0).GetGrowth().ToString();
        }
    }
}
