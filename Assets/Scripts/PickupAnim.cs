using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupAnim : MonoBehaviour
{
    public static UnityAction<Quaternion> OnUpdateCoinRotation;

    [SerializeField] float _speed;
    [SerializeField] Vector3 _direction;
    Vector3 _previousRotV3;



    private void Update()
    {
        if(_previousRotV3 == null)
        {
            _previousRotV3 = Vector3.zero;
        }
        var toRotate = _previousRotV3 + _direction * _speed * Time.deltaTime;
        var sendRotation = Quaternion.Euler(toRotate);
        _previousRotV3 = toRotate;
        OnUpdateCoinRotation?.Invoke(sendRotation);
    }
}
