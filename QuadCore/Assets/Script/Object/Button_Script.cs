using UnityEngine;
using System.Collections;

public class Button_Script : Switch 
{
    void Start()
    {
        IsOnOff = true;
    }

    protected override void Toggle(Collision2D other)
    {
       
    }

    protected override void Active(Collision2D other)
    {
        Debug.Log("You can Toggle");
    }
}
