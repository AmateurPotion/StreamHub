using System;
using System.Collections.Generic;
using StreamHub.Prefabs.Character;
using StreamHub.Prefabs.Interactable;
using StreamHub.World;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StreamHub.Scenes.PersonalWorld
{
  public class Player : Entity
  {
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Hit = Animator.StringToHash("Hit");

    public GravityMode mode = GravityMode.TopView;
    public Character character;
    public Vector2 direction;
    public HealthBar healthBar;
    public float jumpForce = 100;
    public new Camera camera;
    [SerializeField] private List<InteractableObject> focusedObjects = new();
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Vector3 cameraFocus;

    public float CameraSize
    {
      get => camera.orthographicSize;
      set => camera.orthographicSize = value;
    }

    public override int Hp
    {
      get => hp;
      set
      {
        hp = value < 0 ? 0 : value > maxHp ? maxHp : value;
        healthBar.Hp = value;
      }
    }

    public override int MaxHp
    {
      get => maxHp;
      set
      {
        maxHp = value < 0 ? 0 : value;
        healthBar.MaxHp = value;
      }
    }

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
      MaxHp = 6;
      Hp = 6;
      cameraFocus = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);;
    }

    protected override void Update()
    {
      base.Update();
      
      if (Input.GetAxis("Mouse ScrollWheel") < 0 && CameraSize < 10) 
        CameraSize = Mathf.Lerp(CameraSize, 10, Time.deltaTime * 5);
      if (Input.GetAxis("Mouse ScrollWheel") > 0 && CameraSize > 2)
        CameraSize = Mathf.Lerp(CameraSize, 2, Time.deltaTime * 5);
    }

    private void FixedUpdate()
    {
      var moveDirection = mode == GravityMode.TopView ? direction : new Vector2(direction.x, 0);
      body.AddForce(moveDirection * (Speed - slowdown),
        ForceMode2D.Force);

      // Set Character-look Direction
      character.spriteRenderer.flipX =
        direction.magnitude > 0 ? direction.x < 0 : Input.mousePosition.x < (float)Screen.width / 2;

      // Camera Tracking
      cameraFocus = Vector3.Lerp(cameraFocus,
        new Vector3(transform.position.x, transform.position.y, cameraFocus.z), Time.deltaTime * 5);
      
      float dx = cameraFocus.x, dy = cameraFocus.y;

      dx = CanCameraMove(new Vector2(cameraFocus.x, 0)) ? dx : camera.transform.position.x;
      dy = CanCameraMove(new Vector2(0, cameraFocus.y)) ? dy : camera.transform.position.y;

      camera.transform.position = new Vector3(dx, dy, cameraFocus.z);
    }

    private static readonly string[] WallLayer = {"Walls"};

    private bool CanCameraMove(Vector2 target)
    {
      Vector3 dest1 = new(), dest2 = new(),
        dir1 = new(), dir2 = new();

      if (target.x == 0)
      {
        dir1 = new Vector3(0,1);
        dir2 = new Vector3(0,-1);
      }

      if (target.y == 0)
      {
        dir1 = new Vector3(1,0);
        dir2 = new Vector3(-1,0);
      }

      dest1 = camera.ViewportToWorldPoint(new Vector3(
        dir1.x switch
        {
          0 => 0.5f,
          -1 => 0,
          1 => 1,
          _ => 0.5f
        },
        dir1.y switch
        {
          0 => 0.5f,
          -1 => 0,
          1 => 1,
          _ => 0.5f
        }
        ));

      dest2 = camera.ViewportToWorldPoint(new Vector3(
        dir2.x switch
        {
          0 => 0.5f,
          -1 => 0,
           1 => 1,
          _ => 0.5f
        },
        dir2.y switch
        {
          0 => 0.5f,
          -1 => 0,
           1 => 1,
          _ => 0.5f
        }
        ));

      RaycastHit2D hit1 = Physics2D.Raycast(cameraFocus, dir1, Vector3.Distance(camera.transform.position, dest1),
        LayerMask.GetMask(WallLayer)),
        hit2 = Physics2D.Raycast(cameraFocus, dir2, Vector3.Distance(camera.transform.position, dest2),
          LayerMask.GetMask(WallLayer)) ;
      
      return !hit1.collider && !hit2.collider;
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

    public override bool TakeDamage(int damage)
    {
      var damaged = base.TakeDamage(damage);

      if (damaged)
        character.animator.SetTrigger(Hit);

      return damaged;
    }

    private void OnJump()
    {
      if (mode == GravityMode.Platform && Math.Abs(body.velocity.y) < 0.1f)
        body.AddForce(Vector2.up * jumpForce,
          ForceMode2D.Impulse);
    }

    private void OnFire()
    {
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

    public void ChangeMode()
    {
      mode = mode == GravityMode.TopView ? GravityMode.Platform : GravityMode.TopView;

      body.gravityScale = mode == GravityMode.Platform ? 10 : 0;
    }
  }
}
