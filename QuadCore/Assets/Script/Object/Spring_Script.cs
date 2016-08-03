using UnityEngine;
using System.Collections;

public class Spring_Script : Object
{
    public Animator animator;
    public float force;

    private float t;

    void Start()
    {
        IsPlayerOn = false;
        t = 0;
    }

    void FixedUpdate()
    {
        if(IsPlayerOn)
        {
            IsPlayerOn = false;
            animator.SetBool("isPlayerOn", IsPlayerOn);
        }
    }

    protected override void Active(Collider2D other)
    {
        if (!IsPlayerOn)
        {
            IsPlayerOn = true;
            animator.SetBool("isPlayerOn", IsPlayerOn);
            Rigidbody2D otherRigid = other.GetComponent<Rigidbody2D>();
            otherRigid.velocity = new Vector2(0, 0);
            otherRigid.AddForce(Vector2.up * force, ForceMode2D.Force);
        }      
    }
}
