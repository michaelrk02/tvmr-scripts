using UnityEngine;
using System.Collections;

public class HealthPickup : Pickup
{
    protected override void TakeAction()
    {
        Player player = GameObject.Find("Tank").GetComponent<Player>();
        player.health += 25.0f;

        base.TakeAction();
    }
}
