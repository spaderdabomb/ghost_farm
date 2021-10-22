using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public GameObject shopInventoryMenu;
    public GameObject itemBuyText;
    public GameObject buttonBuy;
    public GameObject buttonBuyText;
    public GameObject shopBuyInventoryMenu;
    public GameObject shopSellInventoryMenu;
    public GameObject shopSellScrollView;
    public GameObject inventoryMenu;

    public ToolHolder toolHolderUI;

    public GameSceneController gameSceneController;

    ShopInventoryController shopInventoryMenuController;
    ShopSellInventoryController shopSellInventoryController;
    InventoryMenuController inventoryMenuController;

    void Awake()
    {
        shopInventoryMenuController = shopInventoryMenu.GetComponent<ShopInventoryController>();
        shopSellInventoryController = shopSellInventoryMenu.GetComponent<ShopSellInventoryController>();
        inventoryMenuController = inventoryMenu.GetComponent<InventoryMenuController>();
    }

    private void Start()
    {
/*        InitializeShop("Lumber Yard");
*/    }

    public void InitializeShop(string shopName)
    {
        shopInventoryMenuController.ClearMenu();
        if (shopName == "Lumber Yard")
        {
            shopInventoryMenuController.AddTool("axe", "shovelNormal", "shovel");
            shopInventoryMenuController.AddTool("axe", "pickaxeNormal", "pickaxe");
            shopInventoryMenuController.AddTool("axe", "scytheNormal", "scythe");
        }
        else if (shopName == "Farm Stall")
        {
            shopInventoryMenuController.AddTool("axe", "seedsCabbage", "seed");
            shopInventoryMenuController.AddTool("axe", "seedsCarrot", "seed");
            shopInventoryMenuController.AddTool("axe", "seedsCorn", "seed");
            shopInventoryMenuController.AddTool("axe", "seedsTomato", "seed");
            shopInventoryMenuController.AddTool("axe", "seedsGreenBean", "seed");
            shopInventoryMenuController.AddTool("axe", "seedsSquash", "seed");
        }
    }

    public void ShopItemSelectedUpdated(Tool tempTool)
    {
        if (tempTool != null)
        {
            buttonBuy.GetComponent<Button>().interactable = true;
            buttonBuyText.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            itemBuyText.GetComponent<Text>().text = "Buy " + GlobalData.itemDict[tempTool.toolName]["displayName"] + " for " + GlobalData.itemDict[tempTool.toolName]["shopCost"] + " coins?";
        }
        else
        {
            buttonBuy.GetComponent<Button>().interactable = false;
            buttonBuyText.GetComponent<Text>().color = new Color32(255, 255, 255, 125);
            itemBuyText.GetComponent<Text>().text = "";
        }
    }

    public void ShopSellItemSelectedUpdated(Tool tempTool)
    {
        shopSellInventoryController.SyncInventoryToSellInventory();
        tempTool = shopSellInventoryController.toolHolderTools[shopSellInventoryController.slotSelectedIndex];
        if (tempTool != null)
        {
            buttonBuy.GetComponent<Button>().interactable = true;
            buttonBuyText.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            int sellPrice = Mathf.RoundToInt(int.Parse(GlobalData.itemDict[tempTool.toolName]["shopCost"]) / 2);
            itemBuyText.GetComponent<Text>().text = "Sell " + GlobalData.itemDict[tempTool.toolName]["displayName"] + " for " + sellPrice.ToString() + " coins?";
        }
        else
        {
            buttonBuy.GetComponent<Button>().interactable = false;
            buttonBuyText.GetComponent<Text>().color = new Color32(255, 255, 255, 125);
            itemBuyText.GetComponent<Text>().text = "";
        }
    }

    public void BuyItem()
    {
        if (buttonBuyText.GetComponent<Text>().text == "Buy")
        {
            Tool tempTool = shopInventoryMenuController.toolHolderTools[shopInventoryMenuController.slotSelectedIndex];
            if (tempTool != null)
            {
                if (gameSceneController.numCoins >= int.Parse(GlobalData.itemDict[tempTool.toolName]["shopCost"]))
                {
                    gameSceneController.numCoins -= int.Parse(GlobalData.itemDict[tempTool.toolName]["shopCost"]);
                    shopInventoryMenuController.RemoveTool(shopInventoryMenuController.slotSelectedIndex, shopInventoryMenuController.toggleArray[shopInventoryMenuController.slotSelectedIndex]);
                    toolHolderUI.GetComponent<ToolHolder>().AddTool(tempTool.prefabName, tempTool.toolName, tempTool.toolType);
                    gameSceneController.SyncToolHolderAndInventory();
                    shopSellInventoryController.SyncInventoryToSellInventory();
                }
            }
        }
        else if (buttonBuyText.GetComponent<Text>().text == "Sell")
        {
            Tool tempTool = shopSellInventoryController.toolHolderTools[shopSellInventoryController.slotSelectedIndex];
            if (tempTool != null)
            {
                int sellPrice = Mathf.RoundToInt(int.Parse(GlobalData.itemDict[tempTool.toolName]["shopCost"]) / 2);
                gameSceneController.numCoins += sellPrice;
                shopSellInventoryController.RemoveTool(shopSellInventoryController.slotSelectedIndex, shopSellInventoryController.toggleArray[shopSellInventoryController.slotSelectedIndex]);
                shopSellInventoryController.SyncSellInventoryToInventory();
                gameSceneController.SyncInventoryToToolHolder();
                gameSceneController.returningToGame = true;
            }
        }
    }

    public void ShopTabClicked(int tabIndex)
    {
        if (tabIndex == 0)
        {
            shopBuyInventoryMenu.GetComponent<Canvas>().enabled = true;
            shopBuyInventoryMenu.GetComponent<GraphicRaycaster>().enabled = true;
            shopSellInventoryMenu.GetComponent<Canvas>().enabled = false;
            shopSellInventoryMenu.GetComponent<GraphicRaycaster>().enabled = false;
            shopSellScrollView.GetComponent<Canvas>().enabled = false;

            buttonBuyText.GetComponent<Text>().text = "Buy";
        }
        else if (tabIndex == 1)
        {
            shopBuyInventoryMenu.GetComponent<Canvas>().enabled = false;
            shopBuyInventoryMenu.GetComponent<GraphicRaycaster>().enabled = false;
            shopSellInventoryMenu.GetComponent<Canvas>().enabled = true;
            shopSellInventoryMenu.GetComponent<GraphicRaycaster>().enabled = true;
            shopSellScrollView.GetComponent<Canvas>().enabled = true;

            buttonBuyText.GetComponent<Text>().text = "Sell";

            shopSellInventoryController.SyncInventoryToSellInventory();
        }
    }

    public void ShopOpened()
    {
        shopSellInventoryController.SyncInventoryToSellInventory();
    }
}
