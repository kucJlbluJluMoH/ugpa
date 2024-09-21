using UnityEngine;
public class WeaponSway : MonoBehaviour
{
    public float smoothness = 5.0f; // ��� ���� ��������, ��� ������� ������ ����� ������������ � �������� ���������
    public float swayAmount = 0.05f; // ����������, ��������� ������ ������ ����� �����������
    
    private CameraController _cameraController;
    private MiniGamesSwitcher _miniGamesSwitcher;
    void Start()
    {
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        _miniGamesSwitcher = GameObject.Find("MiniGamesCanvas").GetComponent<MiniGamesSwitcher>();
    }
    void Update()
    {
        if (!_cameraController.isPaused || !_miniGamesSwitcher.isInGame )
        {
            // �������� �������� ���� �� ���� X � Y
            float mouseX = Input.GetAxisRaw("Mouse X") * swayAmount;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayAmount;
            // ������������ ����� ��������� ������
            Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
            Quaternion targetRotaion = rotationX * rotationY;

            transform.localRotation =
                Quaternion.Slerp(transform.localRotation, targetRotaion, smoothness * Time.deltaTime);

        }
    }
}
