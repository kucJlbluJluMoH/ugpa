using UnityEngine;

public class RecoilController : MonoBehaviour
{
    // ��������� ������
    [Header("Recoil Settings")]
    public float recoilX = 10f;          // ���� ������ �� �����������
    public float recoilY = 5f;           // ���� ������ �� ���������
    public float recoilZ = 2f;           // ���� �������� �� ��� Z
    public float recoilSpeed = 5f;       // �������� �������� ������
    public float recoilRandomness = 2f;   // ����������� ������

    // ��������� ����������
    private Vector3 _currentRecoil;
    private Vector3 _targetRecoil;

    void FixedUpdate()
    {
        // ������ ���������� ������ � ����
        if (_currentRecoil.magnitude > 0.01f)
        {
            _currentRecoil = Vector3.Lerp(_currentRecoil, Vector3.zero, recoilSpeed * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Euler(_currentRecoil);
        }
    }

    // �������� ����� ��� ��������
    public void AddRecoil(int strength)
    {
        // ��������� ��������� �������� ������
        float recoilXRandom = Random.Range(-recoilRandomness, recoilRandomness);
        float recoilYRandom = Random.Range(-recoilRandomness, recoilRandomness);

        // ������������� ������� ������
        _targetRecoil = new Vector3(-strength + recoilYRandom, recoilX + recoilXRandom, recoilZ);
        _currentRecoil = _targetRecoil;
    }
}