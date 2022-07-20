/*
 * Ryan Scheppler
 * 7/18/2021
 * Character component to run the characters.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterScriptable originalValues;
    [HideInInspector]
    public string charName;
    [HideInInspector]
    public string description;
    [HideInInspector]
    public Sprite artwork;
    [HideInInspector]
    public int health;
    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public dice[] attackDice = new dice[] { };
    [HideInInspector]
    public dice[] baseAttackDice = new dice[] { };
    [HideInInspector]
    public dice[] defenseDice = new dice[] { };
    [HideInInspector]
    public dice[] baseDefenseDice = new dice[] { };
    [HideInInspector]
    public dice[] survivalDice = new dice[] { };
    [HideInInspector]
    public dice[] baseSurvivalDice = new dice[] { };
    [HideInInspector]
    public dice[] specialDice = new dice[] { };
    [HideInInspector]
    public dice[] baseSpecialDice = new dice[] { };
    public Abilities ability = Abilities.None;

    public void copyBase()
    {
        charName = originalValues.charName;
        description = originalValues.description;
        artwork = originalValues.artwork;
        health = originalValues.health;
        maxHealth = originalValues.maxHealth;
        originalValues.attackDice.CopyTo(attackDice, 0);
        originalValues.baseAttackDice.CopyTo(baseAttackDice, 0);
        defenseDice.CopyTo(defenseDice, 0);
        baseDefenseDice.CopyTo(baseDefenseDice, 0);
        survivalDice.CopyTo(survivalDice, 0);
        baseSurvivalDice.CopyTo(baseSurvivalDice, 0);
        specialDice.CopyTo(specialDice, 0);
        baseSpecialDice.CopyTo(baseSpecialDice, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
