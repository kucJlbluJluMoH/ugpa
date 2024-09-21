using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentRotation : MonoBehaviour
{


    // �������� �������� �����������
    private float _rotationSpeed = 1000f;
    private void Start()
    {
        _rotationSpeed = Random.Range(100, 1500);
    }
    void Update()
    {
        // ������� ��������� ������ ��� Y
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }


}
