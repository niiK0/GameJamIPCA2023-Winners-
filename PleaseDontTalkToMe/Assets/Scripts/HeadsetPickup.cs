using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetPickup : MonoBehaviour
{
    public GameObject pickUpVFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var headset = collision.GetComponent<Headsets>();

        if(headset == null) return;

        headset.ActivateEffect();
        var vfx = Instantiate(pickUpVFX, transform.position, Quaternion.identity);
        Destroy(vfx, 2.0f);
        Destroy(gameObject);
    }
}
