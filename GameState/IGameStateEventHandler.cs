using System;
using UnityEngine;

interface IGameStateEventHandler
{
	
	void onGameState(GameStateEvent gsEvent);
	void onGameInitialize(GameStateEvent gsEvent);
	void onLevelChanged(LevelChangedEvent gsEvent);
	void onEventHandlerRegistered(EventHandlerRegisteredEvent gsEvent);
	
	void onLevelState(LevelStateEvent gsEvent);
	void onLevelFinished(LevelFinishedEvent gsEvent);
	void onLevelStarted(LevelStartedEvent gsEvent);
	
	void onCurrentLevelCheckpointsCheckedChange(GameStateEvent gsEvent);
	void onCurrentLevelCheckpointsCheckedReset(GameStateEvent gsEvent);
	
	void onCurrentLevelCheckpointsCheckedChanged(GameStateEvent gsEvent);
	void onCurrentLevelCheckpointsCheckedIncreased(GameStateEvent gsEvent);
	void onCurrentLevelCheckpointsCheckedDeacreased(GameStateEvent gsEvent);
	
	void onCurrentLevelCheckpointsMissedChange(CurrentLevelCheckpointsMissedChangeEvent gsEvent);
	void onCurrentLevelCheckpointsMissedReset(CurrentLevelCheckpointsMissedResetEvent gsEvent);
	void onCurrentLevelCheckpointsMissedChanged(CurrentLevelCheckpointsMissedChangedEvent gsEvent);
	
	void onLevelPassed(GameStateEvent gsEvent);
	void onLevelFailed(GameStateEvent gsEvent);
	
	void onLandingZoneHit(LandingZoneHitEvent gsEvent);
	
	void onMenuState(MenuStateEvent gsEvent);
	
	void onSelectedMissionChanged(SelectedMissionChangedEvent gsEvent);
	
}

