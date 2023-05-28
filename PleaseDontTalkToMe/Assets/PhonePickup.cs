using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhonePickup : MonoBehaviour
{
    public GameObject pickUpVFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var phone = collision.GetComponent<ThrowPhone>();

        if (phone == null) return;

        phone.AddAmmo();
        var vfx = Instantiate(pickUpVFX, transform.position, Quaternion.identity);
        Destroy(vfx, 2.0f);
        Destroy(gameObject);
    }
}
