using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    bool used = false;

    private void OnTriggerEnter(Collider other)
    {

        string test = "X" + other.gameObject.tag + "Y";
        if (!used)
        {
            if (other.CompareTag("player_main"))
            {
                Player player = other.gameObject.GetComponent<Player>();
                player.TakeDamage();
                used = true;
                Destroy(this.gameObject);
            }
        }
    }
}
