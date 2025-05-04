using UnityEngine;
using UnityEngine.Pool;

namespace StreamHub.Scenes.PersonalWorld.Stack
{
  public class BrickLine : MonoBehaviour
  {
    private static ObjectPool<Brick> Pool => StackManager.Instance.brickPool;
    public int index;
    public Brick[] bricks;
    private RectTransform rect => transform as RectTransform;

    public void Generate(int count)
    {
      rect.sizeDelta = new Vector2(count, 1);
      bricks = new Brick[count];

      for (var i = 0; i < count; i++)
      {
        var brick = bricks[i] = Pool.Get();
        brick.transform.SetParent(transform);
        brick.index = index;
        brick.BodyActive = false;
      }
    }

    public void Deconstruct()
    {
      foreach (var brick in bricks)
      {
        brick.transform.SetParent(transform.parent);
        brick.BodyActive = true;
      }

      Destroy(gameObject);
    }
  }
}