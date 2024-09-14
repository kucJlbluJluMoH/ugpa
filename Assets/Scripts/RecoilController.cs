using UnityEngine;

public class RecoilController : MonoBehaviour
{
    // Настройки отдачи
    [Header("Recoil Settings")]
    public float recoilX = 10f;          // Сила отдачи по горизонтали
    public float recoilY = 5f;           // Сила отдачи по вертикали
    public float recoilZ = 2f;           // Сила вращения по оси Z
    public float recoilSpeed = 5f;       // Скорость возврата отдачи
    public float recoilRandomness = 2f;   // Случайность отдачи

    // Приватные переменные
    private Vector3 currentRecoil;
    private Vector3 targetRecoil;

    void FixedUpdate()
    {
        // Плавно возвращаем отдачу к нулю
        if (currentRecoil.magnitude > 0.01f)
        {
            currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, recoilSpeed * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Euler(currentRecoil);
        }
    }

    // Вызываем метод при стрельбе
    public void AddRecoil(int Strength)
    {
        // Вычисляем случайную величину отдачи
        float recoilXRandom = Random.Range(-recoilRandomness, recoilRandomness);
        float recoilYRandom = Random.Range(-recoilRandomness, recoilRandomness);

        // Устанавливаем целевую отдачу
        targetRecoil = new Vector3(-Strength + recoilYRandom, recoilX + recoilXRandom, recoilZ);
        currentRecoil = targetRecoil;
    }
}