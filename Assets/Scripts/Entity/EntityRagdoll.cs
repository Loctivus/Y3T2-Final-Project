using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRagdoll : MonoBehaviour
{
    #region Variables
    [Header("Colliders and Rigidbodies for Ragdolling")]
    public Collider[] nonRagColliders;
    [SerializeField]
    Collider[] allRagColliders;
    [SerializeField]
    List<Rigidbody> ragRigidbodies;
    Rigidbody rb;
    Animator anim;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Get components in children, as ragdoll colliders should be the only colliders in children
    /// Set colliders to be triggers and check for rigidbodies
    /// If rigidbodies are found, store them in ragRigidBodies array and set useGravity to false.
    /// </summary>
    void StoreRagdoll()
    {
        allRagColliders = gameObject.GetComponentsInChildren<Collider>(true);

        foreach (var collider in allRagColliders)
        {
            if (collider.transform.parent != null)
            {
                collider.isTrigger = true;

                var ragdollRB = collider.GetComponent<Rigidbody>();
                if (ragdollRB)
                {
                    ragdollRB.useGravity = false;
                    ragRigidbodies.Add(ragdollRB);
                }
            }

        }

    }


    /// <summary>
    /// Enable/Disable ragdoll depending on parameter value
    /// Entities that are spawned in call EnableRagdoll(false) to prevent entity from ragdolling on spawn
    /// </summary>
    /// <param name="enableRag"></param>
    public void EnableRagdoll(bool enableRag)
    {
        anim.enabled = !enableRag;
        foreach (Collider col in allRagColliders)
        {
            if (col.transform.parent != null)
            {
                col.enabled = enableRag;
                col.isTrigger = !enableRag;
            }
        }

        foreach (var ragdollRB in ragRigidbodies)
        {
            ragdollRB.useGravity = enableRag;
            ragdollRB.isKinematic = !enableRag;
        }

        foreach (var nonRagCol in nonRagColliders)
        {
            nonRagCol.enabled = !enableRag;
        }
    }

}
