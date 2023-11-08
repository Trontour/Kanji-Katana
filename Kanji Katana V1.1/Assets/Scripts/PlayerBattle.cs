using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    //[Header("References")]
    //public GameObject freeLookCamera;
    //public GameObject battleCamera;

    //public CameraStyle currentStyle;
    [Header("Battle")]
    [SerializeField] private float battleRange = 50f;
    private bool inBattleRange = false;
    private bool inBattleMode = false;
    [SerializeField] private LayerMask whatIsEnemy;

    [Header("Keybinds")]
    public KeyCode battleKey = KeyCode.R;

    public void Awake()
    {
        //SwitchCameraStyle(CameraStyle.Freelook);
    }


    // Update is called once per frame
    void Update()
    {
        //UpdateBattleMode();
        
        

    }
    public bool checkBattleMode()
    {
        //Debug.Log(transform.position);
        //if (!inBattleMode)
        //{
        //    if (Physics.CheckSphere(transform.position, battleRange, whatIsEnemy))
        //    {
        //        inBattleRange = true;
        //        inBattleMode = true;
        //        //SwitchCameraStyle(CameraStyle.Battle);

        //    }
        //}
        //else
        //{
        //    if (!Physics.CheckSphere(transform.position, battleRange, whatIsEnemy))
        //    {
        //        inBattleRange = false;
        //        inBattleMode = false;
        //        //SwitchCameraStyle(CameraStyle.Freelook);

        //    }
        //}
        if (Physics.CheckSphere(transform.position, battleRange, whatIsEnemy) && Input.GetKeyDown(battleKey))
        {
            return true;
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, battleRange);

    }
}
