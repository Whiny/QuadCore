using UnityEngine;
using System.Collections;

public class Spike_Script : Collider_Object
{
    protected override void Active(Collider2D other)
	{
        Debug.Log("죽음");
		if (other.GetComponentInParent<PlayerControl>().Angle == 0) other.GetComponentInParent<PlayerControl>().Damaged(2.5f, -1, "Spike");
		else other.GetComponentInParent<PlayerControl>().Damaged(2.5f, 1, "Spike");
	}
}