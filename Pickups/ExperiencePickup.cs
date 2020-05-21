using UnityEngine;
using System.Collections;

public class ExperiencePickup : Pickup
{
    protected override void TakeAction()
    {
        Player player = GameObject.Find("Tank").GetComponent<Player>();
        player.experience += world.settings.xpIncreaseFactor * 50.0f;

        base.TakeAction();
    }
}
