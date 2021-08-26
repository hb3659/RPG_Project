using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Backup : MonoBehaviour
{
    #region Variables
    private CharacterController controller;
    private UnityEngine.AI.NavMeshAgent agent;
    private Camera cam;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private LayerMask ground;

    readonly int moveHash = Animator.StringToHash("Move");
    #endregion Variables

    void Start()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;

        cam = Camera.main;
    }

    void Update()
    {
        // Process mose left buton input
        if (Input.GetMouseButtonDown(0))
        {
            // Make ray from screen to world
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // Check hit from ray
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, ground))
            {
                // Move our character to what we hit
                agent.SetDestination(hit.point);
            }
        }
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            controller.Move(agent.velocity * Time.deltaTime);
            animator.SetBool(moveHash, true);
        }
        else
        {
            controller.Move(Vector3.zero);
            animator.SetBool(moveHash, false);
        }
    }

    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }
}
