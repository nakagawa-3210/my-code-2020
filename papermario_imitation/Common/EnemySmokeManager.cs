using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmokeManager : MonoBehaviour
{
  [SerializeField] ParticleSystem smokeParticle;

  public void PlayDefeatedSmoke ()
  {
    smokeParticle.Play ();
  }

  public bool EndPlayingParticle ()
  {
    return !smokeParticle.isPlaying;
  }
}