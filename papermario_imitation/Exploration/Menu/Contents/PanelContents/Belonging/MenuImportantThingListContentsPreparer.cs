using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuImportantThingListContentsPreparer
{
  private JsonReaderFromResourcesFolder jsonReaderFromResourcesFolder;
  private BaseMenuListPreparer baseMenuListPreparer;
  private GameObject buttonForImportantThings;
  private Transform importantThingListContainer;
  private ImportantThingDataArray importantThingDataArray;
  private string nameTextGameObjectName;
  private string imgGameObjectName;
  public MenuImportantThingListContentsPreparer (
    GameObject buttonForImportantThings,
    Transform importantThingListContainer,
    string imgGameObjectName,
    string nameTextGameObjectName
  )
  {
    jsonReaderFromResourcesFolder = new JsonReaderFromResourcesFolder ();
    baseMenuListPreparer = new BaseMenuListPreparer ();
    this.buttonForImportantThings = buttonForImportantThings;
    this.importantThingListContainer = importantThingListContainer;
    this.imgGameObjectName = imgGameObjectName;
    this.nameTextGameObjectName = nameTextGameObjectName;
    importantThingDataArray = jsonReaderFromResourcesFolder.GetImportantThingDataArray ("JSON/GameImportantThingsData");
    SetupImportantThingListViewButtons ();
  }

  void SetupImportantThingListViewButtons ()
  {
    string[] havingImportantThingsArr = SaveSystem.Instance.userData.havingImportantThing;
    for (var i = 0; i < havingImportantThingsArr.Length; i++)
    {
      string importantThingName = havingImportantThingsArr[i];
      GameObject newImportantThingButton = GameObject.Instantiate<GameObject> (buttonForImportantThings);
      Sprite buttonSprite = new GetGameSprite ().GetSameNameImportantThingSprite (importantThingName);
      baseMenuListPreparer.SetupListButtonBaseInformation (
        newImportantThingButton,
        importantThingListContainer,
        buttonSprite,
        importantThingName,
        imgGameObjectName,
        nameTextGameObjectName
      );
      SetImportantThingButtonInformation (importantThingName, newImportantThingButton, importantThingDataArray);
    }
  }

  void SetImportantThingButtonInformation (string importantThingName, GameObject newButton, ImportantThingDataArray importantThingDataArray)
  {
    BelongingButtonInfoContainer buttonInfoContainer = newButton.GetComponent<BelongingButtonInfoContainer> ();
    buttonInfoContainer.BelongingName = importantThingName;
    foreach (var importantThing in importantThingDataArray.gameImportantThings)
    {
      if (importantThingName == importantThing.name)
      {
        buttonInfoContainer.Description = importantThing.description;
        buttonInfoContainer.Type = BelongingButtonInfoContainer.State.importantThing.ToString ();
      }
    }
  }
}