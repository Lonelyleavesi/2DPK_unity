using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrouchHelper : MonoBehaviour
{
    /// <summary>
    /// ���µ�ʱ�������ƶ����ٶ�
    /// </summary>
    public float _moveSpeedWhenCrouch;

    /// <summary>
    /// ���º���ײ��ߴ���ٵı���
    /// </summary>
    public float _crouchReduceAmend;

    /// <summary>
    /// ��ǰ�Ƿ���
    /// </summary>
    public bool _isCrouch;


    // �������ʱ��¼�û���ǰ��ײ���Ĵ�Сƫ�ƣ��뿪����״̬ʱ�ָ�
    private Vector2 _playCollSize;
    private Vector2 _playCollOffset;

    /// <summary>
    ///  ������µ�״̬��
    ///  - �޸�player����ײ�����
    /// </summary>
    public void initCrouchState()
    {
        _isCrouch = true;
        var player = (GameObject)Variables.Object(this).Get("player");
        var playerColl = player.GetComponent<CapsuleCollider2D>();

        _playCollSize = playerColl.size;
        _playCollOffset = playerColl.offset;

        var newSize = new Vector2(_playCollSize.x, _playCollSize.y * _crouchReduceAmend);
        playerColl.size = newSize;

        var newoffset = new Vector2(_playCollOffset.x, _playCollOffset.y * _crouchReduceAmend);
        playerColl.offset = newoffset;
    }

    /// <summary>
    ///  �뿪���µ�״̬��
    ///  - �޸�player����ײ�����
    /// </summary>
    public void exitCrouchState( )
    {
        _isCrouch = false;
        var player = (GameObject)Variables.Object(this).Get("player");
        var playerColl = player.GetComponent<CapsuleCollider2D>( );


        playerColl.size =  _playCollSize;
        playerColl.offset = _playCollOffset;
    }

    // �û��ڶ���ʱ�Ĳ����������ƶ�
    public void HandleUserInput(Vector2 userInputHorizontal)
    {
        if (userInputHorizontal.x == 0) {
            return;
        }
        var player = (GameObject)Variables.Object(this).Get("player");
        var playerRB = player.GetComponent<Rigidbody2D>( );

        playerRB.velocity = new Vector2(_moveSpeedWhenCrouch * userInputHorizontal.x, playerRB.velocity.y);
    }
}
