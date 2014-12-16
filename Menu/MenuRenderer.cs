using UnityEngine;
using System.Collections;

public class MenuRenderer {
	
	public void render(MenuData data, GUISkin style) {
		
		scaleToResolultion(data, style);
		drawBackground(data, style);
		drawTitle(data, style);
		drawMainButtons(data, style);
		
	}
	
	protected void drawBackground(MenuData data, GUISkin style) {
	
		if( style.FindStyle(data.name) != null && style.FindStyle(data.name).normal.background != null ) {
			GUI.DrawTexture(
				new Rect(0,0,style.FindStyle(data.name).fixedWidth,style.FindStyle(data.name).fixedHeight),
				style.FindStyle(data.name).normal.background,
				ScaleMode.StretchToFill
			);
		}
		
	}
	
	protected void drawMainButtons(MenuData data, GUISkin style) {
	}
	
	protected void drawTitle(MenuData data, GUISkin style) {
		
		GUIStyle titleStyle = style.FindStyle("Title");
		if( titleStyle != null ) {
			GUI.Label(
				new Rect(titleStyle.margin.left,titleStyle.margin.top,243,65),
				data.title,
				titleStyle
			);
			
		}
		
	}
		
	protected void scaleToResolultion(MenuData data, GUISkin style) {
		
		GUI.matrix = Matrix4x4.TRS(
			new Vector3(
				(float)Screen.width / (float)style.FindStyle(data.name).fixedWidth,
				(float)Screen.height / (float)style.FindStyle(data.name).fixedHeight,
				0.0f
			),
			Quaternion.identity,
			new Vector3(
				(float)Screen.width / (float)style.FindStyle(data.name).fixedWidth,
				(float)Screen.height / (float)style.FindStyle(data.name).fixedHeight,
				1.0f
			)
		);
			
	}
	
}
