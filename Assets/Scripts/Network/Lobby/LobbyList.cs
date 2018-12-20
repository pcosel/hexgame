using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;
using UnityEngine.UI;

public class LobbyList : MonoBehaviour {

	public static LobbyList _instance;

	public RectTransform viewportPlayerList;

	protected VerticalLayoutGroup _layout;
	protected List<LobbyPlayer> _players = new List<LobbyPlayer>();

	public ReadOnlyCollection<LobbyPlayer> lobbyPlayers
	{
		get
		{
			return _players.AsReadOnly ();
		}
	}

	public void OnEnable()
	{
		_instance = this;
		_layout = viewportPlayerList.GetComponent<VerticalLayoutGroup> ();
	}

	public void AddPlayer(LobbyPlayer lobbyPlayer)
	{
		if (_players.Contains (lobbyPlayer))
			return;

		_players.Add (lobbyPlayer);

		lobbyPlayer.transform.SetParent (viewportPlayerList.transform, false);
	}

	public void RemovePlayer(LobbyPlayer lobbyPlayer)
	{
		_players.Remove (lobbyPlayer);
	}
}
