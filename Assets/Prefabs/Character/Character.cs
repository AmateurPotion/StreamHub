using System;
using UnityEngine;

namespace StreamHub.Prefabs.Character
{
  public class Character : MonoBehaviour
  {
    /// <summary>
    /// 
    /// </summary>
    public Animator animator;
    public float speed = 5;
    public Vector2 direction;
    
    [SerializeField]
    private Rigidbody2D body;

    private void FixedUpdate()
    {
      body.velocity = direction * (speed * Time.deltaTime);
    }
  }
}