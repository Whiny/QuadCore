using UnityEngine;
using System.Collections;
using System;

public class Portal_Script : Object
{
    public Transform red; // 빨간색 문
    public Transform green; // 초록색 문

    private BoxCollider2D redColider; // 빨간색 문 콜라이더
    private BoxCollider2D greenColider; // 초록색 문 콜라이더
    private bool isExit;

	void Start()
    {
        ColliderSet();
    }

    void ColliderSet()
    {
        isExit = false;
        redColider = gameObject.AddComponent<BoxCollider2D>();
        redColider.size = new Vector2(0.277f, 0.35f);
        redColider.offset = red.position;
        redColider.isTrigger = true;

        greenColider = gameObject.AddComponent<BoxCollider2D>();
        greenColider.size = new Vector2(0.277f, 0.35f);
        greenColider.offset = green.position;
        greenColider.isTrigger = true;
    }

    protected override void Active(Collider2D other)
    {
        if (!isExit)
        {
            if (redColider.IsTouching(other))
                other.transform.position = green.position;
            if (greenColider.IsTouching(other))
                other.transform.position = red.position;

            other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, -other.GetComponent<Rigidbody2D>().velocity.y);
            isExit = true;
        }
        else if (isExit)
            isExit = false;
    }
}
