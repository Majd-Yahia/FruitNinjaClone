using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private GameManager gameManager;
    private bool collided = false;
    private new ParticleSystem particleSystem;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collided) { return; }
        Blade playerScript = other.gameObject.GetComponent<Blade>();

        if (other.gameObject.tag == "Blade")
        {
            collided = true;

            particleSystem.Play();

            switch (gameManager.mode)
            {
                case Mode.Timer:
                    SubtractFromTimer();
                    break;
                case Mode.Points:
                    gameManager.tries--;
                    break;
            }
        }
    }

    private void SubtractFromTimer()
    {
        if (gameManager.m_TimerSeconds <= 0)
        {
            if (gameManager.m_TimerMinutes > 1)
            {
                gameManager.m_TimerMinutes--;
                gameManager.m_TimerSeconds = 60;
            }
        }
        gameManager.m_TimerSeconds -= 3f;
    }
}
