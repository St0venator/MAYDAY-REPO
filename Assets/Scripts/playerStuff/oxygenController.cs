using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class oxygenController : MonoBehaviour
{
    public float maxOxygen = 100f;
    public float currOxygen;

    public string oxygenString;
    public TextMeshProUGUI oxygenText;
    public TextMeshProUGUI oxygenText2;
    public TextMeshProUGUI oxygenText3;

    public bool isDead = false;
    public bool isWin = false;
    // Start is called before the first frame update
    void Start()
    {
        currOxygen = maxOxygen;
        oxygenString = "Oxygen: " + currOxygen + "/" + maxOxygen;
        oxygenText.text = oxygenString;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            oxygenText3.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else if (isWin)
        {
            oxygenText2.gameObject.SetActive(true);
        }
    }

    public void reduceOxygen(float amnt)
    {
        currOxygen -= amnt;

        currOxygen = Mathf.Floor(currOxygen);

        if(currOxygen <= 0f)
        {
            isDead = true;
        }
        oxygenString = "Oxygen: " + currOxygen + "/" + maxOxygen;
        oxygenText.text = oxygenString;
    }
}
