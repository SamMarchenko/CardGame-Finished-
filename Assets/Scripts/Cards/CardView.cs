using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using DefaultNamespace;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler,
    IDragHandler, IPointerClickHandler, IDamageable
{
    [SerializeField] private IncreaseStatsParameters _increaseStatsParameters;
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
    private CardSignalBus _bus;
    
    
    public List<CardView> MyBuffers = new List<CardView>();
    public List<EAbility> MyAbilities;
    public TextMeshPro NameText => _nameText;
    public int CardId => _cardId;
    public Vector3 StepPosition => _stepPosition;
    public float Scale => _scale;

    public bool IsEnable
    {
        get => _icon.enabled;
        set
        {
            _icon.enabled = value;
            _frontCard.SetActive(value);
        }
    }

    public bool CanBeDragged;
    public bool CanAttack;
    public bool IsScaled;
    public bool IsTaunt;
    public int HealthIncreaseBuff = 0;
    public int BaseHp;
    public int DamageIncreaseBuff = 0;
    public int BaseDMG;
    public ECardStateType StateType { get; set; } = ECardStateType.InDeck;
    public Player Owner;
    public IncreaseStatsParameters IncreaseStatsParameters => _increaseStatsParameters;
    public ECardUnitType MyType;


    public void Configuration(CardPropertiesData data, string description, Material icon)
    {
        _icon.sharedMaterial = icon;
        _cosText.text = data.Cost.ToString();
        _nameText.text = data.Name;
        _descriptionText.text = description;
        _typeText.text = data.Type == ECardUnitType.All ? string.Empty : data.Type.ToString();
        _damageText.text = (data.Attack + DamageIncreaseBuff).ToString();
        _healthText.text = (data.Health + HealthIncreaseBuff).ToString();
        _cardId = (int) data.Id;
        MyType = data.Type;
        BaseHp = data.Health;
        BaseDMG = data.Attack;
    }


    public void SetIncreaseStatsParameters(IncreaseStatsParameters increaseStatsParameters)
    {
        if (increaseStatsParameters == null)
        {
            _increaseStatsParameters = new IncreaseStatsParameters();
            return;
        }

        _increaseStatsParameters = increaseStatsParameters;

        if (!MyAbilities.Contains(EAbility.IncreaseStats))
        {
            MyAbilities.Add(EAbility.IncreaseStats);
        }
    }

    public int GetCost()
    {
        return int.Parse(_cosText.text);
    }

    public int GetActualHealth()
    {
        return int.Parse(_healthText.text);
    }

    public int GetBaseHealth()
    {
        return BaseHp;
    }

    public void SetHealth(int health)
    {
        _healthText.text = health.ToString();
        if (HealthIncreaseBuff > 0)
        {
            _healthText.color = Color.green;
        }
        if (health == 0)
        {
            Owner.KillCardFromTable(this);
        }
    }

    public void Init(CardSignalBus bus)
    {
        _bus = bus;
    }

    [ContextMenu("Switch Visual")]
    public void SwitchVisual()
    {
        IsEnable = !IsEnable;
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

    public Transform GetStartCardPosition()
    {
        return _startPosition;
    }

    public Transform GetEndCardPosition()
    {
        return _endPosition;
    }

    public void SetStartCardPosition(Transform startPosition)
    {
        _startPosition = startPosition;
    }

    public void SetEndCardPosition(Transform endPosition)
    {
        _endPosition = endPosition;
    }

    public void MoveAnimation(Transform endPosition)
    {
        StartCoroutine(MoveCardRoutine(endPosition));
    }

    public void DestroySelf()
    {
        Debug.Log($"Карта {_nameText.text} уничтожена!");
        //todo: включать корутиной анимацию уничтожения. При завершении анимации дестроить.
        Destroy(gameObject);
    }

    private IEnumerator MoveCardRoutine(Transform parent)
    {
        var time = 0f;
        var startPos = transform.position;
        var endPos = parent.position;
        NormalizedScale();
        transform.eulerAngles = new Vector3(0, 0, 180);

        while (time < 1f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
        }

        //todo: тут передавался parent. Не знаю почему, заменил на position.
        transform.position = parent.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _bus.FirePointerOn(new CardPointerSignal(this));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _bus.FirePointerOff(new CardPointerSignal(this));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _bus.FireDragStart(new CardDragSignal(this));
//        Debug.Log("OnBeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _bus.FireDragEnd(new CardDragSignal(this));
        // Debug.Log("OnEndDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (StateType != ECardStateType.InHand)
        {
            return;
        }

        _bus.FireDragging(new CardDragSignal(this));
//        Debug.Log("OnDrag");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _bus.CardClickFire(new CardClickSignal(this));
    }

    public int GetActualDamage()
    {
        return int.Parse(_damageText.text);
    }

    public int GetBaseDamage()
    {
        return BaseDMG;
    }
    

    public void SetDamage(int value)
    {
        _damageText.text = value.ToString();
        if (DamageIncreaseBuff > 0)
        {
            _damageText.color = Color.green;
        }
    }

    public void ApplyDamage(int damage)
    {
        var health = GetActualHealth();
        if (health - damage <= 0)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }

        SetHealth(health);
    }

    public void SetCoolDownAttack(bool value)
    {
        CanAttack = !value;
    }
}