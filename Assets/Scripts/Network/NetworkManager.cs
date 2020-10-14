using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        
        Server.Start(2, 26950);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer(int _id)
    {
        Vector3 _spawnPos = Vector3.zero;
        Quaternion _spawnRotation = Quaternion.identity;

        try
        {
            if (_id == 1)
            {
                Transform _spawnTransform = GameObject.FindWithTag("SpawnPlayer1").transform;
                _spawnPos = _spawnTransform.position;
                _spawnRotation = _spawnTransform.rotation;
            }
            else if (_id == 2)
            {
                Transform _spawnTransform = GameObject.FindWithTag("SpawnPlayer2").transform;
                _spawnPos = _spawnTransform.position;
                _spawnRotation = _spawnTransform.rotation;
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error setting up player spawn position or rotation, default values assigned: {e}");
            _spawnPos = Vector3.zero;
            _spawnRotation = Quaternion.identity;
        }

        return Instantiate(playerPrefab, _spawnPos, _spawnRotation).GetComponent<Player>();
    }
}
