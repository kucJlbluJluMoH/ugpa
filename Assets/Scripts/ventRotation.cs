using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ventRotation : MonoBehaviour
{


    // �������� �������� �����������
    public float rotationSpeed = 1000f;
    void Update()
    {
        // ������� ��������� ������ ��� Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }


}
