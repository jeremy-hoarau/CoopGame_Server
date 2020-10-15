﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerMesh : MonoBehaviour
{
    public Player player;
    public GameObject meshRobot1, meshRobot2;

    private void Start()
    {
        if (player.id == 1)
        {
            meshRobot1.SetActive(true);
        }
        else
        {
            meshRobot2.SetActive(true);
        }
    }
}
