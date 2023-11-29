using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Events / GameObject Event Channel")]
public class GOEventChannelSO : ScriptableObject
{
    public UnityAction<GameObject> onEventRaised;

    public void RaiseEvent(GameObject obj)
    {
        if (onEventRaised != null)
        {
            onEventRaised.Invoke(obj);
        }
    }
}
