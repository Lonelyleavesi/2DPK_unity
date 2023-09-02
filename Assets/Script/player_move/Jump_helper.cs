using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jump_helper : MonoBehaviour
{
    public StateMachine machine;
    public Transform foot;
    public bool isGround;
    public LayerMask Ground;


    public void TriggerJump()
    {
        var player = (GameObject)Variables.Object(this).Get("player");
        var speed = (int)Variables.Object(this).Get("move_speed");

        var playerRB = player.GetComponent<Rigidbody2D>();
        playerRB.velocity = new Vector2(playerRB.velocity.x, speed);
    }

    public void checkOnGround ()
    {
        isGround = Physics2D.OverlapCircle(foot.position, 0.1f, Ground);
        Debug.Log("is ground " + isGround);
        //if (isGround ) {
        //    EventBus.Trigger("jump_to_land");
        //}
    }
}
