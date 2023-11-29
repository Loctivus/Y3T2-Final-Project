using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TransformRef
{
    public bool useConstant = false;
    public Transform constantValue;
    public TransformVariable variable;

    public Transform value
    {
        get { return useConstant ? constantValue : variable.value; }
    }
}
