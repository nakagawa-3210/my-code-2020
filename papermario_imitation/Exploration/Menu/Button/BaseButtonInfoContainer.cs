using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseButtonInfoContainer : MonoBehaviour
{
  
  private bool isSelected = false;
  void Start ()
  {
    Button button = gameObject.GetComponent<Button> ();
    button.onClick.AddListener (() =>
    {
      Selected ();
    });
  }

  public virtual void Selected ()
  {
    isSelected = true;
  }
  
  public bool IsSelected
  {
    set { isSelected = value; }
    get { return isSelected; }
  }
}