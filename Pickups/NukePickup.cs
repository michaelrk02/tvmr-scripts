using UnityEngine;
using System.Collections;

public class NukePickup : Pickup
{
    // Constants
    public SplashEffect explosion;

    protected override void TakeAction()
    {
        foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            // Disable boss :D
            if (obj.tag == "Alien" && !obj.name.Contains("Boss"))
            {
                SplashEffect exp = Instantiate<SplashEffect>(explosion);
                exp.gameObject.SetActive(true);
                exp.transform.position = obj.transform.position;

                Destroy(obj);
            }
        }

        base.TakeAction();
    }
}
