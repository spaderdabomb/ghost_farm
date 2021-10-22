
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventController : MonoBehaviour
{
    public GameObject toolHolderUI;
    public MainCharacter mainCharacter;
    public GameSceneController gameSceneController;

    int UILayer;

    void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (RaycastResult result in GetEventSystemRaycastResults())
            {
                // print(result);
            }
            if (!mainCharacter.shopOpened && !gameSceneController.actionsMenuShowing && !IsPointerOverUIElement())
            {
                mainCharacter.CharacterMouseClicked();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (!gameSceneController.actionsMenuShowing)
            {
                mainCharacter.EPressed();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!mainCharacter.shopOpened)
            {
                gameSceneController.TabPressed();
            }
        }
    }

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}

