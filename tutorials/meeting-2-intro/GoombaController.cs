using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GoombaController : MonoBehaviour {

    public float maxSpeed = 0.5f;
    public float timerSeconds = 5.0f; // seconds until it turns

    private float timer = 0f;
    private bool facingRight = true;
    private Rigidbody2D rb;

	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
        Vector2 velocity = rb.velocity;

        // moving on a timer
        timer += Time.fixedDeltaTime;
        if (timer >= timerSeconds) {
            facingRight = !facingRight;
            timer = 0;
        }

        // move in the direction it faces
        if (facingRight) {
            velocity.x = maxSpeed;
        } else {
            velocity.x = maxSpeed * -1;
        }

        rb.velocity = velocity;
	}
}
