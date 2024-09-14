using UnityEngine;
public class CameraController : MonoBehaviour
{
    public LayerMask ignoreLayer;
    public float raycastDistance = 3f;
    public bool IsPaused = false;
    public float mouseSensitivity;
    public Transform orientation;
    public Transform syncCam;
    [Header("Recoil")]
    [HideInInspector]
    public float recoilStrength = 2f;  // Сила отдачи
    public float recoilRecoverySpeed = 5f;  // Скорость восстановления после отдачи
    private Vector2 currentRecoil = Vector2.zero;  // Текущая отдача
    float xRotation;
    float yRotation;
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

        if (!IsPaused)
        {


            // Вычисляем разницу во времени между кадрами
            float deltaTime = Time.deltaTime;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Применяем разницу во времени к вращению
            yRotation += mouseX * deltaTime + currentRecoil.x;
            xRotation -= mouseY * deltaTime + currentRecoil.y;

            // Применяем отдачу с учетом deltaTime
            //xRotation -= currentRecoil.y * deltaTime;
            //yRotation += currentRecoil.x * deltaTime;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);

            // Применяем deltaTime к Lerp для плавного движения камеры
            transform.position = Vector3.Lerp(
                transform.position,
                syncCam.position,
                deltaTime * 200
            );

            // Восстановление камеры после отдачи с учетом deltaTime
            currentRecoil = Vector2.Lerp(currentRecoil, Vector2.zero, deltaTime * recoilRecoverySpeed);
        }
    }
    public void ApplyRecoil()
    {
        currentRecoil += new Vector2(
            Random.Range(-recoilStrength / 2, recoilStrength / 2), // Случайное отклонение по горизонтали
            recoilStrength // Основная отдача по вертикали
        );
    }
}
