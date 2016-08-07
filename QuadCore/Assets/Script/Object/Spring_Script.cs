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

			// 접근이 편하게 플레이어 스크립트에 리지드 바디를 저장하는 맴버 변수를 사용하여 쓰는 건 어떤가요? by soughoo
            // 지금은 이것밖에 물리를 안쓰니깐 귀찮으시면 굳이 안하셔도 됩니다.
        }      
    }
}
