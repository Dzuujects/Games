using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Signal]
	public delegate void HitEventHandler();
	
	//How fast the player moves in ms-1
	[Export]
	public int Speed {get; set;} = 14;
	
	//How fast the player falls in ms-1
	[Export]
	public int FallAcceleration {get; set;} = 75;
	
	[Export]
	public int JumpImpulse {get; set;} = 20;
	
	[Export]
	public int BounceImpulse{get; set;} = 16;
	
	private Vector3 _targetVelocity = Vector3.Zero;
	
	public override void _PhysicsProcess(double delta){
		//Local veriable to set direction
		var direction = Vector3.Zero;
		
		//check for input and move player accordingly
		if (Input.IsActionPressed("MoveLeft")){
			direction.X -= 1.0f;
		}
		if (Input.IsActionPressed("MoveRight")){
			direction.X += 1.0f;
		}
		if (Input.IsActionPressed("MoveBackwards")){
			direction.Z += 1.0f;
		}
		if (Input.IsActionPressed("MoveForward")){
			direction.Z -= 1.0f;
		}
		
		if (IsOnFloor() && Input.IsActionPressed("Jump")){
			_targetVelocity.Y = JumpImpulse;
		}
		
		//when rotation of player to the direction they are moving in
		if (direction != Vector3.Zero){
			direction = direction.Normalized();
			GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
			GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = 4;
		}
		else {
			GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = 1;
		}
		
		//ground velocity
		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;
		
		//vertical velocity and applies gravity
		if (!IsOnFloor()){
			_targetVelocity.Y  -= FallAcceleration * (float) delta;
		}
		
		for (int i = 0;i < GetSlideCollisionCount(); i++ ){
			KinematicCollision3D collision = GetSlideCollision(i);
			
			if (collision.GetCollider() is Mob mob){
				mob.Squash();
				_targetVelocity.Y = BounceImpulse;
				break;
			}
		}
		
		//moving the character
		Velocity = _targetVelocity;
		MoveAndSlide();
	}
		
	private void Die(){
		EmitSignal(SignalName.Hit);
		QueueFree();
	}
	
	private void OnMobDetectorBodyEntered(Node3D body){
		Die();
	}
}
