using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button hostButton; // Assign in Inspector
    public Button clientButton; // Assign in Inspector

    void Start()
    {
        hostButton.onClick.AddListener(StartAsHost);
        clientButton.onClick.AddListener(StartAsClient);
    }

    public void StartAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartAsClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}