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
    public float recoilStrength = 2f;  // Сила отдачи
    public float recoilRecoverySpeed = 5f;  // Скорость восстановления после отдачи
    private Vector2 _currentRecoil = Vector2.zero;  // Текущая отдача
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
            // Вычисляем разницу во времени между кадрами
            float deltaTime = Time.deltaTime;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            mouseX += _currentRecoil.x;
            mouseY += _currentRecoil.y;
            // Применяем разницу во времени к вращению
            _yRotation += mouseX * deltaTime;
            _xRotation -= mouseY * deltaTime;

            // Применяем отдачу с учетом deltaTime
            //xRotation -= currentRecoil.y * deltaTime;
            //yRotation += currentRecoil.x * deltaTime;

            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, _yRotation, 0);

            // Применяем deltaTime к Lerp для плавного движения камеры
            transform.position = Vector3.Lerp(
                transform.position,
                syncCam.position,
                deltaTime * 200
            );

            // Восстановление камеры после отдачи с учетом deltaTime
            _currentRecoil = Vector2.Lerp(_currentRecoil, Vector2.zero, deltaTime * recoilRecoverySpeed);
        }
    }
    public void ApplyRecoil()
    {
        _currentRecoil += new Vector2(
            Random.Range(-recoilStrength / 2, recoilStrength / 2), // Случайное отклонение по горизонтали
            recoilStrength // Основная отдача по вертикали
        );
    }
}
