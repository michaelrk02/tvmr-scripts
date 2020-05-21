using UnityEngine;
using System.Collections;

public class HealthRestorePickup : Pickup
{
    protected override void TakeAction()
    {
        Player player = GameObject.Find("Tank").GetComponent<Player>();
        player.health = player.maxHealth;

        base.TakeAction();
    }
}
