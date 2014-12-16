using System;
using UnityEngine;

public abstract class GameStateEventHandler : MonoBehaviour, IGameStateEventHandler
{
	
	public virtual void onGameState(GameStateEvent gsEvent) {}
	public virtual void onGameInitialize(GameStateEvent gsEvent) {}
	public virtual void onLevelChanged(LevelChangedEvent gsEvent) {}
	public virtual void onEventHandlerRegistered(EventHandlerRegisteredEvent gsEvent) {}
	
	public virtual void onLevelState(LevelStateEvent gsEvent) {}
	public virtual void onLevelFinished(LevelFinishedEvent gsEvent) {}
	public virtual void onLevelStarted(LevelStartedEvent gsEvent) {	}
	
	public virtual void onCurrentLevelCheckpointsCheckedChange(GameStateEvent gsEvent) {}
	public virtual void onCurrentLevelCheckpointsCheckedReset(GameStateEvent gsEvent) {}
	
	public virtual void onCurrentLevelCheckpointsCheckedChanged(GameStateEvent gsEvent) {}
	public virtual void onCurrentLevelCheckpointsCheckedIncreased(GameStateEvent gsEvent) {}
	public virtual void onCurrentLevelCheckpointsCheckedDeacreased(GameStateEvent gsEvent) {}
	
	public virtual void onCurrentLevelCheckpointsMissedChange(CurrentLevelCheckpointsMissedChangeEvent gsEvent) {}
	public virtual void onCurrentLevelCheckpointsMissedReset(CurrentLevelCheckpointsMissedResetEvent gsEvent) {}
	public virtual void onCurrentLevelCheckpointsMissedChanged(CurrentLevelCheckpointsMissedChangedEvent gsEvent) {}
	
	public virtual void onLevelPassed(GameStateEvent gsEvent) {}
	public virtual void onLevelFailed(GameStateEvent gsEvent) {}
	
	public virtual void onLandingZoneHit(LandingZoneHitEvent gsEvent) {}
	
	public virtual void onMenuState(MenuStateEvent gsEvent) {}
	
	public virtual void onSelectedMissionChanged(SelectedMissionChangedEvent gsEvent) {}
	
	void OnLevelWasLoaded(int level) {
		GameState.Instance.registerEventHandler(this);
	}
	
	public void OnDestroy() {
		GameState.Instance.detachEventHandler(this);
	}
	
}

