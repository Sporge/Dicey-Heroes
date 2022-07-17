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
    public int dieType;
    public int dieNum;
}

public enum RollType
{
    Attack,
    Defense,
    Survive,
    Magic
}

[CreateAssetMenu(fileName = "New Character", menuName = "Characters")]
public class CharacterScriptable : ScriptableObject
{
    public string charName;
    public string description;
    public Sprite artwork;
    public int health;
    [SerializeField]
    public dice[] attackDice;
    [SerializeField]
    public dice[] defenseDice;
    [SerializeField]
    public dice[] survivabilityDice;
    [SerializeField]
    public dice[] magicDice;


    public CharacterScriptable(string _charName, string _description, Sprite _artwork, int _health, dice[] _attackDice, dice[] _defenseDice, dice[] _survivabilityDice, dice[] _magicDice)
    {
        charName = _charName;
        description = _description;
        artwork = _artwork;
        health = _health;
        attackDice = _attackDice;
        defenseDice = _defenseDice;
        survivabilityDice = _survivabilityDice;
        magicDice = _magicDice;
    }
    public CharacterScriptable(CharacterScriptable toCopy)
    {
        charName = toCopy.charName;
        description = toCopy.description;
        artwork = toCopy.artwork;
        health = toCopy.health;
        attackDice = toCopy.attackDice;
        defenseDice = toCopy.defenseDice;
        survivabilityDice = toCopy.survivabilityDice;
        magicDice = toCopy.magicDice;
    }
    public int RollDice(dice[] toRoll)
    {
        int roll = 0;
        for (int i = 0; i < toRoll.Length; ++i)
        {
            for (int j = 0; j < toRoll[i].dieNum; ++j)
            {
                roll += Random.Range(1, toRoll[i].dieType);
            }
        }

        return roll;
    }
}
