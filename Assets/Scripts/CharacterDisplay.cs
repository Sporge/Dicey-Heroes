/*
 * Ryan Scheppler
 * 7/15/2021
 * Display character data on a character
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterDisplay : MonoBehaviour
{
    public CharacterScriptable character;
    private SpriteRenderer mySR;
    // Start is called before the first frame update
    void Start()
    {
        FillFields();
    }

    public void FillFields()
    {
        mySR = GetComponent<SpriteRenderer>();
        mySR.sprite = character.artwork;
        
        if (transform.childCount > 0)
        {
            GameObject highlightMenu = transform.GetChild(0).gameObject;
            TMP_Text[] Texts = highlightMenu.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text t in Texts)
            {
                if (t.name == "HealthText")
                {
                    t.text = character.health.ToString();
                }
                else if (t.name == "AttackText")
                {
                    t.text = "";
                    for (int i = 0; i < character.attackDice.Length; ++i)
                    {
                        t.text += character.attackDice[i].dieNum + "d" + character.attackDice[i].dieType;
                        if (i + 1 != character.attackDice.Length)
                            t.text += ", ";
                    }
                }
                else if (t.name == "DefenseText")
                {
                    t.text = "";
                    for (int i = 0; i < character.defenseDice.Length; ++i)
                    {
                        t.text += character.defenseDice[i].dieNum + "d" + character.defenseDice[i].dieType;
                        if (i + 1 != character.defenseDice.Length)
                            t.text += ", ";
                    }
                }
                else if (t.name == "SurvivalText")
                {
                    t.text = "";
                    for (int i = 0; i < character.survivalDice.Length; ++i)
                    {
                        t.text += character.survivalDice[i].dieNum + "d" + character.survivalDice[i].dieType;
                        if (i + 1 != character.survivalDice.Length)
                            t.text += ", ";
                    }
                }
                else if (t.name == "MagicText")
                {
                    t.text = "";
                    int i = 0;
                    for (i = 0; i < character.specialDice.Length; ++i)
                    {
                        t.text += character.specialDice[i].dieNum + "d" + character.specialDice[i].dieType;
                        if (i + 1 != character.specialDice.Length)
                            t.text += ", ";
                    }
                    if (i == 0)
                        t.text = "-";
                }
                else if (t.name == "Special")
                {
                    t.text = "Special: " + System.Enum.GetName(typeof(Abilities),character.ability);
                }
                else if (t.name == "Description")
                {
                    t.text = character.description;
                }
            }
        }
    }

}
