/*
 * Ryan Scheppler
 * 7/18/2021
 * object to hold the team of characters of player and opponents
 */
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Team", menuName = "Character Team")]
public class CharacterTeam : ScriptableObject
{
    
    public CharacterScriptable[] Team = new CharacterScriptable[4];

    public CharacterScriptable EmptyStart;

    

    
}
