using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class oxygenController : MonoBehaviour
{
    public float maxOxygen = 30f;
    public float currOxygen;

    public string oxygenString;
    public TextMeshProUGUI oxygenText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public GameObject restartButton;

    public bool isDead = false;
    public bool isWin = false;
    // Start is called before the first frame update
    void Start()
    {
        restartButton.SetActive(false); 
        currOxygen = maxOxygen;
        oxygenString = "Oxygen: " + currOxygen + "/" + maxOxygen;
        oxygenText.text = oxygenString;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            loseText.gameObject.SetActive(true);
            gameObject.SetActive(false);
            restartButton.SetActive(true);

        }
        else if (isWin)
        {
            winText.gameObject.SetActive(true);
            restartButton.SetActive(true);
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
