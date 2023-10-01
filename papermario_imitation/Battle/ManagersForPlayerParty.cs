using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersForPlayerParty : MonoBehaviour
{
  // アクションコマンドによるダメージ量判定処理等管理
  public ActionCommandJudgeManager actionCommandJudgeManager;
  // ハンマーアクションのカウントダウン管理
  public ActionCommandCountdownManager actionCommandCountdownManager;
  // にげるアクションの連打ゲーム管理
  public ActionCommandEscapeManager actionCommandEscapeManager;
  public ActionCommandDictionaryManager actionCommandDictionaryManager;
  // アクションコマンドのUI管理
  public ActionUiMotionManager actionUiMotionManager;
  // アイテム攻撃のダメージ判定処理管理
  public ItemActionManager itemActionManager;
  // バトルカメラをまとめたクラス
  public BattleVirtualCameras battleVirtualCameras;
  // 敵の体力情報管理
  public BattleEnemyStatusManager battleEnemyStatusManager;
  // バトルの説明管理
  public BattleInformationManager battleInformationManager;
  // 経験値情報管理
  public BattleExperienceManager battleExperienceManager;
  // レベルアップ情報管理
  public BattleLevelUpManager battleLevelUpManager;
  // ものしり、敵との会話用
  public GameConversationManager gameConversationManager;
}