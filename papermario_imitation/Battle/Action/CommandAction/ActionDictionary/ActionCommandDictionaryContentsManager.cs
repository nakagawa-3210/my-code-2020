using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCommandDictionaryContentsManager
{
  private Image enemyImage;
  private Text enemyNameText;

  public ActionCommandDictionaryContentsManager (
    Image enemyImage,
    Text enemyNameText
  )
  {
    this.enemyImage = enemyImage;
    this.enemyNameText = enemyNameText;
  }

  public void SetupDictionaryContents (string enemyImageName, string enemyName)
  {
    enemyImage.sprite = new GetGameSprite ().GetSameNameEnemySprite (enemyImageName);
    enemyNameText.text = enemyName;
  }
}