using UnityEngine;
using UnityEngine.Pool;

namespace StreamHub.Scenes.PersonalWorld.Stack
{
  public class Brick : MonoBehaviour
  {
    private static float DestroyHeight => StackManager.Instance.destroyHeight;
    private static ObjectPool<Brick> Pool => StackManager.Instance.brickPool;

    public int index;
    public bool positionFixed;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private new BoxCollider2D collider;
    [SerializeField] private SpriteRenderer sprite;

    private void OnEnable()
    {
      sprite.color = new Color(1, 1, 1);
    }
    
    private void Update()
    {
      if (gameObject.activeSelf &&
          transform.position.y < DestroyHeight &&
          index != StackManager.Instance.lineIndex)
      {
        if (!positionFixed) StackManager.Instance.Score -= 2;
        Pool.Release(this);
      }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      if (other.gameObject.CompareTag("Interactable") && other.gameObject.TryGetComponent<Brick>(out var brick))
      {
        if (!positionFixed && brick.index == index - 1)
        {
          StackManager.Instance.FixedBrickCount++;
          sprite.color = new Color(120 / 255f, 1, 1);
          StackManager.Instance.Score++;
          positionFixed = true;
          body.constraints = RigidbodyConstraints2D.FreezeAll;
        }
      }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
      if (positionFixed) return;

      if (other.gameObject.CompareTag("GameController") && other.gameObject.TryGetComponent<BrickLine>(out var line))
        if (index == 0)
        {
          sprite.color = new Color(120 / 255f, 1, 1);
          StackManager.Instance.Score++;
          StackManager.Instance.FixedBrickCount++;
          positionFixed = true;
          body.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public bool BodyActive
    {
      get => body != null;
      set
      {
        switch (value)
        {
          case true when body == null:
            body = gameObject.AddComponent<Rigidbody2D>();
            collider.usedByComposite = false;
            body.constraints = RigidbodyConstraints2D.None;
            sprite.color = new Color(1, 160 / 255f, 1);
            break;

          case false when body != null:
            collider.usedByComposite = true;
            Destroy(body);
            body = null;
            break;
        }
      }
    }
  }
}