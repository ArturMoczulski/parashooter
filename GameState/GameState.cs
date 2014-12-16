using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class GameState : IGameStateEventHandler
{
	
	public int SelectedLevelNumber { get { return selectedLevelNumber; } 
		set { 
			selectedLevelNumber = value; 
			dispatchEvent(new SelectedMissionChangedEvent());
		} 
	}
	
	public bool CurrentLevelLandingZoneHit { get { return currentLevelLandingZoneHit; } 
		set { 
			currentLevelLandingZoneHit = value;
			if( currentLevelLandingZoneHit ) dispatchEvent(new LandingZoneHitEvent());
		} 
	}
	
	public float CurrentLevelScore { get { return calculateCurrentScore(); } }
		
	public bool LevelFinished { get { return levelFinished; } 
		set { 
			levelFinished = value; 
			if (levelFinished) dispatchEvent(new LevelFinishedEvent());
		}
	}
	
	public bool LevelStarted { get { return levelStarted; } 
		set { 
			levelStarted = value; 
			if (levelStarted) dispatchEvent(new LevelStartedEvent());
		}
	}
	
	public int CurrentLevelNumber {
		get { return currentLevelNumber; }
		set {
			currentLevelNumber = value;
			dispatchEvent(new LevelChangedEvent(value));
		}
	}
	
	// level passed property is read-only; it's value should be only
	// a function of other game state variables
	public bool LevelPassed { 
		get { return levelPassed; } 
		protected set {
			levelPassed = value;
			if (levelPassed) dispatchEvent(new LevelPassedEvent());
		}
	}
	
	public float CurrentLevelCheckpointsChecked {
		get { return currentLevelCheckpointsChecked; }
		set { 
			float oldValue = currentLevelCheckpointsChecked;
				
			if( value == 0  ) dispatchEvent(new CurrentLevelCheckpointsCheckedResetEvent());
				
			currentLevelCheckpointsChecked = value; 
				
			if( value - oldValue > 0 ) dispatchEvent(new CurrentLevelCheckpointsCheckedIncreasedEvent());
			else if( value - oldValue < 0 ) dispatchEvent(new CurrentLevelCheckpointsCheckedDeacreasedEvent());
		}
	}
	
	public float CurrentLevelCheckpointsMissed {
	
		get { return currentLevelCheckpointsMissed; }
		set { 
				
			if( value == 0 ) dispatchEvent( new CurrentLevelCheckpointsMissedResetEvent() );
			else dispatchEvent(new CurrentLevelCheckpointsMissedChangeEvent());
				
			currentLevelCheckpointsMissed = value; 
			
			dispatchEvent(new CurrentLevelCheckpointsMissedChangedEvent());
		}
	}
	
	public List<GameStateEventHandler> getListeningEventHandlers() {
		return eventHandlers;
	}
	
	private GameState()
	{
		dispatchedEventTypes = new List<Type>();
		eventHandlers = new List<GameStateEventHandler>();
	}
	
	public static GameState Instance {
		get {
			if (instance == null) {
				instance = new GameState();
				instance.initialize();
			}	
			return instance;	
		}
	}
	
	public void initialize() {
		registerEventTypes();
		registerAllEventHandlers();
		dispatchEvent(new GameInitializeEvent());
	}
	
	protected float calculateCurrentScore() {
		
		if( !((ParashooterLevelManager)LevelManager.Instance).CurrentLevel )
				throw new UnityException("Trying to get current level score without current level loaded!");
		
		int checkpointsNumber = (((ParashooterLevelManager)LevelManager.Instance).CurrentLevel.GetComponent<ParashooterLevel>().Checkpoints.Count);
		float score = 0;
		
		if( checkpointsNumber != 0 ) {
			score = 1.0f - GameState.Instance.CurrentLevelCheckpointsMissed / checkpointsNumber;
		}
		
		return score*100f;
	}
	
	private void registerEventTypes() {
		
		
		dispatchedEventTypes.Add(typeof(GameStateEvent));
		dispatchedEventTypes.Add(typeof(GameInitializeEvent));
		dispatchedEventTypes.Add(typeof(LevelChangedEvent));
		dispatchedEventTypes.Add(typeof(EventHandlerRegisteredEvent));
		
		dispatchedEventTypes.Add(typeof(LevelStateEvent));
		dispatchedEventTypes.Add(typeof(LevelFinishedEvent));
		dispatchedEventTypes.Add(typeof(LevelStartedEvent));
		
		dispatchedEventTypes.Add(typeof(CurrentLevelCheckpointsCheckedChangeEvent));
		dispatchedEventTypes.Add(typeof(CurrentLevelCheckpointsCheckedResetEvent));
		
		dispatchedEventTypes.Add(typeof(CurrentLevelCheckpointsCheckedChangedEvent));
		dispatchedEventTypes.Add(typeof(CurrentLevelCheckpointsCheckedIncreasedEvent));
		dispatchedEventTypes.Add(typeof(CurrentLevelCheckpointsCheckedDeacreasedEvent));
		
		dispatchedEventTypes.Add(typeof(CurrentLevelCheckpointsMissedChangeEvent));
		dispatchedEventTypes.Add(typeof(CurrentLevelCheckpointsMissedResetEvent));
		dispatchedEventTypes.Add(typeof(CurrentLevelCheckpointsMissedChangedEvent));
		
		dispatchedEventTypes.Add(typeof(LandingZoneHitEvent));
		
		dispatchedEventTypes.Add(typeof(LevelPassedEvent));
		
		dispatchedEventTypes.Add(typeof(MenuStateEvent));
		dispatchedEventTypes.Add(typeof(SelectedMissionChangedEvent));
		
		
	}
	
	public void registerAllEventHandlers() {
		
		foreach(UnityEngine.Object obj in UnityEngine.Object.FindObjectsOfType(typeof(GameStateEventHandler))) {
			GameStateEventHandler gsEventHandler = (GameStateEventHandler) obj;
			registerEventHandler(gsEventHandler);
		}
		
	}
	
	public void registerEventHandler(GameStateEventHandler eventHandler) {
		if( !eventHandlers.Contains(eventHandler) ) {
			eventHandlers.Add(eventHandler);
			dispatchEvent(new EventHandlerRegisteredEvent());
		}
	}
	
	public void detachEventHandler(GameStateEventHandler eventHandler) {
		eventHandlers.Remove(eventHandler);
	}
	
	private void dispatchEvent(GameStateEvent gsEvent) {
		
		gsEvent.ReceivedBy = eventHandlers;
		
		foreach(Type eventType in dispatchedEventTypes) {
		
			if (gsEvent.GetType() == eventType ||
				gsEvent.GetType().IsSubclassOf(eventType)) {
				
				string handlerMethodName = "on" + eventType.FullName.Remove(eventType.FullName.Length-("Event").Length);
				
				object[] handlerMethodParams = new object[1] {gsEvent};
				
				foreach(GameStateEventHandler eventHandler in eventHandlers) {
						
					MethodInfo handlerMethod = eventHandler.GetType().GetMethod(handlerMethodName);
					if (handlerMethod != null) {
						handlerMethod.Invoke(eventHandler, handlerMethodParams);
					}
					
				}
				
				// Game state is also self-listening, mostly to handle internal dependencies
				// of state variables.
				MethodInfo selfHandlerMethod = this.GetType().GetMethod(handlerMethodName);
				selfHandlerMethod.Invoke(this, handlerMethodParams);
				
			}
		}
		
	}
	
	public void onGameState(GameStateEvent gsEvent) {}
	
	public void onGameInitialize(GameStateEvent gsEvent) {}
	
	public void onEventHandlerRegistered(EventHandlerRegisteredEvent gsEvent) {}
	
	public void onLevelChanged(LevelChangedEvent gsEvent) {
		LevelFinished = false;
		LevelPassed = false;
		CurrentLevelLandingZoneHit = false;
		CurrentLevelCheckpointsChecked = 0;
		CurrentLevelCheckpointsMissed = 0;
	}
	
	public void onLevelState(LevelStateEvent lsEvent) {
		if( ((ParashooterLevelManager)LevelManager.Instance).CurrentLevel != null ) {
			ParashooterLevel currentLevel = ((ParashooterLevelManager)LevelManager.Instance).CurrentLevel.GetComponent<ParashooterLevel>();
			if( currentLevel.LevelPassedCriteriaSet.satisfied(currentLevel) && !LevelPassed ) {
				LevelPassed = true;
			}
		}
	}
	
	public void onLevelFinished(LevelFinishedEvent gsEvent) {}
	public void onLevelStarted(LevelStartedEvent gsEvent) {}
	
	public void onCurrentLevelCheckpointsCheckedChange(GameStateEvent gsEvent) {}
	public void onCurrentLevelCheckpointsCheckedReset(GameStateEvent gsEvent) {}
	
	public void onCurrentLevelCheckpointsCheckedChanged(GameStateEvent gsEvent) {}
	public void onCurrentLevelCheckpointsCheckedIncreased(GameStateEvent gsEvent) {}
	public void onCurrentLevelCheckpointsCheckedDeacreased(GameStateEvent gsEvent) {}
	
	public void onCurrentLevelCheckpointsMissedChange(CurrentLevelCheckpointsMissedChangeEvent gsEvent) {}
	public void onCurrentLevelCheckpointsMissedReset(CurrentLevelCheckpointsMissedResetEvent gsEvent) {}
	public void onCurrentLevelCheckpointsMissedChanged(CurrentLevelCheckpointsMissedChangedEvent gsEvent) {}
	
	public void onLevelPassed(GameStateEvent gsEvent) {
	}
	public void onLevelFailed(GameStateEvent gsEvent) {}
	
	public void onLandingZoneHit(LandingZoneHitEvent gsEvent) {}
	
	public void onMenuState(MenuStateEvent gsEvent) {}
	
	public void onSelectedMissionChanged(SelectedMissionChangedEvent gsEvent) {}
	
	private List<Type> dispatchedEventTypes;
	private GameObject scoreDisplayObject;
	private List<GameStateEventHandler> eventHandlers;
	private static GameState instance;
	
	private bool levelFinished = false;
	private bool levelStarted = false;
	private bool levelPassed = false;
	private int currentLevelNumber;
	private float currentLevelCheckpointsChecked;
	private float currentLevelCheckpointsMissed;
	private bool currentLevelLandingZoneHit = false;
	private int selectedLevelNumber = 1;
	
}
