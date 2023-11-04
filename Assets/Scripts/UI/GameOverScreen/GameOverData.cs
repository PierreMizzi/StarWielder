public enum GameOverReason
{
	ShipDestroyed,
	StarDied,
}

public class GameOverData
{
	public GameOverReason reason;
	public float time;
	public float starEnergy;
}

public delegate void GameOverDelegate(GameOverReason reason);
public delegate void GameOverScreenDelegate(GameOverData data);

