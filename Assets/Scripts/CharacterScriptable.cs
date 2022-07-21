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
    public dice(dice _d)
    {
        dieNum = _d.dieNum;
        dieType = _d.dieType;
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
    public List<dice> attackDice = new List<dice>();
    [HideInInspector]
    public List<dice> baseAttackDice = new List<dice>();
    [SerializeField]
    public List<dice> defenseDice = new List<dice>();
    [HideInInspector]
    public List<dice> baseDefenseDice = new List<dice>();
    [SerializeField]
    public List<dice> survivalDice = new List<dice>();
    [HideInInspector]
    public List<dice> baseSurvivalDice = new List<dice>();
    [SerializeField]
    public List<dice> specialDice = new List<dice>();
    [HideInInspector]
    public List<dice> baseSpecialDice = new List<dice>();
    public Abilities ability = Abilities.None;

    public CharacterScriptable()
    {
        charName = "Empty";
        description = "Nothing here";
        artwork = null;
        health = 0;
        ability = Abilities.None;

        attackDice = new List<dice>();
        baseAttackDice = new List<dice>();
        defenseDice = new List<dice>();
        baseDefenseDice = new List<dice>();
        specialDice = new List<dice>();
        baseSpecialDice = new List<dice>();
        survivalDice = new List<dice>();
        baseSurvivalDice = new List<dice>();
        //array nonsense arrrrg
       /* System.Array.Resize(ref attackDice, 0);

        System.Array.Resize(ref defenseDice, 0);

        System.Array.Resize(ref survivalDice, 0);

        System.Array.Resize(ref specialDice, 0);

        System.Array.Resize(ref baseAttackDice, 0);

        System.Array.Resize(ref baseDefenseDice, 0);

        System.Array.Resize(ref baseSurvivalDice, 0);

        System.Array.Resize(ref baseSpecialDice,0);*/
    }
    public CharacterScriptable(string _charName, string _description, Sprite _artwork, int _health, List<dice> _attackDice, List<dice> _defenseDice, List<dice> _survivalDice, List<dice> _specialDice, Abilities _ability)
    {
        charName = _charName;
        description = _description;
        artwork = _artwork;
        health = _health;
        maxHealth = _health;
        ability = _ability;
        //array nonsense arrrrg
        CopyDiceList(attackDice, _attackDice);
        CopyDiceList(baseAttackDice, _attackDice);
        CopyDiceList(defenseDice, _defenseDice);
        CopyDiceList(baseDefenseDice, _defenseDice);
        CopyDiceList(survivalDice, _survivalDice);
        CopyDiceList(baseSurvivalDice, _survivalDice);
        CopyDiceList(specialDice, _specialDice);
        CopyDiceList(baseSpecialDice, _specialDice);
    }
    public void CopyValues(CharacterScriptable toCopy)
    {
        charName = toCopy.charName;
        description = toCopy.description;
        artwork = toCopy.artwork;
        health = toCopy.health;
        maxHealth = toCopy.health;
        ability = toCopy.ability;

        
        
        CopyDiceList(attackDice, toCopy.attackDice);
        CopyDiceList(baseAttackDice, toCopy.baseAttackDice);
        CopyDiceList(defenseDice, toCopy.defenseDice);
        CopyDiceList(baseDefenseDice, toCopy.baseDefenseDice);
        CopyDiceList(survivalDice, toCopy.survivalDice);
        CopyDiceList(baseSurvivalDice, toCopy.baseSurvivalDice);
        CopyDiceList(specialDice, toCopy.specialDice);
        CopyDiceList(baseSpecialDice, toCopy.baseSpecialDice);
        

    }

    public void CopyDiceList(List <dice> destination, List<dice> source)
    {
        //System.Array.Resize(ref destination, source.Length);


        //System.Array.Copy(source, destination, source.Length);


        /*for (int i = 0; i < source.Length; i++)
        {
            destination[i] = new dice();
            destination[i].dieNum = source[i].dieNum;
            destination[i].dieType = source[i].dieType;
        }*/

        destination.Clear();

        foreach(dice d in source)
        {
            dice temp = new dice(d);
            destination.Add(temp);
        }
    }
    
}
