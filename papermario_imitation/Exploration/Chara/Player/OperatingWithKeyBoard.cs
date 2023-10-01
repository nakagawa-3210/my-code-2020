using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatingWithKeyBoard : MonoBehaviour
{
  // 移動させる親元の取得
  [SerializeField] GameObject rootObject;
  [SerializeField] GameObject isOnGroundDetector;
  [SerializeField] float jumpSpeed = 10.0f;
  [SerializeField] float walkSpeed = 5.0f;
  [SerializeField] float jumpDecrease = 10.0f;
  private HanaPlayerScript hanaPlayerScript;

  private float moveX;
  private float moveY;
  private float moveZ;

  private bool isDirectionLeft;
  private bool isOnGround;
  private bool startJumping;
  private bool startFalling;
  private bool collideWall;

  void Start ()
  {
    isDirectionLeft = false;
    startJumping = false;
    startFalling = false;

    isOnGround = isOnGroundDetector.GetComponent<IsOnGroundManager> ().IsOnGround;
    hanaPlayerScript = transform.parent.gameObject.GetComponent<HanaPlayerScript> ();
  }

  // transform.Translateを用いない方法に変更
  // 
  public void MovePlayerBasedOnMoveValue ()
  {
    // ジャンプや落下のvelocityはそのままにしておく
    moveY = rootObject.GetComponent<Rigidbody> ().velocity.y;
    rootObject.GetComponent<Rigidbody> ().velocity = new Vector3 (moveX, moveY, moveZ);
    // Debug.Log ("いどう");

    // ジャンプ
    if (startJumping)
    {
      startJumping = false;
      // Debug.Log ("ジャンプしてる");
      rootObject.GetComponent<Rigidbody> ().AddForce (Vector3.up * jumpSpeed, ForceMode.Impulse);
    }

    // ジャンプを途中でやめたときの自然落下開始
    if (startFalling)
    {
      float speed = rootObject.GetComponent<Rigidbody> ().velocity.y;
      // Debug.Log ("speed : " + speed);
      if (speed > 0)
      {
        float velY = rootObject.GetComponent<Rigidbody> ().velocity.y - jumpDecrease * Time.deltaTime;
        float velX = rootObject.GetComponent<Rigidbody> ().velocity.x;
        float velZ = rootObject.GetComponent<Rigidbody> ().velocity.z;
        rootObject.GetComponent<Rigidbody> ().velocity = new Vector3 (velX, velY, velZ);
      }
      else
      {
        startFalling = false;
      }
    }
    // Debug.Log ("collideWall : " + collideWall);
  }
  // private void Update ()
  // {
  //   Debug.Log ("startJumping : " + startJumping);
  // }

  public void ResetVelocity ()
  {
    float velocityZero = 0.0f;
    rootObject.GetComponent<Rigidbody> ().velocity = new Vector3 (velocityZero, velocityZero, velocityZero);
  }

  // ベクトルの削除
  public void StopGetAxisMotionAndSpaceJump ()
  {
    float stop = 0.0f;
    float gravity = rootObject.GetComponent<Rigidbody> ().velocity.y;
    rootObject.GetComponent<Rigidbody> ().velocity = new Vector3 (stop, gravity, stop);

    startJumping = false;
  }

  // キーボードの使用による移動の制限
  // https://teratail.com/questions/195996
  // キーボードによっては3つ以上のキーの同時押しに反応しない組み合わせがある
  // 矢印キー二つ同時押しでの斜め移動中にジャンプが出来ない挙動が生じる
  public void ManageMovement ()
  {
    // キーで移動方向操作
    moveX = Input.GetAxis ("Horizontal") * walkSpeed;
    moveZ = Input.GetAxis ("Vertical") * walkSpeed;

    // ジャンプ移動
    isOnGround = isOnGroundDetector.GetComponent<IsOnGroundManager> ().IsOnGround;
    // 地上にいるかつ、スペースキー
    if (Input.GetKeyDown (KeyCode.Space) && isOnGround)
    {
      // Debug.Log ("ジャンプしてる");
      startJumping = true;
    }

    // ジャンプ高さ調整
    // 空中でスペースキーを離した際
    if (Input.GetKeyUp (KeyCode.Space) && !isOnGround)
    {
      float speed = rootObject.GetComponent<Rigidbody> ().velocity.y;
      // 上昇する余力がある時
      if (speed > 0)
      {
        startFalling = true;
      }
    }
  }

  void OnCollisionStay (Collision other)
  {
    if (other.collider.tag == "StageWall")
    {
      // Vector3 velocity = rootObject.GetComponent<Rigidbody> ().velocity;
      // Debug.Log ("velocity.x : " + velocity.x);
      // collideWall = true;
      // Debug.Log ("かべに衝突 : " + collideWall);
      // // // StopGetAxisMotionAndSpaceJump ();
      // float velY = rootObject.GetComponent<Rigidbody> ().velocity.y - jumpDecrease * Time.deltaTime;
      // float velX = rootObject.GetComponent<Rigidbody> ().velocity.x;
      // float velZ = rootObject.GetComponent<Rigidbody> ().velocity.z;
      // rootObject.GetComponent<Rigidbody> ().velocity = new Vector3 (velX, velY, velZ);
    }
  }

  // 壁の摩擦をゼロにして解決
  // collideWallがtrueになる式しか書いていなくても、次のフレームでfalseに変わる
  // Unityのライフサイクルを確認した上で、壁に接触している状態では落下するのみの条件を書いても、移動する処理が働いてしまう原因がわからない
  // （MovePlayerBasedOnMoveValueはFixedUpdateで呼んでいて、そのあとにOnCollision系が呼ばれるので、OnCollisionStayに移動制限の処理を書いてもダメだった
  // private void OnCollisionEnter (Collision other)
  // {
  //   if (other.collider.tag == "StageWall")
  //   {
  //     collideWall = true;
  //   }
  // }

  // void OnCollisionExit (Collision other)
  // {
  //   Debug.Log ("壁から離れた");
  //   // collideWall = false;
  // }

}