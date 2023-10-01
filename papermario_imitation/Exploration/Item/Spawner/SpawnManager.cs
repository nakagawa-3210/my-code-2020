using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
  [SerializeField] StageItem stageItemPrefab;
  [SerializeField] StageCoin stageCoinPrefab;

  void Start ()
  {
    SetupSpawnAreasStageSpawnManager ();
  }

  // 改修予定
  void SetupSpawnAreasStageSpawnManager ()
  {
    GameObject[] spawnAreaArray = GameObject.FindGameObjectsWithTag ("SpawnArea");
    foreach (var spawnArea in spawnAreaArray)
    {
      if (spawnArea.GetComponent<SpawnByAwarenessArea> () != null)
      {
        SpawnByAwarenessArea spawnByAwarenessArea = spawnArea.GetComponent<SpawnByAwarenessArea> ();
        spawnByAwarenessArea.StageSpawnManager = this;
      }
      else if (spawnArea.GetComponent<SpawnByWinningBattle> () != null)
      {
        SpawnByWinningBattle spawnByWinningBattle = spawnArea.GetComponent<SpawnByWinningBattle> ();
        spawnByWinningBattle.StageSpawnManager = this;
      }

    }
  }

  public async UniTask SpawnItem (Vector3 spawnPosition, string itemName)
  {
    GameObject stageItem = null;
    stageItem = GameObject.Instantiate (stageItemPrefab.gameObject);
    stageItem.transform.position = spawnPosition;

    Sprite itemSprite = new GetGameSprite ().GetSameNameItemSprite (itemName);
    stageItem.GetComponent<StageItem> ().ChangeSelfSprite (itemSprite);

    await UniTask.WaitUntil (() => stageItem != null);
  }

  public async UniTask SpawnCoin (Vector3 spawnPosition)
  {
    GameObject stageCoin = null;
    stageCoin = GameObject.Instantiate (stageCoinPrefab.gameObject);
    stageCoin.transform.position = spawnPosition;

    await UniTask.WaitUntil (() => stageCoin != null);
  }
}