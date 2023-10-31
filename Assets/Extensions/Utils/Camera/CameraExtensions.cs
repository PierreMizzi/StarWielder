using UnityEngine;

public static class CameraExtensions
{
	public static float MagnitudeToEdge(this Camera camera, float angle)
	{
		float absCosAngle = Mathf.Abs(Mathf.Cos(angle));
		float absSinAngle = Mathf.Abs(Mathf.Sin(angle));

		float magnitude;

		if (camera.pixelWidth / 2f * absSinAngle <= camera.pixelHeight / 2f * absCosAngle)
			magnitude = camera.pixelWidth / 2f / absCosAngle;
		else
			magnitude = camera.pixelHeight / 2f / absSinAngle;

		return magnitude;
	}

	public static float MagnitudeToEdge(this Camera camera, float angle, float sizeOffset = 1f)
	{
		float absCosAngle = Mathf.Abs(Mathf.Cos(angle));
		float absSinAngle = Mathf.Abs(Mathf.Sin(angle));

		float magnitude;
		Vector2 cameraExtents = new Vector2(camera.pixelWidth * 0.5f * sizeOffset, camera.pixelHeight * 0.5f * sizeOffset);

		if (cameraExtents.x * absSinAngle <= cameraExtents.y * absCosAngle)
			magnitude = cameraExtents.x / absCosAngle;
		else
			magnitude = cameraExtents.y / absSinAngle;

		return magnitude;
	}


	public static bool IsHorizontalOrVerticalEdge(this Camera camera, float angle)
	{
		float absCosAngle = Mathf.Abs(Mathf.Cos(angle));
		float absSinAngle = Mathf.Abs(Mathf.Sin(angle));

		return camera.pixelWidth / 2f * absSinAngle <= camera.pixelHeight / 2f * absCosAngle;
	}
}