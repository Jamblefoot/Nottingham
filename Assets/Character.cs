using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    List<Tile> moves = new List<Tile>();


    int moveSpeed = 3;

    [SerializeField] GameObject tilePrefab;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask obstacleLayer;
    // Start is called before the first frame update
    void Start()
    {
        FindMoves();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
                GameObject move = GetGroundAtPosition(transform.position + Vector3.right * x + Vector3.forward * z);
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

    GameObject GetGroundAtPosition(Vector3 pos)
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
                    transform.position = new Vector3(x, pos.y, z);
                    FindMoves();
                    if(GameControl.instance.currentCharacter == this)
                        GameControl.instance.followCam.FocusOnCurrentCharacter();
                }
            }
        }
    }


}
