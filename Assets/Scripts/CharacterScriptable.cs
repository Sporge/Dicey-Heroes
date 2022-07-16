using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters")]
public class CharacterScriptable : ScriptableObject
{
    public string charName;
    public string description;
    public Sprite artwork;
    public int health;
    public int attackDieNum;
    public int attackDieType;
    public int defenseDieNum;
    public int defenseDieType;

}
