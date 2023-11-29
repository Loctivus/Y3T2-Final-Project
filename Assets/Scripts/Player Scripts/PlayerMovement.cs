using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Components and SOs")]
    public BoolVariable casting;
    CharacterController charCtrl;
    public Vector3Variable currentPos;
    
    Animator anim;

    [Header("Movement Variables")]
    public PlayerStatsRef playerStatRef;
    public FloatVariable currentMoveSpeed;
    [Tooltip("Change the value to affect character rotation speed. The lower the value the faster the rotation.")]
    public float rotSmoothSpeed;
    float rotSmoothVel;
    public float speedSmoothTime;
    float speedSmoothVel;
    Vector3 gravVel;
    Vector3 moveVel;
    Vector2 moveDir;

    [Header("Gravity Detection")]
    [SerializeField]bool onGround;
    public float gravity = -10f;
    [SerializeField] Transform groundCheck;
    float groundDist = 0.15f;
    public LayerMask groundMask;
    #endregion

    void Start()
    {
        currentMoveSpeed.value = playerStatRef.playerStats.startMoveSpeed;
        charCtrl = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        currentPos.value = transform.position;
        onGround = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        if (onGround && gravVel.y < 0)
        {
            gravVel.y = 0f;
        }
        else
        {
            gravVel.y += gravity * Time.deltaTime;
        }
        charCtrl.Move(gravVel * Time.deltaTime);
       

        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        //calculate movement direction used for smoothing calculation
        moveDir = new Vector2(hInput, vInput).normalized;

        if (!casting.value)
        {
            if (moveDir != Vector2.zero)
            {
                //Calculate a smoothness dampener on player character rotation so that it does not snap as much when changing directions
                //Also calculate smoothness dampener to movement speed so that movement isn't snapping and also has a smooth climb from not moving to full move speed
                float targetLookRot = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg + CameraMovement.instCM.mainCamTF.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetLookRot, ref rotSmoothVel, rotSmoothSpeed);
                float targetSpeed = currentMoveSpeed.value * moveDir.magnitude;
                currentMoveSpeed.value = Mathf.SmoothDamp(currentMoveSpeed.value, targetSpeed, ref speedSmoothVel, speedSmoothTime);
                moveVel = transform.forward * currentMoveSpeed.value;
                charCtrl.Move(moveVel * Time.deltaTime);
                anim.SetFloat("MoveSpeed_f", 0.35f);


            }
            else
            {
                anim.SetFloat("MoveSpeed_f", 0f);
            }
        }
        else
        { 
            if (moveDir != Vector2.zero)
            {

                float targetSpeed = currentMoveSpeed.value * moveDir.magnitude;
                currentMoveSpeed.value = Mathf.SmoothDamp(currentMoveSpeed.value, targetSpeed, ref speedSmoothVel, speedSmoothTime);
                moveVel = transform.forward * currentMoveSpeed.value;
                charCtrl.Move(moveVel * Time.deltaTime);
                transform.eulerAngles = Vector3.up * CameraMovement.instCM.mainCamTF.eulerAngles.y;
                anim.SetFloat("MoveSpeed_f", 0.35f);
            }
            else
            {
                transform.eulerAngles = Vector3.up * CameraMovement.instCM.mainCamTF.eulerAngles.y;
                anim.SetFloat("MoveSpeed_f", 0f);

            }
        }
    }


   
    /// <summary>
    /// Normally called through animation event to change movement speed when casting a spell
    /// </summary>
    /// <param name="f"></param>
    public void ChangeSpeed(float f)
    {


        if (casting.value)
        {
            currentMoveSpeed.value = f;
            //casting.value = false;
        }
        else if (!casting.value)
        {
            currentMoveSpeed.value = playerStatRef.playerStats.startMoveSpeed;
        }
        
    }

    public void ReduceSpeed(float newSpeed)
    {
        currentMoveSpeed.value = newSpeed;
    }

    /// <summary>
    /// Reset movement speed to base starting speed
    /// Normally called near end of spell cast animations
    /// </summary>
    public void ResetSpeed()
    {
        casting.value = false;
        currentMoveSpeed.value = playerStatRef.playerStats.startMoveSpeed;
    }
}
