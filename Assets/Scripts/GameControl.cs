using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    public Character currentCharacter;

    public CameraControl followCam;

    public LayerMask characterLayer;
    
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
        if (Input.GetButtonDown("Fire1"))//GetMouseButton(0))
        {
            Debug.Log("GameControl looking for character");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Casts the ray and get the first game object hit
            if(Physics.Raycast(ray, out hit, 1000, characterLayer, QueryTriggerInteraction.Ignore))
            {
                //if(hit.collider.gameObject.layer == characterLayer)
                //{
                Debug.Log("GameControl fount potential character");
                Character c = hit.collider.GetComponentInParent<Character>();
                if(c != null && c.controllable)
                {
                    if(currentCharacter == null)
                    {
                        currentCharacter = c;
                        currentCharacter.EnableNodes(true);
                    } 
                    else if(c != currentCharacter)
                    {
                        currentCharacter.EnableNodes(false);
                        currentCharacter = c;
                        c.EnableNodes(true);
                    }
                }
                //}
            }
        }
    }
}
