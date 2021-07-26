using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public Transform Fruit;
    [SerializeField] private List<GameObject> fruitPrefabs;
    [SerializeField] private Transform[] spawnPosition;
    [SerializeField] private float minWait = .3f;
    [SerializeField] private float maxWait = 1f;
    [SerializeField] private float minForce = 200f;
    [SerializeField] private float maxForce = 500f;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(SpawnFruits());
    }

    private IEnumerator SpawnFruits()
    {
        while (!gameManager.isGameDone)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
            SpawnFruit();
        }
    }

    private void SpawnObjectWithForce()
    {
        float xForce = Random.Range(-0.2f, 0.2f);
    }

    private void SpawnFruit()
    {
        GameObject prefab;

        if (Random.Range(1, 100) <= 35)
        {
            prefab = fruitPrefabs[0];                                                       // Get bomb.
        }
        else
        {
            prefab = fruitPrefabs[Random.Range(1, fruitPrefabs.Count)];                                      // Get fruit.
        }

        float force = Random.Range(minForce, maxForce);                                     // Calculate random force.

        // Get Random spawnPosition to spawn from.
        Transform spot = spawnPosition[Random.Range(0, spawnPosition.Length)];
        GameObject fruit = Instantiate(prefab, spot.position, spot.rotation);               // Spawn object.

        fruit.GetComponent<Rigidbody2D>().AddForce(spot.up * force, ForceMode2D.Impulse);
        fruit.transform.rotation = Random.rotation;

        fruit.transform.parent = Fruit;
    }

}
