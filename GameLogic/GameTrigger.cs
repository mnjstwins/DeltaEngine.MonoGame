﻿using System;
using DeltaEngine.Commands;

namespace DeltaEngine.GameLogic
{
	/// <summary>
	/// Abstract class that defines the necessary methods for all GameTriggers 
	/// </summary>
	public abstract class GameTrigger : Trigger
	{
		protected GameTrigger()
		{
			GameStarting += StartingLevel;
			EnemyReachTheGoal += EnemyReachGoal;
			GameIsOver += GameOver;
			UpdateEverySecond += UpdateAfterOneSecond;
		}

		protected static event Action GameStarting;
		protected static event Action EnemyReachTheGoal;
		protected static event Action GameIsOver;
		protected static event Action UpdateEverySecond;

		protected virtual void StartingLevel() { }
		protected virtual void EnemyReachGoal() { }
		protected virtual void GameOver() { }
		protected virtual void UpdateAfterOneSecond() { }

		public static void OnGameStarting()
		{
			Action handler = GameStarting;
			if (handler != null)
				handler();
		}

		public static void OnEnemyReachGoal()
		{
			Action handler = EnemyReachTheGoal;
			if (handler != null)
				handler();
		}

		public static void OnGameOver()
		{
			Action handler = GameIsOver;
			if (handler != null)
				handler();
		}

		public static void OnUpdateEverySecond()
		{
			Action handler = UpdateEverySecond;
			if (handler != null)
				handler();
		}
	}
}
