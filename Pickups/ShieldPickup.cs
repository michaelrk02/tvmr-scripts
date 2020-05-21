using UnityEngine;
using System.Collections;

public class ShieldPickup : Pickup
{
    protected override void TakeAction()
    {
        Player player = GameObject.Find("Tank").GetComponent<Player>();
        player.shield += 25.0f;

        base.TakeAction();
    }
}
