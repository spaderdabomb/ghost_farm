using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransitioner : MonoBehaviour
{
    public GameObject cinematicCameraController;
    public Vector3 playerChange;
    public int vcamIndex;

    // Start is called before the first frame update
    void Awake()
    {

    }

    private void OnTriggerEnter2D(Collider2D collisionObject)
    {
        if (collisionObject.CompareTag("Player"))
        {
            collisionObject.transform.position += playerChange;
            cinematicCameraController.GetComponent<CinematicCameraController>().SetVirtualCameraActive(vcamIndex);

        }
    }
}
