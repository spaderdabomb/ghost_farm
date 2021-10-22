using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCameraController : MonoBehaviour
{
    public GameObject virtualCamera_1;
    public GameObject virtualCamera_2;
    public GameObject[] virtualCameraArray;

    int currentIndex;
     
    private void Awake()
    {
        virtualCameraArray = new GameObject[2];
        virtualCameraArray[0] = virtualCamera_1;
        virtualCameraArray[1] = virtualCamera_2;

        currentIndex = 0;
    }

    public void SetVirtualCameraActive(int cameraIndex)
    {
        for (int i = 0; i < virtualCameraArray.Length; i++)
        {
            if (i == cameraIndex)
            {
                virtualCameraArray[i].SetActive(true);
                currentIndex = i;
            }
            else
            {
                virtualCameraArray[i].SetActive(false);
            }
        }

    }

    public GameObject GetCurrentVirtualCamera()
    {
        return virtualCameraArray[currentIndex];
    }
}
