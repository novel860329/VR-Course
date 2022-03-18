using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_number : MonoBehaviour
{
    public Text uiText;

    GameManager gm;

    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        uiText.text = "1";
    }

    public void RefreshUI()
    {
        uiText.text =  ""+gm.level;
    }
}
