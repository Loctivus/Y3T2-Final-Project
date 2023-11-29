using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO eventChannel = default;

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

    private void Response()
    {
        if (onEventRaised != null)
        {
            onEventRaised.Invoke();
        }
    }

}
