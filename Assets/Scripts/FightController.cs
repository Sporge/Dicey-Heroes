using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    public GameObject characterPrefab;

    public Vector3[] PlayerPositions;
    public Vector3[] OpponentPositions;


    // Start is called before the first frame update
    void Start()
    {
        //loop through and place player characters
        for(int i = 0; i < GameManager.instance.playerCharacters.Count; ++i)
        {
            GameObject clone;
            if (i >= PlayerPositions.Length)
            {
                
                clone = Instantiate(characterPrefab, new Vector3(-1.5f + -1 * i, -1, 0), Quaternion.identity);
            }
            else
            {
                clone = Instantiate(characterPrefab, PlayerPositions[i], Quaternion.identity);
            }
            CharacterDisplay display = clone.GetComponent<CharacterDisplay>();
            display.character = GameManager.instance.playerCharacters[i];
            display.FillFields();
            print(display.character.health);
        }
        //loop through and place opponent characters
        for (int i = 0; i < GameManager.instance.opponentCharacters.Count; ++i)
        {
            GameObject clone;
            if (i >= PlayerPositions.Length)
            {

                clone = Instantiate(characterPrefab, new Vector3(1.5f + i, -1, 0), Quaternion.Euler(0, 180, 0));
            }
            else
            {
                clone = Instantiate(characterPrefab, OpponentPositions[i], Quaternion.Euler(0, 180, 0));
            }

            CharacterDisplay display = clone.GetComponent<CharacterDisplay>();
            display.character = GameManager.instance.opponentCharacters[i];
            display.FillFields();
            print(display.character.health);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
