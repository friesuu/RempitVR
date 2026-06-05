using UnityEngine;

public class MotorbikeRev : MonoBehaviour
{
    public AudioSource exhaustSound;
    public ParticleSystem exhaustSmoke;

    void Update()
    {
        // Detects when the player holds down the Spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (exhaustSound != null) exhaustSound.Play();
            if (exhaustSmoke != null) exhaustSmoke.Play();
        }

        // Stops the smoke when they let go of the Spacebar
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (exhaustSmoke != null) exhaustSmoke.Stop();
        }
    }
}