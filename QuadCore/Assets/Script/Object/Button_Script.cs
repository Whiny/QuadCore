using UnityEngine;
using System.Collections;

public class Button_Script : Switch 
{
    void Start()
    {
        IsOnOff = true;
    }

    protected override void Toggle(Collider2D other)
    {
       
    }

    protected override void Active(Collider2D other)
    {
        Debug.Log("You can Toggle");
    }
}
