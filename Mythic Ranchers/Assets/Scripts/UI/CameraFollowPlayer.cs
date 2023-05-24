using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public static CameraFollowPlayer instance;

    private PlayerUnit player;

    public void SetPlayerUnit(PlayerUnit playerUnit)
    {
        this.player = playerUnit;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (player)
        {
            transform.position = this.player.transform.position;
        }
    }
}
