using UnityEngine;
using System.Collections;

public class Cloud_Script : MonoBehaviour
{
    private float speed; public float Speed { get { return speed; } set { speed = value; } } // 속도
    private Vector2 direction; public Vector2 Direction { get { return direction; } set { direction = value; } } // 방향
    private float lifeTime; public float LifeTime { get { return lifeTime; } set { lifeTime = value; } } // 생명 시간

    private Rigidbody2D rigid;
    private float t;
    private static Vector2 zero = new Vector2(0, 0);

    void Start()
    {
        lifeTime = 10;
        t = 0;
        direction = new Vector2(1, 0);
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x) > 200)
            Destroy();
    }

    void FixedUpdate()
    {
       

        if (t > lifeTime)
            Destroy(); 
        else
        {
            t += Time.deltaTime;
            rigid.velocity = direction * speed;
        }
            

    }

    void Destroy()
    {
        this.gameObject.SetActive(false);
        CloudGenerator.cloudList.Enqueue(this.gameObject);
        transform.position = zero;
        t = 0;
    }
}
