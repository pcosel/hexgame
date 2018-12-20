using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using GameLogic;
using Hexlibrary;

namespace Network
{
	public class client : NetworkBehaviour
	{
		HexGrid hexGrid {
			get {
				return HexGrid._instance;
			}
		}

		TurnController turnController {
			get {
				return TurnController._instance;
			}
		}

		Text textLocalPlayerInfo;

		[SyncVar]
		public Roles playerRole;
	
		/*
		[Command]
		public void CmdRequestBattlefield ()
		{
			hexGrid.sendBattlefield ();
		}

		[Command]
		public void CmdSendBattlefield (byte[] message)
		{
			RpcReceiveBattlefield (message);
		}

		[ClientRpc]
		public void RpcReceiveBattlefield (byte[] message)
		{
			hexGrid.receiveBattlefield (message);
		}
		*/

		[Command]
		public void CmdSendAction (byte[] message)
		{
			RpcReceiveAction (message);
		}

		[ClientRpc]
		public void RpcReceiveAction (byte[] message)
		{
			hexGrid.executeRemoteAction (message);
		}

		[Command]
		public void CmdSyncActivePlayer()
		{
			RpcSyncActivePlayer (turnController.activePlayer);
		}

		[ClientRpc]
		public void RpcSyncActivePlayer(Roles activePlayer)
		{
			turnController.activePlayer = activePlayer;
		}


		void Awake ()
		{
			textLocalPlayerInfo = GameObject.Find ("Text_Local_Player_Info").GetComponent<Text> ();
		}

		void Start()
		{
			turnController.AddPlayer (this);
		}

		public override void OnStartLocalPlayer ()
		{
			hexGrid.localPlayer = this;

			turnController.localPlayer = this;

			if (!isServer)
				CmdSyncActivePlayer ();

			textLocalPlayerInfo.text = playerRole.ToString ();
	
			/* Taken out because of buffer too large errors. Should probably be replaced with some kind of seed mechanism.
			 * Then again, if we want to synch maps for clients that have been disconnected we would still need to synch this.
			if (!isServer) {
				CmdRequestBattlefield ();
			}
			*/
		}
	}
}
