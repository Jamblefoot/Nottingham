using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    List<Tile> moves = new List<Tile>(); //RENAME THIS TO NODES OR SOMETHING
    
    Vector3[][] movePoints;

    List<Vector3> moveList = new List<Vector3>();


    int moveSpeed = 3;
    /*public */float moveAnimationSpeed = 10f;
    bool moving;

    [SerializeField] GameObject tilePrefab;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask obstacleLayer;

    public bool controllable;

    public bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Initialize", 0.5f);
        //FindMoves();
    }

    void Initialize()
    {
        FindPosition();
        SetupArrays();
        FindMoves();
        EnableNodes(false);
    }

    void SetupArrays()
    {
        movePoints = new Vector3[moveSpeed * 2 + 1][];
        for(int i = 0; i < movePoints.Length; i++)
        {
            movePoints[i] = new Vector3[moveSpeed * 2 + 1];
        }
    }

    void FindPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.z = Mathf.RoundToInt(pos.z);
        pos.y = GetGroundHeightAtPosition(pos);

        transform.position = pos;
    }

    void MoveToNearestNode(Vector3 pos)
    {

    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    void ClearMoves()
    {                       //THIS IS TEMPORARY, POOL THIS SHIT
        for(int i = moves.Count - 1; i >= 0; i--)
        {
            Destroy(moves[i].gameObject);
        }
        moves.Clear();
    }

    void FindMoves()
    {
        if(moves.Count > 0)
        {
            ClearMoves();
        }

        for(int x = -moveSpeed; x <= moveSpeed; x++)
        {
            for(int z = -moveSpeed; z <= moveSpeed; z++)
            {
                GameObject move = SetTileAtPosition(transform.position + Vector3.right * x + Vector3.forward * z);
                if(move != null)
                    moves.Add(move.GetComponent<Tile>());
            }
        }

        for(int i = moves.Count - 1; i >= 0; i--)
        {
            if(PathToPosition(moves[i].transform.position.x, 0, moves[i].transform.position.z).Length >= moveSpeed)
            {
                Destroy(moves[i].gameObject);
                moves.RemoveAt(i);
            }
        }
    }

    Tile[] PathToPosition(float x, float y, float z)
    {

        List<Tile> path = new List<Tile>();

        Tile currentTile = GetTileAt(x, y, z);
        /*while(Vector3.Distance(currentTile.tran.position, transform.position) > 1.3f)
        {

        }*/

        return path.ToArray();
    }

    Tile GetTileAt(float x, float y, float z)
    {
        for(int i = 0; i < moves.Count; i++)
        {
            if(Mathf.RoundToInt(moves[i].tran.position.x) == Mathf.RoundToInt(transform.position.x) 
                && Mathf.RoundToInt(moves[i].tran.position.z) == Mathf.RoundToInt(transform.position.z))
                return moves[i];
        }

        return null;
    }

    float GetGroundHeightAtPosition(Vector3 pos)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos + Vector3.up * 10, Vector3.down, out hit, 20f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            Vector3 groundPos = hit.point;

            return groundPos.y;

            /*if (Physics.Raycast(groundPos + Vector3.up * 10, Vector3.down, out hit, 9.5f, obstacleLayer, QueryTriggerInteraction.Ignore))
            {
                return null;
            }*/
        }

        return transform.position.y;
        
    }

    GameObject SetTileAtPosition(Vector3 pos)
    {
        RaycastHit hit;
        if(Physics.Raycast(pos + Vector3.up * 10, Vector3.down, out hit, 20f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            Vector3 groundPos = hit.point;
            /*for(int i = 0; i < 10; i++)
            {
                if(Physics.Raycast(groundPos + Vector3.up * 2 * i, Vector3.down, out hit, 2, obstacleLayer, QueryTriggerInteraction.Ignore))
                {
                    if(hit.distance >= 0.9f) // is half cover

                }
            }*/
            if (Physics.Raycast(groundPos + Vector3.up * 10, Vector3.down, out hit, 9.5f, obstacleLayer, QueryTriggerInteraction.Ignore))
            {
                return null;
            }

            GameObject tile = Instantiate(tilePrefab, groundPos, Quaternion.identity, transform);


            return tile;
        }

        return null;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        for(int i = 0; i < moves.Count; i++)
        {
            Gizmos.DrawSphere(moves[i].transform.position, 0.2f);
        }
    }

    public bool IsThisMove(Vector3 pos)
    {
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);
        int z = Mathf.RoundToInt(pos.z);

        for(int i = 0; i < moves.Count; i++)
        {
            if(Mathf.RoundToInt(moves[i].tran.position.x) == x)
            {
                if(Mathf.RoundToInt(moves[i].tran.position.z) == z)
                {
                    //SHOULD ADD Y POSITION STUFF, TOO
                    return true;
                }
            }
        }

        return false;
    }

    public void Move(Vector3 pos)
    {

        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);
        int z = Mathf.RoundToInt(pos.z);

        for (int i = 0; i < moves.Count; i++)
        {
            if (Mathf.RoundToInt(moves[i].tran.position.x) == x)
            {
                if (Mathf.RoundToInt(moves[i].tran.position.z) == z)
                {
                    //SHOULD ADD Y POSITION STUFF, TOO
                    //transform.position = new Vector3(x, pos.y, z);
                    moveList.Add(new Vector3(x, pos.y, z));
                    if (!moving)
                        StartCoroutine(MoveCo(moveSpeed));
                    //FindMoves();
                    //if(GameControl.instance.currentCharacter == this)
                        //GameControl.instance.followCam.FocusOnCurrentCharacter();
                }
            }
        }
    }

    IEnumerator MoveCo(float speed)
    {
        moving = true;
        EnableNodes(false);
        while(moveList.Count > 0)//Vector3.Distance(transform.position, pos) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, moveList[0], (speed * Time.deltaTime) / Vector3.Distance(transform.position, moveList[0]));
            if((transform.position - moveList[0]).sqrMagnitude < 0.09f)
                moveList.RemoveAt(0);

            if (GameControl.instance.currentCharacter == this && GameControl.instance.followCam.cam.orthographicSize < 5f)
                GameControl.instance.followCam.FocusOnCurrentCharacter();
            yield return null;
        }

        moving = false;
        FindMoves();
        if(GameControl.instance.currentCharacter == this)
        {
            //GameControl.instance.followCam.FocusOnCurrentCharacter();
            EnableNodes(true);
        }
        else EnableNodes(false);
    }

    public void EnableNodes(bool setting)
    {
        for(int i = 0; i < moves.Count; i++)
        {
            moves[i].gameObject.SetActive(setting);
        }
    }


}
