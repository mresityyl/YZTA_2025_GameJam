using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float movementSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public Animator animator;
    public bool move = true;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0)
        {
            Vector3 newScale = originalScale;
            // sola giderken x negatif, sağa giderken pozitif
            newScale.x = Mathf.Sign(movement.x) * Mathf.Abs(originalScale.x);
            transform.localScale = newScale;
        }

        float animX, animY;
        DetermineAnimationDirection(out animX, out animY);

        animator.SetFloat("Horizontal", animX);
        animator.SetFloat("Vertical",   animY);
        animator.SetFloat("Speed",      movement.sqrMagnitude);
    }
    void FixedUpdate()
    {
        if(move) rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }
     private void DetermineAnimationDirection(out float horizontal, out float vertical)
    {
        if (movement.sqrMagnitude < 0.01f)
        {
            horizontal = animator.GetFloat("Horizontal");
            vertical   = animator.GetFloat("Vertical");
            if (horizontal == 0f && vertical == 0f)
                vertical = -1f;
        }
        else
        {
            horizontal = movement.x;
            vertical   = movement.y;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("movableObject"))
        {
            animator.SetBool("Push",true);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetBool("Push", false);
    }
}
