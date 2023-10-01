using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UniRx.Async;
using UnityEngine;

public class SpawnByAwarenessArea : MonoBehaviour
{
  [SerializeField] string[] itemNameArray;
  [SerializeField] bool canSpawnCoin = default;
  [SerializeField] bool canSpawnItem = default;
  private SpawnManager spawnManager;
  private SEManager seManager;

  private bool isSpawning;
  private bool endSpawn;

  public SpawnManager StageSpawnManager
  {
    set { spawnManager = value; }
  }

  void Start ()
  {
    seManager = SEManager.Instance;
    isSpawning = false;
  }

  void OnTriggerEnter (Collider other)
  {
    if (other.tag == "Player")
    {
      GameObject awarenessSign = other.gameObject.transform.Find ("Signs").Find ("AwarenessSign").gameObject;
      awarenessSign.SetActive (true);
    }
  }

  async UniTask OnTriggerStay (Collider other)
  {
    if (other.tag == "Player" && Input.GetKeyDown (KeyCode.Space))
    {
      HanaPlayerScript.State hanaPlayerState = other.GetComponent<HanaPlayerScript> ().PlayerState;
      if (!isSpawning && hanaPlayerState == HanaPlayerScript.State.Normal)
      {
        // Input.ResetInputAxes ();

        isSpawning = true;

        seManager.Play (SEPath.WEED);
        await SpawnCoin ();
        await SpawnItem ();

        int intervalMilSec = 500;
        await UniTask.Delay (intervalMilSec);
        isSpawning = false;
      }
    }
  }

  async UniTask SpawnCoin ()
  {
    if (canSpawnCoin)
    {
      canSpawnCoin = false;

      // 改修予定
      Vector3 selfPosition = this.transform.position;
      selfPosition.y += 1.5f;
      spawnManager.SpawnCoin (selfPosition);
    }
  }

  async UniTask SpawnItem ()
  {
    if (canSpawnItem)
    {
      canSpawnItem = false;

      Vector3 selfPosition = this.transform.position;
      selfPosition.y += 1.5f;
      string itemName = itemNameArray[Random.Range (0, itemNameArray.Length)];
      spawnManager.SpawnItem (selfPosition, itemName);
    }
  }

  void OnTriggerExit (Collider other)
  {
    if (other.tag == "Player")
    {
      GameObject awarenessSign = other.gameObject.transform.Find ("Signs").Find ("AwarenessSign").gameObject;
      awarenessSign.SetActive (false);
    }
  }

}