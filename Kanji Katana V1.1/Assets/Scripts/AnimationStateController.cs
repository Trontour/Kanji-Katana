using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private int isWalkingHash;
    private int isSprintingHash;

    [Header("References")]
    [SerializeField] private PlayerMovement playerScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isSprintingHash = Animator.StringToHash("isSprinting");
    }

    // Update is called once per frame
    void Update()
    {
        ///bool forwardPressed = Input.GetKey("w");
        //if (forwardPressed)
        //{
        //    animator.SetBool(isWalkingHash, true);
        //}
        //else
        //{
        //    animator.SetBool(isWalkingHash, false);
        //}
        //if (playerScript.state == PlayerMovement.MovementState.walking)
        //{
        //    animator.SetBool(isWalkingHash, true);
        //}
        //else
        //{
        //    animator.SetBool(isWalkingHash, false);
        //}

        if (playerScript.isSprinting)
        {
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isSprintingHash, true);
        }
        else if (playerScript.isWalking)
        {
            animator.SetBool(isWalkingHash, true);
            animator.SetBool(isSprintingHash, false);
        }
        else { 
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isSprintingHash, false);
        }
    }
}
