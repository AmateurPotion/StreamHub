using UnityEngine;
using UnityEngine.Pool;

namespace StreamHub.Scenes.PersonalWorld.Stack
{
  public class BrickLine : MonoBehaviour
  {
    private static ObjectPool<Brick> Pool => StackManager.Instance.brickPool;
    public int index;
    public Brick[] bricks;
    [SerializeField] private RectTransform rect;
    private int brickCount;

    public void Generate(int count)
    {
      brickCount = count;
      rect.sizeDelta = new Vector2(count, 1);
      bricks = new Brick[count];

      for (var i = 0; i < count; i++)
      {
        var brick = Pool.Get();
        brick.gameObject.name = $"{index}_{count}_{i}";
        brick.transform.SetParent(transform);
        brick.transform.rotation = Quaternion.identity;
        brick.index = index;
        brick.BodyActive = false;

        bricks[i] = brick;
      }
    }

    private static StackManager manager => StackManager.Instance;

    private void FixedUpdate()
    {
      if (index != -1)
      {
        if (brickCount > 0)
        {
          if (manager.left && transform.position.x > manager.rightPos)
          {
            // 왼쪽에서 시작해서 끝에 도달했을 때 블럭 하나 줄이고 다시 왼쪽으로
            var brick = bricks[brickCount - 1];
            Pool.Release(brick);
            brickCount--;
            rect.sizeDelta -= new Vector2(1, 0);
            transform.position = new Vector3(manager.leftPos, transform.position.y, transform.position.z);
          }
          else if (!manager.left && transform.position.x < manager.leftPos)
          {
            // 오른쪽에서 시작해서 끝에 도달했을 때 블럭 하나 줄이고 다시 오른쪽으로
            var brick = bricks[brickCount - 1];
            Pool.Release(brick);
            brickCount--;
            rect.sizeDelta -= new Vector2(1, 0);
            transform.position = new Vector3(manager.rightPos, transform.position.y, transform.position.z);
          }
          else
          {
            transform.position += (5 + manager.lineIndex * 0.5f) * Time.deltaTime *
                                  (StackManager.Instance.left ? Vector3.right : Vector3.left);
          }
        }
        else
        {
          manager.GameOver();
        }
      }
    }

    public void Deconstruct()
    {
      foreach (var brick in bricks)
      {
        if (brick == null) continue;
        brick.transform.SetParent(transform.parent);
        brick.BodyActive = true;
      }

      Destroy(gameObject);
    }
  }
}