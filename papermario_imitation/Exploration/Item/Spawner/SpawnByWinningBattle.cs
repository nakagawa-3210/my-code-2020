using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;

public class SpawnByWinningBattle : MonoBehaviour
{
  [SerializeField] string[] itemNameArray = default;
  [SerializeField] bool canSpawnCoin = default;
  [SerializeField] bool canSpawnItem = default;

  private Vector3 selfPosition;
  private SpawnManager spawnManager;
  private SEManager seManager;

  private bool isSpawning;
  private bool endSpawn;

  public SpawnManager StageSpawnManager
  {
    set { spawnManager = value; }
  }

  public async UniTask SpawnSomething ()
  {
    // Debug.Log ("アイテム排出関数は呼ばれてる");
    await UniTask.WaitUntil (() => spawnManager != null);
    seManager = SEManager.Instance;
    selfPosition = this.transform.position;
    // ランダムにアイテムかコインを出現
    // if (Random.Range (0, 2) == 0)
    // {
    //   SpawnItem ();
    // }
    // else
    // {
    //   SpawnCoin ();
    // }
    // serializeに従う
    if (canSpawnItem)
    {
      await SpawnItem ();
    }
    else if (canSpawnCoin)
    {
      await SpawnCoin ();
    }
  }

  async UniTask SpawnItem ()
  {
    string itemName = itemNameArray[Random.Range (0, itemNameArray.Length)];
    await spawnManager.SpawnItem (selfPosition, itemName);
  }

  async UniTask SpawnCoin ()
  {
    await spawnManager.SpawnCoin (selfPosition);
  }

}