using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
//using UnityEngine.InputSystem;

public class CinemachineSwitcher : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private KeyCode battleKey;
    [SerializeField]
    private CinemachineVirtualCamera targetLockCam;
    [SerializeField]
    private CinemachineFreeLook freelookCam;
    [SerializeField]
    private PlayerBattle playerScript;

    public bool isFreelook;
    private Animator animator;
    private Transform currentCamTransform;



    private void Awake()
    {
        currentCamTransform = freelookCam.transform; 
        if (targetLockCam.Priority == 1)
        {
            isFreelook = false;
        }
        else
        {
            isFreelook = true;
        }
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame

    void Update()
    {
        transform.position = currentCamTransform.position;
        if (playerScript.checkBattleMode())
        {
            //Debug.Log("WOrks");
            switchCam();
        }
        else if (!isFreelook && playerScript.inBattleMode == false)
        {
            switchCam();
        }

    }
    void switchCam()
    {
        if (isFreelook)
        {
            currentCamTransform = targetLockCam.transform;
            animator.Play("TargetCamera");
            isFreelook = false;
        }
        else
        {
            currentCamTransform = freelookCam.transform ;
            animator.Play("FreelookCamera");
            isFreelook = true;
        }
    }
}
