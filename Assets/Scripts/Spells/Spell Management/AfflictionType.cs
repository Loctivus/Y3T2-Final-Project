using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AfflictionType : MonoBehaviour
{
    [Tooltip("Affliction type for affliction visual effect objects so that it knows which object to spawn when affliction is applied to entity.")]
    public Entity.SpellAffliction afflictionType;
}
