using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{ 
    public GameObject toolHolderUI;
    GameObject craftingButtonHolder;
    GameObject buildMenuInventory;
    int toolHolderToggleIndex;
    ToolHolder toolHolderController;
    CraftingHolderController craftingHolderController;

    // Start is called before the first frame update
    void Awake()
    {

    }

    void Start()
    {
        toolHolderController = toolHolderUI.GetComponent<ToolHolder>();
        craftingButtonHolder = GameObject.Find("CraftingButtonHolder");
        craftingHolderController = craftingButtonHolder.GetComponent<CraftingHolderController>();
        buildMenuInventory = GameObject.Find("BuildMenuInventory");

        toolHolderToggleIndex = transform.GetSiblingIndex();
    }

    public void UpdateToolHolder()
    {
        // Update current slot selected
        if (transform.parent.gameObject.name == "InventoryMenu" || transform.parent.gameObject.name == "BuildMenuInventory")
        {
            transform.parent.gameObject.GetComponent<InventoryMenuController>().UpdateSlotSelectedIndex(toolHolderToggleIndex);
        }
        else if (transform.parent.gameObject.name == "ShopInventoryMenu")
        {
            transform.parent.gameObject.GetComponent<ShopInventoryController>().UpdateSlotSelectedIndex(toolHolderToggleIndex);
        }
        else if (transform.parent.gameObject.name == "ShopSellInventoryMenu")
        {
            transform.parent.gameObject.GetComponent<ShopSellInventoryController>().UpdateSlotSelectedIndex(toolHolderToggleIndex);
        }
        else if (transform.parent.gameObject.name == "ToolHolderUI")
        {
            transform.parent.gameObject.GetComponent<ToolHolder>().UpdateToolSelectedIndex(toolHolderToggleIndex);
        }
        else
        {
            print("unable to find tool parent");
        }

        // Move tool to build menu
        if (transform.parent.gameObject.name == "BuildMenuInventory")
        {
            GameObject tempItemTransform = Core.FindComponentInChildWithTag<Transform>(transform.gameObject, "item").gameObject;
            if (tempItemTransform != null)
            {
                craftingHolderController.MoveToolIn(tempItemTransform.gameObject, toolHolderToggleIndex);
            }
        }

    }
}
