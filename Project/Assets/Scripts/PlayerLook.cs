using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] Camera cam;

    [SerializeField] float lookSensitivity;
    [SerializeField] float smoothing;
    Vector2 currentLookingPosition;
    Vector2 smoothedVelocity;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (Input.GetMouseButtonDown(0))
                Cursor.lockState = CursorLockMode.Locked;

            cam.transform.localRotation = Quaternion.AngleAxis(-currentLookingPosition.y, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(currentLookingPosition.x, transform.up);
            return;
        }

        RotateCamera();
    }

    void RotateCamera()
    {
        Vector2 inputValues = new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        inputValues = Vector2.Scale(inputValues, new Vector2(smoothing * lookSensitivity, smoothing * lookSensitivity));

        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1 / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1 / smoothing);

        currentLookingPosition += smoothedVelocity;

        cam.transform.localRotation = Quaternion.AngleAxis(-currentLookingPosition.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(currentLookingPosition.x, transform.up);
    }
}
