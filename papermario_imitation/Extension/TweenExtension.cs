// 参考サイト
// https://baba-s.hatenablog.com/entry/2018/05/08/085900

// using System.Collections;
// using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
// using UnityEngine;

// Task なぜこれで拡張が出来ているのかを知る
public static class TweenExtension
{
  public static TaskAwaiter<Tween> GetAwaiter (this Tween self)
  {
    var source = new TaskCompletionSource<Tween> ();
    TweenCallback onComplete = null;
    onComplete = () =>
    {
      self.onComplete -= onComplete;
      source.SetResult (self);
    };
    self.onComplete += onComplete;
    return source.Task.GetAwaiter ();
  }
}