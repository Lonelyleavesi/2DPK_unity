using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start( )
    {
        _isCrouch = true;
        _circleTween = null;
        _circleOutlineImage = _circleOutline.GetComponent<Image>( );
        this.ResetCircleImage( );
    }

    /// <summary>
    ///  ������µ�״̬��
    ///  - �޸�player����ײ�����
    /// </summary>
    public void initCrouchState()
    {
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
        _circleOutline.SetActive(false);

        var player = (GameObject)Variables.Object(this).Get("player");
        var playerColl = player.GetComponent<CapsuleCollider2D>( );

        playerColl.size =  _playCollSize;
        playerColl.offset = _playCollOffset;

        _circleTween.Restart( );
        _circleTween.Pause( );
        _circleColorTween.Restart( );
        _circleColorTween.Pause( );

        this.ResetCircleImage();
    }

    // �û��ڶ���ʱ�Ĳ����������ƶ�
    public void HandleUserInput(Vector2 userInputHorizontal)
    {
        if (userInputHorizontal.x == 0 || _readySuperJump) {
            return;
        }
        var player = (GameObject)Variables.Object(this).Get("player");
        var playerRB = player.GetComponent<Rigidbody2D>( );

        playerRB.velocity = new Vector2(_moveSpeedWhenCrouch * userInputHorizontal.x, playerRB.velocity.y);
    }

    [Header("���������")]
    [SerializeField] private Color _initCircleColor;
    [SerializeField] private Color _saveEnergyColor;
    [SerializeField] private float _tweenDuration;  // ���ȶ������ʱ��
    private Tween _circleTween;   // תȦ���ȶ���
    private Tween _circleColorTween;  // ��ɫ���ȶ���
    [SerializeField] private GameObject _circleOutline;
    private Image _circleOutlineImage;

    private bool _readySuperJump;

    public int _super_jump_speed_incremental_num; // �����������������
    public void readySuperJump()
    {
        _circleOutline.SetActive(true);
        if (!_circleTween.playedOnce && !_circleColorTween.playedOnce)
        {
            _circleTween.Restart();
            _circleColorTween.Restart();
            _circleTween.Play( );
            _circleColorTween.Play( );
        }
    }

    void ResetCircleImage( )
    {
        _circleOutlineImage.color = _initCircleColor;
        _circleOutlineImage.fillAmount = 0;
        _circleTween = _circleOutlineImage.DOFillAmount(1, _tweenDuration).SetEase(Ease.Linear).Pause( );
        _circleColorTween = _circleOutlineImage.DOColor(Color.red, _tweenDuration).SetEase(Ease.Linear).Pause( );
    }

    public void triggerSuperJump( )
    {
        var superJumpReadyRatio = 1f * _circleOutlineImage.fillAmount;
        Variables.Object(this).Set("super_jump_speed_incremental_ratio", superJumpReadyRatio);
        var player = (GameObject)Variables.Object(this).Get("player");
        CustomEvent.Trigger(player, "crouch_super_jump_event");
    }



}
