using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// An enum to define types of terrain for the hexes
public enum hexType { DeepWater, Water, Sand, GrassLand, Snow, Mountains };
// An enum to define the resources available on a hex
public enum resourceType { None, Cow, Sheep, Horse, Iron, Coal }
/// <summary>
/// A struct representing a hexagonal tile on a game board.
/// </summary>
public struct Hex
{
    public hexType hexTexture; // An enum representing the tile type of the hexagonal tile.
    public resourceType hexResource; // An enum representing the resource type of the hexagonal tile.
    public bool isForest; // A boolean indicating whether the hexagonal tile has a forest.
    public GameObject hexObj; //  reference to the game object representing the hexagonal tile.
    public GameObject resourceObject; // A reference to the game object representing the resource on the hexagonal tile.
    public Classes.Unit currentPassiveUnit;
    public Classes.Unit currentMilitaryUnit;
    public Classes.Unit currentTileBuilding;
    public GameObject forestTile;
    public Vector2 XY;
    public Vector3 pos;
    public int parent;
    public int moveCost;
    public float gCost;
};
/// <summary>
/// Responsible for creating the hexagonal map
/// </summary>
/// <brief>
/// This class is responsible for creating the hexagonal map and all the objects and UI elements associated with it. It contains references to all the game objects and 
/// variables necessary tocreate and populate the map. It uses perlin noise to generate terrain height and assign hex textures,as well as to spawn resources on the map. 
/// It also creates players and their associated units and cities, and is responsible for updating the game state with each turn.
/// </brief>
public class CreateHex : MonoBehaviour
{
    // Arrays of game objects used for creating different terrain types and game objects
    public GameObject[] myMountains;
    public GameObject[] mySnow;
    public GameObject[] mySand;
    public GameObject[] myGrassland;
    public GameObject[] myForest;
    public GameObject[] myWater;
    public GameObject[] myPassiveUnits;
    public GameObject[] myMilitaryUnits;
    public GameObject[] myInfoUIBoxes;
    public GameObject[] myAccumDisplays;
    public GameObject[] myIncrementDisplays;
    public GameObject[] myCities;
    public GameObject infoUIBox;
    public GameObject myBaseImage;
    public GameObject mySheep;
    public GameObject myHorse;
    public GameObject myIron;
    public GameObject myCoal;
    public GameObject hexCap;

    // GameObject to store map resources (e.g. sheep, coal, iron ect...)
    public GameObject mapResources;
    public GameObject mapUnits;
    public GameObject mapShrubbery;

    // GameObjects to store UI elements
    public GameObject unitNameText;
    public GameObject action1Text;
    public GameObject action2Text;
    public GameObject action3Text;
    public GameObject attackOverlay;
    public GameObject buildOption;

    // Game objects used for storing units and cities
    public GameObject passiveUnitParent;
    public GameObject militaryUnitParent;
    public GameObject cityParent;
    public GameObject billboardParent;

    // My game camera
    public GameObject myCamera;

    // Name and unit Billboard objects
    public GameObject nameBillboard;
    public GameObject unitBillboard;
    // hp bar object
    public GameObject hpBar;

    // Build menu object
    public GameObject buildMenuObject;



    // Floats for perlin noise
    int scale;
    int globalUnitID;
    int currentGlobalUnitID;
    int previousGlobalUnitID;

    // Height and Width of map
    public int width;
    public int height;

    public int lastClickedButtonNo;
    // Z component of map
    int mapHeight;

    // Map 
    public GameObject map;
    public GameObject cityMap;

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

    //Turn
    int turn;

    //Player
    public List<Classes.Player> myPlayers;
    public List<Classes.PassiveUnit> passiveUnits;
    public Sprite[] myPassiveUnitIcons;
    public Sprite[] myPassiveUnitIconsCircles;
    public Sprite[] myMilitaryUnitIcons;
    public Sprite[] myMilitaryUnitIconsCircles;
    public Sprite[] myBuildingUnitIcons;
    public Sprite[] myYieldIcons;
    public Sprite[] myLeaderIcons;
    public Classes.Player currentPlayer;
    Classes.Unit currentUnit;
    int lastDrawnLeaderNo = 0;

    float unit = 2.0f / 1.2f;

    int noPlayers = 6; //Number of players

    bool attackReady = false; // If a unit is in attack mode

    // Awake is called when the script instance is being loaded
    private void Awake() 
    {
        SetDimensions();
        lastClickedButtonNo = -1;
    }

    // Start is called before the first frame update
    void Start() 
    {
        turn = 0; // Initialises turn to 0.
        mapHeight = 1; // Initialises map height to 1.

        scale = 15 / (384 / width); // Set scale of map proportional to a spacial dimension of it

        hexs = new List<Hex>(width * height); // Creates a new list of Hex objects with a size based on the width and height.
        pixels = new Texture2D(width, height).GetPixels(); // Gets the pixels of a new texture with a size based on the width and height.
        myPlayers = new List<Classes.Player>(); // Creates a new list of Player objects.
        for (int i = 0; i < height; i++) // Loop through the height of the map.
        {
            for (int j = 0; j < width; j++)
            {
                rotation = Quaternion.Euler(0, 0, 0); // Initialises the rotation to zero.
                createTiles(i, j); // Creating Tiles
                createResources(i, j); // Creating Resources
            }
        }
        createPlayers(); // Calls the method to create players.

        changePlayer(myPlayers[0]); // Changes the current player to the first player in the list.

        //Debug.Log(currentPlayer.GetCities()[0].GetFestivity());

        map.GetComponent<MapScript>().RevealMap(pixels); // Calls the method to draw the map.
    }
    /// <summary>
    /// This method creates all the players for the game. 
    /// </summary>
    /// <brief>
    ///  It creates a human player and a number of AI players based on the value of noPlayers. Each player is represented by an instance of the Player class 
    ///  and is given a leader and a starting position.The starting position is created using the createFirstSettler method with a specified Vector2 position. 
    ///  The players are then added to the myPlayers list.
    /// </brief>
    void createPlayers()
    {
        int a = 0; // a counter for the x-axis position of the starting settlement for each player
        int add = 0; // a flag to add an extra player if the human player is chosen as a leader
        currentPlayer = new Classes.Player(); // create a new instance of the Player class
        currentPlayer.SetHuman(); // set the current player as human
        currentPlayer.SetLeader(PlayerPrefs.GetInt("leader")); // set the current player's leader based on PlayerPrefs
        currentPlayer.SetStartingPosition(createFirstSettler(new Vector2(0, 0))); // create the starting settlement at (0,0) using createFirstSettler method
        myPlayers.Add(currentPlayer); // add the current player to the list of players
        for (int i = 0; i < noPlayers - 1 + add; i++) // loop through the remaining players
        {   
            if (i == PlayerPrefs.GetInt("leader")) // check if the current player is the leader chosen by the human player
            {
                add = 1; // set flag to add an extra player
            }
            else
            {
                currentPlayer = new Classes.Player();
                int b = i % 2; // calculate the y-axis position of the starting settlement for each player
                currentPlayer.SetLeader(i); // set the leader of the AI player
                currentPlayer.SetStartingPosition(createFirstSettler(new Vector2(a * width / (noPlayers / 2), b * height / 2))); // create the starting settlement at the calculated position using createFirstSettler method
                if (b == 0) // check if the current player is on the top row
                {
                    a += 1; // increment the x-axis counter for the next row
                }
                myPlayers.Add(currentPlayer); // add the current player to the list of players
            }
        }
    }

