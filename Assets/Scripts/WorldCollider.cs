using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Fruit")
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Bomb")
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fruit")
        {
            Destroy(other.gameObject);
        }
    }


}
