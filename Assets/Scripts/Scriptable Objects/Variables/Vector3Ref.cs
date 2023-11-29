using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Vector3Ref 
{
    public bool useConstant = true;
    public Vector3 constantValue;
    public Vector3 offset;
    public Vector3Variable variable;

    public Vector3 value
    {
        get { return useConstant ? constantValue : variable.value; }
    }
}
