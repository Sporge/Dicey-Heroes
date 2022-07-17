using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    TMP_Text myText;
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<TMP_Text>();
        GameManager.GoldUpdate.AddListener(UpdateText);
        UpdateText();
    }

    void UpdateText()
    {
        myText.text =  ": " + GameManager.PlayerGold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
