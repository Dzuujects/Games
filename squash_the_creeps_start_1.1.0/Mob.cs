using Godot;
using System;

public partial class Mob : CharacterBody3D
{
	[Signal]
	public delegate void SquashedEventHandler();
		
	[Export]
	public int minSpeed {get; set;} = 10;
	
	[Export]
	public int maxSpeed {get; set;} = 18;
	
	public override void _PhysicsProcess(double delta){
		MoveAndSlide();
	}
	
	//function will be called to main scene
	public void Initialize(Vector3 startPosition, Vector3 playerPosition){
		//position and rotate mob depending on startPosition and playePosition respectively
		LookAtFromPosition(startPosition, playerPosition, Vector3.Up);
		
		//adds a bit of random rotation to the mob spawn direction
		RotateY((float)GD.RandRange(-Mathf.Pi/4.0, Mathf.Pi/4.0));
		
		int randomSpeed = GD.RandRange(minSpeed, maxSpeed);
		
		Velocity = Vector3.Forward * randomSpeed;
		
		Velocity = Velocity.Rotated(Vector3.Up, Rotation.Y);
	}
	public void Squash(){
		EmitSignal(SignalName.Squashed);
		QueueFree();
	}
	
	private void OnVisibilityNotifierScreenExited(){
		QueueFree();
	}
}
