using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpHelper : MonoBehaviour
{
  
    [Header("��Ծ���")]
    /// <summary>
    /// ����ʱ�ٶ�����ϵ������Ծ��ʱ���ʹ��ǰ��ˮƽ�ƶ��ٶȳ��Դ�ϵ��
    /// </summary>
    public float _moveSpeedAmend;

    /// <summary>
    /// ��Ծ��ʱ�������ƶ����ٶ�
    /// </summary>
    public float _moveSpeedWhenJump;


    public bool _enableMultiJump;
    public int _multiJumpMaxTimes;
    public int _multiJumpCurrTimes;
    public List<float> _multiJumpSpeeds;


    [Header("UVS���")]
    public StateMachine _machine;
    [Header("��ײ�ж����")]
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
            return;  //��ʾ�������� ���ʱ�����ж�
        }

        _isGround = Physics2D.OverlapCircle(_foot.position, 0.1f, _Ground);
        if (_isGround) {
      
            CustomEvent.Trigger(player, "jump_to_land");
        }
    }

    // �û�����Ծ�еĲ����������ƶ�
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
    /// �û�����Ծ;���ٴΰ�����Ծ�����Զ����
    /// </summary>
    public void TryMultiJump( )
    {
        if (!_enableMultiJump || _multiJumpCurrTimes >= _multiJumpMaxTimes - 1) {
            return;
        }
        var player = (GameObject)Variables.Object(this).Get("player");
        var playerRB = player.GetComponent<Rigidbody2D>();

        var jumpSpeed = 0f;
        // ���û����д��ǰ��Ծ��������Ծ�ٶȣ��������һ����Ծ�ٶ�
        if (_multiJumpCurrTimes < _multiJumpSpeeds.Count - 1) {
            jumpSpeed = _multiJumpSpeeds[_multiJumpCurrTimes];
        } else {
            jumpSpeed = _multiJumpSpeeds[_multiJumpSpeeds.Count - 1];
        }
       
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpSpeed);
        _multiJumpCurrTimes++;
    }
}
