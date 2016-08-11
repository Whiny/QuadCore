using UnityEngine;
using System.Collections;

public abstract class Destructible : MonoBehaviour
{
    public bool destructible; // 파괴 가능한가?

    protected virtual void OnTriggerEnter2D(Collider2D other) { }

    protected abstract void Destroy();
}
