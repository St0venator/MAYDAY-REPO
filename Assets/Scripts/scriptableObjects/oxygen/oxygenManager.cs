using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/oxygenController", order = 2)]
public class oxygenManager : ScriptableObject
{
    public float oxygenAmnt = 30f;
    const float oxygenInc = 30f;
    const float defaultOxygen = 30f;

    public void resetOxygen()
    {
        oxygenAmnt = defaultOxygen;
    }

    public void oxygenIncrement()
    {
        oxygenAmnt += oxygenInc;
    }

    public void oxygenDecrement(float oxygen)
    {
        oxygenAmnt -= oxygen;
    }

    public string displayOxygen()
    {
        return Mathf.RoundToInt(oxygenAmnt).ToString();
    }
}
