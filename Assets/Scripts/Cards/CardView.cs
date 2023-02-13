using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using DefaultNamespace;
using DG.Tweening;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler,
    IDragHandler, IPointerClickHandler, IDamageable
{
    [SerializeField] private BuffParameters buffStatsParameters;
    [SerializeField] private GameObject _frontCard;
    [Space, SerializeField] private MeshRenderer _icon;
    [SerializeField] private TextMeshPro _cosText;
    [SerializeField] private TextMeshPro _nameText;
    [SerializeField] private TextMeshPro _descriptionText;
    [SerializeField] private TextMeshPro _typeText;
    [SerializeField] private TextMeshPro _damageText;
    [SerializeField] private TextMeshPro _healthText;
    [SerializeField] private int _cardId;

    private Vector3 _stepPosition = new Vector3(0, 0.5f, 0);
    private const float _scale = 1.7f;
    private Transform _startPosition;
    private Transform _endPosition;
    private CardSignalBus _cardSignalBus;
    private int _startMaxHp;
    private int _currentMaxHp;


    public List<EAbility> MyAbilities;
    public TextMeshPro NameText => _nameText;
    public int CardId => _cardId;
    public bool CanBeDragged;
    public bool CanAttack;
    public bool IsScaled;
    public bool IsTaunt;
    public int SumHpBuff = 0;
    public int Hp;
    public int SumDmgBuff = 0;
    public int DMG;
    public ECardStateType StateType { get; set; } = ECardStateType.InDeck;
    public Player Owner;
    public BuffParameters BuffStatsParameters => buffStatsParameters;
    public BattlecryParameters BattlecryParameters = new BattlecryParameters();
    public ECardUnitType MyType;

    [ContextMenu("Switch Visual")]
    public void SwitchVisual()
    {
        IsEnable = !IsEnable;
    }

    public bool IsEnable
    {
        get => _icon.enabled;
        set
        {
            _icon.enabled = value;
            _frontCard.SetActive(value);
        }
    }

    public void Init(CardSignalBus bus)
    {
        _cardSignalBus = bus;
    }

    public void Configuration(CardPropertiesData data, string description, Material icon)
    {
        _icon.sharedMaterial = icon;
        _cosText.text = data.Cost.ToString();
        _nameText.text = data.Name;
        _descriptionText.text = description;
        _typeText.text = data.Type == ECardUnitType.All ? string.Empty : data.Type.ToString();
        _damageText.text = data.Attack.ToString();
        _healthText.text = data.Health.ToString();
        _cardId = (int) data.Id;
        MyType = data.Type;
        Hp = data.Health;
        _startMaxHp = Hp;
        _currentMaxHp = _startMaxHp + SumHpBuff;
        DMG = data.Attack;
        BattlecryParameters.ID = _cardId;
    }


    public void SetIncreaseStatsParameters(BuffParameters buffParameters)
    {
        if (buffParameters == null)
        {
            buffStatsParameters = new BuffParameters();
            return;
        }

        buffStatsParameters = buffParameters;

        if (!MyAbilities.Contains(EAbility.IncreaseStats))
        {
            MyAbilities.Add(EAbility.IncreaseStats);
        }
    }

    public void RestoreHp(int value)
    {
        if (Hp + value > _currentMaxHp)
        {
            SetHealth(_currentMaxHp, 0);
            return;
        }

        SetHealth(Hp + value, 0);
    }

    public int GetCost()
    {
        return int.Parse(_cosText.text);
    }

    public int GetHealth()
    {
        Hp = int.Parse(_healthText.text);
        return Hp;
    }

    public void SetHealth(int currentHp, int hpBuff)
    {
        if (hpBuff > 0)
        {
            _healthText.transform.DOShakeScale(1, new Vector3(0, 3, 0), 10, 10f);
        }

        SumHpBuff += hpBuff;
        _healthText.text = currentHp.ToString();
        Hp = Int32.Parse(_healthText.text);
        UpdateCurrentMaxHp();

        _healthText.color = SumHpBuff > 0 ? Color.green : Color.white;

        if (currentHp == 0)
        {
            Owner.KillCardFromTable(this);
        }
    }

    private void UpdateCurrentMaxHp()
    {
        _currentMaxHp = _startMaxHp + SumHpBuff;
    }


    public void UpScaleCard()
    {
        if (IsScaled)
        {
            return;
        }

        transform.localPosition += _stepPosition;
        transform.localScale *= _scale;
        IsScaled = true;
    }

    public void DownScaleCard()
    {
        if (!IsScaled)
        {
            return;
        }

        transform.localPosition -= _stepPosition;
        transform.localScale /= _scale;
        IsScaled = false;
    }

    private void NormalizedScale()
    {
        if (IsScaled)
        {
            DownScaleCard();
        }
    }

    public void MoveAnimation(Transform endPosition)
    {
        NormalizedScale();
        transform.DOMove(endPosition.position, 1f);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _cardSignalBus.FirePointerOn(new CardPointerSignal(this));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _cardSignalBus.FirePointerOff(new CardPointerSignal(this));
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        _cardSignalBus.FireDragEnd(new CardDragSignal(this));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (StateType != ECardStateType.InHand)
        {
            return;
        }

        _cardSignalBus.FireDragging(new CardDragSignal(this));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _cardSignalBus.CardClickFire(new CardClickSignal(this));
    }

    public int GetDamage()
    {
        DMG = int.Parse(_damageText.text);
        return DMG;
    }


    public void SetDamage(int currentDmg, int dmgBuff)
    {
        SumDmgBuff += dmgBuff;
        if (dmgBuff > 0)
        {
            _damageText.transform.DOShakeScale(1, new Vector3(0, 3, 0), 10, 10f);
        }

        _damageText.text = currentDmg.ToString();
        DMG = Int32.Parse(_damageText.text);
        _damageText.color = SumDmgBuff > 0 ? Color.green : Color.white;
    }

    public void ApplyDamage(int damage)
    {
        var health = GetHealth();
        if (health - damage <= 0)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }

        if (damage != 0)
        {
            transform.DOShakeRotation(1f, new Vector3(0, 5, 0), 10, 10f).OnComplete(() => SetHealth(health, 0));
        }
    }

    public void SetCoolDownAttack(bool value)
    {
        CanAttack = !value;
    }
}