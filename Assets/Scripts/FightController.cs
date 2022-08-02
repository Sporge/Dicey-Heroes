/*
 * Ryan Scheppler
 * 7/15/2021
 * Overly complicated auto fight system
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FightController : MonoBehaviour
{
    public GameObject characterPrefab;

    public Vector3[] PlayerPositions;
    public Vector3[] OpponentPositions;

    private GameObject[] PlayerObjects;
    private GameObject[] OpponentObjects;
    //spawning dice on screen
    public Vector3 diceOffset = new Vector3(0, 2, 0);
    public float diceSpeedMax = 10;
    public float diceSpeedMin = 5;
    public GameObject attackSymbol;
    public GameObject defenseSymbol;
    public GameObject specialSymbol;
    public GameObject dplus1;
    public GameObject d4;
    public GameObject d6;
    public GameObject d8;
    public GameObject d10;
    public GameObject d12;
    public GameObject d20;
    public Color PlayerColor = new Color(20, 50, 230);
    public Color OpponentColor = new Color(230, 50, 20);

    public float turnTime = 1;

    private int turn = 0;

    //next level info
    public string winLevel = "Won Screen";
    public string loseLevel = "Lost Screen";

    public bool debugging = false;
    public CharacterScriptable[] debugPlayerTeam;
    public CharacterScriptable[] debugOpponentTeam;


    // Start is called before the first frame update
    void Start()
    {
        if (debugging)
            DebugFillCharacters();

        System.Array.Resize(ref PlayerObjects, GameManager.PlayerTeam.Team.Length);
        System.Array.Resize(ref OpponentObjects, GameManager.OpponentTeam.Team.Length);
        PlaceCharacters(GameManager.PlayerTeam.Team, PlayerPositions, PlayerObjects, false);
        PlaceCharacters(GameManager.OpponentTeam.Team, OpponentPositions, OpponentObjects, true);

        StartCoroutine(RunTurn());

    }

    void DebugFillCharacters()
    {
        for (int i = 0; i < GameManager.PlayerTeam.Team.Length; ++i)
        {
            GameManager.PlayerTeam.Team[i].CopyValues(debugPlayerTeam[i]);
            GameManager.OpponentTeam.Team[i].CopyValues(debugOpponentTeam[i]);
            print(debugPlayerTeam[i].attackDice[0].dieType);
            //print(GameManager.PlayerTeam.Team[i].attackDice[0]);
        }
    }

    void PlaceCharacters(CharacterScriptable[] team, Vector3[] positions, GameObject[] objects, bool flip)
    {
        //loop through and place player characters
        for (int i = 0; i < team.Length; ++i)
        {
            GameObject clone;
            //Incase just a basic positioning is used (can overflow offscreen?) may be remove later
            if (i >= positions.Length)
            {

                clone = Instantiate(characterPrefab, new Vector3(-1.5f + -1 * i, -1, 0), Quaternion.identity);
            }
            else
            {
                
                clone = Instantiate(characterPrefab, positions[i], Quaternion.identity);
            }
            CharacterDisplay display = clone.GetComponent<CharacterDisplay>();
            display.character = team[i];
            if (flip)
            {
                clone.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            display.FillFields();
            objects[i] = clone;
        }
    }

    IEnumerator RunTurn()
    {

        //pause first for a turn time to let player see scene more(also pauses here between turns
        yield return new WaitForSeconds(turnTime);
        print("Turn" + turn++);

        
        //first front characters fight (the first with health)
        for (int p = 0; p < GameManager.PlayerTeam.Team.Length; ++p)
        {

            if (GameManager.PlayerTeam.Team[p].health > 0)
            {
                for (int o = 0; o < GameManager.OpponentTeam.Team.Length; ++o)
                {

                    if (GameManager.OpponentTeam.Team[o].health > 0)
                    {

                        //trigger attacks, defense,  and damage
                        int pAttack = RollDice(GameManager.PlayerTeam.Team[p].attackDice, GameManager.PlayerTeam.Team[p].ability, PlayerObjects[p].transform.position, PlayerColor);
                        int oAttack = RollDice(GameManager.OpponentTeam.Team[o].attackDice, GameManager.OpponentTeam.Team[o].ability, OpponentObjects[o].transform.position, OpponentColor);

                        print("Player Attack: " + pAttack);
                        print("Enemy Attack: " + oAttack);
                        yield return new WaitForSeconds(turnTime);

                        int pDefense = RollDice(GameManager.PlayerTeam.Team[p].defenseDice, GameManager.PlayerTeam.Team[p].ability, PlayerObjects[p].transform.position, PlayerColor);
                        int oDefense = RollDice(GameManager.OpponentTeam.Team[o].defenseDice, GameManager.OpponentTeam.Team[o].ability, OpponentObjects[o].transform.position, OpponentColor);

                        print("Player Defense: " + pDefense);
                        print("Enemy Defense: " + oDefense);

                        print("Player Health before: " + GameManager.PlayerTeam.Team[p].health);
                        print("Enemy Health before: " + GameManager.OpponentTeam.Team[o].health);
                        yield return new WaitForSeconds(turnTime);

                        int pDamage = Mathf.Max(0, pAttack - oDefense);
                        int oDamage = Mathf.Max(0, oAttack - pDefense);

                        GameManager.PlayerTeam.Team[p].health -= oDamage;
                        GameManager.OpponentTeam.Team[o].health -= pDamage;

                        if (GameManager.PlayerTeam.Team[p].health < 0)
                            GameManager.PlayerTeam.Team[p].health = 0;
                        if (GameManager.OpponentTeam.Team[o].health < 0)
                            GameManager.OpponentTeam.Team[o].health = 0;

                        print("Player Health after: " + GameManager.PlayerTeam.Team[p].health);
                        print("Enemy Health after: " + GameManager.OpponentTeam.Team[o].health);
                        yield return new WaitForSeconds(turnTime);
                        //if one dies move the others forward, remove the one, possible ability triggers
                        //attacks done from front break loop

                        ShiftUnits(GameManager.PlayerTeam.Team, PlayerObjects, PlayerPositions);
                        ShiftUnits(GameManager.OpponentTeam.Team, OpponentObjects, OpponentPositions);

                        break;
                    }
                    //if no enemy has health trigger end
                    else if (o == GameManager.OpponentTeam.Team.Length - 1)
                    {

                        print("Opponent Lost.");
                        Invoke("WonLevel", turnTime);
                        yield break;
                    }
                }
                //attacks done from front break loop
                break;
            }
            //if no character has health trigger end
            else if (p == GameManager.PlayerTeam.Team.Length - 1)
            {
                //quick tie check
                for (int o = 0; o < GameManager.OpponentTeam.Team.Length; ++o)
                {
                    if (GameManager.OpponentTeam.Team[o].health != 0)
                    {
                        break;
                    }
                    else if (o == GameManager.OpponentTeam.Team.Length - 1)
                    {
                        print("Tie");
                        Invoke("WonLevel", turnTime);
                        yield break;
                    }
                }

                print("Player Lost.");
                Invoke("LostLevel", turnTime);
                yield break;
            }
        }
        

        
        //then loop through other characters behind in order and trigger their abilities
        bool pNotFirst = false;
        bool oNotFirst = false;
        //keep track for magic missile first one with health
        int pInFront = 0;
        int oInFront = 0;
        int pToHeal = 0;
        int oToHeal = 0;
        int pi = 0, oj = 0;

        while (pi < GameManager.PlayerTeam.Team.Length || oj < GameManager.OpponentTeam.Team.Length)
        {
            bool waitNeeded = false;
            //move to next valid characters in each team
            while (pi < GameManager.PlayerTeam.Team.Length && GameManager.PlayerTeam.Team[pi].health <= 0)
            {
                ++pi;
            }

            while (oj < GameManager.OpponentTeam.Team.Length && GameManager.OpponentTeam.Team[oj].health <= 0)
            {
                ++oj;
            }

            if (pNotFirst && pi < GameManager.PlayerTeam.Team.Length)
            {
                switch (GameManager.PlayerTeam.Team[pi].ability)
                {
                    case Abilities.Archery:
                        waitNeeded = true;
                        int pArrowAttack = RollDice(GameManager.PlayerTeam.Team[pi].specialDice, GameManager.PlayerTeam.Team[pi].ability, PlayerObjects[pi].transform.position, PlayerColor);
                        int otarget = GameManager.OpponentTeam.Team.Length - 1;
                        while (otarget >= 0 && GameManager.OpponentTeam.Team[otarget].health == 0)
                        {
                            otarget--;
                        }
                        if (otarget < 0)
                            break;
                        int oDefense = RollDice(GameManager.OpponentTeam.Team[otarget].defenseDice, GameManager.OpponentTeam.Team[otarget].ability, OpponentObjects[otarget].transform.position, OpponentColor);
                        int pArrowDamage = Mathf.Max(0, pArrowAttack - oDefense);

                        GameManager.OpponentTeam.Team[otarget].health -= pArrowDamage;

                        if (GameManager.OpponentTeam.Team[otarget].health < 0)
                            GameManager.OpponentTeam.Team[otarget].health = 0;

                        break;
                    case Abilities.MagicMissile:
                        waitNeeded = true;
                        int pMagicAttack = RollDice(GameManager.PlayerTeam.Team[pi].specialDice, GameManager.PlayerTeam.Team[pi].ability, PlayerObjects[pi].transform.position, PlayerColor);

                        GameManager.OpponentTeam.Team[oInFront].health -= pMagicAttack;


                        if (GameManager.OpponentTeam.Team[oInFront].health < 0)
                            GameManager.OpponentTeam.Team[oInFront].health = 0;
                        break;
                    case Abilities.HealInFront:
                        waitNeeded = true;
                        int pHealing = RollDice(GameManager.PlayerTeam.Team[pi].specialDice, GameManager.PlayerTeam.Team[pi].ability, PlayerObjects[pi].transform.position, PlayerColor);
                        GameManager.PlayerTeam.Team[pToHeal].health += pHealing;

                        if (GameManager.PlayerTeam.Team[pToHeal].health > GameManager.PlayerTeam.Team[pToHeal].maxHealth)
                            GameManager.PlayerTeam.Team[pToHeal].health = GameManager.PlayerTeam.Team[pToHeal].maxHealth;
                        break;
                }
            }
            if (oNotFirst && oj < GameManager.OpponentTeam.Team.Length)
            {
                switch (GameManager.OpponentTeam.Team[oj].ability)
                {
                    case Abilities.Archery:
                        waitNeeded = true;
                        int oArrowAttack = RollDice(GameManager.OpponentTeam.Team[oj].specialDice, GameManager.OpponentTeam.Team[oj].ability, OpponentObjects[oj].transform.position, OpponentColor);
                        int ptarget = GameManager.PlayerTeam.Team.Length - 1;
                        while (ptarget >= 0 && GameManager.PlayerTeam.Team[ptarget].health == 0)
                        {
                            ptarget--;
                        }
                        if (ptarget < 0)
                            break;
                        int pDefense = RollDice(GameManager.PlayerTeam.Team[ptarget].defenseDice, GameManager.PlayerTeam.Team[ptarget].ability, PlayerObjects[ptarget].transform.position, PlayerColor);
                        int oArrowDamage = Mathf.Max(0, oArrowAttack - pDefense);

                        GameManager.PlayerTeam.Team[ptarget].health -= oArrowDamage;

                        if (GameManager.PlayerTeam.Team[ptarget].health < 0)
                            GameManager.PlayerTeam.Team[ptarget].health = 0;
                        break;
                    case Abilities.MagicMissile:
                        waitNeeded = true;
                        int oMagicAttack = RollDice(GameManager.OpponentTeam.Team[oj].specialDice, GameManager.OpponentTeam.Team[oj].ability, OpponentObjects[oj].transform.position, OpponentColor);

                        GameManager.PlayerTeam.Team[pInFront].health -= oMagicAttack;


                        if (GameManager.PlayerTeam.Team[pInFront].health < 0)
                            GameManager.PlayerTeam.Team[pInFront].health = 0;
                        break;
                    case Abilities.HealInFront:
                        waitNeeded = true;
                        int oHealing = RollDice(GameManager.OpponentTeam.Team[oj].specialDice, GameManager.OpponentTeam.Team[oj].ability, OpponentObjects[oj].transform.position, OpponentColor);
                        GameManager.OpponentTeam.Team[oToHeal].health += oHealing;

                        if (GameManager.OpponentTeam.Team[oToHeal].health > GameManager.OpponentTeam.Team[oToHeal].maxHealth)
                            GameManager.OpponentTeam.Team[oToHeal].health = GameManager.OpponentTeam.Team[oToHeal].maxHealth;
                        break;
                }
            }

            if (pNotFirst && pi < GameManager.PlayerTeam.Team.Length)
            {
                //if one dies move the others forward, remove the one
                ShiftUnits(GameManager.OpponentTeam.Team, OpponentObjects, OpponentPositions);
            }
            if (oNotFirst && oj < GameManager.OpponentTeam.Team.Length)
            {
                //if one dies move the others forward, remove the one
                ShiftUnits(GameManager.PlayerTeam.Team, PlayerObjects, PlayerPositions);
            }


            if (pi < GameManager.PlayerTeam.Team.Length && GameManager.PlayerTeam.Team[pi].health > 0)
            {
                if (!pNotFirst)
                {
                    pInFront = pi;
                }
                pNotFirst = true;
                pToHeal = pi;
            }
            if (oj < GameManager.OpponentTeam.Team.Length && GameManager.OpponentTeam.Team[oj].health > 0)
            {
                if (!oNotFirst)
                {
                    oInFront = oj;
                }
                oNotFirst = true;
                oToHeal = oj;
            }
            pi++;
            oj++;
            if (waitNeeded)
            {
                yield return new WaitForSeconds(turnTime);
            }
        }

        

        //keep going till end point reached
        StartCoroutine(RunTurn());


    }

    void WonLevel()
    {
        SceneManager.LoadScene(winLevel);
    }
    void LostLevel()
    {
        SceneManager.LoadScene(loseLevel);
    }

    void ShiftUnits(CharacterScriptable[] stats, GameObject[] objects, Vector3[] positions)
    {
        int p = 0;
        for (int i = 0; i < objects.Length; ++i)
        {
            if (stats[i].health <= 0)
            {
                objects[i].SetActive(false);
            }
            else
            {
                objects[i].transform.position = positions[p];
                ++p;
            }
        }

    }

    public int RollDice(List<dice> toRoll, Abilities ability, Vector3 diceSpawnLocation, Color dieColor)
    {
        int roll = 0;
        //for all dice groups
        foreach(dice d in toRoll)
        { 
            //for each individual die
            for (int j = 0; j < d.dieNum; ++j)
            {
                int dieRoll = 0;
                if (ability == Abilities.Criticals)
                {
                    //if crits are active on unit check for dice exploding on a max roll
                    int critcheck;
                    do
                    {
                        critcheck = Random.Range(1, d.dieType + 1);
                        SpawnDie(d.dieType, critcheck, diceSpawnLocation, dieColor);
                        dieRoll += critcheck;
                    }
                    while (critcheck == d.dieType);
                }
                //normal dice rolling
                else
                {

                    dieRoll = Random.Range(1, d.dieType + 1);
                    SpawnDie(d.dieType, dieRoll, diceSpawnLocation, dieColor);
                }
                roll += dieRoll;
            }
        }

        return roll;
    }
    //spawn object of a specific dietype (run for each die rolled
    private void SpawnDie(int type, int roll, Vector3 diceSpawnLocation, Color dieColor)
    {
        Vector3 direction = Random.insideUnitSphere;
        direction.z = 0;
        direction *= Random.Range(diceSpeedMin, diceSpeedMax);
        Vector3 spawnPos = diceSpawnLocation + diceOffset;
        GameObject clone;
        //what kind of die?
        switch (type)
        {
            case 4:
                clone = Instantiate(d4, spawnPos, Quaternion.identity);
                break;
            case 6:
                clone = Instantiate(d6, spawnPos, Quaternion.identity);
                break;
            case 8:
                clone = Instantiate(d8, spawnPos, Quaternion.Euler(0, 0, 180));
                break;
            case 10:
                clone = Instantiate(d10, spawnPos, Quaternion.identity);
                break;
            case 12:
                clone = Instantiate(d12, spawnPos, Quaternion.identity);
                break;
            case 20:
                clone = Instantiate(d20, spawnPos, Quaternion.identity);
                break;
                //default to add 1 (for barbarians mainly but could be used on other bonuses in future)
            default:
                clone = Instantiate(dplus1, spawnPos, Quaternion.identity);
                break;
        }
        //set die properties
        clone.GetComponent<Rigidbody2D>().velocity = direction;
        clone.GetComponent<SpriteRenderer>().color = dieColor;
        TMP_Text text = clone.GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = roll.ToString();
        }
    }


    /*
    // Start is called before the first frame update
    void Start()
    {
        System.Array.Resize(ref PlayerObjects, PlayerPositions.Length);
        System.Array.Resize(ref OpponentObjects, OpponentPositions.Length);
        //loop through and place player characters
        for (int i = 0; i < GameManager.instance.playerCharacters.Count && GameManager.instance.playerCharacters[i].attackDice != null; ++i)
        {
            GameObject clone;
            //print("clone: " + i);
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
        //loop through and place opponent characters
        for (int i = 0; i < GameManager.instance.opponentCharacters.Count && GameManager.instance.opponentCharacters[i].attackDice != null; ++i)
        {
            GameObject clone;
            //print("oclone: " + i);
            if (i >= OpponentPositions.Length)
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
            OpponentObjects[i] = clone;
        }
        StartCoroutine(RunTurn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator RunTurn()
    {
        yield return new WaitForSeconds(turnTime);
        print("Turn" + turn++);

        //first front characters fight (the first with health)
        for (int p = 0; p < GameManager.instance.playerCharacters.Count; ++p)
        {

            if (GameManager.instance.playerCharacters[p].health > 0)
            {
                for (int o = 0; o < GameManager.instance.opponentCharacters.Count; ++o)
                {

                    if (GameManager.instance.opponentCharacters[o].health > 0)
                    {

                        //trigger attacks, defense,  and damage
                        int pAttack = RollDice(GameManager.instance.playerCharacters[p].attackDice, GameManager.instance.playerCharacters[p].ability, PlayerObjects[p].transform.position, PlayerColor);
                        int oAttack = RollDice(GameManager.instance.opponentCharacters[o].attackDice, GameManager.instance.opponentCharacters[o].ability, OpponentObjects[o].transform.position, OpponentColor);

                        //print("Player Attack: " + pAttack);
                        //print("Enemy Attack: " + oAttack);
                        yield return new WaitForSeconds(turnTime);

                        int pDefense = RollDice(GameManager.instance.playerCharacters[p].defenseDice, GameManager.instance.playerCharacters[p].ability, PlayerObjects[p].transform.position, PlayerColor);
                        int oDefense = RollDice(GameManager.instance.opponentCharacters[o].defenseDice, GameManager.instance.opponentCharacters[o].ability, OpponentObjects[o].transform.position, OpponentColor);

                       // print("Player Defense: " + pDefense);
                        //print("Enemy Defense: " + oDefense);

                       // print("Player Health before: " + GameManager.instance.playerCharacters[p].health);
                       // print("Enemy Health before: " + GameManager.instance.opponentCharacters[o].health);
                        yield return new WaitForSeconds(turnTime);

                        int pDamage = Mathf.Max(0, pAttack - oDefense);
                        int oDamage = Mathf.Max(0, oAttack - pDefense);

                        GameManager.instance.playerCharacters[p].health -= oDamage;
                        GameManager.instance.opponentCharacters[o].health -= pDamage;

                        if (GameManager.instance.playerCharacters[p].health < 0)
                            GameManager.instance.playerCharacters[p].health = 0;
                        if (GameManager.instance.opponentCharacters[o].health < 0)
                            GameManager.instance.opponentCharacters[o].health = 0;

                        print("Player Health after: " + GameManager.instance.playerCharacters[p].health);
                        print("Enemy Health after: " + GameManager.instance.opponentCharacters[o].health);
                        yield return new WaitForSeconds(turnTime);
                        //if one dies move the others forward, remove the one, possible ability triggers
                        //attacks done from front break loop

                        ShiftUnits(GameManager.instance.playerCharacters, PlayerObjects, PlayerPositions);
                        ShiftUnits(GameManager.instance.opponentCharacters, OpponentObjects, OpponentPositions);

                        break;
                    }
                    //if no enemy has health trigger end
                    else if (o == GameManager.instance.opponentCharacters.Count - 1)
                    {

                        print("Opponent Lost.");
                        Invoke("WonLevel", turnTime);
                        yield break;
                    }
                }
                //attacks done from front break loop
                break;
            }
            //if no character has health trigger end
            else if (p == GameManager.instance.playerCharacters.Count - 1)
            {
                //quick tie check
                for (int o = 0; o < GameManager.instance.opponentCharacters.Count; ++o)
                {
                    if (GameManager.instance.opponentCharacters[o].health != 0)
                    {
                        break;
                    }
                    else if (o == GameManager.instance.opponentCharacters.Count - 1)
                    {
                        print("Tie");
                        Invoke("WonLevel", turnTime);
                        yield break;
                    }
                }

                print("Player Lost.");
                Invoke("LostLevel", turnTime);
                yield break;
            }
        }


        //then loop through other characters behind in order and trigger their abilities
        bool pNotFirst = false;
        bool oNotFirst = false;
        //keep track for magic missile first one with health
        int pInFront = 0;
        int oInFront = 0;
        int pToHeal = 0;
        int oToHeal = 0;
        int pi = 0, oj = 0;

        while(pi < GameManager.instance.playerCharacters.Count || oj < GameManager.instance.opponentCharacters.Count )
        {   
            bool waitNeeded = false;

            while(pi < GameManager.instance.playerCharacters.Count && GameManager.instance.playerCharacters[pi].health <= 0)
            {
                ++pi;

            }

            while (pi < GameManager.instance.playerCharacters.Count && GameManager.instance.playerCharacters[oj].health <= 0)
            {
                ++oj;

            }

            if (pNotFirst && pi < GameManager.instance.playerCharacters.Count)
            {
                switch(GameManager.instance.playerCharacters[pi].ability)
                {
                    case Abilities.Archery:
                        waitNeeded = true;
                        int pArrowAttack = RollDice(GameManager.instance.playerCharacters[pi].specialDice, GameManager.instance.playerCharacters[pi].ability, PlayerObjects[pi].transform.position, PlayerColor);
                        int otarget = GameManager.instance.opponentCharacters.Count - 1;
                        while (otarget >= 0 && GameManager.instance.opponentCharacters[otarget].health == 0 )
                        {
                            otarget--;
                        }
                        if (otarget < 0)
                            break;
                        int oDefense = RollDice(GameManager.instance.opponentCharacters[otarget].defenseDice, GameManager.instance.opponentCharacters[otarget].ability, OpponentObjects[otarget].transform.position, OpponentColor);
                        int pArrowDamage = Mathf.Max(0, pArrowAttack - oDefense);

                        GameManager.instance.opponentCharacters[otarget].health -= pArrowDamage;

                        if (GameManager.instance.opponentCharacters[otarget].health < 0)
                            GameManager.instance.opponentCharacters[otarget].health = 0;

                        break;
                    case Abilities.MagicMissile:
                        waitNeeded = true;
                        int pMagicAttack = RollDice(GameManager.instance.playerCharacters[pi].specialDice, GameManager.instance.playerCharacters[pi].ability, PlayerObjects[pi].transform.position, PlayerColor);

                        GameManager.instance.opponentCharacters[oInFront].health -= pMagicAttack;


                        if (GameManager.instance.opponentCharacters[oInFront].health < 0)
                            GameManager.instance.opponentCharacters[oInFront].health = 0;
                        break;
                    case Abilities.HealInFront:
                        waitNeeded = true;
                        int pHealing = RollDice(GameManager.instance.playerCharacters[pi].specialDice, GameManager.instance.playerCharacters[pi].ability, PlayerObjects[pi].transform.position, PlayerColor);
                        GameManager.instance.playerCharacters[pToHeal].health += pHealing;

                        if (GameManager.instance.playerCharacters[pToHeal].health > GameManager.instance.playerCharacters[pToHeal].maxHealth)
                            GameManager.instance.playerCharacters[pToHeal].health = GameManager.instance.playerCharacters[pToHeal].maxHealth;
                        break;
                }
            }
            if (oNotFirst && oj < GameManager.instance.opponentCharacters.Count)
            {
                switch (GameManager.instance.opponentCharacters[oj].ability)
                {
                    case Abilities.Archery:
                        waitNeeded = true;
                        int oArrowAttack = RollDice(GameManager.instance.opponentCharacters[oj].specialDice, GameManager.instance.opponentCharacters[oj].ability, OpponentObjects[oj].transform.position, OpponentColor);
                        int ptarget = GameManager.instance.playerCharacters.Count - 1;
                        while(ptarget >=0 && GameManager.instance.playerCharacters[ptarget].health == 0 )
                        {
                            ptarget--;
                        }
                        if (ptarget < 0)
                            break;
                        int pDefense = RollDice(GameManager.instance.playerCharacters[ptarget].defenseDice, GameManager.instance.playerCharacters[ptarget].ability, PlayerObjects[ptarget].transform.position, PlayerColor);
                        int oArrowDamage =  Mathf.Max(0, oArrowAttack - pDefense);

                        GameManager.instance.playerCharacters[ptarget].health -= oArrowDamage;

                        if (GameManager.instance.playerCharacters[ptarget].health < 0)
                            GameManager.instance.playerCharacters[ptarget].health = 0;
                        break;
                    case Abilities.MagicMissile:
                        waitNeeded = true;
                        int oMagicAttack = RollDice(GameManager.instance.opponentCharacters[oj].specialDice, GameManager.instance.opponentCharacters[oj].ability, OpponentObjects[oj].transform.position, OpponentColor);

                        GameManager.instance.playerCharacters[pInFront].health -= oMagicAttack;


                        if (GameManager.instance.playerCharacters[pInFront].health < 0)
                            GameManager.instance.playerCharacters[pInFront].health = 0;
                        break;
                    case Abilities.HealInFront:
                        waitNeeded = true;
                        int oHealing = RollDice(GameManager.instance.opponentCharacters[oj].specialDice, GameManager.instance.opponentCharacters[oj].ability, OpponentObjects[oj].transform.position, OpponentColor);
                        GameManager.instance.opponentCharacters[oToHeal].health += oHealing;

                        if (GameManager.instance.opponentCharacters[oToHeal].health > GameManager.instance.opponentCharacters[oToHeal].maxHealth)
                            GameManager.instance.opponentCharacters[oToHeal].health = GameManager.instance.opponentCharacters[oToHeal].maxHealth;
                        break;
                }
            }

            if (pNotFirst && pi < GameManager.instance.playerCharacters.Count)
            {
                //if one dies move the others forward, remove the one
                ShiftUnits(GameManager.instance.opponentCharacters, OpponentObjects, OpponentPositions);
            }
            if (oNotFirst && oj < GameManager.instance.opponentCharacters.Count)
            {
                //if one dies move the others forward, remove the one
                ShiftUnits(GameManager.instance.playerCharacters, PlayerObjects, PlayerPositions);
            }


            if (pi < GameManager.instance.playerCharacters.Count && GameManager.instance.playerCharacters[pi].health > 0)
            {
                if(!pNotFirst)
                {
                    pInFront = pi;
                }
                pNotFirst = true;
                pToHeal = pi;
            }
            if (oj < GameManager.instance.opponentCharacters.Count && GameManager.instance.opponentCharacters[oj].health > 0)
            {
                if (!oNotFirst)
                {
                    oInFront = oj;
                }
                oNotFirst = true;
                oToHeal = oj;
            }
            pi++;
            oj++;
            if (waitNeeded)
            {
                yield return new WaitForSeconds(turnTime);
            }
        }



        //keep going till end point reached
        StartCoroutine(RunTurn());
    }


    void ShiftUnits(List<CharacterScriptable> stats, GameObject[] objects, Vector3[] positions)
    {
        int p = 0;
        for(int i = 0; i < objects.Length; ++i)
        {
            if(stats[i].health <= 0)
            {
                objects[i].SetActive(false);
            }
            else
            {
                objects[i].transform.position = positions[p];
                ++p;
            }
        }

    }

    public int RollDice(dice[] toRoll, Abilities ability, Vector3 diceSpawnLocation, Color dieColor)
    {
        int roll = 0;
        for (int i = 0; i < toRoll.Length; ++i)
        {

            for (int j = 0; j < toRoll[i].dieNum; ++j)
            {
                int dieRoll = 0;
                if (ability == Abilities.Criticals)
                {
                    //if crits are active on unit check for dice exploding on a max roll
                    int critcheck;
                    do
                    {
                        critcheck = Random.Range(1, toRoll[i].dieType + 1);
                        SpawnDie(toRoll[i].dieType, critcheck, diceSpawnLocation, dieColor);
                        dieRoll += critcheck;
                    }
                    while (critcheck == toRoll[i].dieType);
                }
                else
                {

                    dieRoll = Random.Range(1, toRoll[i].dieType + 1);
                    SpawnDie(toRoll[i].dieType, dieRoll, diceSpawnLocation, dieColor);
                }
                roll += dieRoll;
            }
        }

        return roll;
    }

    private void SpawnDie(int type, int roll, Vector3 diceSpawnLocation, Color dieColor)
    {
        Vector3 direction = Random.insideUnitSphere;
        direction.z = 0;
        direction *= Random.Range(diceSpeedMin, diceSpeedMax);
        Vector3 spawnPos = diceSpawnLocation + diceOffset;
        GameObject clone;
        switch (type)
        {
            case 4:
                clone = Instantiate(d4, spawnPos, Quaternion.identity);
                break;
            case 6:
                clone = Instantiate(d6, spawnPos, Quaternion.identity);
                break;
            case 8:
                clone = Instantiate(d8, spawnPos, Quaternion.Euler(0,0,180));
                break;
            case 10:
                clone = Instantiate(d10, spawnPos, Quaternion.identity);
                break;
            case 12:
                clone = Instantiate(d12, spawnPos, Quaternion.identity);
                break;
            case 20:
                clone = Instantiate(d20, spawnPos, Quaternion.identity);
                break;
            default:
                clone = Instantiate(dplus1, spawnPos, Quaternion.identity);
                break;
        }

        clone.GetComponent<Rigidbody2D>().velocity = direction;
        clone.GetComponent<SpriteRenderer>().color = dieColor;
        TMP_Text text = clone.GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = roll.ToString();
        }
    }

    void WonLevel()
    {
        SceneManager.LoadScene(winLevel);
    }
    void LostLevel()
    {
        SceneManager.LoadScene(loseLevel);
    }
    */
}
