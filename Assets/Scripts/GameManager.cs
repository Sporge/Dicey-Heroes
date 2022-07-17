/*
 * Ryan Scheppler
 * 7/15/2021
 * Holds values and data to be preserved between scenes
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    [SerializeField]
    public CharacterScriptable[] testplayerCharacters;

    [SerializeField]
    public CharacterScriptable[] testopponentCharacters;

    [HideInInspector]
    public List<CharacterScriptable> playerCharacters;

    [HideInInspector]
    public List<CharacterScriptable> opponentCharacters;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        //debug filling the objects
        for(int i = 0; i < testplayerCharacters.Length; ++i)
        {
            playerCharacters[i] = new CharacterScriptable(testplayerCharacters[i]);
        }
        for (int i = 0; i < testopponentCharacters.Length; ++i)
        {
            opponentCharacters[i] = new CharacterScriptable(testopponentCharacters[i]);
        }
    }

    private void Update()
    {
        
    }

}
