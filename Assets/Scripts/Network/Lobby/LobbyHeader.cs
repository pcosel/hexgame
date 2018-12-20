using UnityEngine;
using UnityEngine.UI;

public class LobbyHeader : MonoBehaviour {

	public Text textStatusInfo;
	public Text textHostInfo;

	public void SetServerInfo()
	{
		SetServerInfo ("Offline", "none");
	}

	public void SetServerInfo(string status, string host)
	{
		textStatusInfo.text = status;
		textHostInfo.text = host;
	}
}
