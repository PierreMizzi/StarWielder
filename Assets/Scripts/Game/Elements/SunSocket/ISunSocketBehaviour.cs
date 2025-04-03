using StarWielder.Gameplay.Player;
using UnityEngine;

public interface ISunSocketBehaviour
{
	GameObject gameObject{ get; }
	SunSocket sunSocket {get; set;}

	void Subscribe()
	{
		if (gameObject.TryGetComponent(out SunSocket sunSocket))
		{
			this.sunSocket = sunSocket;
			this.sunSocket.onSocket += CallbackSocket;
			this.sunSocket.onUnsocket += CallbackUnsocket;
		}
	}

	void Unsubscribe()
	{
		if (this.sunSocket != null)
		{
			this.sunSocket.onSocket -= CallbackSocket;
			this.sunSocket.onUnsocket -= CallbackUnsocket;
		}
	}

	void CallbackSocket(Star sun);
	void CallbackUnsocket(Star sun);

}