using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] GameObject top;
    [SerializeField] GameObject bottom;
    [SerializeField] GameObject left;
    [SerializeField] GameObject right;

    public Transform tran;


    // Start is called before the first frame update
    void Awake()
    {
        tran = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
