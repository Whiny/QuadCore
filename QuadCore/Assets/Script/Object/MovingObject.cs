using UnityEngine;
using System.Collections;

public class MovingObject : MonoBehaviour
{

    public float angular; // 각도
    public float speed; // 속도
    public float distance; // 움직일 거리

    private Vector2 direction; // 방향
    private Rigidbody2D rigid;
    private Vector2 zero; // 원점
    private bool swap;

    void Start()
    {
        swap = true;
        zero = transform.position;
        rigid = GetComponent<Rigidbody2D>();
        angular = Mathf.Deg2Rad * angular;
        direction = new Vector2(Mathf.Cos(angular), Mathf.Sin(angular));
    }

    void FixedUpdate()
    {
        Move();  
    }

    protected virtual void Move()
    {
        if (Vector2.Distance(zero, transform.position) < distance)
            rigid.velocity = direction * speed;
        else
        {
            speed *= -1;
            rigid.velocity = direction * speed;
        }
    }
}
