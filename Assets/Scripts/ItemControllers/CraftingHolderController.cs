using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CraftingHolderController : MonoBehaviour
{
    public Tool[] toolHolderTools;
    public GameObject[] buttonArray;
    public GameObject buildMenuInventory;
    public GameObject toolHolder;
    public bool buildProductInitialized;
    public bool validRecipe;
    public bool itemCrafted;
    InventoryMenuController buildMenuInventoryController;
    InventoryMenuController inventoryMenuController;
    ToolHolder toolHolderController;

    GameObject buildProductButton;
    void Awake()
    {
        buildProductInitialized = false;
        validRecipe = false;
        itemCrafted = false;
        buildMenuInventoryController = buildMenuInventory.GetComponent<InventoryMenuController>();
        inventoryMenuController = Core.FindGameObjectByNameAndTag("InventoryMenu", "menu").GetComponent<InventoryMenuController>();
        buildProductButton = Core.FindGameObjectByNameAndTag("BuildProductButton", "toggle");
        toolHolderController = toolHolder.GetComponent<ToolHolder>();


        toolHolderTools = new Tool[transform.childCount];
        buttonArray = new GameObject[transform.childCount];
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i] = transform.GetChild(i).gameObject;
            toolHolderTools[i] = null;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetNextAvailableIndex()
    {
        int nextAvailableIndex = -1;

        for (int i = 0; i < toolHolderTools.Length; i++)
        {
            if (toolHolderTools[i] == null)
            {
                nextAvailableIndex = i;
                break;
            }

        }

        return nextAvailableIndex;
    }

    public void MoveToolOut(int index, Button parentButton)
    {
        toolHolderTools[index] = null;
        foreach (Transform child in parentButton.transform)
        {
            if (child.GetComponent<Tool>())
            {
                int buildMenuNextIndex = buildMenuInventoryController.GetNextAvailableIndex();
                GameObject tempButton = buildMenuInventoryController.toggleArray[buildMenuNextIndex];
                child.transform.SetParent(tempButton.transform, false);
                child.transform.localScale = new Vector2(1.0f, 1.0f);
                buildMenuInventoryController.toolHolderTools[buildMenuNextIndex] = child.GetComponent<Tool>();
                break;
            }
        }
    }

    public void MoveToolIn(GameObject itemToMove, int buildMenuItemIndex)
    {
        int nextAvailableIndex = GetNextAvailableIndex();
        GameObject tempButton = buttonArray[nextAvailableIndex];
        itemToMove.transform.SetParent(tempButton.transform, false);
        itemToMove.transform.localScale = new Vector2(1, 1);
        toolHolderTools[nextAvailableIndex] = itemToMove.GetComponent<Tool>();
        buildMenuInventoryController.toolHolderTools[buildMenuItemIndex] = null;

        CheckCraftingRecipes();
    }

    public void ClearMenu()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            GameObject tempButton = transform.GetChild(i).gameObject;
            foreach (Transform child in tempButton.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            toolHolderTools[i] = null;
        }
    }

    public void CheckCraftingRecipes()
    {
        // Get list of tools
        List<string> currentItemsList = new List<string>();
        foreach (Transform childButtonTransform in transform)
        {
            foreach (Transform childItemTransform in childButtonTransform)
            {
                if (childItemTransform.GetComponent<Tool>())
                {
                    Tool tempTool = childItemTransform.GetComponent<Tool>();
                    currentItemsList.Add(tempTool.toolName);
                }
            }
        }

        // Check for valid recipe
        DestroyBuildProductPrefab();
        string[] tempArray = currentItemsList.ToArray();

        validRecipe = false;
        foreach (KeyValuePair<string, string[]> entry in GlobalData.Recipes)
        {
            var result = tempArray.Intersect(GlobalData.Recipes[entry.Key]);
            if (result.Count() == GlobalData.Recipes[entry.Key].Length && tempArray.Length == GlobalData.Recipes[entry.Key].Length)
            {
                InstantiateBuildProductPrefab(GlobalData.itemDict[entry.Key]["prefabName"], GlobalData.itemDict[entry.Key]["toolName"], GlobalData.itemDict[entry.Key]["toolType"]);
                validRecipe = true;
            }
        }
    }

    public void CraftNewItem()
    {
        if (validRecipe)
        {
            // Create new item
            foreach (Transform childPrefabTransform in buildProductButton.transform)
            {
                childPrefabTransform.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                buildProductInitialized = true;
            }

            // Destroy all item prefabs
            foreach (Transform buttonTransform in transform)
            {
                foreach (Transform childPrefabTransform in buttonTransform)
                {
                    GameObject.Destroy(childPrefabTransform.gameObject);
                }
            }

            // Reset tool holder array
            for (int i = 0; i < buttonArray.Length; i++)
            {
                toolHolderTools[i] = null;
            }

            itemCrafted = true;
        }
    }

    public void DestroyBuildProductPrefab()
    {
        if (!buildProductInitialized)
        {
            foreach (Transform childPrefabTransform in buildProductButton.transform)
            {
                GameObject.Destroy(childPrefabTransform.gameObject);

            }
        }
    }

    public void MoveFromProductMenuToBuildInventory()
    {
        if (itemCrafted)
        {
            foreach (Transform child in buildProductButton.transform)
            {
                if (child.GetComponent<Tool>())
                {
                    int buildMenuNextIndex = buildMenuInventoryController.GetNextAvailableIndex();
                    GameObject tempButton = buildMenuInventoryController.toggleArray[buildMenuNextIndex];
                    child.transform.SetParent(tempButton.transform, false);
                    child.transform.localScale = new Vector2(1.0f, 1.0f);
                    buildMenuInventoryController.toolHolderTools[buildMenuNextIndex] = child.GetComponent<Tool>();

                    SyncBuildInventoryToInventory();
                    break;
                }
            }
            itemCrafted = false;
        }
    }

    public void SyncBuildInventoryToInventory()
    {
        // Clear our build product area
        ClearMenu();
        DestroyBuildProductPrefab();

        for (int i = 0; i < toolHolderController.toolHolderTools.Length; i++)
        {
            inventoryMenuController.toolHolderTools[i] = null;
            GameObject tempToggle = inventoryMenuController.toggleArray[i];
            foreach (Transform childItem in tempToggle.transform)
            {
                if (childItem.GetComponent<Tool>())
                {
                    GameObject.Destroy(childItem.gameObject);
                }
            }

            Tool tempTool = buildMenuInventoryController.toolHolderTools[i];
            if (tempTool != null)
            {
                inventoryMenuController.AddTool(tempTool);
            }
        }

        itemCrafted = false;
    }

    public void CheckBuildProduct()
    {
        foreach (Transform child in buildProductButton.transform)
        {
            if (child.GetComponent<Tool>())
            {
                print("moving product");
                itemCrafted = true;
                MoveFromProductMenuToBuildInventory();
            }
        }
    }

    private void InstantiateBuildProductPrefab(string prefabName, string toolName, string toolType)
    {
        GameObject loadedPrefab = Resources.Load("Prefabs/" + prefabName) as GameObject;
        GameObject instantiatedPrefab = Instantiate(loadedPrefab, Vector3.zero, Quaternion.identity);
        GameObject tempToggle = Core.FindGameObjectByNameAndTag("BuildProductButton", "toggle");
        instantiatedPrefab.transform.SetParent(tempToggle.transform, false);
        instantiatedPrefab.transform.localScale = new Vector2(1, 1);
        instantiatedPrefab.GetComponent<Image>().color = new Color32(255, 255, 255, 120);
        instantiatedPrefab.GetComponent<Tool>().Initialize(prefabName, toolName, toolType, -1, -1, "buildProductMenu");
    }
}
