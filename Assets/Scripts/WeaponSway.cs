using UnityEngine;
public class WeaponSway : MonoBehaviour
{
    public float smoothness = 5.0f; // Чем выше значение, тем быстрее оружие будет возвращаться в исходное положение
    public float swayAmount = 0.05f; // Определяет, насколько сильно оружие будет отклоняться
    
    private CameraController _cameraController => CameraController.Instance;
    private MiniGamesSwitcher _miniGamesSwitcher => MiniGamesSwitcher.Instance;

    void Update()
    {
        if (!_cameraController.isPaused || !_miniGamesSwitcher.isInGame)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayAmount;

            float mouseY = Input.GetAxisRaw("Mouse Y") * swayAmount;
            // Рассчитываем новое положение оружия
            Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
            Quaternion targetRotaion = rotationX * rotationY;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotaion, smoothness * Time.deltaTime);
        }
    }
}
