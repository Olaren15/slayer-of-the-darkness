﻿using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlatformerController : PhysicsObject
{
	public float jumpTakeOffSpeed = 10;
	public float maxSpeed = 7;

	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private Controls controls;

	private static readonly int Grounded = Animator.StringToHash("grounded");
	private static readonly int VelocityX = Animator.StringToHash("velocityX");
	private static readonly int VelocityY = Animator.StringToHash("velocityY");

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		
		controls = new Controls();
		controls.Enable();
		controls.Player.JumpPress.performed += context => JumpPressed();
		controls.Player.JumpRelease.performed += context => JumpReleased();
	}

	private void JumpPressed() 
	{
		if (grounded)
		{
			velocity.y = jumpTakeOffSpeed;
		}
	}

	private void JumpReleased()
	{
		if (velocity.y > 0.0f)
		{
			velocity.y *= 0.5f;
		}
	}

	private Vector2 Move(float inputX)
	{
		bool flipSprite = spriteRenderer.flipX ? inputX > 0.0f : inputX < 0.0f;
		if (flipSprite)
		{
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}
		
		return new Vector2(inputX * maxSpeed, 0.0f);
	}

	protected override void ComputeVelocity()
	{
		animator.SetBool(Grounded, grounded);
		animator.SetFloat(VelocityX, Mathf.Abs(velocity.x) / maxSpeed);
		animator.SetFloat(VelocityY, velocity.y);

		targetVelocity = Move(controls.Player.Move.ReadValue<float>());
	}
}
