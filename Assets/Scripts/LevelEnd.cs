using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] [Range(1, 2)] private int nbPlayersRequired = 2;

    private bool player1Arrived, player2Arrived;
    private void OnTriggerEnter(Collider other)
    {
        if (nbPlayersRequired == 1 && other.tag.Contains("Player"))
        {
            LoadNextScene();
        }
        else if(nbPlayersRequired == 2)
        {
            if (other.CompareTag("Player1"))
            {
                player1Arrived = true;
            }
            else if(other.CompareTag("Player2"))
            {
                player2Arrived = true;
            }
            
            if (player1Arrived && player2Arrived)
            {
                LoadNextScene();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(nbPlayersRequired == 2)
        {
            if (other.CompareTag("Player1"))
            {
                player1Arrived = false;
            }
            else if(other.CompareTag("Player2"))
            {
                player2Arrived = false;
            }
        }
    }

    private void LoadNextScene()
    {
        int _nextSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (SceneManager.sceneCountInBuildSettings - 1 < (_nextSceneIndex - 1)) //check if the scene can be loaded
        {
            Debug.Log($"Cannot load the next scene, this index might be wrong: -server_index({_nextSceneIndex}) -client_index({_nextSceneIndex + 1})");
            return;
        }
        
        if(Server.clients.ContainsKey(1) && Server.clients[1].tcp.socket != null)
            GameObject.FindWithTag("Player1").GetComponent<PlayerGrabObject>().DestroyGrabbedObject();
        if(Server.clients.ContainsKey(2) && Server.clients[2].tcp.socket != null)
            GameObject.FindWithTag("Player2").GetComponent<PlayerGrabObject>().DestroyGrabbedObject();
        
        ServerSend.LoadScene(_nextSceneIndex + 1);    // +1 for the client because it has a menu scene that the server doesn't
        SceneManager.LoadScene(_nextSceneIndex);
    }
}
