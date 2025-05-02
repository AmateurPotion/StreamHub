using System;
using UnityEngine;

namespace StreamHub.Util
{
  [Serializable]
  public struct Pair<T1, T2>
  {
    [SerializeField]
    public T1 a;
    [SerializeField]
    public T2 b;

    public readonly void Deconstruct(out T1 a, out T2 b)
    {
      a = this.a;
      b = this.b;
    }

    public static implicit operator T1(Pair<T1, T2> origin) => origin.a;
    public static implicit operator T2(Pair<T1, T2> origin) => origin.b;
  }
}