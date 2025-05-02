using System.Collections.Generic;
using StreamHub.Util;
using UnityEngine;

namespace StreamHub.World
{
  public class Entity : MonoBehaviour
  {
    public float additionalSpeed;
    [SerializeField] [GetSet("Hp")] protected int hp = 6;
    [SerializeField] [GetSet("MaxHp")] protected int maxHp = 6;
    public float slowdown;
    public float immuneTime = 1;
    [SerializeField] private float immuneTimer;
    [SerializeField] private int toxicity;
    [SerializeField] private List<TileInfo> visitedTiles = new();

    public virtual int Hp
    {
      get => hp;
      set => hp = value < 0 ? 0 : value > maxHp ? maxHp : value;
    }

    public virtual int MaxHp
    {
      get => maxHp;
      set => maxHp = value < 0 ? 0 : value;
    }

    public virtual bool TakeDamage(int damage)
    {
      if (immuneTimer > 0) return false;

      Hp -= damage;
      immuneTimer = immuneTime;

      return true;
    }

    protected virtual void Update()
    {
      if (immuneTimer > 0)
        immuneTimer -= Time.deltaTime;

      if (toxicity > 0)
        TakeDamage(toxicity);
    }

    public void AddVisitTile(TileInfo tileInfo)
    {
      visitedTiles.Add(tileInfo);

      foreach (var info in visitedTiles)
      {
        slowdown = slowdown < info.slowdown ? info.slowdown : slowdown;
        toxicity = toxicity < info.toxicity ? info.toxicity : toxicity;
      }
    }

    public void RemoveVisitTile(TileInfo tileInfo)
    {
      visitedTiles.Remove(tileInfo);

      if (visitedTiles.Count > 0)
      {
        foreach (var info in visitedTiles)
        {
          slowdown = slowdown < info.slowdown ? info.slowdown : slowdown;
          toxicity = toxicity < info.toxicity ? info.toxicity : toxicity;
        }
      }
      else
      {
        toxicity = 0;
        slowdown = 0;
      }
    }
  }
}