/*
 * Ryan Scheppler
 * 7/15/2021
 * When object gets clicked disable or enable a whole object
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeVisibleOnClick : MonoBehaviour
{
    public GameObject turnThisOn;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void OnMouseDown()
    {
        
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1)){
            //do stuff here
            turnThisOn.SetActive(!turnThisOn.activeInHierarchy);
        }
    }
}
