using Cinemachine;
using StreamHub.Prefabs.Character;
using StreamHub.Scenes.PersonalWorld;
using StreamHub.Scenes.PersonalWorld.Interactable;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StreamHub.Scenes.World
{
  public class Player : MonoBehaviour
  {
    public Character character;
    public Vector2 direction;
    public float additionalSpeed = 0;

    public float CameraSize
    {
      get => camera.m_Lens.OrthographicSize;
      set => camera.m_Lens.OrthographicSize = value;
    }

    [SerializeField] private InteractableObject currentTarget;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private new CinemachineVirtualCamera camera;
    [SerializeField] private TMP_Text descriptionText;

    public float Speed
    {
      get => character.speed + additionalSpeed;
      set => additionalSpeed = value - character.speed;
    }

    public InteractableObject CurrentTarget
    {
      get => currentTarget;
      set
      {
        if (currentTarget != null) currentTarget.Highlight = false;

        currentTarget = value;
      }
    }

    private void Update()
    {
      if (Input.GetAxis("Mouse ScrollWheel") > 0 && CameraSize < 10) 
        CameraSize = Mathf.Lerp(CameraSize, 10, Time.deltaTime * 5);
      if (Input.GetAxis("Mouse ScrollWheel") < 0 && CameraSize > 2)
        CameraSize = Mathf.Lerp(CameraSize, 2, Time.deltaTime * 5);
    }

    private void FixedUpdate()
    {
      var floorMap = WorldManager.Instance.map.Floors;
      
      bool canMoveX = floorMap.GetTile(floorMap.WorldToCell(transform.position + new Vector3(direction.x, 0))) != null,
        canMoveY = floorMap.GetTile(floorMap.WorldToCell(transform.position + new Vector3(0, direction.y))) != null;
      
      body.AddForce(new Vector2(canMoveX ? direction.x : 0, 
          canMoveY ? direction.y : 0) * Speed,
        ForceMode2D.Impulse);
      
      if(!canMoveX) body.velocity = new Vector2(0, body.velocity.y);
      if(!canMoveY) body.velocity = new Vector2(body.velocity.x, 0);
    }

    private void OnMove(InputValue value)
    {
       direction = value.Get<Vector2>();
    }
  }
}
