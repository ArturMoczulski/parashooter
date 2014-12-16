using System;
using UnityEngine;
using System.Collections;

public class AchievementTrigger : GameStateEventHandler
{
	
	public override void onCurrentLevelCheckpointsCheckedIncreased (GameStateEvent gsEvent)
	{
							
		AchievementManager.Instance.AddProgressToAchievement("First Circle", 1.0f);
		AchievementManager.Instance.AddProgressToAchievement("Second Circle", 1.0f);
		
	}

}


