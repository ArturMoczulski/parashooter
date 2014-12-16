using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class GameStateEventsDebugPopUp : KeyToggablePopUpWindow
{	
	public int eventsListCapacity = 3;
	
	protected void Awake() {
		base.Awake();
		window = new Rect(
			Screen.width - Screen.width/2,
			0,
			Screen.width/2,
			Screen.height/6);
		events = new List<GameStateEvent>();
		title = "Game State Events Debug";
	}
	
	public void onGameState(GameStateEvent gsEvent) {
		if( events == null )
			events = new List<GameStateEvent>();
		events.Insert(0, gsEvent);
		if( events.Count > eventsListCapacity && detailedView == 0 ) 
			events.RemoveAt( events.Count-1 );
	}
	
	protected override void drawWindow(int windowId) {
		drawModeSwitch();
		switch(mode) {
			case Mode.Events:
				drawEventsStack();
				break;
			case Mode.EventHandlers:
				drawEventHandlers();
				break;
			case Mode.DetailedView:
				drawEventDetails();
				break;
		}
	}
	
	protected void drawModeSwitch() {
		GUILayout.BeginHorizontal();
		if( GUILayout.Toggle(mode == Mode.Events, "Show events") )
			mode = Mode.Events;
		if( GUILayout.Toggle(mode == Mode.EventHandlers, "Show handlers") )
			mode = Mode.EventHandlers;
		GUILayout.EndHorizontal();
	}
	
	protected void drawEventHandlers() {
		foreach( GameStateEventHandler handler in GameState.Instance.getListeningEventHandlers() ) {
			GUILayout.Label(handler.GetType().Name);
		}
	}
	
	protected void drawEventDetails() {
		if( GUILayout.Button("Back") ) {
			detailedView = 0;
			mode = Mode.Events;
		}
		GUILayout.Label(events[detailedView].GetType().Name);
		GUILayout.Label("received by:");
		foreach( GameStateEventHandler handler in events[detailedView].ReceivedBy ) {
			GUILayout.Label(handler.GetType().Name);	
		}
	}
	
	protected void drawEventsStack() {
		
		int i = 1;
		foreach(GameStateEvent gsEvent in events) {
			
			drawEventsStackEntry(gsEvent, i);
			i++;
			
		}
		
	}
	
	protected void drawEventsStackEntry(GameStateEvent gsEvent, int index) {
		
		GUILayout.BeginHorizontal();
		GUILayout.Label(index.ToString()+". "+gsEvent.GetType().ToString());
		if( GUILayout.Button(">") ) {
			mode = Mode.DetailedView;
			detailedView = index-1;
		}
		GUILayout.EndHorizontal();
		
	}
	
	protected List<GameStateEvent> events;
	protected enum Mode { Events, EventHandlers, DetailedView };
	protected Mode mode = Mode.Events;
	protected int detailedView = 0;
	
}