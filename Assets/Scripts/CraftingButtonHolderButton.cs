using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingButtonHolderButton : MonoBehaviour
{
    public Button button;
    public GameObject craftingButtonHolder;
    int childIndex;
        
    void Start()
    {
        // Get child index
        childIndex = transform.GetSiblingIndex();

        //Fetch the Toggle GameObject
        button = GetComponent<Button>();

        //Add listener for when the state of the Toggle changes, to take action
        button.onClick.AddListener(delegate { ButtonClicked(button); });
    }

    //Output the new state of the Toggle into Text
    void ButtonClicked(Button buttonChanged)
    {
        craftingButtonHolder.GetComponent<CraftingHolderController>().MoveToolOut(childIndex, buttonChanged);
        craftingButtonHolder.GetComponent<CraftingHolderController>().CheckCraftingRecipes();
    }
}
