using System.Collections;
using UnityEngine;
public class Spawn : MonoBehaviour
{
    // Префаб, который будет спавниться
    public GameObject prefabToSpawn;
    // Интервал в секундах между спавнами
    public float spawnInterval = 3.0f;
    private void Start()
    {
        // Запускаем корутину для спавна префабов
        StartCoroutine(SpawnPrefab());
    }
    private IEnumerator SpawnPrefab()
    {
        while (true)
        {
            // Спавним префаб
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
            // Ждем указанный интервал перед следующим спавном
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
