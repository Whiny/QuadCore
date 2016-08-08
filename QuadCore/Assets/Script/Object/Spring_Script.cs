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
			// re>>	귀하의 소중한 의견은 잘 듣고 고려해 봤습니다.
			//		꽤 흥미로운 의견이긴 하지만 활용할 곳이 적다는 귀하의 생각이 저의 생각과 일치해
			//		그냥 지금의 체제를 유지하는게 낫다고 판단했습니다.
			//		혹시 다시 추후에 필요성이 높아지면 그때 다시 이 의견을 검토해 보겠습니다.
			//		다시한번 귀하의 소중한 의견에 감사드립니다.
			//													by jandy14
			//
			//		p.s. 스프링 점프력이 조금 부족한거 같습니다. 1.4배 정도 상향되면 좋을거 같군요
        }      
    }
}
