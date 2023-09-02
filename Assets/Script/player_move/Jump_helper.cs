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
    public int jumpSpeed;


    public void TriggerJump()
    {
        isGround = false;
        var player = (GameObject)Variables.Object(this).Get("player");
        var playerRB = player.GetComponent<Rigidbody2D>();

        var speedX = playerRB.velocity.x;
        playerRB.velocity = new Vector2(speedX / 2, jumpSpeed);
    }

    public void checkOnGround ()
    {
        var player = (GameObject)Variables.Object(this).Get("player");
        var playerRB = player.GetComponent<Rigidbody2D>();
        if (playerRB.velocity.y > 0) {
            return;  //表示还在上升 这个时候不做判断
        }

        isGround = Physics2D.OverlapCircle(foot.position, 0.1f, Ground);
        if (isGround) {
      
            CustomEvent.Trigger(player, "jump_to_land");
        }
    }
}
