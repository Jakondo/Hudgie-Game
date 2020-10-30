using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxMoveSpeed = 10;
    private Vector3 moveSpeed;
    private float dashingTimeLeft;

    private CharacterController controllerComponent;
    private Animator animatorComponent;
    private Quaternion targetPositon;

    private static readonly int WalkSpeedParam = Animator.StringToHash("Speed");

    // Start is called before the first frame update
    void Start()
    {
        animatorComponent = GetComponent<Animator>();
        controllerComponent = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
       
        UpdateWalk();
                
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.X)) Dash(false);
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.X)) Dash(true);
           
        
        animatorComponent.SetFloat(WalkSpeedParam, new Vector3(moveSpeed.x, 0, moveSpeed.z).sqrMagnitude);
    }

    private void Dash(bool holding)
    {
        if (dashingTimeLeft < (holding ? -.4f : -.2f))
        {
            dashingTimeLeft = .3f;            
        }
    }

    private void UpdateWalk()
    {
        float ySpeed = moveSpeed.y;
        moveSpeed.y = 0;
        if (dashingTimeLeft <= 0)
        {
            Vector3 target = maxMoveSpeed * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            moveSpeed = Vector3.MoveTowards(moveSpeed, target, Time.deltaTime * 300);

            if (moveSpeed.sqrMagnitude > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveSpeed), Time.deltaTime * 720);
            }
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveSpeed), Time.deltaTime * 720);
        }
        else
        {
            moveSpeed = maxMoveSpeed * 5 * moveSpeed.normalized;
        }

        dashingTimeLeft -= Time.deltaTime;

        moveSpeed.y = ySpeed + Physics.gravity.y * Time.deltaTime;
        controllerComponent.Move(moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);           
        }
    }

}

