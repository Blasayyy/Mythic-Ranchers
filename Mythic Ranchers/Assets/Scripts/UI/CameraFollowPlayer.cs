using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    public static CameraFollowPlayer instance;
    private Transform target;
    public bool isSet;

    private PlayerUnit player;

    public void SetPlayerUnit(PlayerUnit playerUnit)
    {
        this.player = playerUnit;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        isSet = false;
        target = null;
    }

    public void SetCameraFollowPlayer(Transform player)
    {
        this.target = player;
    }

    void LateUpdate()
    {
        if (!isSet)
        {
            if (target != null)
            {
                SetCameraFollowPlayer(player.transform);
                isSet = true;
            }

        }

        if (target != null)
        {
            transform.position = target.position;
        }
    }
}
