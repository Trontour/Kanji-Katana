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

    private bool isFreelook;
    private Animator animator;



    private void Awake()
    {
        if (targetLockCam.Priority == 1)
        {
            isFreelook = true;
        }
        else
        {
            isFreelook = false;
        }
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (playerScript.checkBattleMode())
        {
            Debug.Log("WOrks");
            switchCam();
        }
    }
    void switchCam()
    {
        if (isFreelook)
        {
            animator.Play("TargetCamera");
            isFreelook = false;
        }
        else
        {
            animator.Play("FreelookCamera");
            isFreelook = true;
        }
    }
}
