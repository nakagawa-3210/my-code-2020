using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharactorOrderInLayerManager : MonoBehaviour
{
  [SerializeField] GameObject rootParent;
  [SerializeField] GameObject charactorImg;
  private int currentImgOrderInLayer;
  private int imgOrderInLayer;
  private float rootParentPositionZ;
  void Start ()
  {
    imgOrderInLayer = charactorImg.GetComponent<SpriteRenderer> ().sortingOrder;
    rootParentPositionZ = rootParent.transform.position.z;
  }
  void Update ()
  {
    // 自身の親の位置取得
    rootParentPositionZ = rootParent.transform.position.z;
  }

  void OnTriggerStay (Collider other)
  {
    if (other.CompareTag ("Mob"))
    {
      // モブの描画順と自身の描画順を比較する
      int mobImgOrderInLayer = other.GetComponent<GameCharactorOrderInLayerManager> ().ImgOrderInLayer;
      // モブの親のｚ軸位置と、自身のｚ軸位置を比較する
      float mobPositionZ = other.GetComponent<GameCharactorOrderInLayerManager> ().RootParentPositionZ;
      // 自身の描画順がモブの描画順より優先されている時に、自身の位置がモブよりも後ろだった場合
      // ゲームシーン内でのz軸位置の値が小さいほど画面の前に来る
      if (imgOrderInLayer > mobImgOrderInLayer && rootParentPositionZ > mobPositionZ)
      {
        // 自身の描画優先度を下げる
        imgOrderInLayer = mobImgOrderInLayer - 1;
        charactorImg.GetComponent<SpriteRenderer> ().sortingOrder = imgOrderInLayer;
      }
      if (imgOrderInLayer < mobImgOrderInLayer && rootParentPositionZ < mobPositionZ)
      {
        // 自身の描画優先度を上げる
        imgOrderInLayer = mobImgOrderInLayer + 1;
        charactorImg.GetComponent<SpriteRenderer> ().sortingOrder = imgOrderInLayer;
      }
    }
  }

  public int ImgOrderInLayer
  {
    get { return imgOrderInLayer; }
  }

  public float RootParentPositionZ
  {
    get { return rootParentPositionZ; }
  }
}