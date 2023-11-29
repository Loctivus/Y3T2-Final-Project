using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BoolRef
{
    public bool useConstant = false;
    public bool constantValue;
    public BoolVariable variable;

    public bool value
    {
        get { return useConstant ? constantValue : variable.value; }
    }
}
