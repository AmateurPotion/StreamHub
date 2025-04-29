using UnityEngine;
using UnityEngine.InputSystem;

namespace StreamHub.Scenes.World
{
  public class Player : MonoBehaviour
  {
    public Vector2 direction;
    public float speed;
    
    [SerializeField]
    private Rigidbody2D body;

    private void FixedUpdate()
    {
      body.velocity = direction * speed;
    }

    private void OnMove(InputValue value)
    {
      direction = value.Get<Vector2>();
    }
  }
}
