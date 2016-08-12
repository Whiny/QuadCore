using UnityEngine;
using System.Collections;

public class Kill_Script : Collider_Object 
{
    protected override void Active(Collider2D other)
    {
        other.transform.gameObject.GetComponentInParent<PlayerControl>().Die();
    }
}
