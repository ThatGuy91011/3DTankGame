using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PowerUp powerup;

    public AudioClip FeedbackAudioClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        // variable to store other object's PowerupController - if it has one
        PowerUpController powCon = other.GetComponent<PowerUpController>();

        // If the other object has a PowerupController
        if (powCon != null)
        {
            // Add the powerup
            powCon.Add(powerup);
            if (FeedbackAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(FeedbackAudioClip, gameObject.GetComponent<Transform>().position, 1.0f);
            }
            // Destroy this pickup
            Destroy(this.gameObject);
        }


    }
}
