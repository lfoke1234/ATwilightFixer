using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Snow : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    ParticleSystem.VelocityOverLifetimeModule velocityModule;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        velocityModule = _particleSystem.velocityOverLifetime;
    }

    private void Update()
    {
        if (PlayerManager.instance.player.rb.velocity.x > 0)
        {
            velocityModule.x = -1;
        }
        else if (PlayerManager.instance.player.rb.velocity.x < 0)
        {
            velocityModule.x = 1;
        }
        else if(PlayerManager.instance.player.rb.velocity.x == 0)
        {
            velocityModule.x = 0;
        }
    }
}
