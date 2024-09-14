using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ventRotation : MonoBehaviour
{


    // Скорость вращения пропеллеров
    public float rotationSpeed = 1000f;
    void Update()
    {
        // Вращаем пропеллер вокруг оси Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }


}
