using UnityEngine;
public class WeaponSway : MonoBehaviour
{
    public float smoothness = 5.0f; // „ем выше значение, тем быстрее оружие будет возвращатьс€ в исходное положение
    public float swayAmount = 0.05f; // ќпредел€ет, насколько сильно оружие будет отклон€тьс€

    void Start()
    { 
    }
    void Update()
    {
        // ѕолучаем движение мыши по ос€м X и Y
        float mouseX = Input.GetAxisRaw("Mouse X") * swayAmount;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayAmount;
        // –ассчитываем новое положение оружи€
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion targetRotaion  = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation,targetRotaion,smoothness*Time.deltaTime);
    }
}
