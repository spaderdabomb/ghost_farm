using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOverride : MonoBehaviour
{
    [SerializeField] Toggle m_Toggle;
    [SerializeField] SkillsMenuController skillsMenuController;

    void Start()
    {
        m_Toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(m_Toggle); });
    }

    void ToggleValueChanged(Toggle change)
    {
        if (m_Toggle.CompareTag("skillsMenuButton"))
        {
            if (change.isOn)
            {
                skillsMenuController.UpdateSkillSelectedIndex(m_Toggle.transform.GetSiblingIndex());
            }
        }
    }
}
