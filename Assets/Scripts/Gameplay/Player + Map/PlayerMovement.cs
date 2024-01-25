/**
 *@file PlayerMovement.cs
* @brief This file contains the implementation of the PlayerMovement class.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * @brief This class handles player movement within the game world.
 */
public class PlayerMovement : MonoBehaviour
{
    Queue<Vector2> mousePositions; // stores recent mouse positions as 2D vectors
    List<Hex> hexs; // a list of Hex objects
    Vector2 mouseMovement; // tracks the current mouse movement as a 2D vector
    bool inMenu = false; // indicates whether the player is in the Esc Menu or not
    bool isOverMap = false; // indicates whether the mouse is currently over the map or not
    bool isOverTopUI = false; // indicates whether the mouse is currently over the top UI or not
    bool terrainOnly = false; // indicates whether the mouse is currently over the terrain or not
    public GameObject escMenu; // a reference to the Esc Menu game object
    public GameObject[] menus; // a reference to the Esc Menu game object
    public GameObject map; // a reference to the Map game object
    public GameObject trapezium; // a reference to the Trapezium game object
    public GameObject cityMap; // a reference to the Trapezium game object
    public GameObject terrain; // a reference to the Terrain game object
    Vector2 currentTilePos; // the position of the current tile as a 2D vector
    Vector2 lastKnownUnit; // the last known position of the selected unit as a 2D vector
    HashSet<int> previousHashSet; // a hash set of integers representing previous actions
    int moveSpeed = 20; // the movement speed as an integer
    int height; // the height of the map as an integer
    int width; // the width of the map as an integer
    int zoomLevel; // the current zoom level as an integer
    int currentScale = 1; // the current scale as an integer
    float accumX; // the accumulated x-axis value as a float
    float accumY; // the accumulated y-axis value as a float
    public bool isClicking1; // indicates whether the player is currently clicking the mouse or not
    public bool isDragging1; // indicates whether the player is currently dragging the mouse or not
    public bool isClicking2; // indicates whether the player is currently clicking the mouse or not
    public bool isDragging2; // indicates whether the player is currently dragging the mouse or not

    private void Awake()
    {
        zoomLevel = 6;
        previousHashSet = new HashSet<int>();
    }
    private void Start()
    {
        height = terrain.GetComponent<CreateHex>().height;
        width = terrain.GetComponent<CreateHex>().width;
        escMenu.SetActive(false);
        // Instantiate Lists, Vectors, structs and variables
        mousePositions = new Queue<Vector2>();
        currentTilePos = new Vector2(150.1111f, 150);
        hexs = terrain.gameObject.GetComponent<CreateHex>().hexs;
    }

    void Update()
    {

        // Keyboard inputs
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            flipMenu();
        }

        // Check mouse position and change in position
        checkMousePosInfo();

        // Change zoom level of game/map
        zoom();

        lastKnownUnit = currentTilePos;
        // Check if camera is above a new hexagon
        // x
        if (transform.position.x - currentTilePos.x > moveSpeed * Mathf.Sqrt(3) / 6)
        {
            currentTilePos.x += moveSpeed * Mathf.Sqrt(3) / 6;
        }
        if (transform.position.x - currentTilePos.x < -moveSpeed * Mathf.Sqrt(3) / 6 )
        {
            currentTilePos.x -= moveSpeed * Mathf.Sqrt(3) / 6;
        }
        // y
        if (transform.position.z - currentTilePos.y > moveSpeed / 3)
        {
            currentTilePos.y += moveSpeed / 3;
        }
        if (transform.position.z - currentTilePos.y < -moveSpeed / 3)
        {
            currentTilePos.y -= moveSpeed / 3;
        }
        if (lastKnownUnit != currentTilePos)
        {
            lastKnownUnit = currentTilePos;
        }

