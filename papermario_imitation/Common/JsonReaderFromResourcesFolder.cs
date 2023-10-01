using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonReaderFromResourcesFolder
{

  public ItemDataArray GetItemDataArray (string filePath)
  {
    ItemDataArray items;
    TextAsset file = Resources.Load (filePath) as TextAsset;
    items = JsonUtility.FromJson<ItemDataArray> (file.text);
    return items;
  }

  public ImportantThingDataArray GetImportantThingDataArray (string filePath)
  {
    ImportantThingDataArray importantThings;
    TextAsset file = Resources.Load (filePath) as TextAsset;
    importantThings = JsonUtility.FromJson<ImportantThingDataArray> (file.text);
    return importantThings;
  }

  public PartnerDataArray GetPartnerDataArray (string filePath)
  {
    PartnerDataArray partners;
    TextAsset file = Resources.Load (filePath) as TextAsset;
    partners = JsonUtility.FromJson<PartnerDataArray> (file.text);
    return partners;
  }

  public PartnerSkillDataArray GetPartnerSkillDataArray (string filePath)
  {
    PartnerSkillDataArray skills;
    TextAsset file = Resources.Load (filePath) as TextAsset;
    skills = JsonUtility.FromJson<PartnerSkillDataArray> (file.text);
    return skills;
  }

  public EnemyDataArray GetEnemyDataArray (string filePath)
  {
    EnemyDataArray enemise;
    TextAsset file = Resources.Load (filePath) as TextAsset;
    enemise = JsonUtility.FromJson<EnemyDataArray> (file.text);
    return enemise;
  }

  public EnemySkillArray GetEnemySkillDataArray (string filePath)
  {
    EnemySkillArray enemySkills;
    TextAsset file = Resources.Load (filePath) as TextAsset;
    enemySkills = JsonUtility.FromJson<EnemySkillArray> (file.text);
    return enemySkills;
  }

  public BadgeDataArray GetBadgeDataArray (string filePath)
  {
    BadgeDataArray badges;
    TextAsset file = Resources.Load (filePath) as TextAsset;
    badges = JsonUtility.FromJson<BadgeDataArray> (file.text);
    return badges;
  }

  public GameMenuButtonDataArray GetMenuButtonDataArray (string filePath)
  {
    GameMenuButtonDataArray buttons;
    TextAsset file = Resources.Load (filePath) as TextAsset;
    buttons = JsonUtility.FromJson<GameMenuButtonDataArray> (file.text);
    return buttons;
  }

  public HowToCommandDescriptionDataArray GetCommandDescriptionDataArray (string filePath)
  {
    HowToCommandDescriptionDataArray descriptions;
    TextAsset file = Resources.Load (filePath) as TextAsset;
    descriptions = JsonUtility.FromJson<HowToCommandDescriptionDataArray> (file.text);
    return descriptions;
  }

  public StrategyDataArray GetStrategyDataArray (string filePath)
  {
    StrategyDataArray strategy;
    TextAsset file = Resources.Load (filePath) as TextAsset;
    strategy = JsonUtility.FromJson<StrategyDataArray> (file.text);
    return strategy;
  }

  public LevelUpOptionDataArray GetLevelUpOptionDataArray ()
  {
    LevelUpOptionDataArray levelUpOptionDataArray;
    string filePath = "JSON/GameLevelUpOptionsData";
    TextAsset file = Resources.Load (filePath) as TextAsset;
    levelUpOptionDataArray = JsonUtility.FromJson<LevelUpOptionDataArray> (file.text);
    return levelUpOptionDataArray;
  }
}