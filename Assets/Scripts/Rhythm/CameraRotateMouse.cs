using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateMouse : MonoBehaviour {

    public float rotationTreshold = 2;
    public float rotationSpeed = 2;
    public bool unscaledTime = false;

    private Vector3 initialRotation;

    void Start () {
        initialRotation = transform.eulerAngles;
    }
	
	void Update () {
        Vector3 mousePosition = Input.mousePosition;
        float xMousPositionRatio = mousePosition.x / Screen.width;
        float yMousePositionRatio = 1 - mousePosition.y / Screen.height; // Inverse the axis

        Vector2 maxRotation = new Vector2(initialRotation.x + rotationTreshold, initialRotation.y + rotationTreshold);
        Vector2 minRotation = new Vector2(initialRotation.x - rotationTreshold, initialRotation.y - rotationTreshold);

        Vector3 newRotation = initialRotation;
        float speed = rotationSpeed * Time.deltaTime;
        newRotation.y = Mathf.LerpAngle(minRotation.y, maxRotation.y, xMousPositionRatio);
        newRotation.x = Mathf.LerpAngle(minRotation.x, maxRotation.x, yMousePositionRatio);

        transform.localEulerAngles = newRotation;
    }
}
