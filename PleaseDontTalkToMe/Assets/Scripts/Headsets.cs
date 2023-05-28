using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headsets : MonoBehaviour
{
    public GameObject headsetVFX;
    public float invulnerabilityDuration = 10f;
    public string headsetLayer;
    private float invulnerabilityTimer = 0;
    bool invulnerable = false;
    string resetLayer;

    private void Awake()
    {
        resetLayer = transform.gameObject.layer.ToString();
    }
    public void ActivateEffect()
    {
        invulnerable = true;
        transform.gameObject.layer = LayerMask.NameToLayer(headsetLayer);
        headsetVFX.SetActive(true);
    }

    private void Update()
    {
        if(invulnerable) invulnerabilityTimer += Time.deltaTime;
        if (invulnerabilityTimer >= invulnerabilityDuration) DeactivateEffect();
    }

    private void DeactivateEffect()
    {
        invulnerable = false;
        transform.gameObject.layer = LayerMask.NameToLayer("Player");
        headsetVFX.SetActive(false);
    }
}
