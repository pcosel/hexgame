using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexlibrary;

public class UIHandler : MonoBehaviour {

	HexGrid hexGrid;

	[HideInInspector]
	public Vector3 touchPosition;
	[HideInInspector]
	public bool clicked;

	public bool sizeRelativeToCamera;
	public float subItemsDegrees;
	public float animationSpeed;
	public float minDistance;
	public float maxScale;
	public float sizeQuotient;
	public Transform[] sources;
	public Transform[] destinations;
	public UIBuilder builder;

	public GameLogic.TurnController turncontroller; 

	public enum uiStatus
	{
		folded,
		unfolded,
		folding,
		unfolding
	}

	private int index;
	private Hex touchedHex;
	private Hex previousHex;
	private uiStatus status;
	private GameLogic.Units.GenericUnit unit;
	private GameLogic.Actions.GenericAction action;
	private bool preparingAction;
	private List<Hex> validHexes;
	private GameObject activeItem;

	void Start () {
		index = 0;
		status = uiStatus.folded;
		preparingAction = false;
		clicked = false;
		hexGrid = HexGrid._instance;
	}

	void Update () {


		if (sizeRelativeToCamera) {
			float scale = Camera.main.orthographicSize * sizeQuotient;
			gameObject.transform.localScale = new Vector3 (scale, scale, scale);
		}

		if(!preparingAction && clicked) {

			touchedHex = hexGrid.GetInputHex (touchPosition);

			if (status == uiStatus.folded) {
				unit = hexGrid.units.getByType1 (touchedHex);
				if (unit != null) {

					if(turncontroller.activePlayer == unit.owner) {

					if (builder.builtUnit == null) {
						builder.build (unit);
					} else if(builder.builtUnit != unit) {
						builder.clear ();
						builder.build (unit);
					}
					sources [0].GetComponent<UIItem> ().action = unit.Actions [0];
					Vector3 centerOfHex = GridUtility.pointToVector3 (hexGrid.layout.hexToUnit (touchedHex));
					gameObject.transform.position = centerOfHex;
					status = uiStatus.unfolding;
					previousHex = touchedHex;
					}
				}

			} else if(status == uiStatus.unfolded){
				
				GameObject touchedUIItem = touchedObject (touchPosition);

				//touched UIItem
				if (touchedUIItem != null) {

					if (touchedUIItem == activeItem) {
						collapseAll (); 
						status = uiStatus.unfolding;

					} else {

						if (touchedUIItem.GetComponent<UIItem> () == null) {
							Debug.LogError ("Touched object not a UIItem!");
						}

						//touched UIItem is leave
						if (touchedUIItem.GetComponent<UIItem> ().successors.Count == 0) {

							if (touchedUIItem.GetComponent<UIItem> ().action == null) {
						
							} else {

								action = touchedUIItem.GetComponent<UIItem> ().action;
								validHexes = hexGrid.prepareAction (action);
								preparingAction = true;	
							}

							collapseAll ();

							//touched UIItem has successors
						} else {

							activeItem = touchedUIItem;

							foreach (Transform source in sources) {
								if (source.GetComponent<UIItem> () != touchedUIItem.GetComponent<UIItem> ()) {
									collapse (source);							
								}
							}

							Vector3 lastPosition = touchedUIItem.transform.position; 
							
							foreach (UIItem successor in touchedUIItem.GetComponent<UIItem> ().successors) {
								successor.transform.position = lastPosition;
								successor.transform.RotateAround (gameObject.transform.position, Vector3.back, subItemsDegrees);
								successor.gameObject.SetActive (true);
								lastPosition = successor.transform.position;
							}

						}
					}

					//!touched UIItem
				} else if (touchedUIItem == null) {

					unit = hexGrid.units.getByType1 (touchedHex);

					if (unit != null) {

						if (touchedHex != previousHex) {

							collapseAll();
							Vector3 centerOfHex = GridUtility.pointToVector3 (hexGrid.layout.hexToUnit (touchedHex));
							gameObject.transform.position = centerOfHex;
							status = uiStatus.unfolding;
							previousHex = touchedHex;
						} else if (touchedHex == previousHex) {
							if (activeItem == null) {
								status = uiStatus.folding;
							} else {
								collapseAll ();
								status = uiStatus.unfolding;
							}
						} 
					} else if (unit == null) {
						collapseAll ();
					}
				}
			}
		} 

		//executing action
		else if(preparingAction && clicked) {

			touchedHex = hexGrid.GetInputHex (touchPosition);

			if (validHexes.Contains (touchedHex)) {

				hexGrid.executeAction (touchedHex);

				preparingAction = false;
				action = null;
				unit = null;
				validHexes = null;

				collapseAll ();

			} /*else if (hexGrid.units.getByType1 (touchedHex) == unit) {

				preparingAction = false;
				action = null;
				unit = null;
			}*/
		} 

		if (status == uiStatus.unfolding) {
			unfold ();			
		} else if (status == uiStatus.folding) {
			fold ();
		}

		if (preparingAction) {
			action.PrepareAction (hexGrid);
		}
	}

