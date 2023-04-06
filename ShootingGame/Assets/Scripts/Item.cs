using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float moveSpeed;

    void Update()
    {
        transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime));
        transform.position += -transform.up * (moveSpeed * Time.deltaTime);
    }
}
