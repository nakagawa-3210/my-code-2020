using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelectableButtonClickSound : MenuButtonClickSound
{
  [SerializeField] bool isBelongingButton = default;
  [SerializeField] bool isBadgeButton = default;
  private BelongingButtonInfoContainer belongingButtonInfoContainer;
  private BadgeButtonInfoContainer badgeButtonInfoContainer;
  void Start ()
  {
    if (isBelongingButton)
    {
      belongingButtonInfoContainer = this.gameObject.GetComponent<BelongingButtonInfoContainer> ();
    }
    else if (isBadgeButton)
    {
      badgeButtonInfoContainer = this.gameObject.GetComponent<BadgeButtonInfoContainer> ();
    }
  }

  public void OnClickSelectableButton ()
  {
    if ((belongingButtonInfoContainer != null && belongingButtonInfoContainer.IsSelectable) || badgeButtonInfoContainer != null)
    {
      base.OnClickButton ();
    }
  }
}