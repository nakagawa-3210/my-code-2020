using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerInfoContainer : MonoBehaviour
{
  private string partnerName = "";
  private string description = "";

  public string PartnerName
  {
    set { partnerName = value; }
    get { return partnerName; }
  }

  public string Description
  {
    set { description = value; }
    get { return description; }
  }

}