    /// <summary>
    /// Script to be used only for creating first settler per player, determines things such as starting position. Must be seperate to normal settler generation, because all 
    /// future settlers will be created by a city, requiring a city(these do not)
    /// </summary>
    /// <param name="position">The position on the map where the first settler should be created.</param>
    /// <returns>The position of the hex object where the settler is created.</returns>
    Vector3 createFirstSettler(Vector2 position)
    {
        // Create settler unit class object
        Classes.PassiveUnit newUnit = new Classes.PassiveUnit();
        newUnit.SetPassiveUnit(Classes.Unit.passiveUnits.Settler);
        newUnit.SetUnitID(globalUnitID);
        globalUnitID += 1;

        // Set unit's attributes
        newUnit.SetMaxHP(1);
        newUnit.SetHP(1);
        newUnit.SetMovement(2);
        newUnit.SetMaxMovement(2);

        // Set settler position
        Vector2 myVec2 = getSettlerPosition(position); // Get the position of the settler
        Hex currentHex = hexs[(int)(myVec2.y * width + myVec2.x)]; // Get the hex object at that position
        while (currentHex.hexTexture == hexType.DeepWater || currentHex.hexTexture == hexType.Water || currentHex.hexTexture == hexType.Mountains) // Check if the hex object is not water or mountains, and if not, create a new settler object
        {
            myVec2 = getSettlerPosition(position);
            currentHex = hexs[(int)(myVec2.y * width + myVec2.x)];
        }

        // Create settler game object
        newUnit.SetGameObject(Instantiate(myPassiveUnits[0], new Vector3(currentHex.hexObj.transform.position.x, currentHex.hexObj.transform.position.y, currentHex.hexObj.transform.position.z), rotation) as GameObject);
        newUnit.GetGameObject().transform.SetParent(passiveUnitParent.transform);
        newUnit.SetHexID((int)(myVec2.y * width + myVec2.x));


        // Create settler Billboard
        GameObject settlerBillboard = Instantiate(unitBillboard, new Vector3(newUnit.GetGameObject().transform.position.x, newUnit.GetGameObject().transform.position.y + 4.0f, newUnit.GetGameObject().transform.position.z), Quaternion.identity);
        settlerBillboard.transform.GetComponent<SpriteRenderer>().sprite = myPassiveUnitIconsCircles[0];
        Vector3 playerColour = newUnit.GetGameObject().GetComponent<OutLineScript>().GetColour(currentPlayer.GetLeader());
        settlerBillboard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(playerColour.x, playerColour.y, playerColour.z, 0.5f);
        newUnit.SetBillboardGameObject(settlerBillboard);
        // Create settler HPBar
        GameObject settlerHPBar = Instantiate(hpBar, new Vector3(newUnit.GetGameObject().transform.position.x, newUnit.GetGameObject().transform.position.y, newUnit.GetGameObject().transform.position.z), Quaternion.identity);

        settlerHPBar.GetComponent<HealthBarScript>().SetHealth(newUnit.GetHP(), newUnit.GetMaxHP());
        settlerHPBar.transform.SetParent(settlerBillboard.transform);
        settlerHPBar.transform.localScale = new Vector3(0.055f, 0.05f, 0.01f);
        settlerHPBar.transform.localPosition += new Vector3(0.0f, 31f, 0.0f);
        newUnit.SetHPBarGameObject(settlerHPBar);

        ChangeBottomUI(newUnit);
        changeCurrentUnit(newUnit);
        currentPlayer.AddPassiveUnit(newUnit);
        currentHex.currentPassiveUnit = newUnit;
        hexs[(int)(myVec2.y * width + myVec2.x)] = currentHex;
        return currentHex.hexObj.transform.position;
    }
    /// <summary>
    /// Generates a random position for a settler unit based on the given position
    /// </summary>
    /// <param name="position">position The initial position to add the random offset to</param>
    /// <returns>Returns a Vector2 representing the new position for the settler unit</returns>
    Vector2 getSettlerPosition(Vector2 position)
    {
        Vector2 myVec2 = new Vector2(Random.Range(0, width / 2), Mathf.Floor(Random.Range(0, height / 3))); // Generate a random position for the settler unit within a given range
        myVec2 += position; // Add the initial position to the random offset
        if (myVec2.x >= width) // Check if the position is out of bounds and adjust if necessary
        {
            myVec2.x -= width;
        }
        if (myVec2.x < 0)
        {
            myVec2.x -= width;
        }
        if (myVec2.y >= height)
        {
            myVec2.y -= height;
        }
        if (myVec2.y < 0)
        {
            myVec2.y += width;
        }
        return myVec2; // Return the new position for the settler unit
    }
    /// <summary>
    /// Updates the bottom UI panel with information based on the currently selected unit
    /// </summary>
    /// <param name="unitToOverlay">The unit whose information will be displayed on the UI</param>
    void ChangeBottomUI(Classes.Unit unitToOverlay)
    {
        if (unitToOverlay.GetUnitType() == Classes.Unit.unitType.Passive) // Check if the unit to overlay is of type "Passive"
        {
            if (unitToOverlay.GetPassiveUnitType() == Classes.Unit.passiveUnits.Settler) // Check if the passive unit type is a "Settler"
            {
                GameObject.Find("CurrentUnitImage").GetComponent<Image>().sprite = myPassiveUnitIcons[0]; // Set the image of the current unit to the first passive unit icon
                unitNameText.GetComponent<TextMeshProUGUI>().text = "Settler"; // Set the name of the unit to "Settler"
                action1Text.GetComponent<TextMeshProUGUI>().text = "Create City"; // Set the first action text to "Create City"
                action2Text.GetComponent<TextMeshProUGUI>().text = "Sleep"; // Set the second action text to "Sleep"
            }
        }
        if (unitToOverlay.GetUnitType() == Classes.Unit.unitType.Building) // Check if the unit to overlay is of type "Building"
        {
            if (unitToOverlay.GetBuildingUnitType() == Classes.Unit.buildingUnits.City) // Check if the building unit type is a "City"
            {
                GameObject.Find("CurrentUnitImage").GetComponent<Image>().sprite = myBuildingUnitIcons[0]; // Set the image of the current unit to the first building unit icon
                unitNameText.GetComponent<TextMeshProUGUI>().text = unitToOverlay.GetGameObject().name; // Set the name of the unit to the name of the game object
                action1Text.GetComponent<TextMeshProUGUI>().text = "Build"; // Set the first action text to "Build"
                action2Text.GetComponent<TextMeshProUGUI>().text = "Sleep"; // Set the second action text to "Sleep"
            }
        }
        if (unitToOverlay.GetUnitType() == Classes.Unit.unitType.Military) // Check if the unit to overlay is of type "Military"
        {
            if (unitToOverlay.GetMilitaryUnitType() == Classes.Unit.militaryUnits.Scout) // Check if the military unit type is a "Scout"
            {
                GameObject.Find("CurrentUnitImage").GetComponent<Image>().sprite = myMilitaryUnitIcons[0]; // Set the image of the current unit to the first military unit icon
                unitNameText.GetComponent<TextMeshProUGUI>().text = unitToOverlay.GetGameObject().name; // Set the name of the unit to the name of the game object
                action1Text.GetComponent<TextMeshProUGUI>().text = "Attack"; // Set the first action text to "Attack"
                action2Text.GetComponent<TextMeshProUGUI>().text = "Sleep"; // Set the second action text to "Sleep"
            }
        }
        if (unitToOverlay.GetUnitType() == Classes.Unit.unitType.Military) // Check if the unit to overlay is of type "Military"
        {
            if (unitToOverlay.GetMilitaryUnitType() == Classes.Unit.militaryUnits.Axeman) // Check if the military unit type is an "Axeman"
            {
                GameObject.Find("CurrentUnitImage").GetComponent<Image>().sprite = myMilitaryUnitIcons[1]; // Set the image of the current unit to the second military unit icon
                unitNameText.GetComponent<TextMeshProUGUI>().text = unitToOverlay.GetGameObject().name; // Set the name of the unit to the name of the game object
                action1Text.GetComponent<TextMeshProUGUI>().text = "Attack"; // Set the first action text to "Attack"
                action2Text.GetComponent<TextMeshProUGUI>().text = "Sleep"; // Set the second action text to "Sleep"
            }
        }
    }
    /// <summary>
    /// This function generates a hexagonal tile based on the given i and j coordinates.
    /// </summary>
    /// <param name="i">The row index of the tile</param>
    /// <param name="j">The column index of the tile</param>
    void createTiles(int i, int j)
    {
        // Calculate x and y coordinates based on the given i and j indices, the map width and height, and the scale value
        float xCoord = (float)(2 * j) / width * scale;
        float xCoordPlusOne = (float)(1 + (2 * j)) / width * scale;
        float yCoordPlusOne = (float)(1 + (2 * i)) / height * scale;
        float yCoord = (float)(2 * i) / height * scale;
        float xFirstYPos;
        // Generate different types of terrain based on the generated Perlin noise value xFirstYPos
        // Generate Perlin noise value based on the calculated x and y coordinates
        if (i % 2 == 0)
        {
            xFirstYPos = Mathf.PerlinNoise(xCoord, yCoord);
        }
        else
        {
            xFirstYPos = Mathf.PerlinNoise(xCoordPlusOne, yCoordPlusOne);
        }
        // Generate different types of terrain based on the generated Perlin noise value xFirstYPos
        // Set the hexTexture and moveCost properties of the tile based on the generated terrain type
        // Select a hex model from arrays of models specific to each terrain type
        // Set the color and position of the tile based on the selected terrain type and model
        // Instantiate the tile game object with the selected model at the calculated position and rotation
        // Mountains
        if (xFirstYPos > 0.9f * mapHeight)
        {
            rotation = Quaternion.Euler(0, 60 * (int)Random.Range(0f, 6f), 0);
            tempHex.hexTexture = hexType.Mountains;
            tempHex.moveCost = 999;
            chosenHex = myMountains[Random.Range(0, 4)];
            colour.r = 0.5343f;
            colour.g = 0.7068f;
            colour.b = 0.4362f;
        }
        // Snow
        else if (xFirstYPos > 0.75f )
        {
            tempHex.hexTexture = hexType.Snow;
            tempHex.moveCost = 2;
            chosenHex = mySnow[0];
            colour.r = 0.5108f;
            colour.g = 0.6872f;
            colour.b = 0.4205f;
        }
        // Grassland
        else if (xFirstYPos > 0.5f )
        {
            tempHex.hexTexture = hexType.GrassLand;
            tempHex.moveCost = 1;
            chosenHex = myGrassland[0];
            colour.r = 0.4912f;
            colour.g = 0.6676f;
            colour.b = 0.3931f;
        }
        // Sand
        else if (xFirstYPos > 0.45f )
        {
            tempHex.hexTexture = hexType.Sand;
            tempHex.moveCost = 1;
            chosenHex = mySand[0];
            colour.r = 0.4716f;
            colour.g = 0.6480f;
            colour.b = 0.3696f;
        }
        // Shallow water
        else if (xFirstYPos > 0.3f)
        {
            tempHex.hexTexture = hexType.Water;
            tempHex.moveCost = 1;
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
            tempHex.moveCost = 1;
            chosenHex = myWater[1];
            xFirstYPos = 1.5f * mapHeight;
            colour.r = 0.4490f;
            colour.g = 0.5941f;
            colour.b = 0.6960f;
        }

        xFirstYPos *= 3.3333f * mapHeight; // Multiply xFirstYPos by 3.3333 times the mapHeight
        colour.a = 1; // Set the alpha value of the 'colour' variable to 1
        pixels[(height * j) + i] = colour; // Set the pixel value of the pixel array at the specified position to 'colour'

        if (i % 2 == 0)// Check if which column we are processing
        {
            tempHex.pos = new Vector3(i * 1.44337567289f, xFirstYPos, j * unit); // Set the position of tempHex to a new Vector3 object with x, y, and z values based on i, xFirstYPos, and j
        }
        else
        {
            tempHex.pos = new Vector3(i * 1.44337567298f, xFirstYPos, (1.44337567298f / 2) + (1.44337567298f / 10) + (j * unit)); // Set the position of tempHex to a new Vector3 object with x, y, and z values based on i, xFirstYPos, and j
        }

        if (tempHex.hexTexture == hexType.DeepWater || tempHex.hexTexture == hexType.Water) // Check if the hexTexture of tempHex is equal to hexType.DeepWater or hexType.Water
        {
            tempHex.pos = new Vector3(tempHex.pos.x, 1.15f, tempHex.pos.z); // Adjust the y value to waterlevel
        }
        if (tempHex.hexTexture == hexType.Mountains) // Check if the hexTexture of tempHex is equal to hexType.Mountains
        {
            tempHex.pos = new Vector3(tempHex.pos.x, tempHex.pos.y + (chosenHex.GetComponent<BoxCollider>().size.y * 25), tempHex.pos.z); // Adjust the y value of tempHex.pos to the current y value plus the height of the mountain
        }

        tempHex.hexObj = Instantiate(chosenHex, tempHex.pos, rotation) as GameObject; // Instantiate a new hex object based on the chosen hex object, with position and rotation based on tempHex, and add it to the hexs list
        tempHex.hexObj.GetComponent<TileScript>().SetIndex(hexs.Count); // Set the index of the new hex object in the hexs list to the current count of the hexs list
    }
    /// <summary>
    /// This function creates resources on the map based on certain conditions. It sets properties of the hexagon such as the type of resource and whether there is a forest present.
    /// </summary>
    /// <param name="i">The column index of the hexagon being processed</param>
    /// <param name="j">The row index of the hexagon being processed</param>
    void createResources(int i, int j)
    {
        // Set currentMilitaryUnit, currentPassiveUnit, currentTileBuilding, resourceObject, and hexResource to null or None
        tempHex.currentMilitaryUnit = null; 
        tempHex.currentPassiveUnit = null;
        tempHex.currentTileBuilding = null;
        tempHex.resourceObject = null;
        tempHex.hexResource = resourceType.None;
        if (Random.Range(0, 35) == 1) // Randomly assign a resource object to the hexagon with a 1 in 35 chance
        {
            int randomInt = Random.Range(0, 4); // Randomly assign a resource type
            if (tempHex.hexTexture == hexType.GrassLand)  // If hexagon is grassland, create either sheep, horse, iron, or coal
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
            // If hexagon is sand, create either iron or coal
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
                else if (randomInt == 2)
                {
                    //Debug.Log("CreatingCoal");
                    tempHex.hexResource = resourceType.Coal;
                    tempHex.resourceObject = Instantiate(myCoal, tempHex.pos + new Vector3(0.0f, 0.5f, 0.0f), rotation) as GameObject;
                }
            }
            // If hexagon is snow, create either iron or coal
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
                else if (randomInt == 2)
                {
                    //Debug.Log("CreatingCoal");
                    tempHex.hexResource = resourceType.Coal;
                    tempHex.resourceObject = Instantiate(myCoal, tempHex.pos + new Vector3(0.0f, 0.5f, 0.0f), rotation) as GameObject;
                }
            }
        }
        float yCoord = (float)(j) / height * scale; // Set the y coordinate of the hexagon using the current column (j) and the height of the map, and scale it by a given factor
        float xCoord = (float)(i) / width * scale; // Set the x coordinate of the hexagon using the current row (i) and the width of the map, and scale it by a given factor

        if ((Mathf.PerlinNoise(xCoord * 3, yCoord * 3) <= 0.3f)
            && tempHex.hexTexture != hexType.DeepWater && tempHex.hexTexture != hexType.Water && tempHex.hexTexture != hexType.Sand && tempHex.hexTexture != hexType.Mountains)
        {
            Quaternion fRotation = Quaternion.Euler(0, 60 * (int)Random.Range(0f, 6f), 0); // Set the rotation of the forest tile to a random multiple of 60 degrees
            tempHex.isForest = true; // Set the hexagon's "isForest" variable to true
            if (tempHex.hexTexture == hexType.Snow) // If the hexagon's texture is snow, instantiate a snow forest tile; otherwise, instantiate a regular forest tile
            {
                tempHex.forestTile = Instantiate(myForest[1], tempHex.pos, fRotation) as GameObject;
            }
            else
            {
                tempHex.forestTile = Instantiate(myForest[0], tempHex.pos, fRotation) as GameObject;
            }
            // Set the forest tile's parent to the mapShrubbery object and disable it
            tempHex.forestTile.transform.SetParent(mapShrubbery.transform);
            tempHex.forestTile.SetActive(false);
        }
        else
        {
            // Set the hexagon's "isForest" variable to false and its forest tile to null
            tempHex.isForest = false;
            tempHex.forestTile = null;
        }
        // Disable the hexagon object and set its parent to the current object's parent
        tempHex.hexObj.SetActive(false);
        tempHex.hexObj.transform.SetParent(transform);
        if (tempHex.hexResource != resourceType.None) // If the hexagon has a resource type other than "None", add its index to its resource object's name and set its parent to the mapResources object, then disable it
        {
            tempHex.resourceObject.name = tempHex.resourceObject.name + hexs.Count.ToString();
            tempHex.resourceObject.transform.SetParent(mapResources.transform);
            tempHex.resourceObject.SetActive(false);
        }
        tempHex.XY = new Vector2(i, j); // Set the hexagon's XY variable to a vector with its current row and column as its x and y values, respectively
        // Add the hexagon to the list of hexagons and set its connections
        hexs.Add(tempHex);
        SetConnections(hexs.Count - 1);
    }

    /// <summary>
    /// Perform an action for the current unit based on its type.
    /// </summary>
    /// <brief>
    /// If the unit is passive, check if it is a settler and call the settleCity() function if true.
    /// If the unit is a building, check if it is a city and toggle the building menu if true.
    /// If the unit is military, toggle the attackReady flag and enable/disable the attack overlay
    /// </brief>
    public void Action1()
    {
        if (currentUnit.GetUnitType() == Classes.Unit.unitType.Passive)
        {
            if (currentUnit.GetPassiveUnitType() == Classes.Unit.passiveUnits.Settler)
            {
                settleCity(); // Call the function to settle a new city
            }
        }
        else if (currentUnit.GetUnitType() == Classes.Unit.unitType.Building)
        {
            if (currentUnit.GetBuildingUnitType() == Classes.Unit.buildingUnits.City)
            {
                ToggleBuildMenu(); // Toggle the building menu
            }
        }
        else if (currentUnit.GetUnitType() == Classes.Unit.unitType.Military)
        {
            attackReady = !attackReady; // Toggle the attackReady flag
            attackOverlay.SetActive(attackReady); // Enable/disable the attack overlay based on the attackReady flag
        }
    }
    /// <summary>
    /// Creates a new city object and adds it to the current player's city list.
    /// </summary>
    void settleCity()
    {
        if (hexs[currentUnit.GetHexID()].currentTileBuilding != null)
        {
            return;
        }

        // Create city unit
        Classes.City newCity = new Classes.City(currentPlayer.GetLeader(), currentPlayer.GetCities().Count, Instantiate(myCities[0], currentUnit.GetGameObject().transform.position, Quaternion.identity) as GameObject);
        newCity.SetUnitID(globalUnitID);
        globalUnitID += 1;
        newCity.GetGameObject().transform.SetParent(cityParent.transform);
        newCity.SetHexID(currentUnit.GetHexID());

        // Set city attributes
        newCity.SetMaxHP(50);
        newCity.SetHP(50);
        newCity.SetDefense(10);
        newCity.SetAttack(5);

        // Billboard
        GameObject cityNameBillboard = Instantiate(nameBillboard, new Vector3(currentUnit.GetGameObject().transform.position.x, currentUnit.GetGameObject().transform.position.y + 2.5f, currentUnit.GetGameObject().transform.position.z), Quaternion.identity);
        cityNameBillboard.GetComponentInChildren<TextMeshPro>().text = newCity.GetName();
        Vector3 playerColour = newCity.GetGameObject().GetComponent<OutLineScript>().GetColour(currentPlayer.GetLeader());
        cityNameBillboard.GetComponent<SpriteRenderer>().color = new Color(playerColour.x, playerColour.y, playerColour.z, 0.5f);
        cityNameBillboard.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = myLeaderIcons[currentPlayer.GetLeader()];
        newCity.SetBillboardGameObject(cityNameBillboard);
        newCity.SetBuildingUnit(Classes.Unit.buildingUnits.City);

        // HP Bar
        GameObject cityHPBar = Instantiate(hpBar, new Vector3(currentUnit.GetGameObject().transform.position.x, currentUnit.GetGameObject().transform.position.y + 2.5f, currentUnit.GetGameObject().transform.position.z), Quaternion.identity);
        cityHPBar.GetComponent<HealthBarScript>().SetHealth(newCity.GetHP(), newCity.GetMaxHP());
        cityHPBar.transform.SetParent(cityNameBillboard.transform);
        cityHPBar.transform.localScale = new Vector3(0.095f, 0.05f, 0.01f);
        cityHPBar.transform.localPosition += new Vector3(0.0f, 9f, 0.0f);
        newCity.SetHPBarGameObject(cityHPBar);

        // Create a settler building option inside my city
        newCity.AddProduct(new Classes.Produce(myPassiveUnitIcons[0], 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        newCity.AddProduct(new Classes.Produce(myPassiveUnitIcons[0], 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        newCity.AddProduct(new Classes.Produce(myPassiveUnitIcons[0], 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0));

        currentPlayer.SettleCity(newCity); // Settle the new city
        myCamera.transform.position = new Vector3(currentUnit.GetGameObject().transform.position.x-(myCamera.transform.position.y / 3.333f), 
            myCamera.transform.position.y, currentUnit.GetGameObject().transform.position.z); // Set the camera position to focus on the current unit
        Kill(currentUnit); // Kill settler
        foreach (GameObject cityBox in myInfoUIBoxes) // Loop over each city box in the UI
        {
            // Instantiate a new city info object and set its parent to the current city box
            GameObject cityOverview = Instantiate(myBaseImage, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject; 
            setParent(cityOverview.transform, cityBox.transform);

            if (currentPlayer.GetCities().Count > 3) // If the player has more than three cities, resize the city box to fit them all
            {
                RectTransform cityBoxrt = cityBox.GetComponent(typeof(RectTransform)) as RectTransform;
                cityBoxrt.sizeDelta = new Vector2(670, currentPlayer.GetCities().Count * 102 + 102);
                cityBoxrt.localPosition = new Vector3(0, -56 * currentPlayer.GetCities().Count, 0);
            }
            if (lastClickedButtonNo != -1) // If a city button has been clicked, update the city info with the appropriate yield information
            {
                cityOverview.transform.Find("YieldImage").GetComponent<Image>().sprite = myYieldIcons[lastClickedButtonNo];
                cityOverview.transform.Find("CityYieldOutput").GetComponent<TextMeshProUGUI>().text = newCity.GetYield(lastClickedButtonNo);
            }

            // Set the name and text of the city info object to match the new city
            cityOverview.name = newCity.GetName();
            cityOverview.transform.Find("CityName").GetComponent<TextMeshProUGUI>().text = newCity.GetName();
        }
        newCity.GetGameObject().name = newCity.GetName(); // Set the name of the new city's game object to match its name
        // City can't move!
        newCity.SetMovement(0);
        newCity.SetMaxMovement(0);
        // Change the current unit to the new city and the bottom UI to match
        changeCurrentUnit(newCity);
        ChangeBottomUI(newCity);
        // Get the hex corresponding to the new city and update it to indicate that a city has been built on it
        Hex cityHex = hexs[newCity.GetHexID()];
        cityHex.currentTileBuilding = newCity;
        hexs[newCity.GetHexID()] = cityHex;
        cityMap.GetComponent<DrawCities>().Draw(newCity.GetHexID()); // Redraw the city map to show the new city
        updateYieldDisplays(currentPlayer); // Update the yield displays for the current player
    }
    /// <summary>
    /// Toggles the visibility of the build menu object
    /// </summary>
    void ToggleBuildMenu() 
    {
        if (buildMenuObject.activeInHierarchy)
        {
            for (int i = 0; i < GameObject.Find("BM").transform.childCount; i++)
            {
                Destroy(GameObject.Find("BM").transform.GetChild(i).gameObject);
            }
        }
        buildMenuObject.SetActive(!buildMenuObject.activeInHierarchy);
        if (buildMenuObject.activeInHierarchy)
        {
            foreach (Classes.Produce product in currentUnit.GetProduce())
            {
                if (product.m_groMult == 1)
                {
                    Debug.Log("yield product");
                }
                else
                {
                    Debug.Log("unit product");
                    // Passive Unit
                    if (product.m_goldInc == 1)
                    {
                        // Settler Unit
                        if (product.m_goldMult == 0)
                        {
                            // Create settler build option object
                            GameObject settlerMenuObject = Instantiate(buildOption, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                            settlerMenuObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = myPassiveUnitIcons[0];
                            settlerMenuObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener( () => BuildSettler());
                            settlerMenuObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Settler";
                            settlerMenuObject.transform.GetChild(2).GetComponent<TextMeshPro>().text = "15";
                            setParent(settlerMenuObject.transform, GameObject.Find("BM").transform);
                            settlerMenuObject.transform.localScale = new Vector3(60.0f, 45.0f, 1.0f);
                        }
                        // Settler Unit
                        if (product.m_goldMult == 1)
                        {

                        }
                    }
                    // Military Unit
                    else if (product.m_magicInc == 1)
                    {
                        // Scout Unit
                        if (product.m_magicMult == 0)
                        {
                            // Create scout build option object
                            GameObject scoutMenuObject = Instantiate(buildOption, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                            scoutMenuObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = myMilitaryUnitIcons[0];
                            scoutMenuObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => BuildScout());
                            scoutMenuObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Scout";
                            scoutMenuObject.transform.GetChild(2).GetComponent<TextMeshPro>().text = "5";
                            setParent(scoutMenuObject.transform, GameObject.Find("BM").transform);
                            scoutMenuObject.transform.localScale = new Vector3(60.0f, 45.0f, 1.0f);
                        }
                        // Axeman Unit
                        if (product.m_magicMult == 1)
                        {
                            // Create axeman build option object
                            GameObject axemanMenuObject = Instantiate(buildOption, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                            axemanMenuObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = myMilitaryUnitIcons[1];
                            axemanMenuObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => BuildAxeman());
                            axemanMenuObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Axeman";
                            axemanMenuObject.transform.GetChild(2).GetComponent<TextMeshPro>().text = "10";
                            setParent(axemanMenuObject.transform, GameObject.Find("BM").transform);
                            axemanMenuObject.transform.localScale = new Vector3(60.0f, 45.0f, 1.0f);
                        }
                    }
                    // Building Unit
                    if (product.m_prodInc == 1)
                    {
                        // City Unit
                        if (product.m_prodMult == 0)
                        {

                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Creates a new settler unit for the current player and assigns it to the current unit, if possible
    /// </summary>
    public void BuildSettler()
    {
        if(buildMenuObject.activeInHierarchy && CreateSettler(currentUnit))
        {
            ToggleBuildMenu();
        }
    }
    /// <summary>
    /// Creates a new scout unit for the current player and assigns it to the current unit, if possible
    /// </summary>
    public void BuildScout()
    {
        if (buildMenuObject.activeInHierarchy && CreateScout(currentUnit))
        {
            ToggleBuildMenu();
        }
    }
    /// <summary>
    /// Creates a new axeman unit for the current player and assigns it to the current unit, if possible
    /// </summary>
    public void BuildAxeman()
    {
        if (buildMenuObject.activeInHierarchy && CreateAxeman(currentUnit))
        {
            ToggleBuildMenu();
        }
    }
    /// <summary>
    /// Destroys the given unit, removes it from its player's list of units, and removes its billboard GameObject
    /// </summary>
    /// <param name="unitToDestroy">The unit to be destroyed</param>
    void Kill(Classes.Unit unitToDestroy)
    {
        if (unitToDestroy.GetID() == currentUnit.GetID()) // Check if the unit to destroy is the current unit
        {
            foreach (Classes.Player player in myPlayers) // Loop through all players and remove the unit to destroy
            {
                player.RemoveUnit(unitToDestroy);
            }
            changeCurrentUnit(currentPlayer.GetNewUnit()); // Change the current unit to the next unit belonging to the current player
            ChangeBottomUI(currentUnit); // Update the UI to reflect the new current unit
        } 
        Hex unitsHex = hexs[unitToDestroy.GetHexID()]; // Get the hex that the unit to destroy is on
        // Check the type of the unit to destroy and destroy any associated objects
        if (unitToDestroy.GetUnitType() == Classes.Unit.unitType.Passive) 
        {
            Destroy(unitsHex.currentPassiveUnit.GetBillboardGameObject());
            unitsHex.currentPassiveUnit = null;
        }
        if (unitToDestroy.GetUnitType() == Classes.Unit.unitType.Building)
        {
            Destroy(unitsHex.currentTileBuilding.GetBillboardGameObject());
            unitsHex.currentTileBuilding = null;
        }
        if (unitToDestroy.GetUnitType() == Classes.Unit.unitType.Military)
        {
            Destroy(unitsHex.currentMilitaryUnit.GetBillboardGameObject());
            unitsHex.currentMilitaryUnit = null;
        }
        hexs[unitToDestroy.GetHexID()] = unitsHex; // Update the hex with the destroyed unit and object
        Destroy(unitToDestroy.GetGameObject()); // Destroy the game object associated with the unit to destroy
    }

    /// <summary>
    /// Sets the parent of a given child transform to a new parent transform and resets the local position, rotation, and scale of both transforms.
    /// </summary>
    /// <param name="child">The child transform to set the parent for</param>
    /// <param name="parent">The new parent transform</param>
    void setParent(Transform child, Transform parent)
    {
        // Set the parent of the child Transform to the new parent.
        child.SetParent(parent);
        // Reset the local position, rotation, and scale of the child Transform to match the new parent's values.
        child.localPosition = Vector3.zero;
        child.localRotation = Quaternion.identity;
        child.localScale = Vector3.one;
        // Reset the local position, rotation, and scale of the new parent Transform to their default values.
        parent.localPosition = Vector3.zero;
        parent.localRotation = Quaternion.identity;
        parent.localScale = Vector3.one;
    }
    /// <summary>
    /// Collect yields for a given player, including income and updates the yield displays
    /// </summary>
    /// <param name="player">The player whose yields are being collected</param>
    public void CollectYields(Classes.Player player)
    {
        // Check if the player has any cities
        if (player.GetCities().Count > 0)
        {
            // Collect income for the player
            player.GetIncome();
            // Update the yield displays for the player
            updateYieldDisplays(player);
        }
    }
    /// <summary>
    /// Resets the movement of all units belonging to a given player
    /// </summary>
    /// <param name="player">The player whose units' movement is to be reset</param>
    public void ResetUnits(Classes.Player player) 
    {
        foreach (Classes.PassiveUnit pUnit in player.GetPassiveUnits()) // Loop through each passive unit belonging to the player
        {
            pUnit.SetMovement(pUnit.GetMaxMovement()); // Reset the movement of the passive unit to its maximum value
            changeCurrentTileCap(pUnit, false); // Update the movement info, but dont show it
        }
        foreach (Classes.MilitaryUnit mUnit in player.GetMilitaryUnits())
        {
            mUnit.SetMovement(mUnit.GetMaxMovement()); // Reset the movement of the military unit to its maximum value
            changeCurrentTileCap(mUnit, false); // Update the movement info, but dont show it
        }
        changeCurrentTileCap(currentUnit, true); // Change the current tile capacity of the current unit to true
    }
    /// <summary>
    /// Updates UI elements to reflect the player's accumulated yield and incremental yield for each category at the end of the turn.
    /// </summary>
    /// <param name="player">The player whose yields will be displayed</param>
    public void updateYieldDisplays(Classes.Player player)
    {
        // Sets the text of each TextMeshProUGUI component in myAccumDisplays to the player's accumulated yield for each category
        myAccumDisplays[0].GetComponent<TextMeshProUGUI>().text = player.GetGold().ToString();
        myAccumDisplays[1].GetComponent<TextMeshProUGUI>().text = player.GetMagicka().ToString();
        myAccumDisplays[2].GetComponent<TextMeshProUGUI>().text = player.GetProduction().ToString();
        myAccumDisplays[3].GetComponent<TextMeshProUGUI>().text = player.GetScience().ToString();
        myAccumDisplays[4].GetComponent<TextMeshProUGUI>().text = player.GetInfluence().ToString();
        myAccumDisplays[5].GetComponent<TextMeshProUGUI>().text = player.GetFestivity().ToString();
        myAccumDisplays[6].GetComponent<TextMeshProUGUI>().text = player.GetGrowth().ToString();

        // Initializes variables for how much the player will receive for each category at the end of the turn
        float goldInc = 0;
        float magickaInc = 0;
        float productionInc = 0;
        float scienceInc = 0;
        float influenceInc = 0;
        float festivityInc = 0;
        float growthInc = 0;

        // Calculates how much of each yield the player will receive based on the yield of each city they own
        foreach (Classes.City city in player.GetCities())
        {
            goldInc += city.GetCityGold();
            magickaInc += city.GetCityMagicka();
            productionInc += city.GetCityProduction();
            scienceInc += city.GetCityScience();
            influenceInc += city.GetCityInfluence();
            festivityInc += city.GetCityFestivity();
            growthInc += city.GetCityGrowth();
        }

        // Sets the text of each TextMeshProUGUI component in myIncrementDisplays to the player's incremental yield for each category at the end of the turn
        myIncrementDisplays[0].GetComponent<TextMeshProUGUI>().text = "+" + goldInc.ToString();
        myIncrementDisplays[1].GetComponent<TextMeshProUGUI>().text = "+" + magickaInc.ToString();
        myIncrementDisplays[2].GetComponent<TextMeshProUGUI>().text = "+" + productionInc.ToString();
        myIncrementDisplays[3].GetComponent<TextMeshProUGUI>().text = "+" + scienceInc.ToString();
        myIncrementDisplays[4].GetComponent<TextMeshProUGUI>().text = "+" + influenceInc.ToString();
        myIncrementDisplays[5].GetComponent<TextMeshProUGUI>().text = "+" + festivityInc.ToString();
        myIncrementDisplays[6].GetComponent<TextMeshProUGUI>().text = "+" + growthInc.ToString();
    }
    /// <summary>
    /// Returns the ID of the current unit. If there is no current unit, it returns -1
    /// </summary>
    /// <returns>The ID of the current unit or -1 if there is no current unit</returns>
    public int GetCurrentID()
    {
        if (currentUnit != null) // check if there is a current unit
        {
            return currentUnit.GetID(); // return the current unit's ID
        }
        else // if there is no current unit
        {
            return -1; // return -1 to indicate that there is no current unit
        }
    }
    /// <summary>
    /// Sets the dimensions of the map based on the one the user selected.
    /// </summary>
    void SetDimensions()
    {
        // Check if the user selected a small map size.
        if (PlayerPrefs.GetInt("mapSize") == 0)
        {
            // If the map size is small, set the width and height accordingly.
            width = 96;
            height = 54;
        }
        // Check if the user selected a medium map size.
        else if (PlayerPrefs.GetInt("mapSize") == 1)
        {
            // If the map size is medium, set the width and height accordingly.
            width = 192;
            height = 108;
        }
        // Otherwise, assume the user selected a large map size.
        else
        {
            // If the map size is large, set the width and height accordingly.
            width = 384;
            height = 216;
        }
    }
    /// <summary>
    /// Called when a tile is clicked
    /// </summary>
    /// <param name="index">Index of the clicked tile</param>
    public void ClickOnTile(int index)
    {
        if (hexs[index].currentPassiveUnit != null)
        {
            currentGlobalUnitID = hexs[index].currentPassiveUnit.GetID();
            // Check if the player owns the unit on the tile and the current unit is not passive
            if (currentPlayer.Owns(hexs[index].currentPassiveUnit) && currentGlobalUnitID != currentUnit.GetID())
            {
                // Change the current unit to the passive unit on the tile
                changeCurrentUnit(hexs[index].currentPassiveUnit);
                // Update the bottom UI to display information about the new unit
                ChangeBottomUI(hexs[index].currentPassiveUnit);
                return;
            } 
        }
        if (hexs[index].currentTileBuilding != null)
        {
            if (attackReady)
            {
                foreach (int connection in hexs[currentUnit.GetHexID()].hexObj.GetComponent<TileScript>().GetConnections())
                {
                    if (index == connection)
                    {
                        ProcessAttack(currentUnit, hexs[index].currentTileBuilding);
                    }
                }
            }
            else
            {
                currentGlobalUnitID = hexs[index].currentTileBuilding.GetID();
                // Check if the player owns the unit on the tile and the current unit is not a building
                if (currentPlayer.Owns(hexs[index].currentTileBuilding) && currentGlobalUnitID != currentUnit.GetID())
                {
                    // Change the current unit to the building on the tile
                    changeCurrentUnit(hexs[index].currentTileBuilding);
                    // Update the bottom UI to display information about the new unit
                    ChangeBottomUI(hexs[index].currentTileBuilding);
                    return;
                }
            }
        }
        if (hexs[index].currentMilitaryUnit != null)
        {
            if (attackReady)
            {
                foreach(int connection in hexs[currentUnit.GetHexID()].hexObj.GetComponent<TileScript>().GetConnections())
                {
                    if(index == connection)
                    {
                        ProcessAttack(currentUnit, hexs[index].currentMilitaryUnit);
                    }
                }
            }
            else
            {
                currentGlobalUnitID = hexs[index].currentMilitaryUnit.GetID();
                // Check if the player owns the unit on the tile and the current unit is not a building
                if (currentPlayer.Owns(hexs[index].currentMilitaryUnit) && currentGlobalUnitID != currentUnit.GetID())
                {
                    // Change the current unit to the building on the tile
                    changeCurrentUnit(hexs[index].currentMilitaryUnit);
                    // Update the bottom UI to display information about the new unit
                    ChangeBottomUI(hexs[index].currentMilitaryUnit);
                    return;
                }
            }
        }
        previousGlobalUnitID = currentGlobalUnitID;
    }
    /// <summary>
    /// Change the current player to a new player.
    /// </summary>
    /// <param name="newPlayer">The new player to change to.</param>
    void changePlayer(Classes.Player newPlayer)
    {
        currentPlayer = newPlayer;// Set the currentPlayer to the newPlayer
        myCamera.transform.position = new Vector3(currentPlayer.GetPosition().x - 3, 10, currentPlayer.GetStartingPosition().z); // Update the camera position to center on the new player's starting position
        changeCurrentUnit(newPlayer.GetNewUnit()); // Change the current unit to the new player's new unit
    }
    /// <summary>
    /// Changes the current unit to the given newUnit
    /// </summary>
    /// <param name="newUnit">newUnit the new unit to change to</param>
    public void changeCurrentUnit(Classes.Unit newUnit)
    {
        attackReady = false; // Set the attackReady flag to false
        attackOverlay.SetActive(false); // Deactivate the attackOverlay game object
        if (currentUnit != null) // If there is a currentUnit
        {
            currentUnit.GetGameObject().GetComponent<OutLineScript>().weakenOutline(lastDrawnLeaderNo); // Weaken the outline of the current unit
            changeCurrentTileCap(currentUnit, false); // Change the current tile cap of the current unit to false
        }
        currentUnit = newUnit; // Set the currentUnit to the newUnit
        currentUnit.GetGameObject().GetComponent<OutLineScript>().drawOutline(currentPlayer.GetLeader()); // Draw the outline of the new current unit
        currentPlayer.SetPosition(currentUnit.GetGameObject().transform.position); // Set the player's position to the position of the current unit
        changeCurrentTileCap(currentUnit, true); // Change the current tile cap of the new current unit to true
        lastDrawnLeaderNo = currentPlayer.GetLeader(); // Set the lastDrawnLeaderNo to the currentPlayer's leader
        UpdateHPBar(); // Update the HP bar
    }
    /// <summary>
    /// Move the current unit to a hex tile with the given ID
    /// </summary>
    /// <brief>
    /// If the tile is reachable by the unit, the unit will be moved to the new tile and its movement will be decreased by the gCost of the tile.
    /// If the unit is of type Passive, it will be removed from its starting hex and added to the new hex, and its move status will be set.
    /// If the unit is of type Military, it will check if there is an enemy unit on the target hex.If there is, and the target hex is adjacent to the current hex, it will initiate an attack.
    /// If the target hex is not reachable, the function will recursively try to find a parent hex which is reachable, until a reachable hex is found or until there is no parent hex left.
    /// </brief>
    /// <param name="hexID">The index of the target hex tile</param>
    public void MoveUnit(int hexID)
    {
        if(currentUnit.GetMoveableTiles().Contains(hexID)) // Check if the unit can move to the target hex
        {
            if (currentUnit != null) // Remove the unit from the current hex and add it to the target hex
            {
                changeCurrentTileCap(currentUnit, false);
            }
            if (currentUnit.GetUnitType() == Classes.Unit.unitType.Passive) // If the unit is Passive, move it to the new hex and update its move status
            {
                currentUnit.SetMovement(currentUnit.GetMovement() - hexs[hexID].gCost);
                // Remove unit from starting hex
                Hex moveHex = hexs[currentUnit.GetHexID()];
                moveHex.currentPassiveUnit = null;
                hexs[currentUnit.GetHexID()] = moveHex;
                // Set new hexID
                currentUnit.SetHexID(hexID);
                // Add unit to hex it is moving to 
                Hex moveHex2 = hexs[hexID];
                moveHex2.currentPassiveUnit = currentUnit;
                hexs[currentUnit.GetHexID()] = moveHex2;

                if (currentUnit != null)
                {
                    changeCurrentTileCap(currentUnit, true);
                    currentUnit.SetMoved();
                }
            }
            if (currentUnit.GetUnitType() == Classes.Unit.unitType.Military)  // If the unit is Military, check for enemy units and initiate an attack if possible
            {
                if (hexs[hexID].currentMilitaryUnit != null)      // If there is an enemy unit on the target hex, check if the target hex is adjacent to the current hex
                {
                    if (hexs[currentUnit.GetHexID()].hexObj.GetComponent<TileScript>().GetConnections().Contains(hexID))
                    {
                        ProcessAttack(currentUnit, hexs[hexID].currentMilitaryUnit);
                    }
                    else // If the target hex is not reachable, try to find a parent hex which is reachable
                    {
                        MoveUnit(hexs[hexID].parent);
                    }
                }
                else // If there is no enemy unit on the target hex, move the unit to the new hex and update its move status
                {
                    // Remove unit from starting hex
                    Hex moveHex = hexs[currentUnit.GetHexID()];
                    moveHex.currentMilitaryUnit = null;
                    hexs[currentUnit.GetHexID()] = moveHex;
                    // Set new hexID
                    currentUnit.SetHexID(hexID);
                    // Add unit to hex it is moving to 
                    Hex moveHex2 = hexs[hexID];
                    moveHex2.currentMilitaryUnit = currentUnit;
                    hexs[currentUnit.GetHexID()] = moveHex2;
                    currentUnit.SetMovement(currentUnit.GetMovement() - hexs[hexID].gCost);
                }

                if (currentUnit != null)
                {
                    changeCurrentTileCap(currentUnit, true);
                    currentUnit.SetMoved();
                }
            }
        }
    }
    /// <summary>
    /// Processes an attack between an attacking unit and a defending unit
    /// </summary>
    /// <brief>
    /// The attacking unit's HP is reduced by the defending unit's defense, and the defending unit's HP is reduced by the attacking unit's attack.
    /// If the defending unit's HP reaches 0 or below, it is killed.
    /// If the attacking unit's HP reaches 0 or below, it is killed.
    /// Updates the HP bar for both units after the attack.
    /// </brief>
    /// <param name="AttackingUnit">The unit that is attacking</param>
    /// <param name="DefendingUnit">The unit that is defending</param>
    public void ProcessAttack(Classes.Unit AttackingUnit, Classes.Unit DefendingUnit)
    {
        AttackingUnit.SetHP(AttackingUnit.GetHP() - DefendingUnit.GetDefense()); // Reduce attacking unit's HP by the defending unit's defense
        DefendingUnit.SetHP(DefendingUnit.GetHP() - AttackingUnit.GetAttack()); // Reduce defending unit's HP by the attacking unit's attack
        UpdateHPBar(AttackingUnit, DefendingUnit); // Update the health bar UI for both units
        // Check if the defending unit's HP is 0 or less, and if so, kill it
        if (DefendingUnit.GetHP() <= 0) 
        {
            Kill(DefendingUnit);
        }
        // Check if the attacking unit's HP is 0 or less, and if so, kill it
        if (AttackingUnit.GetHP() <= 0)
        {
            Kill(AttackingUnit);
        }
        Debug.Log("Attacking processed, Attacking unit HP " + AttackingUnit.GetHP() + " : Defending unit HP " + DefendingUnit.GetHP());
    }
    // Overloaded function
    /// <summary>
    /// Update the health bar of the current unit
    /// </summary>
    public void UpdateHPBar()
    {
        hpBar.GetComponent<HealthBarScript>().SetHealth(currentUnit.GetHP(), currentUnit.GetMaxHP());
    }
    /// <summary>
    /// Update the health bar of the current unit and two units healthbars
    /// </summary>
    /// <param name="unit1">The first unit to update the health bar for</param>
    /// <param name="unit2">The second unit to update the health bar for</param>
    public void UpdateHPBar(Classes.Unit unit1, Classes.Unit unit2)
    {
        hpBar.GetComponent<HealthBarScript>().SetHealth(currentUnit.GetHP(), currentUnit.GetMaxHP()); // Set the health bar for the current unit
        unit1.GetHPBarGameObject().GetComponent<HealthBarScript>().SetHealth(unit1.GetHP(), unit1.GetMaxHP()); // Updates the health bar of the first unit
        unit2.GetHPBarGameObject().GetComponent<HealthBarScript>().SetHealth(unit2.GetHP(), unit2.GetMaxHP()); // Updates the health bar of the second unit
    }
    /// <summary>
    /// Uses Dijkstra's to get distances to each tile based on cost
    /// </summary>
    /// <param name="changeUnit">Unit which the focus is switching to</param>
    /// <param name="boolean">Whether to set the tilecap gameobject on or off</param>
    public void changeCurrentTileCap(Classes.Unit changeUnit, bool boolean)
    {
        // Initialize the open and closed lists
        List<int> openList = new List<int>();
        HashSet<int> closedList = new HashSet<int>();

        // Add the start node to the open list
        openList.Add(changeUnit.GetHexID());

        // Initialize the cost of the start node
        Hex changeHex = hexs[openList[0]];
        changeHex.gCost = 0;
        hexs[openList[0]] = changeHex;

        // Keep track of all nodes within the maximum cost
        HashSet<int> nodesWithinCost = new HashSet<int>();

        while (openList.Count > 0)
        {
            // Get the node with the lowest g cost
            int current = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (hexs[openList[i]].gCost < hexs[current].gCost)
                {
                    current = openList[i];
                }
            }
            // Set cap colour to green for the current unit
            if (current == changeUnit.GetHexID())
            {
                hexs[current].hexObj.GetComponent<TileScript>().SetCapCol(2);
            }
            // Check if the current node is within the maximum cost
            if (hexs[current].gCost <= changeUnit.GetMovement())
            {
                // Add the current node to the list of nodes within the maximum cost
                nodesWithinCost.Add(current);

                // Remove the current node from the open list and add it to the closed list
                openList.Remove(current);
                closedList.Add(current);
                if (current != currentUnit.GetHexID() && hexs[current].currentMilitaryUnit != null)
                {
                    continue;
                }

                // Loop through each neighbor of the current node
                foreach (int neighbor in hexs[current].hexObj.GetComponent<TileScript>().GetConnections())
                {
                    if (neighbor == changeUnit.GetHexID())
                    {
                        hexs[neighbor].hexObj.GetComponent<TileScript>().SetCapCol(2);
                    }
                    // Check if the neighbor is in the closed list
                    if (closedList.Contains(neighbor))
                    {
                        continue;
                    }
                    if (changeUnit.GetUnitType() == Classes.Unit.unitType.Passive && hexs[neighbor].currentPassiveUnit != null)
                    {
                        continue;
                    }
                    if (changeUnit.GetUnitType() == Classes.Unit.unitType.Military && hexs[neighbor].currentMilitaryUnit != null && hexs[neighbor].currentMilitaryUnit.GetID() != currentUnit.GetID())
                    {
                        hexs[neighbor].hexObj.GetComponent<TileScript>().SetCapCol(1);
                    }
                    else
                    {
                        hexs[neighbor].hexObj.GetComponent<TileScript>().SetCapCol(0);
                    }
                    if ((hexs[neighbor].hexTexture == hexType.Water || hexs[neighbor].hexTexture == hexType.DeepWater) && currentUnit.GetAquatic() == false)
                    {
                        continue;
                    }
                    // Calculate the tentative g cost for the neighbor
                    float tentativeGCost = hexs[current].gCost + hexs[neighbor].moveCost;

                    // Check if the neighbor is not in the open list
                    if (!openList.Contains(neighbor))
                    {
                        // Add the neighbor to the open list
                        openList.Add(neighbor);
                    }
                    else if (tentativeGCost >= hexs[neighbor].gCost)
                    {
                        continue;
                    }
                    // Update the neighbor's g cost and parent
                    Hex newChangeHex = hexs[neighbor];
                    newChangeHex.gCost = tentativeGCost;
                    newChangeHex.parent = current;
                    hexs[neighbor] = newChangeHex;
                }
            }
            else
            {
                // If the current node is not within the maximum cost, break the loop
                break;
            }
        }
        changeUnit.SetMoveableTiles(nodesWithinCost);
        foreach (int index in nodesWithinCost)
        {
            // Set tile cap to on or off
            hexs[index].hexObj.GetComponent<TileScript>().SetCap(boolean);
        }
    }
    float Heuristic(int a, int b)
    {
        // Calculate the Manhattan distance between the nodes
        float dx = Mathf.Abs(hexs[a].pos.x - hexs[b].pos.x);
        float dy = Mathf.Abs(hexs[a].pos.y - hexs[b].pos.y);

        // Handle the wrap-around behavior for the x-axis
        if (dx > (width * 1.66666631576f) / 2)
        {
            dx = (width * 1.66666631576f) - dx;
        }

        // Handle the wrap-around behavior for the y-axis
        if (dy > (height * 1.44337567298f) / 2)
        {
            dy = (height * 1.44337567298f) - dy;
        }

        // Return the modified Manhattan distance as the heuristic value
        return dx + dy;
    }
    /// <summary>
    /// Sets the connections for the tile at a specified index
    /// </summary>
    /// <param name="_index">The index of the tile</param>
    public void SetConnections(int _index)
    {            
        // Middle Right
        if (_index + width >= (width * height))
        {
            hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + width - (width * height));
        }
        else
        {
            hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + width);
        }
        // Middle Left
        if (_index - width <= -1)
        {
            hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - width + (width * height));
        }
        else
        {
            hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - width);
        }
        // If tile is on the bottom most row
        if (_index % width == 0)
        {
            // Up
            hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + 1);
            // Down can't go down, so much go to top row
            hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - 1 + width);

            if (hexs[_index].XY.x % 2 == 0)
            {
                // Bottom Right
                if (_index + (2 * width) - 1 >= (width * height))
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + (2 * width) - 1 - (width * height));
                }
                else
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + (2 * width) - 1);
                }
                // Bottom Left
                if (_index - 1 <= -1)
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - 1 + (width * height));
                }
                else
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - 1);
                }
            }
            else
            {
                // Top Right
                if (_index + width + 1 >= (width * height))
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + width + 1 - (width * height));
                }
                else
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + width + 1);
                }
                // Top Left
                if (_index - width + 1 <= -1)
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - width + 1 + (width * height));
                }
                else
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - width + 1);
                }
            }
        }
        // If tile is on the top most row
        else if ((_index + 1) % width == 0)
        {
            // Up
            hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - width + 1);
            // Down
            hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - 1);

            if (hexs[_index].XY.x % 2 == 0)
            {
                // Bottom right
                if (_index + width - 1 >= (width * height))
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + width - 1 - (width * height));
                }
                else
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + width - 1);
                }
                // Bottom Left
                if (_index - width - 1 <= -1)
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - width - 1 + (width * height));
                }
                else
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - width - 1);
                }
            }
            else
            {
                // Top Right
                if (_index + 1 >= (width * height))
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + 1 - (width * height));
                }
                else
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + 1);
                }
                //Top Left
                if (_index + 1 >= (width * height))
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(0);
                }
                else
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - (2 * width) + 1);
                }
            }
        }
        else
        {
            // Up
            if (_index + 1 == (width * height))
            {
                hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(0);
            }
            else
            {
                hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + 1);
            }
            // Down
            if (_index == 0)
            {
                hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + width - 1);
            }
            else
            {
                hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - 1);
            }


            if (hexs[_index].XY.x % 2 == 0)
            {
                if (_index + width - 1 >= (width * height))
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + width - 1 - (width * height));
                }
                else
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + width - 1);
                }
                if (_index - width - 1 <= -1)
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - width - 1 + (width * height));
                }
                else
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - width - 1);
                }
            }
            else
            {
                if (_index + width + 1 >= (width * height))
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + width + 1 - (width * height));
                }
                else
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index + width + 1);
                }
                if (_index - width + 1 <= -1)
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - width + 1 + (width * height));
                }
                else
                {
                    hexs[_index].hexObj.GetComponent<TileScript>().SetConnection(_index - width + 1);
                }
            }
        }

    }
    /// <summary>
    /// Ends the users turn via button press
    /// </summary>
    public void endTurn()
    {
        turn += 1;
        foreach (Classes.Player curPlay in myPlayers)
        {
            ResetUnits(curPlay);
            CollectYields(curPlay);
        }
    }

    //----------Methods to create units----------//

    /// <summary>
    /// Creates a new Settler unit at the specified spawningCity location.
    /// </summary>
    /// <param name="spawningCity">The city that the Settler will be created at</param>
    /// <returns>Returns true if the Settler was created successfully, false otherwise.</returns>
    public bool CreateSettler(Classes.Unit spawningCity)
    {
        // Check if there is no passive unit already on the spawning city hex
        if (hexs[spawningCity.GetHexID()].currentPassiveUnit == null)
        {
            // Create a new PassiveUnit object
            Classes.PassiveUnit newUnit = new Classes.PassiveUnit();
            // Set the new unit's ID and increment the global unit ID
            newUnit.SetUnitID(globalUnitID);
            globalUnitID += 1;

            // Set unit's attributes
            newUnit.SetMaxHP(1);
            newUnit.SetHP(1);
            newUnit.SetMovement(2);
            newUnit.SetMaxMovement(2);

            // create settler game object
            newUnit.SetGameObject(Instantiate(myPassiveUnits[0], spawningCity.GetGameObject().transform.position, rotation) as GameObject);
            newUnit.GetGameObject().transform.SetParent(passiveUnitParent.transform);
            newUnit.SetHexID(spawningCity.GetHexID());
            newUnit.SetPassiveUnit(Classes.Unit.passiveUnits.Settler);

            // Create settler billboard
            GameObject settlerBillboard = Instantiate(unitBillboard, new Vector3(newUnit.GetGameObject().transform.position.x, newUnit.GetGameObject().transform.position.y + 4.0f, newUnit.GetGameObject().transform.position.z), Quaternion.identity);
            settlerBillboard.transform.GetComponent<SpriteRenderer>().sprite = myPassiveUnitIconsCircles[0];
            settlerBillboard.transform.SetParent(billboardParent.transform);
            Vector3 playerColour = newUnit.GetGameObject().GetComponent<OutLineScript>().GetColour(currentPlayer.GetLeader());
            settlerBillboard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(playerColour.x, playerColour.y, playerColour.z, 0.5f);
            newUnit.SetBillboardGameObject(settlerBillboard);

            // Create the Settler HPBar
            GameObject settlerHPBar = Instantiate(hpBar, new Vector3(newUnit.GetGameObject().transform.position.x, newUnit.GetGameObject().transform.position.y, newUnit.GetGameObject().transform.position.z), Quaternion.identity);
            settlerHPBar.GetComponent<HealthBarScript>().SetHealth(newUnit.GetHP(), newUnit.GetMaxHP());
            settlerHPBar.transform.SetParent(settlerBillboard.transform);
            settlerHPBar.transform.localScale = new Vector3(0.055f, 0.05f, 0.01f);
            settlerHPBar.transform.localPosition += new Vector3(0.0f, 31f, 0.0f);
            newUnit.SetHPBarGameObject(settlerHPBar);

            // Update UI information
            ChangeBottomUI(newUnit);
            changeCurrentUnit(newUnit);
            currentPlayer.AddPassiveUnit(newUnit);

            // Set the current passive unit on the hex to the new unit
            Hex currentHex = hexs[newUnit.GetHexID()];
            currentHex.currentPassiveUnit = newUnit;
            hexs[newUnit.GetHexID()] = currentHex;

            // Return true to indicate that the Settler was created successfully
            return true;
        }
        return false;
    }

    /// <summary>
    /// Creates a new Scout unit at the specified spawningCity location.
    /// </summary>
    /// <param name="spawningCity">The city that the Scout will be created at</param>
    /// <returns>Returns true if the Scout was created successfully, false otherwise.</returns>
    public bool CreateScout(Classes.Unit spawningCity)
    {
        // Check if there is no military unit already on the spawning city hex
        if (hexs[spawningCity.GetHexID()].currentMilitaryUnit == null)
        {
            // Create a new MilitaryUnit object, set unit type and set new units ID
            Classes.MilitaryUnit newUnit = new Classes.MilitaryUnit();
            newUnit.SetMilitaryUnit(Classes.Unit.militaryUnits.Scout);
            newUnit.SetUnitID(globalUnitID);
            globalUnitID += 1; // Increment the global unit ID

            // Set the new unit's attributes
            newUnit.SetDefense(2);
            newUnit.SetAttack(2);
            newUnit.SetHP(10);
            newUnit.SetMaxHP(10);
            newUnit.SetMovement(3);
            newUnit.SetMaxMovement(3);

            // Create scout game object
            newUnit.SetGameObject(Instantiate(myMilitaryUnits[0], spawningCity.GetGameObject().transform.position, rotation) as GameObject);
            newUnit.GetGameObject().transform.SetParent(militaryUnitParent.transform);
            newUnit.GetGameObject().name = "Scout";
            newUnit.SetHexID(spawningCity.GetHexID());

            // Create billboard object
            GameObject scoutBillboard = Instantiate(unitBillboard, new Vector3(newUnit.GetGameObject().transform.position.x, newUnit.GetGameObject().transform.position.y + 4.0f, newUnit.GetGameObject().transform.position.z), Quaternion.identity);
            scoutBillboard.transform.GetComponent<SpriteRenderer>().sprite = myMilitaryUnitIconsCircles[0];
            scoutBillboard.transform.SetParent(billboardParent.transform);
            Vector3 playerColour = newUnit.GetGameObject().GetComponent<OutLineScript>().GetColour(currentPlayer.GetLeader());
            scoutBillboard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(playerColour.x, playerColour.y, playerColour.z, 0.5f);
            newUnit.SetBillboardGameObject(scoutBillboard);

            // Create Scout HPBar
            GameObject scoutHPBar = Instantiate(hpBar, new Vector3(newUnit.GetGameObject().transform.position.x, newUnit.GetGameObject().transform.position.y, newUnit.GetGameObject().transform.position.z), Quaternion.identity);
            scoutHPBar.GetComponent<HealthBarScript>().SetHealth(newUnit.GetHP(), newUnit.GetMaxHP());
            scoutHPBar.transform.SetParent(scoutBillboard.transform);
            scoutHPBar.transform.localScale = new Vector3(0.055f, 0.05f, 0.01f);
            scoutHPBar.transform.localPosition += new Vector3(0.0f, 31f, 0.0f);
            newUnit.SetHPBarGameObject(scoutHPBar);

            // Update UI information
            ChangeBottomUI(newUnit);
            changeCurrentUnit(newUnit);
            currentPlayer.AddMilitaryUnit(newUnit);

            // Set the current military unit on the hex to the new unit
            Hex currentHex = hexs[newUnit.GetHexID()];
            currentHex.currentMilitaryUnit = newUnit;
            hexs[newUnit.GetHexID()] = currentHex;
            return true;
        }
        return false;
    }
    /// <summary>
    /// Creates a new Axeman unit at the specified spawningCity location.
    /// </summary>
    /// <param name="spawningCity">The city that the Axeman will be created at</param>
    /// <returns>Returns true if the Axeman was created successfully, false otherwise.</returns>
    public bool CreateAxeman(Classes.Unit spawningCity)
    {
        if (hexs[spawningCity.GetHexID()].currentMilitaryUnit == null)
        {
            // Create a new MilitaryUnit object, set unit type and set new units ID
            Classes.MilitaryUnit newUnit = new Classes.MilitaryUnit();
            newUnit.SetMilitaryUnit(Classes.Unit.militaryUnits.Axeman);
            newUnit.SetUnitID(globalUnitID);
            globalUnitID += 1;

            // Set units attributes
            newUnit.SetDefense(5);
            newUnit.SetAttack(5);
            newUnit.SetHP(15);
            newUnit.SetMaxHP(15);
            newUnit.SetMovement(2);
            newUnit.SetMaxMovement(2);

            // Create axeman game object
            newUnit.SetGameObject(Instantiate(myMilitaryUnits[1], spawningCity.GetGameObject().transform.position, rotation) as GameObject);
            newUnit.GetGameObject().transform.SetParent(militaryUnitParent.transform);
            newUnit.GetGameObject().name = "Axeman";
            newUnit.SetHexID(spawningCity.GetHexID());

            // Create billboard object
            GameObject axemanBillboard = Instantiate(unitBillboard, new Vector3(newUnit.GetGameObject().transform.position.x, newUnit.GetGameObject().transform.position.y + 4.0f, newUnit.GetGameObject().transform.position.z), Quaternion.identity);
            axemanBillboard.transform.GetComponent<SpriteRenderer>().sprite = myMilitaryUnitIconsCircles[1];
            Vector3 playerColour = newUnit.GetGameObject().GetComponent<OutLineScript>().GetColour(currentPlayer.GetLeader());
            axemanBillboard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(playerColour.x, playerColour.y, playerColour.z, 0.5f);
            newUnit.SetBillboardGameObject(axemanBillboard);

            // Create Axeman HPBar
            GameObject axemanHPBar= Instantiate(hpBar, new Vector3(newUnit.GetGameObject().transform.position.x, newUnit.GetGameObject().transform.position.y, newUnit.GetGameObject().transform.position.z), Quaternion.identity);
            axemanHPBar.GetComponent<HealthBarScript>().SetHealth(newUnit.GetHP(), newUnit.GetMaxHP());
            axemanHPBar.transform.SetParent(axemanBillboard.transform);
            axemanHPBar.transform.localScale = new Vector3(0.055f, 0.05f, 0.01f);
            axemanHPBar.transform.localPosition += new Vector3(0.0f, 31f, 0.0f);
            newUnit.SetHPBarGameObject(axemanHPBar);

            // Update UI information
            ChangeBottomUI(newUnit);
            changeCurrentUnit(newUnit);
            currentPlayer.AddMilitaryUnit(newUnit);

            // Set the current military unit on the hex to the new unit
            Hex currentHex = hexs[newUnit.GetHexID()];
            currentHex.currentMilitaryUnit = newUnit;
            hexs[newUnit.GetHexID()] = currentHex;
            return true;
        }
        return false;
    }
}
