using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform cube;

    public float offset;

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");

            rotY += mouseX * mouseSensitivity * Time.deltaTime;
            rotX += mouseY * mouseSensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

            transform.rotation = Quaternion.Euler(rotX, rotY, 0.0f);

            transform.position = cube.position - transform.forward * offset;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnValidate()
    {
        transform.position = cube.position - transform.forward * offset;
    }
}