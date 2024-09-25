using Godot;
using System;

public partial class Mob : RigidBody2D
{
	public override void _Ready(){
		
		//initiate animation
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		//create animation array for variety
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		
		//choose which animation to play
		animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}
	
	//despawns mob when screen exit
	private void OnVisibleOnScreenNotifier2DScreenExcited(){
		QueueFree();
	}
}
