using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target; // ���� target;
    public Vector3 offset; // ī�޶� ������

    void Start()
    {
        
    }

    void Update()
    {
        // �� ������ ī�޶� ������
        transform.position = target.position + offset;
    }
}
