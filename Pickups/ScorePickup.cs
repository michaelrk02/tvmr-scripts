using UnityEngine;
using System.Collections;

public class ScorePickup : Pickup
{
    protected override void TakeAction()
    {
        Player player = GameObject.Find("Tank").GetComponent<Player>();
        player.score += player.level * (int)(world.settings.scoreIncreaseFactor * 500.0f);

        base.TakeAction();
    }
}
