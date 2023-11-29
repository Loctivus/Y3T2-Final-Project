using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FloatRef
{
    public bool useConstant = false;
    public float constantValue;
    public FloatVariable variable;

    public float value
    {
        get { return useConstant ? constantValue : variable.value; }
    }
}
