using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController_Key_Backup : MonoBehaviour
{
    #region Variables

    // Variables
    private float walkSpeed = 5f;
    private float runSpeed = 10f;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private Vector3 moveDirection = Vector3.zero;

    // References
    //PlayerInput playerInput;
    CharacterController controller;
    Animator animator;
    //PlayerStat stat;

    [SerializeField]
    private Transform cam;

    #endregion Variables

    // Start is called before the first frame update
    void Start()
    {
        //playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //stat = FindObjectOfType<PlayerStat>();

        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void Move()
    {
        //moveDirection = new Vector3(playerInput.moveX, 0f, playerInput.moveZ).normalized;

        bool isWalk = moveDirection.magnitude != 0;
        animator.SetBool("isWalk", isWalk);

        if (moveDirection.magnitude >= 0.1f && isWalk)
        {
            // 카메라가 보는 방향으로 회전
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            // 회전 시 부드럽게 회전
            float angle =
                Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 Dir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("isRun", true);
                controller.Move(Dir.normalized * runSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("isRun", false);
                controller.Move(Dir.normalized * walkSpeed * Time.deltaTime);
            }
        }
    }
}
