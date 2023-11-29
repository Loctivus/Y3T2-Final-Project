using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GOEventListener : MonoBehaviour
{
    [Tooltip("Event channel that will raise event functionality when called.")]
    [SerializeField] private GOEventChannelSO eventChannel = default;
    [Tooltip("Assign gameObject this is attached to as the object to compare.")]
    [SerializeField] GameObject objToCompare;
    public UnityEvent onEventRaised;


    private void OnEnable()
    {
        if (eventChannel != null)
        {
            eventChannel.onEventRaised += Response;
        }
    }

    private void OnDisable()
    {
        if (eventChannel != null)
        {
            eventChannel.onEventRaised -= Response;
        }
    }

    void Response(GameObject thisObj)
    {
        if (thisObj == objToCompare)
        {
            if (onEventRaised != null)
            {
                onEventRaised.Invoke();
            }
        }
    }
}
