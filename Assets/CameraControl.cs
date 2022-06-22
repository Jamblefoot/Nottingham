using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Camera cam;
    Transform housing;

    Quaternion rotTarget;
    Vector3 posTarget;

    float moveSpeed = 10f;

    float minOrthoSize = 1;
    float maxOrthoSize = 20;

    Vector3 orthoPosition;
    Vector3 perspectivePosition = Vector3.up * 2f;

    Quaternion orthoCamRot;
    float orthoHouseRot = -45;

    Vector3 headRot = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        housing = transform.parent;
        rotTarget = housing.rotation;
        orthoPosition = cam.transform.localPosition;
        orthoCamRot = cam.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float scroll = Input.mouseScrollDelta.y;
        //housing.rotation = housing.rotation * Quaternion.Euler(0, -horizontal, 0);

        cam.orthographicSize -= scroll;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minOrthoSize, maxOrthoSize);
        if(cam.orthographicSize < minOrthoSize + 0.1f)
        {
            if(cam.orthographic)// && GameControl.instance.currentCharacter != null)
            {
                cam.orthographic = false;
                cam.transform.localPosition = perspectivePosition;
                headRot = cam.transform.localRotation.eulerAngles;
                headRot.z = 0f;

                if (GameControl.instance.currentCharacter != null)
                {
                    housing.position = GameControl.instance.currentCharacter.transform.position;
                }
            }
        }
        else 
        {
            if(!cam.orthographic)
            {
                cam.orthographic = true;
                cam.transform.localPosition = orthoPosition;
                cam.transform.localRotation = orthoCamRot;
                rotTarget = Quaternion.Euler(0, orthoHouseRot, 0);

                if(GameControl.instance.currentCharacter != null)
                {
                    housing.position = GameControl.instance.currentCharacter.transform.position;
                }
            }
        }


        if(cam.orthographic)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                //housing.rotation = housing.rotation * Quaternion.Euler(0, 90, 0);
                rotTarget = rotTarget * Quaternion.Euler(0, 90, 0);
                orthoHouseRot = rotTarget.eulerAngles.y + 90;
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                //housing.rotation = housing.rotation * Quaternion.Euler(0, 90, 0);
                rotTarget = rotTarget * Quaternion.Euler(0, -90, 0);
                orthoHouseRot = rotTarget.eulerAngles.y - 90;
            }

            if(Mathf.Abs(horizontal) > 0.1f)
            {
                housing.position += Time.deltaTime * housing.right * horizontal * moveSpeed;
            }
            if(Mathf.Abs(vertical) > 0.1f)
            {
                housing.position += Time.deltaTime * housing.forward * vertical * moveSpeed;
            }

            //cam.orthographicSize -= scroll;
            //cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minOrthoSize, maxOrthoSize);
            //if(cam.orthographicSize < minOrthoSize)

            if(Input.GetButtonDown("Jump"))
                FocusOnCurrentCharacter();
        }
        else //!cam.orthographic
        {

            //HANDLE HEAD ROTATION
            
            headRot.x -= mouseY;
            headRot.y += mouseX;
            if(Mathf.Abs(headRot.x) > 80)
                headRot.x = 80 * Mathf.Sign(headRot.x);
            headRot.x %= 360;
            cam.transform.localRotation = Quaternion.Euler(headRot);
        }

        housing.rotation = Quaternion.Slerp(housing.rotation, rotTarget, (Time.deltaTime * 100f) / Quaternion.Angle(housing.rotation, rotTarget));
    }

    public void FocusOnCurrentCharacter()
    {
        if(GameControl.instance.currentCharacter == null) return;

        housing.transform.position = GameControl.instance.currentCharacter.transform.position;
    }
}
