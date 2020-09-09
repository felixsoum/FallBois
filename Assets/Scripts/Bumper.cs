using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var contact = collision.contacts[0];
            var direction = collision.gameObject.transform.position - contact.point;
            direction.y = 0;
            direction.Normalize();
            direction.y = 0.1f;
            collision.gameObject.GetComponent<Player>().Push(direction * 100);
        }
    }
}