        // Change active tiles
        changeActive();
        // Keep map within bounds of mask
        CenterMap();
    }




    void changeActive()
    {
        int h = 35;
        int w = 50;
        Vector2Int tempVec = new Vector2Int((int)Mathf.Round(currentTilePos.x / 1.44337567298f), (int)Mathf.Round(currentTilePos.y / 1.66666631576f));
        GameObject.Find("Trapezium").GetComponent<DrawTrapezium>().Draw(tempVec * 2, zoomLevel);
        HashSet<int> currentHashSet = new HashSet<int>();
        int xMultiply = 0;
        int yMultiply = 0;
        int addY = 0;
        // For each tile on the screen
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                int currentI =  12 + tempVec.x + i - h / 2;
                int currentJ =  tempVec.y + j - w / 2;

                while (currentI < 0)
                {
                    currentI += height;
                    xMultiply += 1;
                }
                while(currentJ < 0)
                {
                    currentJ += width;
                    yMultiply += 1;
                }
                while (currentI >= height)
                {
                    currentI -= height;
                    xMultiply -= 1;
                }
                while (currentJ >= width)
                {
                    currentJ -= width;
                    yMultiply -= 1;
                }
                int currentIndex = currentI * width + currentJ;
                currentHashSet.Add(currentIndex);
                hexs[currentIndex].hexObj.transform.position = new Vector3(hexs[currentIndex].pos.x - (height * 1.44337567298f * xMultiply), hexs[currentIndex].pos.y, hexs[currentIndex].pos.z - (width * 1.66666631576f * yMultiply));
                if (hexs[currentIndex].hexResource != resourceType.None)
                {
                    hexs[currentIndex].resourceObject.transform.position = new Vector3(hexs[currentIndex].hexObj.transform.position.x, hexs[currentIndex].resourceObject.transform.position.y, hexs[currentIndex].hexObj.transform.position.z);
                }
                if (hexs[currentIndex].currentPassiveUnit != null)
                {
                    hexs[currentIndex].currentPassiveUnit.GetGameObject().transform.position = hexs[currentIndex].hexObj.transform.position;
                    float yadd = (transform.position.y - 10) / 125;
                    if (hexs[currentIndex].currentPassiveUnit.GetID() == terrain.GetComponent<CreateHex>().GetCurrentID())
                    {
                        hexs[currentIndex].currentPassiveUnit.GetBillboardGameObject().transform.localScale = new Vector3(0.06f + yadd, 0.06f + yadd, 1f);
                    }
                    else
                    {
                        hexs[currentIndex].currentPassiveUnit.GetBillboardGameObject().transform.localScale = new Vector3(0.05f + yadd, 0.05f + yadd, 1f);
                    }
                    hexs[currentIndex].currentPassiveUnit.GetBillboardGameObject().transform.position = new Vector3(hexs[currentIndex].hexObj.transform.position.x, hexs[currentIndex].hexObj.transform.position.y + 4.0f, hexs[currentIndex].hexObj.transform.position.z);
                }
                if (hexs[currentIndex].forestTile != null)
                {
                    hexs[currentIndex].forestTile.transform.position = hexs[currentIndex].hexObj.transform.position;
                }
                if (hexs[currentIndex].currentTileBuilding != null)
                {
                    hexs[currentIndex].currentTileBuilding.GetGameObject().transform.position = hexs[currentIndex].hexObj.transform.position;
                    float yadd = (transform.position.y - 10) / 20;
                    if (hexs[currentIndex].currentTileBuilding.GetID() == terrain.GetComponent<CreateHex>().GetCurrentID())
                    {
                        hexs[currentIndex].currentTileBuilding.GetBillboardGameObject().transform.localScale = new Vector3(0.24f * (1 + yadd), 0.06f * (1 + yadd), 1f);
                    }
                    else
                    {
                        hexs[currentIndex].currentTileBuilding.GetBillboardGameObject().transform.localScale = new Vector3(0.2f * (1 + yadd), 0.05f * (1 + yadd), 1f);
                    }
                    hexs[currentIndex].currentTileBuilding.GetBillboardGameObject().transform.position = new Vector3(hexs[currentIndex].hexObj.transform.position.x, hexs[currentIndex].hexObj.transform.position.y + 2.5f, hexs[currentIndex].hexObj.transform.position.z);
                }
                if (hexs[currentIndex].currentMilitaryUnit != null)
                {
                    hexs[currentIndex].currentMilitaryUnit.GetGameObject().transform.position = hexs[currentIndex].hexObj.transform.position;
                    if (hexs[currentIndex].currentPassiveUnit != null)
                    {
                        addY = 2;
                    }
                    else
                    {
                        addY = 0;
                    }
                    float yadd = (transform.position.y - 10) / 125;
                    if (hexs[currentIndex].currentMilitaryUnit.GetID() == terrain.GetComponent<CreateHex>().GetCurrentID())
                    {
                        hexs[currentIndex].currentMilitaryUnit.GetBillboardGameObject().transform.localScale = new Vector3(0.06f + yadd, 0.06f + yadd, 1f);
                    }
                    else
                    {
                        hexs[currentIndex].currentMilitaryUnit.GetBillboardGameObject().transform.localScale = new Vector3(0.05f + yadd, 0.05f + yadd, 1f);
                    }
                    hexs[currentIndex].currentMilitaryUnit.GetBillboardGameObject().transform.position = new Vector3(hexs[currentIndex].hexObj.transform.position.x, hexs[currentIndex].hexObj.transform.position.y + 4.0f + addY, hexs[currentIndex].hexObj.transform.position.z);
                }
                xMultiply = 0;
                yMultiply = 0;
            }
            //currentI >= 0 && currentI <= width - 2 && currentJ >= 0 && currentJ <= height - 2
        }
        //Debug.Log(tempVec + " : " + width);
        foreach (int currentInt in currentHashSet)
        {
            if (!previousHashSet.Contains(currentInt))
            {
                // If there is an index in the current hash set but not in the previous one, we want to activate the tile at the index
                hexs[currentInt].hexObj.SetActive(true);
                //hexs[currentInt].hexObj
                if (hexs[currentInt].hexResource != resourceType.None)
                {
                    hexs[currentInt].resourceObject.SetActive(true);
                }
                if (hexs[currentInt].currentPassiveUnit != null)
                {
                    hexs[currentInt].currentPassiveUnit.GetGameObject().SetActive(true);
                    hexs[currentInt].currentPassiveUnit.GetBillboardGameObject().SetActive(true);
                }
                if (hexs[currentInt].forestTile != null)
                {
                    hexs[currentInt].forestTile.SetActive(true);
                }
                if (hexs[currentInt].currentTileBuilding != null)
                {
                    hexs[currentInt].currentTileBuilding.GetGameObject().SetActive(true);
                    hexs[currentInt].currentTileBuilding.GetBillboardGameObject().SetActive(true);
                }
                if (hexs[currentInt].currentMilitaryUnit != null)
                {
                    hexs[currentInt].currentMilitaryUnit.GetGameObject().SetActive(true);
                    hexs[currentInt].currentMilitaryUnit.GetBillboardGameObject().SetActive(true);
                }
            }
        }
        foreach (int currentInt in previousHashSet)
        {
            if (!currentHashSet.Contains(currentInt))
            {
                // If there is an index in the previous hash set but not in the current one, we want to de-activate the tile at the index
                if (hexs[currentInt].hexResource != resourceType.None)
                {
                    hexs[currentInt].resourceObject.SetActive(false);
                }
                if (hexs[currentInt].currentPassiveUnit != null)
                {
                    hexs[currentInt].currentPassiveUnit.GetGameObject().SetActive(false);
                    hexs[currentInt].currentPassiveUnit.GetBillboardGameObject().SetActive(false);
                }
                if (hexs[currentInt].forestTile != null)
                {
                    hexs[currentInt].forestTile.SetActive(false);
                }
                if (hexs[currentInt].currentTileBuilding != null)
                {
                    hexs[currentInt].currentTileBuilding.GetGameObject().SetActive(false);
                    hexs[currentInt].currentTileBuilding.GetBillboardGameObject().SetActive(false);
                }
                if (hexs[currentInt].currentMilitaryUnit != null)
                {
                    hexs[currentInt].currentMilitaryUnit.GetGameObject().SetActive(false);
                    hexs[currentInt].currentMilitaryUnit.GetBillboardGameObject().SetActive(false);
                }
                hexs[currentInt].hexObj.SetActive(false);
            }
        }
        previousHashSet = currentHashSet;
    }

    int IsMouseOverMap()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);


        if (raycastResults.Count > 0)
        {
            foreach (var go in raycastResults)
            {
                return go.gameObject.layer;
            }
        }
        return 0;
    }

    void scrlWheelMovePositive(int num)
    {
        // Zoom
        transform.position += new Vector3(-Mathf.Sin(num), -Mathf.Cos(num), 0);
    }

    void scrlWheelMoveNegative(int num)
    {
        // Zoom
        transform.position += new Vector3(Mathf.Sin(num), Mathf.Cos(num), 0);
    }

    public void flipMenu()
    {
        escMenu.SetActive(!escMenu.activeSelf);
        foreach (GameObject menu in menus)
        {
            if(menu.activeSelf)
            {
                escMenu.SetActive(false);
            }
            menu.SetActive(false);
        }
        inMenu = escMenu.activeSelf;
    }


    void moveCameraMap()
    {
        // Store second last mouse position
        mouseMovement = mousePositions.Peek();
        // Remove last mouse position
        mousePositions.Dequeue();
        // Mouse movement is second last mouse position - last mouse position
        mouseMovement -= mousePositions.Peek();
        if (!isOverMap && !isOverTopUI)
        {
            // Move camera in opposite direction of mouse movement
            transform.position += new Vector3(mouseMovement.y * 0.075f * Mathf.Pow(transform.position.y, 1.2f), 0, -mouseMovement.x * 0.075f * Mathf.Pow(transform.position.y, 1.2f));
        }
        if (isOverMap)
        {
            // Move map within bounds of the mask
            accumX = 0;
            accumY = 0;
            if (isDragging1)
            {
                for (int i = 0; i < currentScale - 1; i++)
                {
                    accumX += 19.2f * Mathf.Pow(1.1f, i);
                    accumY += 10.2f * Mathf.Pow(1.1f, i);
                }
                //x
                if (map.transform.localPosition.x - mouseMovement.x * 100 < accumX && map.transform.localPosition.x - mouseMovement.x * 100 > -accumX)
                {
                    map.transform.localPosition -= new Vector3(mouseMovement.x * 100, 0, 0);
                    cityMap.transform.localPosition -= new Vector3(mouseMovement.x * 100, 0, 0);
                    trapezium.transform.localPosition -= new Vector3(mouseMovement.x * 100, 0, 0);
                }

                //y
                if (map.transform.localPosition.y - mouseMovement.y * 100 > -accumY && map.transform.localPosition.y - mouseMovement.y * 100 < accumY)
                {
                    map.transform.localPosition -= new Vector3(0, mouseMovement.y * 100, 0);
                    cityMap.transform.localPosition -= new Vector3(0, mouseMovement.y * 100, 0);
                    trapezium.transform.localPosition -= new Vector3(0, mouseMovement.y * 100, 0);
                }
            }
        }
    }

    void zoom()
    {
        if (!isOverMap && !isOverTopUI)
        {
            // Zoom in/out
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (transform.position.y > 10)
                {
                    zoomLevel -= 2;
                    // Zoom in
                    scrlWheelMoveNegative(3);
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (transform.position.y < 25)
                {
                    zoomLevel += 2;
                    // Zoom out
                    scrlWheelMovePositive(3);
                }
            }
        }
        // Zooom in and out of map
        if (isOverMap)
        {
            //Debug.Log(map.transform.localPosition);
            //Debug.Log((Input.mousePosition.x - 1520 - 193) + ", " + (Input.mousePosition.y - 15 - 108));
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (currentScale < 15)
                {
                    map.transform.localPosition = new Vector3(map.transform.localPosition.x - ((Input.mousePosition.x - 1713) / (2 * map.transform.localScale.x)), map.transform.localPosition.y - ((Input.mousePosition.y - 123) / (2 * map.transform.localScale.x)), 0);
                    trapezium.transform.localPosition = map.transform.localPosition;
                    cityMap.transform.localPosition = map.transform.localPosition;
                    map.transform.localScale *= 1.1f;
                    trapezium.transform.localScale *= 1.1f;
                    cityMap.transform.localScale *= 1.1f;
                    currentScale += 1;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (currentScale > 1)
                {
                    map.transform.localScale /= 1.1f;
                    cityMap.transform.localScale /= 1.1f;
                    trapezium.transform.localScale /= 1.1f;
                    map.transform.localPosition = new Vector3(map.transform.localPosition.x - ((Input.mousePosition.x - 1713) / (2 * map.transform.localScale.x)), map.transform.localPosition.y - ((Input.mousePosition.y - 123) / (2 * map.transform.localScale.x)), 0);
                    trapezium.transform.localPosition = map.transform.localPosition;
                    cityMap.transform.localPosition = map.transform.localPosition;
                    currentScale -= 1;
                }
            }
        }
    }
    /// <summary>
    /// Centers the map within the viewport of the game screen
    /// </summary>
    void CenterMap()
    {
        // Initialize the x and y accumulators to 0
        accumX = 0;
        accumY = 0;
        for (int i = 0; i < currentScale - 1; i++) // Calculate the accumulated x and y positions for each level of zoom
        {
            accumX += 19.2f * Mathf.Pow(1.1f, i);
            accumY += 10.2f * Mathf.Pow(1.1f, i);
        }
        if (map.transform.localPosition.x > accumX) // If the map's local x position is greater than the accumulated x position, reset the local x position to the accumulated x position
        {
            map.transform.localPosition = new Vector3(accumX, map.transform.localPosition.y, 0);
            trapezium.transform.localPosition = map.transform.localPosition;
            cityMap.transform.localPosition = map.transform.localPosition;
        }
        if (map.transform.localPosition.x < -accumX) // If the map's local x position is less than the negative accumulated x position, reset the local x position to the negative accumulated x position
        {
            map.transform.localPosition = new Vector3(-accumX, map.transform.localPosition.y, 0);
            trapezium.transform.localPosition = map.transform.localPosition;
            cityMap.transform.localPosition = map.transform.localPosition;
        }
        if (map.transform.localPosition.y > accumY) // If the map's local y position is greater than the accumulated y position, reset the local y position to the accumulated y position
        {
            map.transform.localPosition = new Vector3(map.transform.localPosition.x, accumY, 0);
            trapezium.transform.localPosition = map.transform.localPosition;
            cityMap.transform.localPosition = map.transform.localPosition;
        }
        if (map.transform.localPosition.y < -accumY) // If the map's local y position is less than the negative accumulated y position, reset the local y position to the negative accumulated y position
        {
            map.transform.localPosition = new Vector3(map.transform.localPosition.x, -accumY, 0);
            trapezium.transform.localPosition = map.transform.localPosition; 
            cityMap.transform.localPosition = map.transform.localPosition; 
        }
    }
    void checkMousePosInfo()
    {
        // Only change focus while mouse is up
        if (!Input.GetMouseButton(0))
        {
            if (IsMouseOverMap() == 6)
            {
                isOverMap = true;
            }
            else if (IsMouseOverMap() == 7)
            {
                isOverTopUI = true;
            }
            else
            {
                isOverMap = isOverTopUI = false;
            }
            if (IsMouseOverMap() == 0)
            {
                terrainOnly = true;
            }
            else
            {
                terrainOnly = false;
            }
        }
        // Move mouse
        if (Input.GetMouseButton(0))
        {
            Vector2 tempVec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (mousePositions.Count >= 1)
            {
                if (mousePositions.Peek() != tempVec / 100)
                {
                    isClicking1 = false;
                    isDragging1 = true;
                }
            }
            mousePositions.Enqueue(tempVec / 100);
            // if mouse moves two or more pixels while mouse 1 is pressed
            if (mousePositions.Count >= 2 && !inMenu)
            {
                moveCameraMap();
            }

            


        }

        // Right Click to move to position on map
        if (Input.GetMouseButton(1))
        {
            Vector2 tempVec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (mousePositions.Count >= 1)
            {
                if (mousePositions.Peek() != tempVec / 100)
                {
                    isClicking2 = false;
                    isDragging2 = true;
                }
            }
            isClicking2 = true; 
            if (isOverMap)
            {
                Vector2 tempVec2 = new Vector2((Input.mousePosition.x - 1510) * (width / 380f), (Input.mousePosition.y - 40) * (height / 215f));
                transform.position = new Vector3(tempVec2.y * 1.44337567298f, transform.localPosition.y, (width * 1.66666631576f) - tempVec2.x * 1.66666631576f);
            }

        }
        if (Input.GetMouseButtonUp(0))
        {

            if (isClicking1)
            {

            }
 
            isDragging1 = false;
            isClicking1 = false;
            // If mouse button one is released, clear mouse positions
            mousePositions.Clear();
        }
        if (Input.GetMouseButtonUp(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (isClicking2)
            {

            }
            if (Physics.Raycast(ray, out hit, 100) && terrainOnly)
            {
                GameObject.Find("CreateHex").GetComponent<CreateHex>().MoveUnit(hit.transform.GetComponent<TileScript>().GetIndex());
            }
            isDragging2 = false;
            isClicking2 = false;
            // If mouse button one is released, clear mouse positions
            mousePositions.Clear();
        }
    }
    Classes.Unit GetCurrentUnit()
    {
        return GameObject.Find("CreateHex").GetComponent<CreateHex>().currentPlayer;
    }
}
