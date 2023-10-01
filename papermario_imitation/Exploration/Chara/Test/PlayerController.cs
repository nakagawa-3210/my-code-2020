using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [SerializeField] GameObject playerImg;
  private Animator playerAnimator;
  // Start is called before the first frame update
  void Start ()
  {
    playerAnimator = playerImg.GetComponent<Animator> ();
  }

  // Update is called once per frame
  void Update ()
  {
    Vector3 playerPosition = gameObject.transform.position;
    float positionX = playerPosition.x;
    float positionZ = playerPosition.z;
    // 移動テスト
    if (Input.GetKey (KeyCode.LeftArrow))
    {
      playerAnimator.SetBool ("IsWalk", true);
      positionX -= 0.01f;
      gameObject.transform.position = new Vector3 (positionX, playerPosition.y, playerPosition.z);
    }
    if (Input.GetKey (KeyCode.RightArrow))
    {
      playerAnimator.SetBool ("IsWalk", true);
      positionX += 0.01f;
      gameObject.transform.position = new Vector3 (positionX, playerPosition.y, playerPosition.z);
    }
    if (Input.GetKey (KeyCode.UpArrow))
    {
      positionZ += 0.01f;
      gameObject.transform.position = new Vector3 (playerPosition.x, playerPosition.y, positionZ);
    }
    if (Input.GetKey (KeyCode.DownArrow))
    {
      positionZ -= 0.01f;
      gameObject.transform.position = new Vector3 (playerPosition.x, playerPosition.y, positionZ);
    }
    if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.RightArrow))
    {
      playerAnimator.SetBool ("IsWalk", false);
    }

  }
}