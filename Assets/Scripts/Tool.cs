using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tool : MonoBehaviour

{
    public bool initialized;
    public string prefabName;
    public string toolName;
    public string toolType;
    public int toolHolderIndex;
    public int inventoryIndex;
    GameObject toolHolder;
    GameObject inventoryMenu;
    GameObject buildMenuInventory;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        toolHolder = GameObject.Find("ToolHolderUI");
        inventoryMenu = GameObject.Find("InventoryMenu");
        buildMenuInventory = GameObject.Find("BuildMenuInventory");

        if (initialized)
        {
            AddToolToToolHolder();
        }
    }

    public void Initialize(string tempPrefabName, string initToolName, string initToolType, int initToolHolderIndex, int initInventoryIndex, string menuType)
    {
        prefabName = tempPrefabName;
        toolName = initToolName;
        toolType = initToolType;
        toolHolderIndex = initToolHolderIndex;
        inventoryIndex = initInventoryIndex;
        Sprite loaded = Resources.Load<Sprite>("Sprites/" + toolName);
        GetComponent<Image>().sprite = loaded;
    }

    void AddToolToToolHolder()
    {
        GameObject parentToggle = transform.parent.gameObject;
        int siblingIndex = parentToggle.transform.GetSiblingIndex();
        toolHolder.GetComponent<ToolHolder>().toolHolderTools[siblingIndex] = this;
    }
}
