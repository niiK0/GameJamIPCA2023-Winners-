using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenFader : MonoBehaviour
{
    public Animator anim;

    public void TriggerFadeIn()
    {
        anim.SetTrigger("FadeIn");
    }

    public void TriggerFadeOut()
    {
        anim.SetTrigger("FadeOut");
    }
}
