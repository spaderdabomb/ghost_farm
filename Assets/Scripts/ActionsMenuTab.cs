using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsMenuTab : MonoBehaviour
{
    public Toggle m_Toggle;
    public int toggleIndex;
    public GameObject actionsMenu;
    public GameObject shopMenu;

    void Start()
    {
        // Find game objects
        shopMenu = Core.FindGameObjectByNameAndTag("ShopMenu", "menu");

        // Fetch the Toggle GameObject
        m_Toggle = GetComponent<Toggle>();

        //Add listener for when the state of the Toggle changes, to take action
        m_Toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(m_Toggle); });
    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        if (transform.parent.gameObject.name == "ActionsHeaderToggleGroup")
        {
            if (change.isOn)
            {
                actionsMenu.GetComponent<ActionsMenuController>().UpdateMenu(toggleIndex);
            }
        }
        else if (transform.parent.gameObject.name == "ShopHeaderToggleGroup")
        {
            if (change.isOn)
            {
                shopMenu.GetComponent<ShopController>().ShopTabClicked(toggleIndex);
            }
        }
    }
}
