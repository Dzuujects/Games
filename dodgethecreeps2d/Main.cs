using Godot;
using System;

public partial class Main : Node
{
	//allows to choose which mob scene to play
	[Export]
	public PackedScene MobScene {get; set;}
	
	private int _score;
	
	//connected hit to stop game
	public void GameOver(){
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<Hud>("HUD").ShowGameOver();
		GetNode<AudioStreamPlayer2D>("Music").Stop();
		GetNode<AudioStreamPlayer2D>("DeathSound").Play();
	}
	
	//starts a new game
	public void NewGame(){
		_score = 0;
		
		//instantiate a player place it the start position
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		
		//starts timer for Score and Mob
		GetNode<Timer>("StartTimer").Start();
		
		//instantiate hud for game to keep score
		var hud = GetNode<Hud>("HUD");
		hud.UpdateScore(_score);
		hud.ShowGetReady();
		
		//despawns creeps
		GetTree().CallGroup("mobs",Node.MethodName.QueueFree);
		
		GetNode<AudioStreamPlayer2D>("Music").Play();
	}
	
	//connected to score timer to display how long player survives
	private void OnScoreTimerTimeout(){
		_score++;
		GetNode<Hud>("HUD").UpdateScore(_score);
	}
	
	//connected to start timer to start both mob timer and score timer
	private void OnStartTimerTimeout(){
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}
	
	//connected to mob timer to create mobs
	private void OnMobTimerTimeout(){
		
		//create new mob scene
		Mob mob = MobScene.Instantiate<Mob>();
		
		//chooses location on path2D
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();
		
		//set direction perpendicular to path
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
		
		//set mob location to a random location 
		mob.Position = mobSpawnLocation.Position;
		
		//add randomness to direction of mob
		direction += (float)GD.RandRange(-Mathf.Pi/4, Mathf.Pi/4);
		mob.Rotation = direction;
		
		//set velocity of mob
		var velocity = new Vector2((float) GD.RandRange(150.0,250.0),0);
		mob.LinearVelocity = velocity.Rotated(direction);
		
		//spawns mob onto main scene
		AddChild(mob);
	}
	
	public override void _Ready(){
	}
}
