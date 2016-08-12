using UnityEngine;
using System.Collections;

public class Kill_Script : Object {

    protected override void Active( Collider2D other )
    {
        other.transform.gameObject.GetComponentInParent<PlayerControl>().Die();
    }
}
