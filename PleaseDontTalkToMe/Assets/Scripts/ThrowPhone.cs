using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowPhone : MonoBehaviour
{
    public GameObject phoneGO;
    public Transform firePoint;
   
    public int phoneCharges = 1;
    public float minForce = 10;
    public float maxForce = 100;
    public float timeToMaxForce = 1;

    public bool chargingThrow;

    //Local
    public float finalThrowForce = 0;
    public float chargeTimer = 0;
    public Vector3 throwDirection;

    [Header("Visuals")]
    public Image chargeBar;
    public Color minForceColor;
    public Color maxForceColor;

    [Header("Requirements")]
    private InputHolder inputHolder;
    Animator anim;


    private void Awake()
    {
        inputHolder = GetComponent<InputHolder>();  
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (chargingThrow)
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Clamp(chargeTimer, 0, timeToMaxForce);
        }
        else chargeBar.transform.parent.gameObject.SetActive(false);

        if (inputHolder.throwObject.isAvailable && Input.GetKeyDown(inputHolder.throwObject.keyCode) && phoneCharges > 0) StartCharging();
        if (chargingThrow && Input.GetKeyUp(inputHolder.throwObject.keyCode)) FinishCharging();

        chargeBar.fillAmount = chargeTimer / timeToMaxForce;
        chargeBar.color = Color.Lerp(minForceColor, maxForceColor, chargeTimer / timeToMaxForce);

    }

    private void FinishCharging()
    {
        float interpolationFactor = Mathf.Clamp01(chargeTimer / timeToMaxForce);

        finalThrowForce = Mathf.Lerp(minForce, maxForce, interpolationFactor);
        Debug.Log("Final Throw Force Was" + finalThrowForce);
        throwDirection = CalculateThrowDirection();

        //Animation Event part
        anim.SetTrigger("Throw");
        //LaunchPhone();

        chargingThrow = false;
        chargeTimer = 0;
    }

    private void LaunchPhone()
    {
        var phone = Instantiate(phoneGO, firePoint.position, Quaternion.identity);
        phone.GetComponent<Rigidbody2D>().AddForce(throwDirection * finalThrowForce);
        phoneCharges--;
        UI_Manager.instance.phoneCharges.text = phoneCharges.ToString();
    }

    public void AddAmmo()
    {
        phoneCharges++;
        UI_Manager.instance.phoneCharges.text = phoneCharges.ToString();
    }

    private Vector3 CalculateThrowDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return (mousePos - firePoint.position).normalized;
    }

    private void StartCharging()
    {
        chargingThrow = true;
        chargeBar.transform.parent.gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(firePoint.position, throwDirection * 100);
    }
}
