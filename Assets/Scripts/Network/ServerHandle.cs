using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();
            
        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        Server.clients[_fromClient].SendIntoGame(_username);
        
        if (Server.clients[1].tcp.socket != null && Server.clients[2].tcp.socket != null)
        {
            Server.PlayersStopWaiting();
        }
        else
        {
            ServerSend.WaitingPlayers();
        }
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();
        Vector3 _cameraPosition = _packet.ReadVector3();

        Server.clients[_fromClient].player.SetMovementInput(_inputs, _rotation, _cameraPosition);
    }
    
    public static void PlayerInputs(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        
        Server.clients[_fromClient].player.SetInput(_inputs);
    }

    public static void LoadScene(int _fromClient, Packet _packet)
    {
        int _sceneIndex = _packet.ReadInt();

        if (SceneManager.sceneCountInBuildSettings - 1 < _sceneIndex - 1) //check if the scene can be loaded
        {
            Debug.Log($"Cannot load the scene, this index might be wrong: -server_index({_sceneIndex-1}) -client_index({_sceneIndex})");
            return;
        }
        
        ServerSend.LoadScene(_sceneIndex);
        SceneManager.LoadScene(_sceneIndex - 1);
    }
}
