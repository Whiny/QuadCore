using UnityEngine;
using System.Collections;

public abstract class Object : MonoBehaviour
{
    private bool isPlayerOn; public bool IsPlayerOn{ get{return isPlayerOn;} set{ isPlayerOn = value;}}

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Active(other);
        }
    }

    protected abstract void Active(Collider2D other);
}