	public GameObject touchedObject(Vector3 touchPosition){

		Ray inputRay = Camera.main.ScreenPointToRay (touchPosition);
		RaycastHit2D hit = Physics2D.GetRayIntersection (inputRay, Mathf.Infinity, 1 << LayerMask.NameToLayer ("UI"));
		if (hit) {
			return hit.transform.gameObject;
		} else {
			return null;
		}
	}

	void unfold () {

		for (var i = 0; i <= index; i++) {
			if (sources [index].gameObject.activeSelf == false) {
				sources [index].gameObject.SetActive (true);
			}
			sources [i].position = Vector3.Lerp (sources [i].position, destinations [i].position, Time.deltaTime * animationSpeed);

			float percentagePassed = Vector3.Distance (sources [i].position, gameObject.transform.position) / Vector3.Distance (gameObject.transform.position, destinations [i].position);
			float scale = maxScale * percentagePassed;
			sources [i].localScale = new Vector3 (scale, scale);	
		}	

		if (Vector3.Distance (sources [index].position, destinations [index].position) < minDistance && index < sources.Length - 1) {
			index++;
		}

		if(properlyUnfolded()) {
			clicked = false;
			status = uiStatus.unfolded;
		}
	}

	void fold () {

		for (var i = sources.Length - 1; i >= index; i--) {

			sources [i].position = Vector3.Lerp (sources [i].position, gameObject.transform.position, Time.deltaTime * animationSpeed);

			float percentagePassed = Vector3.Distance (sources [i].position, gameObject.transform.position) / Vector3.Distance (gameObject.transform.position, destinations [i].position);
			float scale = maxScale * percentagePassed;
			sources [i].localScale = new Vector3 (scale, scale);	
		}	

		if (Vector3.Distance (sources [index].position, gameObject.transform.position) < minDistance && index > 0) {
			if (sources [index].gameObject.activeSelf == true) {
				sources [index].gameObject.SetActive (false);
			}
			index--;
		}

		if (properlyFolded()) {
			clicked = false;
			status = uiStatus.folded;
		}
	}

	void collapse (Transform source) {
		
		source.transform.position = gameObject.transform.position;
		source.gameObject.SetActive (false);
			
		if (source.GetComponent<UIItem> () != null && source.GetComponent<UIItem> ().successors.Count > 0) {
			foreach(UIItem successor in source.GetComponent<UIItem> ().successors) {
				successor.gameObject.SetActive (false);	
			}
		}

		if(source.gameObject == activeItem) {
			activeItem = null;
		}

		index--;
		clicked = false;
	}

	void collapseAll () {

		foreach (Transform source in sources) {
			collapse (source);
		}

		activeItem = null;
		index = 0;
		status = uiStatus.folded;
	}

	bool properlyFolded () {
		if (Vector3.Distance (sources [sources.Length - 1].position, gameObject.transform.position) < 0.001) {
			return true;
		} else {
			return false;
		}
	}

	bool properlyUnfolded () {
		if (Vector3.Distance (sources [sources.Length - 1].position, destinations [sources.Length - 1].position) < 0.1) {
			return true;
		} else {
			return false;
		}
	}

	/*
	 * obsolete
	 * 
	Vector3 midpoint(Vector3 v1, Vector3 v2) {
		Vector3 v3 = v1 - v2;
		float distance = Vector3.Distance (v1, v2);
		Vector3 mid = v1 + (v3 * distance / 2);
		return mid;
	}
	*/
}
