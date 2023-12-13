using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyLocator;
    //[SerializeField] private GameObject targetParent;
    [SerializeField] private Image target;
    private CinemachineSwitcher camScript;
    //public GameObject freeLookCamera;
    //public GameObject battleCamera;

    //public CameraStyle currentStyle;
    [Header("Battle")]
    [SerializeField] private float battleRange = 50f;
    private bool inBattleRange = false;
    private bool inBattleMode = false;
    private bool inTargetLockMode = false;
    [SerializeField] private LayerMask whatIsEnemy;

    [Header("Keybinds")]
    public KeyCode battleKey = KeyCode.R;

    public void Awake()
    {
        //SwitchCameraStyle(CameraStyle.Freelook);
        inBattleMode = false;
        target.enabled = false;
        camScript = GameObject.Find("State-Driven Camera").GetComponent<CinemachineSwitcher>();

    }


    // Update is called once per frame
    void Update()
    {
        //checkBattleMode();
        //UpdateBattleMode();
        if (inBattleMode)
        {
            trackNearestEnemy();
        }
        
        
        

    }

    void trackNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, battleRange, whatIsEnemy);
        Collider nearestEnemy = null;
        float minDistance = float.MaxValue;

        float distance = 0;
        if (hitColliders.Length > 1) {
            foreach (var hitCollider in hitColliders)
            {
                distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = hitCollider;
                }
            }
        }
        else if (hitColliders.Length == 1) {
            nearestEnemy = hitColliders[0];
            distance = Vector3.Distance(transform.position, nearestEnemy.transform.position);
        }
        else
        {
            return;
        }

        //float distance2 = Vector3.Distance(transform.position, nearestEnemy.transform.position);
        if (nearestEnemy != null &&  distance > 40f)
        {
            enemyLocator.transform.position = nearestEnemy.transform.position;
            if (!camScript.isFreelook)
            {
                //Debug.Log("HHH");
                target.enabled = true;
                target.transform.localScale = new Vector3(distance, distance, distance) / 40;
                target.transform.Rotate(Vector3.forward, 25f * Time.deltaTime);
            }
            else
                target.enabled = false;
        }
        else
        {
            target.enabled = false;
        }
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
        if (Physics.CheckSphere(transform.position, battleRange, whatIsEnemy))
        {
            inBattleMode = true;
            if (Input.GetKeyDown(battleKey))
            {
                //inTargetLockMode = true;
                return true;
            }
               
        }
        else
        {
            inBattleMode= false;
        }
        //inTargetLockMode= false;
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, battleRange);

    }
}
