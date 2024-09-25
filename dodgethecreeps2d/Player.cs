using Godot;
using System;

public partial class Player : Area2D
{
	public int Speed {get; set;} = 400; //set speed of Player
	public Vector2 ScreenSize; //instaniate variable
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		ScreenSize = GetViewportRect().Size; //
		Hide();	
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		
		//set movement for player
		var velocity = Vector2.Zero;
		if (Input.IsActionPressed("MoveRight")){
			velocity.X += 1;
		}
		if (Input.IsActionPressed("MoveLeft")){
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("MoveUp")){
			velocity.Y -= 1;
		}
		if (Input.IsActionPressed("MoveDown")){
			velocity.Y += 1;
		}
		
		//let animation play or stop depending on velocity of player
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		if (velocity.Length() > 0){
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else{
			animatedSprite2D.Stop();
		}
		
		//set bounderies on screen so player does not move on screen
		Position += velocity * (float) delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
		
		//chooses animation depending which direction they are moving
		if (velocity.X != 0){
			animatedSprite2D.Animation = "walk";
			animatedSprite2D.FlipV = false;
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0){
			animatedSprite2D.Animation = "up";
			animatedSprite2D.FlipH = false;
			animatedSprite2D.FlipV = velocity.Y > 0;
		}
	}
	
	//create signal 'hit' for collision
	[Signal]
	public delegate void HitEventHandler();
	
	//connect OnBodyEntered node for collision
	private void OnBodyEntered(Node2D body) {
	Hide();
	EmitSignal(SignalName.Hit);
	GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
	
	//call node when game starts
	public void Start (Vector2 position){
	Position = position;
	Show();
	GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}
