using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public abstract class ObjectDebugPopUp : DebugPopUpWindow
{
	
	protected object objectUnderDebug;
	protected string[] ignoredPropertiesNames = new string[] {};
	
	protected abstract object setObjectUnderDebug();
	protected virtual string[] getIngoredPropertiesNames() { return new string[] {}; }
	
	protected virtual PropertyInfo[] getPropertiesToDisplay(Type limitingClass = null) {
		
		List<PropertyInfo> properties = new List<PropertyInfo>();
		
		if( limitingClass == null )
			properties.AddRange( objectUnderDebug.GetType().GetProperties() );
		else {
			
			if( limitingClass != objectUnderDebug.GetType() && !limitingClass.IsSubclassOf(objectUnderDebug.GetType() ) )
				throw new UnityException("The limiting class is not a base class of the object under debug");
			
			Type iteratedType = objectUnderDebug.GetType();
			while( iteratedType != null ) {
				properties.AddRange(iteratedType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance));
				iteratedType = iteratedType == limitingClass ? null : iteratedType.BaseType;
			}
		}
		
		foreach( string ignoredPropertyName in ignoredPropertiesNames ) {
			PropertyInfo ignoredProperty = objectUnderDebug.GetType().GetProperty(ignoredPropertyName);
			if( ignoredProperty != null )
				properties.Remove(ignoredProperty);
		}			
		
		return properties.ToArray();
	}
	
	protected void Awake() {
		
		objectUnderDebug = setObjectUnderDebug();
		if( objectUnderDebug != null )
			title = objectUnderDebug.GetType().Name + " Debug";
		ignoredPropertiesNames = getIngoredPropertiesNames();
		
		base.Awake();
		
	}
	
	protected override void drawWindow(int windowId) {
		drawProperties(windowId);
	}
	
	protected void drawProperties(int windowId) {
		
		PropertyInfo[] propertiesToDisplay = getPropertiesToDisplay();
		foreach(PropertyInfo property in propertiesToDisplay) {
			MethodInfo getter = property.GetGetMethod();
			if( getter != null ) {
				
				drawProperty(property);
				
			}
		}
		
	}
	
	protected void drawProperty(PropertyInfo property) {
		
		MethodInfo getter = property.GetGetMethod();
		MethodInfo setter = property.GetSetMethod();
		
		GUILayout.BeginHorizontal();
				
		GUILayout.Label(property.Name);
		
		string customMethodName = Char.ToLowerInvariant(property.Name[0]) + (property.Name+"Property").Substring(1);
		MethodInfo customDrawMethod = this.GetType().GetMethod(customMethodName, BindingFlags.NonPublic | BindingFlags.Instance);
		
		string afterSetterCallbackName = "after"+property.Name+"Set";
		MethodInfo customAfterSetterCallback = this.GetType().GetMethod(afterSetterCallbackName, BindingFlags.NonPublic | BindingFlags.Instance);
		
		object oldValue = getter.Invoke(objectUnderDebug, null);
		
		if( customDrawMethod != null ) {
			customDrawMethod.Invoke(this, new object[] {getter,setter});
		} else {
			if( setter == null ) { // read-only
				GUILayout.Label(getter.Invoke(objectUnderDebug,null).ToString());
			} else { // read-write	
				drawPropertyWriteDefault(getter, setter);
			}
		}
		
		if( !oldValue.Equals(getter.Invoke(objectUnderDebug, null)) && customAfterSetterCallback != null ) {
			customAfterSetterCallback.Invoke(this, null);
		}
				
		GUILayout.EndHorizontal();
	}
	
	protected void drawPropertyWriteDefault(MethodInfo getter, MethodInfo setter) {
		
		if( getter.ReturnType == typeof(Boolean) ) { // Booleans
			booleanPropertyWrite(getter, setter);
		} else if(  getter.ReturnType == typeof(Int32) ||
					getter.ReturnType == typeof(Int64) ) { // Integers
			integerPropertyWrite(getter,setter);						
		} else if( getter.ReturnType == typeof(Single) ) {
			singlePropertyWrite(getter,setter);
		} else if( getter.ReturnType == typeof(Vector2) ) {
			vector2PropertyWrite(getter,setter);
		}
		
	}
	
	protected void booleanPropertyWrite(MethodInfo getter, MethodInfo setter) {
		bool oldValue = (bool)getter.Invoke(objectUnderDebug, null);
		bool newValue = GUILayout.Toggle(oldValue, "");
		if( oldValue != newValue )
			setter.Invoke(objectUnderDebug, new object[] {newValue});
	}
	
	protected void integerPropertyWrite(MethodInfo getter, MethodInfo setter) {
		
		string oldValue = getter.Invoke(objectUnderDebug,null).ToString();
		string newValue = GUILayout.TextField(oldValue).ToString();
		if( oldValue != newValue ) {
			int newValueInt = (int)(getter.ReturnType == typeof(Int32) ? Int32.Parse(newValue) : Int64.Parse(newValue));
			setter.Invoke(objectUnderDebug,new object[] {newValueInt});
		}
		
	}
	
	protected void singlePropertyWrite(MethodInfo getter, MethodInfo setter) {
		
		string oldValue = getter.Invoke(objectUnderDebug,null).ToString();
		string newValue = GUILayout.TextField(oldValue).ToString();
		if( oldValue != newValue ) {
			float newValueInt = (float)Single.Parse(newValue);
			setter.Invoke(objectUnderDebug,new object[] {newValueInt});
		}
		
	}
	
	protected void rangeIntegerPropertyWrite(MethodInfo getter, MethodInfo setter, int min, int max) {
		int oldValue = (int)getter.Invoke(objectUnderDebug,null);
		GUILayout.Label(getter.Invoke(objectUnderDebug,null).ToString());
		int newValue = (int)GUILayout.HorizontalSlider((int)getter.Invoke(objectUnderDebug,null),min,max);
		if( oldValue != newValue ) {
			setter.Invoke(objectUnderDebug, new object[] {newValue});
		}
	}
	
	protected void vector2PropertyWrite(MethodInfo getter, MethodInfo setter) {
		Vector2 oldValue = (Vector2)getter.Invoke(objectUnderDebug,null);
		Vector2 newValue = new Vector2();
		newValue.x = Single.Parse(GUILayout.TextField(oldValue.x.ToString()));
		newValue.y = Single.Parse(GUILayout.TextField(oldValue.y.ToString()));
		if( oldValue != newValue )
			setter.Invoke(objectUnderDebug, new object[] {newValue});
	}
	
}


