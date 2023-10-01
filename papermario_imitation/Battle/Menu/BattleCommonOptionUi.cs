using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleCommonOptionUi : MonoBehaviour
{
  public GameObject forInstantiateTargetHandCursorPrefab;
  public GameObject forInstantiateTargetNameFramePrefab;
  public GameObject battleCommonOptionMultipleTarget;
  public GameObject listHandCursor;
  public GameObject targetHandCursor;
  public EventSystem eventSystem;
  public Transform optionBalloonMovePositionTransform;
  public GameObject selectedEnemyNameFrame;
  public GameObject optionButtonDescriptionFrame;
  public GameObject attackDescriptionFrame;
  public GameObject itemButton;
  public GameObject itemOptionListContainer;
  public GameObject strategyButton;
  public GameObject strategyOptionListContainer;
  public Sprite itemImage;
  public Sprite strategyImage;
}