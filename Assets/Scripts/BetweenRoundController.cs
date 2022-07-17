/*
 * Ryan Scheppler
 * 7/15/2021
 * Helps set up the start of this menu sceen and other things
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenRoundController : MonoBehaviour
{
    public Vector3[] PlayerPositions;

    private GameObject[] PlayerObjects;

    public GameObject characterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        RefillPlayerArea();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearPlayerArea()
    {
        //loop through and place player characters
        foreach(GameObject o in PlayerObjects)
        {
            Destroy(o);
        }
        
    }

    void RefillPlayerArea()
    {
        System.Array.Resize(ref PlayerObjects, PlayerPositions.Length);
        //loop through and place player characters
        for (int i = 0; i < GameManager.instance.playerCharacters.Count && GameManager.instance.playerCharacters[i].attackDice != null; ++i)
        {
            print("Setting up Clone " + i);
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
            PlayerObjects[i] = clone;
        }
    }
    void SetupPurchases()
    {

    }

}
