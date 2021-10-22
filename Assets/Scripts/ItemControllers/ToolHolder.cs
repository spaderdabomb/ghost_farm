using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolHolder : MonoBehaviour
{
    public int toolSelectedIndex;
    public Tool[] toolHolderTools;
    public GameObject[] toggleArray;

    [SerializeField] GameObject inventoryItemTitle;
    [SerializeField] GameObject inventoryItemDescription;
    [SerializeField] GameObject inventoryItemImage;
    [SerializeField] GameObject inventoryItemCost;


    private void Awake()
    {
        toolSelectedIndex = 0;
        toolHolderTools = new Tool[GlobalData.numToolSlots];
        toggleArray = new GameObject[GlobalData.numToolSlots];
        for (int i = 0; i < toolHolderTools.Length; i++)
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

    public void UpdateToolSelectedIndex(int index)
    {
        toolSelectedIndex = index;
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
                Tool newTool = InstantiateToolHolderPrefab(prefabName, toolName, toolType);
                toolHolderTools[i] = newTool;
                break;
            }
        }
    }

    public void RemoveTool(int index)
    {
        toolHolderTools[index] = null;
        GameObject toolToggle = toggleArray[index];
        foreach (Transform child in toolToggle.transform)
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
        return toolHolderTools[toolSelectedIndex];
    }

    private Tool InstantiateToolHolderPrefab(string prefabName, string toolName, string toolType)
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
