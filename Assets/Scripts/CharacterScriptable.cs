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
        System.Array.Resize(ref attackDice, _attackDice.Length);
        //_attackDice.CopyTo(attackDice, _attackDice.Length);
        System.Array.Copy(_attackDice, attackDice, attackDice.Length);

        System.Array.Resize(ref defenseDice, _defenseDice.Length);
        //_defenseDice.CopyTo(defenseDice, _defenseDice.Length);
        System.Array.Copy(_defenseDice, defenseDice, defenseDice.Length);

        System.Array.Resize(ref survivalDice, _survivalDice.Length);
        //_survivalDice.CopyTo(survivalDice, _survivalDice.Length);
        System.Array.Copy(_survivalDice, survivalDice, survivalDice.Length);

        System.Array.Resize(ref specialDice, _specialDice.Length);
        //_specialDice.CopyTo(specialDice, _specialDice.Length);
        System.Array.Copy(_specialDice, specialDice, specialDice.Length);

        System.Array.Resize(ref baseAttackDice, _attackDice.Length);
        //_attackDice.CopyTo(baseAttackDice, _attackDice.Length);
        System.Array.Copy(_attackDice, baseAttackDice, baseAttackDice.Length);

        System.Array.Resize(ref baseDefenseDice, _defenseDice.Length);
        //_defenseDice.CopyTo(baseDefenseDice, _defenseDice.Length);
        System.Array.Copy(_defenseDice, baseDefenseDice, baseDefenseDice.Length);

        System.Array.Resize(ref baseSurvivalDice, _survivalDice.Length);
        //_survivalDice.CopyTo(baseSurvivalDice, _survivalDice.Length);
        System.Array.Copy(_survivalDice, baseSurvivalDice, baseSurvivalDice.Length);

        System.Array.Resize(ref baseSpecialDice, _specialDice.Length);
        //_specialDice.CopyTo(baseSpecialDice, _specialDice.Length);
        System.Array.Copy(_specialDice, baseSpecialDice, baseSpecialDice.Length);
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
        System.Array.Resize(ref attackDice, toCopy.attackDice.Length);
        //toCopy.attackDice.CopyTo(attackDice, toCopy.attackDice.Length);
        System.Array.Copy(toCopy.attackDice, attackDice, attackDice.Length);

        System.Array.Resize(ref defenseDice, toCopy.defenseDice.Length);
        //toCopy.defenseDice.CopyTo(defenseDice, toCopy.defenseDice.Length);
        System.Array.Copy(toCopy.defenseDice, defenseDice, defenseDice.Length);

        System.Array.Resize(ref survivalDice, toCopy.survivalDice.Length);
        //toCopy.survivalDice.CopyTo(survivalDice, toCopy.survivalDice.Length);
        System.Array.Copy(toCopy.survivalDice, survivalDice, survivalDice.Length);

        System.Array.Resize(ref specialDice, toCopy.specialDice.Length);
        //toCopy.specialDice.CopyTo(specialDice, toCopy.specialDice.Length);
        System.Array.Copy(toCopy.specialDice, specialDice, specialDice.Length);


        System.Array.Resize(ref baseAttackDice, toCopy.attackDice.Length);
        //toCopy.attackDice.CopyTo(baseAttackDice, toCopy.attackDice.Length);
        System.Array.Copy(toCopy.attackDice, baseAttackDice, baseAttackDice.Length);

        System.Array.Resize(ref baseDefenseDice, toCopy.defenseDice.Length);
        //toCopy.defenseDice.CopyTo(baseDefenseDice, toCopy.defenseDice.Length);
        System.Array.Copy(toCopy.defenseDice, baseDefenseDice, baseDefenseDice.Length);

        System.Array.Resize(ref baseSurvivalDice, toCopy.survivalDice.Length);
        //toCopy.survivalDice.CopyTo(baseSurvivalDice, toCopy.survivalDice.Length);
        System.Array.Copy(toCopy.survivalDice, baseSurvivalDice, baseSurvivalDice.Length);

        System.Array.Resize(ref baseSpecialDice, toCopy.specialDice.Length);
        //toCopy.specialDice.CopyTo(baseSpecialDice, toCopy.specialDice.Length);
        System.Array.Copy(toCopy.specialDice, baseSpecialDice, baseSpecialDice.Length);
    }
    
}
