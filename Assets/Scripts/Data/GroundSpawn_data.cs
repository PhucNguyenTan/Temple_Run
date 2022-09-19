using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawn_data : MonoBehaviour
{
    [SerializeField] Ground_data _startGround;
    [SerializeField] List<Ground_data> _easyGrounds;
    [SerializeField] List<Ground_data> _normalGrounds;
    [SerializeField] List<Ground_data> _hardGrounds;
    [SerializeField] float _speedPerLevel;
}
