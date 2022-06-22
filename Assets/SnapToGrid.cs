using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[ExecuteAlways]
public class SnapToGrid : MonoBehaviour
{
    Vector3 position;

    public bool locked;

    public LayerMask groundLayers;
    public List<int> blockLayers = new List<int>();


    private void OnDrawGizmos() 
    {
        Snap();
    }

    void Snap()
    {
        transform.position = EditModeRaycastGridPosition();
        //Vector3 pos = transform.position;
        //transform.position = new Vector3(Mathf.FloorToInt(pos.x), pos.y, Mathf.FloorToInt(pos.z));
    }


    /*// Start is called before the first frame update
    void Start()
    {
        if(Application.IsPlaying(gameObject))
        {
            // Play logic
        }
        else
        {
            // Editor logic
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        if (Application.IsPlaying(gameObject))
        {
            // Play Mode logic
            if(!locked)
            {
                //Vector3 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 targetPos = transform.position;
                RaycastHit hit;
                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, groundLayers, QueryTriggerInteraction.Ignore))
                {
                    if(!blockLayers.Contains(hit.collider.gameObject.layer))
                        targetPos = hit.point;
                }

                targetPos.x = Mathf.RoundToInt(targetPos.x);
                targetPos.z = Mathf.RoundToInt(targetPos.z);

                if (GameControl.instance.currentCharacter != null)
                {
                    if (GameControl.instance.currentCharacter.IsThisMove(targetPos))
                    {
                        transform.position = targetPos;

                        if(Input.GetButtonDown("Fire1"))
                        {
                            GameControl.instance.currentCharacter.Move(targetPos);
                        }
                    }

                    
                }
                else transform.position = targetPos;
            }
            
        }
        else
        {
            // Editor Mode logic
            /*if(Selection.Contains(gameObject))
            {
                //Vector3 screenPos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                Vector3 targetPos = transform.position;
                RaycastHit hit;
                if(Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hit, 1000f, groundLayers, QueryTriggerInteraction.Ignore))
                {
                    targetPos = hit.point;
                }

                targetPos.x = Mathf.FloorToInt(targetPos.x);
                targetPos.z = Mathf.FloorToInt(targetPos.z);

                transform.position = targetPos;
            }*/


        }
    }

    Vector3 EditModeRaycastGridPosition()
    {
        if (Selection.Contains(gameObject))
        {
            //Vector3 screenPos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Vector3 targetPos = transform.position;
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, groundLayers, QueryTriggerInteraction.Ignore))
            {
                

                if (!blockLayers.Contains(hit.collider.gameObject.layer))
                    targetPos = hit.point;
            }

            targetPos.x = Mathf.RoundToInt(targetPos.x);
            targetPos.z = Mathf.RoundToInt(targetPos.z);

            transform.position = targetPos;
        }

        return transform.position;
    }
}
