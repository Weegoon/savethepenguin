using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectComplete : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> listParticles;

    private void Start()
    {
        //if (GameController.Ins.LargeScreen)
        //{
        //    Vector3 pos = fxLeft.transform.position;
        //    pos.x = -largeScreenPos;
        //    fxLeft.transform.position = pos;

        //    pos = fxRight.transform.position;
        //    pos.x = largeScreenPos;
        //    fxRight.transform.position = pos;
        //}
    }

    public void PlayEffect()
    {
        Vector3 v = transform.position;
        transform.position = v;

        listParticles.ForEach(item => item.Play());
    }
}
