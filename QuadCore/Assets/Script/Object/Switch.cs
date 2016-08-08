using UnityEngine;
using System.Collections;

public abstract class Switch : Object 
{
    public Sprite initStatus; // 초기 상태
    public Sprite onStatus; // On 상태
    public Sprite offStatus; // Off 상태

    private bool isOnOff; // On = true, Off = falses
    public bool IsOnOff { get { return isOnOff; } set { isOnOff = value; } }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Toggle(other);
        }
    }

    protected abstract void Toggle(Collider2D other); // 작동시 실행 시킬 함수
}
