using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform target;
    public bool is3D;

    [Header("Camera References and Values")]
    [Header("3D")]
    public ThreeDimensionalCameraMode current3DMode;
    
    [SerializeField] float height;
    [SerializeField] float distance;
    [SerializeField] float sensitivity = 5.0f;

    private float currentXRotation;
    private float currentYRotation;
    private Vector3 offset;
    
    [Header("2D")]
    public TwoDimensionalCameraMode current2DMode;
    public float camHeight;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        offset = new Vector3(0,height,-distance);
    }

    private void Update()
    {
        if (is3D)
        {
            switch (current3DMode)
            {
                case ThreeDimensionalCameraMode.None:
                    return;
                case ThreeDimensionalCameraMode.TPP:
                    TPP_Camera();
                    break;
                case ThreeDimensionalCameraMode.FPP:
                    FPP_Camera();
                    break;
                case ThreeDimensionalCameraMode.ISO:
                    //Camera.main.orthographic = true;
                    ISO_Camera();
                    break;
                case ThreeDimensionalCameraMode.ORB:
                    ORB_Camera();
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (current2DMode)
            {
                case TwoDimensionalCameraMode.None:
                    return;
                case TwoDimensionalCameraMode.TOP:
                    break;
                case TwoDimensionalCameraMode.PLAT:
                    PLAT_Camera();
                    break;
                default:
                    break;
            }
        }
    }

    #region 3D
    public void TPP_Camera()
    {
        // Rotate camera around the target based on mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        currentXRotation += mouseX;
        currentYRotation -= mouseY;
        target.rotation = Quaternion.Euler(0, currentXRotation,0);

        Quaternion cameraRotation = Quaternion.Euler (0,currentXRotation,Mathf.Clamp(currentYRotation,0,90));
        Vector3 desiredPosition = target.position + cameraRotation * offset;

        transform.position = desiredPosition;
        transform.LookAt(target);
    }

    public void FPP_Camera()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        currentXRotation += mouseX;
        currentYRotation -= mouseY;

        target.rotation = Quaternion.Euler(Mathf.Clamp(currentYRotation, -90, 90), currentXRotation, 0);
    }

    public void ORB_Camera()
    {
        // Rotate camera around the target based on mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;

        currentXRotation += mouseX;

        Quaternion cameraRotation = Quaternion.Euler(0, currentXRotation,0);
        Vector3 desiredPosition = target.position + cameraRotation * offset;

        transform.position = desiredPosition;
        transform.LookAt(target);
    }

    public void ISO_Camera()
    {
        //Vector3 newPosition = transform.position;

        //// Update the camera position to follow the target on the X and Z axes
        //newPosition.x = target.position.x;
        //newPosition.z = target.position.z;

        //// Apply the new camera position
        //transform.position = newPosition;
    }
    #endregion

    #region 2D

    public void PLAT_Camera()
    {
        Vector3 targetPosition = target.position;
        Vector3 desiredPositon = new Vector3(targetPosition.x, targetPosition.y , transform.position.z);
        transform.position = desiredPositon + new Vector3(0, camHeight, 0);
    }

    #endregion
}

public enum ThreeDimensionalCameraMode
{
    None,
    FPP,
    TPP,
    ORB,
    ISO
}

public enum TwoDimensionalCameraMode
{
    None,
    TOP,
    PLAT
}
