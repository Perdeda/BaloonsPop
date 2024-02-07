using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particles;
    void Start()
    {
        Baloon.onBaloonPop += Baloon_onBaloonPop;
    }

    private void Baloon_onBaloonPop(int reward, Baloon baloon,bool byPlayer)
    {
        if (byPlayer)
        {
            particles.transform.position = baloon.transform.position;
            particles.Play();
        }
    }
    private void OnDestroy()
    {
        Baloon.onBaloonPop -= Baloon_onBaloonPop;
    }
}
