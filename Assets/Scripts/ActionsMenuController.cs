using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventoryMenu;
    public GameObject buildMenu;
    public GameObject skillsMenu;

    public int currentTabIndex;

    void Start()
    {
        currentTabIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMenu(int headerIndex)
    {
        currentTabIndex = headerIndex;

        if (headerIndex == 0)
        {
            inventoryMenu.GetComponent<GraphicRaycaster>().enabled = true;
            buildMenu.GetComponent<GraphicRaycaster>().enabled = false;
            skillsMenu.GetComponent<GraphicRaycaster>().enabled = false;

            inventoryMenu.GetComponent<Canvas>().enabled = true;
            buildMenu.GetComponent<Canvas>().enabled = false;
            skillsMenu.GetComponent<Canvas>().enabled = false;
        }
        else if (headerIndex == 1)
        {
            inventoryMenu.GetComponent<GraphicRaycaster>().enabled = false;
            buildMenu.GetComponent<GraphicRaycaster>().enabled = true;
            skillsMenu.GetComponent<GraphicRaycaster>().enabled = false;

            inventoryMenu.GetComponent<Canvas>().enabled = false;
            buildMenu.GetComponent<Canvas>().enabled = true;
            skillsMenu.GetComponent<Canvas>().enabled = false;
        }
        else if (headerIndex == 2)
        {
            inventoryMenu.GetComponent<GraphicRaycaster>().enabled = false;
            buildMenu.GetComponent<GraphicRaycaster>().enabled = false;
            skillsMenu.GetComponent<GraphicRaycaster>().enabled = true;

            inventoryMenu.GetComponent<Canvas>().enabled = false;
            buildMenu.GetComponent<Canvas>().enabled = false;
            skillsMenu.GetComponent<Canvas>().enabled = true;
        }
    }



    public void HideAllCanvases()
    {
        inventoryMenu.GetComponent<GraphicRaycaster>().enabled = false;
        buildMenu.GetComponent<GraphicRaycaster>().enabled = false;
        skillsMenu.GetComponent<GraphicRaycaster>().enabled = false;

        inventoryMenu.GetComponent<Canvas>().enabled = false;
        buildMenu.GetComponent<Canvas>().enabled = false;
        skillsMenu.GetComponent<Canvas>().enabled = false;
    }


}
