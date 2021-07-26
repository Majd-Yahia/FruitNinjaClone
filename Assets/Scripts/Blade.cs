using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public float angle;
    public int Points { get; set; }
    private TrailRenderer trail;
    private new Collider2D collider;
    private Vector3 startPosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        TrackMouse();
        SetBladeToMouse();
        CalculateAngle();
    }

    private void CalculateAngle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseDelta = Input.mousePosition - startPosition;

            if (mouseDelta.sqrMagnitude < 0.1f)
            {
                return; // don't do tiny rotations.
            }

            angle = Mathf.Atan2(mouseDelta.y, mouseDelta.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;

        }
        else
        {
            angle = 0;
        }
    }

    private void SetBladeToMouse()
    {
        if (Input.GetMouseButton(0))
        {
            collider.enabled = true;
            trail.enabled = true;

            var mousePosition = Input.mousePosition;
            mousePosition.z = 10;                                           // distance of 10 uint from camera.

            rb.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        else
        {
            collider.enabled = false;
            trail.enabled = false;
        }
    }

    private void TrackMouse()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = 10;                                           // distance of 10 uint from camera.

        rb.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }



}
