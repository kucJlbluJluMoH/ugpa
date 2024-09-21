using System.Collections;
using UnityEngine;
public class Spawn : MonoBehaviour
{
    // ������, ������� ����� ����������
    public GameObject prefabToSpawn;
    // �������� � �������� ����� ��������
    public float spawnInterval = 3.0f;
    private void Start()
    {
        // ��������� �������� ��� ������ ��������
        StartCoroutine(SpawnPrefab());
    }
    private IEnumerator SpawnPrefab()
    {
        while (true)
        {
            // ������� ������
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
            // ���� ��������� �������� ����� ��������� �������
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
