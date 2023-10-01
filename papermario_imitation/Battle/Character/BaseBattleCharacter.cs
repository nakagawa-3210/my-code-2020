using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBattleCharacter : MonoBehaviour
{
  // 最低限の機能から作る
  public enum BattleState
  {
    idle,
    jump,
    hammer,
    normalAttack,
    tackle,
    escape
    // SpecialAttack,
    // Healing,
    // UseHPRecoveryItem,
    // UseFRecoveryItem,
    // UseAbnormalConditionRecoveryItem,
    // IncreaseAttackPowerSkill,
    // IncreaseDefencePowerSkill,
    // DecreaseAttackPowerSkill,
    // DecreaseDefencePowerSkill,
    // AbnormalConditionRecoverySkill,
    // Damage,
    // Guard,
    // Appeal,
    // Dead,
  }

  private int hp = 0;
  private int maxHp = 0;
  private int fp = 0;
  private int maxFp = 0;
  private int at = 0;
  private int defaultAt = 0;
  private int df = 0;
  private int defaultDf = 0;
  private bool isParalysed = false;
  private bool isPoisoned = false;
  private bool isSleeping = false;
  public int Hp
  {
    set { hp = value; }
    get { return hp; }
  }
  public int MaxHp
  {
    set { maxHp = value; }
    get { return maxHp; }
  }
  public int Fp
  {
    set { fp = value; }
    get { return fp; }
  }
  public int MaxFp
  {
    set { maxFp = value; }
    get { return maxFp; }
  }
  public int At
  {
    set { at = value; }
    get { return at; }
  }
  public int DefaultAt
  {
    set { defaultAt = value; }
    get { return defaultAt; }
  }
  public int Df
  {
    set { df = value; }
    get { return df; }
  }
  public int DefaultDf
  {
    set { defaultDf = value; }
    get { return defaultDf; }
  }
  public bool IsParalysed
  {
    set { isParalysed = value; }
    get { return isParalysed; }
  }
  public bool IsPoisoned
  {
    set { isPoisoned = value; }
    get { return isPoisoned; }
  }
  public bool IsSleeping
  {
    set { isSleeping = value; }
    get { return isSleeping; }
  }

  public bool IsDead ()
  {
    return hp <= 0;
  }

  void Start ()
  {

  }

  void Update ()
  {

  }
}