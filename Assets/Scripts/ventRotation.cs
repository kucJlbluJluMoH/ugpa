using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ventRotation : MonoBehaviour
{


    // �������� �������� �����������
    private float rotationSpeed = 1000f;
    private void Start()
    {
        rotationSpeed = Random.Range(100, 1500);
    }
    void Update()
    {
        // ������� ��������� ������ ��� Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }


}
