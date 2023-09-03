using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CrouchHelper : MonoBehaviour
{
    /// <summary>
    /// 蹲下的时候左右移动的速度
    /// </summary>
    public float _moveSpeedWhenCrouch;

    /// <summary>
    /// 蹲下后碰撞体尺寸减少的比例
    /// </summary>
    public float _crouchReduceAmend;

    /// <summary>
    /// 当前是否处于
    /// </summary>
    public bool _isCrouch;


    // 进入蹲下时记录用户当前碰撞器的大小偏移，离开蹲下状态时恢复
    private Vector2 _playCollSize;
    private Vector2 _playCollOffset;

    /// <summary>
    ///  进入蹲下的状态：
    ///  - 修改player的碰撞器体积
    /// </summary>
    public void initCrouchState()
    {
        _isCrouch = true;
        _colorTween = null;

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
    ///  离开蹲下的状态：
    ///  - 修改player的碰撞器体积
    /// </summary>
    public void exitCrouchState( )
    {
        _isCrouch = false;
        var player = (GameObject)Variables.Object(this).Get("player");
        var playerColl = player.GetComponent<CapsuleCollider2D>( );


        playerColl.size =  _playCollSize;
        playerColl.offset = _playCollOffset;
    }

    // 用户在蹲下时的操作，左右移动
    public void HandleUserInput(Vector2 userInputHorizontal)
    {
        if (userInputHorizontal.x == 0 || _readySuperJump) {
            return;
        }
        var player = (GameObject)Variables.Object(this).Get("player");
        var playerRB = player.GetComponent<Rigidbody2D>( );

        playerRB.velocity = new Vector2(_moveSpeedWhenCrouch * userInputHorizontal.x, playerRB.velocity.y);
    }

    [Header("超级跳相关")]
    public Color _saveEnergyColor;
    public float _saveMaxTime;
    private Tween _colorTween;
    private bool _readySuperJump;
   
    public void readySuperJump()
    {
        var player = (GameObject)Variables.Object(this).Get("player");
        Material mat = player.GetComponent<Renderer>( ).material;
        if (_colorTween == null) {
            _colorTween = mat.DOColor(_saveEnergyColor, _saveMaxTime);
        } 
    }
}
