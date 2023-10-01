using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrategyOptionListContentsPreparer
{
  private GameObject strategyButton;
  private Transform strategyOptionListContainer;

  // アイテムと同様でリストの中身を用意しない仲間のためにdefault値をnullにした引数にしてある
  public StrategyOptionListContentsPreparer (
    GameObject strategyButton = null,
    Transform strategyOptionListContainer = null
  )
  {
    if (strategyButton != null && strategyOptionListContainer != null)
    {
      this.strategyButton = strategyButton;
      this.strategyOptionListContainer = strategyOptionListContainer;
      SetupStrategyButtons ();
    }
  }

  // 作戦ボタンの名前、説明、タイプ
  // アイテムと似た方法で作戦ボタンを作成する

  // JSONファイルにさくせんボタンのデータを作成する
  public void SetupStrategyButtons ()
  {
    StrategyDataArray strategyDataArray = new JsonReaderFromResourcesFolder ().GetStrategyDataArray ("JSON/GameStrategiesData");
    foreach (var strategy in strategyDataArray.gameStrategies)
    {
      GameObject newStrategyButton = GameObject.Instantiate<GameObject> (strategyButton);
      Sprite strategySprite = new GetGameSprite ().GetSameNameStrategySprite (strategy.imgFileName);

      string buttonImgGameObjectName = "StrategyImage";
      string buttonTextGameObjectName = "StrategyNameText";

      BaseMenuListPreparer baseMenuListPreparer = new BaseMenuListPreparer ();

      // 見た目の情報用意
      baseMenuListPreparer.SetupListButtonBaseInformation (
        newStrategyButton,
        strategyOptionListContainer,
        strategySprite,
        strategy.name,
        buttonImgGameObjectName,
        buttonTextGameObjectName
      );

      // 中身の情報用意
      StrategyButtonInformationContainer strategyButtonInformationContainer = newStrategyButton.GetComponent<StrategyButtonInformationContainer> ();
      strategyButtonInformationContainer.StrategyName = strategy.name;
      strategyButtonInformationContainer.Description = strategy.description;
      strategyButtonInformationContainer.Type = strategy.type;
      strategyButtonInformationContainer.Amount = strategy.amount;
    }
  }

  // バトル以外でも使いそうならMenuCommonFunctionsに移動する
  // 引数とらずに自分でリストを用意するように変更するかも
  public void ResetStrategyButtonIsSelected (List<GameObject> strategyButtonList)
  {
    foreach (var strategyButton in strategyButtonList)
    {
      StrategyButtonInformationContainer strategyButtonInformationContainer = strategyButton.GetComponent<StrategyButtonInformationContainer> ();
      strategyButtonInformationContainer.IsSelected = false;
    }
  }
}