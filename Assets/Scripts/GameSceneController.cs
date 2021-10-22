using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameSceneController : MonoBehaviour
{
    public GameObject actionsMenuCanvas;
    public GameObject mainCharacter;
    public GameObject toolHolderUI;
    public GameObject UICanvas;
    public GameObject craftingButtonHolder;
    public GameObject inventoryMenu;
    public GameObject buildMenuInventory;
    public GameObject actionsHeaderToggleGroup;
    public GameObject gameData;

    GameObject coinLabel;

    public int numCoins;
    public int fixedUpdateFrame;

    public float gameClock;

    ToolHolder toolHolderController;
    InventoryMenuController inventoryMenuController;
    InventoryMenuController buildMenuInventoryController;
    CraftingHolderController craftingHolderController;
    MainCharacter mainCharacterController;
    ActionsMenuController actionsMenuController;

    public bool actionsMenuShowing;
    public bool returningToGame;

    private void Awake()
    {
        // Initialize game data
        if (GameData.instance == null)
        {
            Instantiate(gameData);
        }
        GameData.instance.Save();
        GameData.instance.ResetData();

        actionsMenuShowing = false;
        returningToGame = true;

        gameClock = 0;

        toolHolderController = toolHolderUI.GetComponent<ToolHolder>();
        inventoryMenuController = GameObject.Find("InventoryMenu").gameObject.GetComponent<InventoryMenuController>();
        buildMenuInventoryController = UICanvas.transform.Find("ActionsMenuCanvas").Find("BuildMenu").Find("BuildMenuInventory").gameObject.GetComponent<InventoryMenuController>();
        craftingHolderController = craftingButtonHolder.GetComponent<CraftingHolderController>();
        mainCharacterController = mainCharacter.GetComponent<MainCharacter>();
        actionsMenuController = UICanvas.transform.Find("ActionsMenuCanvas").GetComponent<ActionsMenuController>();

        coinLabel = GameObject.Find("coinsLabel");

    }

    // Start is called before the first frame update
    void Start()
    {
        numCoins = 60;
    }

    // Update is called once per frame
    void Update()
    { 
        coinLabel.GetComponent<Text>().text = "x" + numCoins.ToString();
    }

    private void FixedUpdate()
    {
        gameClock += Time.fixedDeltaTime;
        fixedUpdateFrame += 1;
    }

    private void LateUpdate()
    {
        if (returningToGame)
        {
            SyncToolHolderAndInventory();
            returningToGame = false;
        }
    }

    public void TabPressed()
    {
        if (actionsMenuShowing)
        {

            actionsMenuController.HideAllCanvases();
            toolHolderUI.GetComponent<Canvas>().enabled = true;
            toolHolderUI.GetComponent<GraphicRaycaster>().enabled = true;
            actionsMenuCanvas.GetComponent<Canvas>().enabled = false;
            actionsMenuCanvas.GetComponent<GraphicRaycaster>().enabled = false;
            actionsHeaderToggleGroup.GetComponent<Canvas>().enabled = false;
            actionsHeaderToggleGroup.GetComponent<GraphicRaycaster>().enabled = false;
            actionsMenuShowing = false;
            craftingHolderController.SyncBuildInventoryToInventory();
            craftingHolderController.CheckBuildProduct();
            SyncInventoryToToolHolder();
            returningToGame = true;
        }
        else
        {
            actionsMenuController.UpdateMenu(actionsMenuController.currentTabIndex);
            toolHolderUI.GetComponent<Canvas>().enabled = false;
            toolHolderUI.GetComponent<GraphicRaycaster>().enabled = false;
            actionsMenuCanvas.GetComponent<Canvas>().enabled = true;
            actionsMenuCanvas.GetComponent<GraphicRaycaster>().enabled = true;
            actionsHeaderToggleGroup.GetComponent<Canvas>().enabled = true;
            actionsHeaderToggleGroup.GetComponent<GraphicRaycaster>().enabled = true;
            SyncToolHolderAndInventory();
            actionsMenuShowing = true;
        }
    }

    public void SyncToolHolderAndInventory()
    {
        for (int i = 0; i < toolHolderController.toolHolderTools.Length; i++)
        {
            inventoryMenuController.toolHolderTools[i] = null;
            buildMenuInventoryController.toolHolderTools[i] = null;
            GameObject inventoryButton = inventoryMenu.transform.GetChild(i).gameObject;
            GameObject buildInventoryButton = buildMenuInventory.transform.GetChild(i).gameObject;
            GameObject craftingButton;
            if (i < craftingButtonHolder.transform.childCount - 1)
            { 
                craftingButton = craftingButtonHolder.transform.GetChild(i).gameObject;
                foreach (Transform child in craftingButton.transform)
                {
                    if (child.GetComponent<Tool>())
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                }
            }

            foreach (Transform child in inventoryButton.transform)
            { 
                if (child.GetComponent<Tool>())
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            foreach (Transform child in buildInventoryButton.transform)
            {
                if (child.GetComponent<Tool>())
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            Tool tempTool = toolHolderController.toolHolderTools[i];
            if (tempTool != null)
            {
                inventoryMenuController.AddTool(tempTool);
                buildMenuInventoryController.AddTool(tempTool);
            }
        }
    }

    public void SyncInventoryToToolHolder()
    {
        for (int i = 0; i < toolHolderController.toolHolderTools.Length; i++)
        {
            // Reset tool holder
            toolHolderController.toolHolderTools[i] = null;
            foreach (Transform childToggle in toolHolderController.toggleArray[i].transform)
            {
                if (childToggle.GetComponent<Tool>())
                {
                    GameObject.Destroy(childToggle.gameObject);
                }
            }


            // Sync Tools
            Tool tempTool = inventoryMenuController.toolHolderTools[i];
            if (tempTool != null)
            {
                toolHolderController.AddTool(tempTool.prefabName, tempTool.toolName, tempTool.toolType);
            }
        }

        craftingHolderController.itemCrafted = false;
    }
}
