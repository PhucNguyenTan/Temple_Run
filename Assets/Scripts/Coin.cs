using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float _speed;
    Vector3 _rotateDirection = new Vector3(0, 1f, 0);

    private void OnEnable()
    {
        PickupAnim.OnUpdateCoinRotation += UpdateRotation;
    }

    private void OnDisable()
    {
        PickupAnim.OnUpdateCoinRotation -= UpdateRotation;

    }

    void UpdateRotation(Quaternion newRotation)
    {
        transform.localRotation = newRotation;
    }

    private void Update()
    {
        //    var currentDir = transform.localRotation.eulerAngles;
        //    var toRotateDir = currentDir + _rotateDirection * _speed * Time.deltaTime;
        //    transform.localRotation = Quaternion.Euler(toRotateDir);
    }
}
