using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour {

	public float scrollSpeed;
	public float zoomSpeed;
	public float clickTime;
	public UIHandler ui;

	private Vector3 scrollStartPosition;
	private Vector3 cameraStartPosition;
	private float cameraStartSize;
	private float startDistance;
	private Matrix4x4 cameraToWorldMatrix;
	private bool zooming;
	private bool scrolling;
	private float time;
	private Vector3 mouseStartPosition;

	void Start() {
		zooming = false;
	}

	void Update () {


		if (Input.touchCount == 0 && !Input.GetMouseButton(0)) {
			scrolling = false;
			zooming = false;
		}

		//mouse
		if (Input.GetMouseButton(0) && !zooming) {

			if (Input.GetMouseButtonDown(0)) {

				scrolling = false;

				time = clickTime;

				mouseStartPosition = Input.mousePosition;

				cameraStartPosition = gameObject.transform.position;

				cameraToWorldMatrix = Camera.main.cameraToWorldMatrix;

				scrollStartPosition = cameraToWorldMatrix.MultiplyPoint (Input.mousePosition);

			} else if(scrolling) {

				Vector3 currentPosition = cameraToWorldMatrix.MultiplyPoint (Input.mousePosition);

				Vector3 movement = scrollStartPosition - currentPosition;

				float scrollFactor = Camera.main.orthographicSize * (scrollSpeed / 100);

				gameObject.transform.position = new Vector3 (movement.x * scrollFactor, movement.y * scrollFactor) + cameraStartPosition;
			}

			if (time < 0 || Vector3.Distance(mouseStartPosition, Input.mousePosition) >= 100) {
				scrolling = true;
			}

			time -= Time.deltaTime;
		}

		if (!scrolling && Input.GetMouseButtonUp(0) && time >= 0) {

				ui.touchPosition = Input.mousePosition;
				ui.clicked = true;
		}

		//touch
		if (Input.touchCount == 1 && !zooming) {

			if (Input.GetTouch (0).phase == TouchPhase.Began) {

				scrolling = false;

				time = clickTime;

				cameraStartPosition = gameObject.transform.position;

				cameraToWorldMatrix = Camera.main.cameraToWorldMatrix;

				scrollStartPosition = cameraToWorldMatrix.MultiplyPoint (Input.GetTouch (0).position);

			} else if(scrolling) {

				Vector3 currentPosition = cameraToWorldMatrix.MultiplyPoint (Input.GetTouch (0).position);

				Vector3 movement = scrollStartPosition - currentPosition;

				float scrollFactor = Camera.main.orthographicSize * (scrollSpeed / 100);

				gameObject.transform.position = new Vector3 (movement.x * scrollFactor, movement.y * scrollFactor) + cameraStartPosition;
			}

			if (!scrolling && Input.GetTouch (0).phase == TouchPhase.Ended && time >= 0) {

				ui.touchPosition = Input.GetTouch(0).position;
				ui.clicked = true;
			}

			if (time < 0 || Input.GetTouch (0).phase == TouchPhase.Moved) {
				scrolling = true;
			}

			time -= Time.deltaTime;
		}

		//zooming
		if (Input.touchCount == 2) {

			if (Input.GetTouch (1).phase == TouchPhase.Began) {

				zooming = true;

				cameraStartSize = Camera.main.orthographicSize;

				startDistance = Vector3.Distance (Input.GetTouch (0).position, Input.GetTouch (1).position);

			} else {

				float currentDistance = Vector3.Distance (Input.GetTouch (0).position, Input.GetTouch (1).position);

				float zoom = startDistance - currentDistance;

				Camera.main.orthographicSize = Mathf.Max(cameraStartSize + zoom / 10 * zoomSpeed, 1);
			}
		}
	}
}
