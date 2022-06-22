using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    public Character currentCharacter;
    
    // Start is called before the first frame update
    void Start()
    {
        if(GameControl.instance != null)
            DestroyImmediate(this);
        else GameControl.instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
