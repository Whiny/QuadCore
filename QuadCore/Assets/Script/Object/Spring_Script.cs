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
			// re>>	이런 그 부분에 대해선 할 말이 없군요.
			//		코드를 조금만 봐도 알 수 있었던 부분인데 말이죠.
			//		사과드립니다. 쨋든 앞으로는 이런 일은 좀더 빠른 메신져를 쓰도록하죠ㅋ
			//		제가 시작해놓고 이런 말을 꺼내 조금 이상하게 보일지 모르겠군요. 이해해 주시길 바랍니다.
			//
			//		쨋든 그동안 꽤 재미있었습니다. 앞으로는 메신저로 만나죠.
        }      
    }
}
