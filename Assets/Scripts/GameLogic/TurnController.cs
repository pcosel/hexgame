using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using Hexlibrary;
using Network;
using GameLogic.Units;
using GameLogic.Actions;

namespace GameLogic
{
    public class TurnController : MonoBehaviour
    {
        public static TurnController _instance;

        Text textActivePlayerInfo;

        public client localPlayer;

        Roles _activePlayer = Roles.FIGHTER;

        public Roles activePlayer
        {
            get
            {
                return _activePlayer;
            }
            set
            {
                _activePlayer = value;
				heroesTurn = value.isHero ();
                textActivePlayerInfo.text = _activePlayer.ToString();
            }
        }

		HexGrid hexGrid {
			get {
				return HexGrid._instance;
			}
		}

        List<GenericUnit> units
        {
            get
            {
                return hexGrid.battlefield.units.getAllType2();
            }
        }

        public bool heroesTurn;

        List<Roles> heroesOrder = new List<Roles>()
        {
            Roles.FIGHTER,
            Roles.ROGUE,
            Roles.MAGE
        };

        public HashSet<Roles> heroesInGame = new HashSet<Roles>();

        void Awake()
        {
            if (!gameObject.GetComponent<NetworkIdentity>())
            {
                gameObject.AddComponent<NetworkIdentity>();
            }
            _instance = this;
            textActivePlayerInfo = GameObject.Find("Text_Active_Player_Info").GetComponent<Text>();
        }

        void Start()
        {
			activePlayer = LobbyList._instance.lobbyPlayers [0].playerRole;
			heroesTurn = activePlayer.isHero ();
        }

        public void AddPlayer(client player)
        {
			if (player.playerRole.isHero ())
            {
                heroesInGame.Add(player.playerRole);
            }
        }

        public bool MayUse(GenericUnit genericUnit)
        {
            if (genericUnit.owner != localPlayer.playerRole)
                return false;

            if (localPlayer.playerRole != activePlayer)
                return false;

            if (localPlayer.playerRole != Roles.VILLAIN)
                return true;
            else
            {
                List<GenericUnit> alreadyUsedUnits = AlreadyUsedUnits();

                if (alreadyUsedUnits.Contains(genericUnit))
                    return true;
                else if (alreadyUsedUnits.Count < 3)
                    return true;
                else
                    return false;
            }
        }

        public void UpdateTurn()
        {
            if (heroesTurn)
            {
                UpdateHeroesTurn();
            }
            else
            {
                UpdateVillainTurn();
            }
        }

        void UpdateHeroesTurn()
        {
            bool allActionsUsed = true;
            foreach (GenericUnit genUnit in units)
            {
                if (genUnit.owner == activePlayer)
                {
                    if (!genUnit.UsedActionType[ActionType.PrimaryAction] || !genUnit.UsedActionType[ActionType.SecondaryAction])
                    {
                        allActionsUsed = false;
                        break;
                    }
                }
            }
            if (allActionsUsed)
            {
                NextHero();
            }
        }

        void NextHero()
        {
            int currentIndex = heroesOrder.LastIndexOf(activePlayer);
            for (int i = 1; i < heroesOrder.Count; i++)
            {
                Roles nextHero = heroesOrder[(currentIndex + i) % heroesOrder.Count];
				if (!heroesInGame.Contains (nextHero))
					continue;
                foreach (GenericUnit genUnit in units)
                {
                    if (genUnit.owner == nextHero)
                    {
                        if (!genUnit.UsedActionType[ActionType.PrimaryAction] || !genUnit.UsedActionType[ActionType.SecondaryAction])
                        {
                            activePlayer = nextHero;
                            return;
                        }
                    }
                }
            }

            StartVillainTurn();
        }

        void StartVillainTurn()
        {
            heroesTurn = false;
            foreach (GenericUnit genUnit in units)
            {
                if (genUnit.owner == Roles.VILLAIN)
                {
                    genUnit.UsedActionType[ActionType.PrimaryAction] = false;
                    genUnit.UsedActionType[ActionType.SecondaryAction] = false;
                }
            }
            activePlayer = Roles.VILLAIN;
        }

        void UpdateVillainTurn()
        {
            List<GenericUnit> alreadyUsedUnits = AlreadyUsedUnits();

			int numberOfVillainUnits = 0;
			foreach (GenericUnit genUnit in units) {
				if (genUnit.owner.Equals (Roles.VILLAIN))
					numberOfVillainUnits++;
			}

			if (alreadyUsedUnits.Count < 3 && numberOfVillainUnits >= 3)
                return;

            foreach (GenericUnit genUnit in alreadyUsedUnits)
            {
                if (!genUnit.UsedActionType[ActionType.PrimaryAction])
                    return;
                if (!genUnit.UsedActionType[ActionType.SecondaryAction])
                    return;
            }
            StartHeroesTurn();
        }

        void StartHeroesTurn()
        {
            heroesTurn = true;
            foreach (GenericUnit genUnit in units)
            {
                if (genUnit.IsHero)
                {
                    genUnit.UsedActionType[ActionType.PrimaryAction] = false;
                    genUnit.UsedActionType[ActionType.SecondaryAction] = false;
                }
            }
            foreach (Roles role in heroesOrder)
            {
                if (heroesInGame.Contains(role))
                {
                    activePlayer = role;
                    return;
                }
            }
            Debug.Log("No heroes in game, starting the next villain turn.");
            StartVillainTurn();
        }

        public List<GenericUnit> AlreadyUsedUnits()
        {
            List<GenericUnit> alreadyUsedUnits = new List<GenericUnit>();
            foreach (GenericUnit genUnit in units)
            {
                Dictionary<ActionType, bool> usedActionType = genUnit.UsedActionType;
                if (usedActionType[ActionType.PrimaryAction] || usedActionType[ActionType.SecondaryAction])
                {
                    alreadyUsedUnits.Add(genUnit);
                }
            }
            return alreadyUsedUnits;
        }
    }
}
