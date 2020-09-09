using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;

    Transform respawnPoint;

    void Start()
    {
        if (respawnPoint == null)
        {
            respawnPoint = GameObject.FindGameObjectWithTag("Respawn").gameObject.transform;
        }

        if (Player.LocalPlayerInstance == null)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, respawnPoint.position, Quaternion.identity, 0); 
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("someone joined the room");
        if (PhotonNetwork.IsMasterClient)
        {
            LoadRoom();
        }
    }

    void LoadRoom()
    {
        PhotonNetwork.LoadLevel("Level1");
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
