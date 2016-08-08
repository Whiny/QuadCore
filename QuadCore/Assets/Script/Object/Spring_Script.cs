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
            Rigidbody2D otherRigid = other.transform.parent.gameObject.GetComponent<Rigidbody2D>();
			otherRigid.velocity = new Vector2(0, 0);
            otherRigid.AddForce(Vector2.up * force, ForceMode2D.Force);
            // public float force 로 누군든지 수치를 조절할 수 있게 이미 해둔사항입니다 자세한 사용법은 이건탁, 윤지수 회원분들께 물어보시길 바랍니다. by sounghoo
        }      
    }
}
