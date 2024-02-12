using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objResetter : MonoBehaviour
{
    public levelSelector LS;
    public levelSelector DLS;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetList()
    {
        LS.allScenes = DLS.allScenes;
        LS.setup();
    }
}
