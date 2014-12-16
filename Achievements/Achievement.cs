using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Achievement
{
    public string Name;
    public string Description;
    public Material IconIncomplete;
    public Material IconComplete;
    public int RewardPoints;
    public float TargetProgress;
    public bool Secret;

//    [HideInInspector]
    public bool Earned = false;
    private float currentProgress = 0.0f;

    public bool AddProgress(float progress)
    {
        if (Earned)
        {
            return false;
        }

        currentProgress += progress;
        if (currentProgress >= TargetProgress)
        {
            Earned = true;
            return true;
        }

        return false;
    }

    public bool SetProgress(float progress)
    {
        if (Earned)
        {
            return false;
        }

        currentProgress = progress;
        if (progress >= TargetProgress)
        {
            Earned = true;
            return true;
        }

        return false;
    }
	
	public Material GetIcon
	{
		get 
		{ 
			return (Earned ? IconComplete : IconIncomplete); 
		}		
	}
}

