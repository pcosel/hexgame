using System;

using UnityEngine;

[Serializable]
public class SerializableColor {

	/// <summary>
	/// red component
	/// </summary>
	public float r;

	/// <summary>
	/// green component
	/// </summary>
	public float g;

	/// <summary>
	/// blue component
	/// </summary>
	public float b;

	/// <summary>
	/// alpha component
	/// </summary>
	public float a;

	/// <summary>
	/// Initializes a new instance of the SerializableColor class.
	/// </summary>
	/// <param name="rR">red component</param>
	/// <param name="rG">green component</param>
	/// <param name="rB">blue component</param>
	/// <param name="rA">alpha component</param>
	public SerializableColor(float rR, float rG, float rB, float rA) {
		r = rR;
		g = rG;
		b = rB;
		a = rA;
	}

	public static implicit operator Color(SerializableColor rColor)
	{
		return new Color (rColor.r, rColor.g, rColor.b, rColor.a);
	}

	public static implicit operator SerializableColor(Color rColor)
	{
		return new SerializableColor (rColor.r, rColor.g, rColor.b, rColor.a);
	}
}
