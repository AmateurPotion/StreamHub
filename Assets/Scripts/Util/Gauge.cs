using System;
using UnityEngine;

namespace StreamHub.Util
{
  public class Gauge : MonoBehaviour
  {
    public float max;
    [SerializeField, GetSet("Value")]
    private float value;

    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private SpriteRenderer sprite;

    public float Value
    {
      get => value;
      set
      {
        this.value = value < 0 ? 0 : Math.Min(value, max);
        rect.localScale = new(this.value / max, 1, 1);
      }
    }

#if UNITY_EDITOR
    [SerializeField, GetSet("color")]
    private Color _color;
#endif
    public Color color
    {
      get => sprite.color;
      set
      {
        sprite.color = value;
      }
    }

    public void ChangeMax(float value)
    {
      max = value;
      Value = Value;
    }
  }
}
