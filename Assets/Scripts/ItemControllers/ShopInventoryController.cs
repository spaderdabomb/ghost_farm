using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInventoryController : MonoBehaviour
{
    public int slotSelectedIndex;
    public Tool[] toolHolderTools;
    public GameObject[] toggleArray;
    public GameObject gameScene;
    public GameObject shopMenu;

    ShopController shopMenuController;

    void Awake()
    {
        shopMenuController = shopMenu.GetComponent<ShopController>();

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

    public void UpdateSlotSelectedIndex(int index)
    {
        slotSelectedIndex = index;

        Tool tempTool = toolHolderTools[index];
        shopMenuController.ShopItemSelectedUpdated(tempTool);
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

    public void AddTool(string prefabName, string toolName, string toolType)
    {
        for (int i = 0; i < toolHolderTools.Length; i++)
        {
            if (toolHolderTools[i] == null)
            {
                Tool newTool = InstantiateShopPrefab(prefabName, toolName, toolType);
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

    private Tool InstantiateShopPrefab(string prefabName, string toolName, string toolType)
    {
        GameObject loadedPrefab = Resources.Load("Prefabs/" + prefabName) as GameObject;
        GameObject instantiatedPrefab = Instantiate(loadedPrefab, Vector3.zero, Quaternion.identity);
        int nextAvailableIndex = GetNextAvailableIndex();
        GameObject tempToggle = toggleArray[nextAvailableIndex];
        instantiatedPrefab.transform.SetParent(tempToggle.transform, false);
        instantiatedPrefab.GetComponent<Tool>().Initialize(prefabName, toolName, toolType, nextAvailableIndex, nextAvailableIndex, "inventory");
        Tool newTool = instantiatedPrefab.GetComponent<Tool>();

        return newTool;
    }
}

