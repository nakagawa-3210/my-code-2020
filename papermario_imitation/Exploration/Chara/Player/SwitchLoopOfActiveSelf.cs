using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLoopOfActiveSelf
{
  private float timeElapsed;
  private float changeTime;
  private bool isActive;

  public SwitchLoopOfActiveSelf ()
  {
    timeElapsed = 0.0f;
    changeTime = 0.2f;

    isActive = true;
  }

  public void SwitchFrontAndBackMeshWithBoneActiveCondition (GameObject frontMeshWithBone, GameObject backMeshWithBone)
  {
    timeElapsed += Time.deltaTime;
    if (timeElapsed >= changeTime)
    {
      if (isActive)
      {
        isActive = false;
        frontMeshWithBone.SetActive (false);
        backMeshWithBone.SetActive (false);
      }
      else
      {
        isActive = true;
        frontMeshWithBone.SetActive (true);
        backMeshWithBone.SetActive (true);
      }
      timeElapsed = 0.0f;
    }
  }

  public void ActivateSelfMeshWithBone (GameObject frontMeshWithBone, GameObject backMeshWithBone)
  {
    frontMeshWithBone.SetActive (true);
    backMeshWithBone.SetActive (true);
  }

}