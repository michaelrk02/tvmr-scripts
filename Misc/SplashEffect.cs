using UnityEngine;
using System.Collections;

public class SplashEffect : Effect
{
    // Constants
    public float lifetime;

    protected override void Start()
    {
        Invoke("Disappear", lifetime);
        
        base.Start();
    }
}
