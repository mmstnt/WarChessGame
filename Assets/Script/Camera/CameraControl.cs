using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraControl : MonoBehaviour
{
    public static CameraControl instance;
    [Header("相機")]
    public Transform followTransform;
    public Transform cameraTeansform;

    [Header("參數")]
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTeansform.localPosition;
    }

    private void Update()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
        else
        { 
            handleMovementInput();
        }

    }

    private void handleMouseInput() 
    {
        
    }

    private void handleMovementInput() 
    {
        Vector2 movementInput = GameEventManager.instance.playerInputControl.Camera.Movement.ReadValue<Vector2>();
        float rotationInput = GameEventManager.instance.playerInputControl.Camera.Rotation.ReadValue<float>();
        float zoomInput = GameEventManager.instance.playerInputControl.Camera.Zoom.ReadValue<float>();

        newPosition += (transform.forward * movementInput.y * movementSpeed * Time.deltaTime);
        newPosition += (transform.right * movementInput.x * movementSpeed * Time.deltaTime);
        newRotation *= Quaternion.Euler(Vector3.up * rotationAmount * rotationInput * Time.deltaTime);
        newZoom += zoomAmount * zoomInput * Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTeansform.localPosition = Vector3.Lerp(cameraTeansform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
