using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{

    [Header("REFERENCES")]
    [SerializeField] private PlayerData dataController;
    [SerializeField] private GameObject amongusEnemy;
    [SerializeField] private Transform spawnPoint;
    

    private List<HiraganaObject> hiraganas;
    private List<GameObject> enemies;
    // Start is called before the first frame update
    void Start()
    {
        dataController.clearAllHiragana();
        dataController.saveNewHiragana("が", "ga", 1);
        dataController.saveNewHiragana("か", "ka", 1);
        dataController.saveNewHiragana("え", "e", 0);
        dataController.saveNewHiragana("く", "ku", 0);
        dataController.saveNewHiragana("へ", "he", 0);
        dataController.saveNewHiragana("い", "i", 2);
        //dataController.saveNewHiragana("ら", "ra");
        //dataController.saveNewHiragana("ぽ", "po");
        //dataController.clearAllHiragana();
        hiraganas = dataController.getHiraganas();
        var todaysHiraganas = hiraganas.Where(h => h.daysTillDue == 0).ToList();
        foreach (HiraganaObject obj in todaysHiraganas)
        {
            Debug.Log($"Hiragana: {obj.hiragana}, Romaji: {obj.romaji}, Days Till Due: {obj.daysTillDue}");
        }
        //test
        GameObject enemy1 = Instantiate(amongusEnemy, spawnPoint.position, Quaternion.Euler(0, 0, 0));
        EnemyAi enemyAi = enemy1.GetComponent<EnemyAi>();
        enemyAi.enemyHiraganas = todaysHiraganas;


        //var splitLists = SplitList(dueHiraganas, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
