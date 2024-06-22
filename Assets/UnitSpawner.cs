using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [Header("Player Unit Variables")]
    [SerializeField] private GameObject[] allyUnits;
    [SerializeField] private float allyUnitQuantity;

    [Header("Opponent Unit Variables")]
    [SerializeField] private GameObject[] enemyUnits;
    [SerializeField] private float enemyUnitQuantity;

    public List<Creature> playerUnits = new List<Creature>();
    public List<Creature> opponentUnits = new List<Creature> ();


    private void Start()
    {
        GeneratePlayerUnits();
        GenerateOpponentUnits();
        BattleController.instance.startBattlePreparation(playerUnits, opponentUnits);
    }

    private void Update()
    {
        //for testing
        if(Input.GetKeyDown(KeyCode.I))
        {
            GeneratePlayerUnits();
            GenerateOpponentUnits();
            BattleController.instance.startBattlePreparation(playerUnits, opponentUnits);
        }
    }

    private void GeneratePlayerUnits()
    {
        for (var i = 0; i < 1 * allyUnitQuantity; i++)
        {
            GameObject spawnedAllies = (GameObject)Instantiate(allyUnits[Random.Range(0, 3)]);
            playerUnits.Add(spawnedAllies.GetComponent<Creature>());
        }
    }

    private void GenerateOpponentUnits()
    {
        for (var i = 0; i < 1 * enemyUnitQuantity; i++)
        {
            GameObject spawnedEnemies = (GameObject) Instantiate(enemyUnits[Random.Range(0,3)]);
            opponentUnits.Add(spawnedEnemies.GetComponent<Creature>());
        }
    }

}
