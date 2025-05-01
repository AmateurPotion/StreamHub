using System.Collections.Generic;
using StreamHub.Prefabs.Character;
using StreamHub.Prefabs.Interactable;
using StreamHub.Scenes.PersonalWorld;
using StreamHub.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StreamHub.Scenes.World
{
  public class Player : MonoBehaviour
  {
    public Character character;
    public Vector2 direction;
    public float additionalSpeed = 0;
    [GetSet("Hp")] private int hp = 6;
    public int maxHp = 6;

    public float CameraSize
    {
      get => camera.orthographicSize;
      set => camera.orthographicSize = value;
    }

    [SerializeField] private List<InteractableObject> focusedObjects = new();
    [SerializeField] private Rigidbody2D body;

    [SerializeField] [Header("Camera")] private new Camera camera;

    public int Hp
    {
      get => hp;
      set => hp = value < 0 ? 0 : value > maxHp ? maxHp : value;
    }

    public float Speed
    {
      get => character.speed + additionalSpeed;
      set => additionalSpeed = value - character.speed;
    }

    public InteractableObject Focused => focusedObjects.Count > 0 ? focusedObjects[0] : null;
    [SerializeField] private CompositeCollider2D collider;

    private void Awake()
    {
      collider.GenerateGeometry();
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

      // Camera Tracking
      camera.transform.position = Vector3.Lerp(camera.transform.position,
        new Vector3(transform.position.x, transform.position.y, camera.transform.position.z), Time.deltaTime * 5);
    }

    private void OnMove(InputValue value)
    {
       direction = value.Get<Vector2>();
    }

    private void OnInteract()
    {
      if (Focused != null) Focused.Interact(this);
    }

    public void AddFocus(InteractableObject interactableObject)
    {
      var previousTarget = Focused;
      if (previousTarget != null) previousTarget.Highlight = false;

      WorldManager.Instance.panel.OpenInteractionPanel(transform.position, interactableObject.Title,
        interactableObject.Description);
      interactableObject.Highlight = true;
      focusedObjects.Add(interactableObject);
    }

    public void RemoveFocus(InteractableObject interactableObject)
    {
      var previousTarget = Focused;
      interactableObject.Highlight = false;
      focusedObjects.Remove(interactableObject);

      if (Focused != null && previousTarget != interactableObject)
      {
        WorldManager.Instance.panel.OpenInteractionPanel(transform.position, Focused.Title,
          Focused.Description);
        Focused.Highlight = true;
      }
      else if (Focused == null)
      {
        WorldManager.Instance.panel.CloseInteractionPanel();
      }
    }
  }
}
