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
            Rigidbody2D otherRigid = other.transform.gameObject.GetComponent<Rigidbody2D>();
			otherRigid.velocity = new Vector2(0, 0);

            Vector2 direction = new Vector2(Mathf.Cos(transform.rotation.z + 90 * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.z + 90 * Mathf.Deg2Rad));
            // 주의점 transform.rotation.z 은 라디안 값을 반환한다.

            otherRigid.AddForce(direction * force, ForceMode2D.Force);
            // 넹
        }      
    }
}
