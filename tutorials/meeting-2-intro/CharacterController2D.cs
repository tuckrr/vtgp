using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour {

    // public variables appear in the editor, and we can assign/change their values there.
    public float maxSpeed = 2.0f;
    public float jumpVelocity;
    public Transform groundCheck;

    // more complicated jump added at the end of meeting
    public float fallMultiplier;
    public float lowJumpMultiplier;

    // private variables do not appear in the editor
    private Rigidbody2D rb;
    private bool facingRight = true;

    // Awake() is called when a gameobject is initialized, whether or not the script component is enabled
    void Awake()
    {
        // so we don't have to GetComponent() every time we need to use physics
        rb = GetComponent<Rigidbody2D>();
    }
    // also usable is Start() which is called when the script component is enabled
    // both Start() and Awake() are called exactly once in a component's lifetime

    // FixedUpdate() is called once per physics step (usually 60 times/second)
    // Update() is bound to framerate, so we want to use FixedUpdate() for anything to do with physics
    void FixedUpdate()
    {
        // copy the rb's velocity so we can modify individual axes
        Vector2 velocity = rb.velocity;

        // simple jumping
        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            velocity = Vector2.up * jumpVelocity;
        }


        // better jumping
        if (velocity.y < 0) {
            velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        } else if (velocity.y > 0 && !Input.GetButton("Jump")) {
            velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        // end better jumping

        // horizontal movement
        float hMove = Input.GetAxis("Horizontal");
        velocity.x = hMove * maxSpeed;

        // flip the sprite
        if (hMove > 0.1 && !facingRight || hMove < -0.1 && facingRight) {
            Flip();
        }

        rb.velocity = velocity;

        // one other way to do things involves adding forces to the rigidbody,
        // here I've used two different methods to modify the velocity
        // (modifying an axis value vs modifying the vector)
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) {
            if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("StompableEnemy"))) {
                collision.gameObject.SetActive(false); // on top of a goomba, so it dies
            } else {
                // shrink not implemented
            }
        }
    }

    // check if the player is on the ground using a linecast to an empty immediately under them
    private bool IsGrounded()
    {
        return Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
    }

    // turns the player around, also could use the component's flip method
    // this way will scale any attached components/spatial values, though, so keep that in mind
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
