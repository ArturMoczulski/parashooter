using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Reflection;

public class AchievementManager : MonoBehaviour
{
	private static AchievementManager instance;
	private int currentRewardPoints = 0;
    private int potentialRewardPoints = 0;
    public Achievement[] Achievements;

	public static AchievementManager Instance {
	
		get {
			if (instance == null) {
				instance = (AchievementManager)UnityEngine.Object.FindObjectOfType(MethodBase.GetCurrentMethod().DeclaringType);
			}
			return instance;
			
		}
	}
		void Start()
	{
	    ValidateAchievements();
        UpdateRewardPointTotals();
	}
	
    // Make sure the setup assumptions we have are met.
    private void ValidateAchievements()
    {
        ArrayList usedNames = new ArrayList();
        foreach (Achievement achievement in Achievements)
        {
            if (achievement.RewardPoints < 0)
            {
                Debug.LogError("AchievementManager::ValidateAchievements() - Achievement with negative RewardPoints! " + achievement.Name + " gives " + achievement.RewardPoints + " points!");
            }

            if (usedNames.Contains(achievement.Name))
            {
                Debug.LogError("AchievementManager::ValidateAchievements() - Duplicate achievement names! " + achievement.Name + " found more than once!");
            }
            usedNames.Add(achievement.Name);
        }
    }

    private Achievement GetAchievementByName(string achievementName)
    {
        return Achievements.FirstOrDefault(achievement => achievement.Name == achievementName);
    }

    private void UpdateRewardPointTotals()
    {
        currentRewardPoints = 0;
        potentialRewardPoints = 0;

        foreach (Achievement achievement in Achievements)
        {
            if (achievement.Earned)
            {
                currentRewardPoints += achievement.RewardPoints;
            }

            potentialRewardPoints += achievement.RewardPoints;
        }
    }

    private void AchievementEarned()
    {
        UpdateRewardPointTotals();   
    }
	


    public void AddProgressToAchievement(string achievementName, float progressAmount)
    {
        Achievement achievement = GetAchievementByName(achievementName);
        if (achievement == null)
        {
            Debug.LogWarning("AchievementManager::AddProgressToAchievement() - Trying to add progress to an achievemnet that doesn't exist: " + achievementName);
            return;
        }

        if (achievement.AddProgress(progressAmount))
        {
            AchievementEarned();
        }
		PrintAllEarnedAchievements();
    }

    public void SetProgressToAchievement(string achievementName, float newProgress)
    {
        Achievement achievement = GetAchievementByName(achievementName);
        if (achievement == null)
        {
            Debug.LogWarning("AchievementManager::SetProgressToAchievement() - Trying to add progress to an achievemnet that doesn't exist: " + achievementName);
            return;
        }

        if (achievement.SetProgress(newProgress))
        {
            AchievementEarned();
        }
		PrintAllEarnedAchievements();
    }
	
	public  void PrintAllEarnedAchievements() {
		foreach(Achievement achievement in Achievements){
			if (achievement.Earned){
					Debug.Log(achievement.Name);	

			}
		}
		
	}
	
	public Achievement[] GetAchievements
	{
		get { return Achievements;}
	}


}
