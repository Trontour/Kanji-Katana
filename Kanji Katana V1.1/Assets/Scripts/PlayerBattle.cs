using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyLocator;
    [SerializeField] private Image target;
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
        inBattleMode = false;
        target.enabled = true;
    }


    // Update is called once per frame
    void Update()
    {
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

        if (hitColliders.Length > 1) {
            foreach (var hitCollider in hitColliders)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = hitCollider;
                }
            }
        }
        else if (hitColliders.Length == 1) {
            nearestEnemy = hitColliders[0];
        }
        

        if (nearestEnemy != null)
        {
            enemyLocator.transform.position = nearestEnemy.transform.position;
            target.enabled = true; 
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
                return true;
            }
               
        }
        else
        {
            inBattleMode= false;
        }
        
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, battleRange);

    }
}
