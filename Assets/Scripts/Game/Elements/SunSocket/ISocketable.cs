using StarWielder.Gameplay.Player;

// Not used, but might be useful later
// For Overheater's SunSocket for example
public interface ISocketable
{
	SunSocket CurrentSunSocket {get; set;}
	void onSocket(SunSocket sunSocket);
	void onUnsocket();
}