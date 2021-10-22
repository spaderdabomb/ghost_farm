using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuController : MonoBehaviour
{
    int slotSelectedIndex;
    public Tool[] toolHolderTools;
    public GameObject[] toggleArray;
    public GameObject gameScene;

    [SerializeField] GameObject inventoryItemTitle;
    [SerializeField] GameObject inventoryItemDescription;
    [SerializeField] GameObject inventoryItemImage;
    [SerializeField] GameObject inventoryItemCost;

    [SerializeField] GameObject craftingScrollViewContent;


    void Awake()
    {
        slotSelectedIndex = 0;
        toolHolderTools = new Tool[transform.childCount];
        toggleArray = new GameObject[transform.childCount];
        for (int i = 0; i < toggleArray.Length; i++)
        {
            toggleArray[i] = transform.GetChild(i).gameObject;
            toolHolderTools[i] = null;
        }

        // Instantiate recipes in crafting menu
        if (transform.parent.gameObject.name == "BuildMenu")
        {
            int j = 0;
            foreach (KeyValuePair<string, string[]> entry in GlobalData.Recipes)
            {
                int numPrefabsInRow = 7;
                for (int k = 0; k < entry.Value.Length; k++)
                {
                    GameObject recipeImagePrefab = Instantiate(Resources.Load("Prefabs/RecipeImage") as GameObject, Vector3.zero, Quaternion.identity);
                    GameObject recipeTextPrefab = Instantiate(Resources.Load("Prefabs/RecipeText") as GameObject, Vector3.zero, Quaternion.identity);
                    recipeImagePrefab.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + entry.Value[k]);
                    recipeTextPrefab.GetComponent<Text>().text = "+";

                    recipeImagePrefab.transform.SetParent(craftingScrollViewContent.transform, false);
                    if (k < entry.Value.Length - 1)
                    {
                        recipeTextPrefab.transform.SetParent(craftingScrollViewContent.transform, false);
                    }
                }

                for (int l = 0; l < numPrefabsInRow - entry.Value.Length; l++)
                {
                    GameObject emptyPrefab = Instantiate(Resources.Load("Prefabs/RecipeImage") as GameObject, Vector3.zero, Quaternion.identity);
                    emptyPrefab.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                    emptyPrefab.transform.SetParent(craftingScrollViewContent.transform, false);
                }

                GameObject productTextPrefab = Instantiate(Resources.Load("Prefabs/RecipeText") as GameObject, Vector3.zero, Quaternion.identity);
                GameObject productImagePrefab = Instantiate(Resources.Load("Prefabs/RecipeImage") as GameObject, Vector3.zero, Quaternion.identity);
                productTextPrefab.GetComponent<Text>().text = "=";
                productImagePrefab.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + entry.Key);
                productTextPrefab.transform.SetParent(craftingScrollViewContent.transform, false);
                productImagePrefab.transform.SetParent(craftingScrollViewContent.transform, false);

                j++;
            }
        }
    }
    void Start()
    {

    }

    void Update()
    {

    }

    public void UpdateSlotSelectedIndex(int index)
    {
        slotSelectedIndex = index;
        Tool currentTool = toolHolderTools[index];
        string itemDisplayName = GlobalData.itemDict[currentTool.toolName]["displayName"].ToUpper();
        string itemDescription = GlobalData.itemDict[currentTool.toolName]["itemDescription"];
        string itemName = GlobalData.itemDict[currentTool.toolName]["toolName"];
        int sellPrice = Mathf.RoundToInt(int.Parse(GlobalData.itemDict[currentTool.toolName]["shopCost"]) / 2);

        inventoryItemTitle.GetComponent<Text>().text = itemDisplayName;
        inventoryItemDescription.GetComponent<Text>().text = itemDescription;
        inventoryItemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + itemName);
        inventoryItemCost.GetComponent<Text>().text = sellPrice.ToString() + " g";
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

    public void RemoveTool(int index, Button parentButton)
    {
        toolHolderTools[index] = null;
        foreach (Transform child in parentButton.transform)
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