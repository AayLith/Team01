using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySystem : MonoBehaviour
{
    [SerializeField] private GameObject[] monsters;
    [SerializeField] private int monsterID;
    [SerializeField] private float monsterQuantity;
    

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            GenerateMonsters();
        }
    }


    private void GenerateMonsters()
    {
        for (var i = 0; i < 1 * monsterQuantity; i++)
        {
            Instantiate(monsters[Random.Range(0,3)]);
        }
    }

}
