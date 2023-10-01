using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ManageSpriteMeshOrder : MonoBehaviour
{
  // メッシュを持つ親元(実際に移動するGameObject)の取得
  [SerializeField] GameObject rootPosition;
  [SerializeField] GameObject frontMeshes;
  [SerializeField] GameObject backMeshes;
  private float rootPositionZ;
  private int meshesOrderInLayer;

  void Start ()
  {
    // 親元の位置
    rootPositionZ = rootPosition.transform.position.z;
    // 描画順
    meshesOrderInLayer = frontMeshes.GetComponent<SortingGroup> ().sortingOrder;
  }

  void Update ()
  {
    // 下記の内容をOnTriggerStayに書いても支障がないかを確認する
    // 親の位置監視
    rootPositionZ = rootPosition.transform.position.z;
    // 描画順監視
    meshesOrderInLayer = frontMeshes.GetComponent<SortingGroup> ().sortingOrder;
  }

  void OnTriggerStay (Collider other)
  {
    if (other.CompareTag ("Mob") || other.CompareTag ("Partner"))
    {
      // Debug.Log ("other.tag : " + other.tag);
      // Debug.Log ("other.name : " + other.name);
      // z軸位置の値が小さい程、描画の優先順位を高くする
      float otherCharaPosition = other.GetComponent<ManageSpriteMeshOrder> ().RootPositionZ;
      int otherCharaMeshesOrderInLayer = other.GetComponent<ManageSpriteMeshOrder> ().MeshesOrderInLayer;
      if (rootPositionZ < otherCharaPosition && meshesOrderInLayer <= otherCharaMeshesOrderInLayer)
      {
        // playerの描画が前にされるように調整
        // Debug.Log ("playerの描画が前にされるように調整");
        meshesOrderInLayer = otherCharaMeshesOrderInLayer + 1;
        frontMeshes.GetComponent<SortingGroup> ().sortingOrder = meshesOrderInLayer;
        backMeshes.GetComponent<SortingGroup> ().sortingOrder = meshesOrderInLayer;
      }
      if (rootPositionZ > otherCharaPosition && meshesOrderInLayer >= otherCharaMeshesOrderInLayer)
      {
        // playerの描画が後ろにされるように調整
        // Debug.Log ("playerの描画が前にされるように調整");
        meshesOrderInLayer = otherCharaMeshesOrderInLayer - 1;
        frontMeshes.GetComponent<SortingGroup> ().sortingOrder = meshesOrderInLayer;
        backMeshes.GetComponent<SortingGroup> ().sortingOrder = meshesOrderInLayer;
      }
    }
  }

  public float RootPositionZ
  {
    get { return rootPositionZ; }
  }

  public int MeshesOrderInLayer
  {
    get { return meshesOrderInLayer; }
  }
}