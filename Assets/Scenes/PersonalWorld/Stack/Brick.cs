using System;
using UnityEngine;
using UnityEngine.Pool;

namespace StreamHub.Scenes.PersonalWorld.Stack
{
  public class Brick : MonoBehaviour
  {
    private static float DestroyHeight => StackManager.Instance.destroyHeight;
    private static ObjectPool<Brick> Pool => StackManager.Instance.brickPool;

    public int index;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private new BoxCollider2D collider;

    private void Update()
    {
      if (gameObject.activeSelf && transform.position.y < DestroyHeight)
        Pool.Release(this);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      if (other.gameObject.CompareTag("Interactable"))
      {
        
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
            body.constraints = RigidbodyConstraints2D.FreezePositionX & RigidbodyConstraints2D.FreezeRotation;
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