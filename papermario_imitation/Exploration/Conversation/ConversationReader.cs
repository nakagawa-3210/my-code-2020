using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// MethodInfoクラスの使用に必要
using System.Linq;
using System.Reflection;

public class ConversationReader
{
  private ConversationContentsManager conversationContentsManager;
  private BaseActions baseActions;
  private HanaPlayerActions hanaPlayerActions;
  private ItemShopBuyingActions itemShopBuyingActions;
  private ItemShopSellingActions itemShopSellingActions;
  private ItemShopLeavingActions itemShopLeavingActions;
  private ItemShopGettingActions itemShopGettingActions;
  private ItemShopConversationActions itemShopConversationActions;
  public ConversationReader (ConversationContentsManager conversationContentsManager)
  {
    this.conversationContentsManager = conversationContentsManager;
    baseActions = conversationContentsManager.baseActions;
    hanaPlayerActions = conversationContentsManager.hanaPlayerActions;
    itemShopBuyingActions = conversationContentsManager.itemShopBuyingActions;
    itemShopSellingActions = conversationContentsManager.itemShopSellingActions;
    itemShopLeavingActions = conversationContentsManager.itemShopLeavingActions;
    itemShopGettingActions = conversationContentsManager.itemShopGettingActions;
    itemShopConversationActions = conversationContentsManager.itemShopConversationActions;
  }

  public void ReadLines (
    ConversationScene cs,
    string itemName = null,
    string itemPrice = null,
    string canLeavingItemNum = null
  )
  {
    if (cs.Index >= cs.Lines.Count) return;
    string line = cs.GetCurrentLines ();
    // Debug.Log ("line : " + line);
    string text = "";
    // 会話内容ではない、または引数の値に変更される情報を持つ文字列の場合
    if (line.Contains ("#"))
    {
      // 仕分けが終わるまでループ
      while (true)
      {
        // 仕分け(タグの解析)済みの場合は抜ける
        if (!line.Contains ("#")) break;
        // 仕分けが終わっていな方場合はタグを削除
        line = line.Replace ("#", "");
        // タグの場合分け
        if (line.Contains ("speaker"))
        {
          line = line.Replace ("speaker=", "");
          // 会話にて発声中のキャラ名を表示
          // たぶん原作通りふきだしを用いるからいらないかも
          // conversationContentsManager.SetSpeaker()
        }
        else if (line.Contains ("chara"))
        {
          line = line.Replace ("chara=", "");
          // 追加するかわからない
          // conversationContentsManager.AddCharactor(line)
        }
        // キャラの画像を表示する場合
        // たぶん使わない
        else if (line.Contains ("image"))
        {
          line = line.Replace ("image_", "");
          string[] splitted = line.Split ('=');
          // int body = 0;
          // int face = 1;
          // 画像を表示
          // conversationContentsManager.SetImage(splitted[body], splitted[face]);
        }
        // 次の会話に進む場合
        else if (line.Contains ("next"))
        {
          line = line.Replace ("next=", "");
          conversationContentsManager.SetScene (line);
        }
        // アクションクラスにて設定した関数を使用する場合
        else if (line.Contains ("baseMethod"))
        {
          InvokeMethod ("baseMethod", line, baseActions);
        }
        else if (line.Contains ("hanaPlayerMethod"))
        {
          InvokeMethod ("hanaPlayerMethod", line, hanaPlayerActions);
          // アニメ再生開始には一旦ループを抜ける
          cs.GoNextLine ();
          break;
        }
        else if (line.Contains ("itemShopBuyingMethod"))
        {
          InvokeMethod ("itemShopBuyingMethod", line, itemShopBuyingActions);
        }
        else if (line.Contains ("itemShopSellingMethod"))
        {
          InvokeMethod ("itemShopSellingMethod", line, itemShopSellingActions);
        }
        else if (line.Contains ("itemShopLeavingMethod"))
        {
          InvokeMethod ("itemShopLeavingMethod", line, itemShopLeavingActions);
        }
        else if (line.Contains ("itemShopGettingMethod"))
        {
          InvokeMethod ("itemShopGettingMethod", line, itemShopGettingActions);
        }
        else if (line.Contains ("shopConversationMethod"))
        {
          InvokeMethod ("shopConversationMethod", line, itemShopConversationActions);
        }
        // 会話選択肢のプレートを選択する
        else if (line.Contains ("optionFrame"))
        {
          line = line.Replace ("optionFrame=", "");
          conversationContentsManager.SetOptionPlate (line);
        }
        // 会話に選択肢が登場する場合
        else if (line.Contains ("options"))
        {
          var options = new List < (string, string) > ();
          while (true)
          {
            cs.GoNextLine ();
            line = cs.Lines[cs.Index];
            // 選択肢があるとき
            if (line.Contains ("{"))
            {
              line = line.Replace ("{", "").Replace ("}", "");
              var splitted = line.Split (',');
              options.Add ((splitted[0], splitted[1]));
            }
            else
            {
              // boolで検知し、optionのセットを行う
              // conversationContentsManager側でのbool値の変更に合わせてGameConversationManagerのupdate関数にて処理
              conversationContentsManager.Options = options;
              conversationContentsManager.CanSetOption = true;
              // もとはここから呼んでいたので一応コメントアウトでのこしている
              // conversationContentsManager.SetOptions (options);
              break;
            }
          }
        }
        cs.GoNextLine ();
        if (cs.IsFinished ()) break;
        line = cs.GetCurrentLines ();
      }
    }

    // 会話内容の情報をもつ文字列の場合
    if (line.Contains ('{'))
    {
      line = line.Replace ("{", "");
      while (true)
      {
        // アイテム購入追加内容 引数から置き換える情報を受け取る
        if (line.Contains ("itemName"))
        {
          line = line.Replace ("itemName", itemName);
        }
        if (line.Contains ("itemPrice"))
        {
          line = line.Replace ("itemPrice", itemPrice);
        }
        if (line.Contains ("canLeavingItemNum"))
        {
          line = line.Replace ("canLeavingItemNum", canLeavingItemNum);
        }
        if (line.Contains ('}'))
        {
          line = line.Replace ("}", "");
          text += line;
          cs.GoNextLine ();
          break;
        }
        else
        {
          text += line;
        }
        cs.GoNextLine ();
        if (cs.IsFinished ()) break;
        line = cs.GetCurrentLines ();
      }
      if (!string.IsNullOrEmpty (text)) conversationContentsManager.SetText (text);
    }
  }

  void InvokeMethod<T> (string methodName, string line, T actions)
  {
    line = line.Replace (methodName + "=", "");
    var type = actions.GetType ();
    MethodInfo mi = type.GetMethod (line);
    mi.Invoke (actions, new object[] { });
  }

}