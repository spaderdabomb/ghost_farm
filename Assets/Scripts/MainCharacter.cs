using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEditor;
using System;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController tileMapReadController;
    [SerializeField] CropsManager cropsManager;
    
    [HideInInspector] public bool shopOpened;
    [HideInInspector] public GameObject closestObject;

    public GameObject toolHolderUI;
    public GameObject UICanvas;
    public GameObject shopMenu;
    public GameObject shopInventoryMenu;
    public GameObject shopSellInventoryMenu;
    public GameObject skillsMenu;

    public GameSceneController gameSceneController;

    GameObject currentEButtonCanvas;
    Vector3Int selectedTilePosition;
    float maxDistance = 1.5f;
    float characterMoveSpeed;
    float horizontal;
    float vertical;
    float moveLimiter;
    bool foundInteractable;
    bool selectable;

    Rigidbody2D rb2d;

    Animator animator;
    ToolHolder toolHolderUIClass;
    InventoryMenuController inventoryMenuController;
    ShopSellInventoryController shopSellInventoryController;

    void Start()
    {
        characterMoveSpeed = 4.5f;
        rb2d = GetComponent<Rigidbody2D>();
        moveLimiter = 0.7f;
        foundInteractable = false;
        shopOpened = false;

        selectable = true;

        currentEButtonCanvas = null;

        animator = GetComponent<Animator>();

        closestObject = null;
        toolHolderUIClass = toolHolderUI.GetComponent<ToolHolder>();
        inventoryMenuController = GameObject.Find("InventoryMenu").gameObject.GetComponent<InventoryMenuController>();
        shopSellInventoryController = shopSellInventoryMenu.GetComponent<ShopSellInventoryController>();
    }

    // Update is called once per frame
    void Update()
    {
        Tool selectedTool = toolHolderUIClass.GetSelectedTool();
        SelectTile();
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        HandleCharacterProximityDetection();
    }

    public void CharacterMouseClicked()
    {
        CheckInput();
        UseToolGrid();
    }

    private void SelectTile()
    {
        selectedTilePosition = tileMapReadController.GetGridPosition(transform.position);
        markerManager.markedCellPosition = selectedTilePosition;
        markerManager.Show(selectable);
    }


    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        if (horizontal != 0)
        {
            animator.SetFloat("moveX", horizontal);
            animator.SetBool("walking", true);
        }


        if (vertical != 0)
        {
            animator.SetFloat("moveY", vertical);
            animator.SetBool("walking", true);
        }

        if (horizontal == 0f && vertical == 0f)
        {
            animator.SetFloat("moveX", horizontal);
            animator.SetFloat("moveY", vertical);
            animator.SetBool("walking", false);

        }

        rb2d.velocity = new Vector2(horizontal * characterMoveSpeed, vertical * characterMoveSpeed);
    }

    private void LateUpdate()
    {
        if (foundInteractable)
        {
            foundInteractable = false;
        }
        else
        {
            if (currentEButtonCanvas != null)
            {
                currentEButtonCanvas.GetComponent<Canvas>().enabled = false;
            }
        }
        closestObject = null;
    }

    void HandleCharacterProximityDetection()
    {
        Collider2D[] colliderCharacter = Physics2D.OverlapCircleAll(transform.position, 0.75f);
        foreach (Collider2D item in colliderCharacter)
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> entry in GlobalData.objectsDict)
            {
                if (item.gameObject.CompareTag(entry.Value["objectTag"]))
                {
                    closestObject = item.gameObject;
                }

                if (item.gameObject.CompareTag("building"))
                {
                    closestObject = item.gameObject;
                    foundInteractable = true;
                    closestObject.GetComponent<Stall>().EButtonCanvas.GetComponent<Canvas>().enabled = true;
                    currentEButtonCanvas = closestObject.GetComponent<Stall>().EButtonCanvas;
                }
            }
        }
    }

    public void EPressed()
    {
        if (closestObject.CompareTag("building") && !shopOpened)
        {
            shopMenu.GetComponent<Canvas>().enabled = true;
            shopMenu.GetComponent<GraphicRaycaster>().enabled = true;
            shopMenu.GetComponent<ShopController>().ShopOpened();
            shopOpened = true;

            if (closestObject.name == "LumberMarketStall")
            {
                shopMenu.GetComponent<ShopController>().InitializeShop("Lumber Yard");
            }
            else if (closestObject.name == "FarmStall")
            {
                shopMenu.GetComponent<ShopController>().InitializeShop("Farm Stall");
            }
        }
        else if (shopOpened)
        {
            shopMenu.GetComponent<Canvas>().enabled = false;
            shopInventoryMenu.GetComponent<GraphicRaycaster>().enabled = false;
            shopOpened = false;
        }
    }

    private void UseToolGrid()
    {
        TileBase tileBase = tileMapReadController.GetTileBase(selectedTilePosition);
        TileBase tileCrops = tileMapReadController.GetTileCrops(selectedTilePosition);
        TileData tileDataBase = null;
        TileData tileDataCrops = null;

        if (tileBase != null) {tileDataBase = tileMapReadController.GetTileData(tileBase); }

        if (tileCrops != null) { tileDataCrops = tileMapReadController.GetTileData(tileCrops); }

        Tool selectedTool = toolHolderUIClass.GetSelectedTool();

        if (tileDataBase != null)
        {
            if (selectable && tileDataBase.plowable && selectedTool.toolName == "shovelNormal")
            {
                cropsManager.Plow(selectedTilePosition);
            }
        }


        if (tileDataCrops != null)
        {
            if (selectable && tileDataCrops.plantable && selectedTool.toolType == "seed")
            {
                string toolNameStr = selectedTool.toolName.Remove(0,5).ToLower();
                cropsManager.Plant(selectedTilePosition, toolNameStr);
                toolHolderUIClass.RemoveTool(toolHolderUIClass.toolSelectedIndex);
            }
            else if (selectable && tileDataCrops.harvestable && selectedTool.toolName == "scytheNormal")
            {
                string foodName = cropsManager.plantedTile[(Vector2Int)selectedTilePosition].plantType;
                cropsManager.ClearCropsTile(selectedTilePosition);
                toolHolderUIClass.AddTool("axe", foodName, "food");
                gameSceneController.SyncToolHolderAndInventory();
            }
        }
    }

    private void CheckInput()
    {
        if (closestObject != null)
        {
            Tool selectedTool = toolHolderUIClass.GetSelectedTool();
            if (closestObject.tag == "treeNormal" && selectedTool?.toolType == "axe")
            {
                Destroy(closestObject);
                toolHolderUIClass.AddTool("axe", "logsNormal", "logs");
                gameSceneController.SyncToolHolderAndInventory();
                skillsMenu.GetComponent<SkillsMenuController>().AddExpToSkill("woodcutting", Int32.Parse(GlobalData.itemDict["logsNormal"]["exp"]));
            }
            else if (closestObject.tag == "treeBirch" && selectedTool?.toolType == "axe")
            {
                Destroy(closestObject);
                toolHolderUIClass.AddTool("axe", "logsBirch", "logs");
                gameSceneController.SyncToolHolderAndInventory();
            }
            else if (closestObject.tag == "rockNormal" && selectedTool?.toolType == "pickaxe")
            {
                Destroy(closestObject);
                toolHolderUIClass.AddTool("axe", "rockNormal", "rock");
                gameSceneController.SyncToolHolderAndInventory();
            }
            else if (closestObject.tag == "oreCopper" && selectedTool?.toolType == "pickaxe")
            {
                Destroy(closestObject);
                toolHolderUIClass.AddTool("axe", "oreCopper", "rock");
                gameSceneController.SyncToolHolderAndInventory();
            }
            else if (closestObject.tag == "oreIron" && selectedTool?.toolType == "pickaxe")
            {
                Destroy(closestObject);
                toolHolderUIClass.AddTool("axe", "oreIron", "rock");
                gameSceneController.SyncToolHolderAndInventory();
            }
            else if (closestObject.tag == "oreGold" && selectedTool?.toolType == "pickaxe")
            {
                Destroy(closestObject);
                toolHolderUIClass.AddTool("axe", "oreGold", "rock");
                gameSceneController.SyncToolHolderAndInventory();
            }
            else if (closestObject.tag == "orePlatinum" && selectedTool?.toolType == "pickaxe")
            {
                Destroy(closestObject);
                toolHolderUIClass.AddTool("axe", "orePlatinum", "rock");
                gameSceneController.SyncToolHolderAndInventory();
            }
            else if (closestObject.tag == "oreGemstone" && selectedTool?.toolType == "pickaxe")
            {
                Destroy(closestObject);
                toolHolderUIClass.AddTool("axe", "oreGemstone", "rock");
                gameSceneController.SyncToolHolderAndInventory();
            }
            else if (closestObject.tag == "oreDark" && selectedTool?.toolType == "pickaxe")
            {
                Destroy(closestObject);
                toolHolderUIClass.AddTool("axe", "oreDark", "rock");
                gameSceneController.SyncToolHolderAndInventory();
            }
        }
    }
}
