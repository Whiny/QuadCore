using UnityEngine;
using System.Collections;

public class Spike_Script : Object
{
    protected override void Active(Collider2D other)
    {
        Debug.Log("Player DEAD");
    }
}
