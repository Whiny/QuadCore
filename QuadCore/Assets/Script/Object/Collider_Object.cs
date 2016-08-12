using UnityEngine;
using System.Collections;

public abstract class Collider_Object : MonoBehaviour 
{
    private bool isPlayerOn; public bool IsPlayerOn { get { return isPlayerOn; } set { isPlayerOn = value; } } // 플레이어 충돌?

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Active(other);
        }
    }

    protected abstract void Active(Collider2D other); // 효과 작동
}
