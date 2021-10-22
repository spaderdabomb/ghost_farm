using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSellInventoryController : MonoBehaviour
{
    public int slotSelectedIndex;
    public Tool[] toolHolderTools;
    public GameObject[] toggleArray;
    public GameObject gameScene;
    public GameObject shopMenu;
    public GameObject inventoryMenu;

    ShopController shopMenuController;
    InventoryMenuController inventoryMenuController;

    void Awake()
    {
        shopMenuController = shopMenu.GetComponent<ShopController>();
        inventoryMenuController = inventoryMenu.GetComponent<InventoryMenuController>();

        slotSelectedIndex = 0;
        toolHolderTools = new Tool[transform.childCount];
        toggleArray = new GameObject[transform.childCount];
        for (int i = 0; i < toggleArray.Length; i++)
        {
            toggleArray[i] = transform.GetChild(i).gameObject;
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

    public void UpdateSlotSelectedIndex(int index)
    {
        slotSelectedIndex = index;

        Tool tempTool = toolHolderTools[index];
        shopMenuController.ShopSellItemSelectedUpdated(tempTool);
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

    public void AddTool(Tool newTool)
    {
        for (int i = 0; i < toolHolderTools.Length; i++)
        {
            if (toolHolderTools[i] == null)
            {
                InstantiateInventoryPrefab(newTool.prefabName, newTool.toolName, newTool.toolType);
                toolHolderTools[i] = newTool;
                break;
            }
        }
    }

    public void RemoveTool(int index, GameObject parentToggle)
    {
        toolHolderTools[index] = null;
        foreach (Transform child in parentToggle.transform)
        {
            if (child.GetComponent<Tool>())
            {
                GameObject.Destroy(child.gameObject);
                break;
            }
        }
    }

    public Tool GetSelectedTool()
    {
        return toolHolderTools[slotSelectedIndex];
    }

    public void ClearMenu()
    {
        for (int i = 0; i < toggleArray.Length; i++)
        {
            GameObject tempToggle = transform.GetChild(i).gameObject;
            foreach (Transform child in tempToggle.transform)
            {
                if (child.GetComponent<Tool>())
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            toolHolderTools[i] = null;
        }
    }

    public void SyncInventoryToSellInventory()
    {
        for (int i = 0; i < toolHolderTools.Length; i++)
        {
            toolHolderTools[i] = null;
            GameObject tempToggle = toggleArray[i];
            foreach (Transform child in tempToggle.transform)
            {
                if (child.GetComponent<Tool>())
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            Tool tempTool = inventoryMenuController.toolHolderTools[i];
            if (tempTool != null)
            {
                AddTool(tempTool);
            }
        }
    }

    public void SyncSellInventoryToInventory()
    {
        for (int i = 0; i < toolHolderTools.Length; i++)
        {
            inventoryMenuController.toolHolderTools[i] = null;
            GameObject inventoryToggle = inventoryMenuController.toggleArray[i];
            foreach (Transform child in inventoryToggle.transform)
            {
                if (child.GetComponent<Tool>())
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            Tool tempTool = toolHolderTools[i];
            if (tempTool != null)
            {
                inventoryMenuController.AddTool(tempTool);
            }
        }
    }

    private void InstantiateInventoryPrefab(string prefabName, string toolName, string toolType)
    {
        GameObject loadedPrefab = Resources.Load("Prefabs/" + prefabName) as GameObject;
        GameObject instantiatedPrefab = Instantiate(loadedPrefab, Vector3.zero, Quaternion.identity);
        int nextAvailableIndex = GetNextAvailableIndex();
        GameObject tempToggle = toggleArray[nextAvailableIndex];
        instantiatedPrefab.transform.SetParent(tempToggle.transform, false);
        instantiatedPrefab.GetComponent<Tool>().Initialize(prefabName, toolName, toolType, nextAvailableIndex, nextAvailableIndex, "inventory");
    }
}