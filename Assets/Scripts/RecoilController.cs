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
    private Vector3 currentRecoil;
    private Vector3 targetRecoil;

    void FixedUpdate()
    {
        // ������ ���������� ������ � ����
        if (currentRecoil.magnitude > 0.01f)
        {
            currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, recoilSpeed * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Euler(currentRecoil);
        }
    }

    // �������� ����� ��� ��������
    public void AddRecoil(int Strength)
    {
        // ��������� ��������� �������� ������
        float recoilXRandom = Random.Range(-recoilRandomness, recoilRandomness);
        float recoilYRandom = Random.Range(-recoilRandomness, recoilRandomness);

        // ������������� ������� ������
        targetRecoil = new Vector3(-Strength + recoilYRandom, recoilX + recoilXRandom, recoilZ);
        currentRecoil = targetRecoil;
    }
}