/*
 * Ryan Scheppler
 * 7/15/2021
 * Character stats and descriptions holder
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct dice
{
    public int dieNum;
    public int dieType;
    public dice(int _dieNum, int _dieType)
    {
        dieNum = _dieNum;
        dieType = _dieType;
    }
}

public enum Abilities
{
    None,
    Criticals,
    HealInFront,
    MagicMissile,
    Archery,
    ArmorIncrease,
    AttackIncrease
}

[CreateAssetMenu(fileName = "New Character", menuName = "Characters")]
public class CharacterScriptable : ScriptableObject
{
    public string charName;
    public string description;
    public Sprite artwork;
    public int health;
    [HideInInspector]
    public int maxHealth;
    [SerializeField]
    public dice[] attackDice = new dice[] { };
    [HideInInspector]
    public dice[] baseAttackDice = new dice[] { };
    [SerializeField]
    public dice[] defenseDice = new dice[] { };
    [HideInInspector]
    public dice[] baseDefenseDice = new dice[] { };
    [SerializeField]
    public dice[] survivalDice = new dice[] { };
    [HideInInspector]
    public dice[] baseSurvivalDice = new dice[] { };
    [SerializeField]
    public dice[] specialDice = new dice[] { };
    [HideInInspector]
    public dice[] baseSpecialDice = new dice[] { };
    public Abilities ability = Abilities.None;

    public CharacterScriptable()
    {
        charName = "Empty";
        description = "Nothing here";
        artwork = null;
        health = 0;
        ability = Abilities.None;
        //array nonsense arrrrg
        System.Array.Resize(ref attackDice, 0);

        System.Array.Resize(ref defenseDice, 0);

        System.Array.Resize(ref survivalDice, 0);

        System.Array.Resize(ref specialDice, 0);

        System.Array.Resize(ref baseAttackDice, 0);

        System.Array.Resize(ref baseDefenseDice, 0);

        System.Array.Resize(ref baseSurvivalDice, 0);

        System.Array.Resize(ref baseSpecialDice,0);
    }
    public CharacterScriptable(string _charName, string _description, Sprite _artwork, int _health, dice[] _attackDice, dice[] _defenseDice, dice[] _survivalDice, dice[] _specialDice, Abilities _ability)
    {
        charName = _charName;
        description = _description;
        artwork = _artwork;
        health = _health;
        maxHealth = _health;
        ability = _ability;
        //array nonsense arrrrg
        CopyDiceArray(attackDice, _attackDice);
        CopyDiceArray(baseAttackDice, _attackDice);
        CopyDiceArray(defenseDice, _defenseDice);
        CopyDiceArray(baseDefenseDice, _defenseDice);
        CopyDiceArray(survivalDice, _survivalDice);
        CopyDiceArray(baseSurvivalDice, _survivalDice);
        CopyDiceArray(specialDice, _specialDice);
        CopyDiceArray(baseSpecialDice, _specialDice);
    }
    public void CopyValues(CharacterScriptable toCopy)
    {
        charName = toCopy.charName;
        description = toCopy.description;
        artwork = toCopy.artwork;
        health = toCopy.health;
        maxHealth = toCopy.health;
        ability = toCopy.ability;
        //array nonsense arrrrg


        CopyDiceArray(attackDice, toCopy.attackDice);
        CopyDiceArray(baseAttackDice, toCopy.baseAttackDice);
        CopyDiceArray(defenseDice, toCopy.defenseDice);
        CopyDiceArray(baseDefenseDice, toCopy.baseDefenseDice);
        CopyDiceArray(survivalDice, toCopy.survivalDice);
        CopyDiceArray(baseSurvivalDice, toCopy.baseSurvivalDice);
        CopyDiceArray(specialDice, toCopy.specialDice);
        CopyDiceArray(baseSpecialDice, toCopy.baseSpecialDice);

        
    }

    public void CopyDiceArray(dice[] destination, dice[] source)
    {
        System.Array.Resize(ref destination, source.Length);
        
        for(int i = 0; i < source.Length; i++)
        {
            //destination[i] = new dice();
            destination[i].dieNum = source[i].dieNum;
            destination[i].dieType = source[i].dieType;
        }
    }
    
}
