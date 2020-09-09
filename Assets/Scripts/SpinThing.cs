using UnityEngine;

public class SpinThing : MonoBehaviour
{
    private Rigidbody myRigidbody;

    float angle;
    [SerializeField] float spinSpeed;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        angle += spinSpeed * Time.deltaTime;
        angle %= 360;
        myRigidbody.rotation = Quaternion.Euler(0, angle, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit something");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit player");
            collision.gameObject.GetComponent<Player>().Push(Vector3.forward * 1000000);
        }
    }
}
