using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    [SerializeField] int fallValue = -10;
    [SerializeField] LayerMask floorMask;
    [SerializeField] float groundRayDistance = 2;
    [SerializeField] float moveForce = 1000;
    [SerializeField] float maxVelocity = 100;
    [SerializeField] float jumpForce = 9000;
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject body;
    [SerializeField] MeshRenderer[] meshRenderers;
    [SerializeField] Material[] bodyMaterials;

    public static GameObject LocalPlayerInstance;


    Transform respawnPoint;
    Camera mainCamera;
    Rigidbody myRigidbody;
    static int instanceCount;
    int myID;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        int chosenMaterial = GameObject.FindGameObjectsWithTag("Player").Length - 1;// Random.Range(0, bodyMaterials.Length);

        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material = bodyMaterials[chosenMaterial];
        }
        myID = instanceCount;
        Debug.Log("count: " + instanceCount++);
        if (LocalPlayerInstance == null)
        {
            Debug.Log("set static instance");
            LocalPlayerInstance = gameObject;
        }
        gameObject.name += "ID:" + myID;
        DontDestroyOnLoad(gameObject);
    }

    internal void Push(Vector3 force)
    {
        myRigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    void Start()
    {
        mainCamera = Camera.main;
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
    }

    void Update()
    {
        //mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, transform.position + offset, Time.deltaTime * 10);

        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        float moveDelta = Time.deltaTime * moveForce;

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera != null)
        {
            mainCamera.transform.position = transform.position + offset;
        }

        if (myRigidbody.velocity.magnitude < maxVelocity)
        {
            myRigidbody.AddForce(Vector3.forward * vertical * moveDelta, ForceMode.VelocityChange);
            myRigidbody.AddForce(Vector3.right * horizontal * moveDelta, ForceMode.VelocityChange);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            var currentVelocity = myRigidbody.velocity;
            currentVelocity.y = jumpForce;
            myRigidbody.velocity = currentVelocity;
        }

        Vector3 desiredOrientation = new Vector3(horizontal, 0, vertical);
        body.transform.forward = Vector3.Lerp(body.transform.forward, desiredOrientation, Time.deltaTime * 10);

        if (transform.position.y < fallValue)
        {
            transform.position = respawnPoint.transform.position;
            myRigidbody.velocity = Vector3.zero;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundRayDistance, floorMask);
    }

}
