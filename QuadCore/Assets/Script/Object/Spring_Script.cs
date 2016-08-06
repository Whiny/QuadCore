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

			//플레이어 구조의 변화에 따라 콜라이더의 부모의 리지드바디에 접근에 주시길 바랍니다.
			//Rigidbody2D otherRigid = other.transform.parent.gameObject.GetComponent<Rigidbody2D>();
			//그냥 고치려다가 급한 것도 아니라서 저보다는 담당자가 하는게 맞는거 같아(막 고치면 기분 나쁠수 있으니깐ㅋㅋ 실수할수도 있고)
			//주석만 달았습니다. 그럼 부탁 드립니다.	
			//											by jandy14
        }      
    }
}
