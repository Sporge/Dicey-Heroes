/*
 * Ryan Scheppler
 * 7/15/2021
 * Holds values and data to be preserved between scenes
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    [SerializeField]
    public CharacterScriptable[] testplayerCharacters;

    [SerializeField]
    public CharacterScriptable[] testopponentCharacters;

    [HideInInspector]
    public List<CharacterScriptable> playerCharacters = new List<CharacterScriptable>();

    [HideInInspector]
    public List<CharacterScriptable> opponentCharacters = new List<CharacterScriptable>();

    private static int _PlayerGold = 20;
    public static UnityEvent GoldUpdate = new UnityEvent();

    public static int PlayerGold
    {
        get
        {
            return _PlayerGold;
        }
        set
        {
            _PlayerGold = value;
            GoldUpdate.Invoke();
        }
    }



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
        if (testplayerCharacters.Length > 0 && testopponentCharacters.Length > 0)
        {
            for (int i = 0; i < 4; ++i)
            {
                playerCharacters[i] = ((CharacterScriptable)ScriptableObject.CreateInstance("CharacterScriptable"));
                opponentCharacters[i] = ((CharacterScriptable)ScriptableObject.CreateInstance("CharacterScriptable"));
            }
            //debug filling the objects
            for (int i = 0; i < testplayerCharacters.Length; ++i)
            {
                //print("adding : " + i);
                
                playerCharacters[i].CopyValues(testplayerCharacters[i]);

            }
            for (int i = 0; i < testopponentCharacters.Length; ++i)
            {
                //print("adding : " + i);
                
                opponentCharacters[i].CopyValues(testopponentCharacters[i]);
            }
        }
        
    }

    private void Update()
    {
        
    }

}
