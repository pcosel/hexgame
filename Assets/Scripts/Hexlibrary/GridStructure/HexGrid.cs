using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Graphics;
using GameLogic;
using GameLogic.Actions.Network;
using GameLogic.Map;
using Network;
using GameLogic.Units;
using GameLogic.Actions;

namespace Hexlibrary
{
    public class HexGrid : MonoBehaviour
    {
        public static HexGrid _instance;

        public Battlefield battlefield;

        public BidirectionalDictionary<Hex, GenericUnit> units { get { return battlefield.units; } private set { } }

        public Dictionary<Hex, HexCellData> map { get { return battlefield.map; } private set { } }

        public Layout layout { get { return battlefield.layout; } private set { } }

        public Color defaultColour { get { return BattlefieldGenerator.defaultColour; } private set { } }

        public Color blockedColour { get { return BattlefieldGenerator.blockedColour; } private set { } }

        public Color touchedColour { get { return BattlefieldGenerator.touchedColour; } private set { } }

        public client localPlayer;

        public SpriteGraphicsHandler spriteGraphicsHandler;

        public Text cellLabelPrefab;

        Canvas gridCanvas;
        HexMesh hexMesh;

        GenericAction currentAction;

        // temporary field for testing
        List<Hex> allowedFields = new List<Hex>();

		TurnController turnController {
			get {
				return TurnController._instance;
			}
		}

        void Awake()
        {
            _instance = this;
            createMap();
            gridCanvas = GetComponentInChildren<Canvas>();
            hexMesh = GetComponentInChildren<HexMesh>();
        }

        void Start()
        {
            updateBattlefield();

            foreach (KeyValuePair<Hex, HexCellData> entry in battlefield.map)
            {
                Hex hex = entry.Key;

                Point point = battlefield.hexToUnit(hex);

                Text label = Instantiate<Text>(cellLabelPrefab);
                label.rectTransform.SetParent(gridCanvas.transform, false);
                label.rectTransform.anchoredPosition = new Vector2((float)point.x, (float)point.y);
                label.text = hex.getQ() + "\n" + hex.getR() + "\n" + hex.getS();
            }
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.M))
            {
                Hex clickedHex = GetInputHex(Input.mousePosition);
                if (clickedHex != null)
                {
                    GenericUnit unit = battlefield.units.getByType1(clickedHex);
                    if (unit != null)
                    {
                        if (turnController.MayUse(unit))
                        {
                            // This is only because I did not want to implement an extra UI to display the possibilities.
                            GenericAction action = getActions(unit)[0];
                            if (action.IsPossible)
                            {
                                allowedFields = prepareAction(action);
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (currentAction != null)
                {
                    Hex clickedHex = GetInputHex(Input.mousePosition);
                    if (allowedFields.Contains(clickedHex))
                    {
                        this.executeAction(clickedHex);
                    }
                }
                spriteGraphicsHandler.displayUnits(battlefield.units);
                resetTouchedCells();
            }
        }

        void updateBattlefield()
        {
            hexMesh.Triangulate(this);
            spriteGraphicsHandler.displayUnits(battlefield.units);
        }

        public void createMap()
        {
//			List<GenericUnit> units = new List<GenericUnit> {
//				new Fighter ("fighter", 100, 400, 3),
//				new Fighter ("fighter", 100, 400, 3)
//			};
//
//			battlefield = BattlefieldGenerator.randomCircleMap (5, units);
            battlefield = BattlefieldGenerator.emptyDefaultMap();
        }

        public Hex GetInputHex(Vector3 interactionPosition)
        {
            Ray inputRay = Camera.main.ScreenPointToRay(interactionPosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                return TouchedCell(hit.point);
            }
            else
            {
				print ("fdshk");
                return null;
            }
        }

        Hex TouchedCell(Vector3 position)
        {
            position = transform.InverseTransformPoint(position);
            Point point = new Point(position.x, position.y);
            return battlefield.unitToHex(point);
        }

        public void SetCellsColour(Hex[] hexes, Color colour)
        {
            foreach (Hex hex in hexes)
            {
                if (battlefield.map.ContainsKey(hex))
                {
                    HexCellData data = battlefield.map[hex];
                    if (data != null)
                    {
                        data.colour = colour;
                    }
                    else
                    {
                        Debug.LogError("ERROR: Trying to set colour of " + hex.ToString() + " in the map but its HexCellData is null.");
                    }
                }
            }
            hexMesh.Triangulate(this);
        }

        public void resetTouchedCells()
        {
            foreach (KeyValuePair<Hex, HexCellData> entry in battlefield.map)
            {
                if (entry.Value.passable)
                {
                    entry.Value.colour = BattlefieldGenerator.defaultColour;
                }
            }
            hexMesh.Triangulate(this);
        }

        public Hex[] getHexesInMovementRange(Hex center, int range)
        {
            Dictionary<Hex, bool> passableTerrain = new Dictionary<Hex, bool>();
            foreach (KeyValuePair<Hex, HexCellData> entry in battlefield.map)
            {
                passableTerrain.Add(entry.Key, entry.Value.passable);
            }
            foreach (Hex hex in battlefield.units.getAllType1())
            {
                passableTerrain[hex] = false;
            }
            return HexUtility.getHexesInMovementRange(center, range, passableTerrain);
        }

        public List<GenericAction> getActions(GenericUnit unit)
        {
            foreach (GenericAction action in unit.Actions)
            {
                action.EvaluatePossible(this);
            }
            return unit.Actions;
        }

        public List<Hex> prepareAction(GenericAction action)
        {
            currentAction = action;
            return action.PrepareAction(this);
        }

        public void executeAction(Hex hex)
        {
            GenericNetworkAction action = currentAction.ExecuteAction(this, new []{ hex });
            currentAction = null;
            resetTouchedCells();
            byte[] serializedAction = NetworkUtility.serialize(action);
            if (localPlayer != null)
            {
                localPlayer.CmdSendAction(serializedAction);
            }
            else
            {
                executeRemoteAction(serializedAction);
            }
        }

        public void executeRemoteAction(byte[] actionArray)
        {
            GenericNetworkAction action = NetworkUtility.deserialize(actionArray);
            action.executeAction(this);
            TurnController._instance.UpdateTurn();
            spriteGraphicsHandler.displayUnits(battlefield.units);
        }

        public Point hexToUnit(Hex hex)
        {
            return battlefield.hexToUnit(hex);
        }

        public Hex unitToHex(Point point)
        {
            return battlefield.unitToHex(point);
        }

        /*
		public void sendBattlefield() {
			localPlayer.CmdSendBattlefield (NetworkUtility.serialize (battlefield));
		}

		public void receiveBattlefield(byte[] battlefieldMessage) {
			battlefield = NetworkUtility.deserializeBattlefield (battlefieldMessage);
			updateBattlefield ();
		}
		*/
    }
}
