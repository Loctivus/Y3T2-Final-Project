using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    public List<GameObject> spellDemoObjs = new List<GameObject>();
    public List<GameObject> practiceRangeDemoObjs = new List<GameObject>();


    public void ResetPracticeRange()
    {
        foreach (GameObject prDemoObj in practiceRangeDemoObjs)
        {
            PracticeRangeDemo prDemo = prDemoObj.GetComponent<PracticeRangeDemo>();
            prDemo.ResetDemo();
        }
    }

    public void ResetSpellDemos()
    {
        foreach (GameObject spellDemoObj in spellDemoObjs)
        {
            DemoController dc = spellDemoObj.GetComponent<DemoController>();
            dc.ResetDemo();
        }
    }
}
