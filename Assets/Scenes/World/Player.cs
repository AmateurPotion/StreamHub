using System.Collections.Generic;
using StreamHub.Prefabs.Character;
using StreamHub.Prefabs.Interactable;
using StreamHub.Scenes.PersonalWorld;
using StreamHub.World;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StreamHub.Scenes.World
{
  public class Player : Entity
  {
    private static readonly int Run = Animator.StringToHash("Run");
    
    public Character character;
    public Vector2 direction;

    public float CameraSize
    {
      get => camera.orthographicSize;
      set => camera.orthographicSize = value;
    }

    [SerializeField] private List<InteractableObject> focusedObjects = new();
    [SerializeField] private Rigidbody2D body;

    [SerializeField] private new Camera camera;

    public float Speed
    {
      get => character.speed + additionalSpeed;
      set => additionalSpeed = value - character.speed;
    }

    public InteractableObject Focused => focusedObjects.Count > 0 ? focusedObjects[0] : null;
    [SerializeField] private new CompositeCollider2D collider;

    private void Awake()
    {
      collider.GenerateGeometry();
    }

    protected override void Update()
    {
      base.Update();
      
      if (Input.GetAxis("Mouse ScrollWheel") > 0 && CameraSize < 10) 
        CameraSize = Mathf.Lerp(CameraSize, 10, Time.deltaTime * 5);
      if (Input.GetAxis("Mouse ScrollWheel") < 0 && CameraSize > 2)
        CameraSize = Mathf.Lerp(CameraSize, 2, Time.deltaTime * 5);
    }

    private void FixedUpdate()
    {
      body.AddForce(direction * (Speed - slowdown),
        ForceMode2D.Impulse);

      // Set Character-look Direction
      character.spriteRenderer.flipX =
        direction.magnitude > 0 ? direction.x < 0 : Input.mousePosition.x < (float)Screen.width / 2;

      // Camera Tracking
      camera.transform.position = Vector3.Lerp(camera.transform.position,
        new Vector3(transform.position.x, transform.position.y, camera.transform.position.z), Time.deltaTime * 5);
    }

    private void OnMove(InputValue value)
    {
      direction = value.Get<Vector2>();

      character.animator.SetBool(Run, direction.magnitude > 0);
    }

    private void OnInteract()
    {
      if (Focused != null) Focused.Interact(this);
    }

    public void AddFocus(InteractableObject interactableObject)
    {
      var previousTarget = Focused;
      if (previousTarget != null) previousTarget.Highlight = false;

      if (interactableObject.descriptable)
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
        if (interactableObject.descriptable)
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
