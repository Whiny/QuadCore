using UnityEngine;
using System.Collections;

public abstract class Collision_Object : MonoBehaviour
{
    private bool isPlayerOn; public bool IsPlayerOn{ get{return isPlayerOn;} set{ isPlayerOn = value;}} // 플레이어 충돌?

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            Active(other);
        }
    }

    protected abstract void Active(Collision2D other); // 효과 작동
}
