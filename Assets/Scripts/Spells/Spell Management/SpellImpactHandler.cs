using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellImpactHandler : MonoBehaviour
{
    public float effectDuration = 1.5f;

    /// <summary>
    /// Handles destroying the impact visual effect after a set amount of time.
    /// </summary>
    private void Awake()
    {
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(effectDuration);
        Destroy(gameObject);
    }
}
