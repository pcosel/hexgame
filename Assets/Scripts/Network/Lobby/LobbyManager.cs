using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

using Network;

public class LobbyManager : NetworkLobbyManager {

	public static LobbyManager _instance;

	[Header("Unity UI Lobby")]
	[Tooltip("Time in second between all players ready & match start")]
	public float prematchCountdown = 5.0f;

	[Space]
	[Header("UI Reference")]
	public LobbyHeader lobbyHeader;

	public RectTransform panelMain;
	public RectTransform panelLobbyList;

	public InputField inputFieldServerLocation;

	protected RectTransform _panelCurrent;

	void Awake()
	{
		if (!FindObjectOfType<EventSystem>())
		{
			GameObject obj = new GameObject("EventSystem");
			obj.AddComponent<EventSystem>();
			obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;
		}
		customConfig = true;
		channels.Add (QosType.ReliableFragmented);
	}

	void Start()
	{
		_instance = this;

		_panelCurrent = panelMain;

		GetComponent<Canvas> ().enabled = true;

		DontDestroyOnLoad (gameObject);

		lobbyHeader.SetServerInfo();
	}

	public override void OnLobbyClientSceneChanged (NetworkConnection conn)
	{
		lobbyHeader.gameObject.SetActive (false);
		panelLobbyList.gameObject.SetActive (false);
	}

	public void ToggleLobbyListFromMainMenu(bool displayLobbyList)
	{
		if (displayLobbyList) {
			panelLobbyList.gameObject.SetActive (true);
			panelMain.gameObject.SetActive (false);
		} else {
			lobbyHeader.SetServerInfo ();

			panelLobbyList.gameObject.SetActive (false);
			panelMain.gameObject.SetActive (true);
		}
	}

	public void AddLocalPlayer()
	{
		TryToAddPlayer ();
	}

	public void RemovePlayer(LobbyPlayer lobbyPlayer)
	{
		lobbyPlayer.RemovePlayer ();
	}

	public override void OnStartHost ()
	{
		base.OnStartHost ();

		ToggleLobbyListFromMainMenu (true);
		lobbyHeader.SetServerInfo("Hosting", networkAddress);
	}

	public void OnClickHost()
	{
		StartHost ();
	}

	public void OnClickJoin()
	{
		ToggleLobbyListFromMainMenu (true);

		networkAddress = inputFieldServerLocation.text;
		StartClient ();

		lobbyHeader.SetServerInfo("Connecting...", networkAddress);
	}

	public override bool OnLobbyServerSceneLoadedForPlayer (GameObject _lobbyPlayer, GameObject _gamePlayer)
	{
		LobbyPlayer lobbyPlayer = _lobbyPlayer.GetComponent<LobbyPlayer> ();
		client gameClient = _gamePlayer.GetComponent<client> ();

		gameClient.playerRole = lobbyPlayer.playerRole;

		return true;
	}

	public override void OnClientConnect (NetworkConnection conn)
	{
		base.OnClientConnect (conn);

		if (!NetworkServer.active) {
			ToggleLobbyListFromMainMenu (true);
			lobbyHeader.SetServerInfo ("Client", networkAddress);
		}
	}

	public override void OnClientDisconnect (NetworkConnection conn)
	{
		base.OnClientDisconnect (conn);
		ToggleLobbyListFromMainMenu (false);
	}

	public override void OnClientError (NetworkConnection conn, int errorCode)
	{
		ToggleLobbyListFromMainMenu (false);
		Debug.Log ("Client error: " + errorCode.ToString ());
	}
}
