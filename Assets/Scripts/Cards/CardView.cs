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
    [SerializeField] private IncreaseStatsParameters buffStatsParameters;
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

    //public Dictionary<CardView, bool> MyBuffers = new Dictionary<CardView, bool>();
    //public List<CardView> MyBuffers = new List<CardView>();
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
    public int SumHpBuff = 0;
    public int Hp;
    public int SumDmgBuff = 0;
    public int DMG;
    public ECardStateType StateType { get; set; } = ECardStateType.InDeck;
    public Player Owner;
    public IncreaseStatsParameters BuffStatsParameters => buffStatsParameters;
    public ECardUnitType MyType;


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
        DMG = data.Attack;
    }


    public void SetIncreaseStatsParameters(IncreaseStatsParameters increaseStatsParameters)
    {
        if (increaseStatsParameters == null)
        {
            buffStatsParameters = new IncreaseStatsParameters();
            return;
        }

        buffStatsParameters = increaseStatsParameters;

        if (!MyAbilities.Contains(EAbility.IncreaseStats))
        {
            MyAbilities.Add(EAbility.IncreaseStats);
        }
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
        SumHpBuff += hpBuff;
        _healthText.text = currentHp.ToString();
        _healthText.color = SumHpBuff > 0 ? Color.green : Color.white;

        if (currentHp == 0)
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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _bus.FireDragEnd(new CardDragSignal(this));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (StateType != ECardStateType.InHand)
        {
            return;
        }

        _bus.FireDragging(new CardDragSignal(this));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _bus.CardClickFire(new CardClickSignal(this));
    }

    public int GetDamage()
    {
        DMG = int.Parse(_damageText.text);
        return DMG;
    }


    public void SetDamage(int currentDmg, int dmgBuff)
    {
        SumDmgBuff += dmgBuff;
        _damageText.text = currentDmg.ToString();
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

        SetHealth(health, 0);
    }

    public void SetCoolDownAttack(bool value)
    {
        CanAttack = !value;
    }
}