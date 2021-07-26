using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private GameObject slicedFruitPrefab;
    [SerializeField] private Spawner spawner;
    [SerializeField] private int pointsToGive;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawner = FindObjectOfType<Spawner>();
        
        pointsToGive = Random.Range(1, 5);
    }
    public void CreateSlicedFruit(float angle)
    {

        // Play Particle Effect.
        Instantiate(particleSystem, transform.position, Quaternion.identity);

        GameObject fruitInstance = (GameObject)Instantiate(slicedFruitPrefab, transform.position, transform.rotation);
        fruitInstance.transform.rotation = Quaternion.Euler(0,0, angle+90);
        fruitInstance.transform.parent = spawner.Fruit;

        Rigidbody[] rbsOnSliced = fruitInstance.transform.GetComponentsInChildren<Rigidbody>();

        Vector3 velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        foreach (Rigidbody rigid in rbsOnSliced)
        {
            rigid.velocity = velocity;
            rigid.AddExplosionForce(Random.Range(50, 150), transform.position, 5f);
        }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Blade playerScript = other.gameObject.GetComponent<Blade>();
        if (other.gameObject.tag == "Blade")
        {
            playerScript.Points += pointsToGive;
            CreateSlicedFruit(playerScript.angle);
        }
    }
}
