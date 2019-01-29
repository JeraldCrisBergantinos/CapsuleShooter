using UnityEngine;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {
	
	public static ulong score { get; private set; }
	float lastEnemyKillTime;
	int streakCount;
	float streakExpiryTime = 1;
	
	void Start() {
		score = 0;
		lastEnemyKillTime = 0;
		streakCount = 0;
		
		Enemy.OnDeathStatic += OnEnemyKilled;
		Player player = FindObjectOfType(typeof(Player)) as Player;
		player.OnDeath += OnPlayerDeath;
	}
	
	void OnEnemyKilled() {
		if ( (Time.time < lastEnemyKillTime + streakExpiryTime) && streakCount < 30 ) {
			++streakCount;
		}
		else {
			streakCount = 0;
		}
		
		lastEnemyKillTime = Time.time;
		score += 5 + (ulong)Mathf.Pow(2,streakCount);
	}
	
	void OnPlayerDeath() {
		Enemy.OnDeathStatic -= OnEnemyKilled;
	}
}
