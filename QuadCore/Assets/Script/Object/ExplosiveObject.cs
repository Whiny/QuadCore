using UnityEngine;
using System.Collections;

public class ExplosiveObject : Object 
{
    public float limitTime;  // 제한 시간

    public GameObject explosion; // 폭발
    public Sprite initStatus; // 초기 상태
    public Sprite[] blinkStatus; // 폭발 직전 깜빡 거리는 상태 스프라이트 배열

    void FixedUpdate()
    {
        if (IsPlayerOn)
        {
            Explosion();
        }
    }

    protected override void Active(Collider2D other)
    {
        IsPlayerOn = true;
    }

    protected virtual void Explosion()
    {
        limitTime -= Time.deltaTime;

        if(limitTime < 0)
        {
            Instantiate(explosion,transform.position,transform.rotation);
            Destroy(this.gameObject);
        }
        else
        {
            Blink();
        }
    }

    protected virtual void Blink()
    {
        if(Mathf.Sin(limitTime * Mathf.PI * 2f) > 0)
            this.gameObject.GetComponent<SpriteRenderer>().sprite = blinkStatus[0];
        else
            this.gameObject.GetComponent<SpriteRenderer>().sprite = blinkStatus[1];
    }
}
