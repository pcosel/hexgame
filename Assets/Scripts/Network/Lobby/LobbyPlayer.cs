using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using GameLogic;

public class LobbyPlayer : NetworkLobbyPlayer {

	public Button buttonReady;

	public Button buttonFighter;
	public Button buttonRogue;
	public Button buttonMage;
	public Button buttonVillain;
	Dictionary<Roles, Button> roleButtons;

	[Header("Disabled colour block")]
	public ColorBlock disabledColorBlock;

	[Space]
	[Header("Enabled colour block")]
	public ColorBlock enabledColorBlock;

	[Space]
	[Header("Local selected colour block")]
	public ColorBlock localSelectedColorBlock;

	[Space]
	[Header("Remote selected colour block")]
	public ColorBlock remoteSelectedColorBlock;

	[SyncVar(hook = "OnMyRole")]
	Roles _playerRole;

	public Roles playerRole {
		get {
			return _playerRole;
		}
	}

	void Awake()
	{
		roleButtons = new Dictionary<Roles, Button>(){
			{Roles.FIGHTER, buttonFighter},
			{Roles.ROGUE, buttonRogue},
			{Roles.MAGE, buttonMage},
			{Roles.VILLAIN, buttonVillain}
		};
	}

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();

		SetupLocalPlayer ();
	}

	public override void OnClientEnterLobby ()
	{
		base.OnClientEnterLobby ();

		LobbyList._instance.AddPlayer (this);

		SetupOtherPlayer ();
	}

	void SetupLocalPlayer()
	{
		buttonReady.interactable = true;
		buttonReady.transform.GetChild (0).GetComponent<Text> ().text = "READY";

		CmdSetRole (AvailableRoles () [0]);
	}

	public void UpdateRoleSelection()
	{
		if (isLocalPlayer) {
			UpdateRoleSelectionLocal ();
		} else {
			UpdateRoleSelectionRemote ();
		}
	}

	void UpdateRoleSelectionRemote()
	{
		foreach (Button roleButton in roleButtons.Values) {
			roleButton.gameObject.SetActive (false);
		}

		Button button = roleButtons [_playerRole];
		button.gameObject.SetActive (true);
		button.interactable = false;
		button.colors = localSelectedColorBlock;
	}

	void UpdateRoleSelectionLocal()
	{
		foreach (Button roleButton in roleButtons.Values) {
			roleButton.gameObject.SetActive (true);
			roleButton.interactable = true;
			roleButton.colors = enabledColorBlock;
		}

		foreach (LobbyPlayer lobbyPlayer in LobbyList._instance.lobbyPlayers) {
			roleButtons [lobbyPlayer._playerRole].gameObject.SetActive (false);
		}

		Button button = roleButtons [_playerRole];
		button.gameObject.SetActive (true);
		button.interactable = false;
		button.colors = localSelectedColorBlock;
	}

	void LocalSelected(Button button)
	{
		button.interactable = false;
	}

	void SetupOtherPlayer()
	{
		foreach (Button button in roleButtons.Values) {
			button.interactable = false;
		}

		buttonReady.interactable = false;
		buttonReady.transform.GetChild (0).GetComponent<Text> ().text = "...";

		UpdateRoleSelection ();

		OnClientReady (false);
	}

	public override void OnClientReady (bool readyState)
	{
		Text readyText = buttonReady.transform.GetChild (0).GetComponent<Text> ();
		if (readyState) {
			readyText.text = "LOCKED IN";
		} else {
			readyText.text = isLocalPlayer ? "READY" : "...";
		}
	}

	public void OnFighterClicked()
	{
		OnRoleClicked (Roles.FIGHTER);
	}

	public void OnRogueClicked()
	{
		OnRoleClicked (Roles.ROGUE);
	}

	public void OnMageClicked()
	{
		OnRoleClicked (Roles.MAGE);
	}

	public void OnVillainClicked()
	{
		OnRoleClicked (Roles.VILLAIN);
	}

	public void OnRoleClicked(Roles role)
	{
		if (AvailableRoles ().Contains (role)) {
			CmdSetRole (role);
		} else {
			Debug.LogError ("Role is not available.");
		}
	}

	public void OnMyRole(Roles role)
	{
		_playerRole = role;
		foreach (LobbyPlayer lobbyPlayer in LobbyList._instance.lobbyPlayers) {
			lobbyPlayer.UpdateRoleSelection ();
		}
	}

	[Command]
	public void CmdSetRole(Roles role)
	{
		_playerRole = role;
	}

	ReadOnlyCollection<Roles> AvailableRoles()
	{
		List<Roles> availableRoles = new List<Roles> ();
		foreach (Roles role in Enum.GetValues (typeof(Roles))) {
			availableRoles.Add (role);
		}
		foreach (LobbyPlayer lobbyPlayer in LobbyList._instance.lobbyPlayers) {
			if (availableRoles.Contains (lobbyPlayer._playerRole))
				availableRoles.Remove (lobbyPlayer._playerRole);
		}
		return availableRoles.AsReadOnly ();
	}

	public void OnReadyClicked()
	{
		SendReadyToBeginMessage ();
	}

	public void OnDestroy()
	{
		LobbyList._instance.RemovePlayer (this);
	}
}
