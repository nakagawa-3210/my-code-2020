using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvasOpenCloseManager
{
  private GameObject defaultSelectedTagButton;
  private GameObject defaultContentsPanel;
  private GameObject descriptionPanel;
  private GameObject[] tagButtonList;
  private GameObject[] contentsPanelList;
  private List<Vector3> tagInitialPositionList;
  private Vector3 tagsOutOfScreenPosition;
  private Vector3 contentsPanelsOutOfScreenPosition;
  private Vector3 descriptionPanelInitialPosition;
  private Vector3 contentsPanelsInitialPosition;
  private Vector3 descriptionPanelOutOfScreenPosition;
  private float doTweenDurationTime;
  private float panelTweenRecoil;
  private float tagsOutOfScreenPositionY;
  private float panelsOutOfScreenPositionX;
  private bool openMenu;
  private bool startOpenCloseMenuTween;
  public MenuCanvasOpenCloseManager (
    GameObject defaultSelectedTagButton,
    GameObject defaultContentsPanel,
    GameObject descriptionPanel,
    GameObject[] tagButtonList,
    GameObject[] contentsPanelList,
    float doTweenDurationTime
  )
  {
    this.defaultSelectedTagButton = defaultSelectedTagButton;
    this.defaultContentsPanel = defaultContentsPanel;
    this.descriptionPanel = descriptionPanel;
    this.tagButtonList = tagButtonList;
    this.contentsPanelList = contentsPanelList;
    this.doTweenDurationTime = doTweenDurationTime;
    panelTweenRecoil = Screen.height / 160.0f;
    tagsOutOfScreenPositionY = Screen.height / 5.0f;
    panelsOutOfScreenPositionX = Screen.width;
    openMenu = false;
    startOpenCloseMenuTween = false;
    SetupMenuInitialPosition ();
    SetupMenu ();
  }
  
  public void OpenMenu ()
  {
    if (!openMenu && !startOpenCloseMenuTween)
    {
      openMenu = true;
      startOpenCloseMenuTween = true;
      ShowAllTags ();
      ShowDefaultContentsPanel ();
      ShowDescriptionPanel ();
    }
  }
  public async void CloseMenu ()
  {
    if (openMenu && !startOpenCloseMenuTween)
    {
      openMenu = false;
      startOpenCloseMenuTween = true;
      HideAllContentPanels ();
      HideDescriptionPanel ();
      await HideAllTags ();
    }
  }
  public bool IsMenuOpen ()
  {
    // メニューが開いている状態、またはtween途中の状態
    // メニューを閉じる時にカーソルの移動が出来てしまう不具合を生むのでコメントアウト中
    // 仕組みを考え直すときにつかうかもなので残す
    // return (openMenu || startOpenCloseMenuTween);
    return openMenu;
  }
  public bool StartOpenCloseMenuTween ()
  {
    return startOpenCloseMenuTween;
  }
  public bool IsMenuClosedCompletely ()
  {
    // メニューが開かれていないかつ、tweenアニメも終わっている状態
    return (!openMenu && !startOpenCloseMenuTween);
  }
  void SetupMenu ()
  {
    SetupTags ();
    SetupContentsPanel ();
    SetupDescriptionPanel ();
  }
  void SetupMenuInitialPosition ()
  {
    SetupTagsInitialPosition ();
    SetupContentsPanelInitialPosition ();
    SetupDescriptionPanelInitialPosition ();
  }
  
  // タグ関数 //
  void SetupTags ()
  {
    for (var tagNum = 0; tagNum < tagButtonList.Length; tagNum++)
    {
      Vector3 tagPosition = tagButtonList[tagNum].transform.position;
      tagPosition.y = tagPosition.y + tagsOutOfScreenPositionY;
      tagsOutOfScreenPosition = tagPosition;
      tagButtonList[tagNum].transform.position = tagsOutOfScreenPosition;
    }
  }
  void SetupTagsInitialPosition ()
  {
    List<Vector3> positionList = new List<Vector3> ();
    for (var tagNum = 0; tagNum < tagButtonList.Length; tagNum++)
    {
      Vector3 tagPosition = tagButtonList[tagNum].transform.position;
      positionList.Add (tagPosition);
    }
    tagInitialPositionList = positionList;
  }

  // voidを直す
  async void ShowAllTags ()
  {
    for (var tagNum = 0; tagNum < tagButtonList.Length; tagNum++)
    {
      var tag = tagButtonList[tagNum];
      await ShowTagTween (tag, tagNum);
    }
    startOpenCloseMenuTween = false;
  }
  async UniTask ShowTagTween (GameObject tag, int tagNum)
  {
    // Debug.Log("tagInitialPositionList : " + tagInitialPositionList);
    Vector3 setPosition = tagInitialPositionList[tagNum];
    Vector3 recoilPosition = setPosition;
    float recoil = 10.0f;
    recoilPosition.y = setPosition.y - recoil;
    // 二回のtweenがあるので時間をその分短くしている
    await tag.transform.DOMove (recoilPosition, doTweenDurationTime / ((tagButtonList.Length) * 2));
    await tag.transform.DOMove (setPosition, doTweenDurationTime / ((tagButtonList.Length) * 2));
  }
  async UniTask HideAllTags ()
  {
    for (var tagNum = tagButtonList.Length - 1; tagNum >= 0; tagNum--)
    {
      var tag = tagButtonList[tagNum];
      await HideTagTween (tag, tagNum);
    }
    startOpenCloseMenuTween = false;
  }
  async UniTask HideTagTween (GameObject tag, int tagNum)
  {
    Vector3 setPosition = tagInitialPositionList[tagNum];
    setPosition.y = setPosition.y + tagsOutOfScreenPositionY;
    await tag.transform.DOMove (setPosition, doTweenDurationTime / (tagButtonList.Length * 1.5f));
  }

  // コンテンツパネル関数 //
  void SetupContentsPanelInitialPosition ()
  {
    contentsPanelsInitialPosition = defaultContentsPanel.transform.position;
  }
  void SetupContentsPanel ()
  {
    for (var panelNum = 0; panelNum < contentsPanelList.Length; panelNum++)
    {
      Vector3 panelPosition = contentsPanelList[panelNum].transform.position;
      panelPosition.x = panelPosition.x + panelsOutOfScreenPositionX;
      contentsPanelsOutOfScreenPosition = panelPosition;
      contentsPanelList[panelNum].transform.position = contentsPanelsOutOfScreenPosition;
    }
  }
  void ShowDefaultContentsPanel ()
  {
    ShowContentsPanelTween (defaultContentsPanel);
  }
  public void ShowContentsPanelTween (GameObject contentsPanel)
  {
    Vector3 setPosition = contentsPanelsInitialPosition;
    Vector3 recoilPosition = setPosition;
    recoilPosition.x = setPosition.x - panelTweenRecoil;
    // パネルの表示操作にて表示されるべきパネルに誤作動が起きるのでコメントアウト
    // await contentsPanel.transform.DOMove (recoilPosition, doTweenDurationTime / 2);
    contentsPanel.transform.DOMove (setPosition, doTweenDurationTime / 2);
  }
  void HideAllContentPanels ()
  {
    for (var panelNum = contentsPanelList.Length - 1; panelNum >= 0; panelNum--)
    {
      var panel = contentsPanelList[panelNum];
      HideContentsPanelTween (panel);
    }
  }
  public void HideContentsPanelTween (GameObject contentsPanel)
  {
    contentsPanel.transform.DOMove (contentsPanelsOutOfScreenPosition, doTweenDurationTime / 2);
  }

  // ディスクリプションパネル関数 //
  void SetupDescriptionPanelInitialPosition ()
  {
    descriptionPanelInitialPosition = descriptionPanel.transform.position;
  }
  void SetupDescriptionPanel ()
  {
    Vector3 panelPosition = descriptionPanelInitialPosition;
    panelPosition.x = panelPosition.x - panelsOutOfScreenPositionX;
    descriptionPanelOutOfScreenPosition = panelPosition;
    descriptionPanel.transform.position = descriptionPanelOutOfScreenPosition;
  }
  void ShowDescriptionPanel ()
  {
    Vector3 setPosition = descriptionPanelInitialPosition;
    Vector3 recoilPosition = setPosition;
    recoilPosition.x = setPosition.x + panelTweenRecoil;
    // await descriptionPanel.transform.DOMove (recoilPosition, doTweenDurationTime / 2);
    descriptionPanel.transform.DOMove (setPosition, doTweenDurationTime / 2);
  }
  void HideDescriptionPanel ()
  {
    descriptionPanel.transform.DOMove (descriptionPanelOutOfScreenPosition, doTweenDurationTime / 2);
  }
}