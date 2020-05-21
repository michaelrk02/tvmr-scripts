using UnityEngine;
using System.Collections;

public class ShieldRestorePickup : Pickup
{
    protected override void TakeAction()
    {
        Player player = GameObject.Find("Tank").GetComponent<Player>();
        player.shield = player.maxShield;

        base.TakeAction();
    }
}
