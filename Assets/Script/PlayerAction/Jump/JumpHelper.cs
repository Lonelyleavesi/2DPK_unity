using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpHelper : MonoBehaviour
{
  
    [Header("跳跃相关")]
    /// <summary>
    /// 起跳时速度修正系数，跳跃的时候会使当前的水平移动速度乘以此系数
    /// </summary>
    public float _moveSpeedAmend;

    /// <summary>
    /// 跳跃的时候左右移动的速度
    /// </summary>
    public float _moveSpeedWhenJump;


    public bool _enableMultiJump;
    public int _multiJumpMaxTimes;
    public int _multiJumpCurrTimes;
    public List<float> _multiJumpSpeeds;


    [Header("UVS相关")]
    public StateMachine _machine;
    [Header("碰撞判断相关")]
    public Transform _foot;
    public bool _isGround;
    public LayerMask _Ground;



    public void TriggerJump()
    {
        _isGround = false;
        _multiJumpCurrTimes = 0;

        var player = (GameObject)Variables.Object(this).Get("player");
        var playerRB = player.GetComponent<Rigidbody2D>();

        var speedX = playerRB.velocity.x;
        playerRB.velocity = new Vector2(speedX * _moveSpeedAmend, _multiJumpSpeeds[_multiJumpCurrTimes]);
    }

    public void CheckOnGround ()
    {
        var player = (GameObject)Variables.Object(this).Get("player");
        var playerRB = player.GetComponent<Rigidbody2D>();
        if (playerRB.velocity.y > 0) {
            return;  //表示还在上升 这个时候不做判断
        }

        _isGround = Physics2D.OverlapCircle(_foot.position, 0.1f, _Ground);
        if (_isGround) {
      
            CustomEvent.Trigger(player, "jump_to_land");
        }
    }

    // 用户在跳跃中的操作，左右移动
    public void HandleUserInput(Vector2 userInputHorizontal)
    {
        if (userInputHorizontal.x == 0) {
            return;
        }
        var player = (GameObject)Variables.Object(this).Get("player");
        var playerRB = player.GetComponent<Rigidbody2D>();

        playerRB.velocity = new Vector2(_moveSpeedWhenJump * userInputHorizontal.x, playerRB.velocity.y);
    }

    /// <summary>
    /// 用户在跳跃途中再次按下跳跃键尝试多段跳
    /// </summary>
    public void TryMultiJump( )
    {
        if (!_enableMultiJump || _multiJumpCurrTimes >= _multiJumpMaxTimes - 1) {
            return;
        }
        var player = (GameObject)Variables.Object(this).Get("player");
        var playerRB = player.GetComponent<Rigidbody2D>();

        var jumpSpeed = 0f;
        // 如果没有填写当前跳跃次数的跳跃速度，就用最后一个跳跃速度
        if (_multiJumpCurrTimes < _multiJumpSpeeds.Count - 1) {
            jumpSpeed = _multiJumpSpeeds[_multiJumpCurrTimes];
        } else {
            jumpSpeed = _multiJumpSpeeds[_multiJumpSpeeds.Count - 1];
        }
       
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpSpeed);
        _multiJumpCurrTimes++;
    }
}
