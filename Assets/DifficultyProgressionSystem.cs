using UnityEngine;

public class DifficultyProgressionSystem : MonoBehaviour
{
    [Header("Monster Types")]
    [SerializeField] private GameObject monster1;
    [SerializeField] private GameObject monster2;
    [SerializeField] private GameObject monster3;
    [SerializeField] private GameObject monster4;
    [SerializeField] private GameObject monster5;

    [Header("Monster Numbers")]
    [SerializeField] private float monster1Number;
    [SerializeField] private float monster2Number;
    [SerializeField] private float monster3Number;
    [SerializeField] private float monster4Number;
    [SerializeField] private float monster5Number;

    [Header("Opponent Battlefield")]
    [SerializeField] private Transform opponentField1;
    [SerializeField] private Transform opponentField2;
    [SerializeField] private Transform opponentField3;



    private void Update()
    {
        //Generate monsters
        if(Input.GetKeyDown(KeyCode.I))
        {
            GenerateCreatures();
        }
    }

    private void GenerateCreatures()
    {
        for (var i = 0; i < 1 * monster1Number; i++)
        {
            Instantiate(monster1, new Vector3(opponentField1.transform.position.x, i * -0.3f, 0), Quaternion.identity, opponentField1);
        }
        for (var i = 0; i < 1 * monster2Number; i++)
        {
            Instantiate(monster2, new Vector3(opponentField2.transform.position.x, i * -0.3f, 0), Quaternion.identity, opponentField2);
        }
        for (var i = 0; i < 1 * monster3Number; i++)
        {
            Instantiate(monster3, new Vector3(opponentField3.transform.position.x, i * -0.3f, 0), Quaternion.identity, opponentField3);
        }
        for (var i = 0; i < 1 * monster4Number; i++)
        {
            Instantiate(monster4, new Vector3(0, i * -0.3f, 0), Quaternion.identity);
        }
        for (var i = 0; i < 1 * monster5Number; i++)
        {
            Instantiate(monster5, new Vector3(0, i * -0.3f, 0), Quaternion.identity);
        }
    }
}
