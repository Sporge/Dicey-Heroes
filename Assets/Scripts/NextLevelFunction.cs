/*
 * Ryan Scheppler
 * 7/15/2021
 * function for use by other events to change the level.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelFunction : MonoBehaviour
{
    public string nextLevel;
    public void NextLevel()
    {
        print("Debug: "+ nextLevel + " Loading");
        SceneManager.LoadScene(nextLevel);
    }
}
