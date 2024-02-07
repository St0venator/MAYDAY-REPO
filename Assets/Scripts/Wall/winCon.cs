using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winCon : MonoBehaviour
{
    public levelSelector LS;
    // Start is called before the first frame update
    void Start()
    {
        LS.setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LS.changeScenes();
            //other.gameObject.GetComponent<oxygenController>().isWin = true;
        }
    }
}
