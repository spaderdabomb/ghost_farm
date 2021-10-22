using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Core
{

    public static GameObject FindGameObjectByNameAndTag(string name, string tag)
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag(tag);
        GameObject returnObject = null;
        foreach (GameObject taggedOne in go)
        {
            // Do you code here e.g.
            if (taggedOne.name == name)
            {
                returnObject = taggedOne;
            }
        }

        return returnObject;
    }

    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }

    public static T[] FindComponentsInChildrenWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component
    {
        if (parent == null) { throw new System.ArgumentNullException(); }
        if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }
        List<T> list = new List<T>(parent.GetComponentsInChildren<T>(forceActive));
        if (list.Count == 0) { return null; }

        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].CompareTag(tag) == false)
            {
                list.RemoveAt(i);
            }
        }
        return list.ToArray();
    }

    public static bool ContainsAny(this string haystack, params string[] needles)
    {
        foreach (string needle in needles)
        {
            if (haystack.Contains(needle))
                return true;
        }

        return false;
    }

    public static bool ContainsAll(string[] haystacks, string[] needles)
    {
        bool foundAll = new bool();
        foundAll = false;

        foreach (string needle in needles)
        {
            foundAll = false;
            foreach (string haystack in haystacks)
            {
                MonoBehaviour.print(haystack);
                if (haystack == needle)
                {
                    MonoBehaviour.print("we true");
                    foundAll = true;
                }
            }
            if (!foundAll)
            {
                MonoBehaviour.print("heredude");
                foundAll = false;
            }
        }

        return foundAll;
    }
}
