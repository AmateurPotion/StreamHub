using UnityEngine;
using UnityEngine.U2D.Animation;

namespace StreamHub.Util
{
  public class LibraryAnimator : MonoBehaviour
  {
    [SerializeField] private SpriteResolver resolver;

    public string SpriteLabel
    {
      get => resolver.GetLabel();
      set => resolver.SetCategoryAndLabel(resolver.GetCategory(), value);
    }
  }
}