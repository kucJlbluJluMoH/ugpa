using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    
    public LayerMask ignoreLayer;
    public float raycastDistance = 3f;
    [FormerlySerializedAs("IsPaused")] public bool isPaused = false;
    public float mouseSensitivity;
    public Transform orientation;
    public Transform syncCam;
    [Header("Recoil")]
    [HideInInspector]
    public float recoilStrength = 2f;  // ���� ������
    public float recoilRecoverySpeed = 5f;  // �������� �������������� ����� ������
    private Vector2 _currentRecoil = Vector2.zero;  // ������� ������
    float _xRotation;
    float _yRotation;
    public void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;
    }
    public void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
    }
    private void Start()
    {
        LockCursor();
    }
    private void Update()
    {

        if (!isPaused)
        {
            // ��������� ������� �� ������� ����� �������
            float deltaTime = Time.deltaTime;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            mouseX += _currentRecoil.x;
            mouseY += _currentRecoil.y;
            // ��������� ������� �� ������� � ��������
            _yRotation += mouseX * deltaTime;
            _xRotation -= mouseY * deltaTime;

            // ��������� ������ � ������ deltaTime
            //xRotation -= currentRecoil.y * deltaTime;
            //yRotation += currentRecoil.x * deltaTime;

            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, _yRotation, 0);

            // ��������� deltaTime � Lerp ��� �������� �������� ������
            transform.position = Vector3.Lerp(
                transform.position,
                syncCam.position,
                deltaTime * 200
            );

            // �������������� ������ ����� ������ � ������ deltaTime
            _currentRecoil = Vector2.Lerp(_currentRecoil, Vector2.zero, deltaTime * recoilRecoverySpeed);
        }
    }
    public void ApplyRecoil()
    {
        _currentRecoil += new Vector2(
            Random.Range(-recoilStrength / 2, recoilStrength / 2), // ��������� ���������� �� �����������
            recoilStrength // �������� ������ �� ���������
        );
    }
}
