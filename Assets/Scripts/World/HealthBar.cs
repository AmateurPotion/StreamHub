using System;
using System.Collections.Generic;
using StreamHub.Util;
using UnityEngine;
using UnityEngine.UI;

namespace StreamHub.World
{
  public class HealthBar : MonoBehaviour
  {
    [SerializeField] private Sprite full, half, empty;
    [SerializeField] [GetSet("MaxHp")] private int maxHp;
    [SerializeField] [GetSet("Hp")] private int hp;
    [SerializeField] private List<Image> sprites = new();

    public int Hp
    {
      get => hp;
      set
      {
        hp = value < 0 ? 0 : value > maxHp ? maxHp : value;

        for (var i = 0; i < sprites.Count; i++)
          if (i * 2 + 1 == hp) sprites[i].sprite = half;
          else if (i * 2 < hp) sprites[i].sprite = full;
          else sprites[i].sprite = empty;
      }
    }

    public int MaxHp
    {
      get => maxHp;
      set
      {
        for (var i = 0; i < Math.Ceiling((float)Math.Abs(value - maxHp) / 2); i++)
          if (value > maxHp)
            CreateHeart();
          else
            RemoveHeart();

        maxHp = value < 0 ? 0 : value;
        Hp = hp;
      }
    }

    private void CreateHeart()
    {
      var obj = new GameObject("Heart");
      obj.transform.SetParent(transform);
      obj.transform.localScale = Vector3.one;
      obj.transform.localPosition = Vector3.zero;
      var image = obj.AddComponent<Image>();
      image.sprite = empty;

      sprites.Add(image);
    }

    private void RemoveHeart()
    {
      if (sprites.Count < 1) return;

      var obj = sprites[^1];
      sprites.Remove(obj);
      Destroy(obj.gameObject);
    }
  }
}