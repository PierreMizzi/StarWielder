using UnityEngine;

public delegate void Vector2Delegate(Vector2 value);
public delegate void FloatDelegate(float value);
public delegate void BoolDelegate(bool value);
public delegate void IntDelegate(int value);
public delegate void Collision2DDelegate(Collision2D other);
public delegate void Collider2DDelegate(Collider2D other);

public delegate float ReturnFloatDelegate();
public delegate int ReturnIntDelegate();