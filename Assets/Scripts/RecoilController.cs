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
    private Vector3 _currentRecoil;
    private Vector3 _targetRecoil;

    void FixedUpdate()
    {
        // Плавно возвращаем отдачу к нулю
        if (_currentRecoil.magnitude > 0.01f)
        {
            _currentRecoil = Vector3.Lerp(_currentRecoil, Vector3.zero, recoilSpeed * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Euler(_currentRecoil);
        }
    }

    // Вызываем метод при стрельбе
    public void AddRecoil(int strength)
    {
        // Вычисляем случайную величину отдачи
        float recoilXRandom = Random.Range(-recoilRandomness, recoilRandomness);
        float recoilYRandom = Random.Range(-recoilRandomness, recoilRandomness);

        // Устанавливаем целевую отдачу
        _targetRecoil = new Vector3(-strength + recoilYRandom, recoilX + recoilXRandom, recoilZ);
        _currentRecoil = _targetRecoil;
    }
}