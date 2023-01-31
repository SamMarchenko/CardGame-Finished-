using System;
using System.Collections;
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
    [SerializeField] private GameObject _frontCard;
    [Space, SerializeField] private MeshRenderer _icon;
    [SerializeField] private TextMeshPro _cosText;
    [SerializeField] private TextMeshPro _nameText;
    public TextMeshPro NameText => _nameText;
    [SerializeField] private TextMeshPro _descriptionText;
    [SerializeField] private TextMeshPro _typeText;
    [SerializeField] private TextMeshPro _attackText;
    [SerializeField] private TextMeshPro _healthText;
    

    private Vector3 _stepPosition = new Vector3(0, 0.5f, 0);
    public Vector3 StepPosition => _stepPosition;
    private const float _scale = 1.7f;
    public float Scale => _scale;
    private Transform _startPosition;
    private Transform _endPosition;

    public bool IsEnable
    {
        get => _icon.enabled;
        set
        {
            _icon.enabled = value;
            _frontCard.SetActive(value);
        }
    }

    private CardSignalBus _bus;
    public bool CanBeDragged = false;
    public bool CanAttack = false;
    public bool IsScaled;

    public ECardStateType StateType { get; set; } = ECardStateType.InDeck;
    public Player Owner;

    public void Configuration(CardPropertiesData data, string description, Material icon)
    {
        _icon.sharedMaterial = icon;
        _cosText.text = data.Cost.ToString();
        _nameText.text = data.Name;
        _descriptionText.text = description;
        _typeText.text = data.Type == ECardUnitType.All ? string.Empty : data.Type.ToString();
        _attackText.text = data.Attack.ToString();
        _healthText.text = data.Health.ToString();
    }

    public int GetCost()
    {
        return int.Parse(_cosText.text);
    }
    
    public int GetHealth()
    {
        return int.Parse(_healthText.text);
    }

    private void SetHealth(int health)
    {
        _healthText.text = health.ToString();
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
        transform.eulerAngles = new Vector3(0, 0,  180);
    
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

    public int GetDamage()
    {
        return int.Parse(_attackText.text);
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
       
        SetHealth(health);
    }

    public void SetCoolDownAttack(bool value)
    {
        CanAttack = value;
    }
}