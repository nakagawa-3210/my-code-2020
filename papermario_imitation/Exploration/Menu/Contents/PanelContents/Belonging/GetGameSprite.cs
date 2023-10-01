using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGameSprite
{
  // 引数のitemNameにはアルファベットはこないとり方
  public Sprite GetSameNameItemSprite (string itemName)
  {
    Sprite itemImg = null;
    ItemDataArray itemDataArray = new JsonReaderFromResourcesFolder ().GetItemDataArray ("JSON/GameItemsData");
    foreach (var item in itemDataArray.gameItems)
    {
      // プレイヤーの所有アイテム名とアイテムデータのアイテム名が一致
      if (itemName == item.name)
      {
        foreach (var itemSprite in GetItemSpriteArray ())
        {
          // 一致したアイテムの画像名がリソースフォルダから見つかった
          if (item.imgFileName == itemSprite.name)
          {
            itemImg = itemSprite;
          }
        }
      }
    }
    return itemImg;
  }
  public Sprite[] GetItemSpriteArray ()
  {
    string folderName = "Explore/Menu/Items";
    Sprite[] itemSpriteArr = Resources.LoadAll<Sprite> (folderName);
    return itemSpriteArr;
  }

  public Sprite GetSameNameImportantThingSprite (string importantThingName)
  {
    Sprite importantThingImg = null;
    ImportantThingDataArray importantThingDataArray = new JsonReaderFromResourcesFolder ().GetImportantThingDataArray ("JSON/GameImportantThingsData");
    foreach (var importantTh in importantThingDataArray.gameImportantThings)
    {
      if (importantThingName == importantTh.name)
      {
        foreach (var importantSprite in GetImportantThingSpriteArray ())
        {
          if (importantTh.imgFileName == importantSprite.name)
          {
            importantThingImg = importantSprite;
          }
        }
      }
    }
    return importantThingImg;
  }
  public Sprite[] GetImportantThingSpriteArray ()
  {
    string folderName = "Explore/Menu/ImportantThings";
    Sprite[] importantSpriteArr = Resources.LoadAll<Sprite> (folderName);
    return importantSpriteArr;
  }

  public Sprite GetSameNameBadgeSprite (string badgeName)
  {
    Sprite badgeImg = null;
    BadgeDataArray havingBadgeDataArray = new JsonReaderFromResourcesFolder ().GetBadgeDataArray ("JSON/GameBadgesData");
    foreach (var badge in havingBadgeDataArray.gameBadges)
    {
      if (badgeName == badge.name)
      {
        foreach (var badgeSprite in GetBadgeSpriteArray ())
        {
          if (badge.imgFileName == badgeSprite.name)
          {
            badgeImg = badgeSprite;
          }
        }
      }
    }
    return badgeImg;
  }
  public Sprite[] GetBadgeSpriteArray ()
  {
    string folderName = "Explore/Menu/Badges";
    Sprite[] importantSpriteArr = Resources.LoadAll<Sprite> (folderName);
    return importantSpriteArr;
  }

  // パートナーのスキル名はskill+数字
  // パートナーで共通のスキル名を使っている
  public Sprite GetSameNamePartnerSkillSprite (string skillName)
  {
    Sprite skillImg = null;
    Sprite[] partnerSkillSpriteArray = GetPartnerSkillSpriteArray ();
    foreach (var partnerSkillSprite in partnerSkillSpriteArray)
    {
      if (skillName == partnerSkillSprite.name)
      {
        skillImg = partnerSkillSprite;
      }
    }
    return skillImg;
  }
  public Sprite[] GetPartnerSkillSpriteArray ()
  {
    string folderName = "Explore/Menu/PartnerSkill";
    Sprite[] partnerSkillSpriteArr = Resources.LoadAll<Sprite> (folderName);
    return partnerSkillSpriteArr;
  }

  public Sprite GetSameNameStrategySprite (string strategyName)
  {
    Sprite strategyImg = null;
    Sprite[] strategySpriteArray = GetStrategySpriteArray ();
    foreach (var strategySprite in strategySpriteArray)
    {
      if (strategyName == strategySprite.name)
      {
        strategyImg = strategySprite;
      }
    }
    return strategyImg;
  }

  Sprite[] GetStrategySpriteArray ()
  {
    string folderName = "Battle/Strategy";
    Sprite[] strategySpriteArr = Resources.LoadAll<Sprite> (folderName);
    return strategySpriteArr;
  }

  public Sprite GetSameNameEnemySprite (string enemySpriteName)
  {
    Sprite enemyImage = null;
    Sprite[] enemySpriteArray = GetEnemySpriteArray ();
    foreach (var enemySprite in enemySpriteArray)
    {
      if (enemySpriteName == enemySprite.name)
      {
        enemyImage = enemySprite;
      }
    }
    return enemyImage;
  }

  Sprite[] GetEnemySpriteArray ()
  {
    string folderName = "Battle/EnemyImage";
    Sprite[] enemySpriteArr = Resources.LoadAll<Sprite> (folderName);
    return enemySpriteArr;
  }

  // ジェネリックを用いたいけどdataArray.gameItemsの変数がdataArrayの型によって異なるのでやり方がわからない
  // public Sprite GetSameNameSprite <Type> (string itemName, string spriteImgResourceFolderName, Type dataArray)
  // {
  //   Sprite itemImg = null;
  //   foreach (var item in dataArray.gameItems)
  //   {
  //     // プレイヤーの所有アイテム名とアイテムデータのアイテム名が一致
  //     if (itemName == item.name)
  //     {
  //       foreach (var itemSprite in GetSpriteArray (spriteImgResourceFolderName))
  //       {
  //         // 一致したアイテムの画像名がリソースフォルダから見つかった
  //         if (item.imgFileName == itemSprite.name)
  //         {
  //           itemImg = itemSprite;
  //         }
  //       }
  //     }
  //   }
  //   return itemImg;
  // }
  // public Sprite[] GetSpriteArray (string folderName)
  // {
  //   Sprite[] spriteArr = Resources.LoadAll<Sprite> (folderName);
  //   return spriteArr;
  // }

}