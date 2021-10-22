using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolHolderToggle : MonoBehaviour
{
    public GameObject toolHolderUI;
    public int toolHolderToggleIndex;
    ToolHolder toolHolderController;
    GameObject craftingButtonHolder;
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

        toolHolderToggleIndex = transform.GetSiblingIndex();
    }


    public void UpdateToolHolder()
    {
        if (transform.gameObject.GetComponent<Toggle>().isOn)
        {
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
                Transform tempItemTransform = Core.FindComponentInChildWithTag<Transform>(transform.gameObject, "item");
                if (tempItemTransform != null)
                {
                    int nextAvailableIndex = craftingHolderController.GetNextAvailableIndex();
                    GameObject tempButton = craftingHolderController.buttonArray[nextAvailableIndex];
                    tempItemTransform.SetParent(tempButton.transform, false);
                    tempItemTransform.localScale = new Vector2(1, 1);
                }
            }
        }
    }
}
