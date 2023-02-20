using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    Queue<Vector2> mousePositions;
    List<Hex> hexs; 
    Vector2 mouseMovement;
    bool inMenu = false;
    bool isOverMap = false;
    bool isOverTopUI = false;
    public GameObject escMenu;
    public GameObject map;
    public GameObject trapezium;
    public GameObject terrain;
    public GameObject currentSelectedUnit;
    public float divideX;
    public float divideZ;
    Vector2 currentTilePos;
    Vector2 lastKnownUnit;
    HashSet<int> previousHashSet;
    int moveSpeed = 20;
    int height;
    int width;
    int zoomLevel;
    int currentScale = 1;
    float accumX;
    float accumY;
    int turn;

    //Properties
    Properties myProperties;

    //Yield number GameObjects
    public GameObject[] yieldTextObjects;

    private void Awake()
    {
        turn = 0;
        myProperties = new Properties();
        zoomLevel = 6;
        previousHashSet = new HashSet<int>();
        height = myProperties.height;
        width = myProperties.width;
    }
    private void Start()
    {
        escMenu.SetActive(false);
        // Instantiate Lists, Vectors, structs and variables
        mousePositions = new Queue<Vector2>();
        currentTilePos = new Vector2(150.1111f, 150);
        hexs = terrain.gameObject.GetComponent<CreateHex>().hexs;
    }

    void Update()
    {
        CastRay();

        // Keyboard inputs
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!terrain.gameObject.GetComponent<CreateHex>().CloseInfoMenu())
            {
                flipMenu();
            }
        }

        // Check mouse position and change in position
        CheckMousePosInfo();

        // Change zoom level of game/map
        Zoom();

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

    void CastRay()
    {
        //Debug.Log("hi");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }



    void changeActive()
    {
        int h = 100;
        int w = 20;
        Vector2Int tempVec = new Vector2Int((int)Mathf.Round(currentTilePos.x / (5 * Mathf.Sqrt(3) / 3)), (int)Mathf.Round(currentTilePos.y * 1.2f));
        GameObject.Find("Trapezium").GetComponent<DrawTrapezium>().Draw(tempVec * 2, zoomLevel);
        HashSet<int> currentHashSet = new HashSet<int>();
        // For each tile on the screen
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                int currentI = tempVec.y + i - h / 2;
                int currentJ = 7 + tempVec.x + j - w / 2;
                if (currentI >= 0 && currentI <= 768 - 2 && currentJ >= 0 && currentJ <= 108 - 2)
                {
                    // Add index of tile to hash set
                    currentHashSet.Add(currentI * 108 + currentJ);
                }
            }
        }
        foreach (int currentInt in currentHashSet)
        {
            if (!previousHashSet.Contains(currentInt))
            {
                // If there is an index in the current hash set but not in the previous one, we want to activate the tile at the index
                hexs[currentInt].hexObj.SetActive(true);
                if (hexs[currentInt].resourceObject != null)
                {
                    hexs[currentInt].resourceObject.SetActive(true);
                }
                if (hexs[currentInt].currentPassiveUnit != null)
                {
                    hexs[currentInt].currentPassiveUnit.SetActive(true);
                }
                if (hexs[currentInt].forestTile != null)
                {
                    hexs[currentInt].forestTile.SetActive(true);
                }
            }
        }
        foreach (int currentInt in previousHashSet)
        {
            if (!currentHashSet.Contains(currentInt))
            {
                // If there is an index in the previous hash set but not in the current one, we want to de-activate the tile at the index
                hexs[currentInt].hexObj.SetActive(false);
                if (hexs[currentInt].resourceObject != null)
                {
                    hexs[currentInt].resourceObject.SetActive(false);
                }
                if (hexs[currentInt].currentPassiveUnit != null)
                {
                    hexs[currentInt].currentPassiveUnit.SetActive(false);
                }
                if (hexs[currentInt].forestTile != null)
                {
                    hexs[currentInt].forestTile.SetActive(false);
                }
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
        inMenu = !inMenu;
        escMenu.SetActive(!escMenu.activeSelf);
    }

    public void endTurn()
    {
        turn += 1;
        terrain.GetComponent<CreateHex>().CollectYields();

    }
    
    public int FromCoordToIndex(int posX, int posY)
    {
        return (int)(108 * Mathf.Floor(transform.position.z / 0.8333f) + Mathf.Floor(transform.position.x / 2.88675125f));
    }

    void MoveCameraMap()
    {
        // Store second last mouse position
        mouseMovement = mousePositions.Peek();
        // Remove second last mouse position
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
            for (int i = 0; i < currentScale - 1; i++)
            {
                accumX += 19.2f * Mathf.Pow(1.1f, i);
                accumY += 10.2f * Mathf.Pow(1.1f, i);
            }
            //x
            if (map.transform.localPosition.x - mouseMovement.x * 100 < accumX && map.transform.localPosition.x - mouseMovement.x * 100 > -accumX)
            {
                map.transform.localPosition -= new Vector3(mouseMovement.x * 100, 0, 0);
                trapezium.transform.localPosition -= new Vector3(mouseMovement.x * 100, 0, 0);
            }

            //y
            if (map.transform.localPosition.y - mouseMovement.y * 100 > -accumY && map.transform.localPosition.y - mouseMovement.y * 100 < accumY)
            {
                map.transform.localPosition -= new Vector3(0, mouseMovement.y * 100, 0);
                trapezium.transform.localPosition -= new Vector3(0, mouseMovement.y * 100, 0);
            }
        }
    }

    void CenterMap()
    {
        accumX = 0;
        accumY = 0;
        for (int i = 0; i < currentScale - 1; i++)
        {
            accumX += 19.2f * Mathf.Pow(1.1f, i);
            accumY += 10.2f * Mathf.Pow(1.1f, i);
        }
        if (map.transform.localPosition.x > accumX)
        {
            map.transform.localPosition = new Vector3(accumX, map.transform.localPosition.y, 0);
            trapezium.transform.localPosition = new Vector3(accumX, map.transform.localPosition.y, 0);
        }
        if (map.transform.localPosition.x < -accumX)
        {
            map.transform.localPosition = new Vector3(-accumX, map.transform.localPosition.y, 0);
            trapezium.transform.localPosition = new Vector3(-accumX, map.transform.localPosition.y, 0);
        }
        if (map.transform.localPosition.y > accumY)
        {
            map.transform.localPosition = new Vector3(map.transform.localPosition.x, accumY, 0);
            trapezium.transform.localPosition = new Vector3(map.transform.localPosition.x, accumY, 0);
        }
        if (map.transform.localPosition.y < -accumY)
        {
            map.transform.localPosition = new Vector3(map.transform.localPosition.x, -accumY, 0);
            trapezium.transform.localPosition = new Vector3(map.transform.localPosition.x, -accumY, 0);
        }
    }

    void Zoom()
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
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (currentScale < 15)
                {
                    //map.transform.localPosition = new Vector3(Input.mousePosition.x - 1520, Input.mousePosition.y - 15, 0);
                    map.transform.localScale *= 1.1f;
                    trapezium.transform.localScale *= 1.1f;
                    currentScale += 1;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (currentScale > 1)
                {
                    //map.transform.localPosition = new Vector3(Input.mousePosition.x - 1520, Input.mousePosition.y - 15, 0);
                    map.transform.localScale /= 1.1f;
                    trapezium.transform.localScale /= 1.1f;
                    currentScale -= 1;
                }
            }
        }
    }

    void CheckMousePosInfo()
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
        }
        // Move mouse
        if (Input.GetMouseButton(0))
        {
            Vector2 tempVec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            mousePositions.Enqueue(tempVec / 100);
            // if mouse moves two or more pixels while mouse 1 is pressed
            if (mousePositions.Count >= 2 && !inMenu)
            {
                MoveCameraMap();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // If mouse button one is released, clear mouse positions
            mousePositions.Clear();
        }
    }
}
