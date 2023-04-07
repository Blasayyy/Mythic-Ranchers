using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{

    private Rigidbody2D rigidBody;
    private Vector2 movement;

    [SerializeField]
    private float speed = 4;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        rigidBody.velocity = movement * speed;
        rigidBody.velocity = rigidBody.velocity.normalized * speed;
    }


}
