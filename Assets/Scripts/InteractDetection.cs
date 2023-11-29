using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDetection : MonoBehaviour
{
    public GOEventChannelSO interactChannel;
    //public GameObject objToInteract;
    Ray ray;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.CompareTag("Interactable"))
            {
                interactChannel.RaiseEvent(hit.transform.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));


    }
}
