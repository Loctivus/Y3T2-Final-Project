using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region Variables
    public static CameraMovement instCM;
    public Vector3Ref targetPos;
    public Vector3 camOffset;
    public Transform camFollowTF;
    public Transform mainCamTF;
    [Tooltip("Target transform for camera to look at. Prevents camera from looking straight down when player is against a wall")]
    public Transform camLookTarget;
    public FloatVariable mouseSens;
    [Tooltip("Sets distance between anchor point above character and the camera.")]
    public float camDist;
    float xRot;
    float yRot;
    #endregion

    void Start()
    {
        instCM = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSens.value * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSens.value * Time.deltaTime;
        xRot -= mouseY;
        yRot += mouseX;
        xRot = Mathf.Clamp(xRot, -35f, 65f);
        

    }

    /// <summary>
    /// Uses raycasts to determine proximity to environment
    /// if raycast hits object, set camera position to point 3/4 along the ray length to help prevent camera clipping into walls
    /// else, set the camera's rotation and direction based on the anchor point and camera distance value
    /// </summary>
    void FixedUpdate()
    {
        RaycastHit standardCam;
        Quaternion rot = Quaternion.Euler(xRot, yRot, 0f);
        camFollowTF.rotation = rot;
        camFollowTF.position = targetPos.value + camOffset;

        if (Physics.Raycast(camFollowTF.position, camFollowTF.TransformDirection(Vector3.back), out standardCam, camDist))
        {
            Vector3 rayDist = standardCam.point - camFollowTF.position;
            Vector3.Normalize(rayDist);
            Vector3 newCamPos = camFollowTF.position + (0.75f * rayDist);
            mainCamTF.position = newCamPos;
        }
        else
        {
            Vector3 dir = new Vector3(0, 0, -camDist);
            mainCamTF.position = camFollowTF.position + rot * dir;
        }
        mainCamTF.LookAt(camLookTarget.position);
    }
}
