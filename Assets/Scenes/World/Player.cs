using StreamHub.Prefabs.Character;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StreamHub.Scenes.World
{
  public class Player : MonoBehaviour
  {
    public Character character;

    public Vector2 Direction
    {
      get => character.direction;
      set => character.direction = value;
    }

    public float Speed
    {
      get => character.speed;
      set => character.speed = value;
    }

    private void OnMove(InputValue value)
    {
      Direction = value.Get<Vector2>();
    }
  }
}